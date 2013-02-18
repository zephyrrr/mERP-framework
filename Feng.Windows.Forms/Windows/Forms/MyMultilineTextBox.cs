using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Text;
using Xceed.Editors;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyMultilineTextBox
    /// </summary>
    public class MyMultilineTextBox : Xceed.Editors.WinTextBox, IMultiDataValueControl, IDataValueControl, IFormatControl
    {
        protected override void OnDropDownOpening(System.ComponentModel.CancelEventArgs e)
        {
            MyDatePickerXceed.RelocateDropdownAnchir(this);

            base.OnDropDownOpening(e);
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        public MyMultilineTextBox()
            : this("Normal")
        {
            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);
        }

        public MyMultilineTextBox(string format)
        {
            Format = format;

            this.Initialize(true);
        }

        private string m_format;
        /// <summary>
        /// 
        /// </summary>
        public string Format
        {
            get { return m_format; }
            set
            {
                m_format = value;
                this.TextBoxArea.CharacterCasing = m_format == "Upper" ? System.Windows.Forms.CharacterCasing.Upper :
                    (m_format == "Lower" ? System.Windows.Forms.CharacterCasing.Lower : System.Windows.Forms.CharacterCasing.Normal);
            }
        }

        protected MyMultilineTextBox(MyMultilineTextBox template)
            : base(template)
        {
            this.InitializeEvents();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public MyMultilineTextBox(System.Type callerType, EnhancedBorderStyle borderStyle, bool allowDropDown)
            : base(callerType, borderStyle)
        {
            this.Initialize(allowDropDown);
        }

        public override object Clone()
        {
            return new MyMultilineTextBox(this);
        }

        protected override Control CreateDefaultDropDownControl()
        {
            Xceed.Editors.WinTextBox dropDownControl = new Xceed.Editors.WinTextBox();
            dropDownControl.TextBoxArea.Multiline = true;
            dropDownControl.TextBoxArea.WordWrap = true;
            dropDownControl.TextBoxArea.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dropDownControl.TextBoxArea.MaxLength = this.TextBoxArea.MaxLength;
            dropDownControl.TextBoxArea.AcceptsTab = true;
            dropDownControl.TextBoxArea.AcceptsReturn = false;
            switch (m_format)
            {
                case "Upper":
                    dropDownControl.TextBoxArea.CharacterCasing = CharacterCasing.Upper;
                    break;
                case "Lower":
                    dropDownControl.TextBoxArea.CharacterCasing = CharacterCasing.Lower;
                    break;
                case "Normal":
                    dropDownControl.TextBoxArea.CharacterCasing = CharacterCasing.Normal;
                    break;
            }
            // bug：宽度必须要大于ToolWindow，不然鼠标移到其他地方时会缩回去
            dropDownControl.Size = new System.Drawing.Size(300, 150);
            return dropDownControl;
        }

        protected override Xceed.Editors.TextBoxArea CreateTextBoxArea()
        {
            return base.CreateTextBoxArea();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.TextBoxArea.KeyPress -= new KeyPressEventHandler(TextBoxArea_KeyPress);
                this.TextBoxArea.KeyDown -= new KeyEventHandler(TextBoxArea_KeyDown);

                if (this.DropDownControl != null)
                {
                    this.DropDownControl.TextBoxArea.KeyDown -= new KeyEventHandler(DropdownControlTextBoxArea_KeyDown);
                }
                this.DroppedDownChanged -= new EventHandler(MyMultilineTextBox_DroppedDownChanged);
            }
            base.Dispose(disposing);
        }

        private void Initialize(bool allowDropDown)
        {
            this.InitializeEvents();
            base.AllowDropDown = allowDropDown;
            WinButton button = new WinButton(base.GetType());
            this.SetDropDownButtonDefaults(button);
            base.SideButtons.Add(button);
            base.DropDownButton = button;
        }

        private void InitializeEvents()
        {
            this.TextBoxArea.KeyPress += new KeyPressEventHandler(TextBoxArea_KeyPress);
            this.TextBoxArea.KeyDown += new KeyEventHandler(TextBoxArea_KeyDown);

            if (this.DropDownControl != null)
            {
                this.DropDownControl.TextBoxArea.KeyDown += new KeyEventHandler(DropdownControlTextBoxArea_KeyDown);
            }

            this.DroppedDownChanged += new EventHandler(MyMultilineTextBox_DroppedDownChanged);
        }

        private bool m_splitWithComma = false;
        /// <summary>
        /// 
        /// </summary>
        public bool SplitWithComma
        {
            get { return m_splitWithComma; }
            set { m_splitWithComma = value; }
        }

        private void SetDopdownControlValue()
        {
            if (string.IsNullOrEmpty(this.TextBoxArea.Text))
            {
                this.DropDownControl.TextBoxArea.Text = null;
                return;
            }

            if (SplitWithComma)
            {
                string[] ss = this.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < ss.Length; ++j)
                    {
                        sb.Append(ss[j].Trim());
                        if (j != ss.Length - 1)
                        {
                            sb.Append(System.Environment.NewLine);
                        }
                    }
                    this.DropDownControl.TextBoxArea.Text = sb.ToString();
                }
            }
            else
            {
                this.DropDownControl.TextBoxArea.Text = this.TextBoxArea.Text;
            }
        }

        private void CalculateText()
        {
            if (SplitWithComma)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < this.DropDownControl.TextBoxArea.Lines.Length; ++i)
                {
                    sb.Append(this.DropDownControl.TextBoxArea.Lines[i].Trim());
                    if (i != this.DropDownControl.TextBoxArea.Lines.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                this.TextBoxArea.Text = sb.ToString();
                this.TextBoxArea.Select(sb.Length, 0);
            }
            else
            {
                this.TextBoxArea.Text = this.DropDownControl.TextBoxArea.Text;
            }
        }

        void MyMultilineTextBox_DroppedDownChanged(object sender, EventArgs e)
        {
            if (!this.DroppedDown)
            {
                CalculateText();
            }
            else
            {
                SetDopdownControlValue();
            }
        }

        // 当鼠标移出是，自动关闭。修改Bug：ToolWindow AutoHide时，DropdownControl并不是属于ToolWindow，因此当焦点移出时，ToolWindow自动Hide，会使Mdi窗体跳到第一个Child
        //void DropDownControl_MouseLeave(object sender, EventArgs e)
        //{
        //this.CloseDropDown();
        //}

        void TextBoxArea_KeyPress(object sender, KeyPressEventArgs e)
        {
            // When TextBox Key pressed, raise this control's Key Press event(OnSearch)
            this.OnKeyPress(e);
        }
        void DropdownControlTextBoxArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.DropDownControl.TextBoxArea.SelectAll();
            }

        }

        void TextBoxArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.TextBoxArea.SelectAll();
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (!this.DroppedDown)
                {
                    this.DroppedDown = true;
                    if (!this.DropDownControl.Focused)
                    {
                        this.DropDownControl.Focus();
                    }
                }
            }
        }

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    base.Invalidate();
        //    base.Update();
        //    base.OnMouseUp(e);
        //}


        protected override void SetDropDownControlCore(Control control)
        {
            throw new InvalidOperationException("A DropDownControl cannot be assigned to a " + base.GetType().ToString() + ".");
        }

        //internal void SynchTextBoxArea()
        //{
        //    //object obj2 = this.Value;
        //    //if (this.CompareObjects(obj2, this.NullValue) == 0)
        //    //{
        //    //    this.TextBoxArea.SetRawTextCore(string.Empty);
        //    //}
        //    //else
        //    //{
        //    //    string str = this.GetValueAsString(obj2, string.Empty, this.ActiveEditFormatProvider);
        //    //    this.TextBoxArea.SetRawTextCore(str);
        //    //}

        //    this.TextBoxArea.SetRawTextCore(this.DropDownControl.Text);
        //    this.TextBoxArea.SelectAll();
        //}

        protected override DropDownAnchor DefaultDropDownAnchor
        {
            get
            {
                return DropDownAnchor.Right;
            }
        }

        protected override Size DefaultDropDownMinSize
        {
            get
            {
                return new Size(180, 60);
            }
        }

        protected override bool DefaultDropDownResizable
        {
            get
            {
                return true;
            }
        }

        protected override bool DefaultDropDownAllowFocus
        {
            get
            {
                return true;
            }
        }
        //internal bool DropDownCalculatorHasEqualPending
        //{
        //    get
        //    {
        //        if (!base.DropDownInstantiated)
        //        {
        //            return false;
        //        }
        //        DropDownCalculator dropDownControl = this.DropDownControl as DropDownCalculator;
        //        return dropDownControl.EqualPending;
        //    }
        //}

        [Description("The WinCalculator used as the WinNumericTextBox's dropdown control."), Category("DropDown"), MergableProperty(false)]
        public new Xceed.Editors.WinTextBox DropDownControl
        {
            get
            {
                return (base.DropDownControl as Xceed.Editors.WinTextBox);
            }
        }

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

                    this.DropDownControl.TextBoxArea.ReadOnly = value;
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

        #region "IDataValueControl"
        //private MyToolTip toolTip = new MyToolTip();
        /// <summary>
        /// 设置OptionPicker时对应的值的组合
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                string text = this.TextBoxArea.Text.Trim();
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }
                else
                {
                    return text;
                }
            }
            set
            {
                if (value == null)
                {
                    this.TextBoxArea.Text = String.Empty;
                    //this.toolTip.SetToolTip(this.TextBoxArea, null);
                }
                else
                {
                    try
                    {
                        this.TextBoxArea.Text = Feng.Utils.ConvertHelper.ToString(value).Trim();
                        //this.toolTip.SetToolTip(this.TextBoxArea, this.TextBoxArea.Text);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyMultilineTextBoxSearch's SelectedDataValue must be string", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 以数组形式表示的SelectedDataValue
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedDataValues
        {
            get
            {
                string split = SplitWithComma ? "," : System.Environment.NewLine;
                ArrayList ret = new ArrayList();
                if (this.SelectedDataValue != null)
                {
                    string s = this.SelectedDataValue.ToString();
                    string[] ss = s.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                    ret.AddRange(ss);
                }
                return ret;
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    this.SelectedDataValue = null;
                }
                else
                {
                    string split = SplitWithComma ? "," : System.Environment.NewLine;
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < value.Count; ++i)
                        {
                            sb.Append(value[i].ToString().Trim());
                            if (i != value.Count - 1)
                            {
                                sb.Append(split);
                            }
                        }
                        this.SelectedDataValue = sb.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyMultilineTextBoxSearch's SelectedDataValue must be object[]", ex);
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
    }
}

