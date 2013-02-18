using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// Org所拥有的Module
    /// </summary>
    [Class(0, Name = "Feng.ModuleByOrgInfo", Table = "AD_Module_Org", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ModuleByOrgInfo : BaseADEntity
    {
        /// <summary>
        /// Module
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ModuleByOrg_Module")]
        public virtual ModuleInfo Module
        {
            get;
            set;
        }

        /// <summary>
        /// 此Module被拥有的Organization
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ModuleByOrg_Org")]
        public virtual OrganizationInfo ModuleByOrg
        {
            get;
            set;
        }
    }
}
