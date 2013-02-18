using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Xceed.Grid.Viewers;
using Xceed.Grid;

namespace Feng.Grid.Viewers
{
    public class IntMultiImageViewer : CellViewerManager
    {
        public IntMultiImageViewer()
            : this("Icons.time_sun.gif", "Icons.time_yueliang.gif")
        {
        }

        public IntMultiImageViewer(string imageNameLeft, string imageNameRight)
            : base()
        {
            m_imageLeft = Feng.Windows.ImageResource.Get(imageNameLeft).Reference;
            m_imageRight = Feng.Windows.ImageResource.Get(imageNameRight).Reference;
        }

        private Image m_imageLeft, m_imageRight;

        /// <summary>
        /// PaintCellBackground
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="e"></param>
        /// <param name="handled"></param>
        protected override void PaintCellBackground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            // Notify the cell that its background has been painted by the CellViewerManager.
            handled = true;
        }

        private int m_imageLeftCount, m_imageRightCount;
        /// <summary>
        /// PaintCellForeground
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="e"></param>
        /// <param name="handled"></param>
        protected override void PaintCellForeground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            m_imageLeftCount = 0;
            m_imageRightCount = 0;

            string value = null;
            if ((cell.Value != null) && (cell.Value != DBNull.Value) && (cell.Value != cell.NullValue))
            {
                value = (string)Convert.ChangeType(cell.Value, typeof(string));
            }
            if (string.IsNullOrEmpty(value))
                return;

            string[] ss = value.Split(new char[] { '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length < 2)
                return;

            int? leftCount = Feng.Utils.ConvertHelper.ToInt(ss[0]);
            int? rightCount = Feng.Utils.ConvertHelper.ToInt(ss[1]);

            int x = e.DisplayRectangle.X;
            int y = e.DisplayRectangle.Y + ((e.DisplayRectangle.Height - m_imageLeft.Height) / 2);
            if (leftCount.HasValue)
            {
                for (int i = 0; i < leftCount.Value; i++)
                {
                    e.Graphics.DrawImage(m_imageLeft, x, y);
                    x += m_imageLeft.Width;
                }
                m_imageLeftCount += leftCount.Value;
            }
            if (rightCount.HasValue)
            {
                for (int i = 0; i < rightCount.Value; i++)
                {
                    e.Graphics.DrawImage(m_imageRight, x, y);
                    x += m_imageRight.Width;
                }
                m_imageRightCount += rightCount.Value;
            }


            handled = true;
        }

        protected override int GetFittedWidthCore(Cell cell, AutoWidthMode mode, Graphics graphics, bool printing)
        {
            return m_imageLeft.Width * Math.Min(1, m_imageLeftCount) + m_imageRight.Width * Math.Min(1, m_imageRightCount);
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
            return Math.Max(m_imageLeft.Height, m_imageRight.Height) +2;
        }
    }
}
