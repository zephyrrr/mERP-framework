/*
 * Xceed Grid for .NET - Extensibility Sample Application
 * Copyright (c) 2002-2007 Xceed Software Inc.
 * 
 * [FilterRow.cs]
 *  
 * This class demonstrates how to create classes that derive from the 
 * ValueRow class in order to do filter the GridControl's data.
 * 
 * This file is part of Xceed Grid for .NET. The source code in this file 
 * is only intended as a supplement to the documentation, and is provided 
 * "as is", without warranty of any kind, either expressed or implied.
 */

using System;
using System.Drawing;
using Xceed.Grid;
using Xceed.Grid.Collections;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// The FilterRow class derives from the Xceed.Grid.ValueRow and provides
    /// filtering functionalities to the grid it is added to.
    /// </summary>
    public class FilterRow : ValueRow
    {
        #region CONSTRUCTORS

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

        #endregion CONSTRUCTORS

        #region PUBLIC PROPERTIES

        /// <summary>
        /// Enables or disables the FilterRow.
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

                if (m_enabled)
                {
                    this.ApplyFilters();
                }
                else
                {
                    this.UnApplyFilters();
                }

                FilterRowSelector filterRowSelector = this.RowSelector as FilterRowSelector;

                // Invalidate the RowSelector so that the icon is refreshed.
                if (filterRowSelector != null)
                {
                    filterRowSelector.Invalidate();
                }
            }
        }

        private bool m_enabled = true;

        #endregion PUBLIC PROPERTIES

        #region PUBLIC METHODS

        /// <summary>
        /// ≥ı ºªØFilter
        /// </summary>
        public void FillFilters()
        {
            try
            {
                this.InitFilterLists();
                this.InitFilterCellsEditorsAndViewers();
            }
            catch (Exception ex)
            {
                throw new GridException("An error occured while initializing the FilterRow.", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearFilters()
        {
            foreach (FilterCell cell in this.Cells)
            {
                cell.ClearFilter();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ApplyFilters()
        {
            if (!this.Enabled)
            {
                return;
            }

            GroupBase parentGroup = this.ParentGroup;

            if (parentGroup == null)
            {
                return;
            }

            ReadOnlyDataRowList siblingDataRows = parentGroup.GetSortedDataRows(true);
            int siblingDataRowsCount = siblingDataRows.Count;

            int cellsCount = this.Cells.Count;

            for (int i = 0; i < siblingDataRowsCount; i++)
            {
                DataRow dataRow = siblingDataRows[i];

                bool rowMatchesFilter = true;

                for (int j = 0; j < cellsCount; j++)
                {
                    if (!this.Cells[j].Visible)
                    {
                        continue;
                    }

                    FilterCell filterCell = (FilterCell) this.Cells[j];

                    IFilter filter = filterCell.GetFilter();

                    //object cellValue = dataRow.Cells[j].Value;
                    if (filter != null)
                    {
                        rowMatchesFilter = filter.Evaluate(dataRow.Cells[j].CellViewerManager is INameValueControl ? dataRow.Cells[j].GetDisplayText() :
                            dataRow.Cells[j].Value);
                    }

                    if (!rowMatchesFilter)
                    {
                        break;
                    }
                }

                dataRow.Visible = rowMatchesFilter;

                // If the row belongs to a group, set the group visibility accordingly to its rows visibility.
                // The group will be hidden if all of its dataRows are filtered out.
                // If at least one dataRow is visible, then the group should also be visible.
                Group dataRowParentGroup = dataRow.ParentGroup as Group;

                if (dataRowParentGroup != null)
                {
                    if (rowMatchesFilter)
                    {
                        MakeParentGroupsVisible(dataRowParentGroup);
                    }
                    else
                    {
                        MakeParentGroupsInvisibleIfNoRowsVisible(dataRowParentGroup);
                    }
                }
            }

            RaiseFilteredEvent();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnApplyFilters()
        {
            // Set all groups/dataRows to be visible.
            GroupBase parentGroup = this.ParentGroup;

            if (parentGroup == null)
            {
                return;
            }

            ReadOnlyDataRowList siblingDataRows = parentGroup.GetSortedDataRows(true);
            int siblingDataRowsCount = siblingDataRows.Count;

            for (int i = 0; i < siblingDataRowsCount; i++)
            {
                siblingDataRows[i].Visible = true;
            }

            this.ShowAllGroups(this.GridControl.Groups);

            RaiseFilteredEvent();
        }

        private void RaiseFilteredEvent()
        {
            //MyGrid grid = this.GridControl as MyGrid;
            //if (grid != null)
            //{
            //    grid.OnFilterApplied();
            //}
        }

        #endregion PUBLIC METHODS

        #region PROTECTED METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Row CreateInstance()
        {
            return new FilterRow(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        /// <returns></returns>
        protected override Cell CreateCell(Column parentColumn)
        {
            FilterCell cell = new FilterCell(parentColumn);
            cell.ReadOnly = false;
            return cell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEditCanceled(EventArgs e)
        {
            this.ApplyFilters();

            base.OnEditCanceled(e);
        }

        #endregion PROTECTED METHODS

        #region PRIVATE METHODS

        private void InitFilterLists()
        {
            try
            {
                // Only build the list from the values under the FilterRow's hierarchical position.
                GroupBase groupBase = this.ParentGroup;
                ReadOnlyDataRowList dataRows = groupBase.GetSortedDataRows(true);

                //int dataRowsCount = dataRows.Count;
                DetailGrid parentGrid = groupBase as DetailGrid;

                if (parentGrid == null)
                {
                    parentGrid = groupBase.ParentGrid;
                }

                int cellsCount = parentGrid.Columns.Count;

                // Clear the existing lists.
                for (int i = 0; i < cellsCount; i++)
                {
                    FilterCell filterCell = (FilterCell) this.Cells[i];
                    if (filterCell.Visible)
                    {
                        filterCell.FillFilterList();
                    }
                }

                //for (int i = 0; i < dataRowsCount; i++)
                //{
                //    DataRow dataRow = dataRows[i];

                //    for (int j = 0; j < cellsCount; j++)
                //    {
                //        FilterCell filterCell = (FilterCell)this.Cells[j];

                //        // If the cell is Visible, add its value to the list.
                //        if (filterCell.Visible)
                //            filterCell.AddToFilterList(dataRow.Cells[j]);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new GridException("An error occured while filling the FilterRow's filter list.", ex);
            }
        }

        private void InitFilterCellsEditorsAndViewers()
        {
            try
            {
                foreach (FilterCell filterCell in this.Cells)
                {
                    // Setup the filterCells CellEditorManager and CellViewerManager.
                    if (filterCell.Visible)
                    {
                        filterCell.FillViewerAndEditor();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GridException("An error occured while initializing the FilterRow's cells.", ex);
            }
        }


        private void ShowAllGroups(ReadOnlyGroupList groups)
        {
            int groupsCount = groups.Count;

            for (int i = 0; i < groupsCount; i++)
            {
                Group group = groups[i];

                group.Visible = true;

                this.ShowAllGroups(group.Groups);
            }
        }

        private void MakeParentGroupsVisible(Group group)
        {
            group.Visible = true;

            Group parentGroup = group.ParentGroup as Group;

            if (parentGroup != null)
            {
                this.MakeParentGroupsVisible(parentGroup);
            }
        }

        private void MakeParentGroupsInvisibleIfNoRowsVisible(Group group)
        {
            bool atLestOneRowVisible = false;

            ReadOnlyDataRowList groupDataRows = group.GetSortedDataRows(true);
            int groupDataRowsCount = groupDataRows.Count;

            for (int i = 0; i < groupDataRowsCount; i++)
            {
                DataRow groupDataRow = groupDataRows[i];

                if (groupDataRow.Visible == true)
                {
                    atLestOneRowVisible = true;
                    break;
                }
            }

            if (!atLestOneRowVisible)
            {
                group.Visible = false;

                Group parentGroup = group.ParentGroup as Group;

                if (parentGroup != null)
                {
                    this.MakeParentGroupsInvisibleIfNoRowsVisible(parentGroup);
                }
            }
        }

        #endregion PRIVATE METHODS
    }
}