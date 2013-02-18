using System;
using System.Collections.Generic;
using System.Text;
using Feng.Utils;

namespace Feng
{
    /// <summary>
    /// Cache
    /// </summary>
    public sealed class Cache : IDataBuffer
    {
        /// <summary>
        /// 
        /// </summary>
        public void LoadData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            ICache c = ServiceProvider.GetService<ICache>();
            if (c != null)
            {
                c.Clear();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Cache()
        {
        }

        /// <summary>
        /// 首先从Cache中找，如果找不到则从函数中返回结果，并填充到Cache中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="funcGet"></param>
        /// <returns></returns>
        public static T TryGetCache<T>(string key, Func<T> funcGet)
        {
            ICache cache = ServiceProvider.GetService<ICache>();

            if (key == null || cache == null)
            {
                Logger.Debug(string.Format("Cache is not enabled, {0} will retrive by func!", key));

                return funcGet();
            }

            bool inCache;
            object cachedRet = cache.Get(key, out inCache);
            if (!inCache)
            {
                Logger.Debug(string.Format("Cache Key {0} is not founded, it will retrive by func!", key));

                var ret = funcGet();
                cache.Put(key, ret);
                return ret;
            }
            else
            {
                Logger.Debug(string.Format("Cache Key {0} is founded!", key));

                return (T)cachedRet;
            }
        }
    }
}
