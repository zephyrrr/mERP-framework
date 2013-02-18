using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Feng.NH
{
    /// <summary>
    /// 简单NHibernate的数据访问层实现
    /// </summary>
    public class NHibernateDao<T> : BaseDao<T>, IEventDao<T>, IRepositoryConsumer
        where T : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NHibernateDao()
            : this(Feng.Utils.RepositoryHelper.GetConfigNameFromType(typeof(T)))
        {

        }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="repCfgName"></param>
        public NHibernateDao(string repCfgName)
            : base(repCfgName)
        {
        }

        #region "Load"
        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(object id)
        {
            using (var rep = GenerateRepository())
            {
                rep.BeginTransaction();
                var ret = rep.Get<T>(id);
                rep.CommitTransaction();
                return ret;
            }
        }

        /// <summary>
        /// 读取全部
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAll(int startIndex, int maxRows, string sortedBy)
        {
            using (var rep = new Repository(this.RepositoryCfgName))
            {
                rep.BeginTransaction();
                NHibernate.ICriteria cri = rep.Session.CreateCriteria(typeof(T));

                var ret = GetAll(cri, startIndex, maxRows, sortedBy);

                rep.CommitTransaction();
                return ret;
            }
        }

        /// <summary>
        /// 分页读取
        /// </summary>
        /// <param name="cri"></param>
        /// <param name="startIndex"></param>
        /// <param name="maxRows"></param>
        /// <param name="sortedBy"></param>
        /// <returns></returns>
        protected IList<T> GetAll(NHibernate.ICriteria cri, int startIndex, int maxRows, string sortedBy)
        {
            cri.SetFirstResult(startIndex);
            if (maxRows > 0)
            {
                cri.SetMaxResults(maxRows);
            }
            if (!string.IsNullOrEmpty(sortedBy))
            {
                string[] ss = sortedBy.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length == 2 && ss[1].ToUpper() == "DESC")
                {
                    cri.AddOrder(new NHibernate.Criterion.Order(ss[0], false));
                }
                else
                {
                    cri.AddOrder(new NHibernate.Criterion.Order(ss[0], true));
                }
            }
            return cri.List<T>();
        }

        /// <summary>
        /// 获得读取的总数量
        /// </summary>
        /// <returns></returns>
        public int GetAllCount()
        {
            using (var rep = new Repository(this.RepositoryCfgName))
            {
                rep.BeginTransaction();
                var ret = GetAllCount(rep.Session.CreateCriteria(typeof (T)));
                rep.CommitTransaction();
                return ret;
            }
        }

        /// <summary>
        /// 获得分页读取的总数量
        /// </summary>
        /// <param name="cri"></param>
        /// <returns></returns>
        protected int GetAllCount(NHibernate.ICriteria cri)
        {
            System.Collections.IList list = cri
                .SetProjection(NHibernate.Criterion.Projections.RowCount())
                .List();
            if (list.Count > 0)
            {
                return (int) list[0];
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region "Operate"
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(T entity)
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();
                    rep.Save(entity);
                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(T entity)
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();
                    rep.Update(entity);
                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// SaveOrUpdate
        /// </summary>
        /// <param name="entity"></param>
        public override void SaveOrUpdate(T entity)
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();
                    rep.SaveOrUpdate(entity);
                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(T entity)
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();
                    rep.Delete(entity);
                    rep.CommitTransaction();
                }
                    // delete will check not-null, but when in web grid, we only have id(and version) set back
                catch (PropertyValueException)
                {
                    rep.RollbackTransaction();
                    LoadandDelete(entity);
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// LoadandDelete
        /// </summary>
        /// <param name="entity"></param>
        private void LoadandDelete(T entity)
        {
            using (var rep = new Repository(this.RepositoryCfgName))
            {
                try
                {
                    rep.Session.Lock(entity, LockMode.None);
                    object id = rep.Session.GetIdentifier(entity);
                    rep.Session.Evict(entity);
                    entity = rep.Session.Get<T>(id);
                    rep.BeginTransaction();
                    rep.Delete(entity);
                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }
        #endregion
    }
}