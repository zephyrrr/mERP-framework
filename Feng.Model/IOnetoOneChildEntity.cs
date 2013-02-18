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
    public interface IOnetoOneChildEntity<T, S> : IEntity
        where T : class, IOnetoOneParentEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// 
        /// </summary>
        T ParentEntity { get; set; }
    }
}
