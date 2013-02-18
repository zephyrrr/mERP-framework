using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Cache
    /// </summary>
    public interface ICache// : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inCache"></param>
        /// <returns></returns>
        object Get(string key, out bool inCache);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Put(string key, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
