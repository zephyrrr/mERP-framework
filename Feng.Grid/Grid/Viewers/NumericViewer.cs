using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid.Viewers
{
    /// <summary>
    /// NumericViewer
    /// 默认是2位小数
    /// </summary>
    public class NumericViewer : Xceed.Grid.Viewers.NumericViewer
    {
        private static System.Collections.Generic.Dictionary<string, System.Globalization.NumberFormatInfo>
            formatNumbers =
                new System.Collections.Generic.Dictionary<string, System.Globalization.NumberFormatInfo>();

        /// <summary>
        /// Constructor
        /// </summary>
        public NumericViewer()
            : this(null)
        {
        }

        private const string s_defaultFormatName = "DefaultFormat";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="format"></param>
        public NumericViewer(string format)
        {
            if (!string.IsNullOrEmpty(format))
            {
                if (format.Length != 2)
                {
                    throw new ArgumentException("format's length shoud be 2!", "format");
                }
                if (!formatNumbers.ContainsKey(format))
                {
                    System.Globalization.NumberFormatInfo formatNumber2 = (System.Globalization.CultureInfo.CurrentCulture.NumberFormat).Clone() as System.Globalization.NumberFormatInfo;
                    formatNumber2.CurrencySymbol = " "; //format[0].ToString();

                    formatNumber2.CurrencyDecimalDigits = Convert.ToInt32(format[1].ToString());
                    formatNumber2.CurrencyNegativePattern = 2; // $-n

                    formatNumber2.NumberDecimalDigits = formatNumber2.CurrencyDecimalDigits;
                    formatNumber2.NumberNegativePattern = 2;

                    formatNumbers.Add(format, formatNumber2);
                }
                m_format = formatNumbers[format];
            }
            else
            {
                if (!formatNumbers.ContainsKey(s_defaultFormatName))
                {
                    System.Globalization.NumberFormatInfo formatNumber2 = (System.Globalization.CultureInfo.CurrentCulture.NumberFormat).Clone() as System.Globalization.NumberFormatInfo;
                    formatNumber2.CurrencySymbol = " ";
                    formatNumbers.Add(s_defaultFormatName, formatNumber2);
                }
                m_format = formatNumbers[s_defaultFormatName];
            }
        }

        private System.Globalization.NumberFormatInfo m_format;

        /// <summary>
        /// GetNumber
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override object GetNumber(Type dataType, object value, Xceed.Grid.CellTextFormatInfo formatInfo,
                                            Xceed.Grid.GridElement gridElement)
        {
            object nullValue = formatInfo.NullValue;
            if (((value != null) && (value != DBNull.Value)) &&
                (!string.Empty.Equals(value) && !value.Equals(nullValue)))
            {
                return Convert.ChangeType(value, dataType, m_format);
            }
            return nullValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override string GetTextCore(object value, Xceed.Grid.CellTextFormatInfo formatInfo,
                                              Xceed.Grid.GridElement gridElement)
        {
            if (((value == null) || (value == DBNull.Value)) || value.Equals(formatInfo.NullValue))
            {
                return formatInfo.NullText;
            }
            IFormatProvider formatProvider = m_format; // formatInfo.FormatProvider;
            string formatSpecifier = formatInfo.FormatSpecifier;
            if (string.IsNullOrEmpty(formatSpecifier))
            {
                formatSpecifier = "C"; // "N"
            }

            if (formatProvider != null)
            {
                if (formatSpecifier.Length > 0)
                {
                    return string.Format(formatProvider, "{0:" + formatSpecifier + "}", new object[] {value});
                }
                return string.Format(formatProvider, "{0}", new object[] {value});
            }
            if (formatSpecifier.Length > 0)
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:" + formatSpecifier + "}",
                                     new object[] {value});
            }
            return value.ToString();
        }
    }
}