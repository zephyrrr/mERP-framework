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
    /// Viewer for object
    /// </summary>
    public class ObjectViewer : CellViewerManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="displayMember"></param>
        public ObjectViewer(string displayMember)
            : base()
        {
            m_displayMember = displayMember;
        }

        private string m_displayMember;

        /// <summary>
        /// GetTextCore
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            if (value == null)
            {
                return string.Empty;
                // 如果返回null会引发GetFittedWidth异常
                // return null;
            }
            else
            {
                // 不带引号
                return EntityHelper.ReplaceEntity(m_displayMember, value, null);
            }
        }
    }
}
