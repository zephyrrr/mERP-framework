using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Xceed.Grid;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public class DataUnboundWithDetailGridLoadOnDemand : DataUnboundGrid, IBoundGridWithDetailGridLoadOnDemand
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