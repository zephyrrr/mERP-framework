using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisplayManagerBindingSource<T> : DisplayManager<T>, IDisplayManagerBindingSource
        where T : class, IEntity
    {
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_bs.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sm"></param>
        public DisplayManagerBindingSource(ISearchManager sm)
            : base(sm)
        {
        }

        #region "BindingSource And Binding Controls"

        private BindingSource m_bs = new BindingSource();

        /// <summary>
        /// BindingSource
        /// </summary>
        public BindingSource BindingSource
        {
            get { return m_bs; }
        }

        /// <summary>
        /// 设置数据绑定。
        /// 目前dataSource必须为IList&lt;T&gt;类型, dataMamber无用"/>
        /// 设置完数据源后，State为<see cref="StateType.View"/>
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">数据成员</param>
        /// <exception cref="NotSupportedException">数据源不符合格式时抛出</exception>
        public override void SetDataBinding(object dataSource, string dataMember)
        {
            if (dataSource != null)
            {
                List<T> list = dataSource as List<T>;

                Debug.Assert(list != null, "dataSource in DisplayManager<T> must be List<T>.");

                m_entities = list;
            }

            m_bs.PositionChanged -= new System.EventHandler(m_bs_PositionChanged);
            m_bs.DataSource = dataSource;
            m_bs.DataMember = string.Empty;
            m_bs.PositionChanged += new EventHandler(m_bs_PositionChanged);

            m_bs_PositionChanged(m_bs, System.EventArgs.Empty);
        }

        private List<T> m_entities;
        /// <summary>
        /// Items
        /// </summary>
        public override IList Items
        {
            get { return m_entities; }
        }

        private void m_bs_PositionChanged(object sender, EventArgs e)
        {
            CancelEventArgs e2 = new CancelEventArgs();
            OnPositionChanging(e2);
            if (e2.Cancel)
            {
                return;
            }

            OnPositionChanged(e);
        }

        /// <summary>
        /// 当前位置。
        /// 因为操作针对当前实体类，所以操作前要设置当前位置
        /// 位置改变后引发PositionChanged事件
        /// </summary>
        public override int Position
        {
            get { return m_bs.Position; }
            set
            {
                if (m_bs.Position != value)
                {
                    CancelEventArgs e = new CancelEventArgs();
                    OnPositionChanging(e);
                    if (e.Cancel)
                    {
                        return;
                    }

                    if (value < 0 || value >= m_bs.Count)
                    {
                        m_bs.Position = -1;
                    }
                    else
                    {
                        m_bs.Position = value;
                    }

                    OnPositionChanged(System.EventArgs.Empty);
                }
            }
        }

        #endregion

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DisplayManagerBindingSource<T> dm = new DisplayManagerBindingSource<T>(this.SearchManager.Clone() as ISearchManager);
            Copy(this, dm);
            return dm;
        }
    }
}