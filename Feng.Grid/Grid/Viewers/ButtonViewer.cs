using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Xceed.Grid;

namespace Feng.Grid.Viewers
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonViewer : Xceed.Grid.Viewers.CellViewerManager
    {
        /// <summary>
        /// 
        /// </summary>
        public ButtonViewer()
            : this("ฯ๊ว้")
        {
        }

        private string m_text;

        /// <summary>
        /// 
        /// </summary>
        public ButtonViewer(string text)
            : base(new Button(), "Text")
        {
            m_text = text;
            ((Button)this.Control).Text = text;
            ((Button) this.Control).TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //((Button)this.Control).BorderStyle = BorderStyle.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        protected override void SetControlValueCore(Xceed.Grid.Cell cell)
        {
            ((Button)this.Control).Text = m_text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="mode"></param>
        /// <param name="cellDisplayWidth"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedHeightCore(Xceed.Grid.Cell cell, Xceed.Grid.AutoHeightMode mode, int cellDisplayWidth, System.Drawing.Graphics graphics, bool printing)
        {
            return (int)Math.Ceiling(cell.Font.GetHeight()) + 4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="mode"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedWidthCore(Cell cell, AutoWidthMode mode, Graphics graphics, bool printing)
        {
            return 20;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="e"></param>
        /// <param name="handled"></param>
        protected override void PaintCellForeground(Xceed.Grid.Cell cell, Xceed.Grid.GridPaintEventArgs e, ref bool handled)
        {
            if ((Control.MouseButtons == MouseButtons.Left) &&
              (cell.RectangleToScreen(e.ClientRectangle).Contains(Control.MousePosition)))
            {
                ControlPaint.DrawButton(e.Graphics, e.DisplayRectangle, ButtonState.Pushed);
            }
            else
            {
                ControlPaint.DrawButton(e.Graphics, e.DisplayRectangle, ButtonState.Normal);
            }

            //using (SolidBrush brush = new SolidBrush(cell.GetDisplayVisualStyle(VisualGridElementState.Idle).ForeColor))
            //{
            //    Rectangle textRectangle = e.DisplayRectangle;
            //    textRectangle.Inflate(-2, -2);

            //    if ((textRectangle.Height > 0) && (textRectangle.Width > 0))
            //    {
            //        // Paint a fixed text
            //        e.Graphics.DrawString(m_text, cell.Font, brush, textRectangle);

            //        // For text that follows the value of the cell, you could do someting like the following :
            //        // e.Graphics.DrawString( this.GetTextToPaint(), this.Font, brush, textRectangle );
            //    }
            //}
        }

    }
}