using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using log4net;
using log4net.Core;

namespace Feng.Windows
{
    /// <summary>
    /// http://www.yapbee.com/post/2009/03/09/Integrating-log4net-with-Enterprise-Library-Exception-Handling-Application-Block.aspx
    /// </summary>
    [Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementType(typeof(Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.CustomHandlerData))]
    public class log4netExceptionHandler : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationData"></param>
        public log4netExceptionHandler(NameValueCollection configurationData)
        {
            string categories = configurationData["logCategories"];

            if (string.IsNullOrEmpty(categories))
            {
                m_categories = new string[] { "General" };
            }
            else
            {
                m_categories = categories.Split(',');
            }

            /*+		["NOTICE"]	{NOTICE}	
+		["WARN"]	{WARN}	
+		["SEVERE"]	{SEVERE}	
+		["ALL"]	{ALL}	
+		["ALERT"]	{ALERT}	
+		["INFO"]	{INFO}	
+		["FINE"]	{FINE}	
+		["VERBOSE"]	{VERBOSE}	
+		["EMERGENCY"]	{EMERGENCY}	
+		["DEBUG"]	{DEBUG}	
+		["FATAL"]	{FATAL}	
+		["TRACE"]	{TRACE}	
+		["CRITICAL"]	{CRITICAL}	
+		["FINEST"]	{FINEST}	
+		["ERROR"]	{ERROR}	
+		["FINER"]	{FINER}	
+		["OFF"]	{OFF}	*/

            string levelString = configurationData["level"];
            m_level = log.Logger.Repository.LevelMap[levelString];

            m_formatterType = Feng.Utils.ReflectionHelper.GetTypeFromName(configurationData["formatterType"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logCategories"></param>
        /// <param name="level"></param>
        /// <param name="formatterType"></param>
        public log4netExceptionHandler(string logCategories, string level, Type formatterType)
        {
            if (string.IsNullOrEmpty(logCategories))
            {
                m_categories = new string[] { "General" };
            }
            else
            {
                m_categories = logCategories.Split(',');
            }

            m_level = log.Logger.Repository.LevelMap[level];

            this.m_formatterType = formatterType;
        }
        private readonly Type m_formatterType;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="handlingInstanceId"></param>
        /// <returns></returns>
        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            if (log.Logger.IsEnabledFor(m_level))
            {
                string msg = CreateMessage(exception, handlingInstanceId);// exception.Message;
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, log.Logger.Repository, log.Logger.Name, m_level,  msg, exception);
                loggingEvent.Properties["Categories"] = m_categories;
                log.Logger.Log(loggingEvent);
            }
            return exception;
        }
        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            lock (this)
            {
                StringWriter writer = null;
                StringBuilder stringBuilder = null;
                try
                {
                    writer = this.CreateStringWriter();
                    this.CreateFormatter(writer, exception, handlingInstanceID).Format();
                    stringBuilder = writer.GetStringBuilder();
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Flush();
                    }
                }
                return stringBuilder.ToString();
            }
        }
        private static StringWriter s_stringWriter;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual StringWriter CreateStringWriter()
        {
            if (s_stringWriter == null)
            {
                s_stringWriter = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                s_stringWriter.GetStringBuilder().Remove(0, s_stringWriter.GetStringBuilder().Length);
            }
            return s_stringWriter;
        }

        private static Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter s_exceptionFormat;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="exception"></param>
        /// <param name="handlingInstanceID"></param>
        /// <returns></returns>
        protected virtual Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter CreateFormatter(StringWriter writer, Exception exception, Guid handlingInstanceID)
        {
            if (s_exceptionFormat == null)
            {
                s_exceptionFormat = (Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionFormatter)this.GetFormatterConstructor().Invoke(new object[] { writer, exception, handlingInstanceID });
            }
            return s_exceptionFormat;
        }

        private System.Reflection.ConstructorInfo GetFormatterConstructor()
        {
            Type[] types = new Type[] { typeof(TextWriter), typeof(Exception), typeof(Guid) };
            System.Reflection.ConstructorInfo constructor = this.m_formatterType.GetConstructor(types);
            if (constructor == null)
            {
                throw new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionHandlingException("can't create log formatter!");
            }
            return constructor;
        }

        private string[] m_categories;
        private Level m_level;
        private readonly static Type ThisDeclaringType = typeof(log4netExceptionHandler);
    }
}


