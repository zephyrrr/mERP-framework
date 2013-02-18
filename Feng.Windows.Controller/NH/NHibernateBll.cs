//using System;
//using System.Collections.Generic;
//using System.Text;
//using NHibernate;

//namespace Feng.NH
//{
//    /// <summary>
//    /// 用于NHibernate的简单业务逻辑层，封装数据访问层
//    /// </summary>
//    public class NHibernateBll : IBaseBll
//    {
//        private IBaseDal m_dal = new NHibernateDal();

//        /// <summary>
//        /// 增加
//        /// </summary>
//        /// <param name="entity"></param>
//        public virtual void Save(object entity)
//        {
//            m_dal.Save(entity);
//        }

//        /// <summary>
//        /// 修改
//        /// </summary>
//        /// <param name="entity"></param>
//        public virtual void Update(object entity)
//        {
//            m_dal.Update(entity);
//        }

//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="entity"></param>
//        public virtual void Delete(object entity)
//        {
//            m_dal.Delete(entity);
//        }

//        /// <summary>
//        /// 清理
//        /// </summary>
//        public virtual void Clear()
//        {
//        }
//    }

//    /// <summary>
//    /// NHibernateDal
//    /// </summary>
//    public class NHibernateDal : IBaseDal
//    {
//        /// <summary>
//        /// 新增
//        /// </summary>
//        /// <param name="entity"></param>
//        public virtual void Save(object entity)
//        {
//            using (var rep = RepositoryFactory.GenerateRepository(entity.GetType()))
//            {
//                try
//                {
//                    rep.BeginTransaction();
//                    rep.Save(entity);
//                    rep.CommitTransaction();
//                }
//                catch (Exception ex)
//                {
//                    rep.RollbackTransaction();
//                    if (ExceptionProcess.ProcessWithWrap(ex))
//                    {
//                        throw;
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// 修改
//        /// </summary>
//        /// <param name="entity"></param>
//        public virtual void Update(object entity)
//        {
//            using (var rep = RepositoryFactory.GenerateRepository(entity.GetType()))
//            {
//                try
//                {
//                    rep.BeginTransaction();
//                    rep.Update(entity);
//                    rep.CommitTransaction();
//                }
//                catch (Exception ex)
//                {
//                    rep.RollbackTransaction();
//                    if (ExceptionProcess.ProcessWithWrap(ex))
//                    {
//                        throw;
//                    }
//                }
//            }
//        }


//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="entity"></param>
//        public virtual void Delete(object entity)
//        {
//            using (var rep = RepositoryFactory.GenerateRepository(entity.GetType()))
//            {
//                try
//                {
//                    rep.BeginTransaction();
//                    rep.Delete(entity);
//                    rep.CommitTransaction();
//                }
//                // delete will check not-null, but when in web grid, we only have id(and version) set back
//                catch (PropertyValueException)
//                {
//                    rep.RollbackTransaction();
//                    LoadandDelete(entity);
//                }
//                catch (Exception ex)
//                {
//                    rep.RollbackTransaction();
//                    if (ExceptionProcess.ProcessWithWrap(ex))
//                    {
//                        throw;
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// LoadandDelete
//        /// </summary>
//        /// <param name="entity"></param>
//        private void LoadandDelete(object entity)
//        {
//            using (var rep = RepositoryFactory.GenerateRepository(entity.GetType()))
//            {
//                try
//                {
//                    rep.Session.Lock(entity, LockMode.None);
//                    object id = rep.Session.GetIdentifier(entity);
//                    rep.Session.Evict(entity);
//                    entity = rep.Session.Get(entity.GetType(), id);
//                    rep.BeginTransaction();
//                    rep.Delete(entity);
//                    rep.CommitTransaction();
//                }
//                catch (Exception ex)
//                {
//                    rep.RollbackTransaction();
//                    if (ExceptionProcess.ProcessWithWrap(ex))
//                    {
//                        throw;
//                    }
//                }
//            }
//        }
//    }
//}