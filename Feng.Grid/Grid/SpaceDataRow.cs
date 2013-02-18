using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    public class SpaceDataCell : Xceed.Grid.DataCell
    {
        protected SpaceDataCell(SpaceDataCell template)
            : base(template) { }

        public SpaceDataCell(Xceed.Grid.Column parentColumn)
            : base(parentColumn) { }

        protected override Xceed.Grid.Cell CreateInstance()
        {
            return new SpaceDataCell(this);
        }

        public override Xceed.Grid.Borders Borders
        {
            get { return Xceed.Grid.Borders.Empty; }
        }
    }

    public class SpaceDataRow : Xceed.Grid.DataRow
    {
        public SpaceDataRow()
            : base() { }

        protected SpaceDataRow(SpaceDataRow template)
            : base(template) { }

        protected override Xceed.Grid.Cell CreateCell(Xceed.Grid.Column parentColumn)
        {
            return new SpaceDataCell(parentColumn);
        }

        protected override Xceed.Grid.Row CreateInstance()
        {
            return new SpaceDataRow(this);
        }
    }

    public static class SpaceDataRowExtention
    {
        public static void AddSpaceDataRow(this IGrid grid)
        {
            var oldTemplate = grid.DataRowTemplate;
            grid.DataRowTemplate = new SpaceDataRow();
            var row = grid.DataRows.AddNew();
            row.Height = 5;
            grid.DataRowTemplate = oldTemplate;
        }
    }
}
