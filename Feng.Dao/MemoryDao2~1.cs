using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 配合<see cref="MasterDao2{T, S}"/>的MemoryDao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    [Obsolete("Too complex, just use MemoryDao")]
    public class MemoryDao2<T, S> : MemoryDao<S>
        where T : class, IEntity
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// Save(实际上是Update操作）
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(S entity)
        {
            base.Update(entity);
        }

        /// <summary>
        /// Update(Invalid in MasterDao2)
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(S entity)
        {
            throw new InvalidOperationException("there is no update in MemoryDao2!");
        }

        /// <summary>
        /// Delete(实际上是Update操作）
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(S entity)
        {
            for (int i = 0; i < base.UpdatedEntities.Count; ++i)
            {
                if (object.ReferenceEquals(base.UpdatedEntities[i], entity))
                {
                    base.UpdatedEntities.RemoveAt(i);
                    return;
                }
            }

            entity.MasterEntity = null;
            base.Update(entity);
        }
    }
}
