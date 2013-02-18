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
    public partial class ArchiveDetailFormAutoWithMultiDetailGrid : ArchiveDetailFormAutoWithDetailGridsPanel, IArchiveDetailFormAuto
    {
        protected ArchiveDetailFormAutoWithMultiDetailGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="controlGroupName"></param>
        /// <param name="detailGridCount"></param>
        public ArchiveDetailFormAutoWithMultiDetailGrid(IDisplayManager dm, string controlGroupName, int detailGridCount, string[] texts)
            : base(dm, controlGroupName)
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            CreateControls(detailGridCount, texts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="controlGroupName"></param>
        /// <param name="detailGridCount"></param>
        public ArchiveDetailFormAutoWithMultiDetailGrid(IWindowControlManager cm, string controlGroupName, int detailGridCount, string[] texts)
            : base(cm, controlGroupName)
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            CreateControls(detailGridCount, texts);
        }

        private void CreateControls(int detailGridCount, string[] texts)
        {
            IBoundGrid[] grdDetails = CreateDetailGrids(detailGridCount);
            MyTabControl tabControl = new MyTabControl();
            tabControl.Dock = DockStyle.Fill;
            this.splitContainer1.Panel2.Controls.Add(tabControl);

            if (grdDetails != null)
            {
                for (int i = 0; i < grdDetails.Length; ++i)
                {
                    TabPage tabPage = new TabPage();
                    tabControl.TabPages.Add(tabPage);
                    tabPage.Text = texts[i];

                    MyGrid gridControl = grdDetails[i] as MyGrid;
                    tabPage.Controls.Add(gridControl);
                    gridControl.Dock = DockStyle.Fill;
                    gridControl.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

                    this.AddDetailGrid(grdDetails[i]);
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IBoundGrid[] CreateDetailGrids(int detailGridCount)
        {
            IBoundGrid[] ret = new IBoundGrid[detailGridCount];
            for (int i = 0; i < ret.Length; ++i)
            {
                if (base.ControlManager != null)
                {
                    ret[i] = new ArchiveUnboundWithDetailGridLoadOnDemand();
                }
                else
                {
                    ret[i] = new DataUnboundWithDetailGridLoadOnDemand();
                }
            }
            return ret;
        }
    }
}
