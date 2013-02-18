using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPersistentCache : ICache
    {
        /// <summary>
        /// 
        /// </summary>
        void Persistent();

        /// <summary>
        /// 
        /// </summary>
        void Unpersistent();

        /// <summary>
        /// 
        /// </summary>
        void Destroy();

        /// <summary>
        /// 
        /// </summary>
        DateTime? CacheTime
        {
            get;
        }
    }
}
