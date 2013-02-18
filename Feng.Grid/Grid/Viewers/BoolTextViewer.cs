using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Xceed.Grid.Viewers;
using Xceed.Grid;

namespace Feng.Grid.Viewers
{
    public class BoolTextViewer : CellViewerManager
    {
        public BoolTextViewer()
            : this("是", "否")
        {
        }

        public BoolTextViewer(string yesText, string noText)
            : base()
        {
            m_yesText = yesText;
            m_noText = noText;
        }

        private string m_yesText, m_noText;

        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            bool? v = Feng.Utils.ConvertHelper.ToBoolean(value);
            if (v.HasValue)
            {
                if (v.Value)
                    return m_yesText;
                else
                    return m_noText;
            }
            else
            {
                return null;
            }
        }
    }
}
