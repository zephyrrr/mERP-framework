using System;
using System.Collections.Generic;
using System.Text;
using Feng.Search;

namespace Feng
{
    /// <summary>
    /// 搜索表达式，关键字需用空格间隔。
    /// 表达式目前支持下列关键字
    /// <list type="bullet">
    /// <item>NOT: 取反</item>
    /// <item>AND: 而且</item>
    /// <item>OR: 或者</item>
    /// <item>(, ): 括号，改变优先级</item>
    /// <item>=: 等于</item>
    /// <item>&lt;&gt;: 不等于</item>
    /// <item>&gt;=: 大于等于</item>
    /// <item>&gt;: 大于</item>
    /// <item>&lt;=: 小于等于</item>
    /// <item>&lt;: 小于</item>
    /// <item>=P: 等于属性。 例如 A =p B, 说明是A字段=B字段</item>
    /// <item>LIKE: 相似</item>
    /// <item>ISNULL: 为空</item>
    /// <item>ISNOTNULL: 不为空</item>
    /// <item>IN: 在集合中。集中表示为[x,..]。例如, A IN [2,3,4], 表示A为2,3,4中的一个</item>
    /// <item>ING: 集合在集合中。集中表示为[x,..]。例如, A ING [2,3,4], 表示A包含2,3,4中的一个，A可能为"2,3"</item>
    /// <item>IsSql: 用sql语句表达</item>
    /// </list>
    /// <example>
    /// <list type="bullet">
    /// <item>Product.Name LIKE '%abc' AND TotalPrice ISNOTNULL</item>
    /// <item>Product.Price >= 1234 OR (Product.Price ISNULL AND Receipt = 'Feng')</item>
    /// </list>
    /// </example>
    /// </summary>
    public sealed class SearchExpression
    {
        private SearchExpression()
        {
        }

