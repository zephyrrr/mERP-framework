using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// OnetoOne关系的Dao
    /// 用途1：更新OneToOne关系的两个表，只是更新，不生成或者删除新纪录
    /// 用途2：可以是两者在同一表，实现Lazy Load Property
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class OnetoOneChildDao<S, T> : AbstractRelationalDao<S, T>
        where T : class, IOnetoOneParentEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentDao"></param>
        public OnetoOneChildDao(IRepositoryDao<T> parentDao)
            : base(parentDao)
        {
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBeginning(OperateArgs<S> e)
        {
            base.OnTransactionBeginning(e);

            this.DetailDao.OnTransactionBeginning(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.ParentEntity));
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBegun(OperateArgs<S> e)
        {
            base.OnTransactionBegun(e);

            this.DetailDao.OnTransactionBegun(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.ParentEntity));
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
                    break;
                case OperateType.Delete:
                    // OneToOne，当删除Child时，不用删除Parent
                    //if (e.Entity.ParentEntity != null)
                    //{
                    //    throw new InvalidUserOperationException("还有后继数据，不能删除当前数据！请先删除后继数据。");
                    //}
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }

        /// <summary>
        /// 在Entity操作后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperated(OperateArgs<S> e)
        {
            base.OnEntityOperated(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    break;
                case OperateType.Update:
                    if (e.Entity.ParentEntity != null)
                    {
                        this.DetailDao.Update(e.Repository, e.Entity.ParentEntity);
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

            this.DetailDao.OnTransactionCommitting(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.ParentEntity));
        }

        /// <summary>
        /// 在Transaction Commited后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommited(OperateArgs<S> e)
        {
            base.OnTransactionCommited(e);

            this.DetailDao.OnTransactionCommited(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.ParentEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacking(OperateArgs<S> e)
        {
            base.OnTransactionRollbacking(e);

            this.DetailDao.OnTransactionRollbacking(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.ParentEntity));
        }

        /// <summary>
        /// 在Transaction Rollback后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacked(OperateArgs<S> e)
        {
            base.OnTransactionRollbacked(e);

            this.DetailDao.OnTransactionRollbacked(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity.ParentEntity));
        }

    }
}
