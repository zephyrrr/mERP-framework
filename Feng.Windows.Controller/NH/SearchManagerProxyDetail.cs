using System;
using System.Collections.Generic;
using System.Text;
using Feng.Collections;

namespace Feng.NH
{
    /// <summary>
    /// SearchManagerProxyDetail
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class SearchManagerProxyDetail<T, S> : SearchManagerProxyDetail<S>
        where T : class, IEntity
        where S : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="searchExpression"></param>
        public SearchManagerProxyDetail(IDisplayManager dmParent, string searchExpression)
            : base(dmParent, searchExpression)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        /// <param name="searchExpression"></param>
        public SearchManagerProxyDetail(IControlManager cmParent, string searchExpression)
            : base(cmParent, searchExpression)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SearchManagerProxyDetail<S> : SearchManagerWithParent<S>
        where S : class, IEntity
    {
        private SearchManagerCriteria<S> m_innerSearchManager;
        private string m_searchExpression;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="searchExpression"></param>
        public SearchManagerProxyDetail(IDisplayManager dmParent, string searchExpression)
            : base(dmParent, null)
        {
            m_innerSearchManager = new SearchManagerCriteria<S>();
            m_searchExpression = searchExpression;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        /// <param name="searchExpression"></param>
        public SearchManagerProxyDetail(IControlManager cmParent, string searchExpression)
            : base(cmParent, null)
        {
            m_searchExpression = searchExpression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem)
        {
            object entity = parentItem;
            if (entity == null)
            {
                return null;
            }
            string exp = EntityHelper.ReplaceEntity(m_searchExpression, entity);
            if (!string.IsNullOrEmpty(exp))
            {
                searchExpression = SearchExpression.And(SearchExpression.Parse(exp), searchExpression);
            }

            return base.GetData(searchExpression, searchOrders);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            object entity = this.ParentDisplayManager.CurrentItem;
            return GetData(searchExpression, searchOrders, entity);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProxyDetail<S> sm = new SearchManagerProxyDetail<S>(this.ParentDisplayManager, m_searchExpression);
            Copy(this, sm);
            return sm;
        }
    }
}
