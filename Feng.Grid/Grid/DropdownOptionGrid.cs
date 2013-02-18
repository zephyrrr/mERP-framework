using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng.Grid
{
    /// <summary>
    /// 带勾选的具有OptionGrid样式的Grid Cell Editor
    /// </summary>
    public class DropdownOptionGrid : MyGrid
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DropdownOptionGrid()
            : base()
        {
            this.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());
            base.GridLineColor = System.Drawing.Color.Transparent;

            this.SingleClickEdit = true;
            this.AddCheckColumn(Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName);
        }

        /// <summary>
        /// Default AutoCreateColumns = false
        /// </summary>
        [DefaultValue(typeof (System.Drawing.Color), "Transparent")]
        public new System.Drawing.Color GridLineColor
        {
            get { return base.GridLineColor; }
            set { base.GridLineColor = value; }
        }

        /// <summary>
        /// SelectCaption
        /// </summary>
        public string SelectColumnName
        {
            get { return Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName; }
        }
    }
}