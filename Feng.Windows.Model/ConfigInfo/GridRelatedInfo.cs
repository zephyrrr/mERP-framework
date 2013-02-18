using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 相关信息类型
    /// </summary>
    public enum GridRelatedType
    {
        /// <summary>
        /// 不合理（=0）
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 按照选定行转到其他窗体（=1）
        /// </summary>
        ByRows = 1,
        /// <summary>
        /// 按照搜索条件转到其他窗体（=2）
        /// </summary>
        BySearchExpression = 2,
        /// <summary>
        /// 根据数据控件状态（=3）
        /// </summary>
        ByDataControl = 3,
        /// <summary>
        /// 无条件，直接转到（=9）
        /// </summary>
        ByNone = 9
    }

    /// <summary>
    /// 表格转到其他窗体所用信息，用于程序-相关页，也可用于<see cref="T:Feng.Windows.GridGotoFormToolStripItem"/>
    /// </summary>
    [Class(0, Name = "Feng.GridRelatedInfo", Table = "AD_Grid_Related", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridRelatedInfo : BaseADEntity
    {
        /// <summary>
        /// 显示标题
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 表格名，用以读入相应表格相关信息
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

        /// <summary>
        /// 是否显示，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 相关信息类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual GridRelatedType RelatedType
        {
            get;
            set;
        }

        /// <summary>
        /// 如果是按照选定行搜索，指定的搜索表达式。当执行动作打开窗体后，按照此搜索表达式进行搜索。格式见<see cref="Feng.SearchExpression"/>
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string SearchExpression
        {
            get;
            set;
        }

        ///// <summary>
        ///// 如果是按照选定行搜索，对应的表格行的内在实体类类型。如果选择的表格行实体类类型和此处设定的不同，则不显示。
        ///// </summary>
        //[Property(Length = 100, NotNull = false)]
        //public virtual string EntityType
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 表格级别（用于有明细表格时，此时顶级为0，第二层第一个Grid为0.0，第二个为0.1
        /// </summary>
        [Property(NotNull = false)]
        public virtual string GridLevel
        {
            get;
            set;
        }

        ///// <summary>
        ///// 要转到的详细窗体
        ///// </summary>
        //[Property(NotNull = false, Length = 100)]
        //public virtual string ToDetailForm
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 点击后要执行的动作的，目前支持Window和Form
        /// </summary>
        [ManyToOne(ForeignKey = "FK_GridRelated_Action")]
        public virtual ActionInfo Action
        {
            get;
            set;
        }

        ///// <summary>
        ///// 点击后要执行的动作的<see cref="P:Feng.ActionInfo.Name"/>，目前支持Window和Form
        ///// </summary>
        //[Property(Column = "ActionId", NotNull = true)]
        //public virtual long ActionId
        //{
        //    get;
        //    set;
        //}
    }
}