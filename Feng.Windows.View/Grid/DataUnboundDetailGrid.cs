using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// 非绑定Grid
    /// </summary>
    public class DataUnboundDetailGrid : DataBoundDetailGrid, IBoundGridWithDetailGridLoadOnDemand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataUnboundDetailGrid()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="synchronizationRoot"></param>
        protected DataUnboundDetailGrid(DetailGrid template, DetailGrid synchronizationRoot)
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
            DataUnboundDetailGrid grid = new DataUnboundDetailGrid(this, synchronizationRoot);
            //grid.SetDisplayManager(this.DisplayManager, this.GridName);

            grid.DisplayManager = this.DisplayManager.Clone() as IDisplayManager;
            grid.GridName = this.GridName;
            grid.DisplayManager.BindingControls.Clear();
            grid.DisplayManager.BindingControls.Add(grid);

            // 如果parent还是DetailGrid，此时因为是从Template复制，ParentDisplayManager也为Tempalte的Parent，
            // 应该是从具体的Parent DetailGrid里来
            ISearchManagerWithParent smp = grid.DisplayManager.SearchManager as ISearchManagerWithParent;
            if (smp != null)
            {
                DataBoundDetailGrid parentGrid = this.ParentGrid as DataBoundDetailGrid;
                if (parentGrid != null)
                {
                    smp.ParentDisplayManager = parentGrid.DisplayManager;
                }
            }

            return grid;
        }

        /// <summary>
        /// 创建Grid
        /// </summary>
        public override void CreateGrid()
        {
            this.CreateUnBoundGrid();

            // DetailGrid，当改变Position时，不改变CurrentRow
            //this.DisplayManager.PositionChanged += new EventHandler(dm_PositionChanged);
        }

        //private void dm_PositionChanged(object sender, EventArgs e)
        //{
        //    if (this.DisplayManager.InBatchOperation)
        //    {
        //        return;
        //    }

        //    this.DisplayManager.BeginBatchOperation();

        //    // grid处于编辑状态时，Position改变，而且先不EndEdit，则要Cancel否则可能ValidationFail
        //    (this.GridControl as MyGrid).CancelEditCurrentDataRow();

        //    if (this.DisplayManager.Position == -1 || this.DisplayManager.Position >= this.DataRows.Count)
        //    {
        //        // do nothing for not change currentRow
        //        //this.CurrentRow = null;
        //    }
        //    else
        //    {
        //        this.CurrentRow = this.DataRows[this.DisplayManager.Position];
        //    }

        //    this.DisplayManager.EndBatchOperation();
        //}

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public override void SetDataBinding(object dataSource, string dataMember)
        {
            try
            {
                this.DisplayManager.BeginBatchOperation();

                this.SetUnBoundGridDataBinding();
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
