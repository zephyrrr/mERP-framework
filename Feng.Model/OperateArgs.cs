using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 操作类别
    /// </summary>
    public enum OperateType
    {
        /// <summary>
        /// 
        /// </summary>
        Save = 1,
        /// <summary>
        /// 
        /// </summary>
        Update = 2,
        /// <summary>
        /// 
        /// </summary>
        Delete = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public class OperateArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="op"></param>
        /// <param name="entity"></param>
        public OperateArgs(IRepository rep, OperateType op, IEntity entity)
        {
            this.Repository = rep;
            this.OperateType = op;
            this.Entity = entity;
        }

        /// <summary>
        /// 
        /// </summary>
        public IRepository Repository { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public OperateType OperateType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IEntity Entity { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OperateArgs<T> : OperateArgs
        where T : class, IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="op"></param>
        /// <param name="entity"></param>
        public OperateArgs(IRepository rep, OperateType op, T entity)
            : base(rep, op, entity)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public new T Entity
        {
            get 
            {
                if (base.Entity == null)
                {
                    return null;
                }
                T newEntity = base.Entity as T;
                if (newEntity == null)
                {
                    throw new ArgumentException("OperateArgs's Entity should be " + typeof(T).ToString());
                }
                return newEntity; 
            }
        }
    }
}
