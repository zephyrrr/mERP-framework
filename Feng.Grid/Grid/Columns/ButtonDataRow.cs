using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.Grid;

namespace Feng.Grid.Columns
{
    /// <summary>
    /// Class that provides the implementation for our custom row.
    /// This will row will then contain our custom button cell.
    /// </summary>
    public class ButtonDataRow : Xceed.Grid.DataRow
    {
        /// <summary>
        /// 
        /// </summary>
        public ButtonDataRow()
            : base()
        {
        }

        ///// <summary>
        ///// clone old datarow template
        ///// </summary>
        ///// <param name="template"></param>
        //internal ButtonDataRow(DataRow template)
        //    : base(template)
        //{
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        protected ButtonDataRow(ButtonDataRow template)
            : base(template)
        {
        }

        /// <summary>
        /// Necessary only if you want to use your row in the Grid Designer
        /// </summary>
        /// <param name="rowSelector"></param>
        public ButtonDataRow(RowSelector rowSelector)
            : base(rowSelector)
        {
        }


        /// <summary>
        /// Create the cells in the custom row. If the parent columns
        /// datatype is Button, we will create our custom button cell.
        /// If the datatype is something else, we will create the default DataCell.
        ///
        /// We can use both types of cells in our custom row because
        /// the button cell derives from DataCell.
        /// </summary>
        /// <param name="parentColumn"></param>
        /// <returns></returns>
        protected override Cell CreateCell(Column parentColumn)
        {
            if (parentColumn.DataType == typeof(Button))
            {
                ButtonDataCell cell = new ButtonDataCell(parentColumn);
                cell.ButtonClick += new EventHandler<EventArgs>(cell_ButtonClick);
                return cell;
            }
            else
            {
                return base.CreateCell(parentColumn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EventArgs> ButtonClick;

        void cell_ButtonClick(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Row CreateInstance()
        {
            return new ButtonDataRow(this);
        }
    }
}
