using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// LogEntity的默认Dao（设置Created，Updated，***）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogEntityDao<T> : BaseDao<T>
        where T : class, ILogEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogEntityDao()
        {
            this.EntityOperating += new EventHandler<OperateArgs<T>>(BaseDao_EntityOperating);
        }

        void BaseDao_EntityOperating(object sender, OperateArgs<T> e)
        {
            switch (e.OperateType)
            {
                case OperateType.Save:
                    e.Entity.Created = System.DateTime.Now;
                    e.Entity.CreatedBy = SystemConfiguration.UserName;
                    break;
                case OperateType.Update:
                    e.Entity.Updated = System.DateTime.Now;
                    e.Entity.UpdatedBy = SystemConfiguration.UserName;
                    break;
                case OperateType.Delete:
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }
    }
}
