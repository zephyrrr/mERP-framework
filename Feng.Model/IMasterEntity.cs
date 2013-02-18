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
    public interface IMasterEntity<T, S> : IEntity
        where T : class, IEntity//IMasterEntity<T, S>
        where S : class, IEntity//IDetailEntity<T, S>
    {
        /// <summary>
        /// 
        /// </summary>
        IList<S> DetailEntities { get; set; }
    }
}