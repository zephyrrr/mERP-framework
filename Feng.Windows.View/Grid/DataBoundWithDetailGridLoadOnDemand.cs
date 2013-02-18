using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class DataBoundWithDetailGridLoadOnDemand : DataBoundGrid, IBoundGridWithDetailGridLoadOnDemand
    {
        /// <summary>
        /// 
        /// </summary>
        public override void CreateGrid()
        {
            base.CreateGrid();

            this.SynchronizeDetailGrids = false;
        }
    }
}
