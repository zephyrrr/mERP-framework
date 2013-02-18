using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="S"></typeparam>
    ///// <typeparam name="T"></typeparam>
    //public class ManytoOneDao<S, T> : ManytoOneDao<S, T, T>
    //    where T : class, IMasterEntity<T, S>
    //    where S : class, IDetailEntity<T, S>
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="childDao"></param>
    //    public ManytoOneDao(IRepositioryDao<T> childDao)
    //        : base(childDao)
    //    {
    //    }
    //}

    /// <summary>
    /// ManytoOne关系的Dao，处于Detail位置来操作Master。与MasterDao相反
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    ///// <typeparam name="TD"></typeparam>
    //public class ManytoOneDao<S, T, TD> : BaseDao<S>, IParentDao<S, TD>
    //    where T : class, IMasterEntity<T, S>
    //    where S : class, IDetailEntity<T, S>
    //    where TD: class, T
    public class ManytoOneDao<S, T> : AbstractRelationalDao<S, T>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="masterDao"></param>
        public ManytoOneDao(IRepositoryDao<T> masterDao)
            : base(masterDao)
        {
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBeginning(OperateArgs<S> e)
        {
            base.OnTransactionBeginning(e);

            this.DetailDao.OnTransactionBeginning(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.MasterEntity));
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBegun(OperateArgs<S> e)
        {
            base.OnTransactionBegun(e);

            this.DetailDao.OnTransactionBegun(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.MasterEntity));
        }

        /// <summary>
        /// 在Entity操作前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperating(OperateArgs<S> e)
        {
            base.OnEntityOperating(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    break;
                case OperateType.Update:
                    if (e.Entity.MasterEntity != null)
                    {
                        this.DetailDao.Update(e.Repository, e.Entity.MasterEntity);
                    }
                    break;
                case OperateType.Delete:
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommitting(OperateArgs<S> e)
        {
            base.OnTransactionCommitting(e);

            this.DetailDao.OnTransactionCommitting(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.MasterEntity));
        }

        /// <summary>
        /// 在Transaction Commited后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommited(OperateArgs<S> e)
        {
            base.OnTransactionCommited(e);

            this.DetailDao.OnTransactionCommited(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.MasterEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacking(OperateArgs<S> e)
        {
            base.OnTransactionRollbacking(e);

            this.DetailDao.OnTransactionRollbacking(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.MasterEntity));
        }

        /// <summary>
        /// 在Transaction Rollback后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacked(OperateArgs<S> e)
        {
            base.OnTransactionRollbacked(e);

            this.DetailDao.OnTransactionRollbacked(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.MasterEntity));
        }

    }
}
