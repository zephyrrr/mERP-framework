using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
	/// <summary>
	/// 数值和描述对应关系(采用Enum而不是数据库数据)
	/// 例如：箱形 -> （大箱，1；小箱，2）
	/// </summary>
	public class EnumNameValue
	{
		private object m_value;
        private int m_index;
        private string m_description;
        private Type m_valueType;

        /// <summary>
        /// 
        /// </summary>
        public Type ValueType
        {
            get { return m_valueType; }
        }

		/// <summary>
		/// 序号
		/// </summary>
		public int Index
		{
			get { return m_index; }
		}

		/// <summary>
		/// 实际值
		/// </summary>
		public object Value
		{
			get { return m_value; }
		}

        /// <summary>
        /// 描述（详细名称）
        /// </summary>
        public string Description
        {
            get { return m_description; }
        }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value"></param>
		/// <param name="index"></param>
        /// <param name="description"></param>
        /// <param name="valueType"></param>
        public EnumNameValue(object value, int index, string description, Type valueType)
		{
			m_value = value;
			m_index = index;
            if (string.IsNullOrEmpty(description))
            {
                m_description = value.ToString();
            }
            else
            {
                m_description = description;
            }
            m_valueType = valueType;
		}

		/// <summary>
		/// IndexName = "代码"
		/// </summary>
        public const string IndexName = "代码";

		/// <summary>
        /// ValueName = "名称";
		/// </summary>
        public const string ValueName = "名称";

        /// <summary>
        /// 详细名称
        /// </summary>
        public const string DescriptionName = "描述";

        /// <summary>
        /// Enum类型
        /// </summary>
        public const string ValueTypeName = "类型";
	}
}
