using System;
using System.Drawing;
using System.IO;
using Xceed.Grid;

namespace Feng.Grid.TextFilter
{
    /// <summary>
    /// FilterRowSelector
    /// </summary>
    public class FilterRowSelector : RowSelector
    {
        private static Image s_searchImage;

        static FilterRowSelector()
        {
            s_searchImage = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRowSearch.png").Reference;
        }

        /// <summary>
        /// 
        /// </summary>
        public FilterRowSelector()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="parentRow"></param>
        protected FilterRowSelector(FilterRowSelector template, Row parentRow)
            : base(template, parentRow)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentRow"></param>
        /// <returns></returns>
        protected override RowSelector CreateInstance(Row parentRow)
        {
            return new FilterRowSelector(this, parentRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void PaintForeground(GridPaintEventArgs e)
        {
            FilterRow filterRow = this.Row as FilterRow;

            if (filterRow == null)
            {
                return;
            }

            Image filterIcon = s_searchImage;

            Rectangle rectangle = this.DisplayRectangle;

            int left = rectangle.Left;
            int top = rectangle.Top;

            left += (int) (rectangle.Width - filterIcon.Width) / 2;
            top += (int) (rectangle.Height - filterIcon.Height) / 2;

            e.Graphics.DrawImage(filterIcon, new Point(left, top));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            FilterRow filterRow = this.Row as FilterRow;

            if (filterRow != null)
            {
                filterRow.EndEdit();
                filterRow.ApplyFilters();
            }

            base.OnClick(e);
        }
    }
}