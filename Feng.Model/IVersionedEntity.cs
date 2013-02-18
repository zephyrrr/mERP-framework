using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 具有版本号的实体类接口
    /// </summary>
    public interface IVersionedEntity : IEntity
    {
        /// <summary>
        /// 版本号
        /// </summary>
        int Version
        {
            get;
            set;
        }
    }
}
