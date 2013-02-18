using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 强类型数据操作管理器。
    /// 用于操作IList(T)类型的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IControlManager<T> : IControlManager
        where T : class, IEntity
    {
        /// <summary>
        /// DisplayManager
        /// </summary>
        IDisplayManager<T> DisplayManagerT
        {
            get;
        }
    }
}
