using System;
using System.Reflection;
using System.Text;
using System.Configuration;
using System.Collections.Generic;

namespace Feng.Utils
{
    /// <summary>
    /// Helper for Repository
    /// </summary>
    public sealed class RepositoryHelper
    {
        private static Dictionary<string, object> GetRepositoryTag(IRepository rep)
        {
            Dictionary<string, object> dict;
            if (rep.Tag == null)
            {
                dict = new Dictionary<string, object>();
                rep.Tag = dict;
            }
            else
            {
                dict = rep.Tag as Dictionary<string, object>;
            }
            if (dict == null)
            {
                throw new ArgumentException("Rep's Tag is used as another format!");
            }
            return dict;
        }

        /// <summary>
        /// 获得Repository中的Tag中保存的数据
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetRepositoryTagData(IRepository rep, string key)
        {
            Dictionary<string, object> dict = GetRepositoryTag(rep);

            if (!dict.ContainsKey(key))
            {
                return null;
            }

            return dict[key];
        }

        /// <summary>
        /// 设置Repository中的Tag中保存的数据
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetRepositoryTagData(IRepository rep, string key, object value)
        {
            Dictionary<string, object> dict = GetRepositoryTag(rep);
            dict[key] = value;
        }

        /// <summary>
        /// 获得Repository中的Tag中保存的用于生成主键的第一次值（主键取最大时）
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string GetRepositoryPk(IRepository rep, string typeName)
        {
            string key = typeName + ":DbPK";

            string keyValue = (string)GetRepositoryTagData(rep, key);

            return keyValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="typeName"></param>
        /// <param name="keyValue"></param>
        public static void SetRepositoryPk(IRepository rep, string typeName, string keyValue)
        {
            string key = typeName + ":DbPK";
            SetRepositoryTagData(rep, key, keyValue);
        }

        /// <summary>
        /// 获得Repository中的Tag中保存的用于生成主键的Delta递增值（主键取最大，同时插入几个记录时）
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static int GetRepositoryDelta(IRepository rep, string typeName)
        {
            string key = typeName + ":DbPKDelta";

            int? delta = (int?)GetRepositoryTagData(rep, key);
            if (!delta.HasValue)
            {
                delta = 0;
            }
            SetRepositoryTagData(rep, key, delta + 1);

            return delta.Value;
        }

        /// <summary>
        /// 从Type得到默认RepositoryName
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetConfigNameFromType(Type type)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(type, typeof(RepositoryConfigNameAttribute));
            if (attrs.Length > 0)
            {
                RepositoryConfigNameAttribute attr = attrs[0] as RepositoryConfigNameAttribute;
                return attr.ConfigName;
            }
            else
            {
                string s = type.AssemblyQualifiedName;
                int idx = s.IndexOf(',');
                int idx2 = s.IndexOf(',', idx + 1);
                s = s.Substring(idx + 1, idx2 - idx - 1).Trim().ToLower();
                s += ".config";

                return s;
            }
        }

        ///// <summary>
        ///// 读取 LazyLoad Proxy
        ///// </summary>
        ///// <param name="proxy"></param>
        ///// <param name="owner"></param>
        //public static void Initialize(object proxy, object owner)
        //{
        //    using (var rep = RepositoryFactory.GenerateRepository(owner.GetType()))
        //    {
        //        rep.Initialize(proxy, owner);
        //    }
        //}

        ///// <summary>
        ///// 根据属性得到实体
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="property"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static T GetByProperty<T>(string property, object value)
        //    where T: IEntity
        //{
        //    using (var rep = RepositoryFactory.GenerateRepository())
        //    {
        //        return rep.GetByProperty<T>(property, value);
        //    }
        //}

        ///// <summary>
        ///// 根据Id得到实体
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public static T Get<T>(object id)
        //    where T : IEntity
        //{
        //    using (var rep = RepositoryFactory.GenerateRepository<T>())
        //    {
        //        return rep.Get<T>(id);
        //    }
        //}

        //#region Static Methods of Dao
        ///// <summary>
        ///// 按照类型生成Repository
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public static NH.Repository CreateRepository<T>()
        //    where T : IEntity
        //{
        //    return new NH.Repository(GetConfigNameFromType(typeof(T)));
        //}

        ///// <summary>
        ///// Retrieves an entity from the data store.
        ///// </summary>
        ///// <typeparam name="T">The type of entity to retrieve.</typeparam>
        ///// <param name="id">The ID of the entity.</param>
        ///// <returns>An entity of type T.</returns>
        //public static T Get<T>(object id)
        //    where T : IEntity
        //{
        //    using (var rep = CreateRepository<T>())
        //    using (var tx = rep.BeginTransaction())
        //    {
        //        var ret = rep.Get<T>(id);

