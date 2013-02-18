using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 记录创建和修改用户和时间的记录
    /// </summary>
    internal abstract class LogEntity_NoAttribute : IVersionedEntity, ILogEntity
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public virtual int Version
        {
            get;
            set;
        }

        /// <summary>
        /// 创建者
        /// </summary>
        public virtual string CreatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// 修改者
        /// </summary>
        public virtual string UpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? Updated
        {
            get;
            set;
        }
    }
}
