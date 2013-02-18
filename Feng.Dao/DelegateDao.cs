using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// DelegateDaoOperation
    /// </summary>
    /// <param name="entity"></param>
    public delegate void DelegateDaoOperation(object entity);

    /// <summary>
    /// 采用自定义Process的Dao
    /// </summary>
    public class DelegateDao : EmptyDao
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="save"></param>
        /// <param name="update"></param>
        /// <param name="delete"></param>
        public DelegateDao(DelegateDaoOperation save, DelegateDaoOperation update, DelegateDaoOperation delete)
        {
            Initialize(save, update, delete);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DelegateDao()
        {
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="save"></param>
        /// <param name="update"></param>
        /// <param name="delete"></param>
        public void Initialize(DelegateDaoOperation save, DelegateDaoOperation update, DelegateDaoOperation delete)
        {
            m_save = save;
            m_update = update;
            m_delete = delete;
        }

        private DelegateDaoOperation m_save, m_update, m_delete;

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(object entity) 
        {
            if (m_save != null)
            {
                m_save(entity);
            }
            else
            {
                throw new ArgumentException("There is no save delegate defined!");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(object entity) 
        {
            if (m_update != null)
            {
                m_update(entity);
            }
            else
            {
                throw new ArgumentException("There is no update delegate defined!");
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(object entity) 
        {
            if (m_delete != null)
            {
                m_delete(entity);
            }
            else
            {
                throw new ArgumentException("There is no delete delegate defined!");
            }
        }
    }
}
