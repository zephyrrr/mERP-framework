using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 资源依靠信息
    /// </summary>
    [Class(0, Name = "Feng.ResourceDependencyInfo", Table = "AD_Resource_Dependency", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ResourceDependencyInfo : BaseADEntity
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string ResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 依靠的资源名称
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string DependentResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 顺序
        /// </summary>
        [Property(NotNull = true)]
        public virtual int SeqNo
        {
            get;
            set;
        }
    }
}
