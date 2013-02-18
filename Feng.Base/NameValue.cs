using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 数值名称对应
    /// </summary>
    public class NameValue
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public NameValue(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        private object m_value;

        /// <summary>
        /// 数值
        /// </summary>
        public object Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        private string m_name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
    }
}