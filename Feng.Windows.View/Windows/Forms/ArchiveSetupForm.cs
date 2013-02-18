using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ArchiveSetupForm : PositionPersistForm
    {
        /// <summary>
        /// 
        /// </summary>
        private ArchiveSetupForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="masterGrid"></param>
        /// <param name="dmMaster"></param>
        public ArchiveSetupForm(IGrid masterGrid, IDisplayManager dmMaster)
        {
            InitializeComponent();

            m_masterGrid = masterGrid;
            m_dmMaster = dmMaster;

            if (m_masterGrid == null)
            {
                this.myTabControl1.TabPages.Remove(this.tabPage1);
            }
            if (m_dmMaster == null)
            {
                this.myTabControl1.TabPages.Remove(this.tabPage2);
            }
        }

        private IGrid m_masterGrid;
        private IDisplayManager m_dmMaster;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            base.Form_Load(sender, e);

            LoadGridInfos();

            LoadSearchControlInfos();
        }

        private void LoadGridInfos()
        {
            LoadGridInfos(grdGridColumns, m_masterGrid);
        }

        private void LoadSearchControlInfos()
        {
            LoadSearchControlInfos(grdSearchControls, m_dmMaster);
        }

        private void SaveGridInfos()
        {
            SaveGridInfos(grdGridColumns, m_masterGrid);
        }

        private void SaveSearchControlInfos()
        {
            SaveSearchControlInfos(this.grdSearchControls, m_dmMaster);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveGridInfos();
            SaveSearchControlInfos();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnResetGrid_Click(object sender, EventArgs e)
        {
            ResetGridInfos(grdGridColumns, m_masterGrid);

            ResetSearchControlInfos(grdSearchControls, m_dmMaster);

            this.Close();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (myTabControl1.SelectedTab == this.tabPage1)
            {
                MoveRow(grdGridColumns, true);
            }
            else if (myTabControl1.SelectedTab == this.tabPage2)
            {
                MoveRow(grdSearchControls, true);
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (myTabControl1.SelectedTab == this.tabPage1)
            {
                MoveRow(grdGridColumns, false);
            }
            else if (myTabControl1.SelectedTab == this.tabPage2)
            {
                MoveRow(grdSearchControls, false);
            }
        }

        #region "Internal Funcs"
        internal static void ResetGridInfos(MyGrid grdSetup, IGrid masterGrid)
        {
            if (masterGrid == null)
                return;

            masterGrid.LoadGridDefaultLayout();
            masterGrid.AutoAdjustColumnWidth();

            //LoadGridInfos(grdSetup, masterGrid);
        }

        internal static void LoadGridInfos(MyGrid grdSetup, IGrid masterGrid)
        {
            if (masterGrid == null)
                return;

            if (grdSetup.Columns.Count == 0)
            {
                grdSetup.Columns.Add(new Xceed.Grid.Column("名称", typeof(string)));
                grdSetup.Columns.Add(new Xceed.Grid.Column("是否显示", typeof(bool)));
                grdSetup.ReadOnly = false;
                grdSetup.Columns["是否显示"].ReadOnly = false;
                grdSetup.Columns["名称"].ReadOnly = true;
            }

            grdSetup.DataRows.Clear();

            if (masterGrid != null)
            {
                Dictionary<string, GridColumnInfo> visibleColumns = new Dictionary<string, GridColumnInfo>();
                IList<GridColumnInfo> gridColumnInfos = ADInfoBll.Instance.GetGridColumnInfos(masterGrid.GridName);
                bool hasInfo = gridColumnInfos.Count > 0;

                foreach (GridColumnInfo info in gridColumnInfos)
                {
                    if (!Authority.AuthorizeByRule(info.ColumnVisible))
                    {
                        continue;
                    }
                    visibleColumns[info.GridColumnName] = info;
                }

                SortedList<int, Xceed.Grid.Column> columns = new SortedList<int, Xceed.Grid.Column>();
                foreach (Xceed.Grid.Column column in masterGrid.Columns)
                {
                    if (hasInfo && !visibleColumns.ContainsKey(column.FieldName))
                    {
                        continue;
                    }

                    columns.Add(column.VisibleIndex, column);
                }

                foreach (KeyValuePair<int, Xceed.Grid.Column> kvp in columns)
                {
                    Xceed.Grid.DataRow row = grdSetup.DataRows.AddNew();
                    row.Cells["是否显示"].Value = kvp.Value.Visible;
                    row.Cells["名称"].Value = kvp.Value.Title;
                    row.EndEdit();

                    if (visibleColumns.ContainsKey(kvp.Value.FieldName))
                    {
                        row.ReadOnly = Authority.AuthorizeByRule(visibleColumns[kvp.Value.FieldName].NotNull);
                    }
                }
            }
        }

        internal static void SaveGridInfos(MyGrid grdSetup, IGrid masterGrid)
        {
            if (masterGrid == null)
                return;

            foreach (Xceed.Grid.DataRow row in grdSetup.DataRows)
            {
                foreach (Xceed.Grid.Column column in masterGrid.Columns)
                {
                    if (row.Cells["名称"].Value.ToString() == column.Title)
                    {
                        column.Visible = Convert.ToBoolean(row.Cells["是否显示"].Value);
                        column.VisibleIndex = row.Index;
                        break;
                    }
                }
            }
        }

        internal static void ResetSearchControlInfos(MyGrid grdSetup, IDisplayManager dmMaster)
        {
            if (dmMaster == null)
                return;

            if (dmMaster.SearchManager != null)
            {
                int n = 0;
                foreach (ISearchControl sc in dmMaster.SearchManager.SearchControls)
                {
                    GridColumnInfo info = sc.Tag as GridColumnInfo;

                    if (info == null || (!string.IsNullOrEmpty(info.SearchControlType)
                        && Authority.AuthorizeByRule(info.SearchControlVisible)))
                    {
                        sc.Available = true;
                    }
                    else
                    {
                        sc.Available = false;
                    }

                    sc.Index = n;
                    n++;
                }

                //LoadSearchControlInfos(grdSetup, dmMaster);
            }
        }

        internal static void LoadSearchControlInfos(MyGrid grdSetup, IDisplayManager dmMaster)
        {
            if (dmMaster == null)
                return;

            if (grdSetup.Columns.Count == 0)
            {
                grdSetup.Columns.Add(new Xceed.Grid.Column("名称", typeof(string)));
                grdSetup.Columns.Add(new Xceed.Grid.Column("是否显示", typeof(bool)));
                grdSetup.ReadOnly = false;
                grdSetup.Columns["是否显示"].ReadOnly = false;
                grdSetup.Columns["名称"].ReadOnly = true;
            }

            grdSetup.DataRows.Clear();

            ISearchManager sm = dmMaster.SearchManager;
            if (sm != null)
            {
                SortedList<int, ISearchControl> scc1 = new SortedList<int, ISearchControl>();
                SortedList<int, ISearchControl> scc2 = new SortedList<int, ISearchControl>();

                foreach (ISearchControl sc in sm.SearchControls)
                {
                    GridColumnInfo info = sc.Tag as GridColumnInfo;
                    if (info == null || (!string.IsNullOrEmpty(info.SearchControlType) &&
                         Authority.AuthorizeByRule(info.SearchControlVisible)))
                    {
                        if (sc.Available)
                        {
                            scc1.Add(sc.Index, sc);
                        }
                        else
                        {
                            scc2.Add(sc.Index, sc);
                        }
                    }
                }

                foreach (KeyValuePair<int, ISearchControl> kvp in scc1)
                {
                    Xceed.Grid.DataRow row = grdSetup.DataRows.AddNew();
                    row.Cells["是否显示"].Value = kvp.Value.Available;
                    row.Cells["名称"].Value = kvp.Value.Caption;
                    row.EndEdit();
                }
                foreach (KeyValuePair<int, ISearchControl> kvp in scc2)
                {
                    Xceed.Grid.DataRow row = grdSetup.DataRows.AddNew();
                    row.Cells["是否显示"].Value = kvp.Value.Available;
                    row.Cells["名称"].Value = kvp.Value.Caption;
                    row.EndEdit();
                }
            }
        }

        internal static void SaveSearchControlInfos(MyGrid grdSetup, IDisplayManager dmMaster)
        {
            if (dmMaster == null)
                return;

            foreach (Xceed.Grid.DataRow row in grdSetup.DataRows)
            {
                ISearchManager fmc = dmMaster.SearchManager;
                if (fmc != null)
                {
                    foreach (ISearchControl sc in fmc.SearchControls)
                    {
                        if (row.Cells["名称"].Value.ToString() == sc.Caption)
                        {
                            sc.Available = Convert.ToBoolean(row.Cells["是否显示"].Value);
                            sc.Index = row.Index;
                            break;
                        }
                    }
                }
            }
        }

        internal static void MoveRow(MyGrid grid, bool up)
        {
            Xceed.Grid.DataRow row = grid.CurrentRow as Xceed.Grid.DataRow;
            if (row != null)
            {
                int n = row.Index;
                if (up)
                {
                    if (n - 1 >= 0)
                    {
                        SwapRow(grid, grid.DataRows[n - 1], row);
                        grid.CurrentRow = grid.DataRows[n - 1];
                    }
                }
                else
                {
                    if (n  + 1 < grid.DataRows.Count)
                    {
                        SwapRow(grid, grid.DataRows[n + 1], row);
                        grid.CurrentRow = grid.DataRows[n + 1];
                    }
                }
            }
        }

        internal static void SwapRow(MyGrid grid, Xceed.Grid.CellRow row1, Xceed.Grid.CellRow row2)
        {
            foreach (Xceed.Grid.Column c in grid.Columns)
            {
                object t = row1.Cells[c.Index].Value;
                row1.Cells[c.Index].Value = row2.Cells[c.Index].Value;
                row2.Cells[c.Index].Value = t;
            }
        }
        #endregion

        //private void SetControlVisible(Control parent)
        //{
        //    parent.Visible = true;
        //    foreach (Control child in parent.Controls)
        //    {
        //        SetControlVisible(child);
        //    }
        //}
    }
}