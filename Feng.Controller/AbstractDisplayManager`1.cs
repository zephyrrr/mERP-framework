using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using Feng.Collections;

namespace Feng
{
    /// <summary>
    /// DataSource为IList(T)的DisplayManager
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractDisplayManager<T> : AbstractDisplayManager, IDisplayManager<T>
        where T : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sm">查找管理器</param>
        protected AbstractDisplayManager(ISearchManager sm)
            : base(sm)
        {
        }

        private IEntityMetadata m_entityInfo;

        /// <summary>
        /// 实体类信息
        /// </summary>
        public override IEntityMetadata EntityInfo
        {
            get 
            {
                if (m_entityInfo == null)
                {
                    var m = ServiceProvider.GetService<IEntityMetadataGenerator>();
                    if (m != null)
                    {
                        m_entityInfo = m.GenerateEntityMetadata(typeof(T));
                    }
                }
                return m_entityInfo;
            }
        }

        /// <summary>
        /// 强类型实体类列表
        /// 注意：和Grid's DataRows中的列表顺序一致(Grid 排序不影响列表顺序)
        /// 因接口为非范型，只能返回object类型，实质为IList&lt;T&gt;
        /// </summary>
        public IList<T> Entities
        {
            get 
            {
                if (this.Items == null)
                    return null;
                IList<T> view = this.Items as IList<T>;
                if (view == null)
                {
                    throw new ArgumentException("in DisplayManager[T] Items should be DataView!");
                }
                return view; 
            }
        }

        ///// <summary>
        ///// 根据序号获得实体类
        ///// </summary>
        ///// <param name="idx"></param>
        //public override object GetItem(int idx)
        //{
        //    if (idx < 0 || idx >= this.Entities.Count)
        //    {
        //        throw new ArgumentException("Invalid index");
        //    }
        //    return this.Entities[idx];
        //}

        /// <summary>
        /// 强类型当前实体类
        /// </summary>
        public T CurrentEntity
        {
            get
            {
                if (this.CurrentItem == null)
                    return null;
                T rowView = this.CurrentItem as T;
                if (rowView == null)
                {
                    throw new ArgumentException("in DisplayManager[T] Item should be T!");
                }
                return rowView;
            }
        }
    }
}