using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 表格相关地址信息
    /// </summary>
    [Class(0, Name = "Feng.GridRelatedAddressInfo", Table = "AD_Grid_Related_Address", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridRelatedAddressInfo : BaseADEntity
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
        /// 表格级别（用于有明细表格时，此时顶级为0）
        /// </summary>
        [Property(NotNull = true)]
        public virtual string GridLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Action地址
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Address
        {
            get;
            set;
        }
    }
}
