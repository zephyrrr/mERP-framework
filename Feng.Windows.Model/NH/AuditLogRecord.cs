using System;
using System.Collections.Generic;

namespace Feng
{
#if !NMA
    using NHibernate.Mapping.Attributes;

    /// <summary>
    /// 审计日志信息
    /// </summary>
    [Class(Name = "Feng.AuditLogRecord", Table = "SD_AuditLogRecord")]
    public class AuditLogRecord : BaseDataEntity
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        [Property(Length = 50, NotNull = true)]
        public virtual string LogType
        {
            get;
            set;
        }

        [Property(Length = 255, NotNull = false)]
        public virtual string EntityName
        {
            get;
            set;
        }

        [Property(Length = 255, NotNull = false)]
        public virtual string EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 日志消息
        /// </summary>
        [Property(Length = 4000, NotNull = false)]
        public virtual string Message
        {
            get;
            set;
        }
#else
    /// <summary>
    /// 审计日志信息
    /// </summary>
    public class AuditLogRecord : IEntity
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public virtual string LogType
        {
            get;
            set;
        }

        public virtual string EntityName
        {
            get;
            set;
        }

        public virtual string EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 日志消息
        /// </summary>
        public virtual string Message
        {
            get;
            set;
        }
#endif

    } 
}
