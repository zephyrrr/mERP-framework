using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRepositoryDao
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRepository GenerateRepository();
    }

    /// <summary>
    /// 带Repository的Dao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryDao<T> : IRepositoryDao, IEventDao<T>
        where T : class, IEntity
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="rep"></param>
        void Save(IRepository rep, T entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="rep"></param>
        void Update(IRepository rep, T entity);

        /// <summary>
        /// SaveOrUpdate
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="rep"></param>
        void SaveOrUpdate(IRepository rep, T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="rep"></param>
        void Delete(IRepository rep, T entity);
    }
}
