using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NHibernate;
using Feng.Collections;
using Feng.Search;
using Feng.Utils;

namespace Feng.NH
{
    /// <summary>
    /// 用于NHibernate的查找管理器
    /// </summary>
    public class SearchManagerCriteria<T> : AbstractSearchManager<T>
        where T : IEntity
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public SearchManagerCriteria()
            : this(null)
        {
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        public SearchManagerCriteria(string repCfgName)
            : base(repCfgName)
        {
            this.EnablePage = true;
        }

        #region "Criteria"
        /// <summary>
        /// 查找字符串是否是精确模式还是模糊模式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsAcurateMode(string s)
        {
            if (s.EndsWith("#"))
            {
                s = s.Substring(0, s.Length - 1);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static NHibernate.Criterion.ICriterion GetLikeCriterion(string propertyName, object data, int? propertyLength)
        {
            if (data is string)
            {
                string s = data.ToString();
                if (IsAcurateMode(s))
                {
                    return NHibernate.Criterion.Expression.Eq(propertyName, s.Substring(0, s.Length - 1));
                }
                else
                {
                    if (propertyLength.HasValue)
                    {
                        if (propertyLength.Value == s.Length)
                            return NHibernate.Criterion.Expression.Eq(propertyName, s);
                        else if (propertyLength == s.Length + 1)
                            return NHibernate.Criterion.Expression.Or(
                                NHibernate.Criterion.Expression.Like(propertyName, s, NHibernate.Criterion.MatchMode.Start),
                                NHibernate.Criterion.Expression.Like(propertyName, s, NHibernate.Criterion.MatchMode.End)); 
                    }
                    return NHibernate.Criterion.Expression.Like(propertyName, s, NHibernate.Criterion.MatchMode.Anywhere);
                }
            }
            else
            {
                return NHibernate.Criterion.Expression.Eq(propertyName, data);
            }
        }

        private static string TryCreateCriteria(string fullPropertyName, ref ICriteria originalCriteria, Dictionary<string, ICriteria> aliasesDict)
        {
            StringBuilder name = new StringBuilder();
            string columnName = fullPropertyName;
            if (fullPropertyName.Contains(":"))
            {
                int idx = columnName.LastIndexOf(':');
                string prefixName = columnName.Substring(0, idx);
                columnName = columnName.Substring(idx + 1);

                string[] prefixNames = prefixName.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < prefixNames.Length; ++j)
                {
                    string prefixAlias = prefixNames[j];

                    NHibernate.SqlCommand.JoinType joinType = NHibernate.SqlCommand.JoinType.InnerJoin;
                    if (j != 0)
                    {
                        if (j == prefixNames.Length - 1)
                        {
                            joinType = GetJoinType(ref columnName);
                        }
                        else
                        {
                            joinType = GetJoinType(ref prefixNames[j]);
                        }
                    }
                    string aliasesKey;
                    if (joinType != NHibernate.SqlCommand.JoinType.InnerJoin)
                        aliasesKey = prefixAlias + "-" + joinType.ToString();
                    else
                        aliasesKey = prefixAlias;

                    if (!aliasesDict.ContainsKey(aliasesKey))
                    {
                        string preCriteria;
                        ICriteria criteria;
                        if (j == 0)
                        {
                            preCriteria = "Current";
                        }
                        else
                        {
                            preCriteria = prefixNames[j - 1];
                        }

                        criteria = CreateJoinCriteria(prefixAlias, preCriteria, aliasesDict, joinType);
                        aliasesDict.Add(aliasesKey, criteria);

                        originalCriteria = criteria;
                    }
                    else
                    {
                        originalCriteria = aliasesDict[aliasesKey];
                    }

                    if (j == prefixNames.Length - 1)
                    {
                        name.Append(prefixAlias).Append(".");
                    }
                }
            }
            else
            {
                name.Append("Current").Append(".");
                originalCriteria = aliasesDict["Current"];
            }

            return name.ToString() + columnName;
        }

        private static NHibernate.SqlCommand.JoinType GetJoinType(ref string s)
        {
            char? c = NHibernateHelper.TryRemoveJoinTypeChar(ref s);
            if (c.HasValue)
            {
                switch (c)
                {
                    case 'L':
                        return NHibernate.SqlCommand.JoinType.LeftOuterJoin;
                    case 'R':
                        return NHibernate.SqlCommand.JoinType.RightOuterJoin;
                    case 'F':
                        return NHibernate.SqlCommand.JoinType.FullJoin;
                    default:
                        return NHibernate.SqlCommand.JoinType.InnerJoin;
                }
            }
            return NHibernate.SqlCommand.JoinType.InnerJoin;
        }
        public static ICriteria CreateJoinCriteria(string prefixAlias, string preCriteria, Dictionary<string, ICriteria> aliases, NHibernate.SqlCommand.JoinType joinType)
        {
            // 不太能搞清楚两者区别
            // ICriteria criteria = aliases[preCriteria].CreateCriteria(prefixAlias, prefixAlias) = new Subcriteria;
            // ICriteria criteria = aliases[preCriteria].CreateAlias(prefixAlias, prefixAlias) = this Criteria;

            ICriteria criteria = aliases[preCriteria].CreateCriteria(prefixAlias, prefixAlias, joinType);
            return criteria;
        }

        // 如果是Component，则用入款金额.数额，不需要用:，不需要创建子Criteria。
        private static NHibernate.Criterion.ICriterion GetCriterion(ISessionFactory sessionFactory, ref ICriteria originalCriteria,
                                                                    Dictionary<string, ICriteria> aliases,
                                                                    ISearchExpression se, ref bool hasCollection, ref int paramCnt)
        {
            if (se == null)
                return null;
            if (se is Feng.Search.QueryExpression)
                return null;

            SimpleExpression cse = se as SimpleExpression;
            if (cse == null)
            {
                LogicalExpression le = se as LogicalExpression;

                NHibernate.Criterion.ICriterion left = GetCriterion(sessionFactory, ref originalCriteria, aliases, le.LeftHandSide, ref hasCollection, ref paramCnt);
                NHibernate.Criterion.ICriterion right = GetCriterion(sessionFactory, ref originalCriteria, aliases, le.RightHandSide, ref hasCollection, ref paramCnt);

                switch (le.LogicOperator)
                {
                    case LogicalOperator.And:
                        return NHibernate.Criterion.Expression.And(left, right);
                    case LogicalOperator.Or:
                        return NHibernate.Criterion.Expression.Or(left, right);
                    case LogicalOperator.Not:
                        return NHibernate.Criterion.Expression.Not(left);
                    default:
                        System.Diagnostics.Debug.Assert(false, "Not supported LogicOperator");
                        return null;
                }
            }
            else
            {
                // No PropertyName Process
                if (cse.Operator == SimpleOperator.Sql)
                {
                    return NHibernate.Criterion.Expression.Sql(cse.FullPropertyName);
                }

                string propertyName = TryCreateCriteria(cse.FullPropertyName, ref originalCriteria, aliases);

                if (!string.IsNullOrEmpty(cse.FullPropertyName))
                {
                    // Convert Data to dest type
                    bool hasC;
                    Type destType;
                    if (cse.Operator != SimpleOperator.EqProperty)
                    {
                        destType = NHibernateHelper.GetPropertyType(sessionFactory, typeof(T), cse.FullPropertyName, out hasC);
                        hasCollection |= hasC;
                    }
                    else
                    {
                        destType = typeof(string);
                    }

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

                        paramCnt += array.Count;
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

                        paramCnt++;
                    }
                }

                switch (cse.Operator)
                {
                    case SimpleOperator.Eq:
                        return NHibernate.Criterion.Expression.Eq(propertyName, cse.Values);
                    case SimpleOperator.NotEq:
                        return NHibernate.Criterion.Expression.Not(NHibernate.Criterion.Expression.Eq(propertyName, cse.Values));
                    case SimpleOperator.EqProperty:
                        {
                            if (cse.Values == null)
                            {
                                throw new ArgumentException("EqProperty's Value should not be null!", "cse");
                            }
                            string toPropertyName = TryCreateCriteria(cse.Values.ToString(), ref originalCriteria, aliases);
                            return NHibernate.Criterion.Expression.EqProperty(propertyName, toPropertyName);
                        }
                    case SimpleOperator.Like:
                        {
                            int? propertyLength = NHibernateHelper.GetPropertyLength(sessionFactory, typeof(T), cse.FullPropertyName);
                            return GetLikeCriterion(propertyName, cse.Values, propertyLength);
                        }
                    case SimpleOperator.InG:
                        return NHibernate.Criterion.Expression.In(propertyName, cse.Values as IList);
                    case SimpleOperator.IsNotNull:
                        return NHibernate.Criterion.Expression.IsNotNull(propertyName);
                    case SimpleOperator.IsNull:
                        return NHibernate.Criterion.Expression.IsNull(propertyName);
                    case SimpleOperator.Gt:
                        return NHibernate.Criterion.Expression.Gt(propertyName, cse.Values);
                    case SimpleOperator.Lt:
                        return NHibernate.Criterion.Expression.Lt(propertyName, cse.Values);
                    case SimpleOperator.Ge:
                        return NHibernate.Criterion.Expression.Ge(propertyName, cse.Values);
                    case SimpleOperator.Le:
                        return NHibernate.Criterion.Expression.Le(propertyName, cse.Values);
                    case SimpleOperator.GInG:
                    {
                        int? propertyLength = NHibernateHelper.GetPropertyLength(sessionFactory, typeof(T), cse.FullPropertyName);

                        IList data = cse.Values as IList;
                        // 通过查询控件控制
                        // 当用于客户用途查找类型时，只能Like
                        //if (data.Count <= 1)
                        {
                            NHibernate.Criterion.ICriterion criterion = GetLikeCriterion(propertyName, data[0], propertyLength);
                            for (int i = 1; i < data.Count; ++i)
                            {
                                criterion = NHibernate.Criterion.Expression.Or(criterion, GetLikeCriterion(propertyName, data[i], propertyLength));
                            }
                            return criterion;
                        }
                        //// 当多行时，不在模糊查找
                        //else
                        //{
                        //    return NHibernate.Criterion.Expression.In(propertyName, data);
                        //}
                    }
                    case SimpleOperator.Any:
                        return NHibernate.Criterion.Expression.EqProperty(propertyName, propertyName);
                    case SimpleOperator.Sql:
                        return NHibernate.Criterion.Expression.Sql(cse.FullPropertyName);
                    default:
                        System.Diagnostics.Debug.Assert(false, "Not supported SimpleOperator");
                        return null;
                }
            }
        }

        

        private static NHibernate.Criterion.Order GetOrder(ref ICriteria originalCriteria,
                                                           Dictionary<string, ICriteria> aliases, ISearchOrder findOrder)
        {
            StringBuilder name = new StringBuilder();
            string columnName = findOrder.PropertyName;
            if (columnName.Contains(":"))
            {
                int idx = columnName.LastIndexOf(':');
                string prefixName = columnName.Substring(0, idx);
                columnName = columnName.Substring(idx + 1);
                string[] prefixNames = prefixName.Split(new char[] {':'}, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < prefixNames.Length; ++j)
                {
                    string prefixAlias = prefixNames[j];
                    if (!aliases.ContainsKey(prefixAlias))
                    {
                        string preCriteria;
                        ICriteria criteria;
                        if (j == 0)
                        {
                            preCriteria = "Current";
                        }
                        else
                        {
                            preCriteria = prefixNames[j - 1];
                        }

                        NHibernate.SqlCommand.JoinType joinType;
                        if (j == prefixNames.Length - 1)
                        {
                            joinType = GetJoinType(ref columnName);
                        }
                        else
                        {
                            joinType = GetJoinType(ref prefixNames[j - 1]);
                        }

                        criteria = CreateJoinCriteria(prefixAlias, preCriteria, aliases, joinType);

                        aliases.Add(prefixAlias, criteria);

                        originalCriteria = criteria;
                    }

                    if (j == prefixNames.Length - 1)
                    {
                        name.Append(prefixAlias).Append(".");
                    }
                }
            }
            else
            {
                name.Append("Current").Append(".");
            }


            if (findOrder.Ascending)
            {
                return NHibernate.Criterion.Order.Asc(name.ToString() + columnName);
            }
            else
            {
                return NHibernate.Criterion.Order.Desc(name.ToString() + columnName);
            }
        }

        /// <summary>
        /// 根据查询条件创建Criteria
        /// </summary>
        /// <param name="session"></param>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="hasCollection"></param>
        /// <returns></returns>
        public static NHibernate.ICriteria CreateCriteria(NHibernate.ISession session, ISearchExpression searchExpression,
                                                          IList<ISearchOrder> searchOrders, ref bool hasCollection)
        {
            hasCollection = false;
            int paramCnt = 0;

            NHibernate.ICriteria criteria = session.CreateCriteria(typeof(T), "Current");
            Dictionary<string, ICriteria> aliases = new Dictionary<string, ICriteria>();
            aliases.Add("Current", criteria);

            NHibernate.Criterion.ICriterion criterion = GetCriterion(session.SessionFactory, ref criteria, aliases, searchExpression, ref hasCollection, ref paramCnt);
            if (criterion != null)
            {
                criteria.Add(criterion);
            }
            if (paramCnt > 2100)
            {
                throw new NotSupportedException("您输入的参数过多，系统最多只支持2100个参数！");
            }

            if (searchOrders != null)
            {
                foreach (ISearchOrder so in searchOrders)
                {
                    criteria.AddOrder(GetOrder(ref criteria, aliases, so));
                }
            }

            return aliases["Current"];
        }

        private bool m_hasCollection;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        protected void OnCriteriaCreated(NHibernate.ICriteria criteria)
        {
            if (this.EagerFetchs != null)
            {
                foreach (string s in this.EagerFetchs)
                {
                    criteria = criteria.SetFetchMode(s, FetchMode.Eager);
                }
            }

            if (EnablePage)
            {
                criteria = criteria.SetFirstResult(FirstResult);
                if (MaxResult != -1)
                {
                    criteria = criteria.SetMaxResults(MaxResult);
                }
            }

            if (IsResultDistinct || m_hasCollection)
            {
                criteria = criteria.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            }
        }
        #endregion
        /// <summary>
        /// 根据查询条件读入数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            GetDataCountInternal(searchExpression, searchOrders);
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
                if (this.EnablePage && this.FirstResult == 0 && dataSource.Count < this.MaxResult
                    && !(IsResultDistinct || m_hasCollection))
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
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="func"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, 
            Action<object> func)
        {
            List<T> dataSource = new List<T>();
            int count;

            using (var rep = new Repository(this.RepositoryCfgName))
            //using (var tx = rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                rep.BeginTransaction();
                ActionableList<object> actionList = new ActionableList<object>(dataSource, func);

                GetData(searchExpression, searchOrders, rep.Session, actionList);

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
        /// 根据查询条件查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            using (var rep = new Repository(this.RepositoryCfgName))
            {
                rep.BeginTransaction();
                var ret = GetData(searchExpression, searchOrders, rep.Session);

                rep.CommitTransaction();
                return ret;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public override int GetCount(ISearchExpression searchExpression)
        {
            using (var rep = new Repository(this.RepositoryCfgName))
            {
                rep.BeginTransaction();
                var ret = GetCount(searchExpression, rep.Session);

                rep.CommitTransaction();
                return ret;
            }
        }

        private System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, NHibernate.ISession session)
        {
            NHibernate.ICriteria criteria = CreateCriteria(session, searchExpression, searchOrders, ref m_hasCollection);
            
            OnCriteriaCreated(criteria);

            return criteria.List<T>();
            //return criteria.Future<T>();
        }

        private void GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders,
            NHibernate.ISession session, ActionableList<object> actionList)
        {
            NHibernate.ICriteria criteria = CreateCriteria(session, searchExpression, searchOrders, ref m_hasCollection);

            OnCriteriaCreated(criteria);

            criteria.List(actionList);
        }

        private int GetCount(ISearchExpression searchExpression, NHibernate.ISession session)
        {
            NHibernate.ICriteria criteria2 = CreateCriteria(session, searchExpression, null, ref m_hasCollection);
            criteria2.SetProjection(NHibernate.Criterion.Projections.RowCount());
            return (int)criteria2.UniqueResult(); // criteria2.FutureValue<int>().Value;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerCriteria<T> sm = new SearchManagerCriteria<T>();
            Copy(this, sm);
            return sm;
        }
    }

    ///// <summary>
    ///// CriteriaEventArgs
    ///// </summary>
    //public class CriteriaEventArgs : EventArgs
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="criteria"></param>
    //    public CriteriaEventArgs(ICriteria criteria)
    //    {
    //        this.Criteria = criteria;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public ICriteria Criteria { get; private set; }
    //}
}