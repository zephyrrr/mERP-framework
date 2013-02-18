using System;
using System.Collections.Generic;
using System.ComponentModel;
using Feng.Collections;

namespace Feng
{
    /// <summary>
    /// 控制管理器。可对多个实体类操作，进行增加、修改、删除等操作。
    /// </summary>
    public abstract class AbstractControlManager : IControlManager
    {
        #region "Constructor"
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
                if (this.DisplayManager != null)
                {
                    this.DisplayManager.Dispose();
                }
                if (m_controlCheckExceptionProcess != null)
                {
                    m_controlCheckExceptionProcess.Dispose();
                }
                m_stateControls.Clear();
                m_checkControls.Clear();

                this.ListChanged = null;

                this.DisplayManager.SearchManager.DataLoading -= new EventHandler<DataLoadingEventArgs>(SearchManager_DataLoading);
                m_displayManager.SearchManager.DataLoaded -= new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);

                this.BeginningEdit = null;
                this.EditBegun = null;
                this.EndingEdit = null;
                this.EditEnded = null;
                this.EditCanceled = null;
                this.CancellingEdit = null;

                this.Dao = null;
            }
        }

        private string m_name = string.Empty;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get
            {
                return m_name; 
            }
            set
            {
                m_name = value;
                if (m_displayManager != null && string.IsNullOrEmpty(m_displayManager.Name))
                {
                    m_displayManager.Name = value;
                }
            }
        }

        private readonly IControlCheckExceptionProcess m_controlCheckExceptionProcess;
        private readonly IDisplayManager m_displayManager;
        private readonly IStateControlCollection m_stateControls;
        private readonly ICheckControlCollection m_checkControls;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="displayManager"></param>
        protected AbstractControlManager(IDisplayManager displayManager)
        {
            m_displayManager = displayManager;

            var ccf = ServiceProvider.GetService<IControlCollectionFactory>();
            if (ccf != null)
            {
                m_controlCheckExceptionProcess = ccf.CreateControlCheckExceptionProcess(this);
                m_stateControls = ccf.CreateStateControlCollection(this);
                m_checkControls = ccf.CreateCheckControlCollection(this);
            }
            else
            {
                m_controlCheckExceptionProcess = null;
                m_stateControls = new StateControlCollection();
                m_checkControls = new CheckControlCollection();
            }

            //this.StateControls.Add(m_displayManager.BindingControls);

            if (m_displayManager != null && m_displayManager.SearchManager != null)
            {
                m_displayManager.SearchManager.DataLoading += new EventHandler<DataLoadingEventArgs>(SearchManager_DataLoading);
                m_displayManager.SearchManager.DataLoaded += new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);
            }
        }

        void SearchManager_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            this.State = StateType.View;
            if (m_controlCheckExceptionProcess != null)
            {
                m_controlCheckExceptionProcess.ClearAllError();
            }
        }

        private void SearchManager_DataLoading(object sender, DataLoadingEventArgs e)
        {
            if (!TryCancelEdit())
            {
                e.Cancel = true;
            }
            else
            {
                this.m_inOperating = false;
                this.State = StateType.View;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDisplayManager DisplayManager
        {
            get { return this.m_displayManager; }
        }

        /// <summary>
        /// 处理<see cref="ControlCheckException" />的接口
        /// </summary>
        public IControlCheckExceptionProcess ControlCheckExceptionProcess
        {
            get { return this.m_controlCheckExceptionProcess; }
        }

        /// <summary>
        /// 状态控件集合
        /// </summary>
        public IStateControlCollection StateControls
        {
            get { return this.m_stateControls; }
        }

        /// <summary>
        /// 检查控件集合
        /// </summary>
        public ICheckControlCollection CheckControls
        {
            get { return this.m_checkControls; }
        }

        /// <summary>
        /// 是否能改变位置
        /// </summary>
        /// <returns></returns>
        public abstract bool TryCancelEdit();


        #endregion

        #region "SetState"

        private StateType m_state = StateType.None;

        /// <summary>
        /// 当前状态
        /// </summary>
        public StateType State
        {
            get 
            {
                if (m_state == StateType.View)
                {
                    if (m_displayManager == null || m_displayManager.Count == 0)
                        SetState(StateType.None);
                }
                else if (m_state == StateType.None)
                {
                    if (m_displayManager != null && m_displayManager.Count > 0)
                        SetState(StateType.View);
                }
                return m_state;
            }

            set { this.SetState(value); }
        }

        /// <summary>
        /// StateChanged
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public void SetState(StateType state)
        {
            if (state == StateType.None)
            {
                //throw new ArgumentException("you should not set StateType.None, use StateType.View instead!");
                state = StateType.View;
            }

            if (state == StateType.View && (this.DisplayManager.Items == null || this.DisplayManager.Count == 0))
            {
                state = StateType.None;
            }

            //// 原来：可能state不变但Editable属性要变
            if (m_state == state)
                return;

            this.m_state = state;

            if (StateChanged != null)
            {
                StateChanged(this, System.EventArgs.Empty);
            }

            this.UpdateControlsState();
        }

        #endregion

        #region "Current Position"

        ///// <summary>
        ///// 根据序号更新实体类
        ///// </summary>
        ///// <param name="idx">序号</param>
        ///// <param name="item">要更新实体类</param>
        //public virtual void SetItem(int idx, object item)
        //{
        //    throw new NotSupportedException("DataTable can't set DataRow");
        //}

        ///// <summary>
        ///// 移除某行
        ///// </summary>
        ///// <param name="idx"></param>
        //public virtual void RemoveItem(int idx)
        //{
        //    throw new NotSupportedException("DataTable can't delete DataRow");
        //}

        /// <summary>
        /// 数据是否改变
        /// </summary>
        public virtual bool Changed
        {
            get
            {
                if (this.DisplayManager.CurrentItem == null)
                {
                    throw new InvalidOperationException("DisplayManager's CurrentItem is null");
                }

                // 当有明细的时候，不能判断是否已经修改，所以无论何时都算作已经修改过
                return true;
                
                //foreach (IDataControl dc in this.DisplayManager.DataControls)
                //{
                //    // if in tab control, the data control may be invisible
                //    //if (!dc.Visible)
                //    //    continue;

                //    if (dc.ReadOnly)
                //    {
                //        continue;
                //    }
                //    if (string.IsNullOrEmpty(dc.PropertyName))
                //    {
                //        continue;
                //    }
                //    //if (this.State == StateType.Add)
                //    //{
                //    //    if (!dc.Insertable)
                //    //        continue;
                //    //}
                //    //else if (this.State == StateType.Edit)
                //    //{
                //    //    if (!dc.Editable)
                //    //        continue;
                //    //}
                //    //else
                //    //{
                //    //    throw new NotSupportedException("Invalid State of " + this.State);
                //    //}

                //    object c1 = EntityHelper.GetPropertyValue(this.DisplayManager.CurrentItem, dc.Navigator,
                //                                                 dc.PropertyName);
                //    object c2 = dc.SelectedDataValue;

                //    //if (c1 != null && c2 != null)
                //    //{
                //    //    System.Windows.Forms.MessageBox.Show(c1.ToString() + "," + c2.ToString());
                //    //}
                //    //else if (c1 == null && c2 != null)
                //    //{
                //    //    System.Windows.Forms.MessageBox.Show("null" + "," + c2.ToString());
                //    //}
                //    //else if (c1 != null && c2 == null)
                //    //{
                //    //    System.Windows.Forms.MessageBox.Show(c1.ToString() + "," + "null");
                //    //}
                //    //else if (c1 == null && c2 == null)
                //    //{
                //    //    System.Windows.Forms.MessageBox.Show("null");
                //    //}

                //    if (!Feng.Utils.ReflectionHelper.ObjectEquals(c1, c2))
                //    {
                //        return true;
                //    }
                //}
                //return false;
            }
        }


        ///// <summary>
        ///// 在保存当前位置数据前发生
        ///// </summary>
        //public event EventHandler SavingCurrent;

        ///// <summary>
        ///// 在保存当前位置数据后发生
        ///// </summary>
        //public event EventHandler CurrentSaved;

        /// <summary>
        /// 保存当前数据
        /// </summary>
        public virtual bool SaveCurrent()
        {
            //if (this.SavingCurrent != null)
            //{
            //    this.SavingCurrent(this, System.EventArgs.Empty);
            //}

            if (!this.CheckControlsValue())
            {
                return false;
            }

            if (this.DisplayManager.CurrentItem == null)
            {
                throw new InvalidOperationException("DisplayManager's CurrentItem is null");
            }

            foreach (IDataControl dc in this.DisplayManager.DataControls)
            {
                // if in tab control, the data control may be invisible
                //if (!dc.Visible)
                //    continue;

                if (dc.ReadOnly)
                {
                    continue;
                }

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

                            //if (this.State == StateType.Add)
                            //{
                            //    if (!dc.Insertable)
                            //        continue;
                            //}
                            //else if (this.State == StateType.Edit)
                            //{
                            //    if (!dc.Editable)
                            //        continue;
                            //}
                            //else
                            //{
                            //    throw new NotSupportedException("Invalid State of " + this.State);
                            //}

                            EntityScript.SetPropertyValue(this.DisplayManager.CurrentItem, dc.Navigator, dc.PropertyName,
                                                             dc.SelectedDataValue);
                        }
                        break;
                    case DataControlType.Expression:
                        {
                            Script.ExecuteStatement(dc.Navigator,
                                    new Dictionary<string, object>{ {"entity", this.DisplayManager.CurrentItem}, 
                                    {"cm", this}});
                        }
                        break;
                }
            }
            return true;
        }

        #endregion

        # region "Operations"

        /// <summary>
        /// 在数据列表有变化时发生
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        /// <summary>
        /// 引发<see cref="ListChanged"/> 事件
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnListChanged(ListChangedEventArgs e)
        {
            if ((e.ListChangedType == ListChangedType.ItemChanged || e.ListChangedType == ListChangedType.ItemAdded)
                && e.NewIndex == this.DisplayManager.Position)
            {
                //  可能有些值是内部计算的
                this.DisplayManager.DisplayCurrent();
            }

            if (this.ListChanged != null)
            {
                this.ListChanged(this, e);
            }
        }

        /// <summary>
        /// BeginningEdit event
        /// </summary>
        public event EventHandler BeginningEdit;

        /// <summary>
        /// EditBegun event
        /// </summary>
        public event EventHandler EditBegun;

        /// <summary>
        /// EndingEdit event
        /// </summary>
        public event EventHandler EndingEdit;

        /// <summary>
        /// EditEnded event
        /// </summary>
        public event EventHandler EditEnded;

        /// <summary>
        /// EditCanceled event
        /// </summary>
        public event EventHandler EditCanceled;

        /// <summary>
        /// CancellingEdit
        /// </summary>
        public event CancelEventHandler CancellingEdit;

        private bool m_inOperating = false;
        /// <summary>
        /// 是否正在操作
        /// </summary>
        public bool InOperating
        {
            get { return m_inOperating; }
        }

        /// <summary>
        /// 在列表末尾添加新的实体类。
        /// 状态设置为<see cref="StateType.Add"></see>，
        /// 当前位置<see cref="P:Feng.IDisplayManager.Position"/>设置为新添加的实体类位置，
        /// 焦点转移到第一个可添加数据的的数据控件
        /// </summary>
        public object AddNew()
        {
            if (!this.AllowInsert)
            {
                throw new InvalidOperationException("您没有添加记录的权限！");
            }

            if (m_inOperating)
            {
                if (!TryCancelEdit())
                {
                    return null;
                }
            }

            if (this.State != StateType.View && this.State != StateType.None)
            {
                throw new InvalidOperationException("当前状态不能添加记录！");
            }

            if (BeginningEdit != null)
            {
                BeginningEdit(this, System.EventArgs.Empty);
            }

            m_inOperating = true;

            object ret = DoAddNew();

            if (EditBegun != null)
            {
                EditBegun(this, System.EventArgs.Empty);
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract object DoAddNew();

        /// <summary>
        /// 编辑当前位置实体类。
        /// 状态设置为<see cref="StateType.Edit"/>，
        /// 焦点转移到第一个可编辑的数据控件
        /// </summary>
        public void EditCurrent()
        {
            if (!this.AllowEdit)
            {
                throw new InvalidOperationException("您没有修改此记录的权限！");
            }

            if (m_inOperating)
            {
                //throw new InvalidOperationException("Now is in operating!");
                if (!TryCancelEdit())
                {
                    return;
                }
            }

            if (this.State != StateType.View)
            {
                throw new InvalidOperationException("当前状态不能修改记录！");
            }

            if (this.DisplayManager.CurrentItem == null)
            {
                throw new InvalidOperationException("请选择要修改的记录！");
            }

            if (BeginningEdit != null)
            {
                BeginningEdit(this, System.EventArgs.Empty);
            }

            m_inOperating = true;

            ICloneable c = this.DisplayManager.CurrentItem as ICloneable;
            if (c != null)
            {
                m_savedItemBeforeEdit = c.Clone();
            }
            else
            {
                m_savedItemBeforeEdit = null;
            }

            DoEditCurrent();

            if (EditBegun != null)
            {
                EditBegun(this, System.EventArgs.Empty);
            }
        }

        private object m_savedItemBeforeEdit;

        /// <summary>
        /// 
        /// </summary>
        protected abstract void DoEditCurrent();


        /// <summary>
        /// 删除当前位置实体类。
        /// 状态为<see cref=" StateType.View"/>
        /// 并重新显示当前实体类数据
        /// </summary>
        /// <exception cref="InvalidOperationException">当前实体类为空</exception>
        public bool DeleteCurrent()
        {
            if (!this.AllowDelete)
            {
                throw new InvalidOperationException("您没有删除此记录的权限！");
            }

            if (m_inOperating)
            {
                //throw new InvalidOperationException("Now is in operating!");
                if (!TryCancelEdit())
                {
                    return false;
                }
            }

            if (this.DisplayManager.CurrentItem == null)
            {
                throw new InvalidOperationException("当前记录为空，请选择要删除的记录！");
            }

            if (this.State != StateType.View)
            {
                throw new InvalidOperationException("当前状态不能删除记录！");
            }

            if (EditBegun != null)
            {
                EditBegun(this, System.EventArgs.Empty);
            }

            m_inOperating = true;

            // ret = true 只代表确认要删除，而不是删除是否成功的标志
            bool ret = DoDeleteCurrent();

            if (ret)
            {
                if (EditEnded != null)
                {
                    EditEnded(this, System.EventArgs.Empty);
                }
            }
            else
            {
                if (EditCanceled != null)
                {
                    EditCanceled(this, System.EventArgs.Empty);
                }
            }

            m_inOperating = false;

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract bool DoDeleteCurrent();

        /// <summary>
        /// 结束编辑或添加。
        /// </summary>
        /// <returns></returns>
        public bool EndEdit()
        {
            return EndEdit(true);
        }

        /// <summary>
        /// 结束编辑或添加。
        /// 检查控件值的合理性, 保存控件值到实体类。
        /// 如果未通过合理性检查，返回false。否则返回true
        /// 如果成果保存，状态转变为<see cref="StateType.View"/>。
        /// 同时清除错误
        /// </summary>
        /// <param name="commit"></param>
        public bool EndEdit(bool commit)
        {
            if (!this.AllowEdit && !this.AllowInsert)
            {
                throw new InvalidOperationException("It can't be saved because it's not AllowEdit or AllowInsert!");
            }

            if (!m_inOperating)
            {
                //throw new InvalidOperationException("Now is not in operating!");
                return true;
            }

            bool ret = true;
            if (commit)
            {
                if (EndingEdit != null)
                {
                    EndingEdit(this, System.EventArgs.Empty);
                }

                //// Success. ret = true only mean it passed the value Check, but there maybe some database errors
                ret = DoEndEdit();

                if (EditEnded != null)
                {
                    EditEnded(this, System.EventArgs.Empty);
                }
            }
            else
            {
                State = StateType.View;
            }

            if (State == StateType.View)
            {
                m_savedItemBeforeEdit = null;
                m_inOperating = false;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool DoEndEdit();

        /// <summary>
        /// 取消当前编辑或添加状态。
        /// 状态转变为<see cref="StateType.View"/>。
        /// 同时清除错误
        /// </summary>
        public void CancelEdit()
        {
            if (!m_inOperating)
            {
                //throw new InvalidOperationException("Now is not in operating!");
                return;
            }

            if (CancellingEdit != null)
            {
                CancelEventArgs e = new CancelEventArgs();
                CancellingEdit(this, e);
                if (e.Cancel)
                    return;
            }

            if (m_savedItemBeforeEdit != null && this.DisplayManager.Position >= 0 && this.DisplayManager.Position < this.DisplayManager.Count)
            {
                this.DisplayManager.Items[this.DisplayManager.Position] = m_savedItemBeforeEdit;
            }

            DoCancelEdit();

            // 要清除主Dao，而不是各个子Dao。主Dao包含所有
            if (this.Dao != null)
            {
                this.Dao.Clear();
            }

            if (EditCanceled != null)
            {
                EditCanceled(this, System.EventArgs.Empty);
            }

            m_inOperating = false;
        }

        /// <summary>
        /// 取消编辑
        /// </summary>
        protected abstract void DoCancelEdit();

        ///// <summary>
        ///// 实体类属性改变后发生
        ///// </summary>
        //public event EventHandler<EntityChangedEventArgs> EntityChanged;

        ///// <summary>
        ///// 当外部操作时，产生Entity更新事件。
        ///// </summary>
        ///// <param name="success">外部操作是否成功</param>
        //public virtual void RaiseEntityChangedEvent(bool success)
        //{
        //    if (success)
        //    {
        //        ListChangedEventArgs arg2 = new ListChangedEventArgs(ListChangedType.ItemChanged,
        //                                                             this.DisplayManager.Position);
        //        OnListChanged(arg2);
        //    }
        //    else
        //    {
        //        //DoOnFailedOperation();
        //    }
        //}

        private IBaseDao m_dao;
        /// <summary>
        ///  Data Access Layer
        /// </summary>
        public IBaseDao Dao
        {
            get { return m_dao; }
            set
            {
                m_dao = value;

                // ?????为啥 EnablePage 有RowNumber
                //if (m_dao is Feng.Data.DataTableDao)
                //{
                //    this.DisplayManager.SearchManager.EnablePage = false;
                //}
            }
        }

        /// <summary>
        /// 处理当前位置的Entity改变后的事情
        /// </summary>
        /// <returns></returns>
        public bool ProcessCurrentEntityChanged()
        {
            return ProcessEntityChanged(this.DisplayManager.Position);
        }

        /// <summary>
        /// 处理位于Position位置的Entity改变后的事情（保存，更新界面）
        /// </summary>
        /// <returns></returns>
        public virtual bool ProcessEntityChanged(int position)
        {
            object entity = this.DisplayManager.Items[position];
            if (entity == null)
            {
                throw new ArgumentException("Position is invalid!");
            }
            EntityChangedEventArgs arg;
            ListChangedEventArgs arg2;
            switch (this.State)
            {
                case StateType.Add:
                    arg = new EntityChangedEventArgs(EntityChangedType.Add, entity);
                    arg2 = new ListChangedEventArgs(ListChangedType.ItemAdded, position);
                    break;
                case StateType.Delete:
                    arg = new EntityChangedEventArgs(EntityChangedType.Delete, entity);
                    arg2 = new ListChangedEventArgs(ListChangedType.ItemDeleted, position);
                    break;
                case StateType.Edit:
                    arg = new EntityChangedEventArgs(EntityChangedType.Edit, entity);
                    arg2 = new ListChangedEventArgs(ListChangedType.ItemChanged, position);
                    break;
                default:
                    throw new InvalidOperationException("Invalid State");
            }

            OnEntityChanged(arg);

            if (arg.Exception == null)
            {
                OnListChanged(arg2);
                return true;
            }
            else
            {
                if (arg.Exception is InvalidUserOperationException)
                {
                    // 只是提示，不刷新。而且在Dao中，不清空内部Dao数据（Clear）。
                }
                else
                {
                    DoOnFailedOperation();
                }
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void DoOnFailedOperation()
        {
            return;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEntityChanged(EntityChangedEventArgs e)
        {
            IBaseDao dao = this.Dao;
            if (dao == null)
            {
                throw new ArgumentException("dao is null!");
            }

            try
            {
                switch (e.EntityChangedType)
                {
                    case EntityChangedType.Add:
                        dao.Save(e.Entity);
                        break;
                    case EntityChangedType.Edit:
                        dao.Update(e.Entity);
                        break;
                    case EntityChangedType.Delete:
                        dao.Delete(e.Entity);
                        break;
                    default:
                        throw new NotSupportedException("invalid e.EntityChangedType");
                }
                e.Exception = null;
            }
            catch (Exception ex)
            {
                e.Exception = ex;

                ExceptionProcess.ProcessWithNotify(ex);

                //if (this.ArchiveGridHelper.m_bThrowExceptionWhenBllError)
                //{
                //    throw;
                //}
                //else
                //{
                //    ExceptionProcess.ProcessWithNotify(ex);
                //    //base.SearchManager.ReloadData();
                //}
            }
        }

        #endregion

        #region "Check"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public virtual bool CheckControlValue(IDataControl dc)
        {
            var cm = this;

            bool ret = true;
            if (!dc.ReadOnly)
            {
                try
                {
                    string errMsg = null;
                    if (dc.NotNull && dc.SelectedDataValue == null)
                    {
                        errMsg = "请输入" + dc.Caption + "！";
                        ret = false;
                    }
                    cm.ControlCheckExceptionProcess.ShowError(dc, errMsg);
                }
                catch (ControlCheckException ex)
                {
                    
                    cm.ControlCheckExceptionProcess.ShowError(ex.InvalidDataControl, ex.Message);
                }
            }
            return ret;
        }
        /// <summary>
        /// 检查控件值的合理性
        /// 如不合理，用<see cref="ControlCheckExceptionProcess"/>处理，返回false；
        /// 否则返回true。
        /// </summary>
        /// <exception cref="ControlCheckException">控件值不符合标准</exception>
        public virtual bool CheckControlsValue()
        {
            bool ret = true;

            foreach (IDataControl dc in this.DisplayManager.DataControls)
            {
                //if ((this.State == StateType.Add && dc.Insertable) || (this.State == StateType.Edit && dc.Editable))
                ret &= CheckControlValue(dc);
            }

            try
            {
                this.CheckControls.CheckControlsValue();
            }
            catch (ControlCheckException ex)
            {
                ret = false;
                if (this.ControlCheckExceptionProcess != null)
                {
                    this.ControlCheckExceptionProcess.ShowError(ex.InvalidDataControl, ex.Message);
                }
            }

            return ret;
        }

        #endregion

        #region "Permission"

        /// <summary>
        /// 是否允许删除
        /// </summary>
        public bool AllowDelete { get; set; }

        /// <summary>
        /// 是否允许添加
        /// </summary>
        public bool AllowInsert { get; set; }

        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public bool AllowEdit { get; set; }

        #endregion

        #region "Clone"
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
        protected static void Copy(AbstractControlManager src, AbstractControlManager dest)
        {
            dest.CheckControls.AddRange(src.CheckControls);
            dest.StateControls.AddRange(src.StateControls);

            dest.Dao = src.Dao;
        }
        #endregion

        #region "Utils"
        /// <summary>
        /// 
        /// </summary>
        public void OnCurrentItemChanged()
        {
            ControlManagerExtention.OnCurrentItemChanged(this);
        }
        #endregion
    }
}