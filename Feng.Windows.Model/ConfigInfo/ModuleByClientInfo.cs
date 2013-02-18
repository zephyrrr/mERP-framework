using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// Client所拥有的Module
    /// </summary>
    [Class(0, Name = "Feng.ModuleByClientInfo", Table = "AD_Module_Client", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ModuleByClientInfo : BaseADEntity
    {
        /// <summary>
        /// Module
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ModuleByClient_Module")]
        public virtual ModuleInfo Module
        {
            get;
            set;
        }

        /// <summary>
        /// 此Module被拥有的Client
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ModuleByClient_Client")]
        public virtual ClientInfo ModuleByClient
        {
            get;
            set;
        }
    }
}
