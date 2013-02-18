using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Xceed.Grid;
using Feng.Windows.Forms;
using Feng.Windows.Utils;
using Feng.Grid.Search;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BoundGridHelper : IDisposable
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
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridControl"></param>
        public BoundGridHelper(IBoundGrid gridControl)
        {
            InitializeComponent();

            m_grid = gridControl;

            m_masterGrid = m_grid as MyGrid;
            if (m_masterGrid == null)
            {
                tsmFind.Visible = false;
                //tsmExportExcel.Visible = false;
                tsmPrintPreview.Visible = false;
                tsmGenerateReport.Visible = false;
                toolStripSeparator1.Visible = false;
                //tsmGroup.Visible = false;

                tsmLoadLayout.Visible = false;
                tsmSaveLayout.Visible = false;
                tsmPresetLayout.Visible = false;
            }

            this.tsmResetColumns.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReset.png").Reference;
            this.tsmInvibleColumn.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconInvisible.png").Reference;
            this.tsmAutoAdjustWidth.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconAdjust.png").Reference;
            this.tsmPresetLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPreset.png").Reference;
            this.tsmLoadLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconOpen.png").Reference;
            this.tsmSaveLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSave.png").Reference;
            this.tsmFilter.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFilter.png").Reference;
            this.tsmGroup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconGroup.png").Reference;
            this.tsmResetColumns.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReset.png").Reference;
            this.tsmFind.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFind.png").Reference;
            this.tsmExportExcel.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExportExcel.png").Reference;
            this.tsmPrintPreview.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPrint.png").Reference;
            this.tsmGenerateReport.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReport.png").Reference;
            this.tsmSetup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSetup.png").Reference;

            this.tsmRefresh.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRefresh.png").Reference;
            //this.tsmRefresh.Visible = false;

            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(m_grid.GridName);
            if (Authority.AuthorizeByRule(gridInfo.AllowInnerMenu) || Authority.IsDeveloper())
            {
                m_grid.GridHelper.MergeContenxtMenuStripForCell(this.contextMenuStrip1);
            }
        }

        internal void Initialize()
        {
            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(m_grid.GridName);
            if (Authority.AuthorizeByRule(gridInfo.AllowInnerSearch))
            {
                m_grid.SetSearchRowVisible(true);
            }
            if (Authority.AuthorizeByRule(gridInfo.AllowInnerFilter))
            {
                m_grid.SetFilterRowVisible(true);
            }
            if (Authority.AuthorizeByRule(gridInfo.AllowInnerTextFilter))
            {
                m_grid.SetTextFilterRowVisible(true);
            }

            m_grid.GridHelper.ContextMenuStripForCell.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStripForCell_Opening);
        }

        private IBoundGrid m_grid;
        private readonly MyGrid m_masterGrid;

        /// <summary>
        /// 创建ColumnManageRow的Tips
        /// </summary>
        internal void CreateColumnManageRowEvent()
        {
            Xceed.Grid.ColumnManagerRow columnManageRow = m_grid.GetColumnManageRow();
            if (columnManageRow != null)
            {
                foreach (Xceed.Grid.Column column in m_grid.Columns)
                {
                    if (!(column is Columns.CheckColumn))
                    {
                        columnManageRow.Cells[column.FieldName].MouseEnter += new EventHandler(ColumnManageCell_MouseEnter);
                        columnManageRow.Cells[column.FieldName].MouseDown += new MouseEventHandler(ColumnManageCell_MouseDown);
                    }
                }
            }
        }

        /// <summary>
        /// Occured in ColumnManageRow's Cell MouseEnter
        /// Now is for Help Msg show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void ColumnManageCell_MouseEnter(object sender, EventArgs e)
        {
            m_grid.GridHelper.GridToolTip.RemoveAll();

            Cell cell = sender as Cell;
            GridColumnInfo info = cell.ParentColumn.Tag as GridColumnInfo;
            if (info != null && !string.IsNullOrEmpty(info.Help))
            {
                m_grid.GridHelper.GridToolTip.SetToolTip(cell.GridControl, info.Help);
            }
        }

        private Cell m_contextMenuManagerColumnCell;
        private void ColumnManageCell_MouseDown(object sender, MouseEventArgs e)
        {
            Cell cell = (Cell)sender;
            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(m_grid.GridName);
            if (!Authority.AuthorizeByRule(gridInfo.AllowInnerMenu) && !Authority.IsDeveloper())
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                // DetailGrid只是从DetailGridTemplate复制，Constructor里的grid只是模版
                DetailGrid detailGrid = ((Xceed.Grid.ColumnManagerCell)sender).ParentGrid;
                if (detailGrid is DataBoundDetailGrid)
                {
                    m_grid = detailGrid as DataBoundDetailGrid;
                }

                m_contextMenuManagerColumnCell = cell;
                contextMenuStrip2.Show(cell.PointToScreen(new System.Drawing.Point(e.X, e.Y)));
            }
            else
            {
                m_contextMenuManagerColumnCell = null;
            }
        }

        private void tsmInvibleColumn_Click(object sender, System.EventArgs e)
        {
            if (m_contextMenuManagerColumnCell != null)
            {
                m_contextMenuManagerColumnCell.ParentColumn.Visible = false;
            }
        }

        private void tsmSetup_Click(object sender, System.EventArgs e)
        {
            if (m_contextMenuManagerColumnCell != null)
            {
                using (ArchiveSetupForm form = new ArchiveSetupForm(m_grid, null))
                {
                    form.ShowDialog(m_contextMenuManagerColumnCell.GridControl.FindForm());
                }
            }
        }

        private void tsmAutoAdjustWidth_Click(object sender, EventArgs e)
        {
            if (m_contextMenuManagerColumnCell != null)
            {
                GridLayoutExtention.AutoAdjustColumnWidth(m_contextMenuManagerColumnCell.ParentGrid.Columns, m_contextMenuManagerColumnCell.ParentGrid.GridControl.Width);
            }
        }

        void ContextMenuStripForCell_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(m_grid.GridName);
            if (!Authority.AuthorizeByRule(gridInfo.AllowInnerMenu) && !Authority.IsDeveloper())
            {
                e.Cancel = true;
                return;
            }
            if (m_grid.GridHelper.ContextCell != null)
            {
                GridColumnInfo columnInfo = m_grid.GridHelper.ContextCell.ParentColumn.Tag as GridColumnInfo;

                if (columnInfo.EnableSelectAll.HasValue)
                {
                    tsmSelectAll.Visible = columnInfo.EnableSelectAll.Value;
                }
                if (columnInfo.EnableCopy.HasValue)
                {
                    m_grid.GridHelper.ContextMenuStripForCell.Items["tsmCopy"].Visible = columnInfo.EnableCopy.Value;
                }
            }
        }

        // 表格配置用.xmlg，搜索窗口配置用.xmls
        void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Xceed.Grid.Cell cell = m_contextMenuManagerColumnCell;
            if (cell == null)
                return;
            GridColumnInfo columnInfo = cell.ParentColumn.Tag as GridColumnInfo;
            if (columnInfo.EnableCopy.HasValue)
            {
                tsmCopyColumn.Visible = columnInfo.EnableCopy.Value;
            }

            GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(m_grid.GridName);
            if (!Authority.AuthorizeByRule(gridInfo.AllowInnerFilter))
            {
                tsmFilter.Visible = false;
            }
            else
            {
                tsmFilter.Checked = m_grid.GetFilterRowVisible();
            }

            tsmGroup.Checked = (m_grid.GetGroupByRow() != null);

            if (m_masterGrid != null
                && tsmPresetLayout.DropDownItems.Count == 0)
            {
                string[] folders = new string[] { m_masterGrid.GetGridDefaultDataDirectory(), m_masterGrid.GetGridGlobalDataDirectory() };

                foreach (string folder in folders)
                {
                    if (!System.IO.Directory.Exists(folder))
                    {
                        continue;
                    }
                    foreach (string fileName in System.IO.Directory.GetFiles(folder, "*.xmlg"))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = System.IO.Path.GetFileName(fileName).Replace(".xmlg", "");
                        item.Tag = fileName;
                        item.Click += new EventHandler(tsmPresetSubitem_Click);
                        tsmPresetLayout.DropDownItems.Add(item);
                    }
                }

                if (tsmPresetLayout.DropDownItems.Count == 0)
                {
                    tsmPresetLayout.Visible = false;
                }
            }
        }
        private void tsmPresetSubitem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string fileName = item.Tag.ToString();
            m_masterGrid.LoadLayout(fileName);
        }

        private void tsmFilter_Click(object sender, EventArgs e)
        {
            tsmFilter.Checked = !tsmFilter.Checked;
            m_grid.SetFilterRowVisible(tsmFilter.Checked);
        }

        private void tsmGroup_Click(object sender, EventArgs e)
        {
            tsmGroup.Checked = !tsmGroup.Checked;
            if (tsmGroup.Checked)
            {
                m_grid.FixedHeaderRows.Insert(0, new BoundGridGroupByRow());
            }
            else
            {
                MyGroupByRow row = m_grid.GetGroupByRow();
                if (row != null)
                {
                    m_grid.FixedHeaderRows.Remove(row);
                }
            }
        }

        private void tsmFind_Click(object sender, EventArgs e)
        {
            Feng.Windows.Forms.FindForm.Instance.Show();
            Feng.Windows.Forms.FindForm.Instance.ToFindGrid = m_masterGrid;
        }

        private void tsmExportExcel_Click(object sender, EventArgs e)
        {
            MyGrid.ExportToExcelCommand.Execute(this.m_grid, ExecutedEventArgs.Empty);
        }

        private void tsmPrintPreview_Click(object sender, EventArgs e)
        {
            m_masterGrid.PrintPriviewGrid();
        }
        private void tsmGenerateReport_Click(object sender, EventArgs e)
        {
            m_masterGrid.GenerateReport();
        }
        private void tsmLoadLayout_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = m_masterGrid.GetGridDefaultDataDirectory();
            openFileDialog1.Filter = "配置文件 (*.xmlg)|*.xmlg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_masterGrid.LoadLayout(openFileDialog1.FileName);
            }
            openFileDialog1.Dispose();
        }

        private void tsmSaveLayout_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = m_masterGrid.GetGridDefaultDataDirectory();
            saveFileDialog1.Filter = "配置文件 (*.xmlg)|*.xmlg";
            //saveFileDialog1.Title = "保存";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_masterGrid.SaveLayout(saveFileDialog1.FileName);
            }
            saveFileDialog1.Dispose();
        }

        private void tsmCopyColumn_Click(object sender, EventArgs e)
        {
            if (m_contextMenuManagerColumnCell != null)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (Xceed.Grid.DataRow row in m_contextMenuManagerColumnCell.ParentGrid.DataRows)
                    {
                        if (!row.Visible)
                            continue;
                        sb.Append(row.Cells[m_contextMenuManagerColumnCell.ParentColumn.FieldName].GetDisplayText().Replace(System.Environment.NewLine, " "));
                        sb.Append(System.Environment.NewLine);
                    }
                    ClipboardHelper.CopyTextToClipboard(sb.ToString());
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        private void tsmResetColumns_Click(object sender, EventArgs e)
        {
            m_grid.LoadGridDefaultLayout();
            m_grid.AutoAdjustColumnWidth();
        }

        private void tsmRefresh_Click(object sender, EventArgs e)
        {
            m_grid.ReloadData();
        }

        void tsmSelectAll_Click(object sender, System.EventArgs e)
        {
            Xceed.Grid.Row contextRow = m_grid.GridHelper.ContextRow;
            if (contextRow != null)
            {
                contextRow.GridControl.SelectedRows.Clear();
                foreach (Xceed.Grid.DataRow row in contextRow.ParentGroup.GetSortedDataRows(false))
                {
                    if (row.Visible)
                    {
                        m_grid.GridControl.SelectedRows.Add(row);
                    }
                }
            }
        }

        private SearchRow m_searchRow;
        /// <summary>
        /// 查找行
        /// </summary>
        public SearchRow SearchRow
        {
            get
            {
                return m_searchRow;
            }
        }

        /// <summary>
        /// 查找行可见属性
        /// </summary>
        public bool SearchRowVisible
        {
            get
            {
                if (m_searchRow == null)
                {
                    return false;
                }
                else
                {
                    return m_searchRow.Visible;
                }
            }
            set
            {
                if (!value)
                {
                    if (m_searchRow != null)
                    {
                        m_searchRow.Visible = false;
                    }
                }
                else
                {
                    if (m_searchRow == null)
                    {
                        m_searchRow = new SearchRow();
                        m_searchRow.Height = 24;

                        SpacerRow spacerRow = new SpacerRow();
                        spacerRow.Height = 4;
                        m_grid.FixedFooterRows.Add(spacerRow);

                        m_grid.FixedFooterRows.Add(m_searchRow);
                    }
                    m_searchRow.Visible = true;
                    //m_searchRow.FillFilters();
                }
            }
        }
    }
}
