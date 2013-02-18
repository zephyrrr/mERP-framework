using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 普通窗体类型
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// 无效。( = 0)
        /// </summary>
        None = 0,
        /// <summary>
        /// 配置信息操作窗体，对应<see cref="T:Feng.Windows.Forms.ArchiveOperationForm"/>。( = 1)
        /// </summary>
        Maintain = 1,       // ArchiveOperationForm(ArchiveUnboundGrid)
        /// <summary>
        /// 普通数据操作窗体，对应<see cref="T:Feng.Windows.Forms.ArchiveOperationForm"/>。( = 2)
        /// </summary>
        Transaction = 2,    // ArchiveOperationForm(ArchiveUnboundGrid)
        /// <summary>
        /// 数据查找窗体，对应<see cref="T:Feng.Windows.Forms.ArchiveSeeForm"/>。( = 3)
        /// </summary>
        Query = 3,           // ArchiveSeeForm(DataUnboundGrid)
        /// <summary>
        /// 数据选择窗体，对应<see cref="T:Feng.Windows.Forms.ArchiveCheckForm"/>。( = 4)
        /// </summary>
        Select = 4,          // ArchiveCheckForm
        /// <summary>
        /// 查询报告。根据查询出来的数据生成报表。( = 6)
        /// </summary>
        DataSetReport = 5,
        /// <summary>
        /// 查询报告。根据查询条件填充报表参数，直接从数据库查询。( = 6)
        /// </summary>
        DatabaseReport = 6,
        /// <summary>
        /// 选择窗体，需先选择打开哪个窗体
        /// </summary>
        SelectWindow = 7,
        /// <summary>
        /// 普通数据操作窗体，但是以DetailForm的形式
        /// </summary>
        DetailTransaction = 8,
        /// <summary>
        /// 数据窗体
        /// </summary>
        DataControl = 9, 
        /// <summary>
        /// Excel类型操作窗体，对应<see cref="T:Feng.Windows.Forms.ArchiveExcelForm"/>。( = 10)
        /// </summary>
        ExcelOperation = 10,
        /// <summary>
        /// 普通数据操作窗体，对应<see cref="T:Feng.Windows.Forms.ArchiveOperationForm"/>。( = 2)
        /// </summary>
        TransactionBound = 12,    // ArchiveOperationForm(BoundGrid)
        /// <summary>
        /// 数据查找窗体，对应<see cref="T:Feng.Windows.Forms.ArchiveSeeForm"/>。( = 3)
        /// </summary>
        QueryBound = 13,           // ArchiveSeeForm(BoundGrid)
    }

    /// <summary>
    /// 窗体主-子时显示关系
    /// </summary>
    [System.Flags]
    public enum WindowMasterDetailState
    {
        /// <summary>
        /// 显示主窗体
        /// </summary>
        EnableViewMaster = 1,
        /// <summary>
        /// 显示子窗体
        /// </summary>
        EnableViewDetail = 2,
    }

    /// <summary>
    /// 普通窗口数据定义
    /// </summary>
    [Class(0, Name = "Feng.WindowInfo", Table = "AD_Window", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class WindowInfo : BaseADEntity
    {
        /// <summary>
        /// 窗体标题
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 窗体类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual WindowType WindowType
        {
            get;
            set;
        }

        /// <summary>
        /// 窗体图标
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string ImageName
        {
            get;
            set;
        }

        /// <summary>
        /// 打开权限，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Access
        {
            get;
            set;
        }

        /// <summary>
        /// 是否需要产生明细窗体<see cref="T:Feng.Windows.Forms.ArchiveDetailForm"/>
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool GenerateDetailForm
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义明细特殊窗体<see cref="FormInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Window_Detail_Form")]
        public virtual FormInfo DetailForm
        {
            get;
            set;
        }

        ///// <summary>
        ///// 自定义明细特殊窗体<see cref="FormInfo"/>Id
        ///// </summary>
        //[Property(Column = "DetailFormId", NotNull = false)]
        //public virtual long? DetailFormId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 自定义明细普通窗体<see cref="WindowInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Window_Detail_Window")]
        public virtual WindowInfo DetailWindow
        {
            get;
            set;
        }

        ///// <summary>
        ///// 自定义明细普通窗体<see cref="WindowInfo"/>Id
        ///// </summary>
        //[Property(Column = "DetailWindowId", NotNull = false)]
        //public virtual long? DetailWindowId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 窗体打开后，自动执行的过程
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Window_Process", NotNull = false)]
        public virtual ProcessInfo AutoProcess
        {
            get;
            set;
        }

        ///// <summary>
        ///// 窗体打开后，自动执行的过程Id
        ///// </summary>
        //[Property(Column = "AutoProcessId", NotNull = false)]
        //public virtual long? AutoProcessId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Window创建后触发的事件，与EventInfo.EventName对应
        /// </summary>
        [Property(Column = "EventInitialized", NotNull = false)]
        public virtual string EventInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// 子级WindowTab(通过数据库读取，为LazyLoad的Proxy)
        /// </summary>
        [Bag(0, Cascade = "none", Inverse = true, OrderBy = "SeqNo")]
        [Key(1, Column = "Window")]
        [Index(2, Column = "SeqNo", Type = "int")]
        [OneToMany(3, ClassType = typeof(WindowTabInfo), NotFound = NotFoundMode.Ignore)]
        public virtual IList<WindowTabInfo> WindowTabs
        {
            get;
            set;
        }

        /// <summary>
        /// 子级WindowMenu(通过数据库读取，为LazyLoad的Proxy)
        /// </summary>
        [Bag(0, Cascade = "none", Inverse = true, OrderBy = "SeqNo")]
        [Key(1, Column = "Window")]
        [Index(2, Column = "SeqNo", Type = "int")]
        [OneToMany(3, ClassType = typeof(WindowTabInfo), NotFound = NotFoundMode.Ignore)]
        public virtual IList<WindowMenuInfo> WindowMenus
        {
            get;
            set;
        }

        /// <summary>
        /// 附件EntityId，如果为空则表示Attachment按钮不显示
        /// </summary>
        [Property(Column = "AttachmentId", NotNull = false)]
        public virtual string AttachmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 窗体状态
        /// </summary>
        [Property(NotNull = false)]
        public virtual WindowMasterDetailState? WindowState
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示菜单项和工具栏项
        /// </summary>
        [Property(NotNull = false)]
        public virtual bool? ShowMenu
        {
            get;
            set;
        }
    }
}
