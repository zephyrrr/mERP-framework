using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 月报表
    /// </summary>
    [Class(0, Name = "Feng.MonthReportInfo", Table = "SD_Month_Report", OptimisticLock = OptimisticLockMode.Version)]
    public class MonthReportInfo : BaseDataEntity,
        IMasterEntity<MonthReportInfo, MonthReportDataInfo>
    {
        #region "Interface"
        IList<MonthReportDataInfo> IMasterEntity<MonthReportInfo, MonthReportDataInfo>.DetailEntities
        {
            get { return Reports; }
            set { Reports = value; }
        }
        #endregion

        /// <summary>
        /// 具体月报表数据
        /// </summary>
        [Bag(0, Cascade = "none", Inverse = true)]
        [Key(1, Column = "MonthReport")]
        [OneToMany(2, ClassType = typeof(MonthReportDataInfo), NotFound = NotFoundMode.Ignore)]
        public virtual IList<MonthReportDataInfo> Reports
        {
            get;
            set;
        }

        /// <summary>
        /// 报表日期起
        /// </summary>
        [Property(NotNull = true)]
        public virtual DateTime 报表日期起
        {
            get;
            set;
        }


        private DateTime m_报表日期止;
        /// <summary>
        /// 报表日期止
        /// </summary>
        [Property(NotNull = true)]
        public virtual DateTime 报表日期止
        {
            get { return m_报表日期止; }
            set { m_报表日期止 = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59); }
        }

        /// <summary>
        /// 产生报表日期的日期
        /// </summary>
        [Property(NotNull = true)]
        public virtual DateTime 报表日期
        {
            get;
            set;
        }
    }
}
