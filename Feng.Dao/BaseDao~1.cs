using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 实现<see cref="IEventDao"/>的基本Dao类。
    /// 各个Operation 函数重载的关系为（以Delete为例)
    /// Delete(object) -> Delete(entity)[Transaction Begin] -> Delete(Rep, entity) [Operation Begin] -> DoDelete(Rep, entity) [Delete]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDao<T> : AbstractEventDao<T>, IRepositoryDao<T>, IRepositoryConsumer, IBatchDao
        where T : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseDao()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repCfgName"></param>
        public BaseDao(string repCfgName)
        {
            m_repCfgName = repCfgName;
        }

        private string m_repCfgName;
        /// <summary>
        /// Repository配置名。可为空，空时采用默认名。
        /// </summary>
        public string RepositoryCfgName
        {
            get
            {
                if (string.IsNullOrEmpty(m_repCfgName))
                {
                    return Feng.Utils.RepositoryHelper.GetConfigNameFromType(typeof(T));
                }
                else
                {
                    return m_repCfgName;
                }
            }
            set
            {
                m_repCfgName = value;
            }
        }

        /// <summary>
        /// 生成IRepository
        /// </summary>
        /// <returns></returns>
        public virtual IRepository GenerateRepository()
        {
            return ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(this);
        }

        /// <summary>
        /// 根据范型类型获取子Dao
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        public R GetSubDao<R>()
            where R : class, IEventDao<T>
        {
            foreach (IEventDao subDao in this.GetRelationalDaos())
            {
                if (subDao is R)
                {
                    return subDao as R;
                }
            }
            return null;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public static T Get(object id)
        //{
        //    using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<T>())
        //    {
        //        return rep.Get<T>(id);
        //    }
        //}

        #region "Prepare"
        private void PreparedEntity(OperateArgs<T> e)
        {
            IOperatingEntity entity2 = e.Entity as IOperatingEntity;
            if (entity2 != null)
            {
                entity2.PreparedOperate(e);
            }
        }

        private void PreparingEntity(OperateArgs<T> e)
        {
            {
                IOperatingEntity entity2 = e.Entity as IOperatingEntity;
                if (entity2 != null)
                {
                    entity2.PreparingOperate(e);
                }
            }

            if (e.OperateType == OperateType.Delete)
            {
                IDeletableEntity entity2 = e.Entity as IDeletableEntity;
                if (entity2 != null && !entity2.CanBeDelete(e))
                {
                    throw new InvalidUserOperationException(entity2.ToString() + "不能删除!");
                }
            }
            else if (e.OperateType == OperateType.Update)
            {
                IUpdatableEntity entity2 = e.Entity as IUpdatableEntity;
                if (entity2 != null && !entity2.CanBeUpdate(e))
                {
                    throw new InvalidUserOperationException(entity2.ToString() + "不能更新!");
                }
            }
            else if (e.OperateType == OperateType.Save)
            {
                ISavableEntity entity2 = e.Entity as ISavableEntity;
                if (entity2 != null && !entity2.CanBeSave(e))
                {
                    throw new InvalidUserOperationException(entity2.ToString() + "不能添加!");
                }
            }
        }
        #endregion

        #region"Operate(rep, entity)"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        public void Save(IRepository rep, T entity)
        {
            OperateArgs<T> e = new OperateArgs<T>(rep, OperateType.Save, entity);

            PreparingEntity(e);

            OnEntityOperating(e);

            DoSave(rep, entity);

            OnEntityOperated(e);

            PreparedEntity(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        public void SaveOrUpdate(IRepository rep, T entity)
        {
            // Todo: know how nhibernate to saveorupdate
            IVersionedEntity ve = entity as IVersionedEntity;
            if (ve == null)
            {
                throw new NotSupportedException("SaveOrUpdate only support IVersionedEntity!");
            }
            OperateArgs<T> e;
            if (ve.Version == 0)
            {
                e = new OperateArgs<T>(rep, OperateType.Save, entity);
            }
            else
            {
                e = new OperateArgs<T>(rep, OperateType.Update, entity);
            }

            PreparingEntity(e);

            OnEntityOperating(e);

            DoSaveOrUpdate(rep, entity);

            OnEntityOperated(e);

            PreparedEntity(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        public void Update(IRepository rep, T entity)
        {
            OperateArgs<T> e = new OperateArgs<T>(rep, OperateType.Update, entity);

            PreparingEntity(e);

            OnEntityOperating(e);

            DoUpdate(rep, entity);

            OnEntityOperated(e);

            PreparedEntity(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        public void Delete(IRepository rep, T entity)
        {
            OperateArgs<T> e = new OperateArgs<T>(rep, OperateType.Delete, entity);

            PreparingEntity(e);

            OnEntityOperating(e);

            DoDelete(rep, entity);

            OnEntityOperated(e);

            PreparedEntity(e);
        }

        #endregion

        #region "DoSave(rep, entity)"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        protected virtual void DoSave(IRepository rep, T entity)
        {
            rep.Save(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        protected virtual void DoSaveOrUpdate(IRepository rep, T entity)
        {
            rep.SaveOrUpdate(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        protected virtual void DoUpdate(IRepository rep, T entity)
        {
            rep.Update(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="entity"></param>
        protected virtual void DoDelete(IRepository rep, T entity)
        {
            rep.Delete(entity);
        }

        #endregion

        #region"Operate(entity)"
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(T entity)
        {
            if (m_suspendRep == null)
            {
                using (var rep = GenerateRepository())
                {
                    var e = new OperateArgs<T>(rep, OperateType.Save, entity);

                    try
                    {
                        BeginTransaction(rep, e);

                        Save(rep, entity);

                        CommitTransaction(rep, e);

                        Clear();
                    }
                    catch (InvalidUserOperationException)
                    {
                        // 出错的时候，MemoryBll不清空
                        RollbackTransaction(rep, e);

                        throw;
                    }
                    catch (Exception)
                    {
                        RollbackTransaction(rep, e);

                        Clear();

                        throw;
                    }
                }
            }
            else
            {
                Save(m_suspendRep, entity);
            }
        }

        private void RollbackTransaction(IRepository rep, OperateArgs<T> e)
        {
            if (rep.IsSupportTransaction)
            {
                OnTransactionRollbacking(e);
                rep.RollbackTransaction();
                OnTransactionRollbacked(e);
            }
        }

        private void CommitTransaction(IRepository rep, OperateArgs<T> e)
        {
            if (rep.IsSupportTransaction)
            {
                OnTransactionCommitting(e);
                rep.CommitTransaction();
                OnTransactionCommited(e);
            }
        }

        private void BeginTransaction(IRepository rep, OperateArgs<T> e)
        {
            if (rep.IsSupportTransaction)
            {
                OnTransactionBeginning(e);
                rep.BeginTransaction();
                OnTransactionBegun(e);
            }
        }

        /// <summary>
        /// 增加或者修改
        /// </summary>
        /// <param name="entity"></param>
        public override void SaveOrUpdate(T entity)
        {
            if (m_suspendRep == null)
            {
                using (var rep = GenerateRepository())
                {
                    IVersionedEntity ve = entity as IVersionedEntity;
                    if (ve == null)
                    {
                        throw new NotSupportedException("SaveOrUpdate only support IVersionedEntity!");
                    }
                    OperateArgs<T> e;
                    if (ve.Version == 0)
                    {
                        e = new OperateArgs<T>(rep, OperateType.Save, entity);
                    }
                    else
                    {
                        e = new OperateArgs<T>(rep, OperateType.Update, entity);
                    }

                    try
                    {
                        BeginTransaction(rep, e);

                        SaveOrUpdate(rep, entity);

                        CommitTransaction(rep, e);

                        Clear();
                    }
                    catch (InvalidUserOperationException)
                    {
                        // 出错的时候，MemoryBll不清空
                        RollbackTransaction(rep, e);

                        throw;
                    }
                    catch (Exception)
                    {
                        RollbackTransaction(rep, e);

                        Clear();

                        throw;
                    }
                }
            }
            else
            {
                SaveOrUpdate(m_suspendRep, entity);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(T entity)
        {
            if (m_suspendRep == null)
            {
                using (var rep = GenerateRepository())
                {
                    var e = new OperateArgs<T>(rep, OperateType.Update, entity);
                    try
                    {
                        BeginTransaction(rep, e);

                        Update(rep, entity);

                        CommitTransaction(rep, e);

                        Clear();
                    }
                    catch (InvalidUserOperationException)
                    {
                        // 出错的时候，MemoryBll不清空
                        RollbackTransaction(rep, e);

                        throw;
                    }
                    catch (Exception)
                    {
                        RollbackTransaction(rep, e);

                        Clear();
                        throw;
                    }
                }
            }
            else
            {
                Update(m_suspendRep, entity);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(T entity)
        {
            if (m_suspendRep == null)
            {
                using (var rep = GenerateRepository())
                {
                    var e = new OperateArgs<T>(rep, OperateType.Delete, entity);
                    try
                    {
                        BeginTransaction(rep, e);

                        Delete(rep, entity);

                        CommitTransaction(rep, e);

                        Clear();
                    }
                    //// delete will check not-null, but when in web grid, we only have id(and version) set back
                    //catch (NHibernate.PropertyValueException)
                    //{
                    //    rep.RollbackTransaction();
                    //    throw;
                    //    //LoadandDelete(entity);
                    //}
                    catch (InvalidUserOperationException)
                    {
                        // 出错的时候，MemoryBll不清空
                        RollbackTransaction(rep, e);
                        throw;
                    }
                    catch (Exception)
                    {
                        RollbackTransaction(rep, e);

                        Clear();
                        throw;
                    }
                }
            }
            else
            {
                Delete(m_suspendRep, entity);
            }
        }

        #endregion

        #region"Suspend"
        private IRepository m_suspendRep;
        /// <summary>
        /// 
        /// </summary>
        public void SuspendOperation()
        {
            m_suspendRep = GenerateRepository();
            if (m_suspendRep.IsSupportTransaction)
            {
                m_suspendRep.BeginTransaction();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CancelSuspendOperation()
        {
            if (m_suspendRep == null)
            {
                return;
                //throw new InvalidOperationException("you should call SuspendOperation first!");
            }

            try
            {
                if (m_suspendRep.IsSupportTransaction)
                {
                    m_suspendRep.RollbackTransaction();
                }
                //OnTransactionCommited(new OperateArgs<T>(rep, OperateType.Update, entity));

                Clear();
            }
            catch (Exception)
            {
                Clear();
                throw;
            }
            finally
            {
                m_suspendRep.Dispose();
                m_suspendRep = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResumeOperation()
        {
            if (m_suspendRep == null)
            {
                throw new InvalidOperationException("you should call SuspendOperation first!");
            }

            try
            {
                if (m_suspendRep.IsSupportTransaction)
                {
                    m_suspendRep.CommitTransaction();
                }
                //OnTransactionCommited(new OperateArgs<T>(rep, OperateType.Update, entity));

                Clear();
            }
            catch (InvalidUserOperationException)
            {
                // 出错的时候，MemoryBll不清空
                if (m_suspendRep.IsSupportTransaction)
                {
                    m_suspendRep.RollbackTransaction();
                }
                //OnTransactionRollbacked(new OperateArgs<T>(rep, OperateType.Update, entity));

                throw;
            }
            catch (Exception)
            {
                if (m_suspendRep.IsSupportTransaction)
                {
                    m_suspendRep.RollbackTransaction();
                }
                //OnTransactionRollbacked(new OperateArgs<T>(rep, OperateType.Update, entity));

                Clear();
                throw;
            }
            finally
            {
                m_suspendRep.Dispose();
                m_suspendRep = null;
            }
        }
        #endregion
    }
}
