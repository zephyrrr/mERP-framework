using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Xceed.Grid;
using Feng.Utils;

namespace Feng.Grid.Columns
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckColumn : Column
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultSelectColumnName = "Ñ¡¶¨";

        private CheckColumnHelper m_checkColumnHelper;

        internal CheckColumnHelper CheckColumnHelper
        {
            get { return m_checkColumnHelper; }
        }

        internal CheckColumn(IGrid gridControl)
            : this(gridControl, DefaultSelectColumnName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        internal CheckColumn(IGrid gridControl, string columnName)
            : base(columnName, typeof(bool))
        {
            //InitializeComponent();

            m_checkColumnHelper = new CheckColumnHelper(this, gridControl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        protected CheckColumn(CheckColumn template)
            : base(template)
        {
            //m_grid = template.GridControl;
        }

        /// <summary>
        /// CreateInstance
        /// </summary>
        /// <returns></returns>
        protected override Column CreateInstance()
        {
            return new CheckColumn(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetSumRow()
        {
            m_checkColumnHelper.ResetSumRow();
        }
    }
}