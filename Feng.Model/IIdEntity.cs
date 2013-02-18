using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 带Id的实体类（通过Id比较是否相同）
    /// </summary>
    /// <typeparam name="TIdentity"></typeparam>
    public interface IIdEntity<TIdentity> : IEquatable<IIdEntity<TIdentity>>
    {
        /// <summary>
        /// GetId
        /// </summary>
        TIdentity Identity
        {
            get;
        }
    }
}
