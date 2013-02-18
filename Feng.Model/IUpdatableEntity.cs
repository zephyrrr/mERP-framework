using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 可修改的实体
    /// </summary>
    public interface IUpdatableEntity
    {
        /// <summary>
        /// 修改后是否可保存
        /// </summary>
        /// <returns></returns>
        bool CanBeUpdate(OperateArgs e);
    }
}
