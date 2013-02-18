using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xceed.Grid;
using Xceed.Grid.Exporting;
using System.Text;
using Feng.Utils;
using Feng.Windows.Forms;
using Feng.Grid.Filter;
using Feng.Grid.Print;

namespace Feng.Grid
{
    /// <summary>
    /// 软件默认Grid，包含如下属性：
    /// <list type="bullet">
    /// <item>SelectionMode = SelectionMode.MultiExtended</item>
    /// <item>ReadOnly = true</item>
    /// </list>
    /// </summary>
    [DefaultEvent("RowDoubleClick")]
    public class MyGrid : Xceed.Grid.GridControl, IBindingControl, IStateControl, IReadOnlyControl, IMasterGrid, ILayoutControl, IProfileLayoutControl
    {
        #region "Default Properties"
        /// <summary>
        /// 
        /// </summary>
        static MyGrid()
        {
            RegisterCommand();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.IsDisposed)
                {
                    return;
                }

                //IBindingList bList = this.DataSource as IBindingList;
                //if (bList != null)
                //{
                //    bList.ListChanged -= new ListChangedEventHandler(bindingList_ListChanged);
                //}

                //foreach (Xceed.Grid.Viewers.CellViewerManager i in this.CellViewerManagerMapping.Values)
                //{
                //    i.Dispose();
                //}
                //this.CellViewerManagerMapping.Clear();
                //foreach (Xceed.Grid.Editors.CellEditorManager i in this.CellEditorManagerMapping.Values)
                //{
                //    i.Dispose();
                //}
                //this.CellEditorManagerMapping.Clear();


                this.DataRows.Clear();

                foreach (Xceed.Grid.Column column in this.Columns)
                {
                    if (column.CellEditorManager != null)
                    {
                        if (column.CellEditorManager.TemplateControl != null)
                        {
                            column.CellEditorManager.TemplateControl.Dispose();
                        }
                        column.CellEditorManager.Dispose();
                        column.CellEditorManager = null;
                    }
                    if (column.CellViewerManager != null)
                    {
                        if (column.CellViewerManager.Control != null)
                        {
                            column.CellViewerManager.Control.Dispose();
                        }
                        column.CellViewerManager.Dispose();
                        column.CellViewerManager = null;
                    }

                    column.WidthChanged -= new EventHandler(MyGrid_ColumnWidthChanged);
                }
                

                this.KeyDown -= new KeyEventHandler(grdctrl_KeyDown);
                this.ColumnAdded -= new ColumnAddedEventHandler(MyGrid_ColumnAdded);

                this.RemoveDefaultEvents();

                if (m_gridHelper != null)
                {
                    m_gridHelper.Dispose();
                    m_gridHelper = null;
                }

