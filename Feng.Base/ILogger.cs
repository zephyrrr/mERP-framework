using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Fatal(string message, Exception exception);

       /// <summary>
       /// 
       /// </summary>
       /// <param name="message"></param>
       /// <param name="exception"></param>
        void Error(string message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Warn(string message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Info(string message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Fatal(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsFatalEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsErrorEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsWarnEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsInfoEnabled
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsDebugEnabled
        {
            get;
        }
    }
}
