using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;
using Xceed.Grid;
using Xceed.Validation;
using Feng.Windows.Forms;

namespace Feng.Grid
{
    /// <summary>
    /// 数据不绑定，且支持简单操作的Grid
    /// </summary>
    public partial class ArchiveUnboundGrid : DataUnboundGrid, IArchiveGrid, IWindowControl
    {
        Control IWindowControl.Control 
        { 
            get { return this; } 
        }

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
                    m_archiveGridHelper.RowSaving -= new System.ComponentModel.CancelEventHandler(row_Saving);
                    m_archiveGridHelper.Dispose();
                    m_archiveGridHelper = null;
                }
                if (this.ControlManager != null)
                {
                    this.ControlManager.ListChanged -= new ListChangedEventHandler(m_cm_ListChanged);

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
        public ArchiveUnboundGrid()
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

            this.CreateArchiveEvents();

            this.SetGridPermissions();
            this.SetColumnManagerRowNotNull();

            this.ControlManager.ListChanged += new ListChangedEventHandler(m_cm_ListChanged);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        //public override void SetDataBinding(object dataSource, string dataMember)
        //{
        //    base.SetDataBinding(dataSource, dataMember);

        //    SetGridRowCellProperties(this);
        //}

        

        private void m_cm_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnListChanged(e, this);
        }

        internal static void OnListChanged(ListChangedEventArgs e, IArchiveGrid grid)
        {
            if (grid.DisplayManager.InBatchOperation)
            {
                return;
            }

            try
            {
                grid.DisplayManager.BeginBatchOperation();
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        if (!grid.AddThrowInsertRow)
                        {
                            Xceed.Grid.DataRow row = grid.DataRows.AddNew();
                            grid.SetDataRowsIListData(grid.ControlManager.DisplayManager.CurrentItem, row);
                            grid.SetGridRowCellPermissions(grid.DataRows[grid.ControlManager.DisplayManager.Position]);
                            grid.SetGridRowCellColors(grid.DataRows[grid.ControlManager.DisplayManager.Position]);
                            row.EndEdit();
                            grid.CurrentRow = row;
                        }
                        grid.ControlManager.DisplayManager.Position = grid.ControlManager.DisplayManager.Count - 1;

                        // 通过InsertRow添加的时候，此时Xceed还未添加上Row ---???
                        // 确定：此时已经有DataRow
                        if (grid.AddThrowInsertRow)
                        {
                            // 重新设置DataRow信息，因为可能在保存的时候有其他信息生成
                            // grid.CurrentRow = InsertRow
                            grid.SetDataRowsIListData(grid.ControlManager.DisplayManager.CurrentItem, grid.DataRows[e.NewIndex]);
                            grid.SetGridRowCellPermissions(grid.DataRows[e.NewIndex]);
                            grid.SetGridRowCellColors(grid.DataRows[e.NewIndex]);
                        }

                        //this.ShowTitleRow();
                        break;
                    case ListChangedType.ItemChanged:
                        //bool inEdit = grid.DataRows[e.NewIndex].IsBeingEdited;
                        //if (inEdit)
                        //{
                        //    grid.DataRows[e.NewIndex].CancelEdit();
                        //}
                        grid.SetDataRowsIListData(grid.ControlManager.DisplayManager.Items[e.NewIndex], grid.DataRows[e.NewIndex]);
                        grid.SetGridRowCellPermissions(grid.DataRows[e.NewIndex]);
                        grid.SetGridRowCellColors(grid.DataRows[e.NewIndex]);

                        // maybe there is a detailGrid, so load data in detailGrid
                        grid.ControlManager.DisplayManager.OnPositionChanged(System.EventArgs.Empty);

                        // 可能已经不能修改，不再进入修改状态
                        //if (inEdit)
                        //{
                        //    grid.DataRows[e.NewIndex].BeginEdit();
                        //}
                        break;
                    case ListChangedType.ItemDeleted:
                        if (e.NewIndex >= 0 && e.NewIndex < grid.DataRows.Count)
                        {
                            grid.DataRows.RemoveAt(e.NewIndex);
                            //this.ShowTitleRow();
                        }
                        break;
                    default:
                        throw new NotSupportedException("not supported listChangedType");
                }
            }
            finally
            {
                grid.DisplayManager.EndBatchOperation();
                grid.DisplayManager.OnPositionChanged(System.EventArgs.Empty);
            }

        }

        /// <summary>
        /// SaveRowValues
        /// </summary>
        /// <param name="row"></param>
        /// <param name="grid"></param>
        public static void SaveRowValues(Xceed.Grid.CellRow row, IArchiveGrid grid)
        {
            if (row is InsertionRow)
            {
                grid.ControlManager.AddNew();

                row.Tag = grid.ControlManager.DisplayManager.CurrentItem;
                grid.AddThrowInsertRow = true;
            }
            else
            {
                grid.ControlManager.EditCurrent();
                // when insert, dataRow endingedit event will occur also
                row.Tag = grid.ControlManager.DisplayManager.CurrentItem;
            }

            try
            {
                foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
                {
                    if (row.Cells[info.GridColumnName] != null && !string.IsNullOrEmpty(info.PropertyName))
                    {
                        if (info.GridColumnType == GridColumnType.Normal)
                        {
                            if (row.Cells[info.GridColumnName].ReadOnly)
                            {
                                continue;
                            }

                            if (info.GridColumnType == GridColumnType.Normal)
                            {
                                EntityScript.SetPropertyValue(grid.ControlManager.DisplayManager.CurrentItem, info.Navigator, info.PropertyName,
                                                                 row.Cells[info.GridColumnName].Value);
                            }
                            else if (info.GridColumnType == GridColumnType.ExpressionColumn)
                            {
                                Script.ExecuteStatement(info.Navigator,
                                    new Dictionary<string, object>{ {"entity", grid.ControlManager.DisplayManager.CurrentItem}, 
                                    {"cm", grid.ControlManager}});
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
                throw;
            }

            grid.ControlManager.EndEdit(true);

            grid.AddThrowInsertRow = false;

            grid.ArchiveGridHelper.ClearError(row);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void row_Saving(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Xceed.Grid.CellRow row = (Xceed.Grid.CellRow) sender;

            SaveRowValues(row, this);

            e.Cancel = (this.ControlManager.State != StateType.View);

            if (e.Cancel)
            {
                this.ControlManager.CancelEdit();
            }
        }

        protected override void DisplayManager_PositionChanged(object sender, EventArgs e)
        {
            if (this.ControlManager.State != StateType.View && this.ControlManager.State != StateType.None)
                return;

            // 当通过InsertionRow添加时，如果触发PositionChanged，则会CancelEdit
            if (this.ControlManager.InOperating)
                return;

            // only set currentRow
            base.DisplayManager_PositionChanged(sender, e);
        }

        #region "Same"

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
            get { return this.ArchiveGridHelper.InsertionRow; }
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