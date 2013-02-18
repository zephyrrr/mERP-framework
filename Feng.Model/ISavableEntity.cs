using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 可新增的实体
    /// </summary>
    public interface ISavableEntity
    {
        /// <summary>
        /// 新增后是否可保存
        /// </summary>
        /// <returns></returns>
        bool CanBeSave(OperateArgs e);
    }
}
