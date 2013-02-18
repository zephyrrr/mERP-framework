using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;

// Copyright (c) 2005 Claudio Grazioli, http://www.grazioli.ch
//
// This code is free software; you can redistribute it and/or modify it.
// However, this header must remain intact and unchanged.  Additional
// information may be appended after this header.  Publications based on
// this code must also include an appropriate reference.
// 
// This code is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE.
//

// Add a public "Nullable" which set whether it allows for nullable
// By Zephyrrr

namespace Feng.Windows.Forms
{
    /// <summary>
    /// Represents a Windows date time picker control. It enhances the .NET standard <b>DateTimePicker</b>
    /// control with the possibility to show empty values (null values).
    /// </summary>
    [ComVisible(false)]
    [ToolboxItem(false)]
    public class NullableDateTimePicker : DateTimePicker
    {
        #region Member variables

        private bool m_bNullable = true;

        // true, when no date shall be displayed (empty DateTimePicker)
        private bool _isNull;

        // If _isNull = true, this value is shown in the DTP
        private string _nullValue;

        // The format of the DateTimePicker control
        private DateTimePickerFormat _format = DateTimePickerFormat.Long;

        // The custom format of the DateTimePicker control
        private string _customFormat;

        // The format of the DateTimePicker control as string
        private string _formatAsString;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NullableDateTimePicker()
            : base()
        {
            base.Format = DateTimePickerFormat.Custom;
            NullValue = "";
            Format = DateTimePickerFormat.Long;

            //this.SetStyle(ControlStyles.UserPaint, true);
        }

        //private Color _backDisabledColor = Color.FromKnownColor(KnownColor.Control);
        //protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        //{
        //    Graphics g = this.CreateGraphics();

        //    //The dropDownRectangle defines position and size of dropdownbutton block, 
        //    //the width is fixed to 17 and height to 16. 
        //    //The dropdownbutton is aligned to right
        //    Rectangle dropDownRectangle =
        //       new Rectangle(ClientRectangle.Width - 17, 0, 17, 16);
        //    Brush bkgBrush;
        //    ComboBoxState visualState;

        //    //When the control is enabled the brush is set to Backcolor, 
        //    //otherwise to color stored in _backDisabledColor
        //    if (this.Enabled)
        //    {
        //        bkgBrush = new SolidBrush(this.BackColor);
        //        visualState = ComboBoxState.Normal;
        //    }
        //    else
        //    {
        //        bkgBrush = new SolidBrush(this._backDisabledColor);
        //        visualState = ComboBoxState.Disabled;
        //    }

        //    // Painting...in action

        //    //Filling the background
        //    g.FillRectangle(bkgBrush, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

        //    //Drawing the datetime text
        //    g.DrawString(this.Text, this.Font, Brushes.Black, 0, 2);

        //    //Drawing the dropdownbutton using ComboBoxRenderer
        //    ComboBoxRenderer.DrawDropDownButton(g, dropDownRectangle, visualState);

