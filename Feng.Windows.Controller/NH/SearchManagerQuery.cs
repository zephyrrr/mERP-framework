using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Feng.Search;
using Feng.Utils;
using System.Linq;
using NHibernate.Linq;

namespace Feng.NH
{
    /// <summary>
    /// 用Hibernate Query Language的SearchManager
    /// </summary>
    public class SearchManagerQuery<T> : AbstractSearchManager<T>
        where T : IEntity
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public SearchManagerQuery(string repCfgName)
            : base(repCfgName)
        {
            this.EnablePage = true;
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        public SearchManagerQuery()
            : this(null)
        {
        }

        internal void GetDataCountInternal(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            IList<T> dataSource;
            int count;

            using (var rep = new Repository(this.RepositoryCfgName))
            //using (var tx = rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                rep.BeginTransaction();
                dataSource = GetData(searchExpression, searchOrders, rep.Session) as IList<T>;
                // 当不是第一页的时候，不能通过返回条数当作总条数
                if (this.EnablePage && this.FirstResult == 0 && dataSource.Count < this.MaxResult)
                {
                    count = dataSource.Count;
                }
                else
                {
                    count = GetCount(searchExpression, rep.Session);
                }

                rep.CommitTransaction();
            }

            base.Count = count;
            base.Result = dataSource;
        }
        /// <summary>
        /// 根据查询条件读入数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            GetDataCountInternal(searchExpression, searchOrders);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            using (var rep = new Repository(this.RepositoryCfgName))
            {
                return GetData(searchExpression, searchOrders, rep.Session);
            }
        }

        /// <summary>
        /// 查询数据条数
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public override int GetCount(ISearchExpression searchExpression)
        {
            using (var rep = new Repository(this.RepositoryCfgName))
            {
                return GetCount(searchExpression, rep.Session);
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, NHibernate.ISession session)
        {
            Dictionary<string, object> paramNames = new Dictionary<string, object>();
            string q = CreateQueryString(searchExpression, searchOrders, session.SessionFactory, paramNames);

            NHibernate.IQuery query = session.CreateQuery(q);
            foreach (KeyValuePair<string, object> kvp in paramNames)
            {
                IList arr = kvp.Value as IList;
                if (arr == null)
                {
                    query.SetParameter(kvp.Key, kvp.Value);
                }
                else
                {
                    query.SetParameterList(kvp.Key, arr);
                }
            }
            OnQueryCreated(query);

            IList<T> list = query.List<T>();
            return list;
        }

        /// <summary>
        /// 查询数据条数
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private int GetCount(ISearchExpression searchExpression, NHibernate.ISession session)
        {
            Dictionary<string, object> paramNames = new Dictionary<string, object>();
            string q = CreateQueryCountString(searchExpression, session.SessionFactory, paramNames);

            NHibernate.IQuery query = session.CreateQuery(q);
            foreach (KeyValuePair<string, object> kvp in paramNames)
            {
                IList arr = kvp.Value as IList;
                if (arr == null)
                {
                    query.SetParameter(kvp.Key, kvp.Value);
                }
                else
                {
                    query.SetParameterList(kvp.Key, arr);
                }
            }

            IList list = query.List();
            return Feng.Utils.ConvertHelper.ToInt(list[0]).Value;
        }

        #region "Query"
        private string CreateQueryCountString(ISearchExpression searchExpression, NHibernate.ISessionFactory sessionFactory, Dictionary<string, object> paramNames)
        {
            if (searchExpression is QueryExpression)
            {
                string s = (searchExpression as QueryExpression).Query;

                int idx;
                //// remove joins
                //string[] joins = new string[] { "inner join", "left outer join", "right outer join", "full outer join", "left join", "right join", "full join" };
                //
                //foreach(string join in joins)
                //{
                //    idx = s.IndexOf(join);
                //    if (idx != -1)
                //    {
                //        s = s.Substring(0, idx).Trim();
                //    }
                //}

                idx = s.IndexOf("from");
                if (idx != -1)
                {
                    s = s.Substring(idx);
                }
                return "select count(*) " + s;
            }

            NHibernate.Metadata.IClassMetadata metaData = sessionFactory.GetClassMetadata(typeof(T));

            StringBuilder sb = new StringBuilder();
            sb.Append("select count(*) ");
            sb.Append("from ");
            sb.Append(metaData.EntityName);
            sb.Append(" ");
            if (searchExpression != null)
            {
                sb.Append("where ");

                sb.Append(GetQueryWhere(searchExpression, sessionFactory, paramNames));
            }
            return sb.ToString();
        }

        private string CreateQueryString(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, NHibernate.ISessionFactory sessionFactory, Dictionary<string, object> paramNames)
        {
            if (searchExpression is QueryExpression)
            {
                return (searchExpression as QueryExpression).Query;
            }

            NHibernate.Metadata.IClassMetadata metaData = sessionFactory.GetClassMetadata(typeof(T));

            StringBuilder sb = new StringBuilder();
            sb.Append("from ");
            sb.Append(metaData.EntityName);
            sb.Append(" as Current ");
            if (searchExpression != null)
            {
                sb.Append("where ");
                sb.Append(GetQueryWhere(searchExpression, sessionFactory, paramNames));
            }

            if (searchOrders != null && searchOrders.Count > 0)
            {
                sb.Append("order by ");
                for (int i = 0; i < searchOrders.Count; ++i)
                {
                    sb.Append(GetQueryOrder(searchOrders[i]));
                    if (i != searchOrders.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
            }

            // Eager Fetch
            if (this.EagerFetchs != null)
            {
                foreach (string s in this.EagerFetchs)
                {
                    sb.Append(" left join fetch Current.");
                    sb.Append(s);
                }
            }

            return sb.ToString();
        }

        private void OnQueryCreated(NHibernate.IQuery query)
        {
            if (EnablePage)
            {
                query.SetFirstResult(FirstResult);
                if (MaxResult != -1)
                {
                    query.SetMaxResults(MaxResult);
                }
            }


            if (IsResultDistinct)// || m_hasCollection)
            {
                query.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            }
        }

        private string GetQueryOrder(ISearchOrder so)
        {
            if (so.Ascending)
            {
                return so.PropertyName;
            }
            else
            {
                return so.PropertyName + " desc";
            }
        }

        private string GetQueryWhere(ISearchExpression se, NHibernate.ISessionFactory sessionFactory, Dictionary<string, object> paramNames)
        {
            SimpleExpression cse = se as SimpleExpression;
            if (cse == null)
            {
                LogicalExpression le = se as LogicalExpression;

                string left = GetQueryWhere(le.LeftHandSide, sessionFactory, paramNames);
                string right = GetQueryWhere(le.RightHandSide, sessionFactory, paramNames);

                switch (le.LogicOperator)
                {
                    case LogicalOperator.And:
                        return "(" + left + " and " + right + ")";
                    case LogicalOperator.Or:
                        return "(" + left + " or " + right + ")";
                    case LogicalOperator.Not:
                        return "(not " + left + ")";
                    default:
                        System.Diagnostics.Debug.Assert(false, "Not supported LogicOperator");
                        return null;
                }
            }
            else
            {
                // Change Type
                if (!string.IsNullOrEmpty(cse.FullPropertyName))
                {
                    // Convert Data to dest type
                    bool hasC;
                    Type destType = NHibernateHelper.GetPropertyType(sessionFactory, typeof(T), cse.FullPropertyName, out hasC);
                    //hasCollection |= hasC;
                    IList array = cse.Values as IList;
                    if (array != null)
                    {
                        for (int i = 0; i < array.Count; ++i)
                        {
                            if (destType != array[i].GetType())
                            {
                                array[i] = ConvertHelper.ChangeType(array[i], destType);
                            }
                        }

                        //paramCnt += array.Count;
                    }
                    else
                    {
                        if (cse.Values != null)
                        {
                            if (destType != cse.Values.GetType())
                            {
                                cse.Values = ConvertHelper.ChangeType(cse.Values, destType);
                            }
                        }

                        //paramCnt++;
                    }
                }

                string propertyName = cse.FullPropertyName.Replace(":", ".");
                string paraName = null;
                switch (cse.Operator)
                {
                    case SimpleOperator.Eq:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " = " + ":" + paraName;
                    case SimpleOperator.NotEq:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " != " + ":" + paraName;
                    case SimpleOperator.EqProperty:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " = " + ":" + paraName;
                    case SimpleOperator.Like:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = "%" + cse.Values + "%";
                        return propertyName + " like " + ":" + paraName;
                    case SimpleOperator.InG:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " in " + ":" + paraName;
                    case SimpleOperator.IsNotNull:
                        return propertyName + " is not null ";
                    case SimpleOperator.IsNull:
                        return propertyName + " is null ";
                    case SimpleOperator.Gt:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " > " + ":" + paraName;
                    case SimpleOperator.Lt:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " < " + ":" + paraName;
                    case SimpleOperator.Ge:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " >= " + ":" + paraName;
                    case SimpleOperator.Le:
                        paraName = CreateParamName(cse, paramNames);
                        paramNames[paraName] = cse.Values;
                        return propertyName + " <= " + ":" + paraName;
                    case SimpleOperator.GInG:
                        {
                            IList data = cse.Values as IList;
                            // 当用于客户用途查找类型时，只能Like
                            StringBuilder sb = new StringBuilder();
                            sb.Append("(");
                            paraName = CreateParamName(cse, paramNames);
                            paramNames[paraName] = "%" + data[0] + "%";
                            sb.Append(propertyName + " like " + ":" + paraName);
                            for (int i = 1; i < data.Count; ++i)
                            {
                                sb.Append(" or ");
                                paraName = CreateParamName(cse, paramNames);
                                 paramNames[paraName] = "%" + data[i] + "%";
                                sb.Append(propertyName + " like " + ":" + paraName);
                            }
                            sb.Append(")");
                            return sb.ToString();
                        }
                    //case SimpleOperator.Any:
                    //    return NHibernate.Criterion.Expression.EqProperty(propertyName, propertyName);
                    //case SimpleOperator.Sql:
                    //    return NHibernate.Criterion.Expression.Sql(propertyName);
                    default:
                        System.Diagnostics.Debug.Assert(false, "Not supported SimpleOperator");
                        return null;
                }
            }
        }

        //private string GetQueryValueString(object value)
        //{
        //    if (value == null)
        //    {
        //        return "''";
        //    }
        //    else if (value is IList)
        //    {
        //        ArrayList arr = value as IList;
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("(");
        //        for (int i = 0; i < arr.Count; ++i)
        //        {
        //            sb.Append(GetQueryValueString(arr[i]));
        //            if (i != arr.Count - 1)
        //            {
        //                sb.Append(",");
        //            }
        //        }
        //        sb.Append(")");
        //        return sb.ToString();
        //    }
        //    else
        //    {
        //        return "'" + value.ToString() + "'";
        //    }
        //}

        private string CreateParamName(SimpleExpression cse, Dictionary<string, object> paramsName)
        {
            string propertyName = cse.FullPropertyName.Replace(":", "");
            string simpleParamName = propertyName;
            string complexParamName = propertyName + cse.Operator.ToString();
            switch (cse.Operator)
            {
                case SimpleOperator.Any:
                case SimpleOperator.EqProperty:
                case SimpleOperator.IsNotNull:
                case SimpleOperator.IsNull:
                case SimpleOperator.NotEq:
                case SimpleOperator.NotEqProperty:
                case SimpleOperator.Sql:
                    return GetParameterName(simpleParamName, paramsName);
                case SimpleOperator.Ge:
                case SimpleOperator.Gt:
                case SimpleOperator.Le:
                case SimpleOperator.Lt:
                    return GetParameterName(complexParamName, paramsName);
                case SimpleOperator.Eq:
                case SimpleOperator.GInG:
                case SimpleOperator.InG:
                case SimpleOperator.Like:
                    return GetParameterName(simpleParamName, paramsName);
                default:
                    throw new NotSupportedException("Invalid SimpleExpression Operator of " + cse.Operator);
            }
        }

        private static string GetParameterName(string baseName, Dictionary<string, object> paramsName)
        {
            baseName = baseName.Replace('(', '9').Replace(')', '9').Replace('*', '9');

            if (!paramsName.ContainsKey(baseName))
            {
                return baseName;
            }
            int i = 0;
            while (true)
            {
                string name = baseName + i.ToString();
                if (!paramsName.ContainsKey(name))
                {
                    return name;
                }
                i++;
            }
        }
        #endregion

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerQuery<T> sm = new SearchManagerQuery<T>();
            Copy(this, sm);
            return sm;
        }
    }
}
