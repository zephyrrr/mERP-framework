using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 可只读控件
    /// </summary>
    public interface IReadOnlyControl
    {
        /// <summary>
        /// 是否只读
        /// </summary>
        bool ReadOnly { get; set; }

        /// <summary>
        /// ReadOnlyChanged
        /// </summary>
        event EventHandler ReadOnlyChanged;
    }
}