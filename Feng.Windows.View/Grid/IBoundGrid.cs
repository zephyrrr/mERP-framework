using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBoundGrid : IGrid, IBindingControl
    {
        /// <summary>
        /// BoundGridHelper
        /// </summary>
        BoundGridHelper BoundGridHelper
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IDisplayManager DisplayManager
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        void CreateGrid();

        ///// <summary>
        ///// 是否正在读入数据
        ///// </summary>
        //bool IsDataLoading { get; set; }

        // /// <summary>
        ///// 是否在手工设置Position
        ///// </summary>
        //bool InPositionSetting { get; set; }
    }
}
