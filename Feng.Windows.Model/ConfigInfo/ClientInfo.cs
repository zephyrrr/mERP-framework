using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 客户信息
    /// </summary>
    [Class(0, Name = "Feng.ClientInfo", Table = "AD_Client", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ClientInfo : BaseDataEntity, IActivableEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Property(NotNull = true, Length = 200)]
        public virtual string Name
        {
            get;
            set;
        }
    }
}
