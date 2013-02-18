using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Editors;
using System.ComponentModel;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 日期选择控件
    /// </summary>
    public partial class MyDatePicker : MyDatePickerXceed
    {
        #region "Default Property"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="displayFormat"></param>
        public MyDatePicker(string displayFormat)
            : base()
        {
            base.DisplayFormatSpecifier = displayFormat;

            this.TextBoxArea.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TextBoxArea_KeyPress);
            this.TextBoxArea.DoubleClick += new EventHandler(TextBoxArea_DoubleClick);
        }

        void TextBoxArea_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectedDataValue == null && !this.TextBoxArea.ReadOnly)
            {
                this.SelectedDataValue = System.DateTime.Today;
            }
        }

        void TextBoxArea_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyDatePicker()
            : this("yyyy/MM/dd")
        {
        }

        ///// <summary>
        ///// Default CustomFormat
        ///// </summary>
        //[DefaultValue("yyyy-MM-dd")]
        //public new string CustomFormat
        //{
        //    get { return base.CustomFormat; }
        //    set { base.CustomFormat = value; }
        //}

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// 设置时为DateTimePicker.Value 或者 null（可空时间控件）
        /// 值中的时间为00:00:00
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
                    if (!IsReturnLastTime)
                    {
                        return new System.DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    }
                    else
                    {
                        return new System.DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                    }
                }
            }
        }

        /// <summary>
        /// 返回时间是00：00：00还是23：59：59
        /// </summary>
        public bool IsReturnLastTime
        {
            get;
            set;
        }
        #endregion
    }
}