using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using Xceed.Grid;
using Xceed.Validation;

namespace Feng.Grid
{
    /// <summary>
    /// 数据绑定，且支持简单操作的Grid
    /// </summary>
    public partial class ArchiveBoundGrid : DataBoundGrid, IArchiveGrid
    {
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_archiveGridHelper != null)
                {
                    m_archiveGridHelper.Dispose();
                    m_archiveGridHelper.RowSaving -= new System.ComponentModel.CancelEventHandler(row_Saving);
                    m_archiveGridHelper = null;
                }
                if (this.ControlManager != null)
                {
                    this.ControlManager.Dispose();
                    this.ControlManager = null;
                }
            }
            base.Dispose(disposing);
        }

        private ArchiveGridHelper m_archiveGridHelper;
        /// <summary>
        /// ArchiveGridHelper
        /// </summary>
        public ArchiveGridHelper ArchiveGridHelper
        {
            get
            {
                if (m_archiveGridHelper == null)
                {
                    m_archiveGridHelper = new ArchiveGridHelper(this);
                }
                return m_archiveGridHelper;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveBoundGrid()
            : base()
        {
        }

        /// <summary>
        /// SetControlManager
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="gridName"></param>
        public void SetControlManager(IControlManager cm, string gridName)
        {
            ArchiveGridExtention.SetControlManager(this, cm, gridName);
        }

        /// <summary>
        /// 操作管理器
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IControlManager ControlManager
        {
            get;
            set;
        }

        /// <summary>
        /// InsertRow
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AddThrowInsertRow
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CreateGrid()
        {
            base.CreateGrid();

            this.ArchiveGridHelper.Initialize();
            this.ArchiveGridHelper.RowSaving += new System.ComponentModel.CancelEventHandler(row_Saving);

            this.SetColumnManagerRowNotNull();

            this.SetGridPermissions();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        //public override void SetDataBinding(object dataSource, string dataMember)
        //{
        //    base.SetDataBinding(dataSource, dataMember);
        //}

        #region "Same"
        /// <summary>
        /// SaveRowValues
        /// </summary>
        /// <param name="row"></param>
        /// <param name="grid"></param>
        public static void SaveRowValues(Xceed.Grid.CellRow row, IArchiveGrid grid)
        {
            if (row is InsertionRow)
            {
                //grid.ControlManager.AddNew();
                grid.ControlManager.State = StateType.Add;

                row.Tag = grid.ControlManager.DisplayManager.CurrentItem;
                grid.AddThrowInsertRow = true;
            }
            else
            {
                grid.ControlManager.EditCurrent();
                // when insert, dataRow endingedit event will occur also
                row.Tag = grid.ControlManager.DisplayManager.CurrentItem;
            } 

            grid.ControlManager.EndEdit(true);

            grid.AddThrowInsertRow = false;

            grid.ArchiveGridHelper.ClearError(row);
        }

        /// <summary>
        /// row_EditEnded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void row_Saving(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Xceed.Grid.CellRow row = (Xceed.Grid.CellRow)sender;

            SaveRowValues(row, this);

            e.Cancel = (this.ControlManager.State != StateType.View);

            if (e.Cancel)
            {
                this.ControlManager.CancelEdit();
            }
        }


        ///// <summary>
        ///// AddGridValidationExpression
        ///// U should call it before load data
        ///// </summary>
        ///// <param name="columnName"></param>
        ///// <param name="expression"></param>
        //public void AddGridValidationExpression(string columnName, ValidationExpression expression)
        //{
        //    this.ArchiveGridHelper.SetGridValidation(columnName, expression);
        //}

        /// <summary>
        /// 
        /// </summary>
        public Xceed.Grid.InsertionRow InsertionRow
        {
            get { return this.ArchiveGridHelper == null ? null : this.ArchiveGridHelper.InsertionRow; }
        }

        /// <summary>
        /// 是否允许内部操作
        /// </summary>
        [DefaultValue(false)]
        public bool AllowInnerInsert { get; set; }

        /// <summary>
        /// 是否允许内部操作
        /// </summary>
        [DefaultValue(false)]
        public bool AllowInnerEdit { get; set; }

        /// <summary>
        /// 是否允许内部操作
        /// </summary>
        [DefaultValue(false)]
        public bool AllowInnerDelete { get; set; }

        /// <summary>
        /// 用户自定义Validate
        /// </summary>
        public event System.ComponentModel.CancelEventHandler ValidateRow
        {
            add { this.ArchiveGridHelper.ValidateRow += value; }
            remove { this.ArchiveGridHelper.ValidateRow -= value; }
        }

        /// <summary>
        /// 是否是主从模式里的从Grid（如果是，要设置状态）
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsInDetailMode
        {
            get;
            set;
        }

        /// <summary>
        /// 对显示控件设置State
        /// 无动作此处
        /// </summary>
        public override void SetState(StateType state)
        {
            if (IsInDetailMode)
            {
                base.SetState(state);
            }
        }

        /// <summary>
        /// CheckControlValue
        /// </summary>
        /// <returns></returns>
        public bool CheckControlValue()
        {
            ArchiveGridExtention.CheckControlValue(this);
            // 自己处理错误
            return true;
        }
        #endregion
    }
}