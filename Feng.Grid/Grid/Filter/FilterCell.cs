/*
 * Xceed Grid for .NET - Extensibility Sample Application
 * Copyright (c) 2002-2007 Xceed Software Inc.
 * 
 * [FilterCell.cs]
 *  
 * This file demonstrates how to create classes that derive from 
 * the ValueCell class in order to filter the GridControl's data.
 * 
 * This file is part of Xceed Grid for .NET. The source code in this file 
 * is only intended as a supplement to the documentation, and is provided 
 * "as is", without warranty of any kind, either expressed or implied.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Xceed.Grid;
using Xceed.Grid.Collections;
using Xceed.Grid.Editors;
using Xceed.Grid.Viewers;
using Xceed.Editors;
using Feng.Grid.Viewers;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterEditor : ComboBoxEditor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <param name="valueMember"></param>
        /// <param name="displayFormat"></param>
        public FilterEditor(object dataSource, string dataMember, string valueMember, string displayFormat)
            : base(dataSource, dataMember, valueMember, displayFormat)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override CreateControlMode CreateControlMode
        {
            get
            {
                // The template control will be the one used.
                // It won't be cloned each time the cell enters edition.
                return CreateControlMode.SingleInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override System.Windows.Forms.Control CreateControl()
        {
            return this.TemplateControl;
        }
    }

    /// <summary>
    /// The FilterCell class is used to populate a FilterRow.  
    /// It contains a list of possible filters that is dynamic to its sibling cells values.
    /// </summary>
    public class FilterCell : ValueCell
    {
        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        public FilterCell()
            : base()
        {
            this.InitializeInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        public FilterCell(Column parentColumn)
            : base(parentColumn)
        {
            this.InitializeInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public FilterCell(FilterCell template)
            : base(template)
        {
        }

        private Dictionary<string, IFilter> m_filterItems;

        private void InitializeInstance()
        {
            m_filterItems = new Dictionary<string, IFilter>();
            this.DoubleClick += new EventHandler(FilterCell_DoubleClick);
        }

        void FilterCell_DoubleClick(object sender, EventArgs e)
        {
            TemplateControl_SelectedValueChanged(this.CellEditorManager.TemplateControl, System.EventArgs.Empty);
        }

        #endregion CONSTRUCTORS

        #region PUBLIC METHODS

        /// <summary>
        /// 
        /// </summary>
        public void ClearFilterList()
        {
            m_filterItems.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void FillFilterList()
        {
            if (!this.Visible)
            {
                return;
            }

            FilterRow filterRow = this.ParentRow as FilterRow;

            // Only build the list from the values under the FilterRow's hierarchical position.
            GroupBase groupBase = filterRow.ParentGroup;
            ReadOnlyDataRowList dataRows = groupBase.GetSortedDataRows(true);

            int dataRowsCount = dataRows.Count;
            DetailGrid parentGrid = groupBase as DetailGrid;

            if (parentGrid == null)
            {
                parentGrid = groupBase.ParentGrid;
            }

            //int cellsCount = parentGrid.Columns.Count;

            //object savedValue = this.Value;

            IFilter customFilter = m_filterItems.ContainsKey(s_customText) ? m_filterItems[s_customText] : null;

            // Clear the existing lists.
            this.ClearFilterList();
            for (int i = 0; i < dataRowsCount; i++)
            {
                Xceed.Grid.DataRow dataRow = dataRows[i];
                this.AddToFilterList(dataRow.Cells[this.FieldName]);
            }

            // Add Any and custom
            m_filterItems[s_anyText] = new AnyFilter();
            m_filterItems[s_customText] = customFilter;

            // Fill editor and viewer
            this.FillViewerAndEditor();

            //this.Value = savedValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IFilter GetFilter()
        {
            object value;
            if (this.IsBeingEdited)
            {
                value = ((FilterEditor) this.CellEditorManager).TemplateControl.SelectedValue;
            }
            else
            {
                value = this.Value;
            }

            if (m_filterItems.ContainsKey(value.ToString()))
            {
                return m_filterItems[value.ToString()];
            }
            else
            {
                // 因为Filter有保存，但是里面的内容有变化，所以可能产生无内容的情况
                // this.Value = s_anyText; // 但是还是保存Value
                return m_filterItems[s_anyText];
            }
        }

        #endregion PUBLIC METHODS

        #region PROTECTED PROPERTIES

        /// <summary>
        /// 
        /// </summary>
        public override bool IsCellEditorManagerAmbient
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsCellViewerManagerAmbient
        {
            get { return false; }
        }

        #endregion PROTECTED PROPERTIES

        #region PROTECTED METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Cell CreateInstance()
        {
            return new FilterCell(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEditLeft(EditLeftEventArgs e)
        {
            //FilterRow filterRow = this.ParentRow as FilterRow;
            //if (filterRow != null)
            //    filterRow.ApplyFilters();


            //if (((FilterEditor)this.CellEditorManager).TemplateControl.SelectedIndex == -1
            //    && m_oldSelectIndex != -1)
            //{
            //    ((FilterEditor)this.CellEditorManager).TemplateControl.SelectedIndex = m_oldSelectIndex;
            //}

            base.OnEditLeft(e);
        }

        //private int m_oldSelectIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEditEntered(EventArgs e)
        {
            //m_oldSelectIndex = ((FilterEditor)this.CellEditorManager).TemplateControl.SelectedIndex;
            //((FilterEditor)this.CellEditorManager).TemplateControl.SelectedIndex = -1;

            base.OnEditEntered(e);
        }

        #endregion PROTECTED METHODS

        #region INTERNAL METHODS

        private static string s_nullText = "(空)";
        private static string s_anyText = "(任意)";
        private static string s_customText = "(自定义)";

        internal void ClearFilter()
        {
            this.Value = s_anyText;
        }

        /// <summary>
        /// Add the cell's value to the filter list.
        /// </summary>
        /// <param name="cell"></param>
        internal void AddToFilterList(Cell cell)
        {
            // The SingleOccurancyList will take care of making sure that the value is only present once in the list.
            if ((cell.Value == null)
                || (cell.Value == DBNull.Value))
            {
                m_filterItems[s_nullText] = new ComparisonFilter(ComparisonType.Eq, null);
            }
            else
            {
                if (cell.CellViewerManager is INameValueControl)
                {
                    m_filterItems[cell.GetDisplayText()] = new ComparisonFilter(ComparisonType.Eq, cell.GetDisplayText());
                }
                else if (cell.Value is IComparable)
                {
                    m_filterItems[cell.GetDisplayText()] = new ComparisonFilter(ComparisonType.Eq, cell.Value);
                }
                else
                {
                    m_filterItems[cell.GetDisplayText()] = new ComparisonFilter(ComparisonType.Eq, "None");
                }
            }
        }

        internal void FillViewerAndEditor()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DisplayText");
            foreach (KeyValuePair<string, IFilter> kvp in m_filterItems)
            {
                dt.Rows.Add(new object[] {kvp.Key});
            }

            this.CellEditorManager = new FilterEditor(dt, "", "DisplayText", "%DisplayText%");
            //m_filterEditorManager.TemplateControl.SelectedItem = null;
            ((FilterEditor) this.CellEditorManager).TemplateControl.DropDownSize = new System.Drawing.Size(150, 150);

            this.CellViewerManager = new Xceed.Grid.Viewers.ComboBoxViewer(dt, "", "DisplayText", "%DisplayText%");

            ((FilterEditor) this.CellEditorManager).TemplateControl.SelectedIndexChanged +=  new EventHandler(TemplateControl_SelectedValueChanged);


            // 如果原有数据，不清空
            if (this.Value == null)
            {
                this.Value = s_anyText;
            }
        }

        #endregion INTERNAL METHODS

        #region PRIVATE FIELDS

        #endregion PRIVATE FIELDS

        #region EVENT HANDLERS

        private FormCustomFilter m_formCustomFilter;

        private void TemplateControl_SelectedValueChanged(object sender, EventArgs e)
        {
            FilterRow filterRow = this.ParentRow as FilterRow;
            if (filterRow == null)
            {
                return;
            }

            object value = ((FilterEditor) this.CellEditorManager).TemplateControl.SelectedValue;
            if (value != null && value.ToString() == s_customText)
            {
                if (m_formCustomFilter == null)
                {
                    m_formCustomFilter = new FormCustomFilter();
                }
                if (m_formCustomFilter.ShowDialog(filterRow.GridControl, this.FieldName) ==
                    System.Windows.Forms.DialogResult.OK)
                {
                    if (m_formCustomFilter.Filter != null)
                    {
                        m_filterItems[s_customText] = m_formCustomFilter.Filter;
                        filterRow.ApplyFilters();
                        return;
                    }
                }

                this.Value = s_anyText;
                ((FilterEditor) this.CellEditorManager).TemplateControl.SelectedValue = s_anyText;
            }
            else
            {
                filterRow.ApplyFilters();
            }
        }

        #endregion EVENT HANDLERS
    }
}