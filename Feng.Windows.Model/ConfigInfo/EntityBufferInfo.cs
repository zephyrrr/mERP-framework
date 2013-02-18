using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 实体类缓存信息。缓存信息见<see cref="T:Feng.IEntityBuffer"/>。
    /// 用于建立一个程序范围内的Id-Entity形的缓存
    /// </summary>
    [Class(0, Name = "Feng.EntityBufferInfo", Table = "AD_EntityBuffer", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class EntityBufferInfo : BaseADEntity
    {
        /// <summary>
        /// 实体类名称
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// 要缓存的实体类类名
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string PersistentClass
        {
            get;
            set;
        }

        /// <summary>
        /// 实体类Id类型名
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string IdTypeName
        {
            get;
            set;
        }
    }
}
