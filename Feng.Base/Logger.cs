using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(string message, Exception exception)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Fatal(message, exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(string message, Exception exception)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Error(message, exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(string message, Exception exception)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Warn(message, exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(string message, Exception exception)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Info(message, exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(string message, Exception exception)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Debug(message, exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(string message)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Fatal(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Warn(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            ILogger log = ServiceProvider.GetService<ILogger>();
            if (log != null)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsFatalEnabled
        {
            get
            {
                ILogger log = ServiceProvider.GetService<ILogger>();
                return log == null ? false : log.IsFatalEnabled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsErrorEnabled
        {
            get
            {
                ILogger log = ServiceProvider.GetService<ILogger>();
                return log == null ? false : log.IsErrorEnabled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsWarnEnabled
        {
            get
            {
                ILogger log = ServiceProvider.GetService<ILogger>();
                return log == null ? false : log.IsWarnEnabled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsInfoEnabled
        {
            get
            {
                ILogger log = ServiceProvider.GetService<ILogger>();
                return log == null ? false : log.IsInfoEnabled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsDebugEnabled
        {
            get
            {
                ILogger log = ServiceProvider.GetService<ILogger>();
                return log == null ? false : log.IsDebugEnabled;
            }
        }
    }
}
