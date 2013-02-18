using System;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using Xceed.Grid;
using Xceed.Grid.Editors;
using Xceed.Grid.Collections;
using Xceed.Editors;
using Feng.Utils;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 软件默认ComboBox，基本形态为多列，其中一列为显示的值，一列为保存的值
    /// 关于自动筛选：只对Visible的Column有效。以原文开头，以原文的拼音全拼开头，以原文的每个汉字的拼音首字母开头。筛选以上三项。
    /// 当移出焦点后，已以下顺序Validate。原文的完全匹配，原文的不分匹配，原文的拼音全拼匹配。如果都没有，清空。
    /// </summary>
    public class MyComboBox : Xceed.Editors.WinComboBox, INameValueMappingBindingControl, IGridDropdownControl, ILayoutControl
    {
        #region "Constructor"
        protected override Size DefaultDropDownMinSize
        {
            get
            {
                return new Size(180, 60);
            }
        }
        protected override void OnDropDownOpening(System.ComponentModel.CancelEventArgs e)
        {
            MyDatePickerXceed.RelocateDropdownAnchir(this);
            
            base.OnDropDownOpening(e);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_editTopNv != null)
                {
                    m_editTopNv.DataSourceChanged -= new EventHandler(MyComboBox_DataTableChanged);
                    m_editTopNv.DataSourceChanging -= new System.ComponentModel.CancelEventHandler(MyComboBox_DataTableChanging);
                }

                this.SelectedIndexChanged -= new EventHandler(MyComboBox_SelectedIndexChanged);
                this.DropDownOpening -= new System.ComponentModel.CancelEventHandler(MyComboBox_DropDownOpening);
            }
            base.Dispose(disposing);
        }

        private void Initialize()
        {
            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);

            base.BorderStyle = Xceed.Editors.EnhancedBorderStyle.UIStyle;
            base.ColumnWidthAdjustment = ColumnWidthAdjustment.None;
            base.IntegralHeight = false;
            this.DropDownAnchor = DropDownAnchor.Right;
            //base.AllowFreeText = false;

            base.Size = new System.Drawing.Size(120, 21);
            base.DropDownSize = new System.Drawing.Size(100, this.DropDownControl.Height);

            this.SelectedIndexChanged += new EventHandler(MyComboBox_SelectedIndexChanged);
            this.DropDownOpening += new System.ComponentModel.CancelEventHandler(MyComboBox_DropDownOpening);

            this.DropDownControl.FixedHeaderRows.Clear();
            this.DropDownControl.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());
        }

        void MyComboBox_DropDownOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.AdjustDropDownControlHeight();
        }
        private void MyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 如果为FreeComboBox，则当内容不在列表中时，光标不移动到最前
            if (this.SelectedIndex != -1)
            {
                // Caret到最前
                this.TextBoxArea.SelectionStart = 0;
                this.TextBoxArea.SelectionLength = 0;

                if (this.DropDownControl.CurrentRow != null && this.DropDownControl.CurrentRow.Height == 0)
                {
                    MyComboBoxTextBoxArea.MoveCurrentRowDown(this.DropDownControl);
                }
            }
        }

        /// <summary>
        /// 初始化MyComboBox，设置默认属性
        /// <list type="bullet">
        /// <item>UIStyle = Xceed.UI.UIStyle.System</item>
        /// <item>BorderStyle = Xceed.Editors.EnhancedBorderStyle.UIStyle</item>
        /// <item>ColumnWidthAdjustment = ColumnWidthAdjustment.None;</item>
        /// <item>IntegralHeight = false</item>
        /// <item>Size = (120,21)</item>
        /// <item>ReadOnly = true</item>
        /// </list>
        /// </summary>
        public MyComboBox()
            : base()
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyComboBox(MyComboBox template)
            : base(template)
        {
            Initialize();

            // this.ValueMember = template.ValueMember;
            this.DisplayMember = template.DisplayMember;
            this.ValidateText = template.ValidateText;
            this.DropDownSize = template.DropDownSize;

            this.m_editTopNv = template.m_editTopNv;
            this.m_filterData = template.m_filterData;

            this.AdjustDropDownControlSize();
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="borderStyle"></param>
        //public MyComboBox(EnhancedBorderStyle borderStyle)
        //    : base(borderStyle)
        //{
        //    Initialize();
        //}
        //

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        internal MyComboBox(Type callerType, EnhancedBorderStyle borderStyle)
            : base(callerType, borderStyle)
        {
            Initialize();
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        ///// <param name="valueMember"></param>
        //public MyComboBox(object dataSource, string dataMember, string valueMember)
        //    : base(dataSource, dataMember, valueMember)
        //{
        //    SetDataBinding(dataSource, dataMember);
        //    Initialize();
        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        ///// <param name="valueMember"></param>
        ///// <param name="displayFormat"></param>
        //public MyComboBox(object dataSource, string dataMember, string valueMember, string displayFormat)
        //    : base(dataSource, dataMember, valueMember, displayFormat)
        //{
        //    SetDataBinding(dataSource, dataMember);
        //    Initialize();
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="valueMember"></param>
        internal MyComboBox(Type callerType, EnhancedBorderStyle borderStyle, object dataSource, string dataMember,
                            string valueMember)
            : base(callerType, borderStyle, dataSource, dataMember, valueMember)
        {
            SetDataBinding(dataSource, dataMember);
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="displayFormat"></param>
        internal MyComboBox(Type callerType, EnhancedBorderStyle borderStyle, object dataSource, string dataMember,
                            string valueMember, string displayFormat)
            : base(callerType, borderStyle, dataSource, dataMember, valueMember, displayFormat)
        {
            SetDataBinding(dataSource, dataMember);
            Initialize();
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        ///// <param name="valueMember"></param>
        ///// <param name="imageMember"></param>
        ///// <param name="imagePosition"></param>
        ///// <param name="imageSize"></param>
        ///// <param name="displayFormat"></param>
        //internal MyComboBox(object dataSource, string dataMember, string valueMember, string imageMember, ImagePosition imagePosition, Size imageSize, string displayFormat)
        //    : base(dataSource, dataMember, valueMember, imageMember, imagePosition, imageSize, displayFormat)
        //{
        //    SetDataBinding(dataSource, dataMember);
        //    Initialize();
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="callerType"></param>
        /// <param name="borderStyle"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="imageMember"></param>
        /// <param name="imagePosition"></param>
        /// <param name="imageSize"></param>
        /// <param name="displayFormat"></param>
        internal MyComboBox(Type callerType, EnhancedBorderStyle borderStyle, object dataSource, string dataMember,
                            string valueMember, string imageMember, ImagePosition imagePosition, Size imageSize,
                            string displayFormat)
            : base(callerType, borderStyle, dataSource, dataMember, valueMember, imageMember, imagePosition, imageSize,
                displayFormat)
        {
            SetDataBinding(dataSource, dataMember);
            Initialize();
        }

        #endregion

        #region "Porperties"

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        public bool ValidateText
        {
            get { return m_bValidateText; }
            set { m_bValidateText = value; }
        }

        private bool m_bValidateText = true;

        private bool m_bReadOnly;

        /// <summary>
        /// 设置ComboBox内置的TextBox的ReadOnly
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return m_bReadOnly; }
            set
            {
                if (m_bReadOnly != value)
                {
                    m_bReadOnly = value;
                    this.TextBoxArea.ReadOnly = value;
                    this.DropDownButton.Enabled = !value;
                    if (ReadOnlyChanged != null)
                    {
                        ReadOnlyChanged(this, System.EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReadOnlyChanged;

        /// <summary>
        /// Default BorderStyle
        /// </summary>
        [DefaultValue(Xceed.Editors.EnhancedBorderStyle.UIStyle)]
        public new Xceed.Editors.EnhancedBorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        /// Default ColumnWidthAdjustment
        /// </summary>
        [DefaultValue(ColumnWidthAdjustment.None)]
        public new ColumnWidthAdjustment ColumnWidthAdjustment
        {
            get { return base.ColumnWidthAdjustment; }
            set { base.ColumnWidthAdjustment = value; }
        }

        /// <summary>
        /// Default IntegralHeight
        /// </summary>
        [DefaultValue(false)]
        public new bool IntegralHeight
        {
            get { return base.IntegralHeight; }
            set { base.IntegralHeight = value; }
        }

        ///// <summary>
        ///// Default AllowFreeText
        ///// </summary>
        //[DefaultValue(false)]
        //public new bool AllowFreeText
        //{
        //    get { return base.AllowFreeText; }
        //    set { base.AllowFreeText = value; }
        //}

        #endregion

        #region "IBindingDataValueControl"

        private string m_displayMember = "";

        /// <summary>
        /// 获取或设置控件的显示栏名
        /// </summary>
        [DefaultValue("")]
        public string DisplayMember
        {
            set
            {
                m_displayMember = value;
                base.DisplayFormat = "%" + value + "%";
            }
            get { return m_displayMember; }
        }

        public static string GetGridNameForNv(string nvName, bool hassOption)
        {
            if (!hassOption)
            {
                return string.Format("GridForNameValueMapping_{0}", nvName);
            }
            else
            {
                return string.Format("GridForNameValueMapping_Option_{0}", nvName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nvcName"></param>
        /// <param name="viewerName"></param>
        /// <param name="editorName"></param>
        /// <param name="editorFilter"></param>
        public void SetDataBinding(string nvcName, string viewerName, string editorName, string editorFilter)
        {
            if (NameValueMappingCollection.Instance[viewerName].ValueMember !=
                NameValueMappingCollection.Instance[editorName].ValueMember)
            {
                throw new ArgumentException(viewerName + "'s ValueMember and " + editorName + "' ValueMember should be same!", "viewerName");
            }
            string editTopNvName = NameValueMappingCollection.Instance.FindTopParentNv(editorName);
            //if (viewerName != editorName)
            //{
            //    string viewTopNvName = NameValueMappingCollection.Instance.FindTopParentNv(viewerName);
            //    if (editTopNvName != viewTopNvName)
            //    {
            //        throw new ArgumentException(viewerName + "'s TopNv and " + editorName + "' TopNv should be same!");
            //    }
            //}

            this.m_nvcName = nvcName;
            this.m_editorNvName = editorName;
            this.m_layoutName = GetGridNameForNv(viewerName + "_" + editorName + "_" + editorFilter, false);

            this.SetDataBinding(NameValueMappingCollection.Instance.GetDataSource(nvcName, viewerName, string.Empty), string.Empty);

            m_editTopNv = NameValueMappingCollection.Instance[editTopNvName];

            m_editTopNv.DataSourceChanged -= new EventHandler(MyComboBox_DataTableChanged);
            m_editTopNv.DataSourceChanged += new EventHandler(MyComboBox_DataTableChanged);
            m_editTopNv.DataSourceChanging -= new System.ComponentModel.CancelEventHandler(MyComboBox_DataTableChanging);
            m_editTopNv.DataSourceChanging += new System.ComponentModel.CancelEventHandler(MyComboBox_DataTableChanging);

            if (viewerName != editorName || !string.IsNullOrEmpty(editorFilter))
            {
                m_needFilter = true;
                m_filterData = NameValueMappingCollection.Instance.GetDataSource(nvcName, editorName, editorFilter);
                FilterEditRow();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NameValueMappingName
        {
            get { return m_editorNvName; }
        }

        private int m_savedSelectedIndex = -1;
        void MyComboBox_DataTableChanging(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.BeginInit();

            m_savedSelectedIndex = this.SelectedIndex;
        }

        private bool m_needFilter;
        private string m_nvcName;
        private string m_editorNvName;
        private NameValueMapping m_editTopNv;
        private System.Data.DataView m_filterData;

        private void FilterEditRow()
        {
            if (m_filterData == null)
            {
                foreach (Xceed.Grid.DataRow row in this.DropDownControl.DataRows)
                {
                    row.ResetHeight();
                    row.ResetReadOnly();
                }
            }
            else
            {

                System.Data.DataView editorData = m_filterData;

                Dictionary<string, bool> dict = new Dictionary<string, bool>();
                foreach (System.Data.DataRowView row in editorData)
                {
                    dict[row[this.ValueMember].ToString()] = true;
                }
                foreach (Xceed.Grid.DataRow row in this.DropDownControl.DataRows)
                {
                    if (!dict.ContainsKey(row.Cells[this.ValueMember].Value.ToString()))
                    {
                        row.Height = 0;
                        row.ReadOnly = true;
                    }
                    else
                    {
                        row.ResetHeight();
                        row.ResetReadOnly();
                    }
                }
            }
        }

        // 当数据源改变时，自动刷新。如果数据源只有一条数据，自动填上
        void MyComboBox_DataTableChanged(object sender, EventArgs e)
        {
            if (this.IsDisposed || this.Parent == null)   // when in cellEditTemplate, || this.Parent == null
            {
                this.EndInit();

                m_editTopNv.DataSourceChanged -= new EventHandler(MyComboBox_DataTableChanged);
                m_editTopNv.DataSourceChanging -= new System.ComponentModel.CancelEventHandler(MyComboBox_DataTableChanging);
                return;
            }

            if (m_needFilter)
            {
                FilterEditRow();
            }

            this.EndInit();

            this.SelectedIndex = m_savedSelectedIndex;
            m_savedSelectedIndex = -1;

            if (this.SelectedIndex == -1)
            {
                System.Data.DataView dv = this.DataSource as System.Data.DataView;
                if (dv != null && dv.Count == 1 && this.DropDownControl.DataRows[0].Height > 0 && !this.DropDownControl.DataRows[0].ReadOnly)
                {
                    IDataControl dc = this.Parent as IDataControl;
                    if (dc != null)
                    {
                        // Raise SelectedDataValueChanged Event
                        dc.SelectedDataValue = dv[0][this.ValueMember];
                    }
                    else
                    {
                        this.SelectedDataValue = dv[0][this.ValueMember];
                    }
                }
                // 不再这里自动设置，因为外面可能会主动设置ReadOnly
                //if (!this.ReadOnly && dt.Rows.Count == 0)
                //{
                //    this.ReadOnly = true;
                //}
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public string EditorMappingName
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public string EditorFilter
        //{
        //    get;
        //    set;
        //}

        public void SelectFirstVisibleItem()
        {
            foreach (Xceed.Grid.DataRow row in this.DropDownControl.DataRows)
            {
                if (row.Height > 0 && !row.ReadOnly)
                {
                    this.SelectedIndex = row.Index;
                }
            }
        }

        /// <summary>
        /// 设置DataSource, 读入数据后重新排列格式
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public new void SetDataBinding(object dataSource, string dataMember)
        {
            object save = null;
            if (this.SelectedIndex != -1)
            {
                save = SelectedValue;
            }

            base.SetDataBinding(dataSource, dataMember);

            if (save != null)
            {
                ((MyComboBoxTextBoxArea) TextBoxArea).OwnTextValidating();
                SelectedValue = save;
                if (this.SelectedIndex == -1)
                {
                    this.TextBoxArea.Text = string.Empty;
                }
            }

            //AdjustDropDownControlSize();
        }

        #endregion


        #region "Style"
        public bool LoadLayout()
        {
            return MyComboBox.LoadLayout(this.DropDownControl, m_layoutName);
        }

        public static bool LoadLayout(GridControl grid, string gridName)
        {
            if (string.IsNullOrEmpty(gridName))
                return false;

            // 在未初始化前，不读入
            if (grid.Columns.Count == 0)
            {
                return false;
            }
            AMS.Profile.IProfile profile = GetProfile(grid.Parent, gridName);
            string sectionName = "ComboGrid.Layout";

            bool ret = true;
            try
            {
                grid.BeginInit();

                string s = profile.GetValue(sectionName, "Column", "");
                if (string.IsNullOrEmpty(s))
                {
                    return false;
                }
                string[] columns = s.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string columnName in columns)
                {
                    string[] ss = columnName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length != 5)
                    {
                        continue;
                    }
                    if (grid.Columns[ss[0]] == null)
                    {
                        continue;
                    }

                    Xceed.Grid.Column column = grid.Columns[ss[0]];
                    // 默认是-1，设置成0是gridcolumnInfo 设置成Invisible
                    if (column != null && column.MaxWidth != 0)
                    {
                        column.Visible = Convert.ToBoolean(ss[1]);
                        column.VisibleIndex = Convert.ToInt32(ss[2]);
                        column.Width = Convert.ToInt32(ss[3]);
                        column.Fixed = Convert.ToBoolean(ss[4]);
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            finally
            {
                grid.EndInit();
            }

            return ret;
        }
        private static AMS.Profile.IProfile GetProfile(Control control, string gridName)
        {
            string fileName = SystemDirectory.UserDataDirectory + "NameValueMappings\\" + gridName + (control.Parent == null ? "_GridEditor" : "_DataControl");
            fileName = fileName.Substring(0, Math.Min(230, fileName.Length)) + ".xml";
            IOHelper.TryCreateDirectory(fileName);

            return new AMS.Profile.Xml(fileName);
        }

        public bool SaveLayout()
        {
            return MyComboBox.SaveLayout(this.DropDownControl, m_layoutName);
        }

        public static bool SaveLayout(GridControl grid, string gridName)
        {
            if (string.IsNullOrEmpty(gridName))
                return false;

            if (grid.Columns.Count == 0)
            {
                return false;
            }

            try
            {
                AMS.Profile.IProfile profile = GetProfile(grid.Parent, gridName);
                string sectionName = "ComboGrid.Layout";

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < grid.Columns.Count; ++i)
                {
                    sb.Append(grid.Columns[i].FieldName);
                    sb.Append(",");
                    sb.Append(grid.Columns[i].Visible);
                    sb.Append(",");
                    sb.Append(grid.Columns[i].VisibleIndex);
                    sb.Append(",");
                    sb.Append(grid.Columns[i].Width);
                    sb.Append(",");
                    sb.Append(grid.Columns[i].Fixed);
                    sb.Append(System.Environment.NewLine);
                }
                profile.SetValue(sectionName, "Column", sb.ToString());
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            return true;
        }

        private string m_layoutName;
        public void AdjustDropDownControlSize()
        {
            if (!this.LoadLayout())
            {
                // Toslow
                int width = GridLayoutExtention.AutoAdjustColumnWidth(this.DropDownControl.Columns, 0);
                this.DropDownSize = new System.Drawing.Size(width + this.DropDownButton.Width + 5, ItemHeight * Math.Max(2, GetVisibleRows()) + 20);
            }
            else
            {
                this.DropDownSize = new System.Drawing.Size(this.DropDownControl.Width + this.DropDownButton.Width + 10, ItemHeight * Math.Max(2, GetVisibleRows()) + 20);
            }

            ////this.DropDownControl.Size = new System.Drawing.Size(width, ItemHeight * Math.Max(2, GetVisibleRows()) + 20);
        }

        /// <summary>
        /// 调整DropDownControl高度
        /// </summary>
        internal void AdjustDropDownControlHeight()
        {
            DropDownSize = new System.Drawing.Size(this.DropDownSize.Width, ItemHeight * Math.Max(2, GetVisibleRows()) + 20);
        }

        /// <summary>
        /// 得到Visible的Row行数
        /// </summary>
        /// <returns></returns>
        internal int GetVisibleRows()
        {
            int cnt = 0;
            foreach (DataRow row in this.DropDownControl.DataRows)
            {
                if (row.Visible && row.Height > 0)
                {
                    cnt++;
                }
            }
            return cnt;
        }

        /// <summary>
        /// Visible or Invisible columns
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="visible"></param>
        public void VisibleColumns(string[] columnNames, bool visible)
        {
            for (int i = 0; i < columnNames.Length; ++i)
            {
                Columns[columnNames[i]].Visible = visible;
            }
        }

        /// <summary>
        /// VisibleColumns
        /// </summary>
        /// <param name="columns"></param>
        public void VisibleColumns(Dictionary<string, bool> columns)
        {
            for (int i = 0; i < this.DropDownControl.Columns.Count; ++i)
            {
                if (columns.ContainsKey(this.DropDownControl.Columns[i].FieldName))
                    this.DropDownControl.Columns[i].Visible = columns[this.DropDownControl.Columns[i].FieldName];
                else
                    this.DropDownControl.Columns[i].Visible = false;
            }
        }
        #endregion

        #region "Override"

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MyComboBox(this);
        }

        /// <summary>
        /// 按照拼音查询
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        internal int FindByPinYin(string pinyin, string columnName)
        {
            GridControl control1 = this.DropDownControl;
            int num1 = control1.Columns.IndexOf(control1.Columns[columnName]);
            if (num1 < 0)
            {
                throw new ArgumentException("The column with the specified column name could not be found.", "columnName");
            }
            ReadOnlyDataRowList list1 = control1.GetSortedDataRows(true);
            int num2 = list1.Count;
            for (int num3 = 0; num3 < num2; num3++)
            {
                Xceed.Grid.DataRow row1 = list1[num3];
                string name = row1.Cells[num1].Value.ToString();

                if (Feng.Windows.Utils.ChineseHelper.IsPinYinofText(pinyin, name))
                {
                    return row1.Index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find(Xceed版本的Find startIndex有问题)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="columnName"></param>
        /// <param name="pattern"></param>
        /// <param name="compareType"></param>
        /// <returns></returns>
        internal int Find(string text, string columnName, SearchPattern pattern, Xceed.Editors.CompareType compareType)
        {
            bool flag1 = (compareType & Xceed.Editors.CompareType.CaseInsensitive) ==
                         Xceed.Editors.CompareType.CaseInsensitive;
            bool flag2 = (compareType & Xceed.Editors.CompareType.AccentInsensitive) ==
                         Xceed.Editors.CompareType.AccentInsensitive;
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            GridControl control1 = this.DropDownControl;
            int num1 = control1.Columns.IndexOf(control1.Columns[columnName]);
            if (num1 < 0)
            {
                throw new ArgumentException("The column with the specified column name could not be found.", "columnName");
            }
            ReadOnlyDataRowList list1 = control1.GetSortedDataRows(true);
            int num2 = list1.Count;
            int num3 = 0;
            Encoding encoding1 = Encoding.GetEncoding(0x4e3);
            switch (pattern)
            {
                case SearchPattern.StartsWith:
                    if (flag2)
                    {
                        text = Encoding.ASCII.GetString(encoding1.GetBytes(text));
                    }
                    for (int num4 = num3; num4 < num2; num4++)
                    {
                        Xceed.Grid.DataRow row1 = list1[num4];
                        object obj1 = row1.Cells[num1].Value;
                        if (obj1 != null)
                        {
                            string text1 = obj1.ToString();
                            if (flag2)
                            {
                                text1 = Encoding.ASCII.GetString(encoding1.GetBytes(text1));
                            }
                            if (string.Compare(text1, 0, text, 0, text.Length, flag1) == 0)
                            {
                                return row1.Index;
                            }
                        }
                    }
                    break;

                case SearchPattern.EndsWith:
                    if (flag2)
                    {
                        text = Encoding.ASCII.GetString(encoding1.GetBytes(text));
                    }
                    for (int num6 = num3; num6 < num2; num6++)
                    {
                        Xceed.Grid.DataRow row3 = list1[num6];
                        object obj3 = row3.Cells[num1].Value;
                        if (obj3 != null)
                        {
                            string text3 = obj3.ToString();
                            if (flag2)
                            {
                                text3 = Encoding.ASCII.GetString(encoding1.GetBytes(text3));
                            }
                            if (string.Compare(text3, 0, text, text3.Length - text.Length, text.Length, flag1) == 0)
                            {
                                return row3.Index;
                            }
                        }
                    }
                    break;

                case SearchPattern.Contains:
                    if (flag2)
                    {
                        text = Encoding.ASCII.GetString(encoding1.GetBytes(text));
                    }
                    if (flag1)
                    {
                        text = text.ToUpper();
                    }
                    for (int num5 = num3; num5 < num2; num5++)
                    {
                        Xceed.Grid.DataRow row2 = list1[num5];
                        object obj2 = row2.Cells[num1].Value;
                        if (obj2 != null)
                        {
                            string text2 = obj2.ToString();
                            if (flag2)
                            {
                                text2 = Encoding.ASCII.GetString(encoding1.GetBytes(text2));
                            }
                            if (flag1)
                            {
                                text2 = text2.ToUpper();
                            }
                            if (text2.IndexOf(text, StringComparison.Ordinal) >= 0)
                            {
                                return row2.Index;
                            }
                        }
                    }
                    break;

                default:
                    if (flag2)
                    {
                        text = Encoding.ASCII.GetString(encoding1.GetBytes(text));
                    }
                    for (int num7 = num3; num7 < num2; num7++)
                    {
                        Xceed.Grid.DataRow row4 = list1[num7];
                        object obj4 = row4.Cells[num1].Value;
                        if (obj4 != null)
                        {
                            string text4 = obj4.ToString();
                            if (flag2)
                            {
                                text4 = Encoding.ASCII.GetString(encoding1.GetBytes(text4));
                            }
                            if (string.Compare(text4, text, flag1) == 0)
                            {
                                return row4.Index;
                            }
                        }
                    }
                    break;
            }
            return -1;
        }

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// 设置ComboBox值时与界面显示对应的内部值
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue
        {
            get
            {
                if (this.SelectedIndex == -1)
                {
                    return null;
                }
                else
                {
                    return this.SelectedValue;
                }
            }
            set
            {
                try
                {
                    if (this.Columns[this.ValueMember] == null)
                    {
                        Text = string.Empty;
                        return;
                    }
                    if (value == null || Items.Count == 0)
                    {
                        if (!string.IsNullOrEmpty(this.ValueMember))
                        {
                            SelectedValue = null;
                        }
                        Text = string.Empty;
                    }
                    else
                    {
                        if (this.Items.Count == 0)
                        {
                            SelectedValue = null;
                        }
                        else
                        {
                            object v = ConvertHelper.TryIntToEnum(value, this.Items[0][ValueMember].GetType());
                            SelectedValue = v;
                        }
                        if (this.SelectedIndex == -1)
                        {
                            //if (this.ReadOnly)
                            //{
                            //    TryLoadNotExitItem(value);
                            //}
                            //else
                            //{
                            Text = string.Empty;
                            //}
                        }
                    }
                }
                catch
                {
                    throw new ArgumentException("MyCombox's SelectedDataValue's type is invalid", "value");
                }
            }
        }

        //private bool TryLoadNotExitItem(object value)
        //{
        //    NameValueMapping nv = NameValueMappingCollection.Instance[this.ViewerMappingName];
        //    string newName = nv.Name;
        //    if (!string.IsNullOrEmpty(nv.ParentName))
        //    {
        //        newName = nv.ParentName;
        //    }
        //    this.ValidateText = false;
        //    this.Text = NameValueMappingCollection.Instance.FindNameFromId(newName, value);
        //    this.ValidateText = true;
        //    return true;
        //}
        #endregion

        #region "IStateControl"

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion

        #region "Create MyComboBoxTextBoxArea"

        /// <summary>
        /// SubClass TextBox in ComboBox
        /// </summary>
        /// <returns></returns>
        protected override Xceed.Editors.TextBoxArea CreateTextBoxArea()
        {
            return new MyComboBoxTextBoxArea();
        }

        ///// <summary>
        ///// CreateDefaultDropDownControl
        ///// </summary>
        ///// <returns></returns>
        //protected override Control CreateDefaultDropDownControl()
        //{
        //    GridControl control = new MyGrid();
        //    control.GridLineColor = Color.Transparent;
        //    control.Name = "DropDownGrid";
        //    return control;
        //}

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void ReloadData()
        {
            if (!this.ReadOnly)
            {
                if (m_editTopNv != null && m_editTopNv.Params.Count == 0)
                {
                    //  一般nvNameEditor是nv.Name的子集。如果更新nvNameEditor，也会更新nv.Name
                    //NameValueMappingCollection.Instance.Reload(nv.Name);
                    m_editTopNv.Reload(m_nvcName);

                    //gridControl.VisibleColumns(nv.MemberVisible);
                    this.AdjustDropDownControlHeight();
                }
            }
        }

        internal void OnOwnDoubleClick(System.EventArgs e)
        {
            base.OnDoubleClick(e);

            ReloadData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal void OnOwnKeyPress(KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        #endregion
    }

    #region "MyComboBoxTextBoxArea"

    /// <summary>
    /// ComboBox内置TextBox，用于处理Key和MouseWheel事件
    /// </summary>
    public class MyComboBoxTextBoxArea : Xceed.Editors.ComboBoxTextBoxArea
    {
        /// <summary>OBSOLETE: The ComboBoxTextBoxArea( WinComboBox )
        /// constructor is obsolete and has been replaced by the
        /// ComboBoxTextBoxArea() constructor. Initializes a new instance
        /// of the ComboBoxTextBoxArea class specifying the WinComboBox to
        /// associate it to.</summary>
        public MyComboBoxTextBoxArea() : base()
        {
            base.CausesValidation = true;
        }

        /// <summary>Initializes a new instance of the ComboBoxTextBoxArea class
        /// specifying the ComboBoxTextBoxArea control that will be used as a
        /// template.</summary>
        /// <remarks>    If the Clone method is used, all property values will be
        /// cloned however event handlers will not.</remarks>
        /// <param name="template">The ComboBoxTextBoxArea to use as a template.</param>
        protected MyComboBoxTextBoxArea(MyComboBoxTextBoxArea template)
            : base(template)
        {
            base.CausesValidation = true;
        }

        /// <summary>Creates a shallow copy of the control.</summary>
        /// <returns>An Object representing the shallow copy of the
        /// control.</returns>
        /// <remarks>    All property values will be cloned however event handlers
        /// will not.</remarks>
        public override object Clone()
        {
            return new MyComboBoxTextBoxArea(this);
        }

        /// <summary>
        /// 重载KeyPress事件，当ComboBox为ReadOnly时，不处理KeyPress事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            MyComboBox box1 = ParentComboBox;
            box1.OnOwnKeyPress(e);

            if (!box1.ReadOnly)
            {
                try
                {
                    base.OnKeyPress(e);
                }// unknown exception in base.OnKeyPress(e);
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                }

                if (base.IsInputChar(e.KeyChar))
                {
                    if (e.KeyChar == (char) Keys.Enter
                        || e.KeyChar == (char) Keys.Escape)
                    {
                        return;
                    }

                    OwnTextChanged();

                    if (box1.GetVisibleRows() > 0)
                    {
                        box1.DroppedDown = true;
                    }
                    else
                    {
                        box1.DroppedDown = false;
                    }
                }
            }
        }


        /// <summary>
        /// OnKeyUp
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            MyComboBox box1 = ParentComboBox;
            if (!box1.ReadOnly)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        OwnTextChanged();
                        break;
                    default:
                        base.OnKeyUp(e);
                        break;
                }
            }
        }

        /// <summary>
        /// OnKeyDown(Process Down Key)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control
                && (e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X))
            {
                base.OnKeyDown(e);
                return;
            }

            try
            {
                MyComboBox box1 = ParentComboBox;
                if (!box1.ReadOnly)
                {
                    GridControl grid = box1.DropDownControl;

                    int idx;
                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            if (!box1.DroppedDown)
                            {
                                box1.DroppedDown = true;
                            }
                            idx = MoveCurrentRowDown(grid);
                            this.ParentComboBox.SelectedIndex = idx;
                            e.Handled = true;
                            break;
                        case Keys.Up:
                            idx = MoveCurrentRowUp(grid);
                            this.ParentComboBox.SelectedIndex = idx;
                            e.Handled = true;
                            break;
                        case Keys.Enter:
                            if (box1.DroppedDown)
                            {
                                box1.DroppedDown = false;
                            }
                            e.Handled = true;
                            break;
                        case Keys.Escape:
                            if (box1.DroppedDown)
                            {
                                box1.DroppedDown = false;
                            }
                            e.Handled = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }


        /// <summary>
        /// 重载OnMouseWheel事件，当ComboBox为ReadOnly时，不处理OnMouseWheel事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            MyComboBox box1 = ParentComboBox;
            if (!box1.ReadOnly)
            {
                int idx;
                GridControl grid = box1.DropDownControl;
                if (e.Delta < 0)
                {
                    idx = MoveCurrentRowDown(grid);
                    this.ParentComboBox.SelectedIndex = idx;
                }
                else if (e.Delta > 0)
                {
                    idx = MoveCurrentRowUp(grid);
                    this.ParentComboBox.SelectedIndex = idx;
                }
            }
        }

        public static int MoveCurrentRowDown(GridControl grid)
        {
            if (grid.DataRows.Count == 0)
                return -1;

            int idx = -1;
            if (!(grid.CurrentRow is Xceed.Grid.DataRow))
            {
                idx = grid.DataRows.Count - 1;
            }
            else
            {
                idx = (grid.CurrentRow as Xceed.Grid.DataRow).Index;
            }

            // skip height=0 DataRow
            Xceed.Grid.DataRow dataRow = null;
            int haveGo = 0;
            while (true)
            {
                if (idx == grid.DataRows.Count - 1)
                {
                    idx = 0;
                    haveGo++;
                }
                else
                {
                    idx++;
                }
                dataRow = grid.DataRows[idx];
                if (dataRow.Visible && dataRow.Height > 0 || haveGo >= 2) 
                    break;
            }
            if (dataRow != null)
            {
                grid.CurrentRow = dataRow;
            }
            return idx;
        }
        public static int MoveCurrentRowUp(GridControl grid)
        {
            if (grid.DataRows.Count == 0)
                return -1;

            int idx = -1;
            if (!(grid.CurrentRow is Xceed.Grid.DataRow))
            {
                idx = 0;
            }
            else
            {
                idx = (grid.CurrentRow as Xceed.Grid.DataRow).Index;
            }

            // skip height=0 DataRow
            Xceed.Grid.DataRow dataRow = null;
            int haveGo = 0;
            while (true)
            {
                if (idx == 0)
                {
                    idx = grid.DataRows.Count - 1;
                }
                else
                {
                    idx--;
                }
                dataRow = grid.DataRows[idx];
                if (dataRow.Visible && dataRow.Height > 0 || haveGo >= 2)
                    break;
            }
            if (dataRow != null)
            {
                grid.CurrentRow = dataRow;
            }
            return idx;
        }

        /// <summary>
        /// 重载OnDoubleClick事件，当ComboBox为ReadOnly时，不处理OnDoubleClick事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDoubleClick(EventArgs e)
        {
            MyComboBox box1 = ParentComboBox;
            if (box1 != null && !box1.ReadOnly)
            {
                base.OnDoubleClick(e);
            }
            box1.OnOwnDoubleClick(e);
        }

        /// <summary>
        /// OnValidating
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            base.OnValidating(e);

            OwnTextValidating();
        }

        /// <summary>
        /// Parent MyComboBox
        /// </summary>
        private MyComboBox ParentComboBox
        {
            get { return base.Parent as MyComboBox; }
        }

        /// <summary>
        /// 检查当前选中的是否是列表项，如果是，则不在Validate
        /// </summary>
        /// <returns></returns>
        private bool IsSelectedInItems()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return false;
            }

            MyComboBox box1 = ParentComboBox;

            if (box1.SelectedItem != null)
            {
                foreach (Xceed.Editors.ColumnInfo column in box1.Columns)
                {
                    if (Text == box1.SelectedItem[column.Name].ToString())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SetIndexWhichIsVisible(int idx)
        {
            MyComboBox box1 = ParentComboBox;
            if (box1.DropDownControl.DataRows[idx].Height > 0)
            {
                box1.SelectedIndex = idx;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 检查出入Text是否在ComboBox Item里面
        /// </summary>
        internal void OwnTextValidating()
        {
            MyComboBox box1 = ParentComboBox;
            foreach (DataRow row in box1.DropDownControl.DataRows)
            {
                row.Visible = true;
            }
            box1.AdjustDropDownControlHeight();

            if (!box1.ValidateText)
            {
                return;
            }

            if (string.IsNullOrEmpty(Text))
            {
                box1.SelectedIndex = -1;
                return;
            }

            if (box1.SelectedIndex != -1
                && box1.DropDownControl.DataRows[box1.SelectedIndex].Height > 0
                && IsSelectedInItems())
            {
                return;
            }

            if (box1.Items.Count > 0)
            {
                // Exact Find in everyColumn
                foreach (Xceed.Editors.ColumnInfo column in box1.Columns)
                {
                    if (!column.Visible)
                    {
                        continue;
                    }
                    int nIndex = box1.Find(Text, column.Name, WinComboBox.SearchPattern.Exact, CompareType.ExactMatch);
                    if (nIndex != -1)
                    {
                        if (SetIndexWhichIsVisible(nIndex))
                        {
                            return;
                        }
                    }
                }

                // Contains Find in everyColumn
                foreach (Xceed.Editors.ColumnInfo column in box1.Columns)
                {
                    if (!column.Visible)
                    {
                        continue;
                    }
                    int nIndex = box1.Find(Text, column.Name, WinComboBox.SearchPattern.Contains,
                                           CompareType.CaseInsensitive);
                    if (nIndex != -1)
                    {
                        if (SetIndexWhichIsVisible(nIndex))
                        {
                            return;
                        }
                    }
                }

                // Pinyin Find in everyColumn
                foreach (Xceed.Editors.ColumnInfo column in box1.Columns)
                {
                    if (!column.Visible)
                    {
                        continue;
                    }
                    int nIndex = box1.FindByPinYin(Text, column.Name);
                    if (nIndex != -1)
                    {
                        if (SetIndexWhichIsVisible(nIndex))
                        {
                            return;
                        }
                    }
                }
            }

            Text = "";
            box1.SelectedIndex = -1;
        }

        /// <summary>
        /// 检查InputText是否包含在text里面（包括拼音）
        /// </summary>
        /// <param name="itemText"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static bool ContainsTextOrPinYin(string itemText, string text)
        {
            // 如果Input是""，则默认是包含
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            if (string.IsNullOrEmpty(itemText))
            {
                return false;
            }

            string realText = text.Replace("%", string.Empty);
            if (itemText.Contains(realText, StringComparison.Ordinal))
            {
                return true;
            }

            return Feng.Windows.Utils.ChineseHelper.IsPinYinofText(text, itemText);
        }

        internal void OwnTextChanged()
        {
            MyComboBox box1 = ParentComboBox;
            GridControl grid = box1.DropDownControl;

            OwnTextChanged(grid, this.Text);

            box1.SelectedIndex = -1;
            box1.AdjustDropDownControlHeight();
        }

        /// <summary>
        /// 当输入时，自动筛选列表项目
        /// </summary>
        public static void OwnTextChanged(GridControl grid, string text)
        {
            DataRow currentRowInvisible = null;
            bool setCurrentRow = false;

            try
            {
                grid.BeginInit();

                foreach (DataRow row in grid.DataRows)
                {
                    bool vis = false;
                    foreach (DataCell cell in row.Cells)
                    {
                        if (!cell.ParentColumn.Visible)
                        {
                            continue;
                        }
                        if (cell.Value == null || cell.Value == cell.NullValue)
                        {
                            continue;
                        }
                        if (ContainsTextOrPinYin(cell.GetDisplayText(), text))
                        {
                            vis = true;
                            break;
                        }
                    }
                    if (!vis && row == grid.CurrentRow)
                    {
                        currentRowInvisible = row;
                        continue;
                    }

                    row.Visible = vis;
                    if (vis && !setCurrentRow)
                    {
                        //currentRowVisible = row;
                        setCurrentRow = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            finally
            {
                grid.EndInit();
            }

            grid.CurrentRow = null;

            if (currentRowInvisible != null)
            {
                currentRowInvisible.Visible = false;
            }
        }
    }

    #endregion
}