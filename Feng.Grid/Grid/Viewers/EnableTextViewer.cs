using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Xceed.Grid.Viewers;
using Xceed.Grid;

namespace Feng.Grid.Viewers
{
    public class EnableTextViewer : CellViewerManager
    {
        public EnableTextViewer(string text)
            : base()
        {
            m_text = text;
            m_foreColorEnable = System.Drawing.SystemColors.ControlText;
            m_foreColorDisable = System.Drawing.SystemColors.GrayText;
        }
        public EnableTextViewer(string text, string foreColorEnable, string foreColorDisable)
            : base()
        {
            m_text = text;
            m_foreColorEnable = System.Drawing.Color.FromName(foreColorEnable);
            m_foreColorDisable = System.Drawing.Color.FromName(foreColorDisable);
        }
        public EnableTextViewer(string text, string foreColorEnable, string foreColorDisable, string backColorEnable, string backColorDisable)
            : base()
        {
            m_text = text;
            m_foreColorEnable = System.Drawing.Color.FromName(foreColorEnable);
            m_foreColorDisable = System.Drawing.Color.FromName(foreColorEnable);
            m_backColorEnable = System.Drawing.Color.FromName(backColorEnable);
            m_backColorDisable = System.Drawing.Color.FromName(backColorDisable);
        }
        private string m_text;
        private Color m_foreColorEnable, m_foreColorDisable;
        private Color? m_backColorEnable, m_backColorDisable;
        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            return m_text;
        }
        private StringFormat sFormat = new StringFormat(StringFormat.GenericTypographic);
        protected override void PaintCellBackground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            bool? v = Feng.Utils.ConvertHelper.ToBoolean(cell.Value);
            bool cellValue = v.HasValue ? v.Value : false;
            if (cellValue)
            {
                if (m_backColorEnable.HasValue)
                {
                    e.Graphics.FillRectangle(new SolidBrush(m_backColorEnable.Value), e.DisplayRectangle);
                    handled = true;
                }
                else
                {
                    base.PaintCellBackground(cell, e, ref handled);
                }
            }
            else
            {
                if (m_backColorDisable.HasValue)
                {
                    e.Graphics.FillRectangle(new SolidBrush(m_backColorDisable.Value), e.DisplayRectangle);
                    handled = true;
                }
                else
                {
                    base.PaintCellBackground(cell, e, ref handled);
                }
            }
        }

        protected override void PaintCellForeground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            string value = m_text;
            if (string.IsNullOrEmpty(value))
                return;
            bool? v = Feng.Utils.ConvertHelper.ToBoolean(cell.Value);
            bool cellValue = v.HasValue ? v.Value : false;

            handled = true;
            Graphics g = e.Graphics;
            Font font = cell.Font;
            PointF origin = new PointF(0, 0);

            string[] ss = value.Split(new char[] { '-' });
            string[] texts = new string[ss.Length];
            float allLength = 0;
            for (int i = 0; i < ss.Length; ++i)
            {
                string s = ss[i];

                string[] ss2 = s.Split(new char[] { '/' });
                if (ss2.Length == 1)
                {
                    texts[i] = s;
                }
                else
                {
                    texts[i] = ss2[1];
                }

                SizeF sizeTxt = g.MeasureString(texts[i], font, origin, sFormat);
                allLength += sizeTxt.Width;
            }

            Rectangle r = e.DisplayRectangle;
            r.Inflate(-1, -1);
            float nowOffset = 0;

            switch (e.DisplayVisualStyle.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    break;
                case HorizontalAlignment.Center:
                    nowOffset = (e.DisplayRectangle.Width - allLength) / 2;
                    break;
                case HorizontalAlignment.Right:
                    nowOffset = e.DisplayRectangle.Width - allLength;
                    break;
            }

            for (int i = 0; i < ss.Length; ++i)
            {
                // http://weblogs.asp.net/israelio/archive/2006/07/30/DrawString-_2F00_-MeasureString-Offset-Problem-Solved-_2100_.aspx
                origin = new PointF(nowOffset, 0);
                SizeF sizeTxt = g.MeasureString(texts[i], font, origin, sFormat);

                Color paintColor = cellValue ? m_foreColorEnable : m_foreColorDisable;
                using (SolidBrush brush = new SolidBrush(paintColor))
                {
                    RectangleF rectangleTxt = (RectangleF)r;
                    rectangleTxt.Width = sizeTxt.Width;
                    rectangleTxt.Offset(nowOffset, 0f);
                    nowOffset += sizeTxt.Width;
                    g.DrawString(texts[i], font, brush, rectangleTxt, sFormat);
                }
            }
        }
    }
}
