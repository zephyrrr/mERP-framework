using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// IMemoryDao
    /// </summary>
    public interface IMemoryDao : IEventDao, IBatchDao
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable SavedItems { get; }
        /// <summary>
        /// 
        /// </summary>
        IEnumerable UpdatedItems { get; }
        /// <summary>
        /// 
        /// </summary>
        IEnumerable DeletedItems { get; }
    }

    /// <summary>
    /// IMemoryDao
    /// </summary>
    public interface IMemoryDao<T> : IEventDao<T>, IBatchDao
       where T : class, IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        IList<T> SavedEntities { get; }
        /// <summary>
        /// 
        /// </summary>
        IList<T> UpdatedEntities { get; }
        /// <summary>
        /// 
        /// </summary>
        IList<T> DeletedEntities { get; }
    }

    /// <summary>
    /// 各种操作的Entity保存在内存中
    /// 适用于MasterDao
    /// </summary>
    public class MemoryDao<T> : AbstractMemoryDao<T>, IMemoryDao<T>, IMemoryDao
        where T : class, IEntity
    {
        #region "Interface Wrapper"
        /// <summary>
        /// 不立即提交操作，而是放在缓存里，等ResumeOperation时提交
        /// </summary>
        void IBatchDao.SuspendOperation() { }

        /// <summary>
        /// 提交操作
        /// </summary>
        void IBatchDao.ResumeOperation() { }

        /// <summary>
        /// 取消挂起的操作
        /// </summary>
        void IBatchDao.CancelSuspendOperation() { }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable IMemoryDao.SavedItems
        {
            get { return m_savedEntities; }
        }
        /// <summary>
        /// 
        /// </summary>
        IEnumerable IMemoryDao.UpdatedItems
        {
            get { return m_updatedEntities; }
        }
        /// <summary>
        /// 
        /// </summary>
        IEnumerable IMemoryDao.DeletedItems
        {
            get { return m_deletedEntities; }
        }

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
        #endregion

        private IList<T> m_savedEntities = new List<T>();
        private IList<T> m_updatedEntities = new List<T>();
        private IList<T> m_deletedEntities = new List<T>();

        /// <summary>
        /// 清空
        /// </summary>
        public override void Clear()
        {
            base.Clear();

            m_savedEntities.Clear();
            m_updatedEntities.Clear();
            m_deletedEntities.Clear();
        }

        /// <summary>
        /// SavedEntities
        /// </summary>
        public IList<T> SavedEntities
        {
            get { return m_savedEntities; }
        }

        /// <summary>
        /// UpdatedEntities
        /// </summary>
        public IList<T> UpdatedEntities
        {
            get { return m_updatedEntities; }
        }

        /// <summary>
        /// UpdatedEntities
        /// </summary>
        public IList<T> DeletedEntities
        {
            get { return m_deletedEntities; }
        }
        
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public override void DoSave(T entity)
        {
            m_savedEntities.Add(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public override void DoUpdate(T entity)
        {
            for (int i = 0; i < m_savedEntities.Count; ++i)
            {
                if (object.ReferenceEquals(m_savedEntities[i], entity))
                {
                    return;
                }
            }

            for (int i = 0; i < m_updatedEntities.Count; ++i)
            {
                if (object.ReferenceEquals(m_updatedEntities[i], entity))
                {
                    return;
                }
            }
            m_updatedEntities.Add(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public override void DoDelete(T entity)
        {
            for (int i = 0; i < m_savedEntities.Count; ++i)
            {
                if (object.ReferenceEquals(m_savedEntities[i], entity))
                {
                    m_savedEntities.RemoveAt(i);
                    return;
                }
            }
            for (int i = 0; i < m_updatedEntities.Count; ++i)
            {
                if (object.ReferenceEquals(m_updatedEntities[i], entity))
                {
                    m_updatedEntities.RemoveAt(i);
                    m_deletedEntities.Add(entity);
                    return;
                }
            }

            for (int i = 0; i < m_deletedEntities.Count; ++i)
            {
                if (object.ReferenceEquals(m_deletedEntities[i], entity))
                {
                    return;
                }
            }
            m_deletedEntities.Add(entity);
        }

        /// <summary>
        /// MergedList
        /// </summary>
        /// <param name="origial"></param>
        /// <returns></returns>
        public IList<T> MergedList(IList<T> origial)
        {
            IList<T> ret = new List<T>();
            foreach (T i in m_savedEntities)
            {
                ret.Add(i);
            }
            foreach (T i in m_updatedEntities)
            {
                ret.Add(i);
            }
            if (origial != null)
            {
                foreach (T i in origial)
                {
                    bool exist = false;
                    foreach (T j in ret)
                    {
                        if (object.ReferenceEquals(i, j))
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        exist = false;
                        foreach (T j in m_deletedEntities)
                        {
                            if (object.ReferenceEquals(i, j))
                            {
                                exist = true;
                                break;
                            }
                        }

                        if (!exist)
                        {
                            ret.Add(i);
                        }
                    }
                }
            }
            return ret;
        }
    }
}