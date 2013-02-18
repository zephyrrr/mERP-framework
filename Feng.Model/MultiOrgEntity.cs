using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 带客户部门的实体类
    /// </summary>
    internal abstract class MultiOrgEntity_NoAttribute : LogEntity_NoAttribute, IActivableEntity, IMultiOrgEntity
    {
        /// <summary>
        /// 是否处于激活状态。当记录停用时，在系统里一般不再进入可选
        /// </summary>
        public virtual bool IsActive
        {
            get;
            set;
        }

        ///// <summary>
        ///// 相关客户，同<see cref="ClientId"/>
        ///// </summary>
        //[ManyToOne(Column = "ClientId", Insert = false, Update = false)]
        //public virtual ClientInfo Client
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 此记录属于的客户Id。
        /// 系统支持多客户组织。客户对应为公司，组织对应为部门，记录只能同客户的操作，同一客户内，部门通过不同权限操作记录。
        /// 目前无用，默认为0
        /// </summary>
        public virtual long ClientId
        {
            get;
            set;
        }

        ///// <summary>
        ///// 相关组织，同<see cref="OrgId"/>
        ///// </summary>
        //[ManyToOne(Column = "OrgId", Insert = false, Update = false)]
        //public virtual OrganizationInfo Org
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 此记录属于的组织Id。
        /// 客户组织的关系见<see cref="ClientId"/>
        /// </summary>
        public virtual long OrgId
        {
            get;
            set;
        }
    }
}
