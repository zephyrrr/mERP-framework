using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 窗口内Tab数据定义。窗口<see cref="WindowInfo"/>必定有且只有一个子<see cref="WindowTabInfo"/>,
    /// <see cref="WindowTabInfo"/>可以有多个子<see cref="WindowTabInfo"/>。目前实际用到3级<see cref="WindowTabInfo"/>。
    /// </summary>
    [Class(0, Name = "Feng.WindowTabInfo", Table = "AD_Window_Tab", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class WindowTabInfo : BaseADEntity
    {
        /// <summary>
        /// 标题。如果是第二级，用于在Tab控件中显示为TabPage标题
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        ///// <summary>
        ///// 级别。最顶级为0
        ///// 目前支持第二级
        ///// </summary>
        //[Property(NotNull = true)]
        //public virtual int TabLevel
        //{
        //    get;
        //    set;
        //}

        //private IList<WindowTabInfo> m_childs = new List<WindowTabInfo>();
        ///// <summary>
        ///// 子级TabInfo，用于缓存程序自动生成。
        ///// 为了提高效率，<see cref="WindowTabInfo"/>为一次性读入，然后程序根据<see cref="Parent"/>来自动生成父子关系，
        ///// 而不是通过数据库的外键和NHibernate自动读取子菜单。
        ///// </summary>
        //public virtual IList<WindowTabInfo> Childs
        //{
        //    get { return m_childs; }
        //}

        /// <summary>
        /// 子级菜单(通过数据库读取，为LazyLoad的Proxy)
        /// </summary>
        [Bag(0, Cascade = "none", Inverse = true, OrderBy = "SeqNo")]
        [Key(1, Column = "Parent")]
        [Index(2, Column = "SeqNo", Type = "int")]
        [OneToMany(3, ClassType = typeof(WindowTabInfo), NotFound = NotFoundMode.Ignore)]
        public virtual IList<WindowTabInfo> ChildTabs
        {
            get;
            set;
        }

        /// <summary>
        /// 父级<see cref="WindowTabInfo"/>，用于确定Tab的父子关系
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Tab_Parent")]
        public virtual WindowTabInfo Parent
        {
            get;
            set;
        }

        ///// <summary>
        ///// 父级TabInfo Id，用于确定Tab的父子关系
        ///// </summary>
        //[Property(Column = "ParentId", NotNull = false)]
        //public virtual long? ParentId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 是否在主表格中显示。
        /// TabInfo默认在主表格和明细窗体中显示。
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool IsInGrid
        {
            get;
            set;
        }

        /// <summary>
        /// 是否在明细窗体中显示。
        /// TabInfo默认在主表格和明细窗体中显示。
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool IsInDetailForm
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

        /// <summary>
        /// 查找管理器附加查询条件
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string WhereClause
        {
            get;
            set;
        }

        /// <summary>
        /// 查找管理器附加查询顺序
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string OrderByClause
        {
            get;
            set;
        }

        /// <summary>
        /// 此TabInfo属于的<see cref="WindowInfo"/>
        /// </summary>
        [ManyToOne(ForeignKey = "FK_TAB_WINDOW")]
        public virtual WindowInfo Window
        {
            get;
            set;
        }

        ///// <summary>
        ///// 此TabInfo属于的<see cref="WindowInfo"/>
        ///// </summary>
        //[Property(Column = "WindowId", NotNull = false)]
        //public virtual long WindowId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 实体类类名（用于ControlManager，DisplayManager，SearchManager，BusinessLayer简易配置）
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string ModelClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 控制管理器<see cref="T:Feng.IControlManager"/>类名，用于在普通窗体中生成操作控制类。
        /// 和<see cref="T:DisplayManagerClassName"/>二选一。
        /// 目前支持:
        /// <list type="bullet">
        /// <item>WindowForm: "Feng.Windows.Forms.ControlManager`1[[T]], Feng.Windows.Controller", 其中T为Entity全名，例如Example.Prodcut, Example.Model</item>
        /// <item>WindowForm: "Feng.Windows.Forms.DisplayManager, Feng.Windows.Controller"</item>
        /// <item>TYPED, 泛型CM，用于NHibernate Entity</item>
        /// <item>UNTYPED，数据表CM，用于System.Data.DataTable</item>
        /// </list>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string ControlManagerClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 显示管理器<see cref="T:Feng.IDisplayManager"/>类名，用于在报表窗体中生成显示控制类。
        /// 和<see cref="T:ControlManagerClassName"/>二选一。
        /// 目前支持:
        /// <list type="bullet">
        /// <item>WindowForm(根据Entity查询): "Feng.Windows.Forms.DisplayManager`1[[T]], Feng.Windows.Controller", 其中T为Entity全名，例如Example.Prodcut, Example.Model</item>
        /// <item>WindowForm(根据数据表查询): "Feng.Windows.Forms.DisplayManager, Feng.Windows.Controller"</item>
        /// <item>TYPED, 泛型CM，用于NHibernate Entity</item>
        /// <item>UNTYPED，数据表CM，用于System.Data.DataTable</item>
        /// </list>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string DisplayManagerClassName
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="ControlManagerClassName"/>或<see cref="DisplayManagerClassName"/>对应的查找管理器<see cref="T:Feng.ISearchManager"/>类名。
        /// <list type="bullet">
        /// <item>根据Entity查询: "Feng.NH.SearchManager`1[[T]], Feng.Controller", 其中T为Entity全名，例如Example.Prodcut, Example.Model</item>
        /// <item>根据数据表（视图）查询: "Feng.Data.SearchManager, Feng.Controller"。
        ///  需在<see cref="SearchManagerClassParams"/>需要填写查询条件，例如"视图查询_出纳票据本期", "Id", "SELECT 票据类型, COUNT(*) AS 数量,源, SUM(金额) AS 金额, 币制", "GROUP BY 票据类型, 源,币制"。
        ///  最多4个数据，分别为数据表名，默认排序列，查询语句，Group语句。用','分隔。如不需要，后2个数据可不填写。</item>
        /// <item>根据数据库函数查询: Feng.Data.SearchManagerFunction, Feng.Controller。
        /// 需在<see cref="SearchManagerClassParams"/>需要填写查询条件，例如"函数查询_凭证(@凭证号Like)", "Id"。
        ///  分别为数据库函数名（包含参数名），默认排序列。</item>
        ///  <item>根据数据库存储过程查询: Feng.Data.SearchManagerProcedure, Feng.Controller。
        /// 需在<see cref="SearchManagerClassParams"/>需要填写查询条件，例如"过程查询_固定资产折旧"，
        ///  为数据库过程名（不用包含参数名）。
        ///  关于函数和过程中用到的参数，如果为Le，Ge，Lt，Gt，则定义方式为 @ + 属性名 + 操作符；
        ///  如果是Eq，Like，GInG，InG，则定义方式为 @ + 属性名。其他操作符不支持。</item>
        /// <item>主从关系中，根据Master查询Detail: "Feng.NH.SearchManagerProxyDetailInMaster`2[[T],[S]], Feng.Controller"。
        /// 其中T为MasterEntity，例如Example.Order, Example.Model；S为DetailEntity，例如Example.OrderLine, Example.Model。
        /// MasterEntity 必须实现 IMasterEntity{T, S}接口；DetailEntity必须实现IDetailEntity{T, S}接口。</item>
        /// <item>主从关系中，根据Detail查询Master: "Feng.NH.SearchManagerProxyManytoOne`2[[S], [T]], Feng.Controller"
        /// 其中T为MasterEntity，例如Example.Order, Example.Model；S为DetailEntity，例如Example.OrderLine, Example.Model。
        /// MasterEntity 必须实现 IMasterEntity{T, S}接口；DetailEntity必须实现IDetailEntity{T, S}接口。</item>
        /// <item>OneToOne关系中，根据OneParent查询OneChild: "Feng.NH.SearchManagerProxyOneToOne`2[[T],[S]], Feng.Controller"
        /// OneParent 必须实现 IOnetoOneParentEntity{T, S}接口；OneChild必须实现IOnetoOneChildEntity{T, S}接口。</item>
        /// <item>OneToOne关系中，根据OneChild查询OneParent: "Feng.NH.SearchManagerProxyOneToOneChild`2[[S],[T]], Feng.Controller"
        /// OneParent 必须实现 IOnetoOneParentEntity{T, S}接口；OneChild必须实现IOnetoOneChildEntity{T, S}接口。</item>
        /// <item>同一记录: "Feng.NH.SearchManagerProxyOneToSame`1[[T]], Feng.Controller"</item>
        /// <item>根据查询条件查询: "Feng.NH.SearchManagerProxyDetail`2[[S],[T]], Feng.Controller"
        /// 需在<see cref="SearchManagerClassParams"/>需要填写查询条件，例如"票:Id = %票.Name%"，格式类似于<see cref="T:Feng.SearchExpression"/>，%%内为变量，根据主Entity会自动填充</item>
        /// <item>UNTYPED：简单数据库查询</item>
        /// <item>UNTYPEDPROCEDURE：数据库Procedure查询</item>
        /// <item>UNTYPEDFUNCTION：数据库Function查询</item>
        /// <item>UNTYPEDGROUPBYDETAIL：数据库GroupBy语句查询</item>
        /// <item>UNTYPEDDETAILINMASTER：数据库查询，用于Detail in Master</item>
        /// <item>TYPED：简单NHibernate查询</item>
        /// <item>TYPEDDETAIL：NH查询，用于Detail in Master</item>
        /// <item>TYPEDONETOSAME：同样的Entity</item>
        /// </list>
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
        /// 业务层管理器<see cref="T:Feng.IDao"/>类名。只对普通窗体有用。只有当<see cref="ControlManagerClassName"/>不为空时有效。
        /// 根据Entity类型，可为"Feng.LogEntityDao`1[[T]], Feng.Controller", "Feng.MultiOrgEntityDao`1[[T]], Feng.Controller"等。
        /// 一般为自定义一个系统基本Dao，如有需要，根据Entity写各自独立Dao。
        /// 针对各种关系查询，有
        /// <list type="bullet">
        /// <item>主从关系: "Feng.MasterDao`2[[T],[S]], Feng.Controller",<see cref="BusinessLayerClassParams"/>为DetailEntity的Dao。</item>
        /// <item>主从关系2: "Feng.MasterDao2`2[[T],[S]], Feng.Controller""</item>
        /// <item>OneToOne关系: "Feng.OnetoOneDao`2[[T],[S]], Feng.Controller</item>
        /// <item>OneToOne关系:</item>
        /// <item>ManyToOne关系: Feng.ManytoOneDao`2[[T],[S]], Feng.Controller</item>
        /// <item>无直接关系: "Feng.MasterNoRelationDao`2[[T],[S]], Feng.Controller"</item>
        /// </list>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string BusinessLayerClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 业务层管理器<see cref="BusinessLayerClassName"/>初始化参数。
        /// 目前只针对关联型<see cref="T:Feng.IDao"/>（例如<see cref="T:Feng.MasterDao"/>)，参数必须是另外一个<see cref="T:Feng.IBaseDao"/>。
        /// </summary>
        [Property(Length = 2000, NotNull = false)]
        public virtual string BusinessLayerClassParams
        {
            get;
            set;
        }

        /// <summary>
        /// Repository配置名（可为空，用默认）
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string RepositoryConfigName
        {
            get;
            set;
        }

        /// <summary>
        /// 主表格对应的表格名。用于对应
        /// <see cref="GridInfo"/>、<see cref="GridColumnInfo"/>、<see cref="GridRowInfo"/>和<see cref="GridCellInfo"/>；
        /// <see cref="GridFilterInfo"/>、<see cref="GridRelatedInfo"/>、<see cref="GridGroupInfo"/>；
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GridName
        {
            get;
            set;
        }

        // 搞不定！
        ///// <summary>
        ///// 
        ///// </summary>
        //[Bag(0, Cascade = "none", Inverse = true, Table="AD_Grid")]
        //[Key(1, Column = "GridName", PropertyRef="GridName")]
        ////[OneToMany(2, ClassType = typeof(GridInfo), NotFound = NotFoundMode.Ignore)]
        //[ManyToMany(2, Column = "GridName", ClassType = typeof(GridInfo), NotFound = NotFoundMode.Ignore)]
        //public virtual IList<GridInfo> GridInfos
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ////[Bag(0, Cascade = "none", Inverse = true, Table = "AD_Grid_Column")]
        ////[Key(1, Column = "GridName", PropertyRef = "GridName")]
        ////[ManyToMany(2, Column = "GridName", ClassType = typeof(GridColumnInfo), NotFound = NotFoundMode.Ignore)]
        //public virtual IList<GridColumnInfo> GridColumnInfos
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ////[Bag(0, Cascade = "none", Inverse = true, Table = "AD_Grid_Row")]
        ////[Key(1, Column = "GridName", PropertyRef = "GridName")]
        ////[ManyToMany(2, Column = "GridName", ClassType = typeof(GridRowInfo), NotFound = NotFoundMode.Ignore)]
        //public virtual IList<GridRowInfo> GridRowInfos
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ////[Bag(0, Cascade = "none", Inverse = true, Table = "AD_Grid_Cell")]
        ////[Key(1, Column = "GridName", PropertyRef = "GridName")]
        ////[ManyToMany(2, Column = "GridName", ClassType = typeof(GridCellInfo), NotFound = NotFoundMode.Ignore)]
        //public virtual IList<GridCellInfo> GridCellInfos
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// SelectedDataValueChanged时执行的Event Process
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string SelectedDataValueChanged
        {
            get;
            set;
        }

        /// <summary>
        /// PositionChanged时执行的Event Process
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string PositionChanged
        {
            get;
            set;
        }
    }
}