        //    g.Dispose();
        //    bkgBrush.Dispose();
        //}
        
        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the date/time value assigned to the control.
        /// </summary>
        /// <value>The DateTime value assigned to the control
        /// </value>
        /// <remarks>
        /// <p>If the <b>Value</b> property has not been changed in code or by the user, it is set
        /// to the current date and time (<see cref="DateTime.Now"/>).</p>
        /// <p>If <b>Value</b> is <b>null</b>, the DateTimePicker shows 
        /// <see cref="NullValue"/>.</p>
        /// </remarks>
        [Bindable(true)]
        [Browsable(false)]
        [DefaultValue(null)]
        public new Object Value
        {
            get
            {
                if (_isNull)
                {
                    return null;
                }
                else
                {
                    return base.Value;
                }
            }
            set
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    SetToNullValue();
                }
                else
                {
                    SetToDateTimeValue();
                    base.Value = (DateTime) value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the format of the date and time displayed in the control.
        /// </summary>
        /// <value>One of the <see cref="DateTimePickerFormat"/> values. The default is 
        /// <see cref="DateTimePickerFormat.Long"/>.</value>
        [Browsable(true)]
        [DefaultValue(DateTimePickerFormat.Long), TypeConverter(typeof (Enum))]
        public new DateTimePickerFormat Format
        {
            get { return _format; }
            set
            {
                _format = value;
                if (!_isNull)
                {
                    SetFormat();
                }
                OnFormatChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the custom date/time format string.
        /// <value>A string that represents the custom date/time format. The default is a null
        /// reference (<b>Nothing</b> in Visual Basic).</value>
        /// </summary>
        public new String CustomFormat
        {
            get { return _customFormat; }
            set
            {
                _customFormat = value;
                Format = Format;
            }
        }

        /// <summary>
        /// Gets or sets the string value that is assigned to the control as null value. 
        /// </summary>
        /// <value>The string value assigned to the control as null value.</value>
        /// <remarks>
        /// If the <see cref="Value"/> is <b>null</b>, <b>NullValue</b> is
        /// shown in the <b>DateTimePicker</b> control.
        /// </remarks>
        [Browsable(true)]
        [Category("Data")]
        [Description("The string used to display null values in the control")]
        [DefaultValue(" ")]
        public String NullValue
        {
            get { return _nullValue; }
            set { _nullValue = value; }
        }

        #endregion

        #region Private methods/properties

        /// <summary>
        /// Stores the current format of the DateTimePicker as string. 
        /// </summary>
        private string FormatAsString
        {
            get { return _formatAsString; }
            set
            {
                _formatAsString = value;
                base.CustomFormat = value;
            }
        }

        /// <summary>
        /// Sets the format according to the current DateTimePickerFormat.
        /// </summary>
        private void SetFormat()
        {
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            DateTimeFormatInfo dtf = ci.DateTimeFormat;
            switch (_format)
            {
                case DateTimePickerFormat.Long:
                    FormatAsString = dtf.LongDatePattern;
                    break;
                case DateTimePickerFormat.Short:
                    FormatAsString = dtf.ShortDatePattern;
                    break;
                case DateTimePickerFormat.Time:
                    FormatAsString = dtf.ShortTimePattern;
                    break;
                case DateTimePickerFormat.Custom:
                    FormatAsString = this.CustomFormat;
                    break;
            }
        }

        /// <summary>
        /// Sets the <b>DateTimePicker</b> to the value of the <see cref="NullValue"/> property.
        /// </summary>
        private void SetToNullValue()
        {
            _isNull = true;
            base.CustomFormat = (string.IsNullOrEmpty(_nullValue)) ? " " : "'" + _nullValue + "'";
        }

        /// <summary>
        /// Sets the <b>DateTimePicker</b> back to a non null value.
        /// </summary>
        private void SetToDateTimeValue()
        {
            if (_isNull)
            {
                SetFormat();
                _isNull = false;
                base.OnValueChanged(new EventArgs());
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This member overrides <see cref="Control.WndProc"/>.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (_isNull)
            {
                if (m.Msg == 0x4e) // WM_NOTIFY
                {
                    NMHDR nm = (NMHDR) m.GetLParam(typeof (NMHDR));
                    if (nm.Code == -746 || nm.Code == -722) // DTN_CLOSEUP || DTN_?
                    {
                        if (this.Value == null)
                        {
                            m_focusToDay = true;
                        }
                        else
                        {
                            m_focusToDay = false;
                        }
                        SetToDateTimeValue();
                    }
                }
            }
            base.WndProc(ref m);
        }

        bool m_focusToDay = false;

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (m_focusToDay)
            {
                m_focusToDay = false;
                System.Windows.Forms.SendKeys.Send("{RIGHT 2}");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr HwndFrom;
            public int IdFrom;
            public int Code;
        }

        /// <summary>
        /// ÊÇ·ñÔÊÐíÎªNull
        /// </summary>
        [DefaultValue(true)]
        public bool Nullable
        {
            get { return m_bNullable; }
            set { m_bNullable = value; }
        }


        /// <summary>
        /// This member overrides <see cref="Control.OnKeyDown"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                if (m_bNullable)
                {
                    this.Value = null;
                    OnValueChanged(EventArgs.Empty);
                    return;
                }
            }
            else if (e.KeyCode == Keys.Space)
            {
                bool emptyValue = this.Value == null;

                this.Value = System.DateTime.Today;
                OnValueChanged(EventArgs.Empty);

                if (emptyValue)
                {
                    SendKeys.SendWait("{RIGHT}");
                }
                return;
            }
            else if ((e.KeyValue >= (int) Keys.D0 && e.KeyValue <= (int) Keys.D9)
                     || (e.KeyValue >= (int) Keys.NumPad0 && e.KeyValue <= (int) Keys.NumPad9))
            {
                if (this.Value == null)
                {
                    this.Value = System.DateTime.Today;
                    OnValueChanged(EventArgs.Empty);

                    SendKeys.SendWait("{RIGHT}");

                    if (e.KeyValue <= (int)Keys.D9)
                    {
                        SendKeys.SendWait((0 + e.KeyValue - (int) Keys.D0).ToString());
                    }
                    else
                    {
                        SendKeys.SendWait((0 + e.KeyValue - (int)Keys.NumPad0).ToString());
                    }
                }
            }
            base.OnKeyUp(e);
        }

        #endregion
    }
}