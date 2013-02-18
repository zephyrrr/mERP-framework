using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    ///// <summary>
    /////  OnetoMany关系的Dao，Many自己操作
    ///// 类似于票-箱的主从关系，需要各种新增或者修改删除子记录
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <typeparam name="S"></typeparam>
    //public class MasterDao<T, S> : MasterDao<T, S, T, S>
    //    where T : class, IMasterEntity<T, S>
    //    where S : class, IDetailEntity<T, S>
    //{
    //    /// <summary>
    //    /// Constructor
    //    /// </summary>
    //    /// <param name="detailDao"></param>
    //    public MasterDao(IRepositioryDao<S> detailDao)
    //        : base(detailDao)
    //    {
    //    }
    //}

    internal static class RepositoryLockHelper
    {
        public static void TryLock(IRepository rep, IEntity entity)
        {
            //Feng.NH.INHibernateRepository r = rep as Feng.NH.INHibernateRepository;
            //if (r != null)
            //{
            //    // 会导致不能保存joined-subclass， 例如内贸出港备案，如果Lock以后，只会保存普通箱
            //    r.Lock(entity, NHibernate.LockMode.None);
            //}
        }
    }

    /// <summary>
    /// OnetoMany关系的Dao，Many自己操作
    /// 类似于票-箱的主从关系，需要各种新增或者修改删除子记录
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    ///// <typeparam name="BT"></typeparam>
    ///// <typeparam name="BS"></typeparam>
    //public class MasterDao<T, S, BT, BS> : AbstractMemoriedMasterDao<T, S>
    //    where T : class, IMasterEntity<T, S>, BT
    //    where S : class, IDetailEntity<T, S>, BS
    //    where BT : class, IMasterEntity<BT, BS>
    //    where BS : class, IDetailEntity<BT, BS>
    public class MasterDao<T, S> : AbstractMemoriedRelationalDao<T, S>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        public override void AddRelationToMemoryDao(IEntityList cm)
        {
            this.DetailMemoryDao.AddRelationalDao(new MasterDetailMemoryDao<T, S>(cm));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public MasterDao(IRepositoryDao<S> detailDao)
            : base(detailDao)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBegun(OperateArgs<T> e)
        {
            base.OnTransactionBegun(e);

            switch (e.OperateType)
            {
                case OperateType.Update:
                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
                        RepositoryLockHelper.TryLock(e.Repository, i);
                    }
                    break;
            }
        }

        /// <summary>
        /// 在Entity操作前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperating(OperateArgs<T> e)
        {
            base.OnEntityOperating(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    // Add it to master entity
                    if (e.Entity.DetailEntities == null)
                    {
                        e.Entity.DetailEntities = new List<S>();
                    }
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        e.Entity.DetailEntities.Add(i);
                    }
                    break;
                case OperateType.Update:
                    // add and remove it in master entity
                    e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);

                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        e.Entity.DetailEntities.Add(i);
                    }
                    foreach (S i in this.DetailMemoryDao.DeletedEntities)
                    {
                        e.Entity.DetailEntities.Remove(i);
                    }
                    break;
                case OperateType.Delete:
                    e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
                    if (e.Entity.DetailEntities != null)
                    {
                        foreach (S i in e.Entity.DetailEntities)
                        {
                            System.Diagnostics.Debug.Assert(i is S, i + "should be " + typeof(S) + "type");
                            this.DetailDao.Delete(e.Repository, i as S);
                        }
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }

        /// <summary>
        /// 在Entity操作后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperated(OperateArgs<T> e)
        {
            base.OnEntityOperated(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        i.MasterEntity = e.Entity;
                        this.DetailDao.Save(e.Repository, i);
                    }
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.DeletedEntities.Count == 0);
                    break;
                case OperateType.Update:
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        i.MasterEntity = e.Entity;
                        this.DetailDao.Save(e.Repository, i);
                    }
                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
                        this.DetailDao.Update(e.Repository, i);
                    }
                    foreach (S i in this.DetailMemoryDao.DeletedEntities)
                    {
                        this.DetailDao.Delete(e.Repository, i);
                    }
                    break;
                case OperateType.Delete:
                    
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }

        

        /// <summary>
        /// 在Transaction Rollback后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacked(OperateArgs<T> e)
        {
            base.OnTransactionRollbacked(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    if (e.Entity.DetailEntities != null)
                    {
                        foreach (S i in this.DetailMemoryDao.SavedEntities)
                        {
                            e.Entity.DetailEntities.Remove(i);
                        }
                    }
                    break;
                case OperateType.Update:
                    // add and remove it in master entity
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        e.Entity.DetailEntities.Remove(i);
                    }
                    foreach (S i in this.DetailMemoryDao.DeletedEntities)
                    {
                        e.Entity.DetailEntities.Add(i);
                    }
                    break;
                case OperateType.Delete:
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }
    }
}
