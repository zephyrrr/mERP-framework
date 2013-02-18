using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class DisplayManagerBindingSource : DisplayManager, IDisplayManagerBindingSource
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
            if (dataSource == null)
            {
                this.Position = -1;
            }
            else
            {
                if (dataSource is DataView)
                {
                    m_dv = dataSource as DataView;
                }
                else if (dataSource is DataTable)
                {
                    m_dv = (dataSource as DataTable).DefaultView;
                }
                else if (dataSource is DataSet)
                {
                    m_dv = (dataSource as DataSet).Tables[dataMember].DefaultView;
                }
                else
                {
                    throw new NotSupportedException("dataSource in DisplayManager must be DataTable, DataView or DataSet!");
                }

                if (m_dv.Count == 0)
                {
                    this.Position = -1;
                }
                else
                {
                    this.Position = 0;
                }
            }

            m_bs.PositionChanged -= new System.EventHandler(m_bs_PositionChanged);
            m_bs.DataSource = dataSource;
            m_bs.DataMember = dataMember;
            m_bs.PositionChanged += new EventHandler(m_bs_PositionChanged);

            m_bs_PositionChanged(m_bs, System.EventArgs.Empty);
        }

        private System.Data.DataView m_dv;
        /// <summary>
        /// Items
        /// </summary>
        public override IList Items
        {
            get { return m_dv; }
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
            DisplayManagerBindingSource dm = new DisplayManagerBindingSource(this.SearchManager.Clone() as ISearchManager);
            Copy(this, dm);
            return dm;
        }
    }
}