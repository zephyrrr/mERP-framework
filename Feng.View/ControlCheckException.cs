using System;

namespace Feng
{
	/// <summary>
	/// 控件填值错误（包括不可为空）的Exception
    /// </summary>
    public class ControlCheckException : Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ControlCheckException()
			:base()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">message in exception</param>
		public ControlCheckException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">message in exception</param>
		/// <param name="ex">inner exception</param>
		public ControlCheckException(string message, Exception ex)
			:base(message, ex)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">exception message</param>
		/// <param name="cc">IDatacontrol caused exception</param>
        public ControlCheckException(string message, object cc)
            :base(message)
		{
			m_invalidControl = cc;
		}

        private object m_invalidControl;
		/// <summary>
		/// 出现填值错误的数据控件
		/// </summary>
        public object InvalidDataControl
		{
			get
			{
				return m_invalidControl;
			}
		}
	}
}
