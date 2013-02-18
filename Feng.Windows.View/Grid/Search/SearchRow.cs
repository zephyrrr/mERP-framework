using System;
using System.Drawing;
using System.Collections.Generic;
using Xceed.Grid;
using Xceed.Grid.Collections;

namespace Feng.Grid.Search
{
    /// <summary>
    /// The SearchRow class derives from the Xceed.Grid.ValueRow and provides
    /// search functionalities to the grid it is added to.
    /// </summary>
    public class SearchRow : ValueRow
    {
        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        public SearchRow()
            : base(new SearchRowSelector())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        protected SearchRow(SearchRow template)
            : base(template)
        {
        }

        #endregion CONSTRUCTORS

        /// <summary>
        /// Enables or disables the SearchRow.
        /// </summary>
        public bool Enabled
        {
            get { return m_enabled; }

            set
            {
                if (m_enabled == value)
                {
                    return;
                }

                m_enabled = value;
                this.ReadOnly = !m_enabled;
                foreach (Cell cell in this.Cells)
                {
                    cell.ReadOnly = !m_enabled;
                }

                if (this.IsBeingEdited)
                {
                    this.EndEdit();
                }

                //if (m_enabled)
                //{
                //    this.ApplyFilters();
                //}
                //else
                //{
                //    this.UnApplyFilters();
                //}

                //FilterRowSelector filterRowSelector = this.RowSelector as FilterRowSelector;

                //// Invalidate the RowSelector so that the icon is refreshed.
                //if (filterRowSelector != null)
                //{
                //    filterRowSelector.Invalidate();
                //}
            }
        }

        private bool m_enabled = true;

        private IList<ISearchExpression> m_searchExps = new List<ISearchExpression>();

        /// <summary>
        /// 
        /// </summary>
        public void ApplyFilters()
        {
            if (!this.Enabled)
            {
                return;
            }

            m_searchExps.Clear();

            int cellsCount = this.Cells.Count;
            for (int j = 0; j < cellsCount; j++)
            {
                if (!this.Cells[j].Visible)
                    continue;
                if (this.Cells[j].ReadOnly)
                    continue;

                SearchCell searchCell = (SearchCell)this.Cells[j];

                ISearchExpression filter = searchCell.GetSearchExpression();

                //object cellValue = dataRow.Cells[j].Value;
                if (filter != null)
                {
                    m_searchExps.Add(filter);
                }
            }

            //if (m_searchExps.Count > 0)
            {
                IBoundGrid grid = this.GridControl as IBoundGrid;
                if (grid != null)
                {
                    ISearchExpression exp = SearchExpression.ConvertListToOne(m_searchExps);
                    grid.DisplayManager.SearchManager.LoadData(exp, null);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Row CreateInstance()
        {
            return new SearchRow(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        /// <returns></returns>
        protected override Cell CreateCell(Column parentColumn)
        {
            SearchCell cell = new SearchCell(parentColumn);
            cell.CellEditorManager = new SearchEditor();

            bool enable = true;
            GridColumnInfo info = parentColumn.Tag as GridColumnInfo;
            if (info != null)
            {
                enable = info.GridColumnType == GridColumnType.Normal;
                if (enable)
                {
                    string searchPropertyName = SearchCell.GetSearchPropertyName(info);
                    IBoundGrid grid = this.GridControl as IBoundGrid;
                    if (grid != null && grid.DisplayManager != null)
                    {
                        var p = grid.DisplayManager.EntityInfo.GetPropertMetadata(searchPropertyName);
                        if (p == null)
                        {
                            enable = false;
                        }
                    }
                }
            }
            cell.ReadOnly = !enable;
            return cell;
        }
    }
}