        //        rep.CommitTransaction();
        //        return ret;
        //    }
        //}

        /////// <summary>
        /////// Retrieves an entity based on the name and value of a property.
        /////// </summary>
        /////// <typeparam name="T">The type of entity to retrieve.</typeparam>
        /////// <param name="property">The name of the property; should be a member of type T.</param>
        /////// <param name="value">The value of the property.</param>
        /////// <returns>An entity of type T.</returns>
        ////public static T GetByProperty<T>(string property, object value)
        ////    where T : IEntity
        ////{
        ////    using (var rep = CreateRepository<T>())
        ////    using (var tx = rep.BeginTransaction())
        ////    {
        ////        StringBuilder hql = new StringBuilder();
        ////        hql.Append(string.Format("FROM {0} a ", typeof(T).FullName));
        ////        hql.Append(string.Format("WHERE a.{0} = ?", property));

        ////        object obj = rep.Session.CreateQuery(hql.ToString())
        ////            .SetParameter(0, value)
        ////            .UniqueResult();

        ////        rep.CommitTransaction();
        ////        return (T)obj;
        ////    }
        ////}

        ///// <summary>
        ///// Retrieves the value of a property from an entity.
        ///// </summary>
        ///// <typeparam name="T">The type of entity that contains the property.</typeparam>
        ///// <typeparam name="R">The return type of the property value.</typeparam>
        ///// <param name="property">The name of the property to retrieve.</param>
        ///// <param name="idName">The name of the ID property of the entity.</param>
        ///// <param name="idValue">The value of the ID property of the entity.</param>
        ///// <returns>The value of the property, which is of type R.</returns>
        //public static R GetPropertyValue<T, R>(string property, string idName, object idValue)
        //    where T : IEntity
        //{
        //    using (var rep = CreateRepository<T>())
        //    using (var tx = rep.BeginTransaction())
        //    {
        //        StringBuilder hql = new StringBuilder();
        //        hql.Append(string.Format("SELECT a.{0} ", property));
        //        hql.Append(string.Format("FROM {0} a ", typeof(T).FullName));
        //        hql.Append(string.Format("WHERE a.{0} = ?", idName));

        //        object obj = rep.Session.CreateQuery(hql.ToString())
        //            .SetParameter(0, idValue)
        //            .UniqueResult();

        //        rep.CommitTransaction();
        //        return (R)obj;
        //    }
        //}

        ///// <summary>
        ///// Retrieves a collection of entities based on the supplied HQL query.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="hql">The HQL query.</param>
        ///// <returns>A collection of entities of type T, as a result from the HQL query.</returns>
        //public static IList<T> Find<T>(string hql)
        //    where T : IEntity
        //{
        //    using (var rep = CreateRepository<T>())
        //    using (var tx = rep.BeginTransaction())
        //    {
        //        //IList list = rep.Session.Find(hql);   // Deprecated with NHibernate 1.2
        //        IList<T> list = rep.Session.CreateQuery(hql).List<T>();
        //        rep.CommitTransaction();
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// Retrieves a collection of entities based on the supplied HQL query.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="hql">The HQL query.</param>
        ///// <param name="value">A value to be bound to a "?" placeholder in the HQL query.</param>
        ///// <param name="type">An NHibernate type of the value.</param>
        ///// <returns>A collection of entities of type T, as a result from the HQL query.</returns>
        //public static IList<T> Find<T>(string hql, object value, IType type)
        //    where T : IEntity
        //{
        //    using (var rep = CreateRepository<T>())
        //    using (var tx = rep.BeginTransaction())
        //    {
        //        IList<T> list = rep.Session.CreateQuery(hql).SetParameter(0, value, type).List<T>();
        //        rep.CommitTransaction();
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// Retrieves a collection of entities based on the supplied HQL query.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="hql">The HQL query.</param>
        ///// <param name="values">An array of values to be bound to the "?" placeholders in the HQL query.</param>
        ///// <param name="types">An array of NHibernate types of the values.</param>
        ///// <returns>A collection of entities of type T, as a result from the HQL query.</returns>
        //public static IList<T> Find<T>(string hql, object[] values, IType[] types)
        //    where T : IEntity
        //{
        //    using (var rep = CreateRepository<T>())
        //    using (var tx = rep.BeginTransaction())
        //    {
        //        IQuery query = rep.Session.CreateQuery(hql);
        //        for (int i = 0; i < values.Length; ++i)
        //        {
        //            query.SetParameter(i, values[i], types[i]);
        //        }
        //        IList<T> list = query.List<T>();
        //        rep.CommitTransaction();
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// Retrieves a collection of entities based on the name and value of a property.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="property">The name of the property; should be a member of type T.</param>
        ///// <param name="value">The value of the property.</param>
        ///// <returns>A collection of entities of type T.</returns>
        //public static IList<T> FindByProperty<T>(string property, object value)
        //    where T : IEntity
        //{
        //    return FindByProperty<T>(property, value, null);
        //}

