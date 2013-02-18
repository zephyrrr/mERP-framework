using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Xceed.Grid;
using Feng.Utils;

namespace Feng.Grid.Columns
{
    /// <summary>
    /// 
    /// </summary>
    internal partial class CheckColumnHelper
    {
        private string m_selectColumnName;

        private IGrid m_grid;
        private Xceed.Grid.Column m_checkColumn;

        internal CheckColumnHelper(Xceed.Grid.Column checkColumn, IGrid grid)
        {
            InitializeComponent();

            m_checkColumn = checkColumn;
            m_grid = grid;
            if (m_grid.GridControl != null)
            {
                m_grid.GridControl.SelectionMode = SelectionMode.MultiExtended;
            }
            (checkColumn.CellEditorManager.TemplateControl as Xceed.Editors.WinCheckBox).ThreeState = false;

            this.m_selectColumnName = checkColumn.FieldName;
        }

        /// <summary>
        /// 
        /// </summary>
        internal CheckColumnHelper(Xceed.Grid.Column checkColumn)
            : this(checkColumn, checkColumn.GridControl as IGrid)
        {
            
        }


        /// <summary>
        /// 
        /// </summary>
        internal void Initialize()
        {
            //this.ParentGrid.DataRowTemplate.Cells[SelectCaption].ValueChanged += new EventHandler(FilterGrid_SelectionValueChanged);

            Xceed.Grid.ColumnManagerRow columnManagerRow = m_grid.GetColumnManageRow();
            if (columnManagerRow != null && columnManagerRow.Cells[m_selectColumnName] != null)
            {
                columnManagerRow.Cells[m_selectColumnName].MouseDown += new MouseEventHandler(ColumnManageRowSelectionCell_MouseDown);
            }

            m_grid.DataRowTemplate.Cells[m_selectColumnName].ValueChanged += new EventHandler(CheckColumn_ValueChanged);
            CheckColumn_ValueChanged(m_grid.DataRowTemplate.Cells[m_selectColumnName], System.EventArgs.Empty);
        }

        /// <summary>
        /// Clear Values in SumRow
        /// </summary>
        public void ResetSumRow()
        {
            ValueRow valueRow = m_grid.GetValueRow();
            if (valueRow != null)
            {
                foreach (Cell cell in valueRow.Cells)
                {
                    cell.Value = null;
                }
            }
        }

        private bool m_isBatchSetting;

        void CheckColumn_ValueChanged(object sender, EventArgs e)
        {
            if (m_isBatchSetting)
                return;

            SummaryRow sumRow = m_grid.GetSummaryRow();

            if (sumRow != null)
            {
                List<string> sumColumnNames = new List<string>();
                List<string> countColumnNames = new List<string>();
                foreach (Cell cell in sumRow.Cells)
                {
                    SummaryCell sumCell = cell as SummaryCell;
                    if (sumCell != null)
                    {
                        if (sumCell.StatFunction == StatFunction.Sum)
                        {
                            sumColumnNames.Add(cell.ParentColumn.FieldName);
                        }
                        else if (sumCell.StatFunction == StatFunction.Count)
                        {
                            countColumnNames.Add(cell.ParentColumn.FieldName);
                        }
                    }
                }

                if (sumColumnNames.Count > 0 || countColumnNames.Count > 0)
                {
                    ValueRow valueRow = m_grid.GetValueRow();
                    if (valueRow == null)
                    {
                        sumRow.Visible = false;
                        valueRow = m_grid.AddValueRowToFixedFooter();
                    }

                    bool allSelect = true;
                    bool noneSelect = true;
                    decimal[] sum = new decimal[sumColumnNames.Count];
                    for (int i = 0; i < sumColumnNames.Count; ++i)
                    {
                        sum[i] = 0;
                    }
                    int[] count = new int[countColumnNames.Count];
                    for (int i = 0; i < countColumnNames.Count; ++i)
                    {
                        count[i] = 0;
                    }

                    foreach (Xceed.Grid.DataRow row in m_grid.DataRows)
                    {
                        if (Convert.ToBoolean(row.Cells[m_selectColumnName].Value))
                        {
                            for (int i = 0; i < sumColumnNames.Count; ++i)
                            {
                                decimal? d = ConvertHelper.ToDecimal(row.Cells[sumColumnNames[i]].Value);
                                sum[i] += d.HasValue ? d.Value : 0;
                            }
                            for (int i = 0; i < countColumnNames.Count; ++i)
                            {
                                count[i] += 1;
                            }
                            noneSelect = false;
                        }
                        else
                        {
                            allSelect = false;
                        }
                    }
                    for (int i = 0; i < sumColumnNames.Count; ++i)
                    {
                        valueRow.Cells[sumColumnNames[i]].Value = sum[i];
                    }
                    for (int i = 0; i < countColumnNames.Count; ++i)
                    {
                        valueRow.Cells[countColumnNames[i]].Value = count[i];
                    }

                    if (allSelect)
                    {
                        valueRow.Cells[m_selectColumnName].Value = true;
                    }
                    else if (noneSelect)
                    {
                        valueRow.Cells[m_selectColumnName].Value = false;
                    }
                    else
                    {
                        valueRow.Cells[m_selectColumnName].Value = null;
                    }
                }
            }
        }

