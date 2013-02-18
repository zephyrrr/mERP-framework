using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Editors;
using System.ComponentModel;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyDatePicker
    /// </summary>
    [ToolboxItem(false)]
    public class MyDatePickerXceed : Xceed.Editors.WinDatePicker, IDataValueControl, IStateControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="winTextBox"></param>
        public static void RelocateDropdownAnchir(WinTextBox winTextBox)
        {
            System.Drawing.Point point = winTextBox.DropDownControl.PointToScreen(System.Drawing.Point.Empty);
            int x = point.X;
            if (x < 2 && winTextBox.DropDownAnchor == DropDownAnchor.Right)
            {
                winTextBox.DropDownAnchor = Xceed.Editors.DropDownAnchor.Left;
            }
            //else if (x > 0 && this.DropDownAnchor == DropDownAnchor.Left)
            //{
            //    this.DropDownAnchor = Xceed.Editors.DropDownAnchor.Right;
            //}

            System.Drawing.Rectangle bounds = System.Windows.Forms.Screen.FromPoint(point).Bounds;
            if (x + winTextBox.DropDownControl.Width > bounds.Right - 2 && winTextBox.DropDownAnchor == DropDownAnchor.Left)
            {
                winTextBox.DropDownAnchor = Xceed.Editors.DropDownAnchor.Right;
            }
        }

        protected override void OnDropDownOpening(System.ComponentModel.CancelEventArgs e)
        {
            RelocateDropdownAnchir(this);

            base.OnDropDownOpening(e);
        }

        #region "Default Property"

        /// <summary>
        /// Constructor
        /// </summary>
        public MyDatePickerXceed()
            : base()
        {
            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);

            base.Value = base.NullDate;
            base.NullDateString = "";

            base.DropDownAnchor = Xceed.Editors.DropDownAnchor.Right;
            this.DropDownDirection = DropDownDirection.Automatic;
            base.DropDownResizable = false;

            base.Size = new System.Drawing.Size(120, 21);

            //// Relocate DropdownControl. Will cause flick in start
            //this.DroppedDown = true;
            //this.DroppedDown = false;
        }

        protected override System.Windows.Forms.Control CreateDefaultDropDownControl()
        {
            WinCalendar calendar = base.CreateDefaultDropDownControl() as WinCalendar;
            ChangeCalendar(calendar);
            // real size after chagne calendar
            calendar.Size = new System.Drawing.Size(267, 141);

            return calendar;
        }

        public static void ChangeCalendar(WinCalendar calendar)
        {
            calendar.DayMargins = new Xceed.Editors.Margins(10, 10, 0, 0);
            calendar.EnableMultipleMonths = false;

            calendar.WeekDaysHeader.DayNames = new string[]
                                               {
                                                   "一",
                                                   "二",
                                                   "三",
                                                   "四",
                                                   "五",
                                                   "六",
                                                   "日"
                                               };

            //if (createTodayButton)
            {
                calendar.TodayButton.Text = "今天";
                calendar.TodayButton.Height = 20;
                //canlendar.TodayButton.Location = new System.Drawing.Point(65, 123);
            }
            //if (createNoneButton)
            {
                calendar.NoneButton.Text = "空";
                calendar.NoneButton.Height = 20;
                //canlendar.NoneButton.Location = new System.Drawing.Point(160, 123);
            }
        }

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// 设置时为DateTimePicker.Value 或者 null（可空时间控件）
        /// 值中的时间为00:00:00
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue
        {
            get 
            {
                if (base.Value == base.NullDate)
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
                if (value == null)
                {
                    base.Value = base.NullDate;
                }
                else
                {
                    try
                    {
                        base.Value = (DateTime) value;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("value should be DateTime type", ex);
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