        ///// <summary>
        ///// Retrieves an ordered collection of entities based on the name and value of a property.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="property">The name of the property.</param>
        ///// <param name="value">The value of the property.</param>
        ///// <param name="orderBy">The OrderBy object to use; contains the property to order by and
        ///// the sort direction.</param>
        ///// <returns>An ordered collection of entities of type T.</returns>
        //public static IList<T> FindByProperty<T>(string property, object value, ISearchOrder orderBy)
        //    where T : IEntity
        //{
        //    // Pass -1 to the overload, which means bring back everything. This is consistent
        //    // with NHibernate implementation.
        //    return FindByProperty<T>(property, value, orderBy, -1);
        //}

        ///// <summary>
        ///// Retrieves a maximum number of an ordered collection of entities based on the name and
        ///// value of a property.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="property">The name of the property.</param>
        ///// <param name="value">The value of the property.</param>
        ///// <param name="orderBy">The OrderBy object to use; contains the property to order by and
        ///// the sort direction.</param>
        ///// <param name="maxResults">The maximum number of entities to return in the collection.</param>
        ///// <returns>An ordered collection of entities of type T.</returns>
        //public static IList<T> FindByProperty<T>(string property, object value, ISearchOrder orderBy, int maxResults)
        //    where T : IEntity
        //{
        //    using (var rep = CreateRepository<T>())
        //    using (var tx = rep.BeginTransaction())
        //    {
        //        NHibernate.Criterion.Order nhibOrder = null;
        //        IList<T> list = null;
        //        ICriteria criteria = null;

        //        // Check to create NHibernate Order object
        //        if (orderBy != null)
        //        {
        //            // Default the order to ascending
        //            nhibOrder = new NHibernate.Criterion.Order(orderBy.PropertyName, true);

        //            // Check to switch to descending
        //            if (!orderBy.Ascending)
        //            {
        //                nhibOrder = new NHibernate.Criterion.Order(orderBy.PropertyName, false);
        //            }
        //        }

        //        criteria = rep.Session.CreateCriteria(typeof(T));
        //        if (value == null)
        //        {
        //            criteria = criteria.Add(Expression.IsNull(property));
        //        }
        //        else
        //        {
        //            criteria = criteria.Add(Expression.Eq(property, value));
        //        }
        //        // Check to add NHibernate Order to criteria
        //        if (nhibOrder != null)
        //        {
        //            criteria = criteria.AddOrder(nhibOrder);
        //        }

        //        // Check to use SetMaxResults or not
        //        if (maxResults == -1)
        //        {
        //            // Do not use SetMaxResults
        //            list = criteria.List<T>();
        //        }
        //        else
        //        {
        //            list = criteria.SetMaxResults(maxResults).List<T>();
        //        }

        //        rep.CommitTransaction();
        //        return list;
        //    }
        //}

        ///// <summary>
        ///// Retrieves a collection of all entities of a given type.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <returns>A collection of all entities of type T.</returns>
        //public static IList<T> FindAll<T>()
        //    where T : IEntity
        //{
        //    return FindAll<T>(null);
        //}

        ///// <summary>
        ///// Retrieves an ordered collection of all entities of a given type.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="orderBy">The OrderBy object to use; contains the property to order by
        ///// and the sort direction.</param>
        ///// <returns>An ordered collection of all entities of type T.</returns>
        //public static IList<T> FindAll<T>(ISearchOrder orderBy)
        //    where T : IEntity
        //{
        //    // Pass -1 to the overload, which means bring back everything. This is consistent
        //    // with NHibernate implementation.
        //    return FindAll<T>(orderBy, -1);
        //}

        ///// <summary>
        ///// Retrieves a maximum number of an ordered collection of all entities of a given type.
        ///// </summary>
        ///// <typeparam name="T">The type of entities to retrieve.</typeparam>
        ///// <param name="orderBy">The OrderBy object to use; contains the property to order by
        ///// and the sort direction.</param>
        ///// <param name="maxResults">The maximum number of entities to return in the collection.</param>
        ///// <returns>An ordered collection of all entities of type T.</returns>
        //public static IList<T> FindAll<T>(ISearchOrder orderBy, int maxResults)
        //    where T : IEntity
        //{
        //    using (var rep = CreateRepository<T>())
        //    using (var tx = rep.BeginTransaction())
        //    {
        //        NHibernate.Criterion.Order nhibOrder = null;
        //        IList<T> list = null;
        //        ICriteria criteria = null;

