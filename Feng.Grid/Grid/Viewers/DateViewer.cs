using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid.Viewers
{
    /// <summary>
    ///  ±º‰œ‘ æViewer
    /// </summary>
    public class DateViewer : Xceed.Grid.Viewers.DateViewer
    {
        private static System.Collections.Generic.Dictionary<string, System.Globalization.DateTimeFormatInfo>
            formatDates =
                new System.Collections.Generic.Dictionary<string, System.Globalization.DateTimeFormatInfo>();

        /// <summary>
        /// Constructor(yyyy-MM-dd)
        /// </summary>
        public DateViewer()
            : this("yyyy-MM-dd")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="format"></param>
        public DateViewer(string format)
        {
            if (!formatDates.ContainsKey(format))
            {
                System.Globalization.DateTimeFormatInfo formatDate = new System.Globalization.DateTimeFormatInfo();
                formatDate.ShortDatePattern = format;
                // "yy-MM-dd"; "HH:mm"; "yy-MM-dd HH:mm"; "MM-dd HH:mm";

                formatDates.Add(format, formatDate);
            }
            m_format = formatDates[format];
        }

        private System.Globalization.DateTimeFormatInfo m_format;

        /// <summary>
        /// GetDate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override DateTime GetDate(object value, Xceed.Grid.CellTextFormatInfo formatInfo,
                                            Xceed.Grid.GridElement gridElement)
        {
            object nullValue = formatInfo.NullValue;
            if (((value != null) && (value != DBNull.Value)) &&
                (!string.Empty.Equals(value) && !value.Equals(nullValue)))
            {
                return Convert.ToDateTime(value, m_format);
            }
            if (nullValue is DateTime)
            {
                return (DateTime) nullValue;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// GetTextCore
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override string GetTextCore(object value, Xceed.Grid.CellTextFormatInfo formatInfo,
                                              Xceed.Grid.GridElement gridElement)
        {
            DateTime minValue;
            DateTime time = this.GetDate(value, formatInfo, gridElement);
            object nullValue = formatInfo.NullValue;
            if (nullValue is DateTime)
            {
                minValue = (DateTime) nullValue;
            }
            else
            {
                minValue = DateTime.MinValue;
            }
            if (time == minValue)
            {
                return formatInfo.NullText;
            }
            string formatSpecifier = formatInfo.FormatSpecifier;
            if (string.IsNullOrEmpty(formatSpecifier))
            {
                formatSpecifier = "d";
            }
            return time.ToString(formatSpecifier, m_format);
        }
    }
}