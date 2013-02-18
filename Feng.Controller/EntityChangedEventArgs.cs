using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
	/// <summary>
	/// 实体类改变类型
	/// </summary>
	public enum EntityChangedType
	{
		/// <summary>
		/// 新加
		/// </summary>
		Add,
		/// <summary>
		/// 编辑
		/// </summary>
		Edit,
		/// <summary>
		/// 删除
		/// </summary>
		Delete
	}

	/// <summary>
	/// 实体类改变事件参数
	/// </summary>
	public class EntityChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Consturctor
		/// </summary>
		/// <param name="type"></param>
		/// <param name="entity"></param>
		public EntityChangedEventArgs(EntityChangedType type, object entity)
		{
			m_type = type;
			m_entity = entity;
		}

		private EntityChangedType m_type;
        private object m_entity;
        private Exception m_failException;

		/// <summary>
		/// 改变类型
		/// </summary>
		public EntityChangedType EntityChangedType 
		{
			get { return m_type; } 
		}

		/// <summary>
		/// 改变的实体类
		/// </summary>
		public object Entity
		{
			get { return m_entity; }
		}

		/// <summary>
		/// 是否成功处理（外部返回）
        /// 如果是Null则成功
		/// </summary>
		public Exception Exception
		{
            get { return m_failException; }
            set { m_failException = value; }
		}
	}
}
