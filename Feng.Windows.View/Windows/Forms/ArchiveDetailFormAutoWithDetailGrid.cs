using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ArchiveDetailFormAutoWithDetailGrid : ArchiveDetailFormAutoWithDetailGridsPanel, IArchiveDetailFormAuto
    {
        /// <summary>
        /// 
        /// </summary>
        protected ArchiveDetailFormAutoWithDetailGrid()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormAutoWithDetailGrid(IDisplayManager dm, string controlGroupName)
            : base(dm, controlGroupName)
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            CreateControls();
        }

        private void CreateControls()
        {
            IBoundGrid grdDetail = CreateDetailGrid();
            if (grdDetail != null)
            {
                MyGrid gridControl = grdDetail as MyGrid;

                gridControl.Dock = DockStyle.Fill;
                gridControl.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());
                this.splitContainer1.Panel2.Controls.Add(gridControl);
            }

            this.AddDetailGrid(grdDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormAutoWithDetailGrid(IWindowControlManager cm, string controlGroupName)
            : base(cm, controlGroupName)
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            CreateControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IBoundGrid CreateDetailGrid()
        {
            if (base.ControlManager != null)
            {
                return new ArchiveUnboundWithDetailGridLoadOnDemand();
            }
            else
            {
                return new DataUnboundWithDetailGridLoadOnDemand();
            }
        }
    }
}
