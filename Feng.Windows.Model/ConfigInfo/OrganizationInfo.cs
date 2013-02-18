using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 部门信息（当前不可用）
    /// </summary>
    [Class(0, Name = "Feng.OrganizationInfo", Table = "AD_Org", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class OrganizationInfo : BaseDataEntity, IActivableEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Property(NotNull = true, Length = 200)]
        public virtual string LoginRoleName
        {
            get;
            set;
        }
    }
}
