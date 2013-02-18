using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid.Viewers;
using Xceed.Grid;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Feng.Utils;

namespace Feng.Grid.Viewers
{
    /// <summary>
    /// 对Cell's Value的数字内容进行对应图形显示的Viewer
    /// </summary>
    public class ImageTextViewer : CellViewerManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imageNames"></param>
        public ImageTextViewer(string imageNames)
            : this(imageNames, string.Empty)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imageNames"></param>
        /// <param name="textNames"></param>
        public ImageTextViewer(string imageNames, string textNames)
            : base()
        {
            this.ImageArea = ImageArea.AllContent;

            string[] ss = imageNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ss2 = textNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for(int i=0; i<ss.Length; ++i)
            {
                if (Feng.Windows.ImageResource.Get(ss[i]).Reference != null)
                {
                    if (i < ss2.Length)
                    {
                        m_images[ss2[i]] = Feng.Windows.ImageResource.Get(ss[i]).Reference;
                    }
                    else
                    {
                        m_images[ss[i]] = Feng.Windows.ImageResource.Get(ss[i]).Reference;
                    }
                }
            }

        }

        private Dictionary<string, Image> m_images = new Dictionary<string, Image>();

        //private static List<Image> s_Images = new List<Image>();
        ///// <summary>
        ///// static Contructor
        ///// </summary>
        //static MultiImageViewer()
        //{
        //    SetImages(new Image[]
        //              {
        //                  Feng.Properties.Resources.time_star,
        //                  Feng.Properties.Resources.time_moon,
        //                  Feng.Properties.Resources.time_sun
        //              });
        //}
        ///// <summary>
        ///// Set Viewer's Images
        ///// </summary>
        ///// <param name="images"></param>
        //public static void SetImages(Image[] images)
        //{
        //    s_Images.Clear();
        //    foreach (Image image in images)
        //    {
        //        s_Images.Add(image);
        //    }
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        public ImageTextViewer()
            : base()
        {
        }

        private const int imageWidth = 16;
        private const int imageHeight = 16;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override Image GetImageCore(object value, CellImageFormatInfo formatInfo, GridElement gridElement)
        {
            if (m_images.Count == 0)
            {
                return null;
            }

            Image ret = null;
            if ((value != null) && (value != DBNull.Value) && (value != formatInfo.NullValue))
            {
                string[] ss = value.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in ss)
                {
                    if (m_images.ContainsKey(s))
                    {
                        Image image = Feng.Windows.Utils.ImageHelper.DrawImageBackgound(m_images[s], Color.White, imageWidth, imageHeight);
                        if (ret == null)
                        {
                            ret = image;
                        }
                        else
                        {
                            ret = Feng.Windows.Utils.ImageHelper.ConcatImage(ret, image, Feng.Windows.Utils.ImageHelper.ImageConcatType.Horizontal);
                        }
                    }
                }
            }
            return ret;
        }

        ///// <summary>
        ///// PaintCellBackground
        ///// </summary>
        ///// <param name="cell"></param>
        ///// <param name="e"></param>
        ///// <param name="handled"></param>
        //protected override void PaintCellBackground(Cell cell, GridPaintEventArgs e, ref bool handled)
        //{
        //    if (m_images.Count == 0)
        //    {
        //        return;
        //    }

        //    if ((cell.Value != null) && (cell.Value != DBNull.Value) && (cell.Value != cell.NullValue))
        //    {
        //        string[] ss = cell.Value.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string s in ss)
        //        {
        //            int height = e.DisplayRectangle.Height;
        //            int cnt = 0;
        //            if (m_images.ContainsKey(s))
        //            {
        //                int x = e.DisplayRectangle.X + height * cnt;
        //                int y = e.DisplayRectangle.Y; // +((e.DisplayRectangle.Height - m_images[s].Height) / 2);
        //                //int imageHeight = MultiImageViewer.m_Images[0].Height;

        //                e.Graphics.FillRectangle(new SolidBrush(Color.Red), x, y, height, height);

        //                e.Graphics.DrawImage(m_images[s], x, y, height, height);
        //                cnt++;
        //            }
        //        }
        //    }

        //    // Notify the cell that its background has been painted by the CellViewerManager.
        //    handled = true;
        //}

        ///// <summary>
        ///// PaintCellForeground
        ///// </summary>
        ///// <param name="cell"></param>
        ///// <param name="e"></param>
        ///// <param name="handled"></param>
        //protected override void PaintCellForeground(Cell cell, GridPaintEventArgs e, ref bool handled)
        //{
        //    // Notify the cell that its foreground has been painted by the CellViewerManager.
        //    handled = true;
        //}

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
            return imageHeight + 2;
        }

        //protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        //{
        //    if (m_texts.Count == 0)
        //    {
        //        return string.Empty;
        //    }

        //    if ((value != null) && (value != DBNull.Value) && (value != formatInfo.NullValue))
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        string[] ss = value.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //        int quantity = -1;
        //        foreach (string s in ss)
        //        {
        //            quantity = -1;
        //            quantity = ConvertHelper.ToInt(s);
        //            if (quantity != -1)
        //            {
        //                sb.Append(m_texts[quantity]);
        //            }
        //            else
        //            {
        //                sb.Append(s);
        //            }
        //            sb.Append(",");
        //        }
        //        return sb.Remove(sb.Length - 1, 1).ToString();
        //    }
        //    return string.Empty;

        //}
    }
}