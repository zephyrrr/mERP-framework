using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 表格定义数据，用于<see cref="T:Feng.Grid.DataBoundGrid"/>和<see cref="T:Feng.Grid.DataUnboundGrid"/>
    /// 关于表格定义的信息，有<see cref="GridInfo"/>、<see cref="GridColumnInfo"/>、<see cref="GridRowInfo"/>和<see cref="GridCellInfo"/>
    /// 表格定义的数据，也用于设置窗体上的<see cref="Feng.IDataControl"/>控件属性
    /// </summary>
    [Class(0, Name = "Feng.GridInfo", Table = "AD_Grid", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class GridInfo : BaseADEntity
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
        /// 是否允许新增，格式见<see cref="T:Feng.Authority"/>
        /// 此新增为全局意义上的新增，同<see cref="P:IControlManager.AllowInsert"/>，包括手工（表格或者明细窗体）和程序（Add-EndEdit）
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string AllowInsert
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许编辑，格式见<see cref="T:Feng.Authority"/>
        /// 此新增为全局意义上的编辑，同<see cref="P:IControlManager.AllowEdit"/>，包括手工（表格或者明细窗体）和程序（Edit-EndEdit）
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string AllowEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许删除，格式见<see cref="T:Feng.Authority"/>
        /// 此新增为全局意义上的编辑，同<see cref="P:IControlManager.AllowDelete"/>，包括手工（表格或者明细窗体）和程序（Delete）
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string AllowDelete
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示新增按钮，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string AllowOperationInsert
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示编辑按钮，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string AllowOperationEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示删除按钮，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string AllowOperationDelete
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示批量编辑按钮，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string AllowExcelOperation
        {
            get;
            set;
        }
        
        #region "Only to Grid"

        /// <summary>
        /// 是否允许在表格中新增（通过InsertRow），格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255)]
        public virtual string AllowInnerInsert
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许在表格中直接编辑，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255)]
        public virtual string AllowInnerEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许在表格中通过右键菜单删除（在RowSelector上右键），格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255)]
        public virtual string AllowInnerDelete
        {
            get;
            set;
        }

        /// <summary>
        /// 表格SummaryRow的标题，格式见<see cref="P:GridColumnInfo.StateTitle"/>
        /// </summary>
        [Property(Length = 255)]
        public virtual string StatTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        [Property(Length = 255)]
        public virtual string ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// 此表格是否可见，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 此表格作为明细表格是否可见，格式见<see cref="T:Feng.Permission"/>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string VisibleAsDetail
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示搜索行
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string AllowInnerSearch
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示筛选行
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string AllowInnerFilter
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示Grid内置菜单
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string AllowInnerMenu
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示模糊文本筛选行
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string AllowInnerTextFilter
        {
            get;
            set;
        }
        #endregion
    }
}