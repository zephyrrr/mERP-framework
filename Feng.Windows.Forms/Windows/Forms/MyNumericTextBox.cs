namespace Feng.Windows.Forms
{
    using System;
    using System.ComponentModel;
    using Feng.Utils;

    /// <summary>
    /// 
    /// </summary>
    public enum NumericTextBoxType
    {
        /// <summary>
        /// 
        /// </summary>
        Integer = 1,
        /// <summary>
        /// 
        /// </summary>
        Long = 2,
        /// <summary>
        /// 
        /// </summary>
        Number = 3,
        /// <summary>
        /// 
        /// </summary>
        Currency = 4
    }

    public class MyIntegerTextBox : MyNumericTextBox
    {
        public MyIntegerTextBox()
            : base(NumericTextBoxType.Integer)
        {
        }
    }
    public class MyLongTextBox : MyNumericTextBox
    {
        public MyLongTextBox()
            : base(NumericTextBoxType.Long)
        {
        }
    }
    public class MyCurrencyTextBox : MyNumericTextBox
    {
        public MyCurrencyTextBox()
            : base(NumericTextBoxType.Currency)
        {
        }
    }

    /// <summary>
    /// Integer text box, same properties for moneytextbox
    /// </summary>
    public class MyNumericTextBox : Xceed.Editors.WinNumericTextBox, IDataValueControl, IFormatControl
    {
        #region "Default Property"

        //可设置显示格式。参数可选，为2位字符，第一位为‘+’或者‘-’，第二位为小数点位数， 默认为"+2",即只允许正数，2位小数 
        public const string DefaultNumericTextBoxFormat = "+2";

        private NumericTextBoxType m_textBoxType;
        /// <summary>
        /// 
        /// </summary>
        public MyNumericTextBox()
            : this(NumericTextBoxType.Number)
        {
        }

        public MyNumericTextBox(NumericTextBoxType textBoxType)
            : this(textBoxType, DefaultNumericTextBoxFormat)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="textBoxType"></param>
        /// <param name="format"></param>
        public MyNumericTextBox(NumericTextBoxType textBoxType, string format)
            : base()
        {
            base.Size = new System.Drawing.Size(120, 21);

            this.TextBoxArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            this.TextBoxArea.DoubleClick += new EventHandler((sender, e) => this.OnDoubleClick(e));
            this.TextBoxArea.KeyPress += new System.Windows.Forms.KeyPressEventHandler((sender, e) => this.OnKeyPress(e));
            this.TextBoxArea.KeyDown += new System.Windows.Forms.KeyEventHandler((sender, e) => this.OnKeyDown(e) );
            this.TextBoxArea.KeyUp += new System.Windows.Forms.KeyEventHandler((sender, e) => this.OnKeyUp(e));

            SetNumericTextBoxFormat(this, m_textBoxType, m_format);
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new MyNumericTextBox(m_textBoxType, m_format);
        }

        private string m_format;
        public string Format
        {
            get { return m_format; }
            set
            {
                m_format = value;

                SetNumericTextBoxFormat(this, m_textBoxType, m_format);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBoxType"></param>
        /// <param name="format"></param>
        public void SetNumericTextBoxFormat(NumericTextBoxType textBoxType, string format)
        {
            m_textBoxType = textBoxType;
            m_format = format;

            SetNumericTextBoxFormat(this, textBoxType, format);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="textBoxType"></param>
        /// <param name="format"></param>
        private static void SetNumericTextBoxFormat(Xceed.Editors.WinNumericTextBox textBox, NumericTextBoxType textBoxType, string format)
        {
            if (textBox.EditFormatProvider == null)
            {
                textBox.EditFormatProvider = (System.Globalization.CultureInfo.CurrentCulture.NumberFormat).Clone() as System.Globalization.NumberFormatInfo;
            }
            if (textBox.DisplayFormatProvider == null)
            {
                textBox.DisplayFormatProvider = (System.Globalization.CultureInfo.CurrentCulture.NumberFormat).Clone() as System.Globalization.NumberFormatInfo;
            }

            switch (textBoxType)
            {
                case NumericTextBoxType.Integer:
                    textBox.DataType = typeof(int);
                    break;
                case NumericTextBoxType.Long:
                    textBox.DataType = typeof(long);
                    break;
                case NumericTextBoxType.Number:
                    textBox.DataType = typeof(double);
                    break;
                case NumericTextBoxType.Currency:
                    textBox.DataType = typeof(decimal);
                    break;
            }

            if (!string.IsNullOrEmpty(format))
            {
                if (format.Length != 2)
                {
                    throw new ArgumentException("format is invalid");
                }
                if (format[0] == '-')
                {
                    textBox.ResetMinValue();
                    //textBox.NegativeSignInputChars = new char[] { '-' };
                    textBox.EditFormatProvider.NegativeSign = "-";
                    textBox.ResetNegativeSignInputChars(); // set accoring EditFormatProvider.NegativeSign
                }
                else
                {
                    //// can't set empty array
                    //this.NegativeSignInputChars = new char[] { };

                    textBox.MinValue = 0;
                    textBox.EditFormatProvider.NegativeSign = "";
                    textBox.ResetNegativeSignInputChars();
                }

                switch (textBoxType)
                {
                    case NumericTextBoxType.Integer:
                        textBox.Decimals = 0;
                        //can't set empty
                        textBox.EditFormatProvider.NumberDecimalSeparator = "`";
                        textBox.DecimalSeparatorInputChars = new char[] { };
                        //2位字符，第一位为‘+’或者‘-’，第二位为最大位数， 默认为"+8",即只允许正数，8位数字 
                        break;
                    case NumericTextBoxType.Long:
                        textBox.Decimals = 0;
                        textBox.EditFormatProvider.NumberDecimalSeparator = "`";
                        textBox.DecimalSeparatorInputChars = new char[] { };
                        break;
                    case NumericTextBoxType.Number:
                    case NumericTextBoxType.Currency:
                        //>2位字符，第一位为‘+’或者‘-’，第二位为小数点位数， 默认为"+2",即只允许正数，2位小数

                        int decimals = ConvertHelper.ToInt(format[1]).Value;
                        // 当位数超过时，会导致Grid不能保存，控件里不能移出
                        //textBox.Decimals = decimals;
                        textBox.EditFormatProvider.NumberDecimalDigits = decimals;
                        textBox.EditFormatProvider.CurrencyDecimalDigits = decimals;

                        textBox.DisplayFormatSpecifier = "0.";
                        for (int i = 0; i < decimals; ++i)
                        {
                            textBox.DisplayFormatSpecifier += "0";
                        }

                        break;
                }
            }
        }
        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// 文本框对应的Int值
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                if (this.Value == null)
                {
                    return null;
                }
                else
                {
                    return this.Value;
                }
            }
            set
            {
                if (value == null)
                {
                    this.Value = null;
                }
                else
                {
                    try
                    {
                        this.Value = Feng.Utils.ConvertHelper.ChangeType(value, this.DataType);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyIntegerTextBox's SelectedDataValue must be int", ex);
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

        public bool ReadOnly
        {
            get { return this.TextBoxArea.ReadOnly; }
            set { this.TextBoxArea.ReadOnly = value; this.DropDownButton.Enabled = !value; }
        }

        public event EventHandler ReadOnlyChanged
        {
            add { this.TextBoxArea.ReadOnlyChanged += value; }
            remove { this.TextBoxArea.ReadOnlyChanged -= value; }
        }

        #endregion
    }
}

//using System;
//using System.ComponentModel;
//using Feng.Utils;

//namespace Feng.Windows.Forms
//{
//    /// <summary>
//    /// numeric text box, same properties for moneytextbox
//    /// </summary>
//    public class MyNumericTextBox : AMS.TextBox.NumericTextBox, IDataValueControl
//    {
//        #region "Default Property"

//        /// <summary>
//        /// 
//        /// </summary>
//        public MyNumericTextBox()
//            : this("+2")
//        {
//        }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="format">2位字符，第一位为‘+’或者‘-’，第二位为小数点位数， 默认为"+2",即只允许正数，2位小数 </param>
//        public MyNumericTextBox(string format)
//            : base()
//        {
//            base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            base.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//            base.Size = new System.Drawing.Size(120, 21);

//            base.DecimalPoint = '.';

//            base.DigitsInGroup = 3;
//            base.GroupSeparator = ',';
//            base.MaxDecimalPlaces = 2;
//            base.MaxWholeDigits = 8;
//            base.RangeMax = 99999999;
//            base.RangeMin = 0;
//            base.Flags = (int) AMS.TextBox.ValidatingFlag.ShowIcon_IfInvalid;
//            base.AllowNegative = false;

//            if (format.Length != 2)
//            {
//                throw new ArgumentException("format is invalid");
//            }
//            this.AllowNegative = (format[0] == '-');
//            this.MaxDecimalPlaces = ConvertHelper.ToInt(format[1]).Value;
//        }

//        /// <summary>
//        /// Default BorderStyle 
//        /// </summary>
//        [DefaultValue(System.Windows.Forms.BorderStyle.Fixed3D)]
//        public new System.Windows.Forms.BorderStyle BorderStyle
//        {
//            get { return base.BorderStyle; }
//            set { base.BorderStyle = value; }
//        }

//        /// <summary>
//        /// Default TextAlign 
//        /// </summary>
//        [DefaultValue(System.Windows.Forms.HorizontalAlignment.Right)]
//        public new System.Windows.Forms.HorizontalAlignment TextAlign
//        {
//            get { return base.TextAlign; }
//            set { base.TextAlign = value; }
//        }

//        /// <summary>
//        /// Default DecimalPoint 
//        /// </summary>
//        [DefaultValue('.')]
//        public new Char DecimalPoint
//        {
//            get { return base.DecimalPoint; }
//            set { base.DecimalPoint = value; }
//        }


//        /// <summary>
//        /// Default DigitsInGroup 
//        /// </summary>
//        [DefaultValue(3)]
//        public new Int32 DigitsInGroup
//        {
//            get { return base.DigitsInGroup; }
//            set { base.DigitsInGroup = value; }
//        }

//        /// <summary>
//        /// Default GroupSeparator 
//        /// </summary>
//        [DefaultValue(',')]
//        public new Char GroupSeparator
//        {
//            get { return base.GroupSeparator; }
//            set { base.GroupSeparator = value; }
//        }

//        /// <summary>
//        /// Default MaxDecimalPlaces 
//        /// </summary>
//        [DefaultValue(2)]
//        public new Int32 MaxDecimalPlaces
//        {
//            get { return base.MaxDecimalPlaces; }
//            set { base.MaxDecimalPlaces = value; }
//        }

//        /// <summary>
//        /// Default MaxWholeDigits 
//        /// </summary>
//        [DefaultValue(8)]
//        public new Int32 MaxWholeDigits
//        {
//            get { return base.MaxWholeDigits; }
//            set { base.MaxWholeDigits = value; }
//        }

//        /// <summary>
//        /// Default RangeMax 
//        /// </summary>
//        [DefaultValue(99999999.0)]
//        public new Double RangeMax
//        {
//            get { return base.RangeMax; }
//            set { base.RangeMax = value; }
//        }

//        /// <summary>
//        /// Default RangeMin 
//        /// </summary>
//        [DefaultValue(0.0)]
//        public new Double RangeMin
//        {
//            get { return base.RangeMin; }
//            set { base.RangeMin = value; }
//        }

//        /// <summary>
//        /// Default AllowNegative 
//        /// </summary>
//        [DefaultValue(false)]
//        public new bool AllowNegative
//        {
//            get { return base.AllowNegative; }
//            set { base.AllowNegative = value; }
//        }

//        #endregion

//        #region "IDataValueControl"

//        /// <summary>
//        /// ????????浽??????е?????CurrencyTextBox.Double
//        /// </summary>
//        [Browsable(false)]
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//        public object SelectedDataValue
//        {
//            get
//            {
//                if (string.IsNullOrEmpty(Text))
//                {
//                    return null;
//                }
//                else
//                {
//                    return Double;
//                }
//            }
//            set
//            {
//                if (value == null)
//                {
//                    Text = string.Empty;
//                }
//                else
//                {
//                    try
//                    {
//                        Double = Feng.Utils.ConvertHelper.ToDouble(value).Value;
//                    }
//                    catch (Exception ex)
//                    {
//                        throw new ArgumentException("MyNumericTextBox's SelectedDataValue must be double", ex);
//                    }
//                }
//            }
//        }

//        #endregion

//        #region "IStateControl"

//        /// <summary>
//        /// ????????????State
//        /// </summary>
//        public void SetState(StateType state)
//        {
//            StateControlHelper.SetState(this, state);
//        }

//        #endregion
//    }
//}