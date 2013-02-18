using System;
using System.Collections.Generic;
using System.Text;
using Feng.Collections;

namespace Feng.Data
{
    /// <summary>
    /// SearchManagerGroupByDetail
    /// </summary>
    public class SearchManagerGroupByDetail : SearchManagerWithParent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        public SearchManagerGroupByDetail(IDisplayManager dmParent, string tableName, string defaultOrder)
            : this(dmParent, tableName, defaultOrder, null, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="groupByClause"></param>
        /// <param name="selectClause"></param>
        public SearchManagerGroupByDetail(IDisplayManager dmParent, string tableName, string defaultOrder, string selectClause, string groupByClause)
            : base(dmParent)
        {
            m_parentSm = dmParent.SearchManager as SearchManager;
            if (m_parentSm == null)
            {
                SearchManagerGroupByDetail se = dmParent.SearchManager as SearchManagerGroupByDetail;
                if (se == null)
                {
                    throw new ArgumentException("SearchManagerGroupByDetail's Parent SearchManager must be Feng.Data.SearchManager or SearchManagerGroupByDetail!", "se");
                }
                m_parentSm = se.m_innerSearchManager;
            }
            if (string.IsNullOrEmpty(m_parentSm.GroupBySql))
            {
                throw new ArgumentException("SearchManagerGroupByDetail's Parent SearchManager must has groupby sub clause!", "GroupBySql");
            }
            string s = m_parentSm.GroupBySql.ToUpper().Replace("GROUP BY", "");
            m_groupByColumns = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < m_groupByColumns.Length; ++i)
            {
                m_groupByColumns[i] = m_groupByColumns[i].Trim();
            }
            m_innerSearchManager = new SearchManager(tableName, defaultOrder, selectClause, groupByClause);
            m_innerSearchManager.Name = this.Name + ".Inner";

            m_tableName = tableName;
            m_defaultOrder = defaultOrder;
            m_selectClause = selectClause;
            m_groupByClause = groupByClause;

            m_innerSearchManager.EnablePage = false;
        }

        private SearchManager m_innerSearchManager;
        private string m_tableName;
        private string m_defaultOrder;
        private string m_selectClause, m_groupByClause;

        private SearchManager m_parentSm;
        private string[] m_groupByColumns;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem)
        {
            if (parentItem == null)
            {
                return null;
            }

            SearchHistoryInfo his = this.ParentDisplayManager.SearchManager.GetHistory(0);
            if (!string.IsNullOrEmpty(his.Expression))
            {
                ISearchExpression exp = SearchExpression.Parse(his.Expression);
                exp = RemoveParentSelectAsExpression(exp);
                searchExpression = SearchExpression.And(searchExpression, exp);
            }

            foreach (string s in m_groupByColumns)
            {
                object r = EntityScript.GetPropertyValue(parentItem, s);
                ISearchExpression se2;

                if (r != null && r != System.DBNull.Value)
                {
                    se2 = SearchExpression.Eq(s, EntityScript.GetPropertyValue(parentItem, s));
                }
                else
                {
                    se2 = SearchExpression.IsNull(s);
                }
                searchExpression = SearchExpression.And(searchExpression, se2);
            }

            this.SetHistory(searchExpression, searchOrders);

            return m_innerSearchManager.GetData(searchExpression, searchOrders);
        }

        /// <summary>
        /// FindData
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            object row = this.ParentDisplayManager.CurrentItem;

            return GetData(searchExpression, searchOrders, row);
        }

        private ISearchExpression RemoveParentSelectAsExpression(ISearchExpression exp)
        {
            if (m_parentSm.SelectAsColumns == null)
               /* || m_innerSearchManager.Count == 0) */ // 不知道为什么有这句话
            {
                return exp;
            }
            Feng.Search.LogicalExpression le = exp as Feng.Search.LogicalExpression;
            if (le != null)
            {
                ISearchExpression sel = RemoveParentSelectAsExpression(le.LeftHandSide);
                ISearchExpression ser = RemoveParentSelectAsExpression(le.RightHandSide);
                return new Feng.Search.LogicalExpression(sel, ser, le.LogicOperator);
            }
            else
            {
                Feng.Search.SimpleExpression se = exp as Feng.Search.SimpleExpression;
                if (se != null)
                {
                    if (m_parentSm.SelectAsColumns.ContainsKey(se.FullPropertyName))
                    {
                        return SearchExpression.True();
                    }
                    else
                    {
                        return se;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerGroupByDetail sm = new SearchManagerGroupByDetail(this.ParentDisplayManager, m_tableName, m_defaultOrder, m_selectClause, m_groupByClause);
            Copy(this, sm);
            return sm;
        }
    }

}
