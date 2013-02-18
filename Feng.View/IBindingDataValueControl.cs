using System;
using System.Collections.Generic;
using System.Text;
using Feng;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface INameValueMappingBindingControl : IBindingDataValueControl
    {
        /// <summary>
        /// View，Editor状态有不同DataBinding
        /// </summary>
        /// <param name="nvcName">NameValueMappingCollection名字</param>
        /// <param name="viewerName"></param>
        /// <param name="editorName"></param>
        /// <param name="editorFilter"></param>
        void SetDataBinding(string nvcName, string viewerName, string editorName, string editorFilter);

        /// <summary>
        /// 针对NameValueMapping的Name
        /// </summary>
        string NameValueMappingName
        {
            get;
        }
    }

	/// <summary>
	/// 可与数据源绑定，而且可读取控件值的接口
	/// </summary>
	public interface IBindingDataValueControl : IBindingControl, IDataValueControl
	{
		/// <summary>
        /// 获取或设置一个属性，该属性将用作<see cref="IBindingDataValueControl"/> 中的项的实际值。
		/// </summary>
		string ValueMember
		{
			get;
			set;
		}

		/// <summary>
        /// 获取或设置要为此 <see cref="IBindingDataValueControl"/> 显示的属性。
		/// </summary>
		string DisplayMember
		{
			get;
			set;
		}

        /// <summary>
        /// 刷新数据
        /// </summary>
        void ReloadData();
	}

    ///// <summary>
    ///// 当刷新时候进行的动作的Delegate
    ///// </summary>
    //public delegate void DataBindingRefreshAction();
}
