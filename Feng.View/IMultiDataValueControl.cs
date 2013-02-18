using System;
using System.Collections;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 多值数据控件
    /// </summary>
    public interface IMultiDataValueControl : IStateControl, IReadOnlyControl
    {
        /// <summary>
        /// 选定值
        /// </summary>
        IList SelectedDataValues { get; set; }
    }
}