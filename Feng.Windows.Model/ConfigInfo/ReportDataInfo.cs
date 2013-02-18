using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 报表数据配置信息
    /// </summary>
    [Class(0, Name = "Feng.ReportDataInfo", Table = "AD_Report_Data", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ReportDataInfo : BaseADEntity
    {
        ///// <summary>
        ///// 报表名
        ///// </summary>
        //[Property(Length = 100, NotNull = true)]
        //public virtual string ReportName
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 报表名
        /// </summary>
        [ManyToOne(ForeignKey = "FK_ReportData_Report")]
        public virtual ReportInfo Report
        {
            get;
            set;
        }

        /// <summary>
        /// 报表Dataset中的Table名称
        /// </summary>
        [Property(NotNull = true, Length = 50)]
        public virtual string DatasetTableName
        {
            get;
            set;
        }

        /// <summary>
        /// 查找管理器名。具体设置见<see cref="WindowTabInfo.SearchManagerClassName"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string SearchManagerClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 查找管理器<see cref="SearchManagerClassName"/>初始化参数。多个参数用','分隔
        /// </summary>
        [Property(Length = 2000, NotNull = false)]
        public virtual string SearchManagerClassParams
        {
            get;
            set;
        }

        /// <summary>
        /// 表格配置名（用于转换编号-名称）
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GridName
        {
            get;
            set;
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Property(NotNull = true)]
        public virtual int SeqNo
        {
            get;
            set;
        }
    }
}
