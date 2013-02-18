using System;
using System.Collections.Generic;
using System.Text;
using Feng.Collections;

namespace Feng.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchManagerProxyDetailInMaster : SearchManagerWithParent
    {
        private ISearchManager m_innerSearchManager;
        private string m_searchExpression;
        private string m_searchOrder;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="searchExpression"></param>
        public SearchManagerProxyDetailInMaster(IDisplayManager dmParent, string tableName, string defaultOrder, string searchExpression)
            : this(dmParent, tableName, defaultOrder, null, null, searchExpression, null, "Normal")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrder"></param>
        public SearchManagerProxyDetailInMaster(IDisplayManager dmParent, string tableName, string defaultOrder, string searchExpression, string searchOrder)
            : this(dmParent, tableName, defaultOrder, null, null, searchExpression, searchOrder, "Normal")
        {
        }

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrder"></param>
        /// <param name="innerSearchManagerType"></param>
        public SearchManagerProxyDetailInMaster(IDisplayManager dmParent, string tableName, string defaultOrder, string searchExpression, string searchOrder, string innerSearchManagerType)
            : this(dmParent, tableName, defaultOrder, null, null, searchExpression, searchOrder, innerSearchManagerType)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="tableName"></param>
        /// <param name="defaultOrder"></param>
        /// <param name="selectClause"></param>
        /// <param name="groupByClause"></param>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrder"></param>
        /// <param name="innerSearchManagerType"></param>
        public SearchManagerProxyDetailInMaster(IDisplayManager dmParent, string tableName, string defaultOrder, string selectClause, string groupByClause, string searchExpression, string searchOrder, string innerSearchManagerType)
            : base(dmParent)
        {
            switch (innerSearchManagerType.ToUpper())
            {
                case "":
                case "NORMAL":
                    m_innerSearchManager = new SearchManager(tableName, defaultOrder, selectClause, groupByClause);
                    break;
                case "FUNCTION":
                    m_innerSearchManager = new SearchManagerFunction(tableName, defaultOrder);
                    break;
                case "PROCEDURE":
                    m_innerSearchManager = new SearchManagerProcedure(tableName);
                    break;
                default:
                    throw new NotSupportedException("innerSearchManagerType is invalid!");
            }
            
            m_searchExpression = searchExpression;
            m_searchOrder = searchOrder;

            m_innerSearchManager.EnablePage = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrder"></param>
        /// <param name="innerSearchManager"></param>
        public SearchManagerProxyDetailInMaster(IDisplayManager dmParent, string searchExpression, string searchOrder, ISearchManager innerSearchManager)
            : base(dmParent)
        {
            m_searchExpression = searchExpression;
            m_searchOrder = searchOrder;

            m_innerSearchManager = innerSearchManager;
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="cmParent"></param>
        //public SearchManagerProxyDetailInMaster(IControlManager cmParent)
        //    : base(cmParent)
        //{
        //}

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
            string exp = EntityHelper.ReplaceEntity(m_searchExpression, parentItem);

            searchExpression = SearchExpression.And(SearchExpression.Parse(exp), searchExpression);

            if (!string.IsNullOrEmpty(m_searchOrder))
            {
                return m_innerSearchManager.GetData(searchExpression, SearchOrder.Parse(m_searchOrder));
            }
            else
            {
                return m_innerSearchManager.GetData(searchExpression, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            object row = this.ParentDisplayManager.CurrentItem;
            return GetData(searchExpression, searchOrders, row);
        }


        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProxyDetailInMaster sm = new SearchManagerProxyDetailInMaster(this.ParentDisplayManager, 
                m_searchExpression, m_searchOrder, m_innerSearchManager);
            Copy(this, sm);
            return sm;
        }
    }
}
