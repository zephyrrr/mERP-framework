using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Xceed.Grid;

namespace Feng.Grid.Columns
{
    /// <summary>
    ///  Class that provides the implementation for our button cell.
    /// </summary>
    public class ButtonDataCell : DataCell
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        protected ButtonDataCell(ButtonDataCell template)
            : base(template)
        {
            this.ButtonClick = template.ButtonClick;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        public ButtonDataCell(Column parentColumn)
            : base(parentColumn)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Cell CreateInstance()
        {
            return new ButtonDataCell(this);
        }

        /// <summary>
        /// The button look is determined in this override.
        /// </summary>
        /// <param name="e"></param>
        protected override void PaintForeground(GridPaintEventArgs e)
        {
            if ((Control.MouseButtons == MouseButtons.Left) &&
              (this.RectangleToScreen(e.ClientRectangle).Contains(Control.MousePosition)))
            {
                ControlPaint.DrawButton(e.Graphics, e.DisplayRectangle, ButtonState.Pushed);
            }
            else
            {
                ControlPaint.DrawButton(e.Graphics, e.DisplayRectangle, ButtonState.Normal);
            }

            using (SolidBrush brush = new SolidBrush(this.GetDisplayVisualStyle(VisualGridElementState.Idle).ForeColor))
            {
                Rectangle textRectangle = e.DisplayRectangle;
                textRectangle.Inflate(-2, -2);

                if ((textRectangle.Height > 0) && (textRectangle.Width > 0))
                {
                    // Paint a fixed text
                    e.Graphics.DrawString("编辑", this.Font, brush, textRectangle);

                    // For text that follows the value of the cell, you could do someting like the following :
                    // e.Graphics.DrawString( this.GetTextToPaint(), this.Font, brush, textRectangle );
                }
            }
        }

        // Remove the grid lines for a better button look
        /// <summary>
        /// 
        /// </summary>
        public override Borders Borders
        {
            get { return Borders.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override bool DefaultCanBeCurrent
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="cellDisplayWidth"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedDisplayHeight(AutoHeightMode mode, int cellDisplayWidth, Graphics graphics, bool printing)
        {
            // +4 is for the button's borders
            return (int)Math.Ceiling(this.Font.GetHeight()) + 4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedDisplayWidth(AutoWidthMode mode, Graphics graphics, bool printing)
        {
            // Assuming a fixed with of 20 pixels
            return 20;
        }

        // The overrides of the On* methods is to allow the grid
        // to refresh itself and have our custom button cell
        // provide the same behavior as a regular button.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Invalidate();
            this.GridControl.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.Invalidate();
            this.GridControl.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            this.Invalidate();
            this.GridControl.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.Invalidate();
            this.GridControl.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> ButtonClick;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            //this.GridControl.CurrentRow = this.ParentRow;
            //this.GridControl.SelectedRows.Clear();
            //this.GridControl.SelectedRows.Add(this.ParentRow);

            if (ButtonClick != null)
            {
                ButtonClick(this, e);
            }
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class ButtonCellClickEventArgs : EventArgs
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="parentRow"></param>
    //    public ButtonCellClickEventArgs(CellRow parentRow)
    //    {
    //        m_parentRow = parentRow;
    //    }

    //    private CellRow m_parentRow;

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public CellRow ParentRow
    //    {
    //        get { return m_parentRow; }
    //    }
    //}
}
