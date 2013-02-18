using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGridDropdownControl 
    {
        /// <summary>
        /// 
        /// </summary>
        void AdjustDropDownControlSize();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        void VisibleColumns(Dictionary<string, bool> columns);
    }
}
