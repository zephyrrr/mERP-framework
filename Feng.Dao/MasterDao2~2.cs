using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 类似于对账单-费用的关系，只是更新主从关系，而不是新增或者修改删除子记录
    /// 与NHibernateMasterNoRelationDao的不同是，当删除主记录时，要更新子记录的MasterEntity属性为null
    /// 需在外部设置DetailEntity.MasterEntity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    [Obsolete("Too complex, just try use MasterDao22")]
    public class MasterDao2<T, S> : AbstractMemoriedRelationalDao<T, S>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public MasterDao2(IRepositoryDao<S> detailDao)
            : base(detailDao, new MemoryDao2<T, S>())
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
                    if (e.Entity.DetailEntities == null)
                    {
                        e.Entity.DetailEntities = new List<S>();
                    }
                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
                        if (i.MasterEntity != null)
                        {
                            e.Entity.DetailEntities.Add(i);
                        }
                        else
                        {
                            e.Entity.DetailEntities.Remove(i);
                        }
                    }
                    break;
                case OperateType.Update:
                   
                    e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);

                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
                        if (i.MasterEntity != null)
                        {
                            e.Entity.DetailEntities.Add(i);
                        }
                        else
                        {
                            e.Entity.DetailEntities.Remove(i);
                        }
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

            System.Diagnostics.Debug.Assert(this.DetailMemoryDao.SavedEntities.Count == 0);
            System.Diagnostics.Debug.Assert(this.DetailMemoryDao.DeletedEntities.Count == 0);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
                        //i.MasterEntity = e.Entity;    // 不能在这里设置，有些为e.Entity，有些为null，需要在外面设置
                        this.DetailDao.Update(e.Repository, i);
                    }
                    break;
                case OperateType.Update:
                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
                        //i.MasterEntity = e.Entity;
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
                case OperateType.Update:
                    if (e.Entity.DetailEntities != null)
                    {
                        e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
                        foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                        {
                            if (i.MasterEntity != null)
                            {
                                e.Entity.DetailEntities.Remove(i);
                            }
                            else
                            {
                                e.Entity.DetailEntities.Add(i);
                            }
                        }
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
