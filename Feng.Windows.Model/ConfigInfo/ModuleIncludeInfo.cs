using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// Module包含的数据类型
    /// </summary>
    public enum ModuleIncludeType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// 参考数据（跟这个Module有关的数据定义）（自定义Table）
        /// </summary>
        ReferenceData = 1,
        /// <summary>
        /// ADInfo类型的数据
        /// </summary>
        ApplicationDictionaryData = 2,
        
        /// <summary>
        /// 新建Table
        /// </summary>
        DbTable = 11,
        /// <summary>
        /// 新建View
        /// </summary>
        DbView = 12,
        /// <summary>
        /// 新建Function
        /// </summary>
        DbFunction = 13,
        /// <summary>
        /// 新建Trigger
        /// </summary>
        DbTrigger = 14,
        /// <summary>
        /// 新建Procedure
        /// </summary>
        DbProcedure = 15,

        /// <summary>
        /// 源代码模型
        /// </summary>
        SourceModel = 21,
        /// <summary>
        /// 源代码脚本
        /// </summary>
        SourceScript = 22,
        /// <summary>
        /// 源代码Report
        /// </summary>
        SourceReport = 23
    }

    /// <summary>
    /// Module包含的数据
    /// </summary>
    [Class(0, Name = "Feng.ModuleIncludeInfo", Table = "AD_Module_Include", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ModuleIncludeInfo : BaseADEntity
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual ModuleIncludeType ModuleIncludeType
        {
            get;
            set;
        }

        /// <summary>
        /// 对应不同数据类型的参数
        /// </summary>
        [Property(NotNull = true, Length = 255)]
        public virtual string Params
        {
            get;
            set;
        }

        /// <summary>
        /// Module
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ModuleInclude_Module")]
        public virtual ModuleInfo Module
        {
            get;
            set;
        }
    }
}
