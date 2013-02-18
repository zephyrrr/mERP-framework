using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 类似于对账单-费用的关系，只是更新主从关系，而不是新增或者修改删除子记录
    /// 与NHibernateMasterNoRelationDao的不同是，当删除主记录时，要更新子记录的MasterEntity属性为null
    /// 和MasterDao2的区别是，不需在外部设置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MasterDao22<T, S> : AbstractMemoriedRelationalDao<T, S>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public MasterDao22(IRepositoryDao<S> detailDao)
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
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
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
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);

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
                    if (e.Entity.DetailEntities != null)
                    {
                        e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
                        foreach (S i in e.Entity.DetailEntities)
                        {
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
                        this.DetailDao.Update(e.Repository, i);
                    }
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.DeletedEntities.Count == 0);
                    break;
                case OperateType.Update:
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        i.MasterEntity = e.Entity;
                        this.DetailDao.Update(e.Repository, i);
                    }
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);
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