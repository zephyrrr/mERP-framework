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
    public partial class ArchiveDetailFormWithDetailGrids : ArchiveDetailForm, IArchiveDetailFormWithDetailGrids
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                this.FormClosed -= new FormClosedEventHandler(ArchiveDetailFormAutoWithDetailGrid_FormClosed);

                if (this.ControlManager != null)
                {
                    this.ControlManager.EditCanceled -= new EventHandler(cm_EditCanceled);
                }

                foreach (IBoundGrid grid in m_detailGrids)
                {
                    grid.Dispose();
                }
                m_detailGrids.Clear();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        protected ArchiveDetailFormWithDetailGrids()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormWithDetailGrids(IDisplayManager dm, string controlGroupName)
            : base(dm, controlGroupName)
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="controlGroupName"></param>
        public ArchiveDetailFormWithDetailGrids(IWindowControlManager cm, string controlGroupName)
            : base(cm, controlGroupName)
        {
            InitializeComponent();

            this.FormClosed += new FormClosedEventHandler(ArchiveDetailFormAutoWithDetailGrid_FormClosed);

            cm.EditCanceled += new EventHandler(cm_EditCanceled);
            //cm.ListChanged += new ListChangedEventHandler(cm_ListChanged);
        }

        void cm_EditCanceled(object sender, EventArgs e)
        {
            foreach (IBoundGrid grid in m_detailGrids)
            {
                MyGrid.CancelEditCurrentDataRow(grid);
            }

            foreach (IBoundGrid grid in m_detailGrids)
            {
                grid.ReloadData(false);
            }
        }


        protected IList<IBoundGrid> m_detailGrids = new List<IBoundGrid>();

        /// <summary>
        /// DetailGrids
        /// </summary>
        public IList<IBoundGrid> DetailGrids
        {
            get { return m_detailGrids; }
        }

        /// <summary>
        /// 增加详细信息Grid
        /// </summary>
        /// <param name="detailGrid"></param>
        protected void AddDetailGrid(IBoundGrid detailGrid)
        {
            m_detailGrids.Add(detailGrid);

            IArchiveGrid archiveGrid = detailGrid as IArchiveGrid;
            if (archiveGrid != null)
            {
                archiveGrid.IsInDetailMode = true;

                if (base.ControlManager != null)
                {
                    base.ControlManager.StateControls.Add(archiveGrid);
                    base.ControlManager.CheckControls.Add(archiveGrid);
                }
            }
        }


        void ArchiveDetailFormAutoWithDetailGrid_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (IBoundGrid grid in m_detailGrids)
            {
                IArchiveGrid archiveGrid = grid as IArchiveGrid;
                if (archiveGrid != null)
                {
                    base.ControlManager.StateControls.Remove(archiveGrid);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void DisplayManager_PositionChanged(object sender, EventArgs e)
        {
            if (IsClosed)
                return;

            //if (this.DisplayManager.InBatchOperation)
            //{
            //    return;
            //}

            base.DisplayManager_PositionChanged(sender, e);

            if (this.DisplayManager.Position == -1)
            {
                foreach (IBoundGrid grid in m_detailGrids)
                {
                    MyGrid.CancelEditCurrentDataRow(grid);

                    grid.DataRows.Clear();
                }
            }
            else
            {
                ReloadDetailGridData();
            }
        }

        private void ReloadDetailGridData()
        {
            foreach (IBoundGrid grid in m_detailGrids)
            {
                // grid.DisplayManager.SearchManager.LoadData();  // 不能停留在上一位置
                grid.ReloadData(false);
            }
        }

        // ArchiveUnboundGrid.OnListChanged 里会调用 PositionChanged 事件
        //void cm_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    switch (e.ListChangedType)
        //    {
        //        case ListChangedType.ItemAdded:
        //            // Position会改变
        //            break;
        //        case ListChangedType.ItemChanged:
        //            ReloadDetailGridData();
        //            break;
        //        case ListChangedType.ItemDeleted:
        //            // Position会改变
        //            break;
        //        default:
        //            throw new NotSupportedException("not supported listChangedType");
        //    }
        //}
    }
}
