using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Repository接口
    /// </summary>
    public interface IRepository : IDisposable
    {
        #region "Operation"
        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="entity"></param>
        void Save(object entity);

        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="entity"></param>
        void Update(object entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(object entity);

        /// <summary>
        /// 新增或修改保存
        /// </summary>
        /// <param name="entity"></param>
        void SaveOrUpdate(object entity);

        /// <summary>
        /// 开始Transaction
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();

        /// <summary>
        /// 提交Transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚Transaction
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 是否支持事务
        /// </summary>
        bool IsSupportTransaction
        {
            get;
        }
        #endregion

        /// <summary>
        /// Tag, 用于用户数据
        /// </summary>
        object Tag
        {
            get;
            set;
        }

        #region "like nh"
        ///// <summary>
        ///// 是否是Proxy
        ///// </summary>
        ///// <param name="proxy"></param>
        ///// <returns></returns>
        //bool IsProxy(object proxy);

        /// <summary>
        ///  读取 LazyLoad Proxy
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="owner"></param>
        void Initialize(object proxy, object owner);

        /// <summary>
        /// 相当于NHibernate中的Lock
        /// </summary>
        /// <param name="obj"></param>
        void Attach(object obj);

        /// <summary>
        /// Refresh
        /// </summary>
        /// <param name="obj"></param>
        void Refresh(object obj);

        #endregion

        ///// <summary>
        ///// 根据Id得到实体
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //T Get<T>(object id);

        /// <summary>
        /// 根据Id得到实体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        object Get(Type type, object id);

        /// <summary>
        /// 得到全部列表
        /// </summary>
        /// <param name="persistantClass"></param>
        /// <returns></returns>
        System.Collections.IList List(Type persistantClass);

        /// <summary>
        /// 按照查询语句查找
        /// 在（NHibernate中，按照HqlQuery）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<T> List<T>(string queryString, Dictionary<string, object> parameters = null)
            where T : class, new();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="queryString"></param>
        ///// <param name="parameters"></param>
        ///// <returns></returns>
        //IList<T> List<T>(string queryString, params object[] parameters)
        //    where T : class;
    }

    /// <summary>
    /// 
    /// </summary>
    public static class RepositoryExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rep"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Get<T>(this IRepository rep, object id)
        {
            return (T)rep.Get(typeof(T), id);
        }

        /// <summary>
        /// 得到全部列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> List<T>(this IRepository rep)
            where T : class, new()
        {
            return rep.List<T>(null, null);
        }   
    }
}
