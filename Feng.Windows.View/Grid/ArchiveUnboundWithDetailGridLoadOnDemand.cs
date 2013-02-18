using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    /// <summary>
    /// ArchiveUnboundWithDetailGridLoadOnDemand
    /// </summary>
    public class ArchiveUnboundWithDetailGridLoadOnDemand : ArchiveUnboundGrid, IBoundGridWithDetailGridLoadOnDemand
    {
        /// <summary>
        /// 
        /// </summary>
        public override void CreateGrid()
        {
            base.CreateGrid();

            this.SynchronizeDetailGrids = true;
        }
    }
}
