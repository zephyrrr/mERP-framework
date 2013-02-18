using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Feng.Collections;

namespace Feng
{
	/// <summary>
	/// 显示控制器
	/// </summary>
	public abstract class AbstractDisplayManager : IDisplayManager
	{
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_sm != null)
                {
                    m_sm.Dispose();
                }
                this.m_bindingControls.Clear();
                this.m_dataControls.Clear();

                this.SelectedDataValueChanged = null;
                this.PositionChanged = null;
                this.PositionChanging = null;
            }
        }

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="sm">查找管理器</param>
        protected AbstractDisplayManager(ISearchManager sm)
		{
            if (sm != null)
            {
                m_sm = sm;
                m_sm.DisplayManager = this;
            }

            var ccf = ServiceProvider.GetService<IControlCollectionFactory>();
            if (ccf != null)
            {
                m_bindingControls = ccf.CreateBindingControlCollection(this);
                m_dataControls = ccf.CreateDataControlCollection(this);
            }
            else
            {
                m_bindingControls = new BindingControlCollection();
                m_dataControls = new DataControlCollection();
            }
		}

        private string m_name = string.Empty;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private IEntityMetadata m_entityInfo;

        /// <summary>
        /// 实体类信息
        /// </summary>
        public virtual IEntityMetadata EntityInfo
        {
            get
            {
                if (m_entityInfo == null)
                {
                    var s = ServiceProvider.GetService<IEntityMetadataGenerator>();
                    if (s != null)
                    {
                        m_entityInfo = s.GenerateEntityMetadata(m_sm);
                    }
                    else
                    {
                        m_entityInfo = EmptyEntityMetadata.Instance;
                    }
                }
                return m_entityInfo;
            }
        }

        private IDataControlCollection m_dataControls;
        /// <summary>
        /// 数据控件集合
        /// </summary>
        public IDataControlCollection DataControls
        {
            get { return m_dataControls; }
        }


        private IBindingControlCollection m_bindingControls;
		/// <summary>
		///  绑定空间集合
		/// </summary>
		public IBindingControlCollection BindingControls
		{
			get { return m_bindingControls; }
		}

        private ISearchManager m_sm;
        /// <summary>
        /// 查询管理器
        /// </summary>
        public ISearchManager SearchManager
        {
            get { return m_sm; }
        }

        /// <summary>
        /// 设置数据绑定。
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">数据成员</param>
        /// <exception cref="System.NotSupportedException">数据源不符合格式时抛出</exception>
        public abstract void SetDataBinding(object dataSource, string dataMember);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataItem"></param>
        public abstract void AddDataItem(object dataItem);

        /// <summary>
        /// 数据源中实体类列表
        /// 注意：和Grid's DataRows中的列表顺序一致(Grid 排序不影响列表顺序)
        /// 因接口为非范型，只能返回object类型。目前支持DataTable和IList(T)
        /// </summary>
        public abstract IList Items
        {
            get;
        }

        //private object m_dataSource;
        ///// <summary>
        ///// 数据源
        ///// </summary>
        //public object DataSource
        //{
        //    get { return m_dataSource; }
        //    protected set { m_dataSource = value; }
        //}

        //private string m_dataMember;
        ///// <summary>
        ///// 数据成员
        ///// </summary>
        //public string DataMember
        //{
        //    get { return m_dataMember; }
        //    protected set { m_dataMember = value; }
        //}

        private int m_position = -1;
        /// <summary>
        /// 当前位置。
        /// 因为操作针对当前实体类，所以操作前要设置当前位置
        /// 位置改变后引发PositionChanged事件
        /// </summary>
        public virtual int Position
        {
            get
            {
                return m_position;
            }
            set
            {
                if (m_position != value)
                {
                    CancelEventArgs e = new CancelEventArgs();
                    OnPositionChanging(e);
                    if (e.Cancel)
                        return;

                    if (value < 0 || value >= this.Count)
                    {
                        m_position = -1;
                    }
                    else
                    {
                        m_position = value;
                    }

                    OnPositionChanged(System.EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 引发PositionChanged<see cref="PositionChanging"/>事件
        /// </summary>
        /// <param name="e"></param>
        protected void OnPositionChanging(CancelEventArgs e)
        {
            if (PositionChanging != null)
            {
                PositionChanging(this, e);
            }
        }

        /// <summary>
        /// 引发PositionChanged<see cref="PositionChanged"/>事件
        /// </summary>
        /// <param name="e"></param>
        public void OnPositionChanged(System.EventArgs e)
        {
            // When in edit, display also
            //if (this.State == StateType.View || this.State == StateType.None)
            {
                DisplayCurrent();
            }

            if (PositionChanged != null)
            {
                PositionChanged(this, e);
            }
        }

        /// <summary>
        /// <see cref="Position"/>改变前发生
        /// </summary>
        public event CancelEventHandler PositionChanging;

        /// <summary>
        /// <see cref="Position"/>改变后发生
        /// </summary>
        public event EventHandler PositionChanged;

        ///// <summary>
        ///// 在显示当前位置数据前发生
        ///// </summary>
        //public event EventHandler CurrentDisplaying;

        /// <summary>
        /// 显示当前位置实体类数据
        /// </summary>
        public virtual void DisplayCurrent()
        {
            foreach (IDataControl dc in this.DataControls)
            {
                // if in tab control, the data control may be invisible
                //if (!dc.Visible)
                //    continue;

                switch (dc.ControlType)
                {
                    case DataControlType.Unbound:
                        break;
                    case DataControlType.Normal:
                        {
                            if (string.IsNullOrEmpty(dc.PropertyName))
                            {
                                continue;
                            }

                            if (this.CurrentItem == null)
                            {
                                dc.SelectedDataValue = null;
                            }
                            else
                            {
                                try
                                {
                                    dc.SelectedDataValue = EntityScript.GetPropertyValue(this.CurrentItem, dc.Navigator, dc.PropertyName);
                                }
                                catch (Exception ex)
                                {
                                    throw new ArgumentException(string.Format("DataControl of {0} is Invalid!", dc.Name), ex);
                                }
                            }
                        }
                        break;
                    case DataControlType.Expression:
                        if (string.IsNullOrEmpty(dc.PropertyName))
                            continue;
                        if (dc.PropertyName.Contains("%"))
                        {
                            dc.SelectedDataValue = EntityScript.CalculateExpression(dc.PropertyName, this.CurrentItem);
                        }
                        else
                        {
                        }
                        break;
                    default:
                        throw new ArgumentException(string.Format("DataControl of {0} is Invalid!", dc.Name));
                }
                
            }

        }

        ///// <summary>
        ///// DataView
        ///// </summary>
        //private DataView DataView
        //{
        //    get 
        //    {
        //        if (this.Items == null)
        //            return null;
        //        DataView view = this.Items as DataView;
        //        if (view == null)
        //        {
        //            throw new ArgumentException("in DisplayManager Items should be DataView!");
        //        }
        //        return view;
        //    }
        //}

        /// <summary>
        /// 当前实体类
        /// </summary>
        public object CurrentItem
        {
            get
            {
                if (Position >= 0 && Position < this.Count)
                {
                    return this.Items[Position];
                }
                else
                {
                    return null;
                }
            }
        }

        ///// <summary>
        ///// 强类型当前实体类
        ///// </summary>
        //private DataRowView CurrentRow
        //{
        //    get
        //    {
        //        if (this.CurrentItem == null)
        //            return null;
        //        DataRowView rowView = this.CurrentItem as DataRowView;
        //        if (rowView == null)
        //        {
        //            throw new ArgumentException("in DisplayManager Item should be DataRowView!");
        //        }
        //        return rowView; 
        //    }
        //}

        ///// <summary>
        ///// 根据序号获得实体类
        ///// </summary>
        ///// <param name="idx"></param>
        //public virtual object GetItem(int idx)
        //{
        //    if (idx < 0 || idx >= this.DataTable.Rows.Count)
        //    {
        //        throw new ArgumentException("Invalid index");
        //    }
        //    return this.DataTable.Rows[idx];
        //}

        /// <summary>
        /// 当前实体类列表数量
        /// </summary>
        public int Count
        {
            get { return this.Items == null ? 0 : this.Items.Count; }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        protected static void Copy(AbstractDisplayManager src, AbstractDisplayManager dest)
        {
            dest.BindingControls.AddRange(src.BindingControls);
            dest.DataControls.AddRange(src.DataControls);
        }


        /// <summary>
        /// SelectedDataValueChanged event
        /// </summary>
        public event EventHandler<SelectedDataValueChangedEventArgs> SelectedDataValueChanged;

        /// <summary>
        /// 引发<see cref="SelectedDataValueChanged"/> 事件
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedDataValueChanged(SelectedDataValueChangedEventArgs e)
        {
            if (this.SelectedDataValueChanged != null)
            {
                this.SelectedDataValueChanged(this, e);
            }
        }

        //private int m_preBatchPos = -1;
        /// <summary>
        /// 开始批量操作
        /// </summary>
        public void BeginBatchOperation()
        {
            //if (m_preBatchPos != -1 || m_cntBatchOperation)
            //{
            //    throw new InvalidOperationException("you should call EndBatchOperation after BeginBatchOperation!");
            //}
            m_cntBatchOperation++;

            //m_preBatchPos = this.Position;
        }

        /// <summary>
        /// 结束批量操作
        /// </summary>
        public void EndBatchOperation()
        {
            if (m_cntBatchOperation > 0)
            {
                m_cntBatchOperation--;
            }
            //if (m_preBatchPos != this.Position)   // 当Position一致的时候，里面的数据也有可能改变
            //{
                //m_preBatchPos = -1;

                // 不再自动处罚PositionChanged，而是程序里手动触发，该干吗干吗
                // OnPositionChanged(System.EventArgs.Empty);  // in PositionChanged, there maybe have BeginBatchOperation
            //}
            //else
            //{
            //    m_preBatchPos = -1;
            //}
        }

        private int m_cntBatchOperation;
        /// <summary>
        /// 是否在批量操作。
        /// 此时，不保存Row（Row_Saving),不响应ListChanged，不响应PositionChanged，Grid.CurrentRowChanged
        /// </summary>
        public bool InBatchOperation
        {
            get { return m_cntBatchOperation > 0; }
        }
	}
}
