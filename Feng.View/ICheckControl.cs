using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 检查控件
    /// </summary>
    public interface ICheckControl
    {
        /// <summary>
        /// 检查控件值
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ControlCheckException">控件值不符合标准</exception>
        bool CheckControlValue();
    }
}