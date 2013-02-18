using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistentHashtableCache : HashtableCache, IPersistentCache
    {
        public string CacheFileName
        {
            get
            {
                return string.Format("{0}\\Cache.{1}.dat", SystemDirectory.DataDirectory, SystemConfiguration.UserName);
            }
        }

        public PersistentHashtableCache()
        {
        }

        // <summary>
        /// 
        /// </summary>
        public void Persistent()
        {
            Utils.SerializeHelper.Serialize(CacheFileName, HashTable);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unpersistent()
        {
            base.HashTable = Utils.SerializeHelper.Deserialize<IDictionary<string, object>>(CacheFileName);
            if (base.HashTable == null)
            {
                base.HashTable = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            if (System.IO.File.Exists(CacheFileName))
            {
                System.IO.File.Delete(CacheFileName);
            }
            this.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CacheTime
        {
            get
            {
                if (System.IO.File.Exists(CacheFileName))
                {
                    return new System.IO.FileInfo(CacheFileName).LastWriteTime;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
