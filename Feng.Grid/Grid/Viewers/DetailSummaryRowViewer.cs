using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid.Viewers;
using Xceed.Grid;
using Feng.Utils;

namespace Feng.Grid.Viewers
{
    public class DetailSummaryRowViewer : CellViewerManager
    {
        private string m_detailColumnName;

        /// <summary>
        /// 
        /// </summary>
        public DetailSummaryRowViewer(string detailColumnName)
            : base()
        {
            m_detailColumnName = detailColumnName;
        }

        /// <summary>
        /// GetTextCore
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            DataCell cell = gridElement as DataCell;
            if (cell == null)
                return null;
            if (cell.Value == null)
            {
                Xceed.Grid.DataRow row = cell.ParentRow as Xceed.Grid.DataRow;
                if (row == null)
                    return null;
                if (row.DetailGrids.Count == 0)
                    return null;
                IGrid grid = row.DetailGrids[0] as IGrid;
                if (grid == null)
                    return null;
                SummaryRow sumRow = grid.GetSummaryRow();
                if (sumRow == null)
                    return null;
                cell.Value = ConvertHelper.ChangeType(sumRow.Cells[m_detailColumnName].Value, cell.ParentColumn.DataType);
                return sumRow.Cells[m_detailColumnName].Value == null ? null : sumRow.Cells[m_detailColumnName].Value.ToString();
            }
            else
            {
                return base.GetTextCore(value, formatInfo, gridElement);
            }
        }
    }
}
