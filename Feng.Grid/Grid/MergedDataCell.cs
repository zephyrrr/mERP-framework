//using System;
//using Xceed.Grid;
//using Xceed.Grid.Collections;
//using System.Windows.Forms;
//using System.Drawing;
//using System.Drawing.Drawing2D;

//namespace Feng.Grid
//{
//    /// <summary>
//    /// Summary description for MergedCell.
//    /// </summary>
//    public class MergedDataCell
//    {
//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="topLeftCell"></param>
//        /// <param name="numberOfColumnToMerge"></param>
//        /// <param name="numberOfRowToMerge"></param>
//        public MergedDataCell(DataCell topLeftCell, int numberOfColumnToMerge, int numberOfRowToMerge)
//        {
//            if (topLeftCell == null)
//                throw new ArgumentNullException("TopLeftCell");

//            if (numberOfColumnToMerge < 1)
//                throw new ArgumentOutOfRangeException("NumberOfRowToMerge");

//            if (numberOfRowToMerge < 1)
//                throw new ArgumentOutOfRangeException("NumberOfColumnToMerge");

//            m_topLeftCell = topLeftCell;
//            m_numberOfRowToMerge = numberOfRowToMerge;
//            m_numberOfColumnToMerge = numberOfColumnToMerge;
//        }

//        /// <summary>
//        /// TopLeftCell
//        /// </summary>
//        public DataCell TopLeftCell
//        {
//            get
//            {
//                return m_topLeftCell;
//            }
//        }

//        /// <summary>
//        /// NumberOfRowToMerge
//        /// </summary>
//        public int NumberOfRowToMerge
//        {
//            get
//            {
//                return m_numberOfRowToMerge;
//            }
//        }

//        /// <summary>
//        /// NumberOfColumnToMerge
//        /// </summary>
//        public int NumberOfColumnToMerge
//        {
//            get
//            {
//                return m_numberOfColumnToMerge;
//            }
//        }

//        /// <summary>
//        /// CalculateRectangles
//        /// </summary>
//        internal void CalculateRectangles()
//        {
//            m_bounds = Rectangle.Empty;
//            m_clipRectangle = Rectangle.Empty;

//            GridControl gridControl = m_topLeftCell.GridControl;

//            if (gridControl == null)
//                return;

//            bool validRectangleFound = false;

//            DataRow currentRow = m_topLeftCell.ParentRow as DataRow;
//            ReadOnlyDataRowList dataRows = currentRow.ParentGroup.GetSortedDataRows(false);
//            int dataRowIndex = dataRows.IndexOf(currentRow);
//            int dataRowCount = dataRows.Count;

//            for (int i = 0; i < m_numberOfRowToMerge; i++)
//            {
//                DataRow row = currentRow;
//                Rectangle rowBounds = row.Bounds;

//                if (rowBounds.IsEmpty)
//                {
//                    if (row.Visible)
//                    {
//                        int rowHeight = row.Height;

//                        if (!validRectangleFound)
//                            m_clipRectangle.Y += rowHeight;

//                        m_bounds.Height += rowHeight;
//                    }
//                }
//                else
//                {
//                    if (!validRectangleFound)
//                    {
//                        m_bounds.Y = rowBounds.Y - m_bounds.Height;

//                        bool cellValidRectangleFound = false;

//                        Column currentColumn = m_topLeftCell.ParentColumn;
//                        ColumnList columns = currentRow.ParentGrid.Columns;
//                        int columnVisibleIndex = currentColumn.VisibleIndex;
//                        int columnCount = columns.Count;

//                        for (int j = 0; j < m_numberOfColumnToMerge; j++)
//                        {
//                            Column column = currentColumn;
//                            Rectangle cellBounds = row.Cells[column.Index].Bounds;

//                            if (cellBounds.IsEmpty)
//                            {
//                                if (column.Visible)
//                                {
//                                    int cellWidth = column.Width;

//                                    if (!cellValidRectangleFound)
//                                        m_clipRectangle.X += cellWidth;

//                                    m_bounds.Width += cellWidth;
//                                }
//                            }
//                            else
//                            {
//                                if (!cellValidRectangleFound)
//                                {
//                                    m_bounds.X = cellBounds.X - m_bounds.Width;
//                                }

