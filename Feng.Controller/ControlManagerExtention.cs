using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng
{
    /// <summary>
    /// ControlManagerExtention
    /// </summary>
    public static class ControlManagerExtention
    {
        /// <summary>
        /// OnCurrentItemChanged
        /// </summary>
        /// <param name="cm"></param>
        public static void OnCurrentItemChanged(this IControlManager cm)
        {
            cm.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, cm.DisplayManager.Position));
        }

        /// <summary>
        /// 粘贴数据控件值到当前数据控件
        /// </summary>
        public static void Paste(this IControlManager cm, Dictionary<string, object> values)
        {
            foreach (IDataControl dc in cm.DisplayManager.DataControls)
            {
                if (values.ContainsKey(dc.Name))
                {
                    dc.SelectedDataValue = values[dc.Name];
                }
            }
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        public static void UpdateControlsState(this IControlManager cm)
        {
            if (cm.State == StateType.Delete)
            {
                return;
            }

            foreach (IDataControl dc in cm.DisplayManager.DataControls)
            {
                dc.SetState(cm.State);
            }

            cm.StateControls.SetState(cm.State);
        }

        /// <summary>
        /// 保存各个数据控件的值到缓存
        /// </summary>
        public static Dictionary<string, object> Copy(this IDisplayManager dm)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Clear();

            foreach (IDataControl dc in dm.DataControls)
            {
                if (dc.SelectedDataValue == null)
                    continue;
                dict.Add(dc.Name, dc.SelectedDataValue);
            }
            return dict;
        }
    }
}
