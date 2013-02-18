using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 用于创建复杂参数
    /// </summary>
    [Class(0, Name = "Feng.ParamCreatorInfo", Table = "AD_Param_Creator", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ParamCreatorInfo : BaseADEntity
    {
        /// <summary>
        /// 参数名
        /// </summary>
        [Property(Length = 50, NotNull = true)]
        public virtual string ParamName
        {
            get;
            set;
        }

        /// <summary>
        /// 参数值
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string ParamValue
        {
            get;
            set;
        }

        /// <summary>
        /// 参数类型
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 权限（对某个用户是A参数，对某个用户是B参数）
        /// </summary>
        [Property(Length = 50, NotNull = true)]
        public virtual string Permission
        {
            get;
            set;
        }

        /// <summary>
        /// 序号
        /// </summary>
        [Property(NotNull = true)]
        public virtual int SeqNo
        {
            get;
            set;
        }
    }
}
