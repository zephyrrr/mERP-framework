using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class DisplayManager : AbstractDisplayManager
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sm"></param>
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
            AddDateItemToBindingControls(this.BindingControls, dataItem);
        }

        /// <summary>
        /// 设置数据绑定。
        /// 最后的数据源为DataView
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
            }
            else
            {
                if (dataSource is DataView)
                {
                    m_dv = dataSource as DataView;
                }
                else if (dataSource is DataTable)
                {
                    m_dv = (dataSource as DataTable).DefaultView;
                }
                else if (dataSource is DataSet)
                {
                    m_dv = (dataSource as DataSet).Tables[dataMember].DefaultView;
                }
                else if (dataSource is IList)
                {
                    m_dv = (dataSource as IList);
                }
                else
                {
                    throw new NotSupportedException("dataSource in DisplayManager must be DataTable, DataView, DataSet or IList!");
                }

                if (m_dv.Count == 0)
                {
                    this.Position = -1;
                }
                else
                {
                    this.Position = 0;
                }
            }

            SetBindingControlsData(this.BindingControls, dataSource, dataMember);
        }

        private IList m_dv;
        /// <summary>
        /// Items
        /// </summary>
        public override IList Items
        {
            get { return m_dv; }
        }
        //private delegate void SetDataBindingDelegate(object dataSource, string dataMember);

        internal static void SetBindingControlsData(IBindingControlCollection bcs, object dataSource, string dataMember)
        {
            foreach (IBindingControl bc in bcs)
            {
                //System.ComponentModel.ISynchronizeInvoke syncControl = bc as System.ComponentModel.ISynchronizeInvoke;
                //if (syncControl != null && syncControl.InvokeRequired)
                //{
                //    syncControl.Invoke(new SetDataBindingDelegate(bc.SetDataBinding), new object[] { dataSource, dataMember });
                //}
                //else
                //{
                    bc.SetDataBinding(dataSource, dataMember);
                //}
            }
        }

        internal static void AddDateItemToBindingControls(IBindingControlCollection bcs, object dataItem)
        {
            foreach (IBindingControl bc in bcs)
            {
                ICanAddItemBindingControl aibc = bc as ICanAddItemBindingControl;
                if (aibc != null)
                {
                    aibc.AddDateItem(dataItem);
                }
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DisplayManager dm = new DisplayManager(this.SearchManager.Clone() as ISearchManager);
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