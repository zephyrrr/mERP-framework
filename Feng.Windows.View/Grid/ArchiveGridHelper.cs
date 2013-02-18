using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid;
using Xceed.Validation;
using System.ComponentModel;
using System.Windows.Forms;
using Feng.Windows.Utils;
using Feng.Windows.Forms;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ArchiveGridHelper : IDisposable
    {
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                this.m_cm = null;
                this.ValidateRow = null;

                m_grid.GridHelper.ContextMenuStripForCell.Opening -= new System.ComponentModel.CancelEventHandler(ContextMenuStripForCell_Opening);

                RemoveRowEvents();
                RemoveCellEvents();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        public ArchiveGridHelper(IArchiveGrid grid)
        {
            m_grid = grid;
            m_cm = grid.ControlManager;

            InitializeComponent();

            this.tsmDelete.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconErase.png").Reference;
            this.tsmDeleteBatch.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconErase.png").Reference;
            this.tsmDelete.Visible = false;

            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(m_grid.GridName);
            if (Authority.AuthorizeByRule(gridInfo.AllowInnerMenu) || Authority.IsDeveloper())
            {
                m_grid.GridHelper.MergeContenxtMenuStripForCell(this.contextMenuStrip1);
            }
        }

        private IArchiveGrid m_grid;
        private IControlManager m_cm;

        /// <summary>
        /// Initialize
        /// </summary>
        internal void Initialize()
        {
            if (m_grid.AllowInnerInsert && m_cm.AllowInsert)
            {
                m_insertionRow = new Xceed.Grid.InsertionRow();
                m_grid.FixedFooterRows.Insert(0, m_insertionRow);
                m_insertionRow.FitHeightToEditors = true;
                m_insertionRow.RowSelector.MouseDown += new MouseEventHandler(InsertionRowSelector_MouseDown);
                foreach (Xceed.Grid.Cell cell in m_insertionRow.Cells)
                {
                    if (cell.CellEditorManager is Editors.MyComboBoxEditor)
                    {
                        (cell.CellEditorManager as Editors.MyComboBoxEditor).TemplateControl.DropDownDirection = Xceed.Editors.DropDownDirection.Up;
                    }
                    else if (cell.CellEditorManager is Editors.MyFreeComboBoxEditor)
                    {
                        (cell.CellEditorManager as Editors.MyFreeComboBoxEditor).TemplateControl.DropDownDirection = Xceed.Editors.DropDownDirection.Up;
                    }
                    else if (cell.CellEditorManager is Editors.MyOptionPickerEditor)
                    {
                        (cell.CellEditorManager as Editors.MyOptionPickerEditor).TemplateControl.DropDownDirection = Xceed.Editors.DropDownDirection.Up;
                    }
                }

                m_insertionRow.EditBegun += new EventHandler(m_insertionRow_EditBegun);
            }
            m_grid.ReadOnly = !(m_grid.AllowInnerEdit && m_cm.AllowEdit);

            if (!m_grid.AllowInnerEdit)
            {
                foreach (Xceed.Grid.DataCell cell in m_grid.DataRowTemplate.Cells)
                {
                    if (!(cell.ParentColumn is Columns.CheckColumn))
                    {
                        cell.ReadOnly = true;
                    }
                }
            }

            AddRowEvents();
            AddCellEvents();

            AddValidations();

            m_grid.GridHelper.ContextMenuStripForCell.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStripForCell_Opening);
        }

        void InsertionRowSelector_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStripForInsertionRowSelector.Show((sender as Xceed.Grid.RowSelector).PointToScreen(new System.Drawing.Point(e.X, e.Y)));
            }
        }
        private void contextMenuStripForInsertionRowSelector_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        void tsmPaste_Click(object sender, System.EventArgs e)
        {
            MyGrid.PasteClipBoardToInsertionRow(m_grid);
        }

        void m_insertionRow_EditBegun(object sender, EventArgs e)
        {
            IList<GridColumnInfo> gridColumnInfos = ADInfoBll.Instance.GetGridColumnInfos(m_grid.GridName);

            try
            {
                foreach (GridColumnInfo info in gridColumnInfos)
                {
                    if (!string.IsNullOrEmpty(info.DataControlDefaultValue))
                    {
                        object defaultValue = ControlFactory.GetControlDefaultValueByUser(info.DataControlDefaultValue);
                        if (defaultValue != null)
                        {
                            m_insertionRow.Cells[info.GridColumnName].Value = Feng.Utils.ConvertHelper.ChangeType(defaultValue, GridColumnInfoHelper.CreateType(info));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }

        #region "Init & Events"

        private Xceed.Grid.InsertionRow m_insertionRow;

        /// <summary>
        /// 
        /// </summary>
        public Xceed.Grid.InsertionRow InsertionRow
        {
            get { return m_insertionRow; }
        }

        private void AddRowEvents()
        {
            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(m_grid.GridName);
            if (Authority.AuthorizeByRule(gridInfo.AllowInnerEdit))
            {
                m_grid.DataRowTemplate.EditBegun += new EventHandler(row_EditBegun);
                m_grid.DataRowTemplate.EndingEdit += new System.ComponentModel.CancelEventHandler(row_EndingEdit);
                //m_grid.DataRowTemplate.EditEnded += new EventHandler(row_EditEnded);
                m_grid.DataRowTemplate.EditCanceled += new EventHandler(row_EditCanceled);
                m_grid.DataRowTemplate.ValidationError += new RowValidationErrorEventHandler(row_ValidationError);
            }

            if (Authority.AuthorizeByRule(gridInfo.AllowInnerInsert))
            {
                // InsertionRow will also raise DataRow Events
                // It will raise two times, but for show error description, we should use InsertionRow Events
                if (m_insertionRow != null)
                {
                    m_insertionRow.EditBegun += new EventHandler(row_EditBegun);
                    m_insertionRow.EndingEdit += new System.ComponentModel.CancelEventHandler(row_EndingEdit);
                    m_insertionRow.ValidationError += new RowValidationErrorEventHandler(row_ValidationError);
                    m_insertionRow.EditCanceled += new EventHandler(row_EditCanceled);
                }
            }
        }
        private void RemoveRowEvents()
        {
            m_grid.DataRowTemplate.EditBegun -= new EventHandler(row_EditBegun);
            m_grid.DataRowTemplate.EndingEdit -= new System.ComponentModel.CancelEventHandler(row_EndingEdit);
            //m_grid.DataRowTemplate.EditEnded -= new EventHandler(row_EditEnded);
            m_grid.DataRowTemplate.EditCanceled -= new EventHandler(row_EditCanceled);
            m_grid.DataRowTemplate.ValidationError -= new RowValidationErrorEventHandler(row_ValidationError);

            if (m_insertionRow != null)
            {
                m_insertionRow.EditBegun -= new EventHandler(row_EditBegun);
                m_insertionRow.EndingEdit -= new System.ComponentModel.CancelEventHandler(row_EndingEdit);
                m_insertionRow.ValidationError -= new RowValidationErrorEventHandler(row_ValidationError);
                m_insertionRow.EditCanceled -= new EventHandler(row_EditCanceled);
            }
        }
        private bool m_isInInsertRowEdit, m_isInDataRowEdit;

        void row_EditBegun(object sender, EventArgs e)
        {
            if (sender is Xceed.Grid.InsertionRow)
            {
                m_isInInsertRowEdit = true;
            }
            else if (sender is Xceed.Grid.DataRow)
            {
                m_isInDataRowEdit = true;
            }
        }

        private void row_EndingEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Todo: Xceed.Grid 不能插入空行
            if (m_grid.DisplayManager.InBatchOperation)
            {
                return;
            }

            CellRow row = sender as CellRow;
            if (row is Xceed.Grid.InsertionRow)
            {
                if (!m_isInInsertRowEdit)
                    return;

                e.Cancel = !DoValidateRow(row, e);

                if (!e.Cancel)
                {
                    if (RowSaving != null)
                    {
                        RowSaving(sender, e);
                    }
                    if (!e.Cancel)
                    {
                        m_isInInsertRowEdit = false;
                    }
                }
            }
            else if (row is Xceed.Grid.DataRow)
            {
                if (!m_isInDataRowEdit)
                    return;

                e.Cancel = !DoValidateRow(row, e);

                if (!e.Cancel)
                {
                    if (RowSaving != null)
                    {
                        RowSaving(sender, e);
                    }

                    // 保存成功否？
                    if (!e.Cancel)
                    {
                        m_isInDataRowEdit = false;
                    }
                }
            }
        }

        private void row_ValidationError(object sender, RowValidationErrorEventArgs e)
        {
            e.CancelEdit = false;
            if (e.Exception != null)
            {
                if (e.Exception is Xceed.Grid.GridException)
                {
                }
                else
                {
                    ExceptionProcess.ProcessWithResume(e.Exception);
                }
            }
        }

        private void row_EditCanceled(object sender, EventArgs e)
        {
            CellRow row = sender as CellRow;

            ClearError(row);
        }

        private void AddCellEvents()
        {
            foreach (Cell cell in m_grid.DataRowTemplate.Cells)
            {
                cell.EditEntered += new EventHandler(cell_EditEntered);
                cell.EditLeft += new EditLeftEventHandler(cell_EditLeft);
            }

            if (m_insertionRow != null)
            {
                foreach (Cell cell in m_insertionRow.Cells)
                {
                    cell.EditEntered += new EventHandler(cell_EditEntered);
                    cell.EditLeft += new EditLeftEventHandler(cell_EditLeft);
                }
            }
        }
        private void RemoveCellEvents()
        {
            foreach (Cell cell in m_grid.DataRowTemplate.Cells)
            {
                cell.EditEntered -= new EventHandler(cell_EditEntered);
                cell.EditLeft -= new EditLeftEventHandler(cell_EditLeft);
            }

            if (m_insertionRow != null)
            {
                foreach (Cell cell in m_insertionRow.Cells)
                {
                    cell.EditEntered -= new EventHandler(cell_EditEntered);
                    cell.EditLeft -= new EditLeftEventHandler(cell_EditLeft);
                }
            }
        }

        private object m_originalSelectedDataValue;
        void cell_EditLeft(object sender, EditLeftEventArgs e)
        {
            Xceed.Grid.Cell cell = sender as Xceed.Grid.Cell;

            if (!Feng.Utils.ReflectionHelper.ObjectEquals(m_originalSelectedDataValue, cell.Value))
            {
                GridColumnInfo info = cell.ParentColumn.Tag as GridColumnInfo;
                string s = info != null ? info.GridColumnName : cell.ParentColumn.FieldName;
                m_cm.DisplayManager.OnSelectedDataValueChanged(new SelectedDataValueChangedEventArgs(s, cell));
            }
        }

        void cell_EditEntered(object sender, EventArgs e)
        {
            Xceed.Grid.Cell cell = sender as Xceed.Grid.Cell;
            m_originalSelectedDataValue = cell.Value;
        }


        internal event System.ComponentModel.CancelEventHandler RowSaving;

        #endregion

        #region "Validate"

        private Feng.Windows.Forms.MyValidationProvider validationProvider1 =
            new Feng.Windows.Forms.MyValidationProvider();

        /// <summary>
        /// AddGridValidationExpression
        /// U should call it before load data
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="expression"></param>
        internal void SetGridValidation(string columnName, ValidationExpression expression)
        {
            this.validationProvider1.SetValidationExpression(m_grid.DataRowTemplate.Cells[columnName], expression);

            if (m_insertionRow != null)
            {
                this.validationProvider1.SetValidationExpression(m_insertionRow.Cells[columnName], expression);
            }
        }

        /// <summary>
        /// 用户自定义Validate
        /// </summary>
        public event System.ComponentModel.CancelEventHandler ValidateRow;

        internal bool DoValidateRow(Xceed.Grid.CellRow row, System.ComponentModel.CancelEventArgs e)
        {
            bool allValid = true;

            foreach (Cell cell in row.Cells)
            {
                // cell.Visible (when invisible validate also)
                if (cell.Visible && !cell.ReadOnly)
                {
                    allValid &= this.validationProvider1.Validate(cell, false, true);

                    if (!allValid)
                    {
                        return false;
                    }
                }
            }

            if (ValidateRow != null)
            {
                ValidateRow(row, e);
                allValid &= !e.Cancel;
            }

            //Validator validator = ValidationFactory.CreateValidator(m_cm.DisplayManager.MainDataManager.EntityInfo.EntityType);
            //ValidationResults results = validator.Validate(m_cm.DisplayManager.CurrentItem);
            //if (!results.IsValid)
            //{
            //    foreach (ValidationResult result in results)
            //    {
            //        row.Cells[result.Key].ErrorDescription = result.Message;
            //    }
            //    return false;
            //}

            return allValid;
        }

        private static ValidationCriterion TryAndValidations(ValidationCriterion cri1, ValidationCriterion cri2)
        {
            ValidationCriterion cri = null;
            if (cri1 != null || cri2 != null)
            {
                if (cri1 != null && cri2 != null)
                {
                    cri = new AndValidCriterion(cri1.Name + "_And_" + cri2.Name,
                     Xceed.Validation.ValidationLevel.Manual,
                     false, true, new Xceed.Validation.CustomValidationMessages(),
                     cri1, cri2);
                }
                else if (cri1 != null)
                {
                    cri = cri1;
                }
                else
                {
                    cri = cri2;
                }
            }
            return cri;
        }
        internal static ValidationCriterion GetValidationCriterion(GridColumnInfo columnInfo, IControlManager cm)
        {
            ValidationCriterion cri1 = null;
            if (!string.IsNullOrEmpty(columnInfo.ValidRegularExpression))
            {
                cri1 = new MyRegularExpressionCriterion(columnInfo.GridColumnName,
                    Xceed.Validation.ValidationLevel.Manual,
                    new System.Text.RegularExpressions.Regex(columnInfo.ValidRegularExpression, System.Text.RegularExpressions.RegexOptions.Singleline),
                    false, true, new Xceed.Validation.CustomValidationMessages());
                cri1.CustomValidationMessages.RegularExpression = columnInfo.ValidErrorMessage;
            }
            ValidationCriterion cri2 = null;
            if (!string.IsNullOrEmpty(columnInfo.ValidScript))
            {
                cri2 = new ScriptCriterion(columnInfo.GridColumnName,
                    Xceed.Validation.ValidationLevel.Manual,
                    columnInfo.ValidScript, new Dictionary<string, object> { { "cm", cm } },
                    false, true, new Xceed.Validation.CustomValidationMessages());
                cri2.CustomValidationMessages.RegularExpression = columnInfo.ValidErrorMessage;
            }

            ValidationCriterion cri3 = null;
            if (Authority.AuthorizeByRule(columnInfo.NotNull))
            {
                cri3 = new RequiredFieldCriterion(columnInfo.GridColumnName,
                    Xceed.Validation.ValidationLevel.Manual, GridColumnInfoHelper.CreateType(columnInfo),
                    false, true, new Xceed.Validation.CustomValidationMessages());
            }
            var ret = TryAndValidations(TryAndValidations(cri1, cri2), cri3);
            if (ret != null)
            {
                ret.Name = columnInfo.GridColumnName;
            }
            return ret;
        }

        public void RemoveValidation(string gridColumnName)
        {
            SetGridValidation(gridColumnName, null);
        }

        public void AddValidations()
        {
            IList<GridColumnInfo> gridColumnInfos = ADInfoBll.Instance.GetGridColumnInfos(m_grid.GridName);
            if (gridColumnInfos.Count > 0)
            {
                foreach (GridColumnInfo columnInfo in gridColumnInfos)
                {
                    ValidationCriterion cri = GetValidationCriterion(columnInfo, m_cm);
                    if (cri != null)
                    {
                        SetGridValidation(columnInfo.GridColumnName, cri);
                    }
                }
            }
            else
            {
                CreatePropertyValidations();
            }
        }

        /// <summary>
        /// 按照Property来默认设置Validation
        /// </summary>
        private void CreatePropertyValidations()
        {
            foreach (Xceed.Grid.Column column in m_grid.Columns)
            {
                if (!column.Visible)
                {
                    continue;
                }

                GridColumnInfo info = column.Tag as GridColumnInfo;
                if (info == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(info.Navigator) && !string.IsNullOrEmpty(info.PropertyName))
                {
                    var attribute = m_cm.DisplayManager.EntityInfo.GetPropertMetadata(info.PropertyName);
                    if (attribute != null)
                    {
                        ValidationCriterion cri1 = null;
                        if (attribute.NotNull)
                        {
                            cri1 = new RequiredFieldCriterion(info.GridColumnName, ValidationLevel.Manual, column.DataType, false, true, null);
                        }

                        ValidationCriterion cri2 = null;
                        if (attribute.Length > 0)
                        {
                            cri2 = new MaxLengthFieldCriterion(info.GridColumnName, ValidationLevel.Manual, attribute.Length, false, true, null);
                        }

                        SetGridValidation(column.FieldName, TryAndValidations(cri1, cri2));
                    }
                }
            }
        }

        ///// <summary>
        ///// CancelEdit
        ///// </summary>
        //private void CancelEdit1()
        //{
        //    for (int i = 0; i < m_grid.DataRows.Count; ++i)
        //    {
        //        if (m_grid.DataRows[i].IsBeingEdited)
        //        {
        //            m_grid.DataRows[i].CancelEdit();
        //        }
        //    }

        //    if (m_insertionRow != null && m_insertionRow.IsBeingEdited)
        //    {
        //        m_insertionRow.CancelEdit();
        //    }

        //    //ClearError();
        //}

        /// <summary>
        /// 清除错误
        /// </summary>
        internal void ClearError(Xceed.Grid.CellRow row)
        {
            foreach (Cell cell in row.Cells)
            {
                cell.ResetErrorDescription();
            }
            row.ResetErrorDescription();

            if (m_insertionRow != null)
            {
                foreach (Cell cell in m_insertionRow.Cells)
                {
                    cell.ResetErrorDescription();
                    cell.ResetReadOnly();
                }
            }
        }

        ///// <summary>
        ///// ErrorIcon(all set icon in validationProvider)
        ///// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public System.Drawing.Icon ErrorIcon
        //{
        //    get { return m_grid.ErrorIcon; }
        //    set
        //    {
        //        m_grid.ErrorIcon = value;
        //        this.validationProvider1.Icon = value;
        //    }
        //}

        #endregion

        #region "ContextMenu"

        private void tsmDeleteBatch_Click(object sender, EventArgs e)
        {
            Xceed.Grid.Row contextRow = m_grid.GridHelper.ContextRow;
            if (contextRow == null)
                return;

            Xceed.Grid.DetailGrid parentGrid = contextRow.ParentGrid;
            //int originalCnt = parentGrid.DataRows.Count;

            List<DataRow> list = new List<DataRow>();
            if (m_grid.GridControl.SelectedRows.Contains(contextRow))
            {
                if (!MessageForm.ShowYesNo("选定记录将要被删除，是否继续？", "确定", true))
                {
                    return;
                }
                foreach (Xceed.Grid.Row row in m_grid.GridControl.SelectedRows)
                {
                    Xceed.Grid.DataRow dataRow = row as Xceed.Grid.DataRow;
                    if (dataRow != null && dataRow.ParentGrid == parentGrid)
                    {
                        list.Add(dataRow);
                    }
                }
            }
            else
            {
                if (!MessageForm.ShowYesNo("当前记录将要被删除，是否继续？", "确认", true))
                {
                    return;
                }
                Xceed.Grid.DataRow dataRow = contextRow as Xceed.Grid.DataRow;
                if (dataRow != null)
                {
                    list.Add(dataRow);
                }
            }

            try
            {
                int unDeleteCnt = 0;
                foreach (Xceed.Grid.DataRow dataRow in list)
                {
                    bool ret = DeleteByRow(dataRow);
                    if (!ret)
                    {
                        unDeleteCnt++;
                    }
                }

                //int nowCnt = parentGrid.DataRows.Count;
                if (unDeleteCnt > 0)
                {
                    MessageForm.ShowInfo(string.Format("因权限原因，有{0}条记录未删除!", unDeleteCnt));
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
        }

        private bool DeleteByRow(Xceed.Grid.DataRow row)
        {
            GridHelper.EndEditRow(row);

            bool rowAllowDelete = m_grid.AllowInnerDelete
                && m_cm.AllowDelete && Permission.AuthorizeByRule(ADInfoBll.Instance.GetGridRowInfo(m_grid.GridName).AllowDelete, row.Tag);
            if (rowAllowDelete)
            {
                m_cm.DisplayManager.Position = row.Index;
                return m_cm.DeleteCurrent();
            }
            return false;
        }

        void ContextMenuStripForCell_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_grid.GridHelper.ContextCell != null
                || m_grid.GridHelper.ContextSelector != null)
            {
                Xceed.Grid.CellRow rowCurrent;
                if (m_grid.GridHelper.ContextCell != null)
                {
                    rowCurrent = m_grid.GridHelper.ContextCell.ParentRow;
                }
                else
                {
                    rowCurrent = m_grid.GridHelper.ContextSelector.Row as Xceed.Grid.CellRow;
                }

                tsm默认本列.Visible = true;
                tsmDeleteBatch.Visible = true;
                tsmDeleteBatch.Enabled = true;
                tsm默认本列.Enabled = true;

                if ((rowCurrent != null && rowCurrent.IsBeingEdited))        // || m_grid.IsInDetailMode 不应该设置
                {
                    tsm默认本列.Visible = false;
                    tsmDeleteBatch.Visible = false;
                }

                if (!m_grid.ControlManager.AllowDelete || !m_grid.AllowInnerDelete)
                {
                    tsmDeleteBatch.Visible = false;
                }

                if (m_grid.ReadOnly)
                {
                    tsmDeleteBatch.Enabled = false;
                    tsm默认本列.Enabled = false;
                }
            }

            if (m_grid.GridHelper.ContextCell != null)
            {
                GridColumnInfo columnInfo = m_grid.GridHelper.ContextCell.ParentColumn.Tag as GridColumnInfo;
                if (columnInfo != null && columnInfo.AllowSetList.HasValue)
                {
                    tsm默认本列.Visible = true;
                    m_allowSetListWarning = columnInfo.AllowSetList.Value;
                }
                else
                {
                    tsm默认本列.Visible = false;
                }
            }
            else if (m_grid.GridHelper.ContextSelector != null)
            {
                tsm默认本列.Visible = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private bool m_allowSetListWarning = true;

        static ArchiveGridHelper()
        {
            RegisterCommand();
        }
        public static ICommand BatchSetCellValueCommand = new Command("默认本列设置值命令");
        private static void RegisterCommand()
        {
            CommandManager.Register(BatchSetCellValueCommand, OnBatchSetCellValueCommand);
        }

        private static void OnBatchSetCellValueCommand(object sender, ExecutedEventArgs e)
        {
            Xceed.Grid.Cell contextMenuCell = sender as Xceed.Grid.DataCell;
            ArchiveGridHelper gridHelper = e.Parameter as ArchiveGridHelper;

            string fieldName = contextMenuCell.ParentColumn.FieldName;

            // 开始操作

            gridHelper.m_grid.DisplayManager.BeginBatchOperation();
            List<Xceed.Grid.DataRow> modifiedRows = new List<Xceed.Grid.DataRow>();

            gridHelper.m_grid.GridControl.SuspendLayout();

            //bool userBreak = false;
            foreach (Xceed.Grid.DataRow row in gridHelper.m_grid.GridControl.SelectedRows)
            {
                if (!row.Visible)
                {
                    continue;
                }
                if (row == contextMenuCell.ParentRow)
                {
                    continue;
                }

                if (row.Cells[fieldName].ReadOnly)
                {
                    continue;
                }

                if (row.Cells[fieldName].Value == null && contextMenuCell.Value == null)
                {
                    continue;
                }

                if (row.Cells[fieldName].Value != null && contextMenuCell.Value != null
                    && row.Cells[fieldName].Value.ToString() == contextMenuCell.Value.ToString())
                {
                    continue;
                }

                bool doit = true;
                if (row.Cells[fieldName].Value != null)
                {
                    if (gridHelper.m_allowSetListWarning)
                    {
                        DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("已有值 \"" + row.Cells[fieldName].GetDisplayText() + "\",是否改变?", "确认", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information);
                        if (dialogResult == DialogResult.Cancel)
                        {
                            //userBreak = true;
                            break;
                        }
                        else
                        {
                            doit = (dialogResult == DialogResult.Yes);
                        }
                    }
                }

                if (doit)
                {
                    try
                    {
                        row.BeginEdit();
                        object oldValue = row.Cells[fieldName].Value;

                        // 设置目标列值
                        row.Cells[fieldName].Value = contextMenuCell.Value;

                        if (row.Cells[fieldName].Value == oldValue)
                            continue;

                        // 有些因为Combo的原因，设置值有限制
                        row.Cells[fieldName].EnterEdit();
                        row.Cells[fieldName].LeaveEdit(true);
                        if (row.Cells[fieldName].Value == null && contextMenuCell.Value != null)
                        {
                            row.Cells[fieldName].Value = oldValue;
                            row.CancelEdit();
                            continue;
                        }
                        try
                        {
                            row.EndEdit();
                        }
                        catch (Exception)
                        {
                            // 当保存不进去的时候，cancel
                            row.CancelEdit();
                        }

                        modifiedRows.Add(row);

                        gridHelper.m_grid.ControlManager.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, row.Index));

                        //CancelEventArgs ee = new CancelEventArgs();
                        //row_EndingEdit(row, ee);
                    }
                    // 不知道
                    //// when in 批量费用登记（有分组的界面）
                    //catch (Xceed.Grid.GridValidationException)
                    //{
                    //}
                    catch (Exception ex)
                    {
                        row.CancelEdit();

                        ExceptionProcess.ProcessWithNotify(ex);

                        if (!MessageForm.ShowYesNo("出现错误，是否继续？"))
                        {
                            break;
                        }
                    }
                }
            }

            // 即使用户取消，也要保存先前的内容
            //if (userBreak)
            //{
            //}
            //else
            //{
            IBatchDao batchDao = gridHelper.m_grid.ControlManager.Dao as IBatchDao;
            if (batchDao == null)
            {
                ServiceProvider.GetService<IMessageBox>().ShowWarning("不支持批量保存，将逐条保存！");
            }
            try
            {
                if (batchDao != null)
                {
                    batchDao.SuspendOperation();
                }

                foreach (Xceed.Grid.DataRow row in modifiedRows)
                {
                    object entity = row.Tag;
                    foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(gridHelper.m_grid.GridName))
                    {
                        if (row.Cells[info.GridColumnName] != null && !string.IsNullOrEmpty(info.PropertyName))
                        {
                            if (row.Cells[info.GridColumnName].ReadOnly)
                            {
                                continue;
                            }

                            if (info.GridColumnType == GridColumnType.Normal)
                            {
                                EntityScript.SetPropertyValue(entity, info.Navigator, info.PropertyName,
                                                                 row.Cells[info.GridColumnName].Value);
                            }
                        }
                    }
                    gridHelper.m_grid.ControlManager.Dao.Update(entity);
                }
                if (batchDao != null)
                {
                    batchDao.ResumeOperation();
                }
            }
            catch (Exception ex)
            {
                if (batchDao != null)
                {
                    batchDao.CancelSuspendOperation();
                }
                ExceptionProcess.ProcessWithNotify(ex);

                gridHelper.m_grid.ReloadData();
            }
            finally
            {

            }

            gridHelper.m_grid.GridControl.ResumeLayout();

            gridHelper.m_grid.DisplayManager.EndBatchOperation();

            MyGrid.SetCurrentRow(gridHelper.m_grid.GridControl, gridHelper.m_grid.DataRows[gridHelper.m_grid.DisplayManager.Position]);
            MyGrid.SyncSelectedRowToCurrentRow(gridHelper.m_grid.GridControl);
        }

        private void tsm默认本列_Click(object sender, EventArgs e)
        {
            Xceed.Grid.Cell contextMenuCell = m_grid.GridHelper.ContextCell;

            if (contextMenuCell == null)
            {
                return;
            }

            if (contextMenuCell.IsBeingEdited)
            {
                contextMenuCell.LeaveEdit(true);
            }
            if (contextMenuCell.ParentRow.IsBeingEdited)
            {
                contextMenuCell.ParentRow.EndEdit();
            }

            BatchSetCellValueCommand.Execute(contextMenuCell, new ExecutedEventArgs(this));
        }
        #endregion

        
    }
}