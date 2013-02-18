using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 针对SubmittedEntity的Dao
    /// </summary>
    public interface ISubmittedEntityDao : IRepositoryDao
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        void Submit(IRepository rep, object entity);

        /// <summary>
        /// 撤销提交
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        void Unsubmit(IRepository rep, object entity);
    }

    /// <summary>
    /// 针对SubmittedEntity的Dao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISubmittedEntityDao<T> : IBaseDao<T>
        where T: class, ISubmittedEntity
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        void Submit(IRepository rep, T entity);

        /// <summary>
        /// 撤销提交
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        void Unsubmit(IRepository rep, T entity);
    }
}
