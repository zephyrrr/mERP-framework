using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Net
{
    public class SearchManager<T> : SearchManager
        where T : class, IEntity, new()
    {
        public SearchManager(string webServiceTypeName = null, string serviceAddress = null)
            : base(webServiceTypeName, serviceAddress)
        {
        }

        /// <summary>
        /// 根据查询条件查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            string exp = SearchExpression.ToString(searchExpression);
            string order = SearchOrder.ToString(searchOrders);
            var list = m_client.GetData(exp, order, this.FirstResult, this.MaxResult);
            var ret = new List<T>();
            foreach (var i in list)
            {
                ret.Add(Feng.Net.Utils.TypeHelper.ConvertTypeFromWSToReal<T>(i));
            }
            return ret;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManager<T> sm = new SearchManager<T>();
            sm.m_client = this.m_client;
            return sm;
        }
    }
}