//                                cellValidRectangleFound = true;

//                                m_bounds.Width += cellBounds.Width;
//                                m_clipRectangle.Width += cellBounds.Width;
//                            }

//                            columnVisibleIndex++;

//                            if (columnVisibleIndex >= columnCount)
//                                break;

//                            currentColumn = columns.GetColumnAtVisibleIndex(columnVisibleIndex);
//                        }
//                    }

//                    validRectangleFound = true;

//                    m_bounds.Height += rowBounds.Height;
//                    m_clipRectangle.Height += rowBounds.Height;
//                }

//                dataRowIndex++;

//                if (dataRowIndex >= dataRowCount)
//                    break;

//                currentRow = dataRows[dataRowIndex];
//            }
//        }

//        /// <summary>
//        /// Paint
//        /// </summary>
//        /// <param name="e"></param>
//        internal void Paint(PaintEventArgs e)
//        {
//            if ((m_clipRectangle.Height == 0) || (m_clipRectangle.Width == 0))
//                return;

//            GraphicsContainer oldContainer = e.Graphics.BeginContainer();

//            try
//            {
//                e.Graphics.TranslateTransform(m_bounds.X, m_bounds.Y);
//                e.Graphics.SetClip(m_clipRectangle);

//                Rectangle displayRectangle = m_bounds;
//                displayRectangle.X = 0;
//                displayRectangle.Y = 0;
//                displayRectangle.Width--;
//                displayRectangle.Height--;

//                this.PaintBorders(e, displayRectangle);
//                this.PaintBackground(e, displayRectangle);
//                this.PaintForground(e, displayRectangle);
//            }
//            finally
//            {
//                e.Graphics.EndContainer(oldContainer);
//            }
//        }

//        private void PaintForground(PaintEventArgs e, Rectangle displayRectangle)
//        {
//            string text = m_topLeftCell.GetDisplayText();

//            if (text.Length != 0)
//            {
//                Rectangle rectangle = displayRectangle;
//                Color foreColor = m_topLeftCell.ForeColor;

//                using (Brush brush = new SolidBrush(foreColor))
//                {
//                    using (StringFormat stringFormat = m_topLeftCell.GetStringFormatToPaint())
//                    {
//                        // We keep one pixel between the text and the borders.
//                        rectangle.Width -= 2;
//                        rectangle.X += 1;
//                        rectangle.Height -= 2;
//                        rectangle.Y += 1;

//                        e.Graphics.DrawString(text, m_topLeftCell.Font, brush, rectangle, stringFormat);
//                    }
//                }
//            }
//        }

//        private void PaintBackground(PaintEventArgs e, Rectangle displayRectangle)
//        {
//            Color backColor = m_topLeftCell.BackColor;

//            if (backColor != Color.Transparent)
//            {
//                using (Brush brush = new SolidBrush(backColor))
//                {
//                    e.Graphics.FillRectangle(brush, displayRectangle);
//                }
//            }
//        }

//        private void PaintBorders(PaintEventArgs e, Rectangle displayRectangle)
//        {
//            GridControl gridControl = m_topLeftCell.GridControl;

//            if (gridControl == null)
//                return;

//            int top = displayRectangle.Top - 1;
//            int bottom = displayRectangle.Bottom;
//            int left = displayRectangle.Left - 1;
//            int right = displayRectangle.Right;

//            // We do not paint the Top or Bottom borders because the row has taken care of it.

//            // Vertical (right) gridline.
//            this.DrawVerticalGridLine(e.Graphics, right, top, bottom - top + 1);
//        }

//        private void DrawVerticalGridLine(Graphics graphics, int x, int y, int height)
//        {
//            Pen pen = m_topLeftCell.GridControl.GridLinePen;
//            graphics.DrawLine(pen, x, y, x, y + height - 1);
//        }

//        private DataCell m_topLeftCell;
//        private int m_numberOfRowToMerge;
//        private int m_numberOfColumnToMerge;

//        private Rectangle m_bounds = Rectangle.Empty;
//        private Rectangle m_clipRectangle = Rectangle.Empty;
//    }
//}
