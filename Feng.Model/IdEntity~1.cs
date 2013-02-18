using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// IdEntity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IdEntity<T>
    {
        /// <summary>
        /// Id
        /// </summary>
        T Id { get; set; }
    }
}
