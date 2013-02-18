using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    ///// <summary>
    ///// 基本关系Dao，可包含其他Dao
    ///// 不行，T可能不一致
    ///// </summary>
    //public interface IRelationalDao<T> : IBaseDao<T>
    //    where T: class, IEntity
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="idx"></param>
    //    /// <returns></returns>
    //    IEventDao<T> GetRelationalDao(int idx);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="subDao"></param>
    //    void AddRelationalDao(IEventDao<T> subDao);
    //}

    /// <summary>
    /// 基本关系Dao，可包含其他Dao，可清理SubDaos功能，生成IRepository功能
    /// </summary>
    public interface IRelationalDao : IBaseDao
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        IEventDao GetRelationalDao(int idx);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subDao"></param>
        void AddRelationalDao(IEventDao subDao);
    }
}
