using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Feng.Windows.Forms;
using Xceed.Grid;
using Feng.Grid.Filter;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GridHelper : IDisposable
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (m_filterRow != null)
                {
                    m_filterRow = null;
                }
                m_toolTip.Dispose();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridControl"></param>
        public GridHelper(IGrid gridControl)
        {
            InitializeComponent();

            m_grid = gridControl;
        }

        private readonly IGrid m_grid;

        #region Filter
        private FilterRow m_filterRow;
        /// <summary>
        /// 筛选行
        /// </summary>
        public FilterRow FilterRow
        {
            get
            {
                return m_filterRow;
            }
        }

        /// <summary>
        /// 筛选行可见属性
        /// </summary>
        public bool FilterRowVisible
        {
            get
            {
                if (m_filterRow == null)
                {
                    return false;
                }
                else
                {
                    return m_filterRow.Visible;
                }
            }
            set
            {
                if (!value)
                {
                    if (m_filterRow != null)
                    {
                        m_filterRow.Visible = false;
                    }
                }
                else
                {
                    if (m_filterRow == null)
                    {
                        m_filterRow = new FilterRow();
                        m_filterRow.Height = 24;

                        m_grid.FixedHeaderRows.Insert(0, m_filterRow);
                        SpacerRow spacerRow = new SpacerRow();
                        spacerRow.Height = 4;
                        m_grid.FixedHeaderRows.Insert(1, spacerRow);
                    }
                    m_filterRow.Visible = true;
                    m_filterRow.FillFilters();
                }
            }
        }


        private Feng.Grid.TextFilter.FilterRow m_textFilterRow;
        /// <summary>
        /// 筛选行
        /// </summary>
        public Feng.Grid.TextFilter.FilterRow TextFilterRow
        {
            get
            {
                return m_textFilterRow;
            }
        }

        /// <summary>
        /// 筛选行可见属性
        /// </summary>
        public bool TextFilterRowVisible
        {
            get
            {
                if (m_textFilterRow == null)
                {
                    return false;
                }
                else
                {
                    return m_textFilterRow.Visible;
                }
            }
            set
            {
                if (!value)
                {
                    if (m_textFilterRow != null)
                    {
                        m_textFilterRow.Visible = false;
                    }
                }
                else
                {
                    if (m_textFilterRow == null)
                    {
                        if (this.m_grid.Columns[Feng.Grid.TextFilter.FilterRow.TextFilterSelectColumnName] == null)
                        {
                            this.m_grid.Columns.Add(new Column(Feng.Grid.TextFilter.FilterRow.TextFilterSelectColumnName, typeof(bool)));
                            this.m_grid.Columns[Feng.Grid.TextFilter.FilterRow.TextFilterSelectColumnName].Visible = false;
                            //this.m_grid.Columns[Feng.Grid.TextFilter.FilterRow.TextFilterSelectColumnName].SortDirection = SortDirection.Ascending;
                        }

                        m_textFilterRow = new Feng.Grid.TextFilter.FilterRow();
                        m_textFilterRow.Height = 24;
                        SpacerRow spacerRow = new SpacerRow();
                        spacerRow.Height = 4;
                        m_grid.FixedFooterRows.Add(spacerRow);
                        m_grid.FixedFooterRows.Add(m_textFilterRow);
                    }
                    m_textFilterRow.Visible = true;
                    //m_textFilterRow.FillFilters();
                }
            }
        }
        #endregion
        

        //private static Dictionary<string, bool> m_groupCollapse;
        //internal static Dictionary<string, bool> GroupCollapseState
        //{
        //    get { return m_groupCollapse; }
        //}

        ///// <summary>
        ///// 清空Group Collapsed状态
        ///// </summary>
        //private static void ClearGroupCollapsedState()
        //{
        //    if (m_groupCollapse != null)
        //    {
        //        m_groupCollapse.Clear();
        //    }
        //}

        internal void group_CollapsedChanged(object sender, EventArgs e)
        {
            //Group group = sender as Group;
            //IGrid grid = group.GridControl as IGrid;

            //if (group.Key != null)
            //{
            //    if (m_groupCollapse == null)
            //    {
            //        m_groupCollapse = new Dictionary<string, bool>();
            //    }

            //    string str = GenerateGroupSavedKey(group);
            //    if (!m_groupCollapse.ContainsKey(str)
            //        || m_groupCollapse[str] != group.Collapsed)
            //    {
            //        m_groupCollapse[str] = group.Collapsed;
            //    }
            //}
        }

        //internal static string GenerateGroupSavedKey(Group group)
        //{
        //    return group.GridControl.Name + "," + group.Key.ToString() + "," + group.Level.ToString();
        //}

        internal bool m_firstLoad;

        private MyToolTip m_toolTip = new MyToolTip();
        /// <summary>
        /// GridToolTop
        /// </summary>
        public MyToolTip GridToolTip
        {
            get { return m_toolTip; }
        }

        ///// <summary>
        ///// RowSelector双击事件
        ///// </summary>
        //public event EventHandler RowSelectorDoubleClick;

        ///// <summary>
        ///// Row双击事件
        ///// </summary>
        //public event EventHandler RowDoubleClick;

        ///// <summary>
        ///// Cell DoubleClick
        ///// </summary>
        //public event EventHandler CellDoubleClick;

        ///// <summary>
        ///// Cell's Mouse Enter
        ///// </summary>
        //public event EventHandler CellMouseEnter;

        //internal void RowSelector_DoubleClick(object sender, EventArgs e)
        //{
        //    RowSelector selector = sender as RowSelector;
        //    if (RowSelectorDoubleClick != null)
        //    {
        //        RowSelectorDoubleClick(sender, e);
        //    }
        //}

        ///// <summary>
        ///// Row_DoubleClick事件，当cell.ReadOnly=true时执行
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //internal void DataRow_DoubleClick(object sender, System.EventArgs e)
        //{
        //    Xceed.Grid.DataCell cell = sender as Xceed.Grid.DataCell;
        //    if (!cell.ReadOnly)
        //    {
        //        return;
        //    }

        //    if (CellDoubleClick != null)
        //    {
        //        CellDoubleClick(sender, e);
        //    }
        //    else if (RowDoubleClick != null)
        //    {
        //        RowDoubleClick(cell.ParentRow, e);
        //    }
        //}

        internal void Cell_MouseLeave(object sender, System.EventArgs e)
        {
            m_toolTip.RemoveAll();
        }
        internal void Cell_MouseEnter(object sender, System.EventArgs e)
        {
            m_toolTip.RemoveAll();

            Xceed.Grid.Cell cell = sender as Xceed.Grid.Cell;

            if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
            {
                if (cell.ParentColumn.Width < cell.GetFittedWidth())
                {
                    m_toolTip.SetToolTip(cell.GridControl, cell.GetDisplayText());
                }
                else if (cell.CellViewerManager != null)
                {
                    if (cell.CellViewerManager.GetType() == typeof(Viewers.MultiLineViewer))
                    {
                        //if (cell.GetDisplayText().Contains("..."))
                        //{
                        //    m_toolTip.SetToolTip(cell.GridControl,
                        //                         cell.Value.ToString().Substring(0, Math.Min(cell.Value.ToString().Length, 500)));
                        //}
                        if (cell.Value != null && cell.Value.ToString().Contains(System.Environment.NewLine))
                        {
                            m_toolTip.SetToolTip(cell.GridControl, cell.Value.ToString());
                        }
                    }
                    else if (cell.CellViewerManager.GetType() == typeof(Viewers.ImageTextViewer))
                    {
                        m_toolTip.SetToolTip(cell.GridControl, cell.Value.ToString());
                    }
                }
            }


            //if (CellMouseEnter != null)
            //{
            //    CellMouseEnter(sender, e);
            //}
        }

        private bool m_bAllowSort = true;
        /// <summary>
        /// 是否允许排序
        /// </summary>
        [DefaultValue(true)]
        public bool AllowSort
        {
            get { return m_bAllowSort; }
            set
            {
                m_bAllowSort = value;

                ColumnManagerRow row1 = m_grid.GetColumnManageRow();
                if (row1 != null)
                {
                    row1.AllowSort = value;
                }
            }
        }

        /// <summary>
        /// MergeContenxtMenuStrip
        /// </summary>
        /// <param name="contextmenuStrip"></param>
        public void MergeContenxtMenuStripForCell(ContextMenuStrip contextmenuStrip)
        {
            if (contextmenuStrip != this.contextMenuStripForCell)
            {
                ToolStripManager.Merge(contextmenuStrip, this.contextMenuStripForCell);
                contextmenuStrip.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Xceed.Grid.RowSelector ContextSelector
        {
            get { return m_contextSelector; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Xceed.Grid.Cell ContextCell
        {
            get { return m_contextCell; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Xceed.Grid.Row ContextRow
        {
            get
            {
                if (m_contextCell != null)
                {
                    return m_contextCell.ParentRow;
                }
                else if (m_contextSelector != null)
                {
                    return m_contextSelector.Row;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.ContextMenuStrip ContextMenuStripForCell
        {
            get { return contextMenuStripForCell; }
        }

        private Xceed.Grid.RowSelector m_contextSelector;
        internal void RowSelector_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_contextSelector = sender as Xceed.Grid.RowSelector;
                m_contextCell = null;
                tsmShowCellContent.Visible = false;
                contextMenuStripForCell.Show(m_contextSelector.PointToScreen(new System.Drawing.Point(e.X, e.Y)));
            }
        }

        private Xceed.Grid.Cell m_contextCell;
        internal void cell_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_contextSelector = null;
                m_contextCell = sender as Xceed.Grid.DataCell;
                if (m_contextCell.CellViewerManager != null && m_contextCell.CellViewerManager is Feng.Grid.Viewers.MultiLineViewer)
                {
                    tsmShowCellContent.Visible = true;
                }
                else
                {
                    tsmShowCellContent.Visible = false;
                }
                contextMenuStripForCell.Show(m_contextCell.PointToScreen(new System.Drawing.Point(e.X, e.Y)));
            }
        }

        void tsmCopy_Click(object sender, System.EventArgs e)
        {
            if (m_contextCell != null)
            {
                MyGrid.CopySelectedCellsToClipboard(m_contextCell);
                m_contextCell = null;
            }
            else if (m_contextSelector != null)
            {
                MyGrid.CopySelectedRowsToClipboard(m_contextSelector.Row);
                m_contextSelector = null;
            }
        }
        void tsmShowCellContent_Click(object sender, System.EventArgs e)
        {
            if (m_contextCell != null)
            {
                if (m_contextCell.CellViewerManager != null && m_contextCell.CellViewerManager is Feng.Grid.Viewers.MultiLineViewer)
                {
                    Xceed.Editors.WinTextBox textBox = Feng.Grid.Viewers.MultiLineViewer.GetMemoTextBox(m_contextCell.GridControl);
                    if (textBox != null)
                    {
                        textBox.Location = m_contextCell.ClientPointToGrid(new Point(0, 0));
                        (textBox.DropDownControl as Xceed.Editors.WinTextBox).TextBoxArea.Text = m_contextCell.Value == null ? null : m_contextCell.Value.ToString();
                        textBox.DroppedDown = true;
                    }
                }
                m_contextCell = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public static void EndEditRow(Xceed.Grid.CellRow row)
        {
            if (row != null && row.IsBeingEdited)
            {
                try
                {
                    row.EndEdit();
                }
                catch (Xceed.Grid.GridValidationException)
                {
                    row.CancelEdit();
                }
            }
        }
    }
}
