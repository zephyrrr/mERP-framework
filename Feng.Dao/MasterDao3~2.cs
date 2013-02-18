using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 类似于凭证费用明细-费用的关系，只是更新主从关系，而不是新增或者修改删除子记录
    /// 与对帐单-费用不同的是，此处不用MemoryDao，直接保存在Master的DetailEntities中（因为没有二级MemoryDao）
    /// 与NHibernateMasterNoRelationDao的不同是，当删除主记录时，要更新子记录的MasterEntity属性为null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MasterDao3<T, S> : AbstractRelationalDao<T, S>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public MasterDao3(IRepositoryDao<S> detailDao)
            : base(detailDao)
        {
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBeginning(OperateArgs<T> e)
        {
            base.OnTransactionBeginning(e);

            //e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
            //if (e.Entity.DetailEntities != null)
            //{
            //    foreach (S i in e.Entity.DetailEntities)
            //    {
            //        this.DetailDao.OnTransactionBeginning(new OperateArgs<S>(e.Repository, OperateType.Update, i));
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBegun(OperateArgs<T> e)
        {
            base.OnTransactionBegun(e);

            e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
            if (e.Entity.DetailEntities != null)
            {
                foreach (S i in e.Entity.DetailEntities)
                {
                    this.DetailDao.OnTransactionBegun(new OperateArgs<S>(e.Repository, OperateType.Update, i));
                }
            }
        }

        /// <summary>
        /// 在Entity操作前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperating(OperateArgs<T> e)
        {
            switch (e.OperateType)
            {
                case OperateType.Save:
                    break;
                case OperateType.Update:
                    break;
                case OperateType.Delete:
                    e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
                    if (e.Entity.DetailEntities != null)
                    {
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
            switch (e.OperateType)
            {
                case OperateType.Save:
                    e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
                    if (e.Entity.DetailEntities != null)
                    {
                        foreach (S i in e.Entity.DetailEntities)
                        {
                            i.MasterEntity = e.Entity;
                            this.DetailDao.Update(e.Repository, i);
                        }
                    }
                    break;
                case OperateType.Update:
                    break;
                case OperateType.Delete:
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }

        /// <summary>
        /// 在Transaction Commited后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommitting(OperateArgs<T> e)
        {
            base.OnTransactionCommitting(e);

            e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
            if (e.Entity.DetailEntities != null)
            {
                foreach (S i in e.Entity.DetailEntities)
                {
                    this.DetailDao.OnTransactionCommitting(new OperateArgs<S>(e.Repository, OperateType.Update, i));
                }
            }
        }

        /// <summary>
        /// 在Transaction Commited后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommited(OperateArgs<T> e)
        {
            base.OnTransactionCommited(e);

            e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
            if (e.Entity.DetailEntities != null)
            {
                foreach (S i in e.Entity.DetailEntities)
                {
                    this.DetailDao.OnTransactionCommited(new OperateArgs<S>(e.Repository, OperateType.Update, i));
                }
            }
        }

        /// <summary>
        /// 在Transaction Rollback后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacking(OperateArgs<T> e)
        {
            base.OnTransactionRollbacking(e);

            e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
            if (e.Entity.DetailEntities != null)
            {
                foreach (S i in e.Entity.DetailEntities)
                {
                    this.DetailDao.OnTransactionRollbacking(new OperateArgs<S>(e.Repository, OperateType.Update, i));
                }
            }
        }

        /// <summary>
        /// 在Transaction Rollback后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacked(OperateArgs<T> e)
        {
            base.OnTransactionRollbacked(e);

            e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
            if (e.Entity.DetailEntities != null)
            {
                foreach (S i in e.Entity.DetailEntities)
                {
                    this.DetailDao.OnTransactionRollbacked(new OperateArgs<S>(e.Repository, OperateType.Update, i));
                }
            }
        }
    }
}
