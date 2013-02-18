using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 包含Control的接口
    /// </summary>
    public interface IControlWrapper
    {
        /// <summary>
        /// 内部Control
        /// </summary>
        object InnerControl { get; }
    }
}
