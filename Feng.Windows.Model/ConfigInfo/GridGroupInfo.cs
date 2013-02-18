using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 表格分组信息（把表格数据按照某列分组）
    /// </summary>
    [Class(0, Name = "Feng.GridGroupInfo", Table = "AD_Grid_Group", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridGroupInfo : BaseADEntity
    {
        /// <summary>
        /// 表格名，用以读入相应表格分组信息
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GridName
        {
            get;
            set;
        }

        /// <summary>
        /// 分组依据，和<see cref="GridColumnInfo.GridColumnName"/>对应
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GroupBy
        {
            get;
            set;
        }

        /// <summary>
        /// 分组GroupRow显示标题格式。格式见<see cref="GridColumnInfo.StatTitle"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string TitleFormat
        {
            get;
            set;
        }

        /// <summary>
        /// 分组顺序
        /// </summary>
        [Property(NotNull = true)]
        public virtual int SeqNo
        {
            get;
            set;
        }

        /// <summary>
        /// 默认是否展开
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool Collapsed
        {
            get;
            set;
        }
    }
}
