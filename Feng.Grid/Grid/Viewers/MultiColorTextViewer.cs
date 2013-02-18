using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Xceed.Grid.Viewers;
using Xceed.Grid;
using Feng.Utils;

namespace Feng.Grid.Viewers
{
    //public class MultiComboColorTextViewer : MultiColorTextViewer
    //{
    //    public MultiComboColorTextViewer(string nvName)
    //        : base()
    //    {
    //        this.m_nvName = nvName;
    //    }
    //    private string m_nvName;

    //    protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
    //    {
    //        return NameValueControlHelper.GetMultiString(m_nvName, value);
    //    }
    //}

    public class MultiColorTextViewer : CellViewerManager
    {

        public MultiColorTextViewer()
            : this(new Color[] { Color.Blue, SystemColors.ControlText })
        {
        }

        public MultiColorTextViewer(Color[] colors)
        {
            m_colors = colors;
        }

        public MultiColorTextViewer(string[] colors)
        {
            m_colors = new Color[colors.Length];
            for (int i = 0; i < colors.Length; ++i)
            {
                m_colors[i] = Color.FromName(colors[i].Trim());
            }
        }

        private StringFormat sFormat = new StringFormat(StringFormat.GenericTypographic);
        private Color[] m_colors;
        protected override void PaintCellForeground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            string value = cell.GetDisplayText();
            if (string.IsNullOrEmpty(value))
                return;

            handled = true;
            Graphics g = e.Graphics;
            Font font = cell.Font;
            PointF origin = new PointF(0, 0);

            string[] ss = value.Split(new char[] { '-'});
            string[] texts = new string[ss.Length];
            Color[] paintColors = new Color[ss.Length];
            float allLength = 0;
            for (int i = 0; i < ss.Length; ++i)
            {
                string s = ss[i];

                string[] ss2 = s.Split(new char[] { '/' });
                if (ss2.Length == 1)
                {
                    texts[i] = s;
                    paintColors[i] = m_colors[i % m_colors.Length];
                }
                else
                {
                    texts[i] = ss2[1];
                    paintColors[i] = m_colors[Convert.ToInt32(ss2[0])];
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

            for(int i=0; i<ss.Length; ++i)
            {

                // http://weblogs.asp.net/israelio/archive/2006/07/30/DrawString-_2F00_-MeasureString-Offset-Problem-Solved-_2100_.aspx
                origin = new PointF(nowOffset, 0);
                SizeF sizeTxt = g.MeasureString(texts[i], font, origin, sFormat);

                using (SolidBrush brush = new SolidBrush(paintColors[i]))
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
