using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Xceed.Grid.Viewers;
using Xceed.Grid;

namespace Feng.Grid.Viewers
{
    /// <summary>
    /// 对Cell's Value的数字内容显示相应个数星号的Viewer
    /// </summary>
    public class IntImageViewer : CellViewerManager
    {
        public IntImageViewer()
            : this("Icons.exclamationpoint.png")
        {
        }

        public IntImageViewer(string imageName)
            : base()
        {
            m_image = Feng.Windows.ImageResource.Get(imageName).Reference;
        }

        //private static Image s_Image = Feng.Properties.Resources.time_star;
        ///// <summary>
        ///// SetImage
        ///// </summary>
        ///// <param name="image"></param>
        //public static void SetImage(Image image)
        //{
        //    s_Image = image;
        //}

        private Image m_image;

        ///// <summary>
        ///// 从Stream添加Image
        ///// </summary>
        ///// <param name="stream"></param>
        //private static void LoadImage(Stream stream)
        //{
        //    m_Image = new Bitmap(new Bitmap(stream), new Size(16, 16));
        //}

        ///// <summary>
        ///// 从文件读入Image
        ///// </summary>
        ///// <param name="fileName"></param>
        //public static void LoadImage(string fileName)
        //{
        //    m_Image = new Bitmap(new Bitmap(fileName), new Size(16, 16));
        //}

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

        private int m_imageCount;
        /// <summary>
        /// PaintCellForeground
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="e"></param>
        /// <param name="handled"></param>
        protected override void PaintCellForeground(Cell cell, GridPaintEventArgs e, ref bool handled)
        {
            m_imageCount = 0;

            if ((cell.Value != null) && (cell.Value != DBNull.Value) && (cell.Value != cell.NullValue))
            {
                m_imageCount = (int)Convert.ChangeType(cell.Value, typeof(int));
            }

            int x = e.DisplayRectangle.X;
            int y = e.DisplayRectangle.Y + ((e.DisplayRectangle.Height - m_image.Height) / 2);
            //int imageHeight = ImageViewer.m_Image.Height;

            for (int i = 1; i <= m_imageCount; i++)
            {
                e.Graphics.DrawImage(m_image, x, y);
                x += m_image.Width;
            }
            
            handled = true;
        }

        protected override int GetFittedWidthCore(Cell cell, AutoWidthMode mode, Graphics graphics, bool printing)
        {
            return m_image.Width * Math.Max(1, m_imageCount);
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