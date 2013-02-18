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
    public class OnetoOneDao<T, S> : AbstractRelationalDao<T, S>
        where T : class, IOnetoOneParentEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="childDao"></param>
        public OnetoOneDao(IRepositoryDao<S> childDao)
            : base(childDao)
        {
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBeginning(OperateArgs<T> e)
        {
            base.OnTransactionBeginning(e);

            this.DetailDao.OnTransactionBeginning(new OperateArgs<S>(e.Repository, e.OperateType, e.Entity.ChildEntity));
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBegun(OperateArgs<T> e)
        {
            base.OnTransactionBegun(e);

            this.DetailDao.OnTransactionBegun(new OperateArgs<S>(e.Repository, e.OperateType, e.Entity.ChildEntity));
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
                    break;
                case OperateType.Update:
                    break;
                case OperateType.Delete:
                    if (e.Entity.ChildEntity != null)
                    {
                        throw new InvalidUserOperationException("还有后继数据，不能删除当前数据！请先删除后继数据。");
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
                    break;
                case OperateType.Update:
                    if (e.Entity.ChildEntity != null)
                    {
                        this.DetailDao.Update(e.Repository, e.Entity.ChildEntity);
                    }
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

            this.DetailDao.OnTransactionCommitting(new OperateArgs<S>(e.Repository, e.OperateType, e.Entity.ChildEntity));
        }

        /// <summary>
        /// 在Transaction Commited后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommited(OperateArgs<T> e)
        {
            base.OnTransactionCommited(e);

            this.DetailDao.OnTransactionCommited(new OperateArgs<S>(e.Repository, e.OperateType, e.Entity.ChildEntity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacking(OperateArgs<T> e)
        {
            base.OnTransactionRollbacking(e);

            this.DetailDao.OnTransactionRollbacking(new OperateArgs<S>(e.Repository, e.OperateType, e.Entity.ChildEntity));
        }

        /// <summary>
        /// 在Transaction Rollback后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacked(OperateArgs<T> e)
        {
            base.OnTransactionRollbacked(e);

            this.DetailDao.OnTransactionRollbacked(new OperateArgs<S>(e.Repository, e.OperateType, e.Entity.ChildEntity));
        }

    }
}
