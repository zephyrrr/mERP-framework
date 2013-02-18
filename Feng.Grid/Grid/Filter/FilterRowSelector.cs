/*
 * Xceed Grid for .NET - Extensibility Sample Application
 * Copyright (c) 2002-2007 Xceed Software Inc.
 * 
 * [FilterRowSelector.cs]
 *  
 * This class demonstrates how to create classes that derive from the 
 * RowSelector class in order to add interactivity to the FilterRow.
 * 
 * This file is part of Xceed Grid for .NET. The source code in this file 
 * is only intended as a supplement to the documentation, and is provided 
 * "as is", without warranty of any kind, either expressed or implied.
 */

using System;
using System.Drawing;
using System.IO;
using Xceed.Grid;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// The FilterRowSelector is the default FilterRow's RowSelector.
    /// It adds a enable/disable on click functionality and an icon showing the current status
    /// of the FilterRow.
    /// </summary>
    public class FilterRowSelector : RowSelector
    {
        private static Image FilterAppliedImage;
        private static Image FilterUnAppliedImage;

        static FilterRowSelector()
        {
            using (Stream stream = typeof(FilterRow).Assembly.GetManifestResourceStream("Feng.Grid.Filter.FilterApplied16by24.png"))
            {
                FilterAppliedImage = new Bitmap(new Bitmap(stream));
            }

            using (Stream stream = typeof(FilterRow).Assembly.GetManifestResourceStream("Feng.Grid.Filter.FilterUnApplied16by24.png"))
            {
                FilterUnAppliedImage = new Bitmap(new Bitmap(stream));
            }
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

            Image filterIcon = FilterRowSelector.FilterUnAppliedImage;

            if (filterRow.Enabled)
            {
                filterIcon = FilterRowSelector.FilterAppliedImage;
            }

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
                filterRow.Enabled = !filterRow.Enabled;
            }

            base.OnClick(e);
        }
    }
}