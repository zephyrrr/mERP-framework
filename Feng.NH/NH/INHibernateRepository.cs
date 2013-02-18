using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// Repository接口
    /// </summary>
    public interface INHibernateRepository : IRepository
    {
        ///// <summary>
        ///// 开始Transaction
        ///// </summary>
        ///// <returns></returns>
        //NHibernate.ITransaction BeginTransaction();

        /// <summary>
        /// 开始Transaction
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        void BeginTransaction(System.Data.IsolationLevel isolationLevel);

        /// <summary>
        /// Session
        /// </summary>
        NHibernate.ISession Session
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lockMode"></param>
        void Lock(object entity, NHibernate.LockMode lockMode);


        /// <summary>
        /// 根据DetachedCriteria得到列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="detachedCriteria"></param>
        /// <returns></returns>
        IList<T> List<T>(NHibernate.Criterion.DetachedCriteria detachedCriteria)
            where T : class, new();

        /// <summary>
        /// 根据DetachedCriteria唯一元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="detachedCriteria"></param>
        /// <returns></returns>
        T UniqueResult<T>(NHibernate.Criterion.DetachedCriteria detachedCriteria)
            where T : class, new();
    }
}
