using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 可激活的记录
    /// </summary>
    public interface IActivableEntity : IEntity
    {
        /// <summary>
        /// 是否活动
        /// </summary>
        bool IsActive
        {
            get;
            set;
        }
    }
}
