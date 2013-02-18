using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 简单参数表，及名称-值对应的参数表（Name-Value）
    /// </summary>
    [Class(0, Name = "Feng.SimpleParamInfo", Table = "AD_SimpleParam", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class SimpleParamInfo : BaseADEntity
    {
        /// <summary>
        /// 值
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Value
        {
            get;
            set;
        }
    }
}
