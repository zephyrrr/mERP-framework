using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 从某一点到另一点的路径（途径至少一条路）
    /// </summary>
    [Class(Table = "AD_Direction", OptimisticLock = OptimisticLockMode.Version)]
    public class Direction : BaseADEntity
    {
        [Property(NotNull = true, Length = 50)]
        public virtual string StartAddress
        {
            get;
            set;
        }

        [Property(NotNull = true, Length = 50)]
        public virtual string EndAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 以Minute计
        /// </summary>
        [Property(NotNull = true)]
        public virtual int Time
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public virtual double Distance
        {
            get;
            set;
        }
    }
}
