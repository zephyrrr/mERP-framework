using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Xceed.Grid.Viewers;
using Xceed.Grid;

namespace Feng.Grid.Viewers
{
    public class BoolImageViewer : CellViewerManager
    {public BoolImageViewer()
            : this("Icons.time_star.gif", true)
        {
        }

        public BoolImageViewer(string imageName, bool? paintWhenTrue)
            : base()
        {
            m_image = Feng.Windows.ImageResource.Get(imageName).Reference;
            m_paintWhenTrue = paintWhenTrue.HasValue ? paintWhenTrue.Value : true;
        }

        private Image m_image;
        private bool m_paintWhenTrue;

        /// <summary>
        /// PaintCellBackground
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="e"></param>
        /// <param name="handled"></param>
        protected override void PaintCellBackground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            handled = true;
        }

        private const int m_imageCount = 1;
        /// <summary>
        /// PaintCellForeground
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="e"></param>
        /// <param name="handled"></param>
        protected override void PaintCellForeground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            bool value = false;
            if ((cell.Value != null) && (cell.Value != DBNull.Value) && (cell.Value != cell.NullValue))
            {
                value = (bool)Convert.ChangeType(cell.Value, typeof(bool));
            }

            handled = true;

            if (m_paintWhenTrue && !value)
                return;
            if (!m_paintWhenTrue && value)
                return;

            int x = e.DisplayRectangle.X;
            int y = e.DisplayRectangle.Y + ((e.DisplayRectangle.Height - m_image.Height) / 2);
            e.Graphics.DrawImage(m_image, x, y);
        }

        protected override int GetFittedWidthCore(Cell cell, AutoWidthMode mode, Graphics graphics, bool printing)
        {
            return m_image.Width;
        }

        /// <summary>
        /// GetFittedHeightCore
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="mode"></param>
        /// <param name="cellDisplayWidth"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedHeightCore(Cell cell, AutoHeightMode mode, int cellDisplayWidth,
                                                   Graphics graphics, bool printing)
        {
            return m_image.Height + 2;
        }
    }
}
