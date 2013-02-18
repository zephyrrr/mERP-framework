using System;
using Xceed.Grid;
using System.Drawing;

namespace Feng.Grid
{
    public class MultiColumnHeaderColumnManagerRow : Row
    {
        protected MultiColumnHeaderColumnManagerRow(MultiColumnHeaderColumnManagerRow template)
            : base(template) { }

        public MultiColumnHeaderColumnManagerRow(RowSelector selector)
            : base(selector) { }

        public MultiColumnHeaderColumnManagerRow(int[] columns, string[] captions)
            : base()
        {
            if (columns.Length != captions.Length)
            {
                throw new ArgumentException("column's count should equal caption's count.", "columnNames");
            }
            //int sum = 0;
            //for (int j = 0; j < columns.Length; j++)
            //{
            //    sum += columns[j];
            //}
            //if (sum > this.ParentGrid.Columns.Count)
            //{
            //    throw new ArgumentException("column count should not great than parent column count!");
            //}
            m_columns = columns;
            m_captions = captions;
        }

        protected override Xceed.Grid.Row CreateInstance()
        {
            return new MultiColumnHeaderColumnManagerRow(this);
        }

        protected override void PaintBackground(GridPaintEventArgs e)
        {
            base.PaintBackground(e);
        }

        protected override void PaintForeground(GridPaintEventArgs e)
        {
            int width = 0;
            int lastColumn = 0;

            if (this.ParentGrid.FixedColumnSplitter != null)
            {
                width = this.ParentGrid.FixedColumnSplitter.Width;
            }
            int lastWidth = width;
            for (int j = 0; j < m_columns.Length; j++)
            {
                for (int i = 0; i < m_columns[j]; i++)
                {
                    width += this.ParentGrid.Columns[lastColumn].Width;
                    lastColumn++;
                }

                e.Graphics.DrawLine(this.GridControl.GridLinePen, e.DisplayRectangle.X + width - 1, 0,
                                     e.DisplayRectangle.X + width - 1, this.Height);

                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                drawFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(m_captions[j], this.GridControl.Font, new SolidBrush(e.DisplayVisualStyle.ForeColor),
                    new RectangleF(e.DisplayRectangle.X + lastWidth, 0, e.DisplayRectangle.X + width - lastWidth, this.Height), drawFormat);

                lastWidth = width;
            }
        }

        protected override int DefaultHeight
        {
            get { return 16; }
        }

        protected override int GetFittedDisplayHeight(AutoHeightMode mode, Graphics graphics, bool printing)
        {
            return this.DefaultHeight;
        }

        protected override bool DefaultCanBeCurrent
        {
            get { return false; }
        }

        public override bool IsBackColorAmbient
        {
            get { return false; }
        }

        protected override System.Drawing.Color DefaultBackColor
        {
            get { return SystemColors.Control; }
        }

        private int[] m_columns;
        private string[] m_captions;
    }
}