        //        // Check to create NHibernate Order
        //        if (orderBy != null)
        //        {
        //            // Default the order to ascending
        //            nhibOrder = new NHibernate.Criterion.Order(orderBy.PropertyName, true);

        //            // Check to switch to descending
        //            if (!orderBy.Ascending)
        //            {
        //                nhibOrder = new NHibernate.Criterion.Order(orderBy.PropertyName, false);
        //            }
        //        }

        //        // Check to add NHibernate Order to criteria
        //        if (nhibOrder != null)
        //        {
        //            criteria = rep.Session.CreateCriteria(typeof(T)).AddOrder(nhibOrder);
        //        }
        //        else
        //        {
        //            criteria = rep.Session.CreateCriteria(typeof(T));
        //        }

        //        // Check to use SetMaxResults
        //        if (maxResults == -1)
        //        {
        //            // Do not use SetMaxResults
        //            list = criteria.List<T>();
        //        }
        //        else
        //        {
        //            list = criteria.SetMaxResults(maxResults).List<T>();
        //        }
        //        rep.CommitTransaction();
        //        return list;
        //    }
        //}

        ///////// <summary>
        ///////// 按照Criterion查询
        ///////// </summary>
        ///////// <param name="criterions"></param>
        ///////// <returns></returns>
        //////public static IList<T> FindByCriterions<T>(IList<NHibernate.Criterion.ICriterion> criterions)
        //////{
        //////    using (var rep = RepositoryFactory.GenerateRepository<T>())
        //////    {
        //////        IList<T> list = null;
        //////        ICriteria criteria = rep.Session.CreateCriteria(typeof(T));
        //////        foreach (ICriterion cri in criterions)
        //////            criteria.Add(cri);

        //////        list = criteria.List<T>();

        //////        return list;
        //////    }
        //////}

        ///////// <summary>
        ///////// 得到Query用的DetachedCriteria
        ///////// </summary>
        ///////// <returns></returns>
        //////public static NHibernate.Criterion.DetachedCriteria GetDetachedCriteria<T>()
        //////{
        //////    return NHibernate.Criterion.DetachedCriteria.For<T>();
        //////}

        ///////// <summary>
        ///////// Load by DetachedCriteria
        ///////// </summary>
        ///////// <param name="detachedCriteria"></param>
        ///////// <returns></returns>
        //////public static IList<T> FindByDetachedCriteria<T>(NHibernate.Criterion.DetachedCriteria detachedCriteria)
        //////{
        //////    return FindByDetachedCriteria<T>(detachedCriteria, -1);
        //////}

        ///////// <summary>
        ///////// Load by DetachedCriteria
        ///////// </summary>
        ///////// <param name="detachedCriteria"></param>
        ///////// <param name="maxResult"></param>
        ///////// <returns></returns>
        //////public static IList<T> FindByDetachedCriteria<T>(NHibernate.Criterion.DetachedCriteria detachedCriteria, int maxResult)
        //////{
        //////    return FindByDelegate<T>(new FindDelegate<T>(delegate(ISession session)
        //////    {
        //////        if (maxResult == -1)
        //////            return detachedCriteria.GetExecutableCriteria(session).List<T>();
        //////        else
        //////            return detachedCriteria.GetExecutableCriteria(session).SetMaxResults(maxResult).List<T>();
        //////    }
        //////    ));
        //////}


        ///////// <summary>
        ///////// 用给定Criteria查询
        ///////// </summary>
        ///////// <param name="criteria"></param>
        ///////// <returns></returns>
        //////public static IList<T> FindByCriteria<T>(ICriteria criteria)
        //////{
        //////    return criteria.List<T>();
        //////}

        ///////// <summary>
        ///////// Submit user's operation
        ///////// </summary>
        ///////// <param name="session"></param>
        //////public delegate IList<T> FindDelegate<T>(ISession session);

        ///////// <summary>
        ///////// 根据Delegate查询
        ///////// </summary>
        ///////// <param name="findDelegate"></param>
        ///////// <returns></returns>
        //////public static IList<T> FindByDelegate<T>(FindDelegate<T> findDelegate)
        //////{
        //////    using (var rep = RepositoryFactory.GenerateRepository<T>())
        //////    {
        //////        return findDelegate(rep.Session);
        //////    }
        //////}

        //#endregion "Public"
    }
}
