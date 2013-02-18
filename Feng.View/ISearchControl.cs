using System.Collections;
using System.Collections.Generic;
using System;


namespace Feng
{
	/// <summary>
	/// 查询控件接口
	/// </summary>
    public interface ISearchControl : IReadOnlyControl
	{
        /// <summary>
        /// 填充查询条件
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrder"></param>
        void FillSearchConditions(IList<ISearchExpression> searchExpression, IList<ISearchOrder> searchOrder);

         /// <summary>
         /// 标题
         /// </summary>
        string Caption
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
        /// 和PropertyName一起的的导航名
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
        /// 名字，用于标识此SearchControl
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 当查找时用的不是Navigator.PropertyName，而是特殊字段时
        /// </summary>
        string SpecialPropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 附加的查询条件
        /// </summary>
        string AdditionalSearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        bool SearchNullUseFull
        {
            get;
            set;
        }

        /// <summary>
        /// 是否取反
        /// </summary>
        bool IsNot
        {
            get;
            set;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        bool IsNull
        {
            get;
            set;
        }

        /// <summary>
        /// 查询顺序
        /// </summary>
        bool? Order
        {
            get;
            set;
        }

        /// <summary>
        /// 是否采用模糊搜索。
        /// 为空则按照默认条件，1条的时候模糊，多条的时候不模糊
        /// </summary>
        bool? UseFuzzySearch
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可以选择模糊搜索
        /// </summary>
        bool CanSelectFuzzySearch
        {
            get;
            set;
        }

		/// <summary>
		/// 查询控件选中值
		/// </summary>
		IList SelectedDataValues
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
        ///  是否用户选择可见（与Visible的关系是，Visible是系统控制的，Available是用户控制的）
        /// The Available property is different from the Visible property in that Available indicates whether the ToolStripItem is shown, while Visible indicates whether the ToolStripItem and its parent are shown.
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
        /// 顺序
        /// </summary>
        int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Tag
        /// </summary>
        object Tag
        {
            get;
            set;
        }
	}
}
