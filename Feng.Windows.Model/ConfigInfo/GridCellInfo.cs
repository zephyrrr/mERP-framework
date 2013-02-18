using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 单元格定义数据,用于<see cref="T:Grid.DataBoundGrid"/>和<see cref="T:Grid.DataUnboundGrid"/>的。
    /// 关于表格定义的信息，有<see cref="GridInfo"/>、<see cref="GridColumnInfo"/>、<see cref="GridRowInfo"/>和<see cref="GridCellInfo"/>
    /// 表格定义的数据，也用于设置窗体上的<see cref="Feng.IDataControl"/>控件属性
    /// </summary>
    [Class(0, Name = "Feng.GridCellInfo", Table = "AD_Grid_Cell", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridCellInfo : BaseADEntity
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
        /// 要配置的单元格。和<see cref="GridColumnInfo.GridColumnName"/>对应
        /// </summary>
        [Property(Column = "FullPropertyName", Length = 255, NotNull = true)]
        public virtual string GridColumName
        {
            get;
            set;
        }

        /// <summary>
        /// 单元格是否只读的表达式，格式见<see cref="T:Feng.Permission"/>
        /// 只有True时才会设置，False时默认（同上一级别一样）。
        /// GridColumn, GridCell, GridRow 任一为 True则为True，当GridColumn为True，GridCell为False时总体也为True。
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 单元格编辑时是否要求非空的表达式。如表达式结果为True，则编辑时会检查单元格值是否为空，如果为空则有错误提示。
        /// 格式见<see cref="T:Feng.Permission"/>。
        /// 关于优先级，见<see cref="ReadOnly"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string NotNull
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