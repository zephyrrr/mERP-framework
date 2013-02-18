using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Feng.Utils;

namespace Feng.Grid
{
    /// <summary>
    /// Description of MyExcelGrid.
    /// </summary>
    public partial class MyExcelGrid : MyGrid
    {
        private const int s_initDataRowsCount = 100;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridTemplate"></param>
        public MyExcelGrid(IGrid gridTemplate)
            : this()
        {
            BeginInitialize();

            foreach (Xceed.Grid.Column col in gridTemplate.Columns)
            {
                this.Columns.Add(new Columns.MyColumn(col));
            }

            EndInitialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyExcelGrid()
        {
            this.InitializeComponent();
        }

        private Xceed.Grid.Cell m_dragStartCell;
        void MyExcelGrid_DragStart(object sender, DragEventArgs e)
        {
            Xceed.Grid.Cell cell = sender as Xceed.Grid.DataCell;
            if (cell != null)
            {
                string s = GetSelectedCellsStrings(cell);
                if (!string.IsNullOrEmpty(s))
                {
                    e.Data.SetData(DataFormats.Text, s);

                    m_dragStartCell = cell;
                }
            }
        }

        void MyExcelGrid_DragDrop(object sender, DragEventArgs e)
        {
            Xceed.Grid.Cell cell = sender as Xceed.Grid.DataCell;
            if (cell != null && m_dragStartCell != null)
            {
                string data = (string)e.Data.GetData(DataFormats.Text);
                if (!string.IsNullOrEmpty(data))
                {
                    PasteToCells(cell, false, data);

                    ClearSelectedCells(m_dragStartCell);
                    m_dragStartCell = null;
                }
            }
        }

        

        /// <summary>
        /// BeginInitialize
        /// </summary>
        public void BeginInitialize()
        {
            this.DataRowTemplate = new NumberedRow();
        }

        /// <summary>
        /// EndInitialize
        /// </summary>
        public void EndInitialize()
        {
            this.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

            this.DataRowTemplate.RowSelector.MouseDown += new MouseEventHandler(RowSelector_MouseDown);
            foreach (Xceed.Grid.DataCell cell in this.DataRowTemplate.Cells)
            {
                cell.MouseDown += new System.Windows.Forms.MouseEventHandler(cell_MouseDown);
            }
            for (int i = 0; i < s_initDataRowsCount; ++i)
            {
                this.DataRows.AddNew().EndEdit();
            }

            this.KeyDown += new KeyEventHandler(MyExcelGrid_KeyDown);
            this.DataRowTemplate.EditEnded += new EventHandler(DataRowTemplate_EditEnded);

            //this.EnableDragDrop = true;
            //this.GridDragStart += new DragEventHandler(MyExcelGrid_DragStart);
            //this.GridDragDrop += new DragEventHandler(MyExcelGrid_DragDrop);
        }

        void DataRowTemplate_EditEnded(object sender, EventArgs e)
        {
            bool hasValue = false;
            Xceed.Grid.DataRow row = sender as Xceed.Grid.DataRow;
            for (int j = 0; j < this.Columns.Count; ++j)
            {
                if (row.Cells[j].Value != null)
                {
                    hasValue = true;
                }
            }

            if (hasValue)
            {
                int lastRow = FindFirstEmptyRow();
                if (row.Index > lastRow - 1)
                {
                    ServiceProvider.GetService<IMessageBox>().ShowWarning("此行上方还有空行，此行不会保存！");
                }
                else if (row.Index == this.DataRows.Count - 1)
                {
                    this.DataRows.AddNew().EndEdit();
                }
            }
        }

        private int FindFirstEmptyRow()
        {
            for (int i = 0; i < this.DataRows.Count - 1; ++i)
            {
                bool hasValue = false;
                for (int j = 0; j < this.Columns.Count; ++j)
                {
                    if (this.DataRows[i].Cells[j].Value != null)
                    {
                        hasValue = true;
                    }
                }
                if (!hasValue)
                    return i;
            }
            return this.DataRows.Count;
        }

        void MyExcelGrid_KeyDown(object sender, KeyEventArgs e)
        {
            m_contextCell = this.CurrentCell;

            if (e.KeyData == (Keys.Control | Keys.C))
            {
                tsmCopy_Click(tsmCopy, System.EventArgs.Empty);
            }
            else if (e.KeyData == (Keys.Control | Keys.X))
            {
                tsmCut_Click(tsmCut, System.EventArgs.Empty);
            }
            else if (e.KeyData == (Keys.Control | Keys.V))
            {
                tsmPaste_Click(tsmPaste, System.EventArgs.Empty);
            }
        }

        private int FindLastEditRow()
        {
            for (int i = this.DataRows.Count - 1; i >= 0; --i)
            {
                for (int j = 0; j < this.Columns.Count; ++j)
                {
                    if (this.DataRows[i].Cells[j].Value != null)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        private Xceed.Grid.RowSelector m_contextSelector;
        void RowSelector_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tsmSetRowsInColumn.Visible = false;

                m_contextSelector = sender as Xceed.Grid.RowSelector;
                contextMenuStrip1.Show(m_contextSelector.PointToScreen(new System.Drawing.Point(e.X, e.Y)));

                m_contextCell = null;
            }
        }

        private Xceed.Grid.Cell m_contextCell;
        void cell_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tsmSetRowsInColumn.Visible = true;

                m_contextCell = sender as Xceed.Grid.DataCell;
                contextMenuStrip1.Show(m_contextCell.PointToScreen(new System.Drawing.Point(e.X, e.Y)));

                m_contextSelector = null;
            }
        }

