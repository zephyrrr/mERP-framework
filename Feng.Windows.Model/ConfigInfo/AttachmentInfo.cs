using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 附件信息
    /// </summary>
    [Class(0, Name = "Feng.AttachmentInfo", Table = "SD_Attachment", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class AttachmentInfo : BaseDataEntity
    {
        /// <summary>
        /// 实体类名称
        /// </summary>
        [Property(NotNull = true, Length = 255)]
        public virtual string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// 实体类Id（类型不一定是string，以string方式保存）
        /// </summary>
        [Property(NotNull = true, Length = 255)]
        public virtual string EntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 具体文件数据
        /// </summary>
        [Property(NotNull = false, Length = int.MaxValue)]
        public virtual byte[] Data
        {
            get;
            set;
        }
    }
}
