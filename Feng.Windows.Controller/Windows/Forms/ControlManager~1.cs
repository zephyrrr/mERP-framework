using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 控制管理器。可对多个实体类操作，进行增加、修改、删除等操作。
    /// 列表顺序不控制，需手工控制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ControlManager<T> : ControlManager, IWindowControlManager<T>
        where T : class, IEntity, new()
    {
        /// <summary>
        /// Constructor
        /// 创建控件集合
        /// </summary>
        /// <param name="sm"></param>
        public ControlManager(ISearchManager sm)
            : base(new DisplayManager<T>(sm))
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
        /// 创建新Entity
        /// </summary>
        /// <returns></returns>
        protected override object AddNewItem()
        {
            T entity = new T();
            this.DisplayManager.Items.Add(entity);
            return entity;
        }

        /// <summary>
        /// 创建新数据源
        /// </summary>
        /// <returns></returns>
        protected override IList NewList()
        {
            return new List<T>();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            ControlManager<T> cm = new ControlManager<T>(this.DisplayManager.SearchManager.Clone() as ISearchManager);
            Copy(this, cm);
            return cm;
        }

    }
}