using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IDetailEntity<T, S> : IEntity
        where T : class, IEntity//IMasterEntity<T, S>
        where S : class, IEntity//IDetailEntity<T, S>
    {
        /// <summary>
        /// 
        /// </summary>
        T MasterEntity { get; set; }
    }
}