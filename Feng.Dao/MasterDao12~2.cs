using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    ///// <summary>
    ///// OnetoMany关系的Dao，Many自己操作
    ///// 类似于票-箱的主从关系，需要各种新增或者修改删除子记录.
    ///// 不同的是，Save的可能是已有的关系，类似于MasterDao和MasterDao2的集合。
    ///// 删除时，只是清空关系，而不是删除DetailEntity
    ///// 也就是，票里可以添加箱，也可以把已有的箱包进来。
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <typeparam name="S"></typeparam>
    //public class MasterDao12<T, S> : MasterDao12<T, S, T, S>
    //    where T : class, IMasterEntity<T, S>
    //    where S : class, IDetailEntity<T, S>
    //{
    //    /// <summary>
    //    /// Constructor
    //    /// </summary>
    //    /// <param name="detailDao"></param>
    //    public MasterDao12(IRepositioryDao<S> detailDao)
    //        : base(detailDao)
    //    {
    //    }
    //}

    /// <summary>
    /// OnetoMany关系的Dao，Many自己操作
    /// 类似于票-箱的主从关系，需要各种新增或者修改删除子记录.
    /// 不同的是，Save的可能是已有的关系，类似于MasterDao和MasterDao2的集合。
    /// 删除时，只是清空关系，而不是删除DetailEntity
    /// 也就是，票里可以添加箱，也可以把已有的箱包进来。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    //public class MasterDao12<T, S, BT, BS> : AbstractMemoriedMasterDao<T, S>
    //    where T : class, IMasterEntity<T, S>, BT
    //    where S : class, IDetailEntity<T, S>, BS
        //where BT : class, IMasterEntity<BT, BS>
        //where BS : class, IDetailEntity<BT, BS>
    public class MasterDao12<T, S> : AbstractMemoriedRelationalDao<T, S>
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
        public MasterDao12(IRepositoryDao<S> detailDao)
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
                    foreach (S i in this.DetailMemoryDao.DeletedEntities)
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
                            //System.Diagnostics.Debug.Assert(i is S, i + "should be " + typeof(S) + "type");
                            i.MasterEntity = null;
                            this.DetailDao.Update(e.Repository, i);
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
                        this.DetailDao.SaveOrUpdate(e.Repository, i);
                    }
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.DeletedEntities.Count == 0);
                    break;
                case OperateType.Update:
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        i.MasterEntity = e.Entity;
                        this.DetailDao.SaveOrUpdate(e.Repository, i);
                    }
                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
                        this.DetailDao.Update(e.Repository, i);
                    }
                    foreach (S i in this.DetailMemoryDao.DeletedEntities)
                    {
                        i.MasterEntity = null;
                        this.DetailDao.Update(e.Repository, i);
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
