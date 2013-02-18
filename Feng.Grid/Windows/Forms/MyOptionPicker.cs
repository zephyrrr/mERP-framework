using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid;
using Xceed.Editors;
using Feng.Grid;
using Feng.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 选项集合。 例如选中811，812，而实际字段值是811，812，则都能找到。
    /// 注意：字段值不能相互包含，例如1-》A，11-》B，这样找A的时候也能找出B。
    /// 关于IMultiDataValueControl：以SelectedDataValue为准，以Text为准。
    /// DropdownControl Open的时候按照Text设置DropdownControl.Value，Close的时候按照DropdownControl.Value设置Text。
    /// 取SelectedDataValues的时候按照SelectedDataValue取值
    /// </summary>
    public partial class MyOptionPicker : Xceed.Editors.WinTextBox, INameValueMappingBindingControl, IMultiDataValueControl, IGridDropdownControl, ILayoutControl
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
        //// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_editTopNv != null)
                {
                    m_editTopNv.DataSourceChanged -= new EventHandler(MyComboBox_DataTableChanged);
                    m_editTopNv.DataSourceChanging -= new System.ComponentModel.CancelEventHandler(MyComboBox_DataTableChanging);
                }

                //this.dropDownGrid.DataRowTemplate.Cells[FilterGrid.SelectCaption].ValueChanged -= new EventHandler(FilterGridSelectColumn_ValueChanged);
                this.DroppedDownChanged -= new EventHandler(OptionWinTextBox_DroppedDownChanged);
                this.Enter -= new EventHandler(MyOptionPicker_Enter);
                this.Leave -= new EventHandler(MyOptionPicker_Leave);

                this.dropDownGrid.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        public MyOptionPicker()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);

            this.DropDownAnchor = DropDownAnchor.Right;
            this.DropDownDirection = DropDownDirection.Automatic;

            //m_toolTip = new MyToolTip();

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);

            this.dropDownGrid.SingleClickEdit = true;
            this.dropDownGrid.GridLineColor = Color.Transparent;

            this.dropDownGrid.DataRowTemplate.Cells[this.dropDownGrid.SelectColumnName].ValueChanged += new EventHandler(FilterGridSelectColumn_ValueChanged);

            this.Enter += new EventHandler(MyOptionPicker_Enter);
            this.Leave += new EventHandler(MyOptionPicker_Leave);

            this.dropDownGrid.ResetReadOnly();
            this.TextBoxArea.ReadOnly = false;

            this.DroppedDownChanged += new EventHandler(OptionWinTextBox_DroppedDownChanged);
        }


        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MyOptionPicker(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyOptionPicker(MyOptionPicker template)
            : base(template)
        {
            InitializeComponent();

            Initialize();

            this.ValueMember = template.ValueMember;
            this.DisplayMember = template.DisplayMember;
            this.DropDownSize = template.DropDownSize;
            this.DropDownControl.SetDataBinding(template.DropDownControl.DataSource, template.DropDownControl.DataMember);

            this.AdjustDropDownControlSize();
        }

        private void MyOptionPicker_Enter(object sender, EventArgs e)
        {
            FilterEditRow();

            AdjustDropDownControlHeight();
        }

        private void SetDopdownControlValue()
        {
            this.dropDownGrid.EndEditCurrentDataRow();

            if (string.IsNullOrEmpty(Text))
            {
                foreach (DataRow row in this.DropDownControl.DataRows)
                {
                    row.Cells[this.DropDownControl.SelectColumnName].Value = false;
                }
                return;
            }

            string[] ss = this.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length > 0)
            {
                for (int i = 0; i < this.DropDownControl.DataRows.Count; i++)
                {
                    string s1 = this.DropDownControl.DataRows[i].Cells[this.ValueMember].Value.ToString();
                    string s2 = this.DropDownControl.DataRows[i].Cells[this.DisplayMember].Value.ToString();
                    bool have = false;
                    for (int j = 0; j < ss.Length; ++j)
                    {
                        if (ss[j].Trim() == s1 || ss[j].Trim() == s2)
                        {
                            have = true;
                            break;
                        }
                    }
                    this.DropDownControl.DataRows[i].Cells[this.DropDownControl.SelectColumnName].Value = have;
                    if (have)
                    {
                        this.DropDownControl.DataRows[i].ResetHeight();
                    }
                }
            }
        }

        private void MyOptionPicker_Leave(object sender, EventArgs e)
        {
            //m_userClick = false;

            //CalculateText();

            //m_userClick = true;
        }

        private void OptionWinTextBox_DroppedDownChanged(object sender, EventArgs e)
        {
            if (!this.DroppedDown)
            {
                if (this.dropDownGrid.CurrentCell != null
                    && this.dropDownGrid.CurrentCell.IsBeingEdited)
                {
                    this.dropDownGrid.CurrentCell.LeaveEdit(true);
                }

                CalculateText();
            }
            else
            {
                SetDopdownControlValue();
            }
        }

        /// <summary>
        /// DropDownControl
        /// </summary>
        public new DropdownOptionGrid DropDownControl
        {
            get { return base.DropDownControl as DropdownOptionGrid; }
            set { base.DropDownControl = value; }
        }

        #endregion

        #region "ReadOnly"

        private bool m_bReadOnly;

        /// <summary>
        /// 获取或设置只读属性
        /// </summary>
        [Category("Data")]
        [DefaultValue(false)]
        [Description("设置是否只读")]
        public bool ReadOnly
        {
            get { return m_bReadOnly; }
            set
            {
                if (m_bReadOnly != value)
                {
                    m_bReadOnly = value;

                    this.dropDownGrid.ReadOnly = value;
                    this.TextBoxArea.ReadOnly = value;
                    //this.dropDownButton1.Enabled = !value;
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
        #endregion

        #region "CalculateText"

        //private MyToolTip m_toolTip;

        //internal bool m_userClick = true;
        private void FilterGridSelectColumn_ValueChanged(object sender, EventArgs e)
        {
            //if (m_userClick)
            //{
            //    CalculateText();
            //}
        }

        private void CalculateText()
        {
            //this.TextBoxArea.Text = string.Empty;
            bool atLeastOneOption = false;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.dropDownGrid.DataRows.Count; i++)
            {
                if (Convert.ToBoolean(this.dropDownGrid.DataRows[i].Cells[this.dropDownGrid.SelectColumnName].Value))
                {
                    if (atLeastOneOption)
                    {
                        sb.Append("," + this.dropDownGrid.DataRows[i].Cells[DisplayMember].Value.ToString());
                    }
                    else
                    {
                        sb.Append(this.dropDownGrid.DataRows[i].Cells[DisplayMember].Value.ToString());
                    }
                    atLeastOneOption = true;
                }
            }

            this.TextBoxArea.Text = sb.ToString(); // m_toolTip.GetToolTip(this.TextBoxArea);

            //if (sb.Length > 0)
            //{
            //    m_toolTip.SetToolTip(this.TextBoxArea, sb.ToString());
            //    //this.TextBoxArea.BackColor = SystemColors.ControlDark;
            //}
            //else
            //{
            //    m_toolTip.SetToolTip(this.TextBoxArea, null);
            //    //this.TextBoxArea.BackColor = Color.White;
            //}
            this.TextBoxArea.Select(sb.Length, 0);
        }

        #endregion

        #region "IBindingDataValueControl"

        private string m_valueMember;

        /// <summary>
        /// ValueMember
        /// </summary>
        public string ValueMember
        {
            get { return m_valueMember; }
            set { m_valueMember = value; }
        }

        private string m_displayMember;

        /// <summary>
        /// DisplayMember
        /// </summary>
        public string DisplayMember
        {
            get { return m_displayMember; }
            set { m_displayMember = value; }
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
            this.m_layoutName = MyComboBox.GetGridNameForNv(viewerName + "_" + editorName + "_" + editorFilter, true);

            this.SetDataBinding(NameValueMappingCollection.Instance.GetDataSource(nvcName, viewerName, string.Empty), string.Empty);

            m_editTopNv = NameValueMappingCollection.Instance[editTopNvName];

            // more call SetDataBinding
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

        private bool m_needFilter;
        private string m_nvcName;
        private string m_editorNvName;
        private NameValueMapping m_editTopNv;
        private System.Data.DataView m_filterData;

        private IList m_savedSelectedIndex = null;
        void MyComboBox_DataTableChanging(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DropDownControl.BeginInit();

            m_savedSelectedIndex = this.SelectedDataValues;
        }

        private void FilterEditRow()
        {
            if (m_filterData == null)
            {
                foreach (Xceed.Grid.DataRow row in this.dropDownGrid.DataRows)
                {
                    row.ResetHeight();
                    //row.ResetReadOnly();
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
                foreach (Xceed.Grid.DataRow row in this.dropDownGrid.DataRows)
                {
                    if (!dict.ContainsKey(row.Cells[this.ValueMember].Value.ToString()))
                    {
                        row.Height = 0;
                        //row.ReadOnly = true;
                    }
                    else
                    {
                        row.ResetHeight();
                        //row.ResetReadOnly();
                    }
                }
            }
        }

        void MyComboBox_DataTableChanged(object sender, EventArgs e)
        {
            if (this.IsDisposed || this.Parent == null)
            {
                this.DropDownControl.EndInit();

                m_editTopNv.DataSourceChanged -= new EventHandler(MyComboBox_DataTableChanged);
                m_editTopNv.DataSourceChanging -= new System.ComponentModel.CancelEventHandler(MyComboBox_DataTableChanging);
                return;
            }

            if (m_needFilter)
            {
                FilterEditRow();
            }

            this.DropDownControl.EndInit();

            if (m_savedSelectedIndex != null)
            {
                this.SelectedDataValues = m_savedSelectedIndex;
                m_savedSelectedIndex = null;
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

        /// <summary>
        /// 与数据库绑定，如果手动设置过Item，会清空
        /// </summary>
        /// <param name="dataSource">DataSource</param>
        /// <param name="dataMember">DataMember</param>
        public void SetDataBinding(object dataSource, string dataMember)
        {
            //m_userClick = false;

            this.dropDownGrid.SetDataBinding(dataSource, dataMember);

            for (int i = 0; i < this.dropDownGrid.DataRows.Count; i++)
            {
                this.dropDownGrid.DataRows[i].Cells[this.dropDownGrid.SelectColumnName].Value = false;
            }

            foreach (Xceed.Grid.Column c in this.dropDownGrid.Columns)
            {
                if (c.FieldName != this.dropDownGrid.SelectColumnName)
                {
                    c.ReadOnly = true;
                }
            }

            //AdjustDropDownControlSize();

            //m_userClick = true;
        }

        public bool SaveLayout()
        {
            return MyComboBox.SaveLayout(this.DropDownControl, m_layoutName);
        }
        public bool LoadLayout()
        {
            return MyComboBox.LoadLayout(this.DropDownControl, m_layoutName);
        }

        private string m_layoutName;
        /// <summary>
        /// 
        /// </summary>
        public void AdjustDropDownControlSize()
        {
            if (!this.LoadLayout())
            {
                // Toslow
                int width = this.dropDownGrid.AutoAdjustColumnWidth();
                this.DropDownSize = new System.Drawing.Size(width + this.DropDownButton.Width + 5, 20 * Math.Max(2, GetVisibleRows()) + 20);
            }
            else
            {
                this.DropDownSize = new System.Drawing.Size(this.DropDownControl.Width + this.DropDownButton.Width + 10, 20 * Math.Max(2, GetVisibleRows()) + 20);
            }
        }

        /// <summary>
        /// 调整DropDownControl高度
        /// </summary>
        internal void AdjustDropDownControlHeight()
        {
            this.DropDownSize = new System.Drawing.Size(this.DropDownSize.Width, 20 * Math.Max(2, GetVisibleRows()) + 30);
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

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// 以数组形式表示的SelectedDataValue
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedDataValues
        {
            get
            {
                ArrayList ret = new ArrayList();
                for (int i = 0; i < this.dropDownGrid.DataRows.Count; i++)
                {
                    if (Convert.ToBoolean(this.dropDownGrid.DataRows[i].Cells[this.dropDownGrid.SelectColumnName].Value))
                    {
                        ret.Add(this.dropDownGrid.DataRows[i].Cells[ValueMember].Value);
                    }
                }
                return ret;
            }
            set
            {
                //m_userClick = false;

                if (value == null || value.Count == 0 || this.dropDownGrid.DataRows.Count == 0)
                {
                    for (int i = 0; i < this.dropDownGrid.DataRows.Count; i++)
                    {
                        this.dropDownGrid.DataRows[i].Cells[this.dropDownGrid.SelectColumnName].Value = false;
                    }
                }
                else
                {
                    try
                    {
                        for (int i = 0; i < this.dropDownGrid.DataRows.Count; i++)
                        {
                            string s = this.dropDownGrid.DataRows[i].Cells[ValueMember].Value.ToString();
                            bool have = false;
                            foreach (object j in value)
                            {
                                if (j.ToString() == s)
                                {
                                    have = true;
                                    break;
                                }
                            }
                            this.dropDownGrid.DataRows[i].Cells[this.dropDownGrid.SelectColumnName].Value = have;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("OptionPicker's SelectedDataValue must be object[]", ex);
                    }
                }

                // show text
                CalculateText();

                //m_userClick = true;
            }
        }

        /// <summary>
        /// 设置OptionPicker时对应的值的组合
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                IList ret = SelectedDataValues;
                foreach (object item in ret)
                {
                    sb.Append(item.ToString());
                    sb.Append(',');
                }
                return sb.Length == 0 ? null : sb.ToString();
            }
            set
            {
                if (value == null)
                {
                    SelectedDataValues = null;
                }
                else
                {
                    try
                    {
                        string[] ss = ((string)value).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < ss.Length; ++i)
                        {
                            ss[i] = ss[i].ToString().Trim();
                        }
                        SelectedDataValues = new ArrayList(ss);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("OptionPicker's SelectedDataValue must be string", ex);
                    }
                }
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override TextBoxArea CreateTextBoxArea()
        {
            return new MyOptionPickerTextBoxArea();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal void OnOwnKeyPress(KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void ReloadData()
        {
            if (!this.ReadOnly)
            {
                if (m_editTopNv != null)
                {
                    //  一般nvNameEditor是nv.Name的子集。如果更新nvNameEditor，也会更新nv.Name
                    //NameValueMappingCollection.Instance.Reload(nv.Name);
                    m_editTopNv.Reload(m_nvcName);

                    //gridControl.VisibleColumns(nv.MemberVisible);
                    this.AdjustDropDownControlHeight();
                }
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

            this.DropDownControl.Columns[this.dropDownGrid.SelectColumnName].Visible = true;
        }
    }

    #region "MyOptionPickerTextBoxArea"

    /// <summary>
    /// ComboBox内置TextBox，用于处理Key和MouseWheel事件
    /// </summary>
    internal class MyOptionPickerTextBoxArea : Xceed.Editors.TextBoxArea
    {
        /// <summary>OBSOLETE: The ComboBoxTextBoxArea( WinComboBox )
        /// constructor is obsolete and has been replaced by the
        /// ComboBoxTextBoxArea() constructor. Initializes a new instance
        /// of the ComboBoxTextBoxArea class specifying the WinComboBox to
        /// associate it to.</summary>
        public MyOptionPickerTextBoxArea()
            : base()
        {
            base.CausesValidation = true;
        }

        /// <summary>Initializes a new instance of the ComboBoxTextBoxArea class
        /// specifying the ComboBoxTextBoxArea control that will be used as a
        /// template.</summary>
        /// <remarks>    If the Clone method is used, all property values will be
        /// cloned however event handlers will not.</remarks>
        /// <param name="template">The ComboBoxTextBoxArea to use as a template.</param>
        protected MyOptionPickerTextBoxArea(MyOptionPickerTextBoxArea template)
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
            return new MyOptionPickerTextBoxArea(this);
        }

        /// <summary>
        /// 重载KeyPress事件，当ComboBox为ReadOnly时，不处理KeyPress事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            MyOptionPicker box1 = this.ParentOptionPicker;
            box1.OnOwnKeyPress(e);

            if (!this.ReadOnly)
            {
                base.OnKeyPress(e);

                if (base.IsInputChar(e.KeyChar))
                {
                    if (e.KeyChar == (char) Keys.Enter)
                    {
                        return;
                    }

                    OwnTextChanged();
                }
            }
        }


        /// <summary>
        /// OnKeyUp
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!this.ReadOnly)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        OwnTextChanged();
                        break;
                }
                base.OnKeyUp(e);
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
                MyOptionPicker box1 = ParentOptionPicker;
                if (!box1.ReadOnly)
                {
                    GridControl grid = box1.DropDownControl;

                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                            if (!box1.DroppedDown)
                            {
                                box1.DroppedDown = true;
                            }
                            MyComboBoxTextBoxArea.MoveCurrentRowDown(grid);
                            e.Handled = true;
                            if (!grid.Focused)
                            {
                                grid.Focus();
                            }
                            break;
                        case Keys.Up:
                            if (box1.DroppedDown)
                            {
                                MyComboBoxTextBoxArea.MoveCurrentRowUp(grid);
                                e.Handled = true;
                            }
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


        private void OwnTextChanged()
        {
            //this.ParentOptionPicker.m_userClick = false;

            if (!string.IsNullOrEmpty(this.Text))
            {
                string[] ss = this.Text.Split(new char[] { ',' });
                if (ss.Length > 0)
                {
                    for (int i = 0; i < this.ParentOptionPicker.DropDownControl.DataRows.Count; i++)
                    {
                        string s1 = this.ParentOptionPicker.DropDownControl.DataRows[i].Cells[this.ParentOptionPicker.ValueMember].Value.ToString();
                        string s2 = this.ParentOptionPicker.DropDownControl.DataRows[i].Cells[this.ParentOptionPicker.DisplayMember].Value.ToString();
                        bool have = false;
                        for (int j = 0; j < ss.Length - 1; ++j)
                        {
                            if (ss[j].Trim() == s1 || ss[j].Trim() == s2)
                            {
                                have = true;
                                break;
                            }
                        }
                        this.ParentOptionPicker.DropDownControl.DataRows[i].Cells[this.ParentOptionPicker.DropDownControl.SelectColumnName].Value = have;
                    }

                    MyComboBoxTextBoxArea.OwnTextChanged(this.ParentOptionPicker.DropDownControl, ss[ss.Length - 1].Trim());
                }
            }
            else
            {
                foreach (DataRow row in this.ParentOptionPicker.DropDownControl.DataRows)
                {
                    row.Visible = true;
                }
            }

            ParentOptionPicker.AdjustDropDownControlHeight();

            int rowCount = ParentOptionPicker.GetVisibleRows();
            if (rowCount > 0)
            {
                this.ParentOptionPicker.OpenDropDown();
            }
            else
            {
                this.ParentOptionPicker.CloseDropDown();
            }

            //this.ParentOptionPicker.m_userClick = true;
        }

        /// <summary>
        /// OnValidating
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            base.OnValidating(e);

            //OwnTextValidating();
        }

        /// <summary>
        /// Parent MyOptionPicker
        /// </summary>
        private MyOptionPicker ParentOptionPicker
        {
            get { return this.Parent as MyOptionPicker; }
        }

    }

    #endregion
}