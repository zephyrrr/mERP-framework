using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 月报表数据
    /// </summary>
    [Class(0, Name = "Feng.MonthReportDataInfo", Table = "SD_Month_Report_Data", OptimisticLock = OptimisticLockMode.Version)]
    public class MonthReportDataInfo : BaseDataEntity,
        IDetailEntity<MonthReportInfo, MonthReportDataInfo>
    {
        #region "Interface"
        MonthReportInfo IDetailEntity<MonthReportInfo, MonthReportDataInfo>.MasterEntity
        {
            get { return MonthReport; }
            set { MonthReport = value; }
        }
        #endregion

        /// <summary>
        /// 月报表
        /// </summary>
        [ManyToOne(NotNull = true, ForeignKey = "FK_MonthReportData_MonthReport")]
        public virtual MonthReportInfo MonthReport
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 数据。以pdf格式存储
        /// </summary>
        [Property(Length = int.MaxValue, NotNull = true)]
        public virtual byte[] Data
        {
            get;
            set;
        }
    }
}
