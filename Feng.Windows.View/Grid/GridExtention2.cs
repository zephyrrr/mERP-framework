using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Grid
{
    public static class GridManageRowExtention2
    {
        /// <summary>
        /// 加入SummaryRow
        /// </summary>
        /// <returns></returns>
        public static MySummaryRow AddSummaryRowToFixedFooter(this IGrid grid)
        {
            MySummaryRow sumRow = new MySummaryRow();
            sumRow.CanBeCurrent = false;
            sumRow.CanBeSelected = false;
            sumRow.BackColor = System.Drawing.Color.LightBlue;
            sumRow.RunningStatGroupLevel = 0;
            grid.FixedFooterRows.Add(sumRow);
            sumRow.TextFormat = "合计";
            return sumRow;
        }

        /// <summary>
        /// 加入合计的SummaryRow
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static MySummaryRow AddSummaryRowToFixedFooter(this IGrid grid, string columnName)
        {
            return AddSummaryRowToFixedFooter(grid, new string[] { columnName });
        }

        /// <summary>
        /// 加入合计的SummaryRow
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public static MySummaryRow AddSummaryRowToFixedFooter(this IGrid grid, string[] columnNames)
        {
            MySummaryRow row = AddSummaryRowToFixedFooter(grid);
            foreach (string columnName in columnNames)
            {
                ((MySummaryCell)row.Cells[columnName]).StatFunction = Xceed.Grid.StatFunction.Sum;
            }
            return row;
        }
    }
}
