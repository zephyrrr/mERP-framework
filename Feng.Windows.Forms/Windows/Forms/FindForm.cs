using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xceed.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FindForm : MyForm
    {
        /// <summary>
        /// 
        /// </summary>
        public FindForm()
        {
            InitializeComponent();

            this.TopLevel = true;
        }

        /// <summary>
        /// Instance
        /// </summary>
        public static FindForm Instance
        {
            get
            {
                if (s_instance == null || s_instance.IsDisposed)
                {
                    s_instance = new FindForm();
                }
                return s_instance;
            }
        }

        private static FindForm s_instance;

        private GridControl m_grid;

        /// <summary>
        /// 
        /// </summary>
        public GridControl ToFindGrid
        {
            get { return m_grid; }
            set { m_grid = value; }
        }

        private bool CheckCellValue(Cell cell)
        {
            if (!cell.Visible)
            {
                return false;
            }

            if (cell.Value != null)
            {
                string s = cell.GetDisplayText();
                if (ckbCaseSensitive.Checked)
                {
                    if (s.IndexOf(txtContent.Text, StringComparison.CurrentCulture) != -1)
                    {
                        return true;
                    }
                }
                else
                {
                    if (s.IndexOf(txtContent.Text, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckDetailGridAsc(Xceed.Grid.DataRow row)
        {
            foreach (DetailGrid detailGrid in row.DetailGrids)
            {
                for (int i = 0; i < detailGrid.DataRows.Count; ++i)
                {
                    for (int j = 0; j < detailGrid.Columns.Count; ++j)
                    {
                        if (CheckCellValue(detailGrid.DataRows[i].Cells[j]))
                        {
                            SetCurrentCell(detailGrid.DataRows[i].Cells[j]);
                            return true;
                        }
                    }

                    if (CheckDetailGridAsc(detailGrid.DataRows[i]))
                        return true;
                }
            }
            return false;
        }

        private bool CheckDetailGridDesc(Xceed.Grid.DataRow row)
        {
            foreach (DetailGrid detailGrid in row.DetailGrids)
            {
                for (int i = detailGrid.DataRows.Count - 1; i >= 0; --i)
                {
                    if (CheckDetailGridDesc(detailGrid.DataRows[i]))
                        return true;

                    for (int j = detailGrid.Columns.Count - 1; j >= 0; --j)
                    {
                        if (CheckCellValue(detailGrid.DataRows[i].Cells[j]))
                        {
                            SetCurrentCell(detailGrid.DataRows[i].Cells[j]);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void SetCurrentCell(Xceed.Grid.Cell currentCell)
        {
            m_grid.CurrentCell = currentCell;
            m_grid.CurrentCell.BackColor = ColorSettings.Setting.LocatedRow;
            m_grid.CurrentCell.BringIntoView();
        }

        private bool CheckGridAsc(DetailGrid currentGrid, int startx, int starty)
        {
            for (int j = starty; j < currentGrid.Columns.Count; ++j)
            {
                if (CheckCellValue(currentGrid.DataRows[startx].Cells[j]))
                {
                    SetCurrentCell(currentGrid.DataRows[startx].Cells[j]);
                    return true;
                }
                if (CheckDetailGridAsc(currentGrid.DataRows[startx]))
                    return true;
            }

            // 只有当前行搜索DetailGrid
            //if (CheckDetailGridAsc(currentGrid.DataRows[startx]))
            //    return true;

            //Xceed.Grid.Collections.ReadOnlyDataRowList list = m_grid.GetSortedDataRows(false);

            for (int i = startx + 1; i < currentGrid.DataRows.Count; ++i)
            {
                for (int j = 0; j < currentGrid.Columns.Count; ++j)
                {
                    if (CheckCellValue(currentGrid.DataRows[i].Cells[j]))
                    {
                        SetCurrentCell(currentGrid.DataRows[i].Cells[j]);
                        return true;
                    }
                }
                if (CheckDetailGridAsc(currentGrid.DataRows[i]))
                    return true;
            }

            if (currentGrid.ParentGrid != null)
            {
                int newx = currentGrid.ParentDataRow.Index + 1;
                int newy = 0;
                if (newx < currentGrid.ParentDataRow.ParentGrid.DataRows.Count)
                {
                    if (CheckGridAsc(currentGrid.ParentGrid, newx, newy))
                        return true;
                }
            }

            return false;
        }

        private bool CheckGridDesc(DetailGrid currentGrid, int startx, int starty)
        {
            for (int j = starty ; j >= 0; --j)
            {
                if (CheckDetailGridDesc(currentGrid.DataRows[startx]))
                    return true;

                if (CheckCellValue(currentGrid.DataRows[startx].Cells[j]))
                {
                    SetCurrentCell(currentGrid.DataRows[startx].Cells[j]);
                    return true;
                }
            }

            //if (CheckDetailGridDesc(currentGrid.DataRows[startx]))
            //    return true;

            for (int i = startx - 1; i >= 0; --i)
            {
                if (CheckDetailGridDesc(currentGrid.DataRows[i]))
                    return true;

                for (int j = currentGrid.Columns.Count - 1; j >= 0; --j)
                {
                    if (CheckCellValue(currentGrid.DataRows[i].Cells[j]))
                    {
                        SetCurrentCell(currentGrid.DataRows[i].Cells[j]);
                        return true;
                    }
                }
            }

            if (currentGrid.ParentGrid != null)
            {
                int newx = currentGrid.ParentDataRow.Index - 1;
                int newy = currentGrid.ParentDataRow.ParentGrid.Columns.Count - 1;
                if (newx >= 0)
                {
                    if (CheckGridDesc(currentGrid.ParentGrid, newx, newy))
                        return true;
                }
            }

            return false;
        }

        private bool FindCell(Xceed.Grid.Cell currentCell)
        {
            if (m_grid.DataRows.Count == 0)
            {
                return false;
            }

            int startx, starty;
            
            DetailGrid currentGrid;
            if (currentCell == null)
            {
                startx = 0;
                starty = 0;
                currentGrid = m_grid.DataRows[0].ParentGrid;
            }
            else
            {
                startx = (currentCell.ParentRow as Xceed.Grid.DataRow).Index;
                starty = !rbDirectionUp.Checked ? currentCell.ParentColumn.Index + 1 : currentCell.ParentColumn.Index - 1;
                currentGrid = currentCell.ParentColumn.ParentGrid;
            }

            if (!rbDirectionUp.Checked)
            {
                bool ret = CheckGridAsc(currentGrid, startx, starty);
                if (!ret)
                {
                    return CheckGridAsc(currentGrid, 0, 0);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                bool ret = CheckGridDesc(currentGrid, startx, starty);
                if (!ret)
                {
                    return CheckGridDesc(currentGrid, currentGrid.DataRows.Count - 1, currentGrid.Columns.Count - 1);
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Canceled;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Finded;


        private void OnFinded()
        {
            if (Finded != null)
            {
                Finded(this, System.EventArgs.Empty);
            }
        }

        private void OnCanceled()
        {
            if (Canceled != null)
            {
                Canceled(this, System.EventArgs.Empty);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtContent.Text))
                return;

            if (m_grid == null || !m_grid.Visible)
            {
                return;
            }

            if (m_grid.CurrentCell != null)
            {
                m_grid.CurrentCell.ResetBackColor();
            }

            bool ret = FindCell(m_grid.CurrentCell);

            if (!ret)
            {
                ServiceProvider.GetService<IMessageBox>().ShowInfo("找不到" + txtContent.Text + "!");
            }

            OnFinded();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (m_grid != null && m_grid.Visible && m_grid.CurrentCell != null)
            {
                m_grid.CurrentCell.ResetBackColor();
            }

            this.Hide();

            OnCanceled();
        }

        private void txtContent_TextChanged(object sender, EventArgs e)
        {
            btnFind.Enabled = (txtContent.Text.Length != 0);
        }

        private void txtContent_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) System.Windows.Forms.Keys.Enter)
            {
                btnFind_Click(btnFind, System.EventArgs.Empty);
            }
        }

        private void FindForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                OnCanceled();
            }
        }
    }
}