        /// <summary>
        /// 根据查询条件列表得到可读字符串
        /// </summary>
        /// <param name="searchExpressions"></param>
        /// <returns></returns>
        public static string ToString(IList<ISearchExpression> searchExpressions)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < searchExpressions.Count; ++i)
            {
                if (i > 0)
                {
                    sb.Append(" AND ");
                }
                sb.Append(searchExpressions[i].ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <param name="searchExpressions"></param>
        /// <returns></returns>
        public static string ToString(ISearchExpression searchExpressions)
        {
            if (searchExpressions == null)
            {
                return string.Empty;
            }
            else
            {
                return searchExpressions.ToString();
            }
        }

        /// <summary>
        /// List To One
        /// </summary>
        /// <param name="searchExpressions"></param>
        /// <returns></returns>
        public static ISearchExpression ToSingle(IList<ISearchExpression> searchExpressions)
        {
            if (searchExpressions == null || searchExpressions.Count == 0)
            {
                return null;
            }
            ISearchExpression se = searchExpressions[0];
            for (int i = 1; i < searchExpressions.Count; ++i)
            {
                se = SearchExpression.And(se, searchExpressions[i]);
            }
            return se;
        }

        /// <summary>
        /// 根据可读字符串得到查询条件
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public static IList<ISearchExpression> ParseToList(string searchExpression)
        {
            IList<ISearchExpression> ret = new List<ISearchExpression>();
            ISearchExpression exp = SearchExpression.Parse(searchExpression);
            if (exp != null)
            {
                ret.Add(exp);
            }
            return ret;
        }

        /// <summary>
        /// 根据可读字符串得到查询条件
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public static ISearchExpression Parse(string searchExpression)
        {
            if (string.IsNullOrEmpty(searchExpression))
                return null;
            if (searchExpression.StartsWith(QueryExpression.QueryExpressionIdentity, StringComparison.InvariantCulture))
            {
                return new QueryExpression(searchExpression.Substring(QueryExpression.QueryExpressionIdentity.Length));
            }

            try
            {
                return new Parser().Parse(searchExpression);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("表达式\"" + searchExpression + "\"格式错误！", ex);
            }
        }

        /// <summary>
        /// 把Expression列表转化为单一Expression（use and）
        /// </summary>
        /// <param name="exps"></param>
        /// <returns></returns>
        public static ISearchExpression ConvertListToOne(IList<ISearchExpression> exps)
        {
            if (exps.Count == 0)
                return null;
            ISearchExpression exp = exps[0];
            for (int i = 1; i < exps.Count; ++i)
            {
                exp = SearchExpression.And(exp, exps[i]);
            }
            return exp;
        }

        /// <summary>
        /// 把SearchExpression分解成SimpleExpression列表
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public static IList<SimpleExpression> GetSimpleExpressions(ISearchExpression searchExpression)
        {
            List<SimpleExpression> list = new List<SimpleExpression>();
            GetSimpleExpressions(searchExpression, list);
            return list;
        }

        private static void GetSimpleExpressions(ISearchExpression searchExpression, List<SimpleExpression> list)
        {
            if (searchExpression is SimpleExpression)
            {
                list.Add(searchExpression as SimpleExpression);
            }
            else if (searchExpression is LogicalExpression)
            {
                LogicalExpression le = searchExpression as LogicalExpression;
                if (le.LogicOperator == LogicalOperator.And || le.LogicOperator == LogicalOperator.Or)
                {
                    GetSimpleExpressions(le.LeftHandSide, list);
                    GetSimpleExpressions(le.RightHandSide, list);
                }
                else if (le.LogicOperator == LogicalOperator.Not)
                {
                    GetSimpleExpressions(le.LeftHandSide, list);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static ISearchExpression And(ISearchExpression lhs, ISearchExpression rhs)
        {
            if (lhs == null)
            {
                return rhs;
            }
            else if (rhs == null)
            {
                return lhs;
            }
            else
            {
                return new LogicalExpression(lhs, rhs, LogicalOperator.And);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static ISearchExpression Or(ISearchExpression lhs, ISearchExpression rhs)
        {
            if (lhs == null)
            {
                return rhs;
            }
            else if (rhs == null)
            {
                return lhs;
            }
            else
            {
                return new LogicalExpression(lhs, rhs, LogicalOperator.Or);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public static ISearchExpression Not(ISearchExpression hs)
        {
            return new LogicalExpression(hs, null, LogicalOperator.Not);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression Eq(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.Eq);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression NotEq(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.NotEq);
        }


        /// <summary>
        /// Apply an "equal" constraint to the named property
        /// </summary>
        /// <param name="propertyName">The name of the Property in the class.</param>
        /// <param name="value">The value for the Property.</param>
        public static ISearchExpression EqProperty(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.EqProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression NotEqProperty(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.NotEqProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression Gt(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.Gt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression Ge(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.Ge);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression Lt(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.Lt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression Le(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.Le);
        }

        /// <summary>
        /// Apply a "like" constraint to the named property
        /// </summary>
        /// <param name="propertyName">The name of the Property in the class.</param>
        /// <param name="value">The value for the Property.</param>
        public static ISearchExpression Like(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.Like);
        }

        /// <summary>
        /// Apply an "equal" constraint to the named property
        /// </summary>
        /// <param name="propertyName">The name of the Property in the class.</param>
        /// <param name="value">The value for the Property.</param>
        public static ISearchExpression NotLike(string propertyName, object value)
        {
            return SearchExpression.Not(new SimpleExpression(propertyName, value, SimpleOperator.Like));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression InG(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.InG);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression NotInG(string propertyName, object value)
        {
            return SearchExpression.Not(new SimpleExpression(propertyName, value, SimpleOperator.InG));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression GInG(string propertyName, object value)
        {
            return new SimpleExpression(propertyName, value, SimpleOperator.GInG);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression NotGInG(string propertyName, object value)
        {
            return SearchExpression.Not(new SimpleExpression(propertyName, value, SimpleOperator.GInG));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ISearchExpression IsNull(string propertyName)
        {
            return new SimpleExpression(propertyName, null, SimpleOperator.IsNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ISearchExpression IsNotNull(string propertyName)
        {
            return new SimpleExpression(propertyName, null, SimpleOperator.IsNotNull);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ISearchExpression Any(string propertyName)
        {
            return new SimpleExpression(propertyName, null, SimpleOperator.Any);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ISearchExpression Sql(string value)
        {
            return new SimpleExpression(value, null, SimpleOperator.Sql);
        }

        /// <summary>
        /// True
        /// </summary>
        /// <returns></returns>
        public static ISearchExpression True()
        {
            return new SimpleExpression("1 = 1", null, SimpleOperator.Sql);
        }

        /// <summary>
        /// False
        /// </summary>
        /// <returns></returns>
        public static ISearchExpression False()
        {
            return new SimpleExpression("1 = 2", null, SimpleOperator.Sql);
        }

        private static IList<ISearchExpression> s_emptySearchExpressions = new List<ISearchExpression>();
        /// <summary>
        /// Empty List
        /// </summary>
        /// <returns></returns>
        public static IList<ISearchExpression> Empty()
        {
            return s_emptySearchExpressions;
        }
    }
}
