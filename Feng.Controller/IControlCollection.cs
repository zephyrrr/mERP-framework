using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public interface IControlCollection<T, S> : IList<T>, IEnumerable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        void AddRange(IEnumerable<T> controls);

        /// <summary>
        /// 
        /// </summary>
        S ParentManager
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IBindingControlCollection : IControlCollection<IBindingControl, IDisplayManager>, IStateControl
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDataControlCollection : IControlCollection<IDataControl, IDisplayManager>
    {
        /// <summary>
        /// 根据FullProeprtyName得到IDataControl
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDataControl this[string name]
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        void FocusFirstInsertableControl();

        /// <summary>
        /// 
        /// </summary>
        void FocusFirstEditableControl();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISearchControlCollection : IControlCollection<ISearchControl, ISearchManager>
    {
        /// <summary>
        /// 根据FullProeprtyName得到ISearchControl
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ISearchControl this[string name]
        {
            get;
        }
    }

    /// <summary>
    /// 状态控件集合
    /// </summary>
    public interface IStateControlCollection : IControlCollection<IStateControl, IControlManager>, IStateControl
    {
    }

    /// <summary>
    /// 查找控件集合
    /// </summary>
    public interface ICheckControlCollection : IControlCollection<ICheckControl, IControlManager>
    {
        /// <summary>
        /// 检查控件
        /// </summary>
        void CheckControlsValue();
    }
}
