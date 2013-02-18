using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Net
{
    /// <summary>
    /// 网页格式发生改变时抛出的Exception
    /// </summary>
	public class WebFormatChangedException : Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public WebFormatChangedException()
			:base()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">message in exception</param>
		public WebFormatChangedException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">message in exception</param>
		/// <param name="e">inner exception</param>
		public WebFormatChangedException(string message, Exception e)
			:base(message, e)
		{
		}
	}
}
