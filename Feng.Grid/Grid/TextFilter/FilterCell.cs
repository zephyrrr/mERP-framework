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

namespace Feng.Grid.TextFilter
{
    /// <summary>
    /// FilterCell
    /// </summary>
    public class FilterCell : Cell
    {
        internal static readonly Type FilterCellType;
        static FilterCell()
        {
            FilterCellType = typeof(FilterCell);
        }
        private object m_value;

        protected override object GetValue()
        {
            return m_value;
        }

        protected override void SetValue(object value)
        {
            m_value = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public FilterCell()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentColumn"></param>
        public FilterCell(Column parentColumn)
            : base(parentColumn)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public FilterCell(FilterCell template)
            : base(template)
        {
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Cell CreateInstance()
        {
            return new FilterCell(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FilterEditor : TextEditor
    {
        /// <summary>
        /// 
        /// </summary>
        public FilterEditor()
            : base()
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
}
