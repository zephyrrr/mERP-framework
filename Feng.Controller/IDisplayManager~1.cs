using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDisplayManager<T> : IDisplayManager
        where T : class, IEntity
    {
        /// <summary>
        /// 强类型实体类列表
        /// </summary>
        IList<T> Entities
        {
            get;
        }

        /// <summary>
        /// 强类型当前实体类
        /// </summary>
        T CurrentEntity
        {
            get;
        }

        
    }
}
