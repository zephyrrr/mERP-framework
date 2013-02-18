using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng.Collections
{
	/// <summary>
	/// 控件集合
	/// </summary>
	/// <typeparam name="T">控件类型</typeparam>
    /// <typeparam name="S">外部管理器类型</typeparam>
	public class ControlCollection<T, S> : IList<T>, IEnumerable<T>
	{
		#region "Constructor"
		private List<T> m_items = new List<T>();
		
		private S m_cm;

        /// <summary>
        /// 外部管理器，可为<see cref="IControlManager"/>或者<see cref="IDisplayManager"/>
        /// </summary>
        public S ParentManager
        {
            get { return m_cm; }
            set { m_cm = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ControlCollection()
        {
		}

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="cm">外部管理器</param>
        public ControlCollection(S cm)
		{
			m_cm = cm;
		}

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="controls"></param>
        public void AddRange(IEnumerable<T> controls)
        {
            if (controls == null)
            {
                throw new ArgumentNullException("controls");
            }

            foreach (T control in controls)
            {
                Add(control);
            }
        }
		#endregion

		#region "Interface"
		/// <summary>
		/// 数量
		/// </summary>
		public int Count
		{
			get
			{
				return this.m_items.Count;
			}
		}

		/// <summary>
		/// 是否只读
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return ((IList)this.m_items).IsReadOnly;
			}
		}

        /// <summary>
        /// 查询索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return m_items.IndexOf(item);
        }

        /// <summary>
        /// 获取或设置指定索引处的元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return m_items[index];
            }
            set
            {
                m_items[index] = value;
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            m_items.Insert(index, item);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            m_items.RemoveAt(index);
        }

		/// <summary>
		/// 添加
		/// </summary>
        /// <param name="item"></param>
		public virtual void Add(T item)
		{
			if (item == null)
			{
                throw new ArgumentNullException("item");
			}

			m_items.Add(item);
		}

		/// <summary>
		/// 清空
		/// </summary>
		public virtual void Clear()
		{
			m_items.Clear();
		}

		/// <summary>
		/// 是否包含
		/// </summary>
        /// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(T item)
		{
			return this.m_items.Contains(item);
		}

		/// <summary>
		/// 拷贝
		/// </summary>
		/// <param name="array"></param>
        /// <param name="arrayIndex"></param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.m_items.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// 移除
		/// </summary>
        /// <param name="item"></param>
		public bool Remove(T item)
		{
			int idx = this.m_items.IndexOf(item);
			if (idx == -1)
			{
				return false;
			}
			else
			{
				m_items.RemoveAt(idx);
				return true;
			}
		}

		/// <summary>
        /// 取得枚举
		/// </summary>
		/// <returns></returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return m_items.GetEnumerator();
		}

		/// <summary>
        /// 取得枚举
		/// </summary>
		/// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
		{
			return m_items.GetEnumerator();
		}

		/// <summary>
		/// 移除集合
		/// </summary>
		/// <param name="controls"></param>
		public void RemoveRange(IEnumerable<T> controls)
		{
			if (controls == null)
			{
				throw new ArgumentNullException("controls");
			}

			foreach (T control in controls)
			{
				Remove(control);
			}
		}
		#endregion
	}
}
