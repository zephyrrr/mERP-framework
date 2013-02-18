using System;
using System.Collections.Generic;
using System.Text;
using Feng.Search;

namespace Feng.NH
{
    public class SearchManager<T> : AbstractSearchManager<T>
        where T : IEntity
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public SearchManager()
            : this(null)
        {
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        public SearchManager(string repCfgName)
            : base(repCfgName)
        {
        }

        public override object Clone()
        {
            SearchManager<T> sm = new SearchManager<T>();
            Copy(this, sm);
            return sm;
        }

        private SearchManagerQuery<T> m_queryManager;
        private SearchManagerQuery<T> QueryManager
        {
            get
            {
                if (m_queryManager == null)
                {
                    m_queryManager = new SearchManagerQuery<T>(base.RepositoryCfgName);
                }
                return m_queryManager;
            }
        }
        private SearchManagerCriteria<T> m_criteriaManager;
        private SearchManagerCriteria<T> CriteriaManager
        {
            get
            {
                if (m_criteriaManager == null)
                {
                    m_criteriaManager = new SearchManagerCriteria<T>(base.RepositoryCfgName);
                }
                return m_criteriaManager;
            }
        }

        private void CopyInner(AbstractSearchManager src, AbstractSearchManager dest)
        {
            dest.EnablePage = src.EnablePage;
            dest.FirstResult = src.FirstResult;
            dest.IsResultDistinct = src.IsResultDistinct;
            dest.MaxResult = src.MaxResult;
            dest.AdditionalSearchExpression = src.AdditionalSearchExpression;
            dest.AdditionalSearchOrder = src.AdditionalSearchOrder;

            dest.EagerFetchs.Clear();
            foreach (string s in src.EagerFetchs)
            {
                dest.EagerFetchs.Add(s);
            }
        }

        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            if (searchExpression is QueryExpression)
            {
                CopyInner(this, this.QueryManager);

                this.QueryManager.GetDataCountInternal(searchExpression, searchOrders);
                this.Count = this.QueryManager.Count;
                this.Result = this.QueryManager.Result;
            }
            else
            {
                CopyInner(this, this.CriteriaManager);

                this.CriteriaManager.GetDataCountInternal(searchExpression, searchOrders);
                this.Count = this.CriteriaManager.Count;
                this.Result = this.CriteriaManager.Result;
            }
        }

        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            if (searchExpression is QueryExpression)
            {
                CopyInner(this, this.QueryManager);
                return this.QueryManager.GetData(searchExpression, searchOrders);
            }
            else
            {
                CopyInner(this, this.CriteriaManager);
                return this.CriteriaManager.GetData(searchExpression, searchOrders);
            }
        }

        public override int GetCount(ISearchExpression searchExpression)
        {
            if (searchExpression is QueryExpression)
            {
                CopyInner(this, this.QueryManager);
                return this.QueryManager.GetCount(searchExpression);
            }
            else
            {
                CopyInner(this, this.CriteriaManager);
                return this.CriteriaManager.GetCount(searchExpression);
            }
        }
    }
}
