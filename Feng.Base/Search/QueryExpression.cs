using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Search
{
    /// <summary>
    /// 直接查询语句的SearchExpression
    /// </summary>
    public class QueryExpression : ISearchExpression
    {
        ///// <summary>
        ///// 从ISearchExpression转换
        ///// </summary>
        ///// <param name="se"></param>
        ///// <param name="isCount"></param>
        ///// <returns></returns>
        //public static QueryExpression ConvertFromSearchExpression(ISearchExpression se, bool isCount)
        //{
        //    LogicalExpression query = se as LogicalExpression;
        //    if (query == null)
        //    {
        //        throw new ArgumentException("SearchManagerQuery SearchExpression must be LogicalExpression!");
        //    }
        //    SimpleExpression realQuery;
        //    if (!isCount)
        //    {
        //        realQuery = query.LeftHandSide as SimpleExpression;
                
        //    }
        //    else
        //    {
        //        realQuery = query.RightHandSide as SimpleExpression;
        //    }

        //    if (realQuery == null || realQuery.Operator != Feng.Search.SimpleOperator.Sql)
        //    {
        //        throw new ArgumentException("SearchManagerQuery SearchExpression's LeftRightHandSide must be SimpleExpression of IsSql!");
        //    }

        //    return new QueryExpression(realQuery.FullPropertyName);
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="query"></param>
        public QueryExpression(string query)
        {
            m_query = query;
        }
        private string m_query;

        /// <summary>
        /// 查询语句
        /// </summary>
        public string Query
        {
            get { return m_query; }
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(m_query))
            {
                return QueryExpressionIdentity + string.Empty;
            }
            else
            {
                return QueryExpressionIdentity + m_query.ToString();
            }
        }

        /// <summary>
        /// QueryExpressionIdentity
        /// </summary>
        public const string QueryExpressionIdentity = "Query ";
    }
}
