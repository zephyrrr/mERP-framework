using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ControlManager : AbstractControlManager, IWindowControlManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void  Dispose(bool disposing)
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
        public ControlManager(ISearchManager sm)
            : base(new DisplayManager(sm))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dm"></param>
        protected ControlManager(IDisplayManager dm)
            : base(dm)
        {
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ControlManager cm = new ControlManager(this.DisplayManager.Clone() as IDisplayManager);
            Copy(this, cm);
            return cm;
        }

        #region "Operation"
        /// <summary>
        /// 是否能变动Position（现有记录有改动的情况下要询问）
        /// </summary>
        /// <returns></returns>
        public override bool TryCancelEdit()
        {
            if (base.InOperating)
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
            }
            return true;
        }

        /// <summary>
        /// 创建新Entity
        /// </summary>
        /// <returns></returns>
        protected virtual object AddNewItem()
        {
            System.Data.DataView dv = this.DisplayManager.Items as System.Data.DataView;
            if (dv != null)
            {
                //System.Data.DataRow row = dv.Table.NewRow();
                //dv.Table.Rows.Add(row);
                //return row;

                System.Data.DataRowView rowView = dv.AddNew();
                rowView.EndEdit();
                return rowView;
            }
            return null;
        }

        /// <summary>
        /// 创建新数据源
        /// </summary>
        /// <returns></returns>
        protected virtual IList NewList()
        {
            return this.DisplayManager.SearchManager.GetSchema() as IList;
        }

        private int m_beforeAddPos = -1;

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

            // before load, m_item is null
            if (this.DisplayManager.Items == null)
            {
                IList newList = NewList();

                // not to set BindingControls
                if (this.DisplayManager.BindingControls.Count > 0)
                {
                    IBindingControl[] bcs = new IBindingControl[this.DisplayManager.BindingControls.Count];
                    this.DisplayManager.BindingControls.CopyTo(bcs, 0);
                    this.DisplayManager.BindingControls.Clear();

                    this.DisplayManager.SetDataBinding(newList, string.Empty);

                    this.DisplayManager.BindingControls.AddRange(bcs);
                }
                else
                {
                    this.DisplayManager.SetDataBinding(newList, string.Empty);
                }
            }

            object entity = AddNewItem();

            m_beforeAddPos = this.DisplayManager.Position;
            this.DisplayManager.Position = this.DisplayManager.Items.Count - 1;

            State = StateType.Add;

            return entity;
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

            if (this.DisplayManager.Position == -1)
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

            int oldPos = this.DisplayManager.Position;
            int newPos = oldPos;
            if (newPos < 0)
            {
                newPos = 0;
            }
            if (newPos >= this.DisplayManager.Count - 1)
            {
                newPos = this.DisplayManager.Count - 2; // 这里还没删除，Count还是原数目
            }

            if (ProcessCurrentEntityChanged())
            {
                this.DisplayManager.Items.RemoveAt(oldPos);
                this.DisplayManager.Position = newPos;
            }

            if (this.State == StateType.Delete)
            {
                State = StateType.View;
            }
            // Remove 不一定会产生PositionChanged事件（Remove Index 0 Item）
            this.DisplayManager.OnPositionChanged(System.EventArgs.Empty);

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
                State = StateType.View;
            }

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
            if (base.State == StateType.Add)
            {
                this.DisplayManager.Items.RemoveAt(this.DisplayManager.Items.Count - 1);

                State = StateType.View;

                // maybe count = 0
                //System.Diagnostics.Debug.Assert(m_beforeAddPos != -1, "Before Add Old Pos should not be -1!");

                this.DisplayManager.Position = m_beforeAddPos;
                //this.DisplayManager.Position = (this.DisplayManager.Items.Count == 0 ? -1 : this.DisplayManager.Items.Count - 1);
            }

            State = StateType.View;

            // CancelEdit不一定会产生PositionChanged事件，例如Edit CancelEdit
            this.DisplayManager.OnPositionChanged(System.EventArgs.Empty);

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
        /// <param name="fullPropertyName"></param>
        public void RemoveValidation(string dataControlName)
        {
            if (m_helper == null)
            {
                m_helper = new ValidationHelper(this);
            }
            m_helper.RemoveValidation(dataControlName);
        }

        internal static bool CheckControlValue(IControlManager cm, ValidationHelper helper, IDataControl dc)
        {
            bool ret = true;
            if (!dc.ReadOnly)
            {
                string errMsg = helper.ValidateControl(dc.Name);
                cm.ControlCheckExceptionProcess.ShowError(dc, errMsg);

                if (!string.IsNullOrEmpty(errMsg))
                {
                    ret = false;
                }
            }
            return ret;
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
                return CheckControlValue(this, m_helper, dc);
            }
            else
            {
                return base.CheckControlValue(dc);
            }
        }

        #endregion

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