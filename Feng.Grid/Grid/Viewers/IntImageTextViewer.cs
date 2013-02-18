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
    public class IntImageTextViewer : TextViewer
    {
        public IntImageTextViewer()
            : this(new string[] { "Red", "Green", "Blue", "Yellow", "Aqua", "Brown", "Olive", "Orange", "Purple", "Violet"})
        {
        }

        public IntImageTextViewer(string[] colors)
        {
            this.ImageArea = ImageArea.Left;

            m_colors = new Color[colors.Length];
            for (int i = 0; i < colors.Length; ++i)
            {
                m_colors[i] = Color.FromName(colors[i]);
            }
        }

        private Color[] m_colors;
        protected override Image GetImageCore(object value, CellImageFormatInfo formatInfo, GridElement gridElement)
        {
            string s = (string)value;
            if (string.IsNullOrEmpty(s))
                return null;
            string[] ss = s.Split(new char[] { '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length < 1)
                return null;
            int? idx = ConvertHelper.ToInt(ss[0]);
            if (!idx.HasValue)
                return null;

            Image image = new Bitmap(16, 16);
            //image = ImageHelper.DrawImageBackgound(image, Color.White, 16, 16);
            Graphics g = Graphics.FromImage(image);
            g.FillEllipse(new SolidBrush(m_colors[idx.Value % m_colors.Length]), 0, 0, image.Width, image.Height);

            return image;
        }

        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            string s = (string)value;
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            string[] ss = s.Split(new char[] { '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length < 2)
                return string.Empty;
            return ss[1];
        }
    }
}
