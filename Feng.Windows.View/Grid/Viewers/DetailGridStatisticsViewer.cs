using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Xceed.Grid;
using Feng.Windows.Utils;

namespace Feng.Grid.Viewers
{
    public class DetailGridStatisticsViewer : Xceed.Grid.Viewers.CellViewerManager
    {
        /// <summary>
        /// 
        /// </summary>
        public DetailGridStatisticsViewer(string statisticsFormat)
            : base()
        {
            m_statisticsFormat = statisticsFormat;
        }

        private string m_statisticsFormat;

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
                MyDetailGrid detailGrid = row.DetailGrids[0] as MyDetailGrid;
                if (detailGrid == null)
                    return null;
               
                string functionName, fieldName, format, where;
                int level = -1;

                MySummaryRow.ExtractParameters(m_statisticsFormat, out functionName, out fieldName, out format, ref level, out where);
                cell.Value = Feng.Utils.ConvertHelper.ChangeType(MySummaryRow.GetVariableFunctionResult(detailGrid.GetSortedDataRows(true),
                                            functionName, fieldName, format, level, where), cell.ParentColumn.DataType);
            }
            return base.GetTextCore(value, formatInfo, gridElement);
        }
    }
}
