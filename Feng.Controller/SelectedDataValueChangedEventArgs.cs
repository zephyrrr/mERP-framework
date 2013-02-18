using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectedDataValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataControlName"></param>
        /// <param name="container"></param>
        public SelectedDataValueChangedEventArgs(string dataControlName, object container)
		{
            m_dataControlName = dataControlName;
            m_container = container;
		}

        private string m_dataControlName;
        private object m_container;

		/// <summary>
        /// DataControlName
		/// </summary>
		public string DataControlName 
		{
			get { return m_dataControlName; } 
		}

        /// <summary>
        /// Cell or DataControl
        /// </summary>
        public object Container
        {
            get { return m_container; }
        }
    }
}