        void tsmDelete_Click(object sender, EventArgs e)
        {
            if (m_contextCell != null)
            {
                m_contextCell.Value = null;
                m_contextCell = null;
            }
            else if (m_contextSelector != null)
            {
                IList<Xceed.Grid.Row> rowList = GetSelectedRowsToOperation(m_contextSelector.Row);
                DeleteRows(rowList);
                
                m_contextSelector = null;
            }
        }

        private void DeleteRows(IList<Xceed.Grid.Row> rowList)
        {
            if (rowList != null && rowList.Count > 0)
            {
                int[] deleted = new int[rowList.Count];
                int i = 0;
                foreach (Xceed.Grid.CellRow row in rowList)
                {
                    deleted[i] = ((row as Xceed.Grid.DataRow).Index);
                    i++;
                }
                Array.Sort(deleted);
                for (int j = deleted.Length - 1; j >= 0; --j)
                {
                    this.DataRows.RemoveAt(deleted[j]);
                }
            }
        }

        void tsmAddNew_Click(object sender, System.EventArgs e)
        {
            if (m_contextCell != null)
            {
                this.DataRows.AddNew().EndEdit();
                m_contextCell = null;
            }
            else if (m_contextSelector != null)
            {
                this.DataRows.AddNew().EndEdit();
                m_contextSelector = null;
            }
        }

        void tsmInsert_Click(object sender, EventArgs e)
        {
            int currentRowIdx = -1;
            if (m_contextCell != null)
            {
                currentRowIdx = (m_contextCell.ParentRow as Xceed.Grid.DataRow).Index;
                m_contextCell = null;
            }
            else if (m_contextSelector != null)
            {
                currentRowIdx = (m_contextSelector.Row as Xceed.Grid.DataRow).Index;
                m_contextSelector = null;
            }
            if (currentRowIdx != -1)
            {
                int lastRow = this.FindLastEditRow();
                
                if (lastRow >= currentRowIdx)
                {
                    if (lastRow == this.DataRows.Count - 1)
                    {
                        this.DataRows.AddNew().EndEdit();
                    }

                    for (int i = lastRow; i >= currentRowIdx; --i)
                    {
                        for (int j = 0; j < this.Columns.Count; ++j)
                        {
                            this.DataRows[i + 1].Cells[j].Value = this.DataRows[i].Cells[j].Value;
                        }
                    }
                    for (int j = 0; j < this.Columns.Count; ++j)
                    {
                        this.DataRows[currentRowIdx].Cells[j].Value = null;
                    }
                }
            }
        }

        void tsmPaste_Click(object sender, EventArgs e)
        {
            if (m_contextCell != null)
            {
                PasteClipboardToCells(m_contextCell, false);
                m_contextCell = null;
            }
            else if (m_contextSelector != null)
            {
                PasteClipboardToCells((m_contextSelector.Row as Xceed.Grid.DataRow).Cells[0], true);
            }
        }

