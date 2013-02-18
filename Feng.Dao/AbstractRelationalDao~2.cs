using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public abstract class AbstractRelationalDao<T, S> : BaseDao<T>, IRelationalDao<T, S>
        where T : class, IEntity
        where S : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public AbstractRelationalDao(IRepositoryDao<S> detailDao)
        {
            m_detailDao = detailDao;
        }

        /// <summary>
        /// 当出错时，清理
        /// </summary>
        public override void Clear()
        {
            m_detailDao.Clear();

            base.Clear();
        }

        private readonly IRepositoryDao<S> m_detailDao;

        /// <summary>
        /// DetailDao
        /// </summary>
        public IRepositoryDao<S> DetailDao
        {
            get { return m_detailDao; }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public abstract class AbstractMemoriedRelationalDao<T, S> : AbstractRelationalDao<T, S>, IMemoriedRelationalDao<T, S>, IMemoriedRelationalDao
        where T : class, IEntity
        where S : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public AbstractMemoriedRelationalDao(IRepositoryDao<S> detailDao)
            : this(detailDao, new MemoryDao<S>())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        /// <param name="memoryDao"></param>
        public AbstractMemoriedRelationalDao(IRepositoryDao<S> detailDao, IMemoryDao<S> memoryDao)
            : base(detailDao)
        {
            m_detailMemoryDao = memoryDao;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        public virtual void AddRelationToMemoryDao(IEntityList cm)
        {
        }

        /// <summary>
        /// 当出错时，清理
        /// </summary>
        public override void Clear()
        {
            m_detailMemoryDao.Clear();

            base.Clear();
        }

        private readonly IMemoryDao<S> m_detailMemoryDao;

        /// <summary>
        /// DetailMemoryDao
        /// </summary>
        public IMemoryDao<S> DetailMemoryDao
        {
            get { return m_detailMemoryDao; }
        }

        /// <summary>
        /// DetailMemoryDao
        /// </summary>
        IMemoryDao IMemoriedRelationalDao.DetailMemoryDao
        {
            get 
            {
                IMemoryDao md = m_detailMemoryDao as IMemoryDao;
                if (md == null)
                {
                    throw new ArgumentException("you should call DetailMemoryDao[S]!");
                }
                return md; 
            }
        }

        /// <summary>
        /// 在Transaction开始前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBeginning(OperateArgs<T> e)
        {
            base.OnTransactionBeginning(e);

            foreach (S i in this.DetailMemoryDao.SavedEntities)
            {
                this.DetailDao.OnTransactionBeginning(new OperateArgs<S>(e.Repository, OperateType.Save, i));
            }
            foreach (S i in this.DetailMemoryDao.UpdatedEntities)
            {
                this.DetailDao.OnTransactionBeginning(new OperateArgs<S>(e.Repository, OperateType.Update, i));
            }
            foreach (S i in this.DetailMemoryDao.DeletedEntities)
            {
                this.DetailDao.OnTransactionBeginning(new OperateArgs<S>(e.Repository, OperateType.Delete, i));
            }
        }

        /// <summary>
        /// 在Transaction开始后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionBegun(OperateArgs<T> e)
        {
            base.OnTransactionBegun(e);

            foreach (S i in this.DetailMemoryDao.SavedEntities)
            {
                this.DetailDao.OnTransactionBegun(new OperateArgs<S>(e.Repository, OperateType.Save, i));
            }
            foreach (S i in this.DetailMemoryDao.UpdatedEntities)
            {
                this.DetailDao.OnTransactionBegun(new OperateArgs<S>(e.Repository, OperateType.Update, i));
            }
            foreach (S i in this.DetailMemoryDao.DeletedEntities)
            {
                this.DetailDao.OnTransactionBegun(new OperateArgs<S>(e.Repository, OperateType.Delete, i));
            }
        }

        /// <summary>
        /// 在Transaction Commited前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommitting(OperateArgs<T> e)
        {
            base.OnTransactionCommitting(e);

            foreach (S i in this.DetailMemoryDao.SavedEntities)
            {
                this.DetailDao.OnTransactionCommitting(new OperateArgs<S>(e.Repository, OperateType.Save, i));
            }
            foreach (S i in this.DetailMemoryDao.UpdatedEntities)
            {
                this.DetailDao.OnTransactionCommitting(new OperateArgs<S>(e.Repository, OperateType.Update, i));
            }
            foreach (S i in this.DetailMemoryDao.DeletedEntities)
            {
                this.DetailDao.OnTransactionCommitting(new OperateArgs<S>(e.Repository, OperateType.Delete, i));
            }
        }

        /// <summary>
        /// 在Transaction Commited后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionCommited(OperateArgs<T> e)
        {
            base.OnTransactionCommited(e);

            foreach (S i in this.DetailMemoryDao.SavedEntities)
            {
                this.DetailDao.OnTransactionCommited(new OperateArgs<S>(e.Repository, OperateType.Save, i));
            }
            foreach (S i in this.DetailMemoryDao.UpdatedEntities)
            {
                this.DetailDao.OnTransactionCommited(new OperateArgs<S>(e.Repository, OperateType.Update, i));
            }
            foreach (S i in this.DetailMemoryDao.DeletedEntities)
            {
                this.DetailDao.OnTransactionCommited(new OperateArgs<S>(e.Repository, OperateType.Delete, i));
            }
        }

        /// <summary>
        /// 在Transaction Rollback前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacking(OperateArgs<T> e)
        {
            base.OnTransactionRollbacking(e);

            foreach (S i in this.DetailMemoryDao.SavedEntities)
            {
                this.DetailDao.OnTransactionRollbacking(new OperateArgs<S>(e.Repository, OperateType.Save, i));
            }
            foreach (S i in this.DetailMemoryDao.UpdatedEntities)
            {
                this.DetailDao.OnTransactionRollbacking(new OperateArgs<S>(e.Repository, OperateType.Update, i));
            }
            foreach (S i in this.DetailMemoryDao.DeletedEntities)
            {
                this.DetailDao.OnTransactionRollbacking(new OperateArgs<S>(e.Repository, OperateType.Delete, i));
            }
        }

        /// <summary>
        /// 在Transaction Rollback后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnTransactionRollbacked(OperateArgs<T> e)
        {
            base.OnTransactionRollbacked(e);

            foreach (S i in this.DetailMemoryDao.SavedEntities)
            {
                this.DetailDao.OnTransactionRollbacked(new OperateArgs<S>(e.Repository, OperateType.Save, i));
            }
            foreach (S i in this.DetailMemoryDao.UpdatedEntities)
            {
                this.DetailDao.OnTransactionRollbacked(new OperateArgs<S>(e.Repository, OperateType.Update, i));
            }
            foreach (S i in this.DetailMemoryDao.DeletedEntities)
            {
                this.DetailDao.OnTransactionRollbacked(new OperateArgs<S>(e.Repository, OperateType.Delete, i));
            }
        }
    }
}
