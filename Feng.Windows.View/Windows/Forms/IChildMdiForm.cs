using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.DockingWindows;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 用于<see cref="TabbedMdiForm"/>的子窗体
    /// </summary>
    public interface IChildMdiForm : IForm
    {
        ///// <summary>
        ///// 在搜索窗体显示的控件
        ///// </summary>
        //Control SearchPanel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        void SetCustomProperty(string propertyName, Func<object> propertyValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        void SetCustomProperty(string propertyName, object propertyValue);

        object GetCustomProperty(string propertyName, bool useCreator = true);

        bool Visible
        {
            get;
            set;
        }

        event EventHandler VisibleChanged;
    }
}