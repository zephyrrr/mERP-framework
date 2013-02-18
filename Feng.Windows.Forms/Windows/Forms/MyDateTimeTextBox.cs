using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Xceed.Editors;
using Feng.Utils;

namespace Feng.Windows.Forms
{
    public class MyDateTimeTextBox : MaskedTextBox, IDataValueControl, IStateControl, IFormatControl
    {
        public MyDateTimeTextBox()
        {
            base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            base.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            base.Multiline = false;
            base.Size = new System.Drawing.Size(120, 21);
            base.TextMaskFormat = MaskFormat.IncludeLiterals;

            this.ValidatingType = typeof(DateTime);
            this.TypeValidationCompleted += new TypeValidationEventHandler(MyDateTimeTextBox_TypeValidationCompleted);
            this.Enter += new EventHandler(MyDateTimeTextBox_Enter);
            this.DoubleClick += new EventHandler(MyDateTimeTextBox_DoubleClick);
        }

        void MyDateTimeTextBox_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                if (this.SelectedDataValue == null)
                {
                    this.Text = null;
                }
            }
        }

        void MyDateTimeTextBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectedDataValue == null && !this.ReadOnly)
            {
                this.SelectedDataValue = System.DateTime.Now;
            }
        }

        private int m_selectionStart = -1, m_selectionLength = -1;
        void MyDateTimeTextBox_Enter(object sender, EventArgs e)
        {
            if (this.SelectedDataValue == null)
                return;

            if (m_selectionStart == -1)
            {
                m_selectionStart = m_datetimeFormat.IndexOf('d');
                int idx2 = m_selectionStart + 1;
                while (true)
                {
                    if (m_datetimeFormat[idx2] == 'd')
                        idx2++;
                    else
                        break;
                }
                m_selectionLength = idx2 - m_selectionStart;
            }

            this.BeginInvoke((MethodInvoker)delegate()
            {
                this.SelectionStart = m_selectionStart;
                this.SelectionLength = m_selectionLength;
            });
        }

        public MyDateTimeTextBox(string format)
            : this()
        {
            this.Format = format;
        }

        public string Format
        {
            get { return m_datetimeFormat; }
            set
            {
                this.m_datetimeFormat = value;

                /* http://msdn.microsoft.com/en-us/library/system.windows.forms.maskedtextbox.mask.aspx
0 Digit, required. This element will accept any single digit between 0 and 9.
9 Digit or space, optional.
# Digit or space, optional. If this position is blank in the mask, it will be rendered as a space in the Text property. Plus (+) and minus (-) signs are allowed.
L Letter, required. Restricts input to the ASCII letters a-z and A-Z. This mask element is equivalent to [a-zA-Z] in regular expressions.
? Letter, optional. Restricts input to the ASCII letters a-z and A-Z. This mask element is equivalent to [a-zA-Z]? in regular expressions.
& Character, required. If the AsciiOnly property is set to true, this element behaves like the "L" element.
C Character, optional. Any non-control character. If the AsciiOnly property is set to true, this element behaves like the "?" element.
A Alphanumeric, required. If the AsciiOnly property is set to true, the only characters it will accept are the ASCII letters a-z and A-Z. This mask element behaves like the "a" element.
a Alphanumeric, optional. If the AsciiOnly property is set to true, the only characters it will accept are the ASCII letters a-z and A-Z. This mask element behaves like the "A" element.
. Decimal placeholder. The actual display character used will be the decimal symbol appropriate to the format provider, as determined by the control's FormatProvider property.
, Thousands placeholder. The actual display character used will be the thousands placeholder appropriate to the format provider, as determined by the control's FormatProvider property.
: Time separator. The actual display character used will be the time symbol appropriate to the format provider, as determined by the control's FormatProvider property.
/ Date separator. The actual display character used will be the date symbol appropriate to the format provider, as determined by the control's FormatProvider property.
$ Currency symbol. The actual character displayed will be the currency symbol appropriate to the format provider, as determined by the control's FormatProvider property.
< Shift down. Converts all characters that follow to lowercase.
> Shift up. Converts all characters that follow to uppercase.
| Disable a previous shift up or shift down.
\ Escape. Escapes a mask character, turning it into a literal. "\\" is the escape sequence for a backslash.
All other characters Literals. All non-mask elements will appear as themselves within MaskedTextBox. Literals always occupy a static position in the mask at run time, and cannot be moved or deleted by the user.*/

                char digit = '0';
                this.Mask = m_datetimeFormat.Replace('y', digit).Replace('M', digit).Replace('d', digit).Replace('H', digit).Replace('h', digit)
                    .Replace('m', digit).Replace('s', digit);

                //var formatProvider = new System.Globalization.DateTimeFormatInfo();
                //string[] ss = new string[] { m_datetimeFormat };
                //formatProvider.SetAllDateTimePatterns(ss, 'd');
                //formatProvider.SetAllDateTimePatterns(ss, 't');
                //formatProvider.SetAllDateTimePatterns(ss, 'T');
                //formatProvider.SetAllDateTimePatterns(ss, 'y');
                //formatProvider.SetAllDateTimePatterns(ss, 'Y');
                //formatProvider.FullDateTimePattern = m_datetimeFormat;
                //formatProvider.LongDatePattern = m_datetimeFormat;
                //this.FormatProvider = formatProvider;
            }
        }
        private string m_datetimeFormat;

        #region "IDataValueControl"

        public static bool TryParseExact(string s, string format, out DateTime result)
        {
            var now = System.DateTime.Now;
            result = now;

            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(format))
                return false;

            bool convert = false;
            if (format.StartsWith("yy") || format.StartsWith("hh") || format.StartsWith("HH"))
            {
                convert = DateTime.TryParseExact(s, format, null, System.Globalization.DateTimeStyles.None, out result);
            }
            else
            {
                string datetimeFormat = "yyyy." + format;
                int year = now.Year;
                DateTime timeNow2;
                double maxTimeSpan = double.MaxValue;
                for (int i = -1; i <= 1; ++i)
                {
                    bool c = DateTime.TryParseExact((year + i).ToString() + "." + s, datetimeFormat, null, System.Globalization.DateTimeStyles.None, out timeNow2);
                    if (c)
                    {
                        var d = Math.Abs((now - timeNow2).TotalSeconds);
                        if (d < maxTimeSpan)
                        {
                            result = timeNow2;
                            maxTimeSpan = d;
                            convert = true;
                        }
                    }
                }
            }

            return convert;
        }

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
                if (!this.MaskCompleted)
                    return null;

                DateTime dt;
                bool convert = TryParseExact(this.Text, m_datetimeFormat, out dt);
                if (convert)
                    return dt;
                else
                    return null;
            }
            set
            {
                if (value == null)
                {
                    base.Text = string.Empty;
                }
                else
                {
                    try
                    {
                        base.Text = ((DateTime)value).ToString(m_datetimeFormat);
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

        ///// <summary>
        ///// ReadOnly = !Enable
        ///// </summary>
        //public bool ReadOnly
        //{
        //    get { return !base.Enabled; }
        //    set
        //    {
        //        if (base.Enabled != !value)
        //        {
        //            base.Enabled = !value;
        //            if (ReadOnlyChanged != null)
        //            {
        //                ReadOnlyChanged(this, System.EventArgs.Empty);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public event EventHandler ReadOnlyChanged;

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
