using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionableList<T> : IList
    {
        private Action<T> action;
        private IList m_originalList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public ActionableList(IList list, Action<T> action)
        {
            m_originalList = list;
            this.action = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(object value)
        {
            action((T)value);

            return m_originalList.Add(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(object value)
        {
            return m_originalList.Contains(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return m_originalList.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized { get { return m_originalList.IsSynchronized; } }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot { get { return m_originalList.SyncRoot; } }

        /// <summary>
        /// 
        /// </summary>
        public int Count 
        {
            get { return m_originalList.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            m_originalList.CopyTo(array, index);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize 
        {
            get
            {
                return m_originalList.IsFixedSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly 
        {
            get
            {
                return m_originalList.IsReadOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index] 
        {
            get
            {
                return m_originalList[index];
            }
            set
            {
                m_originalList[index] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_originalList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(object value)
        {
            return m_originalList.IndexOf(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, object value)
        {
            m_originalList.Insert(index, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Remove(object value)
        {
            m_originalList.Remove(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            m_originalList.RemoveAt(index);
        }
    }
}
