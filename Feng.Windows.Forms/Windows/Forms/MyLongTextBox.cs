//namespace Feng.Windows.Forms
//{
//    using System;
//    using System.ComponentModel;

//    /// <summary>
//    /// Integer text box, same properties for moneytextbox
//    /// </summary>
//    public class MyLongTextBox : AMS.TextBox.IntegerTextBox, IDataValueControl
//    {
//        #region "Default Property"

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public MyLongTextBox()
//            : base()
//        {
//            base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            base.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//            base.Size = new System.Drawing.Size(120, 21);

//            base.DigitsInGroup = 3;
//            base.GroupSeparator = ',';
//            base.MaxDecimalPlaces = 0;
//            base.MaxWholeDigits = 8;
//            base.RangeMax = Int64.MaxValue;
//            base.RangeMin = 0;
//            base.Flags = (Int32)AMS.TextBox.ValidatingFlag.ShowIcon_IfInvalid;
//            base.AllowNegative = false;
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
//        [DefaultValue(0)]
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
//        [DefaultValue(Int64.MaxValue)]
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
//        /// 文本框对应的Int值
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
//                    return Long;
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
//                        Long = Feng.Utils.ConvertHelper.ToLong(value).Value;
//                    }
//                    catch (Exception ex)
//                    {
//                        throw new ArgumentException("MyIntegerTextBox's SelectedDataValue must be long", ex);
//                    }
//                }
//            }
//        }

//        #endregion

//        #region "IStateControl"

//        /// <summary>
//        /// 对显示控件设置State
//        /// </summary>
//        public void SetState(StateType state)
//        {
//            StateControlHelper.SetState(this, state);
//        }

//        #endregion
//    }
//}
