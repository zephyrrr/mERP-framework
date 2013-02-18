using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 月份选择控件
    /// </summary>
    public partial class MyMonthPicker : MyDatePicker
    {
        #region "Default Property"
        /// <summary>
        /// Constructor
        /// </summary>
        public MyMonthPicker()
            : base("yyyy\'年\'MM\'月\'")
        {
            //base.CustomFormat = "yyyy\'年\'MM\'月\'";
        }

        ///// <summary>
        ///// Default CustomFormat
        ///// </summary>
        //[DefaultValue("yyyy\'年\'MM\'月\'")]
        //public new string CustomFormat
        //{
        //    get { return base.CustomFormat; }
        //    set { base.CustomFormat = value; }
        //}

        #endregion

        #region "IDavaValueControl"

        /// <summary>
        /// 设置时为DateTimePicker.Value 或者 null（可空时间控件）
        /// 值中的时间为00:00:00, Day = 1
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override object SelectedDataValue
        {
            get
            {
                if (base.SelectedDataValue == null)
                {
                    return null;
                }
                else
                {
                    DateTime dt = (DateTime)base.SelectedDataValue;
                    return new System.DateTime(dt.Year, dt.Month, 1, 0, 0, 0);
                }
            }
        }

        #endregion
    }

    ///// <summary>
    ///// 月份选择控件
    ///// </summary>
    //public partial class MyMonthPicker : MyDatePicker, IDataValueControl, IStateControl
    //{
    //    #region "Default Property"
    //    /// <summary>
    //    /// Constructor
    //    /// </summary>
    //    public MyMonthPicker() :
    //        base()
    //    {
    //        base.DisplayFormatProvider = new System.Globalization.DateTimeFormatInfo();
    //        base.DisplayFormatProvider.FullDateTimePattern = "yyyy\'年\'MM\'月\'";
    //        base.DisplayFormatSpecifier = "F";

    //        base.EditFormatProvider = new System.Globalization.DateTimeFormatInfo();
    //        base.EditFormatProvider.ShortDatePattern = "yyyy\'年\'MM\'月\'";
    //        base.EditFormatSpecifier = "d";

    //        base.DropDownControl.Size = new Size(base.DropDownControl.Size.Width, 10);
    //    }

    //    /// <summary>
    //    /// 取整数天(当月1号，00:00:00)的Value
    //    /// </summary>
    //    public new object SelectedDataValue
    //    {
    //        get
    //        {
    //            if (base.Value == base.NullDate)
    //                return null;
    //            return new System.DateTime(base.Value.Year, base.Value.Month, 1);
    //        }
    //        set
    //        {
    //            if (value == null)
    //                base.Value = base.NullDate;
    //            DateTime dt = (DateTime)value;
    //            base.Value = new DateTime(dt.Year, dt.Month, 1);
    //        }
    //    }
    //    #endregion
    //}
}