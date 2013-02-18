using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 多客户部门实体
    /// </summary>
    public interface IMultiOrgEntity : IEntity
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        long ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门Id
        /// </summary>
        long OrgId
        {
            get;
            set;
        }
    }
}
