using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// ControlManagerBindingSource for DataView
    /// </summary>
    public class ControlManagerBindingSource : AbstractControlManager, IWindowControlManager
    {
        #region "Constructor"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_helper != null)
                {
                    m_helper.Dispose();
                    m_helper = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sm"></param>
        public ControlManagerBindingSource(ISearchManager sm)
            : base(new DisplayManagerBindingSource(sm))
        {
            m_bs = (this.DisplayManager as DisplayManagerBindingSource).BindingSource;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dm"></param>
        protected ControlManagerBindingSource(IDisplayManager dm)
            : base(dm)
        {
            m_bs = (this.DisplayManager as DisplayManagerBindingSource).BindingSource;
        }
        private BindingSource m_bs;
        #endregion

        # region "Operations"
        ///// <summary>
        ///// 刷新当前行,不在对应控件显示,否则会刷掉刚填入的值
        ///// </summary>
        //public void RefreshCurrentRow()
        //{
        //    if (CurrentPosition != -1)
        //    {
        //        RefreshRow(CurrentPosition);
        //        State = State;
        //        ShowCurrentRow();
        //    }
        //}

        ///// <summary>
        ///// 刷新当前行,不在对应控件显示,否则会刷掉刚填入的值
        ///// </summary>
        ///// <param name="position">位置</param>
        //private void RefreshRow(int position)
        //{
        //    T entity = (m_bs.DataSource as IList<T>)[position];
        //    object id = m_entityType.InvokeMember(m_primaryKeyName, BindingFlags.GetProperty, null, entity, null, null, null, null);
        //    entity = Repository.Get<T>(id);
        //    (m_bs.DataSource as IList<T>)[position] = entity;
        //}
        /// <summary>
        /// 是否能变动Position（现有记录有改动的情况下要询问）
        /// </summary>
        /// <returns></returns>
        public override bool TryCancelEdit()
        {
            if (this.State == StateType.Add)
            {
                if (ServiceProvider.GetService<IMessageBox>().ShowYesNoDefaultNo("所添加的内容将不被保存，是否继续？", "确认"))
                {
                    this.CancelEdit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (this.State == StateType.Edit)
            {
                // 已经被其他用户删除
                if (this.DisplayManager.CurrentItem == null)
                {
                    this.CancelEdit();
                    return true;
                }

                if (this.Changed)
                {
                    if (ServiceProvider.GetService<IMessageBox>().ShowYesNoDefaultNo("所修改的内容将不被保存，是否继续？", "确认"))
                    {
                        this.CancelEdit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    this.CancelEdit();
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// 在列表末尾添加新的实体类。
        /// 如当前状态为StateType.Add或StateType.Edit，则直接返回，不添加新记录
        /// 状态设置为<see cref="StateType.Add"/>，
        /// 当前位置设置为新添加的实体类位置，
        /// 焦点转移到第一个可添加数据的的数据控件
        /// </summary>
        protected override object DoAddNew()
        {
            if (State == StateType.Add || State == StateType.Edit)
            {
                return null;
            }

            State = StateType.Add;

            return m_bs.AddNew();

            //foreach (IDataManager dm in this.DisplayManager.DataManagers)
            //{
            //    if (dm != this.DisplayManager.DataManagers)
            //    {
            //        dm.Item = dm.CreateNew();
            //    }
            //}

            //this.DisplayManager.DataControls.FocusFirstInsertableControl();
        }

        /// <summary>
        /// 编辑当前位置实体类。
        /// 如当前状态为StateType.Add或StateType.Edit，则直接返回，不修改记录
        /// 状态设置为<see cref="StateType.Edit"/>，
        /// 焦点转移到第一个可编辑的数据控件
        /// </summary>
        protected override void DoEditCurrent()
        {
            if (State == StateType.Add || State == StateType.Edit)
            {
                return;
            }

            State = StateType.Edit;

            this.DisplayManager.DataControls.FocusFirstEditableControl();
        }

        /// <summary>
        /// 删除当前位置实体类。
        /// 如当前状态为StateType.Add或StateType.Edit，则直接返回，不删除记录
        /// 状态为<see cref=" StateType.View"/>
        /// 并重新显示当前实体类数据
        /// </summary>
        /// <exception cref="InvalidOperationException">当前实体类为空</exception>
        protected override bool DoDeleteCurrent()
        {
            if (State == StateType.Add || State == StateType.Edit)
            {
                return false;
            }

            if (this.DisplayManager.CurrentItem == null)
            {
                throw new InvalidOperationException("Current is null");
            }

            State = StateType.Delete;
            if (ProcessCurrentEntityChanged())
            {
                m_bs.RemoveCurrent();
            }

            // Remove 不一定会产生PositionChanged事件（Remove Index 0 Item）
            State = StateType.View;
            this.DisplayManager.DisplayCurrent();

            return true;
        }

        /// <summary>
        /// 结束编辑。
        /// 检查控件值的合理性, 保存控件值到实体类。
        /// 如果未通过合理性检查，返回false。否则返回true
        /// 如果成果保存，状态转变为<see cref="StateType.View"/>。
        /// 同时清除错误
        /// </summary>
        protected override bool DoEndEdit()
        {
            if (ProcessCurrentEntityChanged())
            {
                m_bs.EndEdit();

                State = StateType.View;
            }

            // 已经在DoChangeCurrentEntity()中引发ListChanged事件
            //DisplayCurrent();

            if (base.ControlCheckExceptionProcess != null)
            {
                base.ControlCheckExceptionProcess.ClearAllError();
            }

            return true;
        }

        /// <summary>
        /// 取消当前编辑或添加状态。
        /// 状态转变为<see cref="StateType.View"/>。
        /// 同时清除错误
        /// </summary>
        protected override void DoCancelEdit()
        {
            m_bs.CancelEdit();

            // CancelEdit不一定会产生PositionChanged事件，例如Edit CancelEdit
            State = StateType.View;
            this.DisplayManager.DisplayCurrent();

            if (base.ControlCheckExceptionProcess != null)
            {
                base.ControlCheckExceptionProcess.ClearAllError();
            }
        }

        #endregion

        #region "Validation"

        private ValidationHelper m_helper;

        /// <summary>
        /// AddValidationExpression
        /// </summary>
        /// <param name="dataControlName"></param>
        /// <param name="expression"></param>
        public void SetValidation(string dataControlName, Xceed.Validation.ValidationExpression expression)
        {
            if (m_helper == null)
            {
                m_helper = new ValidationHelper(this);
            }
            m_helper.SetValidation(dataControlName, expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataControlName"></param>
        public void RemoveValidation(string dataControlName)
        {
            if (m_helper == null)
            {
                m_helper = new ValidationHelper(this);
            }
            m_helper.RemoveValidation(dataControlName);
        }

        /// <summary>
        /// 检查控件值是否符合标准
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ControlCheckException">控件值不符合标准</exception>
        public override bool CheckControlValue(IDataControl dc)
        {
            if (m_helper != null)
            {
                return ControlManager.CheckControlValue(this, m_helper, dc);
            }
            else
            {
                return base.CheckControlValue(dc);
            }
        }

        #endregion

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ControlManagerBindingSource cm = new ControlManagerBindingSource(this.DisplayManager.Clone() as IDisplayManager);
            Copy(this, cm);
            return cm;
        }

        private static IBaseDao s_defaultDao;
        protected override void OnEntityChanged(EntityChangedEventArgs e)
        {
            if (this.Dao == null)
            {
                if (s_defaultDao == null)
                {
                    s_defaultDao = new Feng.NH.NHibernateDao<IEntity>(null);
                }
                this.Dao = s_defaultDao;
            }
            base.OnEntityChanged(e);
        }
    }
}