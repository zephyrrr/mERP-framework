using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    public class Cache : ICache
    {
        public Cache()
        {
            m_cache = m_cp.BuildCache("Global", m_initProps);
            m_cp.Start(m_initProps);
        }


        private NHibernate.Cache.ICacheProvider m_cp = Feng.Utils.ReflectionHelper.CreateInstanceFromName(s_cacheProvider) as NHibernate.Cache.ICacheProvider;
        private NHibernate.Cache.ICache m_cache;

        //// NHibernate.config: NHibernate.Caches.MemCache.MemCacheProvider,NHibernate.Caches.MemCache
        /* <section name="memcache" type="NHibernate.Caches.MemCache.MemCacheSectionHandler,NHibernate.Caches.MemCache" />
         * <memcache>
    <memcached host="127.0.0.1" port="11211" weight="2" />
	</memcache>
         */
        //private const string s_cacheProvider = "NHibernate.Caches.MemCache.MemCacheProvider,NHibernate.Caches.MemCache";
        //private Dictionary<string, string> m_initProps = new Dictionary<string, string> { { "host", "127.0.0.1" }, { "port", "11211" }, { "weight", "1" } };

        private const string s_cacheProvider = "NHibernate.Caches.SysCache.SysCacheProvider, NHibernate.Caches.SysCache";
        private Dictionary<string, string> m_initProps = new Dictionary<string, string> { };

        // Prevalence要写磁盘，性能不太好
        //private const string s_cacheProvider = "NHibernate.Caches.Prevalence.PrevalenceCacheProvider, NHibernate.Caches.Prevalence";
        //private Dictionary<string, string> m_initProps = new Dictionary<string, string> { { "prevalenceBase", "PrevalenceCache" } };
        // Dont't call stop which will delete cache dir

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_cache.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inCache"></param>
        /// <returns></returns>
        public object Get(string key, out bool inCache)
        {
            object ret = m_cache.Get(key);
            if (ret == null)
            {
                inCache = false;
                return null;
            }
            inCache = true;
            if (ret is string && (string)ret == s_nullValueString)
            {
                return null;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            bool inCache;
            return Get(key, out inCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(string key, object value)
        {
            if (value == null)
            {
                value = s_nullValueString;
            }
            m_cache.Put(key, value);
        }

        private const string s_nullValueString = "Feng.Cache.NullValue";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            m_cache.Remove(key);
        }
    }
}
