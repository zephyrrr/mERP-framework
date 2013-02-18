using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng.Collections
{
	/// <summary>
	/// 查找控件集合
	/// </summary>
    public class SearchControlCollection : ControlCollection<ISearchControl, ISearchManager>, ISearchControlCollection
	{
        private static SearchControlCollection s_scc = new SearchControlCollection();
        /// <summary>
        /// Empty SearchControlCollection(static)
        /// </summary>
        public static SearchControlCollection Empty
        {
            get { return s_scc; }
        }

		/// <summary>
		/// Constructor
		/// </summary>
		public SearchControlCollection()
            : base()
		{
		}

        /// <summary>
        /// Clear
        /// </summary>
        public override void Clear()
        {
            m_dict.Clear();
            base.Clear();
        }

        private Dictionary<string, ISearchControl> m_dict = new Dictionary<string, ISearchControl>();
        private void Init()
        {
            m_dict.Clear();
            foreach (ISearchControl dc in this)
            {
                m_dict[dc.Name] = dc;
            }
        }

        /// <summary>
        /// 获取或设置指定名称的元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ISearchControl this[string name]
        {
            get
            {
                if (m_dict.ContainsKey(name))
                    return m_dict[name];
                else
                {
                    Init();
                    if (m_dict.ContainsKey(name))
                        return m_dict[name];
                    else
                        //throw new ArgumentException("Invalid SearchControl with name of " + name);
                        return null;
                }
            }
        }
	}
}
