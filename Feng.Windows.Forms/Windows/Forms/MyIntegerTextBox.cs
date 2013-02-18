//namespace Feng.Windows.Forms
//{
    ///// <summary>
    ///// Integer text box, same properties for moneytextbox
    ///// </summary>
    //public class MyIntegerTextBox :  AMS.TextBox.IntegerTextBox, IDataValueControl
    //{
    //    #region "Default Property"

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public MyIntegerTextBox()
    //        : this("+8")
    //    {
    //    }

    //    /// <summary>
    //    /// Constructor
    //    /// </summary>
    //    /// <param name="format">2位字符，第一位为‘+’或者‘-’，第二位为最大位数， 默认为"+8",即只允许正数，8位数字 </param>
    //    public MyIntegerTextBox(string format)
    //        : base()
    //    {
    //        base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
    //        base.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
    //        base.Size = new System.Drawing.Size(120, 21);

    //        base.DigitsInGroup = 3;
    //        base.GroupSeparator = ',';
    //        base.MaxDecimalPlaces = 0;
    //        base.RangeMax = Int32.MaxValue;
    //        base.RangeMin = 0;
    //        base.Flags = (Int32) AMS.TextBox.ValidatingFlag.ShowIcon_IfInvalid;

    //        if (format.Length != 2)
    //        {
    //            throw new ArgumentException("format is invalid");
    //        }
    //        this.AllowNegative = (format[0] == '-');
    //        if (this.AllowNegative)
    //        {
    //            base.RangeMin = Int32.MinValue;
    //        }

    //        this.MaxWholeDigits = ConvertHelper.ToInt(format[1]).Value;
    //    }

    //    /// <summary>
    //    /// Default BorderStyle 
    //    /// </summary>
    //    [DefaultValue(System.Windows.Forms.BorderStyle.Fixed3D)]
    //    public new System.Windows.Forms.BorderStyle BorderStyle
    //    {
    //        get { return base.BorderStyle; }
    //        set { base.BorderStyle = value; }
    //    }

    //    /// <summary>
    //    /// Default TextAlign 
    //    /// </summary>
    //    [DefaultValue(System.Windows.Forms.HorizontalAlignment.Right)]
    //    public new System.Windows.Forms.HorizontalAlignment TextAlign
    //    {
    //        get { return base.TextAlign; }
    //        set { base.TextAlign = value; }
    //    }

    //    /// <summary>
    //    /// Default DigitsInGroup 
    //    /// </summary>
    //    [DefaultValue(3)]
    //    public new Int32 DigitsInGroup
    //    {
    //        get { return base.DigitsInGroup; }
    //        set { base.DigitsInGroup = value; }
    //    }

    //    /// <summary>
    //    /// Default GroupSeparator 
    //    /// </summary>
    //    [DefaultValue(',')]
    //    public new Char GroupSeparator
    //    {
    //        get { return base.GroupSeparator; }
    //        set { base.GroupSeparator = value; }
    //    }

    //    /// <summary>
    //    /// Default MaxDecimalPlaces 
    //    /// </summary>
    //    [DefaultValue(0)]
    //    public new Int32 MaxDecimalPlaces
    //    {
    //        get { return base.MaxDecimalPlaces; }
    //        set { base.MaxDecimalPlaces = value; }
    //    }

    //    /// <summary>
    //    /// Default MaxWholeDigits 
    //    /// </summary>
    //    [DefaultValue(8)]
    //    public new Int32 MaxWholeDigits
    //    {
    //        get { return base.MaxWholeDigits; }
    //        set { base.MaxWholeDigits = value; }
    //    }

    //    /// <summary>
    //    /// Default RangeMax 
    //    /// </summary>
    //    [DefaultValue(Int32.MaxValue)]
    //    public new Double RangeMax
    //    {
    //        get { return base.RangeMax; }
    //        set { base.RangeMax = value; }
    //    }

    //    /// <summary>
    //    /// Default RangeMin 
    //    /// </summary>
    //    [DefaultValue(0.0)]
    //    public new Double RangeMin
    //    {
    //        get { return base.RangeMin; }
    //        set { base.RangeMin = value; }
    //    }

    //    /// <summary>
    //    /// Default AllowNegative 
    //    /// </summary>
    //    [DefaultValue(false)]
    //    public new bool AllowNegative
    //    {
    //        get { return base.AllowNegative; }
    //        set { base.AllowNegative = value; }
    //    }

    //    #endregion

    //    #region "IDataValueControl"

    //    /// <summary>
    //    /// 文本框对应的Int值
    //    /// </summary>
    //    [Browsable(false)]
    //    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    //    public object SelectedDataValue
    //    {
    //        get
    //        {
    //            if (string.IsNullOrEmpty(Text))
    //            {
    //                return null;
    //            }
    //            else
    //            {
    //                return Int;
    //            }
    //        }
    //        set
    //        {
    //            if (value == null)
    //            {
    //                Text = string.Empty;
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    Int = Feng.Utils.ConvertHelper.ToInt(value).Value;
    //                }
    //                catch (Exception ex)
    //                {
    //                    throw new ArgumentException("MyIntegerTextBox's SelectedDataValue must be int", ex);
    //                }
    //            }
    //        }
    //    }

    //    #endregion

    //    #region "IStateControl"

    //    /// <summary>
    //    /// 对显示控件设置State
    //    /// </summary>
    //    public void SetState(StateType state)
    //    {
    //        StateControlHelper.SetState(this, state);
    //    }

    //    #endregion
    //}
//}