namespace Feng.Windows.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Windows.Forms.VisualStyles;


    /// <summary>
    /// 软件默认MyDateTimePicker，包含如下属性：
    /// <list type="bullet">
    /// <item>Size = (120,21)</item>
    /// <item>可设置成Null，在控件上按Delete键</item>
    /// <item>格式为"yy/MM/dd"</item>
    /// <item>当值为Null时，显示空</item>
    /// </list>
    /// </summary>
    public class MyDateTimePicker : NullableDateTimePicker, IDataValueControl
    {
        #region "Default Proeprty"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">"yyyy-MM-dd"; "HH:mm"; "yy-MM-dd HH:mm"; "MM-dd HH:mm"</param>
        public MyDateTimePicker(string format)
            : base()
        {
            base.CustomFormat = format;
            base.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            base.NullValue = "";
            base.Size = new System.Drawing.Size(120, 21);

            base.ShowUpDown = true;
            base.Value = null;
            this.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;

            base.ShowUpDown = true;
        }
        


        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyDateTimePicker()
            : this("yy-MM-dd HH:mm")
        {
        }

        /// <summary>
        /// Default Format
        /// </summary>
        [DefaultValue(System.Windows.Forms.DateTimePickerFormat.Custom)]
        public new System.Windows.Forms.DateTimePickerFormat Format
        {
            get { return base.Format; }
            set { base.Format = value; }
        }

        /// <summary>
        /// Default CustomFormat
        /// </summary>
        [DefaultValue("yyyy-MM-dd HH:mm")]
        public new string CustomFormat
        {
            get { return base.CustomFormat; }
            set { base.CustomFormat = value; }
        }

        /// <summary>
        /// Default NullValue
        /// </summary>
        [DefaultValue("")]
        public new string NullValue
        {
            get { return base.NullValue; }
            set { base.NullValue = value; }
        }

        /// <summary>
        /// Default DropDownAlign
        /// </summary>
        [DefaultValue(System.Windows.Forms.LeftRightAlignment.Right)]
        public new System.Windows.Forms.LeftRightAlignment DropDownAlign
        {
            get { return base.DropDownAlign; }
            set { base.DropDownAlign = value; }
        }
        //protected bool m_bRoundTime;
        ///// <summary>
        ///// 是否取整数天(当天晚上23:59:59)
        ///// </summary>
        //public bool RoundTime
        //{
        //    get { return m_bRoundTime; }
        //    set { m_bRoundTime = value; }
        //}

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// 设置时为DateTimePicker.Value 或者 null（可空时间控件）
        /// 如大于9998-12-31 or 小于1753-1-1，设置为null
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue
        {
            get { return base.Value; }
            set
            {
                if (value == null)
                {
                    base.Value = null;
                }
                else
                {
                    try
                    {
                        DateTime d = Feng.Utils.ConvertHelper.ToDateTime(value).Value;
                        if (d > this.MaxDate || d < this.MinDate)
                        {
                            base.Value = null;
                        }
                        else
                        {
                            base.Value = d;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyDateTimePicker's SelectedDataValue must be DateTime", ex);
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// ReadOnly = !Enable
        /// </summary>
        public bool ReadOnly
        {
            get { return !base.Enabled; }
            set
            {
                if (base.Enabled != !value)
                {
                    base.Enabled = !value;
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
        /// 对显示控件设置ReadOnly
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion
    }
}