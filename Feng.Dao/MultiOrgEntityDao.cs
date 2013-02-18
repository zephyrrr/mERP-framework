using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// MultiOrgEntityDao
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiOrgEntityDao<T> : LogEntityDao<T>
        where T : class, IMultiOrgEntity, ILogEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MultiOrgEntityDao()
        {
            this.EntityOperating += new EventHandler<OperateArgs<T>>(MultiOrgEntityDao_EntityOperating);
        }

        void MultiOrgEntityDao_EntityOperating(object sender, OperateArgs<T> e)
        {
            switch (e.OperateType)
            {
                case OperateType.Save:
                    e.Entity.ClientId = SystemConfiguration.ClientId;
                    e.Entity.OrgId = SystemConfiguration.OrgId;
                    break;
                case OperateType.Update:
                    break;
                case OperateType.Delete:
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }
    }
}
