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
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MyObjectPicker : Xceed.Editors.WinTextBox, IDataValueControl, ILayoutControl
    {
        #region "Constructor"
        //// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                //this.dropDownGrid.DataRowTemplate.Cells[FilterGrid.SelectCaption].ValueChanged -= new EventHandler(FilterGridSelectColumn_ValueChanged);
                this.DroppedDownChanged -= new EventHandler(OptionWinTextBox_DroppedDownChanged);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        public MyObjectPicker()
        {
            InitializeComponent();
            Initialize();
        }

        protected override void OnDropDownOpening(System.ComponentModel.CancelEventArgs e)
        {
            MyDatePickerXceed.RelocateDropdownAnchir(this);

            base.OnDropDownOpening(e);
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MyObjectPicker(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template"></param>
        protected MyObjectPicker(MyObjectPicker template)
            : base(template)
        {
            InitializeComponent();

            Initialize();

            this.DisplayMember = template.DisplayMember;
            this.DropDownSize = template.DropDownSize;

            IDisplayManager dmMaster = template.DropDownControl.DisplayManager.Clone() as IDisplayManager;
            dmMaster.BindingControls.Clear();
            dmMaster.BindingControls.Add(this.dropDownGrid);
            this.DropDownControl.SetDisplayManager(dmMaster, template.DropDownControl.GridName);
            //this.DropDownControl.SetDataBinding(template.DropDownControl.DataSource, template.DropDownControl.DataMember);
        }

        private void Initialize()
        {
            XceedUtility.SetUIStyle(this);

            this.DropDownAnchor = DropDownAnchor.Right;
            this.DropDownSize = new System.Drawing.Size(400, 300);

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);

            this.DroppedDownChanged += new EventHandler(OptionWinTextBox_DroppedDownChanged);

            this.dropDownGrid.ReadOnly = true;
            this.TextBoxArea.ReadOnly = false;
            this.dropDownGrid.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

            this.dropDownGrid.MouseDoubleClick += new MouseEventHandler(dropDownGrid_MouseDoubleClick);
            this.TextBoxArea.Validating += new System.ComponentModel.CancelEventHandler(TextBoxArea_Validating);
            this.TextBoxArea.DoubleClick += new EventHandler(TextBoxArea_DoubleClick);
        }

        void TextBoxArea_DoubleClick(object sender, EventArgs e)
        {
            this.DroppedDown = false;
            m_firstLoad = false;
            this.dropDownGrid.DataRows.Clear();
        }

        void TextBoxArea_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            if (string.IsNullOrEmpty(this.TextBoxArea.Text))
            {
                this.SelectedDataValue = null;
            }
            else
            {
                SetText();
            }
        }

        private void SetText()
        {
            if (m_selectedObject != null)
            {
                this.TextBoxArea.Text = EntityHelper.ReplaceEntity(DisplayMember, m_selectedObject, null);
            }
            else
            {
                this.TextBoxArea.Text = null;
            }
        }
        void dropDownGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SaveSelected();
            this.DroppedDown = false;
        }

        private void SaveSelected()
        {
            if (!m_bReadOnly)
            {
                if (this.dropDownGrid.CurrentCell != null
                    && this.dropDownGrid.CurrentCell.IsBeingEdited)
                {
                    this.dropDownGrid.CurrentCell.LeaveEdit(true);
                }

                if (this.dropDownGrid.CurrentRow != null)
                {
                    this.SelectedDataValue = this.dropDownGrid.CurrentRow.Tag;
                }
                else
                {
                    this.SelectedDataValue = null;
                }
            }
        }

        private bool m_firstLoad;
        private void OptionWinTextBox_DroppedDownChanged(object sender, EventArgs e)
        {
            if (!this.DroppedDown)
            {
                
            }
            else
            {
                // 如果读取过，不再读取
                if (!m_firstLoad && this.dropDownGrid.DataRows.Count == 0)
                {
                    this.dropDownGrid.DisplayManager.SearchManager.EnablePage = false;

                    if (!string.IsNullOrEmpty(this.SearchExpressionParam))
                    {
                        throw new NotSupportedException("not supported of SearchExpressionParam in MyOptionPicker!");
                        //string exp = EntityHelper.ReplaceEntity(this.SearchExpressionParam, null);
                        //this.dropDownGrid.DisplayManager.SearchManager.LoadData(SearchExpression.Parse(exp), null);
                    }
                    else
                    {
                        this.dropDownGrid.DisplayManager.SearchManager.LoadDataAccordSearchControls();
                    }
                }
                m_firstLoad = true;

                AdjustDropDownControlSize();

                if (this.SelectedDataValue == null)
                {
                    this.dropDownGrid.CurrentRow = null;
                }
                else
                {
                    foreach (Xceed.Grid.DataRow row in this.dropDownGrid.DataRows)
                    {
                        if (Feng.Utils.ReflectionHelper.ObjectEquals(row.Tag, this.SelectedDataValue))
                        {
                            this.dropDownGrid.CurrentRow = row;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// DropDownControl
        /// </summary>
        public new DataUnboundGrid DropDownControl
        {
            get { return base.DropDownControl as DataUnboundGrid; }
            set { base.DropDownControl = value; }
        }

        #endregion

        #region "ReadOnly, Visible and Enable"

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

                    //this.dropDownGrid.ReadOnly = value;
                    this.TextBoxArea.ReadOnly = value;
                    this.dropDownButton1.Enabled = !value;
                    this.dropDownGrid.Enabled = !value;
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

        #region "IBindingDataValueControl"

        /// <summary>
        /// DisplayMember
        /// </summary>
        public string DisplayMember
        {
            get;
            set;
        }

        /// <summary>
        /// 查询表达式，可空
        /// </summary>
        public string SearchExpressionParam
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AdjustDropDownControlSize()
        {
            
        }

        #endregion

        #region "IDataValueControl"
        private object m_selectedObject;
        /// <summary>
        /// 设置OptionPicker时对应的值的组合
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                return m_selectedObject;
            }
            set
            {
                try
                {
                    m_selectedObject = value;

                    SetText();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("MyDropDownGridPicker's SelectedDataValue must be object", ex);
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

        public bool SaveLayout()
        {
            return this.DropDownControl.SaveLayout();
        }

        public bool LoadLayout()
        {
            return this.DropDownControl.LoadLayout();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override TextBoxArea CreateTextBoxArea()
        {
            return new MyObjectPickerTextBoxArea();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal void OnOwnKeyPress(KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }
    }

    #region "MyOptionPickerTextBoxArea"

    /// <summary>
    /// ComboBox内置TextBox，用于处理Key和MouseWheel事件
    /// </summary>
    internal class MyObjectPickerTextBoxArea : Xceed.Editors.TextBoxArea
    {
        /// <summary>OBSOLETE: The ComboBoxTextBoxArea( WinComboBox )
        /// constructor is obsolete and has been replaced by the
        /// ComboBoxTextBoxArea() constructor. Initializes a new instance
        /// of the ComboBoxTextBoxArea class specifying the WinComboBox to
        /// associate it to.</summary>
        public MyObjectPickerTextBoxArea()
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
        protected MyObjectPickerTextBoxArea(MyObjectPickerTextBoxArea template)
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
            return new MyObjectPickerTextBoxArea(this);
        }

        /// <summary>
        /// 重载KeyPress事件，当ComboBox为ReadOnly时，不处理KeyPress事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            MyObjectPicker box1 = this.ParentObjectPicker;
            box1.OnOwnKeyPress(e);

            if (!this.ReadOnly)
            {
                base.OnKeyPress(e);

                if (base.IsInputChar(e.KeyChar))
                {
                    if (e.KeyChar == (char)Keys.Enter)
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
                MyObjectPicker box1 = ParentObjectPicker;
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
                            break;
                        case Keys.Up:
                            MyComboBoxTextBoxArea.MoveCurrentRowUp(grid);
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

        private void OwnTextChanged()
        {
            MyComboBoxTextBoxArea.OwnTextChanged(this.ParentObjectPicker.DropDownControl, this.Text);

            ParentObjectPicker.AdjustDropDownControlHeight();

            int rowCount = ParentObjectPicker.GetVisibleRows();
            if (rowCount > 0)
            {
                this.ParentObjectPicker.OpenDropDown();
            }
            else
            {
                this.ParentObjectPicker.CloseDropDown();
            }
        }

        /// <summary>
        /// Parent MyObjectPicker
        /// </summary>
        private MyObjectPicker ParentObjectPicker
        {
            get { return this.Parent as MyObjectPicker; }
        }


    }

    #endregion
}