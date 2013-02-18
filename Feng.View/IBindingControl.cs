using System.ComponentModel;
using System;

namespace Feng
{
	/// <summary>
    /// 绑定控件集合
	/// </summary>
	public interface IBindingControl : IStateControl
	{
		/// <summary>
		/// 设置数据源
		/// </summary>
		/// <param name="dataSource"></param>
		/// <param name="dataMember"></param>
		void SetDataBinding(object dataSource, string dataMember);
	}

    /// <summary>
    /// 
    /// </summary>
    public interface ICanAddItemBindingControl
    {
        /// <summary>
        /// 增加一个数据项
        /// </summary>
        /// <param name="dataItem"></param>
        void AddDateItem(object dataItem);
    }
}
