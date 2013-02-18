using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// HashtableCache
    /// </summary>
    public class HashtableCache : ICache
    {
        private const string s_nullValueString = "Feng.Cache.NullValue";

        /// <summary>
        /// 
        /// </summary>
        private IDictionary<string, object> hashtable;

        /// <summary>
        /// 
        /// </summary>
        protected IDictionary<string, object> HashTable
        {
            get { return hashtable; }
            set { hashtable = value; }
        }

        ///// <summary>
        ///// GetEnumerator
        ///// </summary>
        ///// <returns></returns>
        //IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        //{
        //    return hashtable.GetEnumerator();
        //}

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    return hashtable.GetEnumerator();
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        public HashtableCache()
        {
            this.hashtable = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this.hashtable.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="inCache"></param>
        /// <returns></returns>
        public object Get(string key, out bool inCache)
        {
            if (this.hashtable == null)
            {
                inCache = false;
                return null;
            }

            if (!this.hashtable.ContainsKey(key))
            {
                inCache = false;
                return null;
            }
            object ret = this.hashtable[key];
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
            if (this.hashtable == null)
                return;

            if (value == null)
            {
                value = s_nullValueString;
            }
            this.hashtable[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            this.hashtable.Remove(key);
        }
    }
}
