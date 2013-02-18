using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// Module相互之间的依赖关系
    /// </summary>
    [Class(0, Name = "Feng.ModuleDependencyInfo", Table = "AD_Module_Dependency", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ModuleDependencyInfo : BaseADEntity
    {
        /// <summary>
        /// Module（Module依靠DependentModule）
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ModuleDependency_Module")]
        public virtual ModuleInfo Module
        {
            get;
            set;
        }

        /// <summary>
        /// 依靠的Module（需要版本在StartVersion-EndVersion之间）
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ModuleDependency_DependentModule")]
        public virtual ModuleInfo DependentModule
        {
            get;
            set;
        }

        /// <summary>
        /// 依靠的Module的初始版本
        /// </summary>
        [Property(NotNull = true, Length = 10)]
        public virtual string StartVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 依靠的Module的结束版本
        /// </summary>
        [Property(NotNull = false, Length = 10)]
        public virtual string EndVersion
        {
            get;
            set;
        }
    }
}
