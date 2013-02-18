using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 实现<see cref="IEventDao"/>的基本Dao类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractEventDao<T> : IEventDao<T>, IEventDao
        where T : class, IEntity
    {
        #region"Event"
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> TransactionBeginning;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> TransactionBegun;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> EntityOperating;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> EntityOperated;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> TransactionCommitting;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> TransactionCommited;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> TransactionRollbacking;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs<T>> TransactionRollbacked;

        /// <summary>
        /// 在Entity操作前执行
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnEntityOperating(OperateArgs<T> e)
        {
            if (EntityOperating != null)
            {
                EntityOperating(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnEntityOperating(e);
            }
        }

        /// <summary>
        /// 在Entity操作后执行
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnEntityOperated(OperateArgs<T> e)
        {
            if (EntityOperated != null)
            {
                EntityOperated(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnEntityOperated(e);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTransactionBeginning(OperateArgs<T> e)
        {
            if (TransactionBeginning != null)
            {
                TransactionBeginning(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnTransactionBeginning(e);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTransactionBegun(OperateArgs<T> e)
        {
            if (TransactionBegun != null)
            {
                TransactionBegun(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnTransactionBegun(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTransactionCommitting(OperateArgs<T> e)
        {
            if (TransactionCommitting != null)
            {
                TransactionCommitting(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnTransactionCommitting(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTransactionCommited(OperateArgs<T> e)
        {
            if (TransactionCommited != null)
            {
                TransactionCommited(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnTransactionCommited(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTransactionRollbacking(OperateArgs<T> e)
        {
            if (TransactionRollbacking != null)
            {
                TransactionRollbacking(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnTransactionRollbacking(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTransactionRollbacked(OperateArgs<T> e)
        {
            if (TransactionRollbacked != null)
            {
                TransactionRollbacked(this, e);
            }

            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                subDao.OnTransactionRollbacked(e);
            }
        }

        #endregion

        #region "Operate T"
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Save(T entity);

        /// <summary>
        /// 增加或修改
        /// </summary>
        /// <param name="entity"></param>
        public abstract void SaveOrUpdate(T entity);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Update(T entity);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Delete(T entity);
        #endregion

        #region "SubDaos"
        /// <summary>
        /// 
        /// </summary>
        private IList<IEventDao> m_relationalDaos = new List<IEventDao>();

        /// <summary>
        /// 增加子Dao
        /// 当主Dao操作时，不仅主Dao会发生各种事件，子Dao也会发生事件。
        /// 例如Save时，产生EntityOperating事件，子Dao也会产生相应事件
        /// </summary>
        /// <param name="relationalDao"></param>
        public void AddRelationalDao(IEventDao relationalDao)
        {
            if (relationalDao == null)
            {
                throw new ArgumentNullException("relationalDao");
            }
            m_relationalDaos.Add(relationalDao);
        }

        /// <summary>
        /// 根据序号获取子Dao
        /// </summary>
        /// <param name="idx"></param>
        public IEventDao GetRelationalDao(int idx)
        {
            if (idx < 0 || idx >= m_relationalDaos.Count)
            {
                throw new ArgumentException("Invalid idx");
            }
            return m_relationalDaos[idx];
        }

        ///// <summary>
        ///// 根据类型获取子Dao
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public IDao GetSubDao(Type type)
        //{
        //    foreach (IDao subDao in m_relationalDaos)
        //    {
        //        if (subDao.GetType() == type)
        //        {
        //            return subDao;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 当出错时，清理
        /// </summary>
        public virtual void Clear()
        {
            foreach (IEventDao relationaDao in m_relationalDaos)
            {
                relationaDao.Clear();
            }
        }

        /// <summary>
        /// GetSubDaos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEventDao> GetRelationalDaos()
        {
            return m_relationalDaos;
        }
        #endregion

        #region"Interface Wrapper"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void IBaseDao.Save(object entity)
        {
            Save((T)entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void IBaseDao.Update(object entity)
        {
            Update((T)entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void IBaseDao.Delete(object entity)
        {
            Delete((T)entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void IBaseDao.SaveOrUpdate(object entity)
        {
            SaveOrUpdate((T)entity);
        }

        void IEventDao.OnTransactionBeginning(OperateArgs e)
        {
            this.OnTransactionBeginning(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }
        void IEventDao.OnTransactionBegun(OperateArgs e)
        {
            this.OnTransactionBegun(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }
        void IEventDao.OnEntityOperating(OperateArgs e)
        {
            this.OnEntityOperating(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }
        void IEventDao.OnEntityOperated(OperateArgs e)
        {
            this.OnEntityOperated(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }
        void IEventDao.OnTransactionCommitting(OperateArgs e)
        {
            this.OnTransactionCommitting(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }
        void IEventDao.OnTransactionCommited(OperateArgs e)
        {
            this.OnTransactionCommited(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }
        void IEventDao.OnTransactionRollbacking(OperateArgs e)
        {
            this.OnTransactionRollbacking(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }
        void IEventDao.OnTransactionRollbacked(OperateArgs e)
        {
            this.OnTransactionRollbacked(new OperateArgs<T>(e.Repository, e.OperateType, e.Entity as T));
        }


        private IList<EventHandler<OperateArgs>> m_untypedEventsTransactionBeginning;
        event EventHandler<OperateArgs> IEventDao.TransactionBeginning
        {
            add 
            {
                if (m_untypedEventsTransactionBeginning == null)
                {
                    m_untypedEventsTransactionBeginning = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsTransactionBeginning.Count == 0)
                {
                    this.TransactionBeginning += new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionBeginning);
                }
                m_untypedEventsTransactionBeginning.Add(value);
            }
            remove 
            {
                m_untypedEventsTransactionBeginning.Remove(value);
                if (m_untypedEventsTransactionBeginning.Count == 0)
                {
                    this.TransactionBeginning -= new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionBeginning);
                }
            }
        }
        void AbstractEventDao_TransactionBeginning(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsTransactionBeginning)
            {
                handler.Invoke(sender, e);
            }
        }

        private IList<EventHandler<OperateArgs>> m_untypedEventsTransactionBegun;
        event EventHandler<OperateArgs> IEventDao.TransactionBegun
        {
            add
            {
                if (m_untypedEventsTransactionBegun == null)
                {
                    m_untypedEventsTransactionBegun = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsTransactionBegun.Count == 0)
                {
                    this.TransactionBegun+= new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionBegun);
                }
                m_untypedEventsTransactionBegun.Add(value);
            }
            remove
            {
                m_untypedEventsTransactionBegun.Remove(value);
                if (m_untypedEventsTransactionBegun.Count == 0)
                {
                    this.TransactionBegun -= new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionBegun);
                }
            }
        }
        void AbstractEventDao_TransactionBegun(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsTransactionBegun)
            {
                handler.Invoke(sender, e);
            }
        }

        private IList<EventHandler<OperateArgs>> m_untypedEventsEntityOperating;
        event EventHandler<OperateArgs> IEventDao.EntityOperating
        {
            add
            {
                if (m_untypedEventsEntityOperating == null)
                {
                    m_untypedEventsEntityOperating = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsEntityOperating.Count == 0)
                {
                    this.EntityOperating += new EventHandler<OperateArgs<T>>(AbstractEventDao_EntityOperating);
                }
                m_untypedEventsEntityOperating.Add(value);
            }
            remove
            {
                m_untypedEventsEntityOperating.Remove(value);
                if (m_untypedEventsEntityOperating.Count == 0)
                {
                    this.EntityOperating -= new EventHandler<OperateArgs<T>>(AbstractEventDao_EntityOperating);
                }
            }
        }
        void AbstractEventDao_EntityOperating(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsEntityOperating)
            {
                handler.Invoke(sender, e);
            }
        }

        private IList<EventHandler<OperateArgs>> m_untypedEventsEntityOperated;
        event EventHandler<OperateArgs> IEventDao.EntityOperated
        {
            add
            {
                if (m_untypedEventsEntityOperated == null)
                {
                    m_untypedEventsEntityOperated = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsEntityOperated.Count == 0)
                {
                    this.EntityOperated += new EventHandler<OperateArgs<T>>(AbstractEventDao_EntityOperated);
                }
                m_untypedEventsEntityOperated.Add(value);
            }
            remove
            {
                m_untypedEventsEntityOperated.Remove(value);
                if (m_untypedEventsEntityOperated.Count == 0)
                {
                    this.EntityOperated -= new EventHandler<OperateArgs<T>>(AbstractEventDao_EntityOperated);
                }
            }
        }
        void AbstractEventDao_EntityOperated(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsEntityOperated)
            {
                handler.Invoke(sender, e);
            }
        }

        private IList<EventHandler<OperateArgs>> m_untypedEventsTransactionCommitting;
        event EventHandler<OperateArgs> IEventDao.TransactionCommitting
        {
            add
            {
                if (m_untypedEventsTransactionCommitting == null)
                {
                    m_untypedEventsTransactionCommitting = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsTransactionCommitting.Count == 0)
                {
                    this.TransactionCommitting += new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionCommitting);
                }
                m_untypedEventsTransactionCommitting.Add(value);
            }
            remove
            {
                m_untypedEventsTransactionCommitting.Remove(value);
                if (m_untypedEventsTransactionCommitting.Count == 0)
                {
                    this.TransactionCommitting -= new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionCommitting);
                }
            }
        }
        void AbstractEventDao_TransactionCommitting(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsTransactionCommitting)
            {
                handler.Invoke(sender, e);
            }
        }

        private IList<EventHandler<OperateArgs>> m_untypedEventsTransactionCommited;
        event EventHandler<OperateArgs> IEventDao.TransactionCommited
        {
            add
            {
                if (m_untypedEventsTransactionCommited == null)
                {
                    m_untypedEventsTransactionCommited = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsTransactionCommited.Count == 0)
                {
                    this.TransactionCommited += new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionCommited);
                }
                m_untypedEventsTransactionCommited.Add(value);
            }
            remove
            {
                m_untypedEventsTransactionCommited.Remove(value);
                if (m_untypedEventsTransactionCommited.Count == 0)
                {
                    this.TransactionCommited -= new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionCommited);
                }
            }
        }
        void AbstractEventDao_TransactionCommited(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsTransactionCommited)
            {
                handler.Invoke(sender, e);
            }
        }

        private IList<EventHandler<OperateArgs>> m_untypedEventsTransactionRollbacking;
        event EventHandler<OperateArgs> IEventDao.TransactionRollbacking
        {
            add
            {
                if (m_untypedEventsTransactionRollbacking == null)
                {
                    m_untypedEventsTransactionRollbacking = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsTransactionRollbacking.Count == 0)
                {
                    this.TransactionRollbacking += new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionRollbacking);
                }
                m_untypedEventsTransactionRollbacking.Add(value);
            }
            remove
            {
                m_untypedEventsTransactionRollbacking.Remove(value);
                if (m_untypedEventsTransactionRollbacking.Count == 0)
                {
                    this.TransactionRollbacking -= new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionRollbacking);
                }
            }
        }
        void AbstractEventDao_TransactionRollbacking(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsTransactionRollbacking)
            {
                handler.Invoke(sender, e);
            }
        }

        private IList<EventHandler<OperateArgs>> m_untypedEventsTransactionRollbacked;
        event EventHandler<OperateArgs> IEventDao.TransactionRollbacked
        {
            add
            {
                if (m_untypedEventsTransactionRollbacked == null)
                {
                    m_untypedEventsTransactionRollbacked = new List<EventHandler<OperateArgs>>();
                }
                if (m_untypedEventsTransactionRollbacked.Count == 0)
                {
                    this.TransactionRollbacked += new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionRollbacked);
                }
                m_untypedEventsTransactionRollbacked.Add(value);
            }
            remove
            {
                m_untypedEventsTransactionRollbacked.Remove(value);
                if (m_untypedEventsTransactionRollbacked.Count == 0)
                {
                    this.TransactionRollbacked -= new EventHandler<OperateArgs<T>>(AbstractEventDao_TransactionRollbacked);
                }
            }
        }
        void AbstractEventDao_TransactionRollbacked(object sender, OperateArgs<T> e)
        {
            foreach (EventHandler<OperateArgs> handler in m_untypedEventsTransactionRollbacked)
            {
                handler.Invoke(sender, e);
            }
        }

        //void IRelationalDao.AddRelationalDao(IEventDao relationalDao)
        //{
        //    this.AddRelationalDao(relationalDao as IEventDao<T>);
        //}
        //IEventDao IRelationalDao.GetRelationalDao(int idx)
        //{
        //    return this.GetRelationalDao(idx) as IEventDao;
        //}
        #endregion
    }
}
