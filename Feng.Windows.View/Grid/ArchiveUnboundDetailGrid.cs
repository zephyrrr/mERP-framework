using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xceed.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// ArchiveUnboundDetailGrid
    /// </summary>
    public class ArchiveUnboundDetailGrid : DataUnboundDetailGrid, IArchiveGrid
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_archiveGridHelper != null)
                {
                    m_archiveGridHelper.RowSaving -= new System.ComponentModel.CancelEventHandler(row_Saving);
                    m_archiveGridHelper.Dispose();
                    m_archiveGridHelper = null;
                }
                if (this.ControlManager != null)
                {
                    this.ControlManager.ListChanged -= new ListChangedEventHandler(m_cm_ListChanged);

                    this.ControlManager.Dispose();
                    this.ControlManager = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveUnboundDetailGrid()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="synchronizationRoot"></param>
        protected ArchiveUnboundDetailGrid(DetailGrid template, DetailGrid synchronizationRoot)
            : base(template, synchronizationRoot)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synchronizationRoot"></param>
        /// <returns></returns>
        protected override DetailGrid CreateInstance(DetailGrid synchronizationRoot)
        {
            ArchiveUnboundDetailGrid grid = new ArchiveUnboundDetailGrid(this, synchronizationRoot);

            grid.ControlManager = this.ControlManager.Clone() as IControlManager;

            grid.SetGridPermissions();

            //grid.DisplayManager = this.DisplayManager.Clone() as IDisplayManager;
            grid.GridName = this.GridName;
            grid.ControlManager.DisplayManager.BindingControls.Clear();
            grid.ControlManager.DisplayManager.BindingControls.Add(grid);

            // 如果用new CancelEventHandler(row_Saving)则相当于this.row_Saving，在事件处理程序中用this指的是Template
            grid.ArchiveGridHelper.RowSaving += new System.ComponentModel.CancelEventHandler(grid.row_Saving);
            grid.ControlManager.ListChanged += new ListChangedEventHandler(grid.m_cm_ListChanged);

            return grid;
        }

        /// <summary>
        /// 显示控制器
        /// </summary>
        public override IDisplayManager DisplayManager
        {
            get { return this.ControlManager.DisplayManager; }
            set {  }
        }

        private ArchiveGridHelper m_archiveGridHelper;
        /// <summary>
        /// ArchiveGridHelper
        /// </summary>
        public ArchiveGridHelper ArchiveGridHelper
        {
            get
            {
                if (m_archiveGridHelper == null)
                {
                    m_archiveGridHelper = new ArchiveGridHelper(this);
                }
                return m_archiveGridHelper;
            }
            //internal set { m_archiveGridHelper = value; }
        }

        /// <summary>
        /// 操作管理器
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IControlManager ControlManager
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Xceed.Grid.InsertionRow InsertionRow
        {
            get { return this.ArchiveGridHelper.InsertionRow; }
        }

        /// <summary>
        /// 是否允许内部操作
        /// </summary>
        [DefaultValue(false)]
        public bool AllowInnerInsert { get; set; }

        /// <summary>
        /// 是否允许内部操作
        /// </summary>
        [DefaultValue(false)]
        public bool AllowInnerEdit { get; set; }

        /// <summary>
        /// 是否允许内部操作
        /// </summary>
        [DefaultValue(false)]
        public bool AllowInnerDelete { get; set; }

        /// <summary>
        /// InsertRow
        /// </summary>
        public bool AddThrowInsertRow
        {
            get;
            set;
        }

        /// <summary>
        /// 用户自定义Validate
        /// </summary>
        public event System.ComponentModel.CancelEventHandler ValidateRow
        {
            add { this.ArchiveGridHelper.ValidateRow += value; }
            remove { this.ArchiveGridHelper.ValidateRow -= value; }
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public override void SetState(StateType state)
        {
            // do it in parent Grid
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CreateGrid()
        {
            base.CreateGrid();

            this.ArchiveGridHelper.RowSaving += new System.ComponentModel.CancelEventHandler(row_Saving);

            //m_archiveGridHelper.Initialize();

            this.SetGridPermissions();

            this.ControlManager.ListChanged += new ListChangedEventHandler(m_cm_ListChanged);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        //public override void SetDataBinding(object dataSource, string dataMember)
        //{
        //    base.SetDataBinding(dataSource, dataMember);

        //    ArchiveUnboundGrid.SetGridRowCellProperties(this);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void row_Saving(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Xceed.Grid.CellRow row = (Xceed.Grid.CellRow)sender;

            ArchiveUnboundGrid.SaveRowValues(row, row.ParentGrid as ArchiveUnboundDetailGrid);

            e.Cancel = (this.ControlManager.State != StateType.View);

            if (e.Cancel)
            {
                this.ControlManager.CancelEdit();
            }
        }

        private void m_cm_ListChanged(object sender, ListChangedEventArgs e)
        {
            ArchiveUnboundGrid.OnListChanged(e, this);
        }

        /// <summary>
        /// 是否是主从模式里的从Grid（如果是，要设置状态）
        /// </summary>
        public bool IsInDetailMode
        {
            get;
            set;
        }

        /// <summary>
        /// CheckControlValue
        /// </summary>
        /// <returns></returns>
        public bool CheckControlValue()
        {
            return ArchiveGridExtention.CheckControlValue(this);
        }
    }
}
