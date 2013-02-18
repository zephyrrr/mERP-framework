using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 实现<see cref="IEventDao"/>的基本抽象类
    /// </summary>
    public abstract class AbstractEventDao : IEventDao
    {
        #region"Event"
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> TransactionBeginning;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> TransactionBegun;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> EntityOperating;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> EntityOperated;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> TransactionCommitting;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> TransactionCommited;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> TransactionRollbacking;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OperateArgs> TransactionRollbacked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnTransactionBeginning(OperateArgs e)
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
        public virtual void OnTransactionBegun(OperateArgs e)
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
        /// 在Entity操作前执行
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnEntityOperating(OperateArgs e)
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
        public virtual void OnEntityOperated(OperateArgs e)
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
        public virtual void OnTransactionCommitting(OperateArgs e)
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
        public virtual void OnTransactionCommited(OperateArgs e)
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
        public virtual void OnTransactionRollbacking(OperateArgs e)
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
        public virtual void OnTransactionRollbacked(OperateArgs e)
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

        #region "Operate"
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Save(object entity);

        /// <summary>
        /// 增加或修改
        /// </summary>
        /// <param name="entity"></param>
        public abstract void SaveOrUpdate(object entity);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Update(object entity);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Delete(object entity);
        #endregion

        #region "SubDaos"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IRepository GenerateRepository()
        {
            return null;
        }

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
        /// GetRelationalDaos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEventDao> GetRelationalDaos()
        {
            return m_relationalDaos;
        }
        #endregion
    }
}
