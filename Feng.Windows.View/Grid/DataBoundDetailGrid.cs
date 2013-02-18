using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// 数据绑定DetailGrid
    /// </summary>
    public class DataBoundDetailGrid : MyDetailGrid, IBoundGrid, IReadOnlyControl, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_boundGridHelper != null)
                {
                    m_boundGridHelper.Dispose();
                    m_boundGridHelper = null;
                }
                if (this.DisplayManager != null)
                {
                    this.DisplayManager.Dispose();
                    this.DisplayManager = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataBoundDetailGrid()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="synchronizationRoot"></param>
        protected DataBoundDetailGrid(DetailGrid template, DetailGrid synchronizationRoot)
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
            DataBoundDetailGrid grid = new DataBoundDetailGrid(this, synchronizationRoot);
            //grid.SetDisplayManager(this.DisplayManager, this.GridName);

            grid.DisplayManager = this.DisplayManager.Clone() as IDisplayManager;
            grid.GridName = this.GridName;
            grid.DisplayManager.BindingControls.Clear();
            grid.DisplayManager.BindingControls.Add(grid);

            ISearchManagerWithParent fm = grid.DisplayManager.SearchManager as ISearchManagerWithParent;
            if (fm != null)
            {
                DataBoundDetailGrid parentGrid = this.ParentGrid as DataBoundDetailGrid;
                if (parentGrid != null)
                {
                    fm.ParentDisplayManager = parentGrid.DisplayManager;
                }
            }

            // 模版能复制事件，其实为当新Instance事件触发时，也会调用模版的触发处理函数
            return grid;
        }


        private BoundGridHelper m_boundGridHelper;
        /// <summary>
        /// BoundGridHelper
        /// </summary>
        public BoundGridHelper BoundGridHelper
        {
            get
            {
                if (m_boundGridHelper == null)
                {
                    m_boundGridHelper = new BoundGridHelper(this);
                }
                return m_boundGridHelper;
            }
        }

        /// <summary>
        /// 显示控制器
        /// </summary>
        public virtual IDisplayManager DisplayManager
        {
            get;
            set;
        }

        ///// <summary>
        ///// 正在读入数据
        ///// </summary>
        //public bool IsDataLoading
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 是否在手工设置Position
        ///// </summary>
        //public bool InPositionSetting
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public virtual void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        /// <summary>
        /// 创建Grid
        /// </summary>
        public virtual void CreateGrid()
        {
            this.CreateBoundGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public override void SetDataBinding(object dataSource, string dataMember)
        {
            try
            {
                this.DisplayManager.BeginBatchOperation();

                base.SetDataBinding(dataSource, dataMember);
                this.SetGridRowCellProperties();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                this.DisplayManager.EndBatchOperation();
                this.DisplayManager.OnPositionChanged(System.EventArgs.Empty);
            }
        }
    }
}
