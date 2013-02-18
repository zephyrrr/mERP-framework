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
    public interface IOnetoOneParentEntity<T, S> : IEntity
        where T : class, IOnetoOneParentEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// 
        /// </summary>
        S ChildEntity { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IOnetoOneParentGenerateChildEntity<T, S> : IOnetoOneParentEntity<T, S>
        where T : class, IOnetoOneParentEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// 
        /// </summary>
        Type ChildType { get; }
    }
}
