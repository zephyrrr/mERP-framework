using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace Feng.Windows.Collections
{
    /// <summary>
    /// 绑定控件集合
    /// </summary>
    public class BindingControlCollectionBindingSource : Feng.Collections.BindingControlCollection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BindingControlCollectionBindingSource()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bc">数据源</param>
        public BindingControlCollectionBindingSource(BindingSource bc)
            : base()
        {
            m_bindingSource = bc;
        }

        private BindingSource m_bindingSource;

        internal BindingSource BindingSource
        {
            get { return m_bindingSource; }
            set { m_bindingSource = value; }
        }


        /// <summary>
        /// 添加。
        /// 如BindingSource不为空，则设置数据源
        /// </summary>
        /// <param name="item"></param>
        public override void Add(IBindingControl item)
        {
            if (m_bindingSource != null)
            {
                item.SetDataBinding(m_bindingSource, "");
            }

            base.Add(item);
        }
    }
}