using System;
using System.Drawing;
using System.Collections.Generic;
using Xceed.Grid;
using Xceed.Grid.Collections;

namespace Feng.Grid.TextFilter
{
    /// <summary>
    /// FilterRow
    /// </summary>
    public class FilterRow : CellRow
    {
        protected override Type CellType
        {
            get { return FilterCell.FilterCellType; }
        }
        /// <summary>
        /// 
        /// </summary>
        public FilterRow()
            : base(new FilterRowSelector())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        protected FilterRow(FilterRow template)
            : base(template)
        {
        }


        private List<Column> m_savedSortedColumns = new List<Column>();
        /// <summary>
        /// 
        /// </summary>
        public void ApplyFilters()
        {
            List<Xceed.Grid.DataRow> list = new List<DataRow>();
            int rowsCount = this.ParentGrid.GetSortedDataRowCount(false);
            int cellsCount = this.Cells.Count;

            bool hasFilter = false;
            for (int i = 0; i < rowsCount; ++i)
            {
                bool? ret = null;
                Xceed.Grid.DataRow row = this.ParentGrid.GetSortedDataRows(false)[i];
                for (int j = 0; j < cellsCount; j++)
                {
                    if (!this.Cells[j].Visible)
                        continue;
                    if (this.Cells[j].ReadOnly)
                        continue;
                    if (!(this.Cells[j].CellEditorManager is FilterEditor))
                        continue;

                    string toFilter = (string)this.Cells[j].Value;
                    if (string.IsNullOrEmpty(toFilter))
                        continue;

                    hasFilter = true;
                    string cellText = row.Cells[j].GetDisplayText();
                    if (cellText.Contains(toFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!ret.HasValue)
                            ret = true;
                    }
                    else
                    {
                        ret = false;
                        break;
                    }
                }
                if (ret.HasValue && ret.Value)
                {
                    list.Add(row);
                }
            }

            if (list.Count > 0)
            {
                if (this.ParentGrid.Columns[TextFilterSelectColumnName].SortDirection != SortDirection.Ascending)
                {
                    m_savedSortedColumns.Clear();
                    foreach (Column i in this.ParentGrid.SortedColumns)
                    {
                        m_savedSortedColumns.Add(i);
                    }
                }

                this.ParentGrid.SortedColumns.Clear();
                this.ParentGrid.Columns[TextFilterSelectColumnName].SortDirection = SortDirection.Ascending;

                foreach (Xceed.Grid.DataRow row in this.ParentGrid.DataRows)
                {
                    if (list.Contains(row))
                    {
                        row.Cells[TextFilterSelectColumnName].Value = false;
                        row.Font = new Font(row.Font, FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells[TextFilterSelectColumnName].Value = true;
                        row.Font = new Font(row.Font, FontStyle.Regular);
                    }
                }
            }
            else
            {
                this.ParentGrid.SortedColumns.Clear();
                foreach (var i in m_savedSortedColumns)
                {
                    this.ParentGrid.SortedColumns.Add(i);
                }

                foreach (Xceed.Grid.DataRow row in this.ParentGrid.DataRows)
                {
                    row.Cells[TextFilterSelectColumnName].Value = true;
                    row.Font = new Font(row.Font, FontStyle.Regular);
                }

                if (hasFilter)
                {
                    ServiceProvider.GetService<IMessageBox>().ShowInfo("无符合条件的记录。");
                }
            }
        }

        public const string TextFilterSelectColumnName = "TextFilterSelect";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Row CreateInstance()
        {
            return new FilterRow(this);
        }

        protected override Cell CreateCell(Column parentColumn)
        {
            FilterCell cell = new FilterCell(parentColumn);
            if (!parentColumn.Visible)
            {
                cell.ReadOnly = true;
            }
            else
            {
                cell.ReadOnly = false;
                cell.CellEditorManager = new FilterEditor();
            }
            return cell;
        }
    }
}
