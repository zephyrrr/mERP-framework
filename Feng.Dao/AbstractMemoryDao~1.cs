using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 内存操作Dao基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractMemoryDao<T> : AbstractEventDao<T>
        where T : class, IEntity
    {
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="entity"></param>
        public abstract void DoSave(T entity);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        public abstract void DoUpdate(T entity);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        public abstract void DoDelete(T entity);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(T entity)
        {
            //OnTransactionBeginning(new OperateArgs<T>(null, OperateType.Save, entity));

            OnEntityOperating(new OperateArgs<T>(null, OperateType.Save, entity));

            DoSave(entity);

            OnEntityOperated(new OperateArgs<T>(null, OperateType.Save, entity));

            //OnTransactionCommited(new OperateArgs<T>(null, OperateType.Save, entity));
        }

        /// <summary>
        /// 增加或修改
        /// </summary>
        /// <param name="entity"></param>
        public override void SaveOrUpdate(T entity)
        {
            IVersionedEntity ve = entity as IVersionedEntity;
            if (ve == null)
            {
                throw new NotSupportedException("SaveOrUpdate only support IVersionedEntity!");
            }
            if (ve.Version == 0)
            {
                Save(entity);
            }
            else
            {
                Update(entity);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(T entity)
        {
            //OnTransactionBeginning(new OperateArgs<T>(null, OperateType.Update, entity));

            OnEntityOperating(new OperateArgs<T>(null, OperateType.Update, entity));

            DoUpdate(entity);

            OnEntityOperated(new OperateArgs<T>(null, OperateType.Update, entity));

            //OnTransactionCommited(new OperateArgs<T>(null, OperateType.Update, entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(T entity)
        {
            //OnTransactionBeginning(new OperateArgs<T>(null, OperateType.Delete, entity));

            OnEntityOperating(new OperateArgs<T>(null, OperateType.Delete, entity));

            DoDelete(entity);

            OnEntityOperated(new OperateArgs<T>(null, OperateType.Delete, entity));

            //OnTransactionCommited(new OperateArgs<T>(null, OperateType.Delete, entity));
        }
    }
}
