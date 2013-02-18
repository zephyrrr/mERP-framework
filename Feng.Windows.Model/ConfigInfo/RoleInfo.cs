using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 角色信息（当前不可用）
    /// </summary>
    [Class(0, Name = "Feng.RoleInfo", Table = "AD_Role", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class RoleInfo : BaseADEntity
    {
        /// <summary>
        /// 客户列表
        /// </summary>
        [Property(Length = 2000, NotNull = true)]
        public virtual string ClientList
        {
            get;
            set;
        }

        /// <summary>
        /// 部门列表
        /// </summary>
        [Property(Length = 2000, NotNull = true)]
        public virtual string OrgList
        {
            get;
            set;
        }
    }
}
