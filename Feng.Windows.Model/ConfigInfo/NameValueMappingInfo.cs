using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 系统内部DataTable缓存信息（用于各种数据源，例如ComboBox，Grid.CellViewManager）
    /// 并可从ValueMember找到DisplayMember，例如可从员工编号找到员工姓名
    /// </summary>
    [Class(0, Name = "Feng.NameValueMappingInfo", Table = "AD_NameValueMapping", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class NameValueMappingInfo : BaseADEntity
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// 实际值，例如员工表中的编号
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string ValueMember
        {
            get;
            set;
        }

        /// <summary>
        /// 显示值，例如员工表中的姓名
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string DisplayMember
        {
            get;
            set;
        }

        /// <summary>
        /// 表中除了<see cref="ValueMember"/>和<see cref="DisplayMember"/>外其他需要读入的字段，以','分隔
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string OtherMembers
        {
            get;
            set;
        }

        /// <summary>
        /// 如果<see cref="ParentName"/>为空，为读入数据表时Select语句的Where部分；
        /// 如果不为空，则为筛选父数据时的条件
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string WhereClause
        {
            get;
            set;
        }

        ///// <summary>
        ///// 读入数据表时Select语句的Where部分中要作为参数的部分，以","分割。可在程序中指定参数值。
        ///// </summary>
        //[Property(Length = 255, NotNull = false)]
        //public virtual string WhereParams
        //{
        //    get;
        //    set;
        //}
        
        /// <summary>
        /// 显示时控制是否显示，例如在ComboBox中。以';'分隔，格式为"Member0:true(false);Member1:true(false)"
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string MemberVisible
        {
            get;
            set;
        }

        /// <summary>
        /// 编辑这个数据表要采取的动作
        /// </summary>
        [ManyToOne(ForeignKey = "FK_NameValueMapping_Action")]
        public virtual ActionInfo EditAction
        {
            get;
            set;
        }

        ///// <summary>
        ///// 编辑这个数据表要采取的动作
        ///// </summary>
        //[Property(Column = "EditActionId", NotNull = false)]
        //public virtual long? EditActionId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 父NameValueMapping名称。如果不为空，则不从数据库读入数据，而是直接从父NameValueMapping中经过筛选（通过<see cref="WhereClause"/>）得到数据
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string ParentName
        {
            get;
            set;
        }
    }
}
