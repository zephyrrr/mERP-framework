using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisplayManager<T> : AbstractDisplayManager<T>
        where T : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchManager"></param>
        public DisplayManager(ISearchManager sm)
            : base(sm)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataItem"></param>
        public override void AddDataItem(object dataItem)
        {
            DisplayManager.AddDateItemToBindingControls(this.BindingControls, dataItem);
        }

        /// <summary>
        /// 设置数据绑定。
        /// 目前dataSource必须为IList&lt;T&gt;类型, dataMamber无用"/>
        /// 设置完数据源后，State为<see cref="StateType.View"/>
        /// </summary>
        /// <param name="dataSource">数据源</param>
        /// <param name="dataMember">数据成员</param>
        /// <exception cref="NotSupportedException">数据源不符合格式时抛出</exception>
        public override void SetDataBinding(object dataSource, string dataMember)
        {
            if (dataSource == null)
            {
                this.Position = -1;
                DisplayManager.SetBindingControlsData(this.BindingControls, null, null);
            }
            else
            {
                List<T> ret = dataSource as List<T>;
                if (ret == null)
                {
                    ret = new List<T>();

                    System.Collections.IList list = dataSource as System.Collections.IList;
                    Debug.Assert(list != null, "dataSource in DisplayManager<T> must be IList!");

                    foreach (object i in list)
                    {
                        if (i is T)
                        {
                            ret.Add(i as T);
                        }
                        else
                        {
                            // 有时候，不知道DataSource具体什么类型的，例如A有基类B，C，查出来数据只知道是A，但是有2个DisplayManger
                            // 分别是DM<B>, DM<C>， 要分别显示的，此时如果是B，则DM<C>里显示空记录。 例如 进口_信息汇总
                            //Debug.Assert(false, "dataSource must be type " + typeof(T).ToString() + ", but actually is " + i.GetType().ToString());
                        }
                    }
                }

                m_entities = ret;

                if (m_entities.Count == 0)
                {
                    this.Position = -1;
                }
                else
                {
                    this.Position = 0;
                }

                DisplayManager.SetBindingControlsData(this.BindingControls, m_entities, string.Empty);
            }
        }

        private List<T> m_entities;
        /// <summary>
        /// Items
        /// </summary>
        public override IList Items
        {
            get { return m_entities; }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DisplayManager<T> dm = new DisplayManager<T>(this.SearchManager.Clone() as ISearchManager);
            Copy(this, dm);
            return dm;
        }

        public override void DisplayCurrent()
        {
            base.DisplayCurrent();

            foreach (IDataControl dc in this.DataControls)
            {
                switch (dc.ControlType)
                {
                    case DataControlType.Expression:
                        if (string.IsNullOrEmpty(dc.PropertyName))
                            continue;
                        if (dc.PropertyName.Contains("%"))
                        {
                        }
                        else
                        {
                            dc.SelectedDataValue = Feng.Windows.Utils.ProcessInfoHelper.TryExecutePython(dc.PropertyName,
                                new Dictionary<string, object>() { { "entity", this.CurrentItem } });
                        }
                        break;
                }
            }
        }
    }
}