namespace Feng
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// 
    /// </summary>
    public enum DataControlType
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 用于统计的Column，必须有子DetailGrid[0].
        /// CellViewManagerParam为显示内容，格式为 SUM: 收款金额 where = "$%费用项%="101"$"， where里面是表达式
        /// </summary>
        Stat = 3,
        /// <summary>
        /// 表达式
        /// </summary>
        Expression = 4,
        /// <summary>
        /// 不绑定
        /// </summary>
        Unbound = 7,
    }

	/// <summary>
	/// 需检查值并与实体类属性绑定的数据控件
	/// </summary>
	public interface IDataControl : IDataValueControl //, ICheckControl
	{
		/// <summary>
        /// 提示标题。如有label，在Label上显示。默认和对应Column一致。
		/// </summary>
		string Caption
		{
			get;
			set;
		}

        /// <summary>
        /// 顺序
        /// </summary>
        int Index
        {
            get;
            set;
        }
		
        ///// <summary>
        ///// 分组信息
        ///// </summary>
        //[Category("Data")]
        //[Description("分组信息")]
        //[DefaultValue(0)]
        //int Group
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// DataControl's Name.用于标识此DataControl，并用于Item[]形查找
        /// </summary>
        string Name
        {
            get;
            set;
        }

		/// <summary>
        /// 对应实体类属性名称
		/// </summary>
		string PropertyName
		{
			get;
			set;
		}

        /// <summary>
        /// 对应实体类属性导引
        /// </summary>
        string Navigator
        {
            get;
            set;
        }

        /// <summary>
        /// 返回类型
        /// </summary>
        Type ResultType
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        DataControlType ControlType
        {
            get;
            set;
        }

        ///// <summary>
        ///// 是否在插入操作时可用
        ///// </summary>
        //bool Insertable
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 是否在编辑操作时可用
        ///// </summary>
        //bool Editable
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 不可空
        /// </summary>
        bool NotNull
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// 是否用户选择可见（与Visible的关系是，Visible是系统控制的，Available是用户控制的）
        /// The Available property is different from the Visible property in that Available indicates whether the ToolStripItem is shown, while Visible indicates whether the ToolStripItem and its parent are shown.
        /// Grid.Column.Visible可用，当Parent不显示的时候，Visible还是true
        /// </summary>
        bool Available
        {
            get;
            set;
        }

        /// <summary>
        /// AvailableChanged
        /// </summary>
        event EventHandler AvailableChanged;

        /// <summary>
        /// Tag
        /// </summary>
        object Tag
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedDataValueChanged
        /// </summary>
        event EventHandler SelectedDataValueChanged;
	}
}
