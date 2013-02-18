using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 配合<see cref="MasterDao{T,S}"/>使用的MemoryDao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MasterDetailMemoryDao<T, S> : AbstractMemoryDao<S>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
    {
        private IEntityList m_cm;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cm"></param>
        public MasterDetailMemoryDao(IEntityList cm)
        {
            m_cm = cm;

            this.EntityOperated += new EventHandler<OperateArgs<S>>(MasterDetailMemoryDao_EntityOperated);
        }

        void MasterDetailMemoryDao_EntityOperated(object sender, OperateArgs<S> e)
        {
            switch (e.OperateType)
            {
                case OperateType.Save:
                    e.Entity.MasterEntity = m_cm.CurrentItem as T;
                    break;
            }
        }

        /// <summary>
        /// Save(Do Nothing)
        /// </summary>
        /// <param name="entity"></param>
        public override void DoSave(S entity)
        {
        }

        /// <summary>
        /// Update(Do Nothing)
        /// </summary>
        /// <param name="entity"></param>
        public override void DoUpdate(S entity)
        {
        }

        /// <summary>
        /// Delete(Do Nothing)
        /// </summary>
        /// <param name="entity"></param>
        public override void DoDelete(S entity)
        {
        }
    }
}
