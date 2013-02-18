using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 简单型NHibernate查询管理器，无查询条件
    /// </summary>
    public class SearchManagerProxy<T> : SearchManagerCriteria<T>
        where T : IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repCfgName"></param>
        public SearchManagerProxy(string repCfgName)
            : base(repCfgName)
        {
            this.EnablePage = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchManagerProxy()
            : this(null)
        {
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProxy<T> sm = new SearchManagerProxy<T>();
            Copy(this, sm);
            return sm;
        }
    }
}