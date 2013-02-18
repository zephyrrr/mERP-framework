using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 控制管理器。可对多个实体类操作，进行增加、修改、删除等操作。
    /// 类表顺序由<see cref="BindingSource"/>控制
    /// </summary>
    public class ControlManagerBindingSource<T> : ControlManagerBindingSource, IWindowControlManager<T>
        where T : class, IEntity, new()
    {
        /// <summary>
        /// Constructor
        ///  创建控件集合
        /// </summary>
        /// <param name="sm"></param>
        public ControlManagerBindingSource(ISearchManager sm)
            : base(new DisplayManagerBindingSource<T>(sm))
        {
            
        }

        /// <summary>
        /// Typed DisplayManager
        /// </summary>
        public IDisplayManager<T> DisplayManagerT
        {
            get { return this.DisplayManager as DisplayManager<T>; }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ControlManagerBindingSource<T> cm = new ControlManagerBindingSource<T>(this.DisplayManager.SearchManager.Clone() as ISearchManager);
            Copy(this, cm);
            return cm;
        }
    }
}