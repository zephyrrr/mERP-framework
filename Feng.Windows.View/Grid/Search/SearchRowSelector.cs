using System;
using System.Drawing;
using System.IO;
using Xceed.Grid;

namespace Feng.Grid.Search
{
    /// <summary>
    /// The SearchRowSelector is the default SearchRow's RowSelector.
    /// </summary>
    public class SearchRowSelector : RowSelector
    {
        private static Image m_searchImage;

        static SearchRowSelector()
        {
            m_searchImage = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRowSearch.png").Reference;
        }

        /// <summary>
        /// 
        /// </summary>
        public SearchRowSelector()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="parentRow"></param>
        protected SearchRowSelector(SearchRowSelector template, Row parentRow)
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
            return new SearchRowSelector(this, parentRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void PaintForeground(GridPaintEventArgs e)
        {
            SearchRow filterRow = this.Row as SearchRow;

            if (filterRow == null)
            {
                return;
            }

            Image filterIcon = SearchRowSelector.m_searchImage;

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
            SearchRow filterRow = this.Row as SearchRow;

            if (filterRow != null)
            {
                filterRow.EndEdit();
                filterRow.ApplyFilters();
            }

            base.OnClick(e);
        }
    }
}