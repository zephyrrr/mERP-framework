using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
//using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using log4net;

namespace Feng.Windows
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class log4netLogger : ILogger
    {
        public log4netLogger()
        {
            m_log = LogManager.GetLogger("Feng.Windows.log4net");
            //m_log.IsDebugEnabled = true;
            //m_log.IsErrorEnabled = true;
            //m_log.IsFatalEnabled = true;
            //m_log.IsInfoEnabled = true;
            //m_log.IsWarnEnabled = true;
        }

        /// <summary>
        /// Flush all
        /// </summary>
        public static void FlushAllAppenders()
        {
            foreach (log4net.Appender.IAppender appender in log4net.LogManager.GetRepository().GetAppenders())
            {
                log4net.Appender.BufferingAppenderSkeleton buffer = appender as log4net.Appender.BufferingAppenderSkeleton;
                if (buffer != null)
                {
                    buffer.Flush();
                }
            }
        }

        /*  void Debug(object message);
  void Info(object message);
  void Warn(object message);
  void Error(object message);
  void Fatal(object message);*/

        private ILog m_log;

        //private const int s_defaultEventId = 888;
        //private static void DoLog(string category, TraceEventType eventType, int priority, string message)
        //{
            //LogEntry logEntry = new LogEntry();
            //logEntry.EventId = s_defaultEventId;
            //logEntry.Priority = priority;
            //logEntry.Message = message;
            //logEntry.Severity = eventType;
            //logEntry.Categories.Add(category);

            //// ComPlusInformationProvider, ManagedSecurityContextInformationProvider, UnmanagedSecurityContextInformationProvider 
            //Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //DebugInformationProvider informationHelper = new DebugInformationProvider();
            //informationHelper.PopulateDictionary(dictionary);
            //logEntry.ExtendedProperties = dictionary;

            //Logger.Write(logEntry);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFatalEnabled
        {
            get { return m_log.IsFatalEnabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsErrorEnabled
        {
            get { return m_log.IsErrorEnabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsWarnEnabled
        {
            get { return m_log.IsWarnEnabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsInfoEnabled
        {
            get { return m_log.IsInfoEnabled; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsDebugEnabled
        {
            get { return m_log.IsDebugEnabled; }
        }

        /// <summary>
        /// 严重错误日志，Fatal
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message)
        {
            m_log.Fatal(message);

            //DoLog("Fatal", TraceEventType.Critical, 10, message);
        }

        /// <summary>
        /// 一般错误日志，Error
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            m_log.Error(message);
            //DoLog("Error", TraceEventType.Error, 9, message);
        }

        /// <summary>
        /// 警告日志，Warn
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            m_log.Warn(message);
            //DoLog("Warn", TraceEventType.Warning, 8, message);
        }

        /// <summary>
        /// 一般消息日志，Info
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            m_log.Info(message);
            //DoLog("Info", TraceEventType.Information, 7, message);
        }

        /// <summary>
        /// 调试消息日志，Debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            m_log.Debug(message);
            //DoLog("Debug", TraceEventType.Verbose, 6, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(string message, Exception exception)
        {
            m_log.Fatal(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(string message, Exception exception)
        {
            m_log.Error(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(string message, Exception exception)
        {
            m_log.Warn(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(string message, Exception exception)
        {
            m_log.Info(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(string message, Exception exception)
        {
            m_log.Debug(message, exception);
        }

        ///// <summary>
        ///// LogUIEvent
        ///// </summary>
        ///// <param name="message"></param>
        //public static void LogUIEvent(string message)
        //{
        //    DoLog("UI Events", TraceEventType.Information, 15, message);
        //}

        ///// <summary>
        ///// LogError
        ///// </summary>
        ///// <param name="message"></param>
        //public static void LogBIEvent(string message)
        //{
        //    DoLog("BI Events", TraceEventType.Information, 18, message);
        //}

        ///// <summary>
        ///// Log in Database
        ///// </summary>
        ///// <param name="message"></param>
        //public static void LogInDatabase(string message)
        //{
        //    DoLog("Database", TraceEventType.Error, 30, message);
        //}
    }
}