using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Xceed.Grid;
using Xceed.Grid.Viewers;

namespace Feng.Grid.Viewers
{
    public class StartStopActionViewer : CellViewerManager
    {
        public StartStopActionViewer()
            : this("Icons.start.png", "Icons.stop.png")
        {
        }

        public StartStopActionViewer(string startImage, string stopImage)
        {
            m_startImage = startImage;
            m_stopImage = stopImage;
        }

        private string m_startImage, m_stopImage;

        protected override ImageArea DefaultImageArea
        {
            get
            {
                return ImageArea.AllContent;
            }
        }

        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            return null;
        }

        protected override Image GetImageCore(object value, CellImageFormatInfo formatInfo, GridElement gridElement)
        {
            if (value == null)
                return null;

            if (!Convert.ToBoolean(value))
                return Feng.Windows.ImageResource.Get(m_startImage).Reference;
            else
                return Feng.Windows.ImageResource.Get(m_stopImage).Reference;
        }

        protected override int GetFittedWidthCore(Cell cell, AutoWidthMode mode, System.Drawing.Graphics graphics, bool printing)
        {
            return 18;
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
            return 18;
        }
    }
}
