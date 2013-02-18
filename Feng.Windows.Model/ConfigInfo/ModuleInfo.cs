using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// Module信息
    /// </summary>
    [Class(0, Name = "Feng.ModuleInfo", Table = "AD_Module", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ModuleInfo : BaseADEntity
    {
        /// <summary>
        /// 作者
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string Author
        {
            get;
            set;
        }

        /// <summary>
        /// Module版本
        /// </summary>
        [Property(NotNull = true, Length = 10)]
        public virtual string ModuleVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 此Module包含的数据信息
        /// </summary>
        [Bag(0, Cascade = "none", Inverse = true)]
        [Key(1, Column = "Module")]
        [OneToMany(2, ClassType = typeof(ModuleIncludeInfo), NotFound = NotFoundMode.Ignore)]
        public virtual IList<ModuleIncludeInfo> Include
        {
            get;
            set;
        }

        /// <summary>
        /// Module数据（以压缩文档存储）
        /// </summary>
        [Property(NotNull = true, Length = int.MaxValue)]
        public virtual byte[] ModuleData
        {
            get;
            set;
        }
    }
}