        private static void PasteToCells(Xceed.Grid.Cell currentCell, bool hasHeader, string data)
        {
            if (currentCell != null)
            {
                if (!string.IsNullOrEmpty(data))
                {
                    string[] ss = data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
                    if (ss.Length == 0)
                        return;
                    int iStart = 0;
                    string[] headers = null;
                    if (hasHeader)
                    {
                        headers = ss[0].Split(new char[] { '\t' });
                        iStart = 1;
                    }

                    Xceed.Grid.Cell finalDestCell = null;

                    int currentRowIdx = (currentCell.ParentRow as Xceed.Grid.DataRow).Index;
                    int currentColumnIdx = currentCell.ParentColumn.VisibleIndex;
                    for (int i = 0; i < ss.Length - iStart; ++i)
                    {
                        // 超出现有行数，新增
                        if (currentRowIdx + i >= currentCell.ParentGrid.DataRows.Count)
                        {
                            currentCell.ParentGrid.DataRows.AddNew().EndEdit();
                        }

                        string[] values = ss[i + iStart].Split(new char[] { '\t' });

                        for (int j = 0; j < values.Length; ++j)
                        {
                            Xceed.Grid.Cell destCell = null;
                            if (hasHeader)
                            {
                                // Caption<->GridColumnName
                                foreach (Xceed.Grid.Cell cell in currentCell.ParentGrid.DataRows[i].Cells)
                                {
                                    if (j < headers.Length && cell.ParentColumn.Title == headers[j])
                                    {
                                        destCell = cell;
                                        break;
                                    }
                                }
                            }
                            else
                            {

                                if (currentColumnIdx + j < currentCell.ParentGrid.Columns.Count)
                                {
                                    try
                                    {
                                        destCell = currentCell.ParentGrid.DataRows[currentRowIdx + i]
                                            .Cells[currentCell.ParentGrid.Columns.GetColumnAtVisibleIndex(currentColumnIdx + j).FieldName];
                                    }
                                    catch (ArgumentException)
                                    {
                                    }
                                }
                                // destCell = currentCell.ParentGrid.DataRows[currentRowIdx + i].Cells[currentColumnIdx + j];
                            }
                            if (destCell == null)
                                continue;

                            finalDestCell = destCell;

                            string destFromValue = values[j];
                            object destToValue = null;
                            if (string.IsNullOrEmpty(destFromValue))
                            {
                                destToValue = null;
                            }
                            else
                            {
                                if (destCell.CellEditorManager != null && destCell.CellEditorManager is INameValueControl)
                                {
                                    destToValue = ((INameValueControl)destCell.CellEditorManager).GetValue(destFromValue);
                                }
                                else
                                {
                                    destToValue = Feng.Utils.ConvertHelper.ChangeType(destFromValue, destCell.ParentColumn.DataType);
                                    if (destToValue == null)
                                    {
                                        ServiceProvider.GetService<IMessageBox>().ShowWarning("\"" + destFromValue + "\"类型不匹配目标类型！");
                                        continue;
                                    }
                                }
                            }

                            object srcValue = destCell.Value;
                            if (!(srcValue == null && destToValue != null))
                            {
                                if (!Feng.Utils.ReflectionHelper.ObjectEquals(srcValue, destToValue))
                                {
                                    if (MessageForm.ShowYesNo("已有值 \"" + destCell.GetDisplayText() + "\",是否改变?"))
                                    {
                                        continue;
                                    }
                                }
                            }

                            destCell.Value = destToValue;
                        }
                    }
                    if (finalDestCell != null)
                    {
                        currentCell.GridControl.CurrentCell = finalDestCell;
                        currentCell.GridControl.CurrentCell.BringIntoView();
                    }
                }
            }
        }

        /// <summary>
        /// 从currentCell粘贴数据
        /// </summary>
        /// <param name="currentCell"></param>
        public static void PasteClipboardToCells(Xceed.Grid.Cell currentCell, bool hasHeader)
        {
            string data = Feng.Windows.Utils.ClipboardHelper.GetTextFromClipboard();
            PasteToCells(currentCell, hasHeader, data);
        }

        void tsmCopy_Click(object sender, EventArgs e)
        {
            if (m_contextCell != null)
            {
                CopySelectedCellsToClipboard(m_contextCell);
                m_contextCell = null;
            }
            else if (m_contextSelector != null)
            {
                CopySelectedRowsToClipboard(m_contextSelector.Row);
                m_contextSelector = null;
            }
        }

        private void ClearSelectedCells(Xceed.Grid.Cell currentCell)
        {
            if (currentCell.GridControl.SelectedRows.Contains(currentCell.ParentRow))
            {
                foreach (Xceed.Grid.CellRow row in currentCell.GridControl.SelectedRows)
                {
                    if (row.ParentGrid == currentCell.ParentGrid)
                    {
                        Xceed.Grid.Cell cell = row.Cells[currentCell.ParentColumn.Index];
                        cell.Value = null;
                    }
                }
            }
            else
            {
                currentCell.Value = null;
            }
            
        }

        void tsmCut_Click(object sender, EventArgs e)
        {
            if (m_contextCell != null)
            {
                CopySelectedCellsToClipboard(m_contextCell);
                ClearSelectedCells(m_contextCell);
                m_contextCell = null;
            }
            else if (m_contextSelector != null)
            {
                IList<Xceed.Grid.Row> rowList = CopySelectedRowsToClipboard(m_contextSelector.Row);
                DeleteRows(rowList);
                m_contextSelector = null;
            }
        }

        private void tsmSetRowsInColumn_Click(object sender, EventArgs e)
        {
            Xceed.Grid.Cell contextMenuCell = m_contextCell;

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

            string fieldName = contextMenuCell.ParentColumn.FieldName;

            List<Xceed.Grid.DataRow> modifiedRows = new List<Xceed.Grid.DataRow>();

            //bool userBreak = false;
            foreach (Xceed.Grid.DataRow row in contextMenuCell.GridControl.SelectedRows)
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

                if (doit)
                {
                    try
                    {
                        row.BeginEdit();
                        object oldValue = row.Cells[fieldName].Value;
                        row.Cells[fieldName].Value = contextMenuCell.Value;
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

                        if (MessageForm.ShowYesNo("出现错误，是否继续？"))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
