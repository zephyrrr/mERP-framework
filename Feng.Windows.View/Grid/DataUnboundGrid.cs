using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Feng.Grid
{
    /// <summary>
    /// 非绑定Grid
    /// </summary>
    public partial class DataUnboundGrid :  DataBoundGrid, IBoundGrid, ICanAddItemBindingControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataUnboundGrid()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.DisplayManager != null)
                {
                    this.DisplayManager.PositionChanged -= new EventHandler(DisplayManager_PositionChanged);
                }
                this.CurrentRowChanged -= new EventHandler(DataUnboundGrid_CurrentRowChanged);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// 创建Grid
        /// </summary>
        public override void CreateGrid()
        {
            this.CreateUnBoundGrid();
            this.BoundGridHelper.Initialize();

            this.DisplayManager.PositionChanged += new EventHandler(DisplayManager_PositionChanged);
            this.CurrentRowChanged += new EventHandler(DataUnboundGrid_CurrentRowChanged);

            //this.DisplayManager.SearchManager.UseStreamLoad = true;
        }

        private void DataUnboundGrid_CurrentRowChanged(object sender, EventArgs e)
        {
            OnCurrentRowChanged(this);
        }

        internal static void OnCurrentRowChanged(IBoundGrid grid)
        {
            if (!(grid.CurrentRow is Xceed.Grid.DataRow))
                return;

            if (grid.DisplayManager.InBatchOperation)
            {
                return;
            }

            DataUnboundDetailGrid detailGrid = null;
            bool setNewCurrentRow = false;
            try
            {
                grid.DisplayManager.BeginBatchOperation();

                if (grid.CurrentRow != null)
                {
                    detailGrid = grid.CurrentRow.ParentGrid as DataUnboundDetailGrid;
                }
                if (detailGrid == null)
                {
                    setNewCurrentRow = DataUnboundGrid.TryChangeCurrentRow(grid.DisplayManager, grid);
                }
                else
                {
                    setNewCurrentRow = DataUnboundGrid.TryChangeCurrentRow(detailGrid.DisplayManager, detailGrid);
                }
            }
            finally
            {
                grid.DisplayManager.EndBatchOperation();

                if (detailGrid == null)
                {
                    // 已经触发过了
                    //// CurrentRow改变了
                    //if (setNewCurrentRow)
                    //{
                    //    grid.DisplayManager.OnPositionChanged(System.EventArgs.Empty);
                    //}
                }
            }
        }

        internal static bool TryChangeCurrentRow(IDisplayManager dm, IGrid grid)
        {
            int newPos;
            Xceed.Grid.DataRow row = grid.CurrentRow as Xceed.Grid.DataRow;
            if (row == null)
            {
                newPos = -1;
            }
            else
            {
                newPos = row.Index;
            }
            int oldPos = dm.Position;
            if (oldPos == newPos)
            {
                return false;
            }
            dm.Position = newPos;

            // Change failed because of something
            if (dm.Position == oldPos)
            {
                // if in add, there is no addrow
                if (oldPos < grid.DataRows.Count)
                {
                    if (oldPos >= 0 && oldPos < grid.DataRows.Count)
                    {
                        grid.CurrentRow = grid.DataRows[oldPos];
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        protected virtual void DisplayManager_PositionChanged(object sender, EventArgs e)
        {
            if (this.DisplayManager.InBatchOperation)
            {
                return;
            }

            try
            {
                this.DisplayManager.BeginBatchOperation();

                if (this.DisplayManager.Position == -1 || this.DisplayManager.Position >= this.DataRows.Count)
                {
                    // do nothing for not change currentRow
                    //this.CurrentRow = null;
                }
                else
                {
                    // grid处于编辑状态时，Position改变，而且先不EndEdit，则要Cancel否则可能ValidationFail
                    MyGrid.CancelEditCurrentDataRow(this);

                    MyGrid.SetCurrentRow(this, this.DataRows[this.DisplayManager.Position]);
                }
            }
            finally
            {
                this.DisplayManager.EndBatchOperation();
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataItem"></param>
        public void AddDateItem(object dataItem)
        {
            try
            {
                this.DisplayManager.BeginBatchOperation();

                this.AddDateItemToUnBoundGrid(dataItem);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                this.DisplayManager.EndBatchOperation();
            }
        }
    }
}