//using System;
//using System.Text;
//using Xceed.Grid.Viewers;
//using Xceed.Grid;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.IO;

//namespace Feng.Grid.Viewers
//{
//    /// <summary>
//    /// TrackingData
//    /// </summary>
//    public class TrackingData
//    {
//        /// <summary>
//        /// d
//        /// </summary>
//        public string[] address = new string[] { "宁波", "杭州", "宁波" };

//        /// <summary>
//        /// d
//        /// </summary>
//        public int count = 3;
//    }

//    /// <summary>
//    /// 对Cell's Value的内容显示路线
//    /// </summary>
//    public class TrackingViewer : CellViewerManager
//    {
//        /// <summary>
//        /// Construcotr
//        /// </summary>
//        public TrackingViewer()
//            : base()
//        {
//        }

//        /// <summary>
//        /// PaintCellBackground
//        /// </summary>
//        /// <param name="cell"></param>
//        /// <param name="e"></param>
//        /// <param name="handled"></param>
//        protected override void PaintCellBackground(Cell cell, GridPaintEventArgs e, ref bool handled)
//        {
//            TrackingData data = new TrackingData();

//            int x = e.DisplayRectangle.X;
//            int y = e.DisplayRectangle.Y;
//            int width = e.DisplayRectangle.Width;
//            int height = e.DisplayRectangle.Height;

//            int oneWidth = 100;

//            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 0, 0)), x + oneWidth * (0), y, 2*oneWidth, height);
//            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 255, 0)), x + oneWidth * (1), y, oneWidth, height);
//            for (int i = 0; i < data.address.Length; ++i)
//            {
//                if (i != data.address.Length -1)
//                    e.Graphics.DrawString(data.address[i]+"      -----", new Font("Arial", 10), new SolidBrush(Color.Black), new Rectangle(x + oneWidth * (i), y, oneWidth, height));
//                else
//                    e.Graphics.DrawString(data.address[i], new Font("Arial", 10), new SolidBrush(Color.Black), new Rectangle(x + oneWidth * (i), y, oneWidth, height));
//            }

//            // Notify the cell that its background has been painted by the CellViewerManager.
//            handled = true;
//        }

//        /// <summary>
//        /// PaintCellForeground
//        /// </summary>
//        /// <param name="cell"></param>
//        /// <param name="e"></param>
//        /// <param name="handled"></param>
//        protected override void PaintCellForeground(Cell cell, GridPaintEventArgs e, ref bool handled)
//        {
//            // Notify the cell that its foreground has been painted by the CellViewerManager.
//            handled = true;
//        }

//        /// <summary>
//        /// GetFittedWidthCore
//        /// </summary>
//        /// <param name="cell"></param>
//        /// <param name="mode"></param>
//        /// <param name="graphics"></param>
//        /// <param name="printing"></param>
//        /// <returns></returns>
//        protected override int GetFittedWidthCore(Cell cell, AutoWidthMode mode, Graphics graphics, bool printing)
//        {
//            return 100 * 3;
//        }
//    }
//}
