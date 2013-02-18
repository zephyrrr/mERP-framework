using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 报表配置信息。<see cref="P:ReportInfo.Name"/>和<see cref="P:WindowInfo.Name"/>对应，以读入相应Window的报表。
    /// 报表分Push模式(数据从Dataset来)和Pull模式(数据直接从数据库来)2种，对应于WindowInfo.WindowType的DataSetReport(5)和DatabaseReport(6)。
    /// 数据查找定义在<see cref="ReportDataInfo"/>.
    /// 在<see cref="ReportInfo.DatasetName"/>中提供dataset类型名。用通用的Display-Search模式提供数据，当搜索到数据后，填充到强类型的Dataset中。
    /// 另外一种是直接从数据库查询，数据Schema由数据库格式提供。在系统配置文件中ConnectionString中提供数据库链接信息
    /// 报表查询语句中含有参数，而查询控件用于填充参数。注意，报表查询语句中的参数为"@+FullPropertyName+Operator"，例如"@凭证号Eq"。
    /// </summary>
    [Class(0, Name = "Feng.ReportInfo", Table = "AD_Report", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ReportInfo : BaseADEntity
    {
        /// <summary>
        /// 报表文件类型（内嵌资源形，用报表类型名）
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string ReportDocument
        {
            get;
            set;
        }

        /// <summary>
        /// 当使用Dataset模式时，指定Datsset模版
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string DatasetName
        {
            get;
            set;
        }

        /// <summary>
        /// 当在编辑界面打印时，用的默认搜索条件
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string SearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 报表创建后，需执行的特殊的处理
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Report_Process")]
        public virtual ProcessInfo AfterProcess
        {
            get;
            set;
        }

        ///// <summary>
        ///// 报表创建后，需执行的特殊的处理
        ///// </summary>
        //[Property(Column = "AfterProcessId", NotNull = false)]
        //public virtual long? AfterProcessId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 子级WindowMenu(通过数据库读取，为LazyLoad的Proxy)
        /// </summary>
        [Bag(0, Cascade = "none", Inverse = true, OrderBy = "SeqNo")]
        [Key(1, Column = "Report")]
        [Index(2, Column = "SeqNo", Type = "int")]
        [OneToMany(3, ClassType = typeof(ReportDataInfo), NotFound = NotFoundMode.Ignore)]
        public virtual IList<ReportDataInfo> WindowMenus
        {
            get;
            set;
        }


        /// <summary>
        /// 对应窗体
        /// </summary>
        [OneToOne(Cascade = "none", Constrained = false, ForeignKey = "FK_Report_Window")]
        public virtual WindowInfo Window
        {
            get;
            set;
        }
    }
}
