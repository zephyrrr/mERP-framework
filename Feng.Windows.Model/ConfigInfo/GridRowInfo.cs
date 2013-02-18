using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 表格行定义数据，用于<see cref="T:Feng.Grid.DataBoundGrid"/>和<see cref="T:Feng.Grid.DataUnboundGrid"/>
    /// 关于表格定义的信息，有<see cref="GridInfo"/>、<see cref="GridColumnInfo"/>、<see cref="GridRowInfo"/>和<see cref="GridCellInfo"/>
    /// 表格定义的数据，也用于设置窗体上的<see cref="Feng.IDataControl"/>控件属性
    /// </summary>
    [Class(0, Name = "Feng.GridRowInfo", Table = "AD_Grid_Row", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridRowInfo : BaseADEntity
    {
        /// <summary>
        /// 表格名，和<see cref="WindowTabInfo.GridName"/>对应
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string GridName
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行是否只读的表达式，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public virtual string ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行是否可删除的表达式，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public virtual string AllowDelete
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行是否可编辑的表达式，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public virtual string AllowEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行是否显示的表达式，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行下属的DetailGrid是否只读的表达式，
        /// 格式为“GridName:<see cref="T:Feng.Permission"/>; ...”
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public virtual string DetailGridReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 表格行下属的DetailGrid是否可增加的表达式，
        /// 格式为“GridName:<see cref="T:Feng.Permission"/>; ...”
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public virtual string DetailGridAllowInsert
        {
            get;
            set;
        }

        /// <summary>
        /// 背景色的表达式，用Python语法
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string BackColor
        {
            get;
            set;
        }

        /// <summary>
        /// 前景色的表达式，用Python语法
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string ForeColor
        {
            get;
            set;
        }
    }
}