        private void ColumnManageRowSelectionCell_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_checkColumn.ReadOnly)
            {
                return;
            }

            Cell cell = (Cell)sender;
            if (e.Button == MouseButtons.Right && !m_checkColumn.ReadOnly)
            {
                // DetailGrid只是从DetailGridTemplate复制，Constructor里的grid只是模版
                DetailGrid detailGrid = ((Xceed.Grid.ColumnManagerCell)sender).ParentGrid;
                if (detailGrid is MyDetailGrid)
                {
                    m_grid = detailGrid as MyDetailGrid;
                }

                this.tsm当前组全选.Visible = (m_grid.DetailGridTemplates.Count > 0) || (m_grid.Groups.Count > 0);
                this.tsm当前组全不选.Visible = this.tsm当前组全选.Visible;

                contextMenuStrip1.Show(cell.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        private void SuspendBatch()
        {
            m_isBatchSetting = true;
        }
        private void ResumeBatch()
        {
            m_isBatchSetting = false;
            CheckColumn_ValueChanged(m_grid.DataRowTemplate.Cells[m_selectColumnName], System.EventArgs.Empty);
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuspendBatch();

            foreach (Xceed.Grid.DataRow row in m_grid.DataRows)
            {
                SetCheck(row, true, true);
            }

            ResumeBatch();
        }

        private void 全不选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuspendBatch();

            foreach (Xceed.Grid.DataRow row in m_grid.DataRows)
            {
                SetCheck(row, true, false);
            }

            ResumeBatch();
        }

        private void 当前组全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_grid.CurrentRow == null)
                return;

            SuspendBatch();

            IEnumerable rows = m_grid.CurrentRow.ParentGroup.GetSortedDataRows(false);
            foreach (Xceed.Grid.DataRow row in rows)
            {
                SetCheck(row, true, true);
            }

            ResumeBatch();
        }

        private void 选择项选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuspendBatch();

            foreach (Xceed.Grid.DataRow row in m_grid.GridControl.SelectedRows)
            {
                if (IsDataRowIncluded(row, m_grid))
                {
                    SetCheck(row, true);
                }
            }

            ResumeBatch();
        }

        void tsm选择项全不选_Click(object sender, System.EventArgs e)
        {
            SuspendBatch();

            foreach (Xceed.Grid.DataRow row in m_grid.GridControl.SelectedRows)
            {
                if (IsDataRowIncluded(row, m_grid))
                {
                    SetCheck(row, false);
                }
            }

            ResumeBatch();
        }

        void tsm当前组全不选_Click(object sender, System.EventArgs e)
        {
            if (m_grid.CurrentRow == null)
                return;

            SuspendBatch();

            IEnumerable rows = m_grid.CurrentRow.ParentGroup.GetSortedDataRows(false);
            foreach (Xceed.Grid.DataRow row in rows)
            {
                SetCheck(row, true, false);
            }

            ResumeBatch();
        }

        private void SetCheck(Xceed.Grid.DataRow row, bool includeDetail, bool value)
        {
            SetCheck(row, value);

            if (includeDetail)
            {
                foreach (DetailGrid detailGrid in row.DetailGrids)
                {
                    if (detailGrid.Collapsed)
                    {
                        // load detailGrid
                        // Todo: now it's async, so .....
                        //detailGrid.Collapsed = false;
                    }
                    foreach (Xceed.Grid.DataRow subRow in detailGrid.DataRows)
                    {
                        if (detailGrid.Columns[m_selectColumnName] != null)
                        {
                            SetCheck(subRow, includeDetail, value);
                        }
                    }
                }
            }
        }

        private void SetCheck(Xceed.Grid.DataRow row, bool value)
        {
            if (row.Cells[m_selectColumnName].ReadOnly)
            {
                return;
            }
            if (row.Cells[m_selectColumnName].IsBeingEdited)
            {
                row.Cells[m_selectColumnName].LeaveEdit(true);
            }
            row.Cells[m_selectColumnName].Value = value;
        }

        private bool IsDataRowIncluded(Xceed.Grid.DataRow selectedRow, IGrid grid)
        {
            foreach (Xceed.Grid.DataRow row in grid.DataRows)
            {
                if (row == selectedRow)
                    return true;

                foreach (MyDetailGrid detailGrid in row.DetailGrids)
                {
                    if (IsDataRowIncluded(selectedRow, detailGrid))
                        return true;
                }
            }
            return false;
        }
    }
}