using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NHibernateBll<T> : IBaseBll<T>
        where T : class, IEntity
    {
        /// <summary>
        /// 创建数据访问层接口
        /// </summary>
        /// <returns></returns>
        protected override IBaseDal CreateDal()
        {
            return new NHibernateDal<T>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected NHibernateBll()
        {
            m_dal = CreateDal();
        }

        private IBaseDal m_dal;

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public void Save(object entity)
        {
            this.Save((T)entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public void Update(object entity)
        {
            this.Update((T)entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(object entity)
        {
            this.Delete((T)entity);
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Save(T entity)
        {
            m_dal.Save(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            m_dal.Update(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            m_dal.Delete(entity);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public virtual void Clear()
        {
        }
    }
}