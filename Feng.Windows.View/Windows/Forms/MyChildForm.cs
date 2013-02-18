using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xceed.DockingWindows;
using Xceed.Grid;


namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyForm
    /// </summary>
    public partial class MyChildForm : MyForm, IChildMdiForm
    {
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                foreach(var i in m_customPropertiesDisposable)
                {
                    i.Dispose();
                }

                m_properties.Clear();
                m_propertiesCreator.Clear();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyChildForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            if (base.DesignMode)
            {
                return;
            }

            // SetMenuPermissions();

            base.Form_Load(sender, e);
        }

        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            ILayoutControl lc = this.GetCustomProperty(SearchPanelName, false) as ILayoutControl;
            if (lc != null)
            {
                lc.SaveLayout();
            }

            base.Form_Closing(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        protected void UpdateStatus(string status)
        {
            IWinFormApplication app = ServiceProvider.GetService<IApplication>() as IWinFormApplication;
            if (app != null)
            {
                app.UpdateStatus(status);
            }
        }

        private Dictionary<string, Func<object>> m_propertiesCreator = new Dictionary<string, Func<object>>();
        private Dictionary<string, object> m_properties = new Dictionary<string, object>();
        ///// <summary>
        ///// 各种属性
        ///// </summary>
        //public Dictionary<string, object> Properties
        //{
        //    get { return m_properties; }
        //}
        /// <summary>
        /// GetCustomProperty
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetCustomProperty(string propertyName, bool useCreator = true)
        {
            if (m_properties.ContainsKey(propertyName))
                return m_properties[propertyName];

            if (useCreator && m_propertiesCreator.ContainsKey(propertyName))
            {
                var r = m_propertiesCreator[propertyName]();
                IDisposable d = r as IDisposable;
                if (d != null)
                {
                    m_customPropertiesDisposable.Add(d);
                }
                m_properties[propertyName] = r;
                return r;
            }
            else
            {
                return null;
            }
        }

        private List<IDisposable> m_customPropertiesDisposable = new List<IDisposable>();
        /// <summary>
        /// SetCustomProperty
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public void SetCustomProperty(string propertyName, Func<object> propertyValue)
        {
            m_propertiesCreator[propertyName] = propertyValue;
        }
        public void SetCustomProperty(string propertyName, object propertyValue)
        {
            m_properties[propertyName] = propertyValue;
        }

        public const string SearchPanelName = "SearchPanel";
        /// <summary>
        /// 在搜索窗体显示的控件
        /// </summary>
        public Control GetSearchPanel()
        {
            return GetCustomProperty(SearchPanelName) as Control; 
        }
        public void SetSearchPanel(Func<Control> propertyValue)
        {
            SetCustomProperty(SearchPanelName, propertyValue);
        }

        public const string GridRelatedPanelName = "GridRelatedControl";
        /// <summary>
        /// 在搜索窗体显示的控件
        /// </summary>
        public Control GetGridRelatedPanel()
        {
            return GetCustomProperty(GridRelatedPanelName) as Control;
        }
        public void SetGridRelatedPanel(Func<Control> propertyValue)
        {
            SetCustomProperty(GridRelatedPanelName, propertyValue);
        }
        //#region "Menu Permission"

        ///// <summary>
        ///// 设置菜单和工具条权限等信息
        ///// </summary>
        //private void SetMenuPermissions()
        //{
        //    IDictionary<string, ToolStripMenuItem> dict = GetToolStripMenuItems();
        //    foreach (KeyValuePair<string, ToolStripMenuItem> kvp in dict)
        //    {
        //        MenuPropertyInfo info = ADInfoBll.Instance.GetMenuPropertyInfo(kvp.Key);
        //        if (info != null)
        //        {
        //            if (!string.IsNullOrEmpty(info.Visible))
        //            {
        //                kvp.Value.Visible = Authority.AuthorizeByRule(info.Visible);
        //            }

        //            //if (info.Shortcut.HasValue)
        //            //{
        //            //    kvp.Value.ShortcutKeys = info.Shortcut.Value;
        //            //}
        //            Image image = Feng.Windows.ImageResource.Get("Icons." + info.ImageName + ".png").Reference;
        //            if (image != null)
        //            {
        //                kvp.Value.Image = image;
        //            }
        //        }
        //    }

        //    IDictionary<string, ToolStripItem> dict2 = GetToolStripItems();
        //    foreach (KeyValuePair<string, ToolStripItem> kvp in dict2)
        //    {
        //        MenuPropertyInfo info = ADInfoBll.Instance.GetMenuPropertyInfo(kvp.Key);
        //        if (info != null)
        //        {
        //            if (!string.IsNullOrEmpty(info.Visible))
        //            {
        //                kvp.Value.Visible = Authority.AuthorizeByRule(info.Visible);
        //            }

        //            Image image = Feng.Windows.ImageResource.Get("Icons." + info.ImageName + ".png").Reference;
        //            if (image != null)
        //            {
        //                kvp.Value.Image = image;
        //            }
        //        }
        //    }
        //}

        

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private IDictionary<string, ToolStripMenuItem> GetToolStripMenuItems()
        //{
        //    IDictionary<string, ToolStripMenuItem> ret = new Dictionary<string, ToolStripMenuItem>();
        //    foreach (Control c in this.Controls)
        //    {
        //        MenuStrip menu = c as MenuStrip;
        //        if (menu != null)
        //        {
        //            foreach (ToolStripMenuItem item in menu.Items)
        //            {
        //                ret[item.Text] = item;
        //                GetToolStripMenuItems(ret, item);
        //            }
        //        }
        //    }

        //    return ret;
        //}

        //private void GetToolStripMenuItems(IDictionary<string, ToolStripMenuItem> ret, ToolStripMenuItem parent)
        //{
        //    foreach (ToolStripItem child in parent.DropDownItems)
        //    {
        //        ToolStripMenuItem mItem = child as ToolStripMenuItem;
        //        if (mItem == null)
        //        {
        //            continue;
        //        }

        //        GetToolStripMenuItems(ret, mItem);
        //    }
        //}

        //#endregion

        
    }
}