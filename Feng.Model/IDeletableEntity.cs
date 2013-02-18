using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 判断是否能被删除的实体类接口
    /// </summary>
    public interface IDeletableEntity
    {
        /// <summary>
        /// 是否可删除
        /// </summary>
        /// <returns></returns>
        bool CanBeDelete(OperateArgs e);
    }
}
