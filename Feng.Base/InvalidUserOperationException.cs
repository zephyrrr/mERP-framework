using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 用户操作非法的异常
    /// </summary>
    public class InvalidUserOperationException : InvalidOperationException
    {
        /// <summary>
		/// Constructor
		/// </summary>
		public InvalidUserOperationException()
			:base()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">message in exception</param>
		public InvalidUserOperationException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">message in exception</param>
		/// <param name="ex">inner exception</param>
        public InvalidUserOperationException(string message, Exception ex)
			:base(message, ex)
		{
		}
    }
}
