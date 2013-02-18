using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyCalendar
    /// </summary>
    public class MyCalendar : Xceed.Editors.WinCalendar, IDataValueControl
    {
        #region "Default Property"

        /// <summary>
        /// Constructor
        /// </summary>
        public MyCalendar()
            : base()
        {
            this.DayMargins = new Xceed.Editors.Margins(10, 10, 0, 0);
            this.EnableMultipleMonths = false;

            this.WeekDaysHeader.DayNames = new string[]
                                           {
                                               "一",
                                               "二",
                                               "三",
                                               "四",
                                               "五",
                                               "六",
                                               "日"
                                           };

            if (this.TodayButton != null)
            {
                this.TodayButton.Text = "今天";
                this.TodayButton.Height = 20;
                this.TodayButton.Location = new System.Drawing.Point(65, 123);
            }
            if (this.NoneButton != null)
            {
                this.NoneButton.Text = "空";
                this.NoneButton.Height = 20;
                this.NoneButton.Location = new System.Drawing.Point(160, 123);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyCalendar(Xceed.Editors.WinCalendar template)
            : base(template)
        {
            this.DayMargins = new Xceed.Editors.Margins(10, 10, 0, 0);
            this.EnableMultipleMonths = false;

            this.WeekDaysHeader.DayNames = new string[]
                                           {
                                               "一",
                                               "二",
                                               "三",
                                               "四",
                                               "五",
                                               "六",
                                               "日"
                                           };

            if (this.TodayButton != null)
            {
                this.TodayButton.Text = "今天";
                this.TodayButton.Height = 20;
                this.TodayButton.Location = new System.Drawing.Point(65, 123);
            }
            if (this.NoneButton != null)
            {
                this.NoneButton.Text = "空";
                this.NoneButton.Height = 20;
                this.NoneButton.Location = new System.Drawing.Point(160, 123);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="createTodayButton"></param>
        /// <param name="createNoneButton"></param>
        public MyCalendar(bool createTodayButton, bool createNoneButton)
            : base(createTodayButton, createNoneButton)
        {
            this.DayMargins = new Xceed.Editors.Margins(10, 10, 0, 0);
            this.EnableMultipleMonths = false;

            this.WeekDaysHeader.DayNames = new string[]
                                           {
                                               "一",
                                               "二",
                                               "三",
                                               "四",
                                               "五",
                                               "六",
                                               "日"
                                           };

            if (createTodayButton)
            {
                this.TodayButton.Text = "今天";
                this.TodayButton.Height = 20;
                this.TodayButton.Location = new System.Drawing.Point(65, 123);
            }
            if (createNoneButton)
            {
                this.NoneButton.Text = "空";
                this.NoneButton.Height = 20;
                this.NoneButton.Location = new System.Drawing.Point(160, 123);
            }
        }

        /// <summary>
        /// DefaultDayMargins = Xceed.Editors.Margins(10, 10, 0, 0)
        /// </summary>
        protected override Xceed.Editors.Margins DefaultDayMargins
        {
            get { return new Xceed.Editors.Margins(10, 10, 0, 0); }
        }

        /// <summary>
        /// DefaultEnableMultipleMonths = false
        /// </summary>
        protected override bool DefaultEnableMultipleMonths
        {
            get { return false; }
        }


        /// <summary>
        /// EnableMultipleMonths = false
        /// </summary>
        [DefaultValue(false)]
        public new bool EnableMultipleMonths
        {
            get { return base.EnableMultipleMonths; }
            set { base.EnableMultipleMonths = value; }
        }

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// 设置时为DateTimePicker.Value 或者 null（可空时间控件）
        /// 值中的时间为00:00:00
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                if (base.SelectedDate == base.NullDate)
                {
                    return null;
                }
                else
                {
                    return base.SelectedDate;
                }
            }
            set
            {
                if (value == null)
                {
                    base.SelectedDate = base.NullDate;
                }
                else
                {
                    try
                    {
                        base.SelectedDate = Feng.Utils.ConvertHelper.ToDateTime(value).Value;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyCalendar's SelectedDataValue must be DateTime", ex);
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// ReadOnly = !Enable
        /// </summary>
        [Category("Data")]
        [Description("是否可读")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return !this.Enabled; }
            set
            {
                if (this.Enabled != !value)
                {
                    this.Enabled = !value;
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