using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 理论路线
    /// </summary>
    [Class(Table = "AD_Route", OptimisticLock = OptimisticLockMode.Version)]
    public class Route : BaseADEntity
    {
        /// <summary>
        /// 用于自动为作业划定理论路线
        /// </summary>
        [Property(NotNull = false, Length = 255)]
        public virtual string RouteKey
        {
            get;
            set;
        }

        /// <summary>
        /// 表达式类型的路径，只用于配置。配置完后经过计算得到实际路径。
        /// 实际路径是正则表达式子集(有AND OR 关系），可用符号有()|, AND用-连接
        /// </summary>
        [Property(NotNull = false, Length = 255)]
        public virtual string DirectionExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 自动生成
        /// </summary>
        [Property(NotNull = false, Length = 255)]
        public virtual string DirectionReal
        {
            get;
            set;
        }

        /// <summary>
        /// 路径例子（用于计算时间）
        /// </summary>
        [Property(NotNull = false)]
        public virtual long? SampleTrack
        {
            get;
            set;
        }

        /// <summary>
        /// 自动生成
        /// </summary>
        [Property(NotNull = false, Length = int.MaxValue)]
        public virtual byte[] SampleTrackData
        {
            get;
            set;
        }

        /// <summary>
        /// 总时间（minutes）
        /// </summary>
        [Property(NotNull = false)]
        public virtual int? Time
        {
            get;
            set;
        }

        /// <summary>
        /// 中距离（km）
        /// </summary>
        [Property(NotNull = false)]
        public virtual double? Distance
        {
            get;
            set;
        }
    }
}
