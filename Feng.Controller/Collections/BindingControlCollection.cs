using System;
using System.Collections;
using System.Collections.Generic;

namespace Feng.Collections
{
    /// <summary>
    /// 绑定控件集合
    /// </summary>
    public class BindingControlCollection : ControlCollection<IBindingControl, IDisplayManager>, IBindingControlCollection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BindingControlCollection()
        {
        }

		/// <summary>
		/// 添加。
        /// 如父控制管理器不为空，则添加到控制管理器的<see cref="IControlManager.StateControls"/>中
		/// </summary>
        /// <param name="item"></param>
		public override void Add(IBindingControl item)
		{
            base.Add(item);
		}


        ///// <summary>
        ///// 绑定数据源
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        //public void SetDataBinding(object dataSource, string dataMember)
        //{
        //    foreach (IBindingControl item in this)
        //    {
        //        item.SetDataBinding(dataSource, dataMember);
        //    }
        //}

        /// <summary>
        /// 根据State设置控件状态
        /// </summary>
        public void SetState(StateType state)
        {
            foreach (IBindingControl item in this)
            {
                item.SetState(state);
            }
        }
    }
}