                foreach (MyDetailGrid detailGrid in this.DetailGridTemplates)
                {
                    detailGrid.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Main GridControl
        /// </summary>
        public Xceed.Grid.GridControl GridControl
        {
            get { return this; }
        }

        private List<Tuple<Control, string>> m_positionChangeControlsAcccordColumn = new List<Tuple<Control, string>>();

        /// <summary>
        /// 把控件放到columnName对应的ColumnManagerRow上方
        /// </summary>
        /// <param name="control"></param>
        /// <param name="columnName"></param>
        public void ChangeControlPositionAccordColumn(Control control, string columnName)
        {
            if (this.Columns[columnName] == null)
            {
                throw new ArgumentException(string.Format("{0}不存在！", columnName), "columnNames"); 
            }
            if (this.GetColumnManageRow() == null)
            {
                throw new InvalidOperationException("ColumnManagerRow不存在!");
            }
            m_positionChangeControlsAcccordColumn.Add(new Tuple<Control, string>(control, columnName));
            if (m_positionChangeControlsAcccordColumn.Count == 1)
            {
                foreach(Xceed.Grid.Column c in this.Columns)
                    c.WidthChanged += new EventHandler(MyGrid_ColumnWidthChanged);
            }
            control.Location = new Point(control.Parent.PointToClient(
                this.PointToScreen(this.GetColumnManageRow().Cells[columnName].ClientPointToGrid(new Point(0, 0)))).X, control.Location.Y);
        }

        void MyGrid_ColumnWidthChanged(object sender, EventArgs e)
        {
            ChangeControlPos();
        }

        private void ChangeControlPos()
        {
            foreach (var i in m_positionChangeControlsAcccordColumn)
            {
                Control control = i.Item1;
                string columnName = i.Item2;
                control.Location = new Point(control.Parent.PointToClient(
                    this.PointToScreen(this.GetColumnManageRow().Cells[columnName].ClientPointToGrid(new Point(0, 0)))).X, control.Location.Y);
            }
        }
        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyGrid()
        {
            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);

            base.SelectionMode = SelectionMode.MultiExtended;
            base.ReadOnly = false;

            this.KeyDown += new KeyEventHandler(grdctrl_KeyDown);
            this.ColumnAdded += new ColumnAddedEventHandler(MyGrid_ColumnAdded);

            //CreateDefaultViewEditors();

            //this.SelectionBackColor = this.BackColor;
            //this.SelectionForeColor = this.ForeColor;
            //this.InactiveSelectionBackColor = this.BackColor;
            //this.InactiveSelectionForeColor = this.ForeColor;
            this.HideSelection = true;

            if (!string.IsNullOrEmpty(GridSetting.CurrentStyleSheet))
            {
                this.ApplyStyleSheet(GridSetting.CurrentStyleSheet);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyGrid(MyGrid template)
            : base(template)
        {
        }

        private void MyGrid_ColumnAdded(object sender, ColumnAddedEventArgs e)
        {
            this.ApplyDefaultSettings();
            this.ApplyDefaultEvents();
        }

        /// <summary>
        /// Default SelectionMode
        /// </summary>
        [DefaultValue(SelectionMode.MultiExtended)]
        public new SelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        /// <summary>
        /// Default ReadOnly
        /// </summary>
        [DefaultValue(false)]
        public new bool ReadOnly
        {
            get { return base.ReadOnly; }
            set { base.ReadOnly = value; }
        }

        #endregion

        #region "//Merged Cell"

        //private int m_oldDataRowCount = -1;

        ///// <summary>
        ///// OnPaint(ReCalculateColumnWidth)
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    if (this.DataRows.Count != m_oldDataRowCount)
        //    {
        //        //ReCalculateColumnWidth();

        //        m_oldDataRowCount = this.DataRows.Count;
        //    }

        //    base.OnPaint(e);

        //    PaintMergedCells(e);
        //}

        //private System.Collections.Generic.List<MergedDataCell> m_mergedCells = new System.Collections.Generic.List<MergedDataCell>();

        //private void PaintMergedCells(PaintEventArgs e)
        //{
        //    foreach (MergedDataCell mergedCell in m_mergedCells)
        //    {
        //        mergedCell.CalculateRectangles();
        //        mergedCell.Paint(e);
        //    }
        //}

        ///// <summary>
        ///// 增加MergeDataCell
        ///// </summary>
        ///// <param name="x">起始Cell's X</param>
        ///// <param name="y">起始Cell's Y</param>
        ///// <param name="dx">横向dx</param>
        ///// <param name="dy">纵向dy</param>
        //public void AddMergedDataCell(int x, int y, int dx, int dy)
        //{
        //    if (x < 0 || x >= this.DataRows.Count
        //        || y < 0 || y >= this.Columns.Count
        //        || dx < 1 || dx + x > this.DataRows.Count
        //        || dy < 1 || dy + y > this.Columns.Count)
        //    {
        //        throw new ArgumentException("invalid argument for mergedDataCell");
        //    }

        //    AddMergedDataCell(this.DataRows[x].Cells[y] as DataCell, dx, dy);
        //}

        ///// <summary>
        ///// 增加MergeDataCell
        ///// </summary>
        ///// <param name="cell">起始Cell</param>
        ///// <param name="dx">横向dx</param>
        ///// <param name="dy">纵向dy</param>
        //public void AddMergedDataCell(DataCell cell, int dx, int dy)
        //{
        //    m_mergedCells.Add(new MergedDataCell(cell, dx, dy));
        //}

        ///// <summary>
        ///// 删除MergedDataCell
        ///// </summary>
        ///// <param name="x">起始Cell's X</param>
        ///// <param name="y">起始Cell's Y</param>
        //public void RemoveMergedDataCell(int x, int y)
        //{
        //    if (x < 0 || x >= this.DataRows.Count
        //        || y < 0 || y >= this.Columns.Count)
        //    {
        //        throw new ArgumentException("invalid argument for mergedDataCell");
        //    }

        //    RemoveMergedDataCell(this.DataRows[x].Cells[y] as DataCell);
        //}

        ///// <summary>
        ///// 删除MergedDataCell
        ///// </summary>
        ///// <param name="cell">起始Cell</param>
        //public void RemoveMergedDataCell(DataCell cell)
        //{
        //    foreach (MergedDataCell mergedCell in m_mergedCells)
        //    {
        //        if (mergedCell.TopLeftCell == cell)
        //        {
        //            m_mergedCells.Remove(mergedCell);
        //            return;
        //        }
        //    }
        //}

        #endregion

        #region "//ShowCount"
        //private bool m_bShowDataCount;
        ///// <summary>
        ///// 是否在Title上显示行数
        ///// </summary>
        //[DefaultValue(false)]
        //public bool ShowDataCount
        //{
        //    get { return m_bShowDataCount; }
        //    set
        //    {
        //        if (m_bShowDataCount != value)
        //        {
        //            m_bShowDataCount = value;
        //            ShowTitleRow();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 重新显示TitleRow（Count改变）
        ///// </summary>
        //protected internal void ShowTitleRow()
        //{
        //    if (!m_bShowDataCount)
        //    {
        //        SetTitleRow("");
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(m_titleRow))
        //    {
        //        SetTitleRow("共" + this.DataRows.Count + "条", false);
        //    }
        //    else
        //    {
        //        SetTitleRow(m_titleRow + ", 共" + this.DataRows.Count + "条", false);
        //    }
        //}

        //private string m_titleRow;
        ///// <summary>
        ///// 设置Grid's Title
        ///// </summary>
        ///// <param name="title"></param>
        //public void SetTitleRow(string title)
        //{
        //    SetTitleRow(title, true);
        //}

        ///// <summary>
        ///// 设置Grid's Title
        ///// </summary>
        ///// <param name="title"></param>
        ///// <param name="save"></param>
        //private void SetTitleRow(string title, bool save)
        //{
        //    if (string.IsNullOrEmpty(title))
        //    {
        //        if (!string.IsNullOrEmpty(m_titleRow) && this.FixedHeaderRows.Count > 0)
        //        {
        //            for (int i = 0; i < this.FixedHeaderRows.Count; ++i)
        //            {
        //                Xceed.Grid.TextRow textRow = this.FixedHeaderRows[i] as Xceed.Grid.TextRow;
        //                if (textRow != null)
        //                {
        //                    this.FixedHeaderRows.RemoveAt(i);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Xceed.Grid.TextRow textRow = null;
        //        if (this.FixedHeaderRows.Count > 0)
        //        {
        //            for (int i = 0; i < this.FixedHeaderRows.Count; ++i)
        //            {
        //                textRow = this.FixedHeaderRows[i] as Xceed.Grid.TextRow;
        //                if (textRow != null)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        if (textRow != null)
        //        {
        //            textRow.Text = title;
        //        }
        //        else
        //        {
        //            textRow = new TextRow(title);
        //            textRow.CanBeSelected = false;
        //            textRow.CanBeCurrent = false;
        //            textRow.ForeColor = Color.Blue;
        //            textRow.ReadOnly = true;
        //            textRow.HorizontalAlignment = Xceed.Grid.HorizontalAlignment.Left;
        //            this.FixedHeaderRows.Insert(0, textRow);
        //        }
        //    }

        //    if (save)
        //    {
        //        m_titleRow = title;
        //    }
        //}

        

        #endregion

        #region "//Color"

        ///// <summary>
        ///// 设置Grid样式(当前只对string类型有效)，针对特殊Cell值设置Row颜色
        ///// </summary>
        ///// <param name="strPattern">要设置的栏名</param>
        ///// <param name="objValue">目标值</param>
        ///// <param name="color">如果Cell.Value=目标值，设置成的颜色</param>
        //public void SetGridColor(string strPattern, object objValue, Color color)
        //{
        //    foreach (Xceed.Grid.DataRow row in DataRows)
        //    {
        //        if (row.Cells[strPattern].Value == objValue)
        //        {
        //            row.BackColor = color;
        //        }
        //    }
        //}

        ///// <summary>
        ///// CellCompare for SetGridColor
        ///// </summary>
        ///// <param name="row"></param>
        ///// <returns></returns>
        //public delegate bool CellCompare(Xceed.Grid.DataRow row);

        ///// <summary>
        ///// 按照要求设置Grid每行颜色
        ///// </summary>
        ///// <param name="compare"></param>
        ///// <param name="color"></param>
        //public void SetGridColor(CellCompare compare, Color color)
        //{
        //    foreach (Xceed.Grid.DataRow row in DataRows)
        //    {
        //        if (compare(row))
        //        {
        //            row.BackColor = color;
        //        }
        //        else
        //        {
        //            row.ResetBackColor();
        //        }
        //    }
        //}

        //private bool m_bSelectionRowColorChanged;
        ///// <summary>
        ///// Selection Row是否有颜色(参看HideSelection)
        ///// </summary>
        //public bool SelectionRowColorChanged
        //{
        //    get { return m_bSelectionRowColorChanged; }
        //    set { m_bSelectionRowColorChanged = value; }
        //}

        ///// <summary>
        ///// 当SelectionMode == SelectionMode.One，改变SelectionRow默认颜色，和Grid其他Row颜色一样
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnSelectedRowsChanged(EventArgs e)
        //{
        //    base.OnSelectedRowsChanged(e);

        //    if (m_bSelectionRowColorChanged)
        //    {
        //        if (this.SelectedRows.Count != 0 && this.SelectionMode == SelectionMode.One)
        //        {
        //            this.SelectionForeColor = this.SelectedRows[0].ForeColor;
        //            this.InactiveSelectionForeColor = this.SelectedRows[0].ForeColor;
        //            this.SelectionBackColor = this.SelectedRows[0].BackColor;
        //            this.InactiveSelectionBackColor = this.SelectedRows[0].BackColor;
        //        }
        //    }
        //}

        //private Color m_bCurrentCellColor = Color.LightBlue;
        //private bool m_bCurrentCellColorChange = false;
        ///// <summary>
        ///// 是否允许当前Cell不同颜色
        ///// </summary>
        //[DefaultValue(false)]
        //public bool CurrentCellColorChange
        //{
        //    get
        //    {
        //        return m_bCurrentCellColorChange;
        //    }
        //    set
        //    {
        //        m_bCurrentCellColorChange = value;

        //        if (m_bCurrentCellColorChange)
        //        {
        //            this.CurrentCellChanged += new EventHandler(this.CellChanged);
        //            if (this.CurrentCell != null)
        //            {
        //                m_previousCurrentCell = this.CurrentCell;
        //                this.CurrentCell.BackColor = m_bCurrentCellColor;
        //            }
        //        }
        //        else
        //        {
        //            this.CurrentCellChanged -= new EventHandler(this.CellChanged);
        //            if (m_previousCurrentCell != null)
        //            {
        //                m_previousCurrentCell.ResetBackColor();
        //                m_previousCurrentCell = null;
        //            }
        //        }
        //    }
        //}

        ////In the CurrentCellChanged event handler 
        //private Xceed.Grid.Cell m_previousCurrentCell = null;
        //private void CellChanged(object sender, EventArgs e)
        //{
        //    if (m_previousCurrentCell != null)
        //    {
        //        m_previousCurrentCell.ResetBackColor();
        //    }
        //    if (this.CurrentCell != null)
        //    {
        //        this.CurrentCell.BackColor = m_bCurrentCellColor;
        //        m_previousCurrentCell = this.CurrentCell;
        //    }

        //    this.Invalidate();
        //}

        #endregion

        #region "DataSource"

        /// <summary>
        /// 设置数据源(只是为了继承类能override)
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public new virtual void SetDataBinding(object dataSource, string dataMember)
        {
            //IBindingList bList = dataSource as IBindingList;
            //if (bList != null)
            //{
            //    bList.ListChanged -= new ListChangedEventHandler(bindingList_ListChanged);
            //    bList.ListChanged += new ListChangedEventHandler(bindingList_ListChanged);
            //}
            base.SetDataBinding(dataSource, dataMember);

            //this.AfterLoadData();
        }

        //private void bindingList_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    if (e.ListChangedType == ListChangedType.PropertyDescriptorChanged)
        //    {
        //        //this.CancelEditCurrentDataRow();
        //    }
        //    else if (e.ListChangedType == ListChangedType.Reset)
        //    {
        //        //this.AfterLoadData();
        //    }

        //    //this.ShowTitleRow();
        //}

        /// <summary>
        /// Current DataRow(if not dataRow, return null)
        /// </summary>
        public Xceed.Grid.DataRow CurrentDataRow
        {
            get { return this.CurrentRow as Xceed.Grid.DataRow; }
        }

        #endregion

        #region "//Columns"

        //private int m_maxColumnWidth = 300;
        ///// <summary>
        ///// Max width of every column
        ///// </summary>
        //[Category("Data")]
        //[DefaultValue(300)]
        //public int MaxColumnWidth
        //{
        //    get { return m_maxColumnWidth; }
        //    set { m_maxColumnWidth = value; }
        //}

        //private int m_minColumnWidth = 30;
        ///// <summary>
        ///// Min width of every column
        ///// </summary>
        //[Category("Data")]
        //[DefaultValue(30)]
        //public int MinColumnWidth
        //{
        //    get { return m_minColumnWidth; }
        //    set { m_minColumnWidth = value; }
        //}

        //private bool m_bAutoColumnWidth = true;

        ///// <summary>
        ///// 是否自动设置ColunWidth
        ///// </summary>
        //[DefaultValue(true)]
        //public bool AutoColumnWidth
        //{
        //    get { return m_bAutoColumnWidth; }
        //    set { m_bAutoColumnWidth = value; }
        //}

        #endregion

        #region "Utility"
        /// <summary>
        /// Get Grid Level(Master or Detail)
        /// </summary>
        /// <param name="detailGrid"></param>
        /// <returns></returns>
        public static string GetGridLevel(Xceed.Grid.DetailGrid detailGrid)
        {
            if (detailGrid == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            int level = 0;
            Xceed.Grid.DetailGrid nowGrid = detailGrid;
            while (nowGrid != null)
            {
                int seq = 0;
                if (nowGrid.ParentDataRow != null)
                {
                    seq = nowGrid.ParentDataRow.DetailGrids.IndexOf(detailGrid);
                }
                if (sb.Length > 0)
                {
                    sb.Insert(0, '.');
                }
                sb.Insert(0, seq.ToString());

                nowGrid = nowGrid.ParentGrid;
                level++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Key_Down事件，执行拷贝操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdctrl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //System.Diagnostics.Trace.TraceInformation(e.KeyCode.ToString());

            //if ((e.KeyData & Keys.Control) != 0
            //    && (e.KeyData - Keys.Control > 17))
            //{
            //}
            if ((e.KeyData == (Keys.Control | Keys.C)) || (e.KeyData == (Keys.Control | Keys.Add)))
            {
                CopySelectedCellsToClipboard(this.CurrentCell);
            }
            else if (e.KeyData == (Keys.Control | Keys.Shift | Keys.C))
            {
                CopySelectedRowsToClipboard(this.CurrentRow);
            }
            else if (e.KeyData == (Keys.Control | Keys.Shift | Keys.V))
            {
                PasteClipBoardToInsertionRow(this);
            }
            else if ((e.KeyData == (Keys.Control | Keys.P)))
            {
                ExportToExcel();
            }
            else if ((e.KeyData == (Keys.Control | Keys.Shift | Keys.P)))
            {
                //PrintReport();
            }
            else if (e.KeyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                //MyGridSetupForm form = new MyGridSetupForm(this);
                //form.ShowDialog(this);
            }
        }

        /// <summary>
        /// PrintPreview
        /// </summary>
        public void PrintPriviewGrid()
        {
            using (MyGridPrintDocument gridPrintDocument = new MyGridPrintDocument(this))
            {
                // See this.QueryPrintPage for how to use this event.
                gridPrintDocument.QueryPrintPage += new Xceed.Grid.QueryPrintPageEventHandler(this.QueryPrintPage);

                using (PrintPreviewForm printPreviewForm = new PrintPreviewForm(gridPrintDocument))
                {
                    try
                    {
                        printPreviewForm.ShowDialog(this.FindForm());
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithNotify(ex);
                    }
                }

                gridPrintDocument.QueryPrintPage -= new Xceed.Grid.QueryPrintPageEventHandler(this.QueryPrintPage);
            }
           
        }

        // This event handler is called for all pages to allow printing of selected pages. 
        // It can be called one or many times during a single base.OnPrintPage call 
        // ( until a page is allowed to print or printing is cancelled ).
        // Some usefull properties :
        // e.IsPageSeleted selects the pages to be printed.
        // e.PrintPageEventArgs.HasMorePages signals if the last page has been reached.
        // e.CurrentPrintingPage is the number of the page to be allowed to print.
        // e.PrintPageEventArgs.PageSettings.PrinterSettings.PrintRange : <see cref="System.Drawing.Printing.PrintRange"/>
        // e.Cancel can cancell the full printing process ( i.e.: when no page is selected, on an error/user abort ).
        // Depending on PrintRange value ( SomePages, Selection, AllPages ). 
        // You might want this method to behave differently.
        private void QueryPrintPage(object sender, Xceed.Grid.QueryPrintPageEventArgs e)
        {
            // Example 1 : Signaling printing is done
            // ---------------------------------------------------------
            // This would inform the printing process that the last page ( lastPageNumber )
            // was just printed. And therefore will stop printing after this page.
            // Assuming the last page to be printed is an int of name lastPageNumber :
            // if( e.CurrentPageNumber == lastPageNumber )  
            //   e.PrintPageEventArgs.HasMorePages = false;

            // Example 2 : Printing selected pages, or a subset of pages
            // ----------------------------------------------------------
            // This would inform the printing process that this page is not to be printed.
            // This example illustrates printing a Selection 
            // ( a selected page IList could be used to store currently selected pages ).
            // Assuming a selection of type IList named CurrentSelectionPages :
            // if( !CurrentSelectionPages.Contains( e.CurrentPageNumber ) )
            //   e.IsPageSelected = false;
        }

        /// <summary>
        /// GenerateReport
        /// </summary>
        public void GenerateReport()
        {
            // GridUtils.SaveInnerReportStyleSheet();

            using (GenerateReportForm generateReportForm = new GenerateReportForm(this, this.GetGridDefaultDataDirectory(), this.GetGridGlobalDataDirectory()))
            {
                try
                {
                    generateReportForm.ShowDialog(this.FindForm());
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        /// <summary>
        /// 用于标志数据库配置数据的Grid名称
        /// </summary>
        [DefaultValue(null)]
        public virtual string GridName
        {
            get;
            set;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="keyData"></param>
        ///// <returns></returns>
        //protected override System.Boolean ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    // ProcessCmdKey is only used for the Enter and the Escape keys.
        //    if (keyData == Keys.Enter)
        //    {
        //        if (this.CurrentColumn.Index == this.Columns.Count - 1)
        //        {
        //            this.MoveCurrentRow(VerticalDirection.Down);
        //            this.MoveCurrentCell(HorizontalDirection.Leftmost);
        //        }
        //        else
        //        {
        //            // Move to the next cell on the right when the enter key is pressed
        //            this.MoveCurrentCell(HorizontalDirection.Right);
        //            this.CurrentCell.BringIntoView();
        //        }

        //        return true;
        //    }
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="IncludeDetailGrids"></param>
        /// <param name="grid"></param>
        private static void ExportToExcelXml(string fileName, bool IncludeDetailGrids, IGrid grid)
        {
            ExcelExporter excelExporter = new ExcelExporter();

            excelExporter.IncludeColumnHeaders = true;
            excelExporter.GridLineColor = Color.DarkGray;
            excelExporter.IncludeGridStyles = true;

            excelExporter.RepeatParentData = false;
            excelExporter.DetailGridsMode = DetailGridsMode.Independent;

            excelExporter.IncludeDetailGrids = IncludeDetailGrids;

            excelExporter.CellDataFormat = CellDataFormat.Value;
            excelExporter.WritingCell += new WritingCellElementEventHandler(excelExporter_WritingCell);
            try
            {
                if (grid is DetailGrid)
                {
                    excelExporter.ColumnHeaderStyle = new ExcelStyle(Color.White, Color.Black,
                                                             new Font((grid as DetailGrid).Font, FontStyle.Bold),
                                                             ContentAlignment.BottomLeft);
                    excelExporter.Export((grid as DetailGrid), fileName);
                }
                else if (grid is GridControl)
                {
                    excelExporter.ColumnHeaderStyle = new ExcelStyle(Color.White, Color.Black,
                                                             new Font((grid as GridControl).Font, FontStyle.Bold),
                                                             ContentAlignment.BottomLeft);
                    excelExporter.Export((grid as GridControl), fileName);
                }

                //if ((MessageForm.ShowYesNo("已成功导出！" + System.Environment.NewLine + System.Environment.NewLine + "是否要在Excel中打开？", "成功导出")))
                //{
                //    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                //}
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                excelExporter.WritingCell -= new WritingCellElementEventHandler(excelExporter_WritingCell);
            }
        }

        private static void excelExporter_WritingCell(object sender, WritingCellElementEventArgs e)
        {
            if (e.Cell != null)
            {
                if (e.Cell.ParentColumn.DataType == typeof (string) 
                    || e.Cell.ParentColumn.DataType == typeof(Guid) || e.Cell.ParentColumn.DataType == typeof(Guid?))
                {
                    e.Value = e.Cell.GetDisplayText();
                    e.TypeAttribute.Value = "String";
                }
                else if (e.Cell.ParentColumn.DataType == typeof (DateTime)
                         || e.Cell.ParentColumn.DataType == typeof (DateTime?))
                {
                    e.Value = e.Cell.GetDisplayText();
                    e.TypeAttribute.Value = "DateTime";
                }
                else if (e.Cell.CellViewerManager != null &&
                            (e.Cell.CellViewerManager.GetType() == (typeof(Viewers.MyComboBoxViewer))
                            || e.Cell.CellViewerManager.GetType() == (typeof(Viewers.MyOptionPickerViewer))))
                {
                    e.Value = e.Cell.GetDisplayText();
                    e.TypeAttribute.Value = "String";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ICommand ExportToExcelCommand;
        /// <summary>
        /// 
        /// </summary>
        public static string ExportToExcelCommandId;

        private static void RegisterCommand()
        {
            ExportToExcelCommandId = "Grid.ExportToExcel";
            ExportToExcelCommand = new Command(ExportToExcelCommandId);

            CommandManager.Register(ExportToExcelCommand, OnExecutedExportToExcelCommand);
        }

        private static void OnExecutedExportToExcelCommand(object sender, ExecutedEventArgs e)
        {
            ExportToExcel(sender as MyGrid);
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        private void ExportToExcel()
        {
            ExportToExcel(this);
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="detailGrid"></param>
        private static void ExportToExcel(IGrid grid)
        {
            //if (grid.DataRows.Count == 0)
            //{
            //    return;
            //}

            using (MyGridExportSaveFileDialog saveFileDialog = new MyGridExportSaveFileDialog())
            {
                if (grid.DetailGridTemplates.Count == 0)
                {
                    saveFileDialog.CkbIncludeDetailGrids.Enabled = false;
                }

                if (saveFileDialog.ShowDialog(grid.FindForm()) != DialogResult.OK)
                {
                    return;
                }

                string fileName = saveFileDialog.FileDlgFileName;
                string ext = System.IO.Path.GetExtension(fileName);
                if (ext == ".xml")
                {
                    ExportToExcelXml(fileName, saveFileDialog.CkbIncludeDetailGrids.Checked, grid);
                }
                else
                {
                    string tempFileName = System.IO.Path.GetTempFileName();
                    tempFileName = System.IO.Path.ChangeExtension(tempFileName, ".xml");
                    ExportToExcelXml(tempFileName, saveFileDialog.CkbIncludeDetailGrids.Checked, grid);
                    switch (ext)
                    {
                        case ".xls":
                        case ".xlsx":
                            Feng.Windows.Utils.ExcelHelper.ConvertExcel(tempFileName, fileName, !saveFileDialog.CkbIncludeDetailGrids.Checked);
                            System.IO.File.Delete(tempFileName);
                            break;
                        default:
                            throw new NotSupportedException("Invalid file extention of " + ext);
                    }
                }
            }
        }

        private static string m_copyDataForPaste;
        /// <summary>
        /// 
        /// </summary>
        public static void PasteClipBoardToInsertionRow(IGrid grid)
        {
            string data = m_copyDataForPaste;
            if (!string.IsNullOrEmpty(data))
            {
                InsertionRow row = grid.GetInsertionRow();
                if (row != null)
                {
                    row.BeginEdit();
                    string[] ss = data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length < 2)
                        return;
                    Dictionary<int, string> columnNames = new Dictionary<int,string>();
                    string[] sss = ss[0].Split(new char[] { '\t' }, StringSplitOptions.None);
                    {
                        for (int j = 0; j < sss.Length; ++j)
                        {
                            columnNames[j] = sss[j];
                        }
                    }
                    for (int i = 1; i < ss.Length; )
                    {
                        sss = ss[i].Split(new char[] { '\t' }, StringSplitOptions.None);
                        for (int j = 0; j < sss.Length; ++j)
                        {
                            try
                            {
                                if (grid.Columns[columnNames[j]] == null)
                                    continue;

                                if (string.IsNullOrEmpty(sss[j]))
                                {
                                    row.Cells[columnNames[j]].Value = null;
                                }
                                else
                                {
                                    object r = ConvertHelper.ChangeType(sss[j], grid.Columns[columnNames[j]].DataType);
                                    row.Cells[columnNames[j]].Value = r;
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionProcess.ProcessWithNotify(ex);
                            }
                        }
                        break;
                    }
                }
            }
        }
        
        public static IList<Xceed.Grid.Row> GetSelectedRowsToOperation(Xceed.Grid.Row currentRow)
        {
            IList<Xceed.Grid.Row> rowList = new List<Xceed.Grid.Row>();
            if (currentRow.GridControl.SelectedRows.Contains(currentRow))
            {
                foreach (Xceed.Grid.Row row in currentRow.GridControl.SelectedRows)
                {
                    if (row.ParentGrid == currentRow.ParentGrid)
                    {
                        rowList.Add(row);
                    }
                }
            }
            else
            {
                rowList.Add(currentRow);
            }

            return rowList;
        }

        //public static void CopyRowsTagToClipboard(IList<Xceed.Grid.Row> rowList)
        //{
            //System.Collections.ArrayList arr = new System.Collections.ArrayList();
            //foreach (Xceed.Grid.Row row in rowList)
            //{
            //    if (row.Tag != null)
            //    {
            //        arr.Add(row.Tag);
            //    }
            //}
            //IDataObject data = Clipboard.GetDataObject();
            //if (data == null)
            //{
            //    data = new DataObject();
            //}
            //data.SetData("FengsData", arr);
            //Clipboard.SetDataObject(data);
        //}

        /// <summary>
        /// 复制选中行的值到Clipboard
        /// </summary>
        /// <param name="currentRow"></param>
        /// <returns></returns>
        public static IList<Xceed.Grid.Row> CopySelectedRowsToClipboard(Xceed.Grid.Row currentRow)
        {
            try
            {
                if (currentRow != null)
                {
                    StringBuilder sbText = new StringBuilder();
                    StringBuilder sbValue = new StringBuilder();

                    IList<Xceed.Grid.Row> rowList = GetSelectedRowsToOperation(currentRow);

                    // Column Title
                    for (int j = 0; j < currentRow.ParentGrid.Columns.DisplayableColumnCount; ++j)
                    {
                        sbText.Append(currentRow.ParentGrid.Columns.GetColumnAtDisplayableIndex(j).Title);
                        if (j != currentRow.ParentGrid.Columns.DisplayableColumnCount - 1)
                        {
                            sbText.Append("\t");
                        }
                    }
                    sbText.Append(System.Environment.NewLine);

                    // Column Title
                    for (int j = 0; j < currentRow.ParentGrid.Columns.DisplayableColumnCount; ++j)
                    {
                        sbValue.Append(currentRow.ParentGrid.Columns.GetColumnAtDisplayableIndex(j).FieldName);
                        if (j != currentRow.ParentGrid.Columns.DisplayableColumnCount - 1)
                        {
                            sbValue.Append("\t");
                        }
                    }
                    sbValue.Append(System.Environment.NewLine);

                    for (int i = 0; i < rowList.Count; ++i)
                    {
                        Xceed.Grid.CellRow row = rowList[i] as Xceed.Grid.CellRow;

                        for (int j = 0; j < currentRow.ParentGrid.Columns.DisplayableColumnCount; ++j)
                        {
                            Xceed.Grid.Cell cell = row.Cells[currentRow.ParentGrid.Columns.GetColumnAtDisplayableIndex(j).FieldName];
                            if (cell.Value == null)
                            {
                                sbText.Append(string.Empty);
                            }
                            else
                            {
                                if (cell.CellViewerManager != null && cell.CellViewerManager is INameValueControl)
                                {
                                    sbText.Append(cell.GetDisplayText());
                                }
                                else
                                {
                                    sbText.Append(cell.Value.ToString());
                                }
                            }
                            if (j != currentRow.ParentGrid.Columns.DisplayableColumnCount - 1)
                            {
                                sbText.Append("\t");
                            }

                            if (cell.Value == null)
                            {
                                sbValue.Append(string.Empty);
                            }
                            else
                            {
                                sbValue.Append(cell.Value.ToString());
                            }
                            if (j != currentRow.ParentGrid.Columns.DisplayableColumnCount - 1)
                            {
                                sbValue.Append("\t");
                            }
                        }
                        if (i != rowList.Count - 1)
                        {
                            sbText.Append(System.Environment.NewLine);
                            sbValue.Append(System.Environment.NewLine);
                        }
                    }

                    Feng.Windows.Utils.ClipboardHelper.CopyTextToClipboard(sbText.ToString());

                    m_copyDataForPaste = sbValue.ToString();

                    //CopyRowsTagToClipboard(rowList);

                    return rowList;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            return null;
        }

        public static string GetSelectedCellsStrings(Xceed.Grid.Cell currentCell)
        {
            try
            {
                if (currentCell != null)
                {
                    StringBuilder sb = new StringBuilder();

                    System.Collections.IList rowList;
                    if (currentCell.GridControl.SelectedRows.Contains(currentCell.ParentRow))
                    {
                        rowList = currentCell.GridControl.SelectedRows;
                    }
                    else
                    {
                        rowList = new List<Xceed.Grid.DataRow>();
                        rowList.Add(currentCell.ParentRow);
                    }

                    foreach (Xceed.Grid.CellRow row in rowList)
                    {
                        Xceed.Grid.Cell cell = row.Cells[currentCell.ParentColumn.Index];
                        if (cell.Value == null)
                        {
                            sb.Append(string.Empty);
                        }
                        else
                        {
                            if (cell.CellViewerManager != null && cell.CellViewerManager is INameValueControl)
                            {
                                sb.Append(cell.GetDisplayText());
                            }
                            else
                            {
                                sb.Append(cell.Value.ToString());
                            }
                        }
                        sb.Append(System.Environment.NewLine);
                    }

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            return null;
        }

        /// <summary>
        /// 复制选中行的currentCell列中的值到剪贴板
        /// </summary>
        /// <param name="cell"></param>
        public static void CopySelectedCellsToClipboard(Xceed.Grid.Cell currentCell)
        {
            Feng.Windows.Utils.ClipboardHelper.CopyTextToClipboard(GetSelectedCellsStrings(currentCell));
        }
        
        ///// <summary>
        ///// Copy操作
        ///// </summary>
        //public void DoCopy()
        //{
        //    if (this.CurrentCell != null)
        //    {
        //        CopySelectedCellsToClipboard(this.CurrentCell);
        //    }
        //}

        ///// <summary>
        ///// Paste操作
        ///// </summary>
        //public void DoPaste()
        //{
        //    if (this.CurrentCell != null && !this.CurrentCell.ReadOnly)
        //    {
        //        try
        //        {
        //            string data = (string)Clipboard.GetDataObject().GetData(DataFormats.Text);
        //            if (!string.IsNullOrEmpty(data))
        //            {
        //                if (this.CurrentCell.IsBeingEdited)
        //                    this.CurrentCell.LeaveEdit(false);
        //                this.CurrentCell.Value = data;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorLogs.ShowExceptionMsg(ex);
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public void EndEditCurrentDataRow()
        {
            Xceed.Grid.CellRow row = this.CurrentRow as Xceed.Grid.CellRow;
            if (row != null && row.IsBeingEdited)
            {
                row.EndEdit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CancelEditCurrentDataRow(IGrid grid)
        {
            // 当enter Edit Mode 后，查找不存在的列表，出现错误
            try
            {
                Xceed.Grid.CellRow row = grid.CurrentRow as Xceed.Grid.CellRow;
                if (row != null && row.IsBeingEdited)
                {
                    row.CancelEdit();
                }

                InsertionRow insertionRow = grid.GetInsertionRow();
                if (insertionRow != null) //不一定是Edit && insertionRow.IsBeingEdited)
                {
                    insertionRow.CancelEdit();

                    // bug in xceed?
                    foreach (Xceed.Grid.Cell cell in insertionRow.Cells)
                    {
                        cell.ResetReadOnly();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public virtual void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion

        #region "Wraper"
        private GridHelper m_gridHelper;
        /// <summary>
        /// GridHelper
        /// </summary>
        public GridHelper GridHelper
        {
            get 
            {
                if (m_gridHelper == null)
                {
                    m_gridHelper = new GridHelper(this);
                }
                return m_gridHelper; 
            }
        }

        ///// <summary>
        ///// RowSelectorDoubleClick
        ///// </summary>
        //public event EventHandler RowSelectorDoubleClick
        //{
        //    add { this.GridHelper.RowSelectorDoubleClick += value; }
        //    remove { this.GridHelper.RowSelectorDoubleClick -= value; }
        //}

        ///// <summary>
        ///// RowDoubleClick
        ///// </summary>
        //public event EventHandler RowDoubleClick
        //{
        //    add { this.GridHelper.RowDoubleClick += value; }
        //    remove { this.GridHelper.RowDoubleClick -= value; }
        //}

        ///// <summary>
        ///// CellDoubleClick
        ///// </summary>
        //public event EventHandler CellDoubleClick
        //{
        //    add { this.GridHelper.CellDoubleClick += value; }
        //    remove { this.GridHelper.CellDoubleClick -= value; }
        //}

        /// <summary>
        /// ToolTip used at Grid
        /// </summary>
        public MyToolTip GridToolTip
        {
            get { return this.GridHelper.GridToolTip; }
        }
        #endregion

        #region "Layout"
        /// <summary>
        /// 
        /// </summary>
        public new Xceed.Grid.Row CurrentRow
        {
            get { return base.CurrentRow; }
            set { SetCurrentRow(this, value); }
        }

        /// <summary>
        /// 把当前行选中
        /// </summary>
        /// <param name="grid"></param>
        public static void SyncSelectedRowToCurrentRow(GridControl grid)
        {
            grid.SelectedRows.Clear();
            if (grid.CurrentRow != null)
            {
                grid.SelectedRows.Add(grid.CurrentRow);
            }
        }

        /// <summary>
        /// 设置当前行
        /// </summary>
        /// <param name="row"></param>
        public static void SetCurrentRow(GridControl grid, Xceed.Grid.Row row)
        {
            try
            {
                if (grid.CurrentRow != row)
                {
                    // when datarow invisible, can't set currentRow
                    //if (!row.CanBeCurrent)
                    {
                        if (row.ParentGrid != null && row.ParentGrid.Collapsed)
                        {
                            row.ParentGrid.Expand();
                        }
                        if (row.ParentGroup != null && row.ParentGroup.Collapsed)
                        {
                            row.ParentGroup.Expand();
                        }
                    }
                    grid.CurrentRow = row;

                    if (grid.Visible && row.Visible)
                    {
                        row.BringIntoView();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }


        internal const string m_layoutDefaultFileName = "system.xmlg.default";
        public string LayoutFilePath
        {
            get
            {
                if (this.FindForm() != null && !string.IsNullOrEmpty(this.GridName))
                {
                    return (this.FindForm().Name + "\\") + (this.GridName + "\\") + m_layoutDefaultFileName;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool LoadLayout()
        {
            bool ret = GridLayoutExtention.LoadLayout(this);

            foreach (Xceed.Grid.Column column in this.Columns)
            {
                if (column.CellEditorManager != null)
                {
                    ILayoutControl c = column.CellEditorManager.TemplateControl as ILayoutControl;
                    if (c != null)
                    {
                        c.LoadLayout();
                    }
                }
            }
            return ret;
        }

        public bool SaveLayout()
        {
            bool ret = GridLayoutExtention.SaveLayout(this);
            foreach (Xceed.Grid.Column column in this.Columns)
            {
                if (column.CellEditorManager != null)
                {
                    ILayoutControl c = column.CellEditorManager.TemplateControl as ILayoutControl;
                    if (c != null)
                    {
                        c.SaveLayout();
                    }
                }
            }
            return ret;
        }

        public bool LoadLayout(AMS.Profile.IProfile profile)
        {
            bool ret = GridLayoutExtention.LoadLayout(this, profile);

            ChangeControlPos();

            return ret;
        }

        public bool SaveLayout(AMS.Profile.IProfile profile)
        {
            return GridLayoutExtention.SaveLayout(this, profile);
        }
        #endregion

        #region "DragDrop"
        private bool m_enableDragdrop;

        /// <summary>
        /// EnableDragDrop
        /// </summary>
        [DefaultValue(false)]
        public bool EnableDragDrop
        {
            get { return m_enableDragdrop; }
            set
            {
                if (m_enableDragdrop != value)
                {
                    m_enableDragdrop = value;

                    this.BeginInit();
                    this.AllowDrop = m_enableDragdrop;

                    this.DragOver -= new DragEventHandler(this.DragDrop_Drag_Over);
                    this.DragDrop -= new DragEventHandler(this.DragDrop_Drag_Drop);
                    if (m_enableDragdrop)
                    {
                        this.DragOver += new DragEventHandler(this.DragDrop_Drag_Over);
                        this.DragDrop += new DragEventHandler(this.DragDrop_Drag_Drop);
                    }

                    foreach (DataCell cell in this.DataRowTemplate.Cells)
                    {
                        cell.AllowDrop = m_enableDragdrop;

                        cell.MouseDown -= new MouseEventHandler(this.DragDrop_Mouse_Down);
                        cell.MouseUp -= new MouseEventHandler(this.DragDrop_Mouse_Up);
                        cell.MouseMove -= new MouseEventHandler(this.DragDrop_Mouse_Move);
                        cell.DragOver -= new DragEventHandler(this.DragDrop_Drag_Over);
                        cell.DragDrop -= new DragEventHandler(this.DragDrop_Drag_Drop);

                        if (m_enableDragdrop)
                        {
                            cell.MouseDown += new MouseEventHandler(this.DragDrop_Mouse_Down);
                            cell.MouseUp += new MouseEventHandler(this.DragDrop_Mouse_Up);
                            cell.MouseMove += new MouseEventHandler(this.DragDrop_Mouse_Move);
                            cell.DragOver += new DragEventHandler(this.DragDrop_Drag_Over);
                            cell.DragDrop += new DragEventHandler(this.DragDrop_Drag_Drop);
                        }
                    }
                    foreach (DataRow row in this.DataRows)
                    {
                        foreach (DataCell cell in row.Cells)
                        {
                            cell.AllowDrop = m_enableDragdrop;

                            cell.MouseDown -= new MouseEventHandler(this.DragDrop_Mouse_Down);
                            cell.MouseUp -= new MouseEventHandler(this.DragDrop_Mouse_Up);
                            cell.MouseMove -= new MouseEventHandler(this.DragDrop_Mouse_Move);
                            cell.DragOver -= new DragEventHandler(this.DragDrop_Drag_Over);
                            cell.DragDrop -= new DragEventHandler(this.DragDrop_Drag_Drop);

                            if (m_enableDragdrop)
                            {
                                cell.MouseDown += new MouseEventHandler(this.DragDrop_Mouse_Down);
                                cell.MouseUp += new MouseEventHandler(this.DragDrop_Mouse_Up);
                                cell.MouseMove += new MouseEventHandler(this.DragDrop_Mouse_Move);
                                cell.DragOver += new DragEventHandler(this.DragDrop_Drag_Over);
                                cell.DragDrop += new DragEventHandler(this.DragDrop_Drag_Drop);
                            }
                        }
                    }
                    this.EndInit();
                }
            }
        }


        //private string[] m_dragDropDataFormats = null;
        ///// <summary>
        ///// 
        ///// </summary>
        //public string DragDropDataFormat
        //{
        //    get { return m_dragDropDataFormat; }
        //    set { m_dragDropDataFormat = value; }
        //}


        private Point m_dragdropMouseLocation = Point.Empty;
        private void DragDrop_Mouse_Down(object sender, MouseEventArgs e)
        {
            // Get the location of the mouse when the mouse button is pressed.
            m_dragdropMouseLocation = new Point(e.X, e.Y);
        }

        private void DragDrop_Mouse_Up(object sender, MouseEventArgs e)
        {
            // Reset the location of the mouse when the mouse button is released.
            m_dragdropMouseLocation = Point.Empty;
        }

        private void DragDrop_Mouse_Move(object sender, MouseEventArgs e)
        {
            // The mouse button is pressed
            if (m_dragdropMouseLocation != Point.Empty)
            {
                //System.Diagnostics.Debug.Write("Move" + e.X + "," + e.Y + "\r\n");

                // The mouse has moved!
                if (Math.Abs(m_dragdropMouseLocation.X - e.X) > 3 || Math.Abs(m_dragdropMouseLocation.Y - e.Y) > 3)
                {
                    //System.Diagnostics.Debug.Write("Move1 " + e.X + "," + e.Y + "\r\n");

                    if (GridDragStart != null)
                    {
                        //Point p = (sender as Xceed.Grid.Cell).ClientPointToGrid(new Point(e.X, e.Y));

                        GridDataGragEventArgs e2 = new GridDataGragEventArgs();
                        GridDragStart(sender, e2);

                        //m_dragDropDataFormats = e2.Data.GetFormats();

                        //// Create our DataObject that will contain the data (rows/cells) to drag.
                        //DataObject data = new DataObject(this.CreateDataObject());

                        // Initialize the drag/drop operation.
                        if (e2.Data != null && e2.AllowedEffect != DragDropEffects.None)
                        {
                            this.DoDragDrop(e2.Data, e2.AllowedEffect);
                        }
                    }

                    // Reset the location of the mouse.
                    m_dragdropMouseLocation = Point.Empty;
                }
            }
        }

        private void DragDrop_Drag_Over(object sender, DragEventArgs e)
        {
            // default is no operation
            e.Effect = DragDropEffects.None;

            if (GridDragOver != null)
            {
                //Point p = (sender as Xceed.Grid.Cell).ClientPointToGrid(new Point(e.X, e.Y));
                //DragEventArgs e2 = new DragEventArgs(e.Data, e.KeyState, p.X, p.Y, e.AllowedEffect, e.Effect);
                GridDragOver(sender, e);
            }
        }

        private void DragDrop_Drag_Drop(object sender, DragEventArgs e)
        {
            //// Retrieve the data that is being dropped onto the grid.
            //string data = (string)e.Data.GetData(DataFormats.Text);

            //// If there is data, we will parse it and insert the new DataRows.
            //if (data != string.Empty)
            //    this.InsertNewRows(data);

            if (GridDragDrop != null)
            {
                //Point p = (sender as Xceed.Grid.Cell).ClientPointToGrid(new Point(e.X, e.Y));
                //DragEventArgs e2 = new DragEventArgs(e.Data, e.KeyState, p.X, p.Y, e.AllowedEffect, e.Effect);

                GridDragDrop(sender, e);
            }
        }

        public event EventHandler<GridDataGragEventArgs> GridDragStart;

        public event DragEventHandler GridDragDrop;

        public event DragEventHandler GridDragOver;
        #endregion
    }
}