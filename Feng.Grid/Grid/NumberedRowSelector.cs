/*
* Xceed Grid for .NET - Custom UI Sample Application
* Copyright (c) 2002-2007 Xceed Software Inc.
*
* [NumberedRowSelector.cs]
*
* This file demonstrates how to derive from the RowSelector
* class to create row selector that displays the index of its corresponding
* data row either in relation to the data row's position in the data source
* or according to the data rows position in a group.
*
* This file is part of Xceed Grid for .NET. The source code in this file
* is only intended as a supplement to the documentation, and is provided
* "as is", without warranty of any kind, either expressed or implied.
*/

using System;
using System.Drawing;
using Xceed.Grid;

namespace Feng.Grid
{
    public class NumberedRowSelector : RowSelector
    {
        // Initializes a new instance of the NumberedRowSelector class.
        public NumberedRowSelector()
        {
        }

        // Initializes a new instance of the NumberedRowSelector class specifying the
        // template that will be used to create other selectors.
        protected NumberedRowSelector(NumberedRowSelector template, Row parentRow)
            : base(template, parentRow)
        {
        }

        // Creates a clone of the NumberedRowSelector.
        protected override RowSelector CreateInstance(Row parentRow)
        {
            return new NumberedRowSelector(this, parentRow);
        }

        private static Font s_font = new Font(Xceed.Grid.GridControl.DefaultFont.FontFamily, Xceed.Grid.GridControl.DefaultFont.Size - 4);

        // Paints the foreground of the NumberedRowSelector.
        protected override void PaintForeground(GridPaintEventArgs e)
        {
            Xceed.Grid.DataRow dataRow = this.Row as Xceed.Grid.DataRow;
		
			// Only paint the datarows
			if (dataRow != null)
			{
				int number = 0;
				number = dataRow.Index + 1;
				// number = dataRow.ParentGroup.GetSortedDataRows( false ).IndexOf( dataRow );

				// It is recommended to use the color returned from GetForeColorToPaint()
				// rather than the ForeColor property directly.
				using (SolidBrush brush = new SolidBrush(this.GetDisplayVisualStyle(Xceed.Grid.VisualGridElementState.InactiveSelection).ForeColor))
				{
					Rectangle offsetRectangle = this.ClientRectangle;
					offsetRectangle.Offset(0, 1);
                    e.Graphics.DrawString(number.ToString(), s_font, brush, offsetRectangle);
				}
			}
            else
            {
                // Not a DataRow, so let the base class paint itself.
                base.PaintForeground(e);
            }
            
        }

    }
}