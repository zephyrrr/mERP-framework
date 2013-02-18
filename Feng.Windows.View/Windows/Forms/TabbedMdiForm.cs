using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Xceed.SmartUI;
using Xceed.SmartUI.Controls;
using Xceed.DockingWindows;
using Xceed.DockingWindows.TabbedMdi;
using Xceed.SmartUI.Controls.OutlookShortcutBar;
using Feng.Windows.Utils;
using Feng.Grid;
using Feng.Scripts;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MainForm
    /// </summary>
    public partial class TabbedMdiForm : Form, IWinFormApplication
    {
        #region Tabbed & Docked

        /// <summary>
        /// Fix bug leak of the last closed form
        /// http://memprofiler.com/forum/viewtopic.php?t=1160
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);
            try
            {
                typeof(Form).InvokeMember("FormerlyActiveMdiChild",
                  BindingFlags.Instance | BindingFlags.SetProperty |
                  BindingFlags.NonPublic, null,
                  this, new object[] { null });
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                // Something went wrong. Maybe we don't have enough permissions
                // to perform this or the "FormerlyActiveMdiChild" property
                // no longer exists.
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TabbedMdiForm()
        {
            InitializeComponent();

            this.Load += new EventHandler(MainForm_FormLoad);
            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            tsmCascade.Visible = false;
            tsmArrangeIcons.Visible = false;
            tsmTileHorizontal.Visible = false;
            tsmTileVertical.Visible = false;
            tsmCombineGroup.Visible = false;
            
            tsmCheckUpdate.Visible = false;
            tssCheckUpdate.Visible = false;

            tsmGotoCurrentGridADInfo.Visible = true;
            tsmGridStyleSheets.Visible = false;
            tsm根据当前配置更新GridColumn.Visible = false;

            tsbGetAddress.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRefresh.png").Reference;

            CreateStyleMenuItems();
        }

        private void CreateComponent()
        {
            //s_tabbedMdiForm = this;

            m_mdiManager = new TabbedMdiManager(this);
            m_mdiManager.Style = RenderStyle.VS2005; //VS2005 has error
            m_mdiManager.AllowModifications = true;
            m_mdiManager.ShowMenuOnCtrlTab = true;
            m_mdiManager.AllowClose = true;
            m_mdiManager.FormSelected += new FormSelectedEventHandler(ChildForm_FormSelected);

            m_mdiManager.TabGroupOrientation = Orientation.Horizontal;

            m_topGroup = new TabbedMdiTabGroup();
            m_mdiManager.TabGroups.Add(m_topGroup);
            m_bottomGroup = new TabbedMdiTabGroup();
            //m_mdiManager.TabGroups.Add(m_bottomGroup);

            m_dockManager = new DockLayoutManager(this, null);
            m_dockManager.Style = RenderStyle.VS2005;
            m_dockManager.AllowAutoHide = true;
            m_dockManager.AllowDocking = true;
            m_dockManager.AllowFloating = true;
            m_dockManager.AllowHide = true;
            m_dockManager.AutoHideFrameAppearance.ShowHideDelay = 50;

            this.smartOutlookShortcutBar1 = new Xceed.SmartUI.Controls.OutlookShortcutBar.SmartOutlookShortcutBar(this.components);
            //this.smartOutlookShortcutBar1.UIStyle = Xceed.SmartUI.UIStyle.UIStyle.WindowsClassic;
            XceedUtility.SetUIStyle(this.smartOutlookShortcutBar1);

            CreateToolWindow();
        }
        void MainForm_FormLoad(object sender, EventArgs e)
        {
            this.Visible = false;
            SplashScreen.Instance.BeginDisplay();
            SplashScreen.Instance.SetCommentaryString("正在载入主界面......");

            CreateComponent();

            ResetPerUser();

            m_dockManager.ActiveToolWindowChanged += new EventHandler(m_dockManager_ActiveToolWindowChanged);

            LoadLayoutDockManager(this.LayoutFileName);

            // 打开初始界面
            UserConfigurationInfo userInfo = ADInfoBll.Instance.GetUserConfigurationInfo(SystemConfiguration.UserName);
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.StartForm))
            {
                this.NavigateTo(userInfo.StartForm);
            }

            SplashScreen.Instance.EndDisplay();
            this.Visible = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_dockManager.SaveLayout(this.LayoutFileName);
                m_dockManager.ActiveToolWindowChanged -= new EventHandler(m_dockManager_ActiveToolWindowChanged);
                m_dockManager.ToolWindows.Clear();

                //SaveHistoryMenuItems();
                //SaveBookmarkMenuItems();

                SaveMdiFormSettings();

                UserActions.Instance.LogoutUser();

                LoginHelper.Logout();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }
        

        private void ResetPerUser()
        {
            if (Authority.IsAdministrators())
            {
                var s = ConfigurationHelper.GetDefaultServerDatabaseName();
                if (!string.IsNullOrEmpty(s))
                {
                    this.tssServer.Text = string.Format("服务器: db://{0};", s);
                }
                else
                {
                    this.tssServer.Text = string.Format("服务器: {0};", SystemConfiguration.Server);
                }
            }
            else
            {
                this.tssServer.Text = string.Empty;
            }
            this.tssTime.Text = "登录时间: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm tt") + ";";
            this.tssUser.Text = "用户名: " + System.Threading.Thread.CurrentPrincipal.Identity.Name + ";";

            LoadMenus();
            tsmAdmin.Visible = Authority.IsAdministrators();
            tsmDevelopment.Visible = Authority.IsDeveloper();

            //SplashScreen.Instance.SetCommentaryString("正在载入书签......");
            LoadHistoryMenuItems();
            LoadBookmarkMenuItems();

            LoadMdiFormSettings();
        }

        private void LoadMenus()
        {
            ClearAll();

            this.LoadMenus(UserActions.Instance.TopMenuInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(string status)
        {
            this.tssStatus.Text = status;
        }

        /// <summary>
        /// 在状态栏上添加控件
        /// </summary>
        /// <param name="item"></param>
        public void InsertStatusItem(int pos, ToolStripItem item)
        {
            if (pos == -1)
            {
                this.statusStrip1.Items.Add(item);
            }
            else
            {
                this.statusStrip1.Items.Insert(pos, item);
            }
        }

        /// <summary>
        /// 在状态栏上移除控件
        /// </summary>
        /// <param name="item"></param>
        public void RemoveStatusItem(ToolStripItem item)
        {
            this.statusStrip1.Items.Remove(item);
        }

        private TabbedMdiManager m_mdiManager;
        private TabbedMdiTabGroup m_topGroup, m_bottomGroup;
        private DockLayoutManager m_dockManager;
        private Xceed.SmartUI.Controls.OutlookShortcutBar.SmartOutlookShortcutBar smartOutlookShortcutBar1;
        private TaskPaneAlert m_taskPaneAlert;
        private MyToolWindow m_twSearch, m_twNavigator, m_twReletedInfos, m_twTask;

        protected virtual MyToolWindow CreateToolWindow(string twName)
        {
            if (twName == "导航" || twName == "搜索" || twName == "相关信息" || twName == "警示")
            {
                return new MyToolWindow(twName);
            }
            return null;
        }

        private void AddToolWindow(MyToolWindow tw)
        {
            if (tw != null)
            {
                m_dockManager.ToolWindows.Add(tw);
                tw.Closed += new EventHandler(toolWindow_Closed);
                tw.ToolWindowVisibleChanged += new EventHandler(toolWindow_ToolWindowVisibleChanged);
                tw.AutoScroll = true;
            }
        }
        private void CreateToolWindow()
        {
            if (m_dockManager.ToolWindows.Count == 0)
            {
                m_dockManager.SuspendLayout();

                m_twSearch = CreateToolWindow("搜索");
                AddToolWindow(m_twSearch);

                m_twReletedInfos = CreateToolWindow("相关信息");
                AddToolWindow(m_twReletedInfos);

                m_twNavigator = CreateToolWindow("导航");
                if (m_twNavigator != null)
                {
                    m_twNavigator.Controls.Add(this.smartOutlookShortcutBar1);
                    this.smartOutlookShortcutBar1.Dock = DockStyle.Fill;
                }
                AddToolWindow(m_twNavigator);

                m_twTask = CreateToolWindow("警示");
                if (m_twTask != null)
                {
                    m_taskPaneAlert = new TaskPaneAlert();
                    m_twTask.Controls.Add(this.m_taskPaneAlert);
                    this.m_taskPaneAlert.Dock = DockStyle.Fill;
                }
                AddToolWindow(m_twTask);

                m_dockManager.ResumeLayout();
            }
        }

        private void LoadLayoutDockManager(string layoutFileName)
        {
            if (System.IO.File.Exists(layoutFileName))
            {
                try
                {
                    m_dockManager.LoadLayout(layoutFileName);
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    System.IO.File.Delete(layoutFileName);
                }
            }
            else
            {
                tsmResetLayout_Click(tsmResetLayout, System.EventArgs.Empty);
            }
        }

        void m_dockManager_ActiveToolWindowChanged(object sender, EventArgs e)
        {
            if (m_dockManager.ActiveToolWindow != null
                && m_dockManager.ActiveToolWindow == m_twTask)
            {
                m_taskPaneAlert.ReloadData();
            }
        }

        void toolWindow_ToolWindowVisibleChanged(object sender, EventArgs e)
        {
            tsmSearchWindow.Checked = m_twSearch == null ? false : m_twSearch.Visible;
            tsmNavigatorWindow.Checked = m_twNavigator == null ? false : m_twNavigator.Visible;
            tsmReleatedWindow.Checked = m_twReletedInfos == null ? false : m_twReletedInfos.Visible;
            tsmTaskWindow.Checked = m_twTask == null ? false : m_twTask.Visible;
        }

        private void toolWindow_Closed(object sender, EventArgs e)
        {
        }

        private void RefreshToolStripSeparator()
        {
            //int count = m_forms.Count;
            //count = 1;
            //this.tsSeparator1.Visible = count > 0;
            //this.tsSeparator2.Visible = count > 0;
        }

        private void ChildForm_FormSelected(object sender, FormSelectedEventArgs e)
        {
            //this.SuspendLayout();

            MyChildForm child = e.Form as MyChildForm;
            if (child == null)
            {
                throw new ArgumentException("TabbledMdiForm's ChildForm must be MyChildForm!");
            }

            if (child.MenuStrip != null)
            {
                ToolStripManager.Merge(child.MenuStrip, menuStripMain);
                child.MenuStrip.Visible = false;
            }

            //ToolStripManager.LoadSettings(child);

            Feng.Windows.Forms.FindForm.Instance.Visible = false;

            RefreshToolStripSeparator();

            OnChildFormShow(child);

            //this.ResumeLayout();
        }

        private string LayoutFileName
        {
            get
            {
                return SystemDirectory.UserConfigFileName + ".layout";
            }
        }

        void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyChildForm child = sender as MyChildForm;
            if (child == null)
            {
                throw new ArgumentException("TabbledMdiForm's ChildForm must be MyChildForm!");
            }

            RefreshToolStripSeparator();

            child.FormClosing -= new FormClosingEventHandler(ChildForm_FormClosing);
            child.FormClosed -= new FormClosedEventHandler(ChildForm_FormClosed);
            child.VisibleChanged -= new EventHandler(childForm_VisibleChanged);
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyChildForm child = sender as MyChildForm;
            if (child == null)
            {
                throw new ArgumentException("TabbledMdiForm's ChildForm must be MyChildForm!");
            }
            m_forms.Remove(child.Name);

            OnChildFormShow(null);

            if (child.MenuStrip != null)
            {
                ToolStripManager.RevertMerge(menuStripMain, child.MenuStrip);
            }
            //ToolStripManager.SaveSettings(child);
        }

        #endregion

        #region "Form & Menus"

        ///// <summary>
        ///// 重新读入菜单
        ///// </summary>
        //private void InitMenus()
        //{
        //    foreach (Form form in this.MdiChildren)
        //    {
        //        form.Close();
        //    }
        //    m_forms.Clear();

        //    while (tsmFunctions.Items.Count > 0)
        //        tsmFunctions.Items.RemoveAt(0);
        //}

        private Dictionary<string, MyChildForm> m_forms = new Dictionary<string, MyChildForm>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formShow"></param>
        /// <returns></returns>
        internal MyChildForm ShowChildForm(MyChildForm formShow)
        {
            if (formShow == null)
                return null;

            if (m_forms.ContainsKey(formShow.Name))
            {
                m_forms[formShow.Name].Activate();
                return m_forms[formShow.Name];
            }

            try
            {
                formShow.FormClosing += new FormClosingEventHandler(ChildForm_FormClosing);
                formShow.FormClosed += new FormClosedEventHandler(ChildForm_FormClosed);

                AddFormToGroup(formShow, 0);
                formShow.Show();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);

                formShow.Close();
                //m_forms.Remove(menuInfo.Name);
                return null;
            }

            m_forms[formShow.Name] = formShow;
            return m_forms[formShow.Name];
        }

        /// <summary>
        /// 加入窗口到MdiManager的某一Group
        /// </summary>
        /// <param name="childForm"></param>
        /// <param name="groupIdx"></param>
        public void AddFormToGroup(Form childForm, int groupIdx)
        {
            childForm.MdiParent = this;

            if (groupIdx >= m_mdiManager.TabGroups.Count)
            {
                if (!m_bottomGroup.MdiForms.Contains(childForm))
                {
                    m_bottomGroup.MdiForms.Add(childForm);
                    childForm.VisibleChanged += new EventHandler(childForm_VisibleChanged);
                }
                m_mdiManager.TabGroups.Add(m_bottomGroup);
            }

            if (!m_mdiManager.TabGroups[groupIdx].MdiForms.Contains(childForm))
            {
                m_mdiManager.TabGroups[groupIdx].MdiForms.Add(childForm);
                childForm.VisibleChanged += new EventHandler(childForm_VisibleChanged);
            }
            else
            {
                m_mdiManager.SelectedTabGroup = m_mdiManager.TabGroups[groupIdx];
                childForm.Activate();
            }
        }

        void childForm_VisibleChanged(object sender, EventArgs e)
        {
            Form form = sender as Form;
            if (!form.Visible)
            {
                foreach (TabbedMdiTabGroup group in m_mdiManager.TabGroups)
                {
                    foreach (Form f in group.MdiForms)
                    {
                        if (f == form)
                        {
                            group.MdiForms.Remove(f);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MyChildForm ActiveChildMdiForm
        {
            get
            {
                if (m_mdiManager.SelectedTabGroup == null)
                {
                    return null;
                }

                return m_mdiManager.SelectedTabGroup.SelectedForm as MyChildForm;
            }
        }

        private Group m_defaultGroup;
        private void CreateShortcutBar(IEnumerable<MenuInfo> topMenuInfos, SmartItem parent)
        {
            foreach (MenuInfo info in topMenuInfos)
            {
                if (!Authority.AuthorizeByRule(info.Visible))
                {
                    continue;
                }

                SmartItem nowItem;
                if (info.ChildMenus.Count == 0 && info.Action != null)
                {
                    if (parent == null)
                    {
                        if (m_defaultGroup == null)
                        {
                            m_defaultGroup = new Group("Default");
                            m_defaultGroup.Text = "Default";
                            m_defaultGroup.ToolTipText = "Default";
                            m_defaultGroup.TextPosition = TextPosition.Bottom;
                            m_defaultGroup.Items.VerticalScrollBarVisibility = ScrollBarVisibility.VisibleWhenNeeded;
                            m_defaultGroup.Items.ScrollBarStyle = ScrollBarStyle.Standard;
                            this.smartOutlookShortcutBar1.Items.Add(m_defaultGroup);
                            nowItem = m_defaultGroup;
                        }
                        parent = m_defaultGroup;
                    }
                }

                if (parent == null)
                {
                    Group group = new Group(info.Name);
                    group.Text = info.Text;
                    group.ToolTipText = info.Help;
                    group.TextPosition = TextPosition.Bottom;
                    group.Items.VerticalScrollBarVisibility = ScrollBarVisibility.VisibleWhenNeeded;
                    group.Items.ScrollBarStyle = ScrollBarStyle.Standard;
                    this.smartOutlookShortcutBar1.Items.Add(group);
                    nowItem = group;
                }
                else
                {
                    Xceed.SmartUI.Controls.OutlookShortcutBar.Shortcut item = new Xceed.SmartUI.Controls.OutlookShortcutBar.Shortcut(info.Name);
                    item.Text = info.Text;
                    item.ToolTipText = info.Help;
                    parent.Items.Add(item);
                    nowItem = item;

                    if (info.ChildMenus.Count == 0)
                    {
                        item.Click += new SmartItemClickEventHandler(ShortCutItem_Click);
                    }
                }

                if (!string.IsNullOrEmpty(info.ImageName))
                {
                    nowItem.Image = Feng.Windows.ImageResource.Get("Icons." + info.ImageName + ".png").Reference;
                }
                if (nowItem.Image == null)
                {
                    nowItem.Image = GetDefaultImage(info);
                }

                nowItem.Visible = Authority.AuthorizeByRule(info.Visible);
                //if (info.Shortcut.HasValue)
                //{
                //    nowItem.ShortcutKeys = info.Shortcut.Value;
                //}

                nowItem.Tag = info;

                CreateShortcutBar(info.ChildMenus, nowItem);
            }
        }

        void ShortCutItem_Click(object sender, SmartItemClickEventArgs e)
        {
            SmartItem mi = sender as SmartItem;
            MenuInfo info = mi.Tag as MenuInfo;

            ExecuteMenu(info);

            AddToHistoryMenuItems(info);
        }

        private void ClearAll()
        {
            this.tsmFunctions.DropDownItems.Clear();
            this.smartOutlookShortcutBar1.Items.Clear();
        }

        /// <summary>
        /// LoadMenus
        /// </summary>
        /// <param name="menuInfos"></param>
        public void LoadMenus(IEnumerable<MenuInfo> menuInfos)
        {
            if (menuInfos == null)
                return;

            CreateMenus(menuInfos, null);

            CreateShortcutBar(menuInfos, null);
            if (this.smartOutlookShortcutBar1.Items.Count > 0)
            {
                (this.smartOutlookShortcutBar1.Items[0] as Group).Expanded = true;
            }

        }

        private Image GetDefaultImage(MenuInfo info)
        {
            Image ret = null;
            if (info.ChildMenus.Count > 0 || info.Action == null)
            {
                ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFolder.png").Reference;
            }
            else
            {
                ActionInfo actionInfo = ADInfoBll.Instance.GetActionInfo(info.Action.Name);
                if (actionInfo != null)
                {
                    switch (actionInfo.ActionType)
                    {
                        case ActionType.Window:
                            if (actionInfo.Window == null)
                            {
                                throw new ArgumentException("Action " + actionInfo.Name + "'s Window is not defind!");
                            }
                            WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(actionInfo.Window.Name);
                            if (windowInfo != null)
                            {
                                switch (windowInfo.WindowType)
                                {
                                    case WindowType.Maintain:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.applicationManagement.png").Reference;
                                        break;
                                    case WindowType.Transaction:
                                    case WindowType.TransactionBound:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconAutoForm.png").Reference;
                                        break;
                                    case WindowType.Query:
                                    case WindowType.QueryBound:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReport.png").Reference;
                                        break;
                                    case WindowType.DatabaseReport:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReport.png").Reference;
                                        break;
                                    case WindowType.DataSetReport:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReport.png").Reference;
                                        break;
                                    case WindowType.SelectWindow:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconAutoForm.png").Reference;
                                        break;
                                    case WindowType.DetailTransaction:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconAutoForm.png").Reference;
                                        break;
                                    default:
                                        ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconAutoForm.png").Reference;
                                        break;
                                }
                            }
                            break;
                        case ActionType.Form:
                            ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconConfigForm.png").Reference;
                            break;
                        case ActionType.Process:
                            ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconProcess.png").Reference;
                            break;
                        case ActionType.Url:
                            ret = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExternalLink.png").Reference;
                            break;
                    }
                }
            }
            return ret;

        }

        //private Dictionary<string, MenuInfo> m_menuInfos = new Dictionary<string, MenuInfo>();

        private void CreateMenus(IEnumerable<MenuInfo> menuInfos, ToolStripMenuItem parent)
        {
            foreach (MenuInfo info in menuInfos)
            {
                //m_menuInfos[info.Name] = info;

                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                
                menuItem.Text = info.Text;

                if (info.ChildMenus.Count == 0)
                {
                    menuItem.Click += new EventHandler(MenuItem_Click);
                }
                if (!string.IsNullOrEmpty(info.ImageName))
                {
                    menuItem.Image = Feng.Windows.ImageResource.Get("Icons." + info.ImageName + ".png").Reference;
                }
                if (menuItem.Image == null)
                {
                    menuItem.Image = GetDefaultImage(info);
                }
                //if (info.Shortcut.HasValue)
                //{
                //    menuItem.ShortcutKeys = info.Shortcut.Value;
                //}

                menuItem.Visible = Authority.IsDeveloper() || Authority.AuthorizeByRule(info.Visible);

                menuItem.Tag = info;

                if (parent == null)
                {
                    this.tsmFunctions.DropDownItems.Add(menuItem);
                }
                else
                {
                    parent.DropDownItems.Add(menuItem);
                }
                CreateMenus(info.ChildMenus, menuItem);
            }
        }


        /// <summary>
        /// OnMenuClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            MenuInfo info = mi.Tag as MenuInfo;

            ExecuteMenu(info);

            AddToHistoryMenuItems(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public object ExecuteAction(string actionName)
        {
            ActionInfo info = ADInfoBll.Instance.GetActionInfo(actionName);
            if (info == null)
                return null;

            Form form = ExecuteAction(info, true);
            return form;
        }

        /// <summary>
        /// 显示菜单中窗体
        /// </summary>
        /// <param name="menuInfo"></param>
        private Form ExecuteAction(ActionInfo info, bool addToMdiForm = false)
        {
            if (!Authority.IsDeveloper() && !Authority.AuthorizeByRule(info.Access))
            {
                MessageForm.ShowError("您没有执行此动作的权限！");
                return null;
            }

            Form form = null;
            switch (info.ActionType)
            {
                case ActionType.Window:
                    WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(info.Window.Name);
                    if (windowInfo == null)
                    {
                        throw new ArgumentException(string.Format("Invalid WindowId of {0} in menuInfo", info.Window.Name));
                    }
                    if (!Authority.IsDeveloper() && !Authority.AuthorizeByRule(windowInfo.Access))
                    {
                        MessageForm.ShowError("您没有打开此窗体的权限！");
                        return null;
                    }

                    //bool opened = OpenOnce(windowInfo.Name);
                    if (m_forms.ContainsKey(windowInfo.Name))
                    {
                        m_forms[windowInfo.Name].Activate();
                        return m_forms[windowInfo.Name];
                    }
                    try
                    {
                        form = ServiceProvider.GetService<IWindowFactory>().CreateWindow(windowInfo) as Form;
                        if (form == null)
                        {
                            return null;
                        }
                        form.Text = windowInfo.Text;
                        if (!string.IsNullOrEmpty(windowInfo.ImageName))
                        {
                            form.Icon = PdnResources.GetIconFromImage(Feng.Windows.ImageResource.Get("Icons." + windowInfo.ImageName + ".png").Reference);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithNotify(ex);
                    }
                    break;
                case ActionType.Form:
                    FormInfo formInfo = null;
                    if (info.Form != null)
                    {
                        formInfo = ADInfoBll.Instance.GetFormInfo(info.Form.Name);
                    }

                    if (formInfo == null)
                    {
                        throw new ArgumentException("Invalid FormInfo in menuInfo");
                    }
                    if (!Authority.IsDeveloper() && !Authority.AuthorizeByRule(formInfo.Access))
                    {
                        MessageForm.ShowError("您没有打开此窗体的权限！");
                        return null;
                    }

                    //bool opened = OpenOnce(formInfo.Name);
                    if (m_forms.ContainsKey(formInfo.Name))
                    {
                        m_forms[formInfo.Name].Activate();
                        return m_forms[formInfo.Name];
                    }
                    try
                    {
                        form = ArchiveFormFactory.CreateForm(formInfo);
                        if (!string.IsNullOrEmpty(formInfo.Text))
                        {
                            form.Text = formInfo.Text;
                        }
                        if (!string.IsNullOrEmpty(formInfo.ImageName))
                        {
                            form.Icon = PdnResources.GetIconFromImage(Feng.Windows.ImageResource.Get("Icons." + formInfo.ImageName + ".png").Reference);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithNotify(ex);
                    }

                    break;
                case ActionType.Process:
                    ProcessInfo processInfo = ADInfoBll.Instance.GetProcessInfo(info.Process.Name);
                    ProcessInfoHelper.ExecuteProcess(processInfo);
                    break;
                case ActionType.Url:
                    ProcessHelper.OpenUrl(info.WebAddress);
                    break;
                default:
                    throw new ArgumentException("menuInfo's MenuAction is Invalid");
            }

            if (form != null)
            {
                m_actionInfos[form.Name] = info;

                if (addToMdiForm)
                {
                    ShowChildForm(form as MyChildForm);
                }
            }

            return form;
        }

        private Dictionary<string, ActionInfo> m_actionInfos = new Dictionary<string, ActionInfo>();

        public void ExecuteMenu(string menuName)
        {
            MenuInfo menuInfo = ADInfoBll.Instance.GetMenuInfo(menuName);
            if (menuInfo != null)
            {
                ExecuteMenu(menuInfo);
            }
        }

        private void ExecuteMenu(MenuInfo menuInfo)
        {
            ActionInfo actionInfo = menuInfo.Action;
            if (menuInfo.Action != null)
            {
                actionInfo = ADInfoBll.Instance.GetActionInfo(menuInfo.Action.Name);
            }
            if (actionInfo == null)
            {
                return;
            }

            //System.Threading.AutoResetEvent are = new System.Threading.AutoResetEvent(false);

            Feng.Async.AsyncHelper.Start(new Feng.Async.AsyncHelper.DoWork(delegate()
                {
                    //System.Threading.Thread.Sleep(100000);
                    Form form = ExecuteAction(actionInfo);
                    //are.Set();
                    return form;
                }),
                new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                {
                    Form form = result as Form;
                    // 例如打开Url，执行Process等, Form = null
                    if (form != null)
                    {
                        form.TabStop = false;

                        if (menuInfo.AsDialog)
                        {
                            form.ShowDialog(this);
                            form.Dispose();
                            form = null;
                        }
                        MyChildForm childForm = form as MyChildForm;
                        if (childForm != null)
                        {
                            if (childForm.Icon == null && !string.IsNullOrEmpty(menuInfo.ImageName))
                            {
                                childForm.Icon = PdnResources.GetIconFromImage(Feng.Windows.ImageResource.Get("Icons." + menuInfo.ImageName + ".png").Reference);
                            }

                            ShowChildForm(childForm);
                        }
                    }
                }));

            //are.WaitOne();
        }
        #endregion

        #region "Operations"

        internal bool SearchToolWindowVisible
        {
            get { return m_twSearch == null ? false : m_twSearch.Visible; }
        }

        internal bool ShowSearchToolWindow()
        {
            if (m_twSearch != null)
            {
                m_twSearch.Visible = true;
                m_twSearch.State = ToolWindowState.Docked;
                foreach (Control c in m_twSearch.Controls)
                {
                    c.Visible = true;
                }
                return true;
            }
            return false;
        }

        private MyChildForm m_lastChildForm = null;
        /// <summary>
        /// 根据不同的子窗体，在MdiForm上做响应改变
        /// </summary>
        /// <param name="childForm"></param>
        public void OnChildFormShow(MyChildForm childForm)
        {
            // 有时候ArchiveSeeForm是作为弹出窗体使用
            if (childForm != null && !childForm.IsMdiChild)
                return;

            if (m_lastChildForm == childForm)
                return;

            m_lastChildForm = childForm;

            if (m_twSearch != null)
            {
                m_twSearch.Controls.Clear();
                if (childForm != null)
                {
                    var r = childForm.GetSearchPanel();
                    if (r != null)
                    {
                        r.Dock = DockStyle.Fill;
                        m_twSearch.Controls.Add(r);
                    }
                }
            }

            if (m_twReletedInfos != null)
            {
                m_twReletedInfos.Controls.Clear();
                if (childForm != null)
                {
                    GridRelatedControl taskPane = childForm.GetGridRelatedPanel() as GridRelatedControl;
                    if (taskPane != null)
                    {
                        m_twReletedInfos.Controls.Add(taskPane);
                        taskPane.Dock = DockStyle.Fill;
                    }
                }
            }
        }

        private void tsmRelogin_Click(object sender, EventArgs e)
        {
            using (LoginForm frm = new LoginForm(false))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    UserActions.Instance.LogoutUser();

                    tsmCloseAll_Click(tsmCloseAll, System.EventArgs.Empty);

                    SystemDirectory.Clear();
                    SystemProfileFile.Clear();

                    UserActions.Instance.LoginUser();

                    ResetPerUser();
                }
            }
        }

        private void tsmRefreshGlobalData_Click(object sender, EventArgs e)
        {
            //tsmCloseAll_Click(tsmCloseAll, System.EventArgs.Empty);
            //if (m_forms != null)
            //{
            //    m_forms.Clear();
            //}

            IDataBuffers db = ServiceProvider.GetService<IDataBuffers>();
            if (db != null)
            {
                db.Reload();
            }

            SystemDirectory.Clear();
            SystemProfileFile.Clear();

            var sm = ServiceProvider.GetService<Feng.NH.ISessionFactoryManager>();
            if (sm != null)
            {
                sm.DeleteSessionFactoryCache();
                Feng.NH.ICacheConfigurationManager cm = sm as Feng.NH.ICacheConfigurationManager;
                if (cm != null)
                {
                    cm.DeleteConfigurationCaches();
                }
            }

            IPersistentCache c = ServiceProvider.GetService<IPersistentCache>();
            if (c != null)
            {
                c.Destroy();
            }
            ICache c2 = ServiceProvider.GetService<IPersistentCache>();
            if (c2 != null)
            {
                c2.Clear();
            }

            UserActions.Instance.LoadInitializeData();

            PythonHelper.Reset();
        }

        private void tsmQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmAbout_Click(object sender, EventArgs e)
        {
            using (AboutBox form = new AboutBox())
            {
                form.ProductInfo["title"] = ProductInfo.Instance.Name;
                //form.ProductInfo["description"] = null;
                form.ProductInfo["product"] = ProductInfo.Instance.Name;
                form.ProductInfo["version"] = ProductInfo.Instance.CurrentVersion.ToString();
                form.ProductInfo["company"] = ProductInfo.Instance.CompanyName;
                form.ProductInfo["year"] = System.DateTime.Today.Year.ToString();
                form.ProductInfo["year"] = System.DateTime.Today.Year.ToString();
                //form.ProductInfo["trademark"] = ProductInfo.Instance.CompanyName;
                form.AppMoreInfo = null;
                form.AppDetailsButton = Authority.IsAdministrators();

                form.ShowDialog(this);
            }
        }

        private void tsmCalculator_Click(object sender, EventArgs e)
        {
            string path = Environment.SystemDirectory + "\\calc.exe";
            if (System.IO.File.Exists(path))
            {
                ProcessHelper.ExecuteApplication(path);
            }
            else
            {
                MessageForm.ShowError("未能找到计算器程序！");
            }
        }

        private void tsmNotebook_Click(object sender, EventArgs e)
        {
            string path = Environment.SystemDirectory + "\\notepad.exe";
            if (System.IO.File.Exists(path))
            {
                ProcessHelper.ExecuteApplication(path);
            }
            else
            {
                MessageForm.ShowError("未能找到记事本程序！");
            }
        }

        private void tsmChangePassword_Click(object sender, EventArgs e)
        {
            using (ChangePwdForm form = new ChangePwdForm())
            {
                form.ShowDialog(this);
            }
        }

        private void tsmCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tsmTileHorizontal_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tsmTileVertical_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void tsmArrangeIcons_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void tsmTaskWindow_Click(object sender, EventArgs e)
        {
            m_twTask.Visible = tsmTaskWindow.Checked;
        }

        private void tsmNavigatorWindow_Click(object sender, EventArgs e)
        {
            m_twNavigator.Visible = tsmNavigatorWindow.Checked;
        }

        private void tsmSearchWindow_Click(object sender, EventArgs e)
        {
            m_twSearch.Visible = tsmSearchWindow.Checked;
        }

        private void tsmReleatedWindow_Click(object sender, EventArgs e)
        {
            m_twReletedInfos.Visible = tsmReleatedWindow.Checked;
        }

        private void tsmStatusStrip_Click(object sender, EventArgs e)
        {
            statusStrip1.Visible = tsmStatusStrip.Checked;
        }

        private void tsmAutoFloatAll_Click(object sender, EventArgs e)
        {
            foreach (ToolWindow tw in m_dockManager.ToolWindows)
            {
                tw.State = ToolWindowState.AutoHide;
            }
        }

        private void tsmNewHorizontalGroup_Click(object sender, EventArgs e)
        {
            // 还不可用，有Bug
            //if (this.ActiveMdiChild != null)
            //{
            //    m_mdiManager.TabGroupOrientation = Orientation.Horizontal;
            //    m_bottomGroup = new TabbedMdiTabGroup();
            //    Form mdiChild = this.ActiveMdiChild;
            //    m_bottomGroup.MdiForms.Add(mdiChild);
            //    m_mdiManager.TabGroups.Add(m_bottomGroup);

            //    tsmCombineGroup.Visible = true;
            //    tsmNewHorizontalGroup.Visible = false;
            //    tsmNewVerticalGroup.Visible = false;
            //}
        }

        private void tsmNewVerticalGroup_Click(object sender, EventArgs e)
        {
            //if (this.ActiveMdiChild != null)
            //{
            //    m_mdiManager.TabGroupOrientation = Orientation.Vertical;
            //    m_bottomGroup = new TabbedMdiTabGroup();
            //    Form mdiChild = this.ActiveMdiChild;
            //    m_bottomGroup.MdiForms.Add(mdiChild);
            //    m_mdiManager.TabGroups.Add(m_bottomGroup);

            //    tsmCombineGroup.Visible = true;
            //    tsmNewHorizontalGroup.Visible = false;
            //    tsmNewVerticalGroup.Visible = false;
            //}
        }

        private void tsmCombineGroup_Click(object sender, EventArgs e)
        {
            //foreach (Form form in m_bottomGroup.MdiForms)
            //{
            //    m_topGroup.MdiForms.Add(form);
            //}

            //tsmCombineGroup.Visible = false;
            //tsmNewHorizontalGroup.Visible = true;
            //tsmNewVerticalGroup.Visible = true;
        }

        private void tsmCloseAll_Click(object sender, EventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                form.Close();
            }
        }

        protected virtual void tsmResetLayout_Click(object sender, EventArgs e)
        {
            m_dockManager.SuspendLayout();

            if (m_twSearch != null)
            {
                m_twSearch.DockTo(DockTargetHost.DockHost, DockPosition.Right);
                m_twSearch.ParentGroup.DockTo(DockTargetHost.DockHost, DockPosition.Right);
                m_twSearch.State = ToolWindowState.Docked;
                m_twSearch.Visible = true;
                m_twSearch.Width = 215;
            }

            if (m_twReletedInfos != null)
            {
                m_twReletedInfos.DockTo(m_twSearch, DockPosition.Group);
                //m_twReletedInfos.ParentGroup.DockTo(DockTargetHost.DockHost, DockPosition.Right);
                m_twReletedInfos.State = ToolWindowState.Docked;
                m_twReletedInfos.Visible = true;
            }

            if (m_twTask != null)
            {
                m_twTask.DockTo(m_twSearch, DockPosition.Group);
                //m_twTask.ParentGroup.DockTo(DockTargetHost.DockHost, DockPosition.Right);
                m_twTask.State = ToolWindowState.Docked;
                m_twTask.Visible = false;
            }

            if (m_twSearch != null)
            {
                m_twSearch.ParentGroup.SelectedToolWindow = m_twSearch;
            }

            if (m_twNavigator != null)
            {
                m_twNavigator.DockTo(DockTargetHost.DockHost, DockPosition.Left);
                m_twNavigator.ParentGroup.DockTo(DockTargetHost.DockHost, DockPosition.Left);
                m_twNavigator.State = ToolWindowState.AutoHide;
                m_twNavigator.Visible = true;
                m_twNavigator.Width = 250;
            }

            m_dockManager.ResumeLayout();

            this.statusStrip1.Visible = tsmStatusStrip.Checked = true;
            this.tsAddress.Visible = tsmAddressTextBox.Checked = false;
        }

        private void tsmOptions_Click(object sender, EventArgs e)
        {
            string optionDialogClassName = ServiceProvider.GetService<IDefinition>().TryGetValue(DefinitionString.OptionDialog);
            if (!string.IsNullOrEmpty(optionDialogClassName))
            {
                Form form = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(optionDialogClassName)) as Form;
                if (form != null)
                {
                    form.ShowDialog();
                    form.Dispose();
                }
            }
            else
            {
                MessageBox.Show("此程序无选项窗口。");
            }
        }

        private void tsmHomepage_Click(object sender, EventArgs e)
        {
            string homePage = ServiceProvider.GetService<IDefinition>().TryGetValue(DefinitionString.HomePage);
            if (!string.IsNullOrEmpty(homePage))
            {
                homePage = SystemConfiguration.Server + "/" + SystemConfiguration.ApplicationName;
            }
            ProcessHelper.OpenUrl(homePage);
        }

        private void tsmHelpContent_Click(object sender, EventArgs e)
        {
        	if (this.ActiveMdiChild == null)
        		return;

            string homePage = ServiceProvider.GetService<IDefinition>().TryGetValue(DefinitionString.HomePage);
            if (!string.IsNullOrEmpty(homePage))
            {
                homePage = SystemConfiguration.Server + "/" + SystemConfiguration.ApplicationName;
            }

        	string windowName = this.ActiveMdiChild.Name;
            string address = ServiceProvider.GetService<IDefinition>().TryGetValue("HelpAddress");
            if (string.IsNullOrEmpty(address))
            {
                address = string.Format("{0}/{1}/help/", homePage, SystemConfiguration.ApplicationName);
            }
            address += "window_" + windowName + ".html";
            ProcessHelper.OpenUrl(address);

            //if (!string.IsNullOrEmpty(fileName))
            //{
            //    ProcessHelper.OpenUrl("file://" + SystemDirectory.WorkDirectory + "\\" + fileName);
            //}

            //string fileName = Utils.HelpGenerator.GenerateWindowHelp(windowName);
        	
            //// Attachment
            //string attachmentFolder = System.IO.Path.GetDirectoryName(fileName) + "\\Attachment\\";
            //IList<AttachmentInfo> attachments = null;
            //using (NH.INHibernateRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository() as NH.INHibernateRepository)
            //{
            //    attachments = rep.Session.CreateCriteria<AttachmentInfo>()
            //        .Add(NHibernate.Criterion.Expression.Eq("EntityName",  "WindowInfo"))
            //        .Add(NHibernate.Criterion.Expression.Eq("EntityId", windowName))
            //        .List<AttachmentInfo>();
            //}
            //foreach(var i in attachments)
            //{
            //    System.IO.FileStream fs = new System.IO.FileStream(attachmentFolder + i.FileName, System.IO.FileMode.Create);
            //    fs.Write(i.Data, 0, i.Data.Length);
            //    fs.Close();
                
            //    if (i.FileName.EndsWith(".zip"))
            //    {
            //        CompressionHelper.DecompressToFolder(i.Data, attachmentFolder + i.FileName + "\\");
            //    }
            //}
        	
            //if (!string.IsNullOrEmpty(fileName))
            //{
            //    ProcessHelper.OpenUrl("file://" + SystemDirectory.WorkDirectory + "\\" + fileName);
            //}
        }

        private void tsmCheckUpdate_Click(object sender, EventArgs e)
        {
            //ProcessHelper.ExecuteApplication(System.IO.Path.Combine(SystemDirectory.WorkDirectory, "wyUpdate.exe"));
        }

        private void tsmUploadUserConfig_Click(object sender, EventArgs e)
        {
            UploadUserConfig(SystemConfiguration.UserName, SystemDirectory.UserDataDirectory);
        }
        private void tsmUploadGlobalData_Click(object sender, EventArgs e)
        {
            Feng.Utils.IOHelper.DirectoryCopy(SystemDirectory.UserDataDirectory, SystemDirectory.GlobalDataDirectory, true);

            UploadUserConfig("Global", SystemDirectory.GlobalDataDirectory);

            foreach (var file in System.IO.Directory.GetFiles(SystemDirectory.DataDirectory, "*.zip"))
            {
                string userName = System.IO.Path.GetFileNameWithoutExtension(file);
                byte[] s = System.IO.File.ReadAllBytes(file);

                UserConfigurationHelper.UploadConfiguration(userName, s);
            }
        }
        private void tsmDownloadUserConfig_Click(object sender, EventArgs e)
        {
            DownloadUserConfig(SystemConfiguration.UserName, SystemDirectory.UserDataDirectory);
        }

        private void tsmDownloadGlobalData_Click(object sender, EventArgs e)
        {
            DownloadUserConfig("Global", SystemDirectory.GlobalDataDirectory);
        }

        private void UploadUserConfig(string userName, string userDirectory)
        {
            tssStatus.Text = "正在上传配置文件，请稍候......";

            UserConfigurationHelper.UploadConfiguration(userName, userDirectory);

            tssStatus.Text = "";

            //Feng.Async.AsyncHelper asyncHelper = new Feng.Async.AsyncHelper(
            //    new Feng.Async.AsyncHelper.DoWork(delegate()
            //    {
            //        lock (this)
            //        {
            //            ConfigurationHelper.UploadConfiguration(userName, userDirectory);
            //            return true;
            //        }
            //    }),
            //    new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
            //    {
            //        tssStatus.Text = "";
            //    }));

            //asyncHelper.WaitForWorker();
        }

        // 下载，和本地合并
        private void DownloadUserConfig(string userName, string userDirectory)
        {
            tssStatus.Text = "正在下载用户配置......";
            UserConfigurationHelper.DownloadConfiguration(userName, userDirectory);
            tssStatus.Text = "";

            //Feng.Async.AsyncHelper asyncHelper = new Feng.Async.AsyncHelper(
            //    new Feng.Async.AsyncHelper.DoWork(delegate()
            //    {
            //        lock (this)
            //        {
            //            ConfigurationHelper.DownloadConfiguration(userName, userDirectory);
            //            return true;
            //        }
            //    }),
            //    new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
            //    {
            //        tssStatus.Text = "";
            //    }));

            //// bookmark等要依靠配置文件
            //asyncHelper.WaitForWorker();
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //ProcessHelper.ExecuteApplication(System.IO.Path.Combine(SystemDirectory.WorkDirectory, "wyupdate.exe"), "-quickcheck -noerr");

                GCMemory();
                SetWorkingSet(750000, 300000);
            }
            catch (Exception)
            {
            }
        }

        private void CreateStyleMenuItems()
        {
            foreach (string s in Feng.Grid.GridStyleSheetExtention.GetGridStyles())
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.CheckOnClick = true;
                menuItem.Text = s;
                menuItem.Click += new EventHandler(GridStyleMenuItem_Click);
                menuItem.Tag = s;

                tsmGridStyleSheets.DropDownItems.Add(menuItem);
            }

            //ToolStripMenuItem nullMenuItem = new ToolStripMenuItem();
            //nullMenuItem.Text = "无";
            //nullMenuItem.Click += new EventHandler(GridStyleMenuItem_Click);
            //nullMenuItem.Tag = null;
            //tsmGridStyleSheets.DropDownItems.Add(nullMenuItem);
        }

        void GridStyleMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            item.Checked = true;
            foreach (ToolStripMenuItem otherItem in tsmGridStyleSheets.DropDownItems)
            {
                if (otherItem != item)
                {
                    otherItem.Checked = false;
                }
            }

            string styleSheetName = item.Tag as string;
            Feng.Grid.GridSetting.CurrentStyleSheet = styleSheetName;

            foreach (Form form in this.MdiChildren)
            {
                ResetGridStyle(form, styleSheetName);
            }
        }

        private void ResetGridStyle(Control parentControl, string styleSheetName)
        {
            if (parentControl is IGrid)
            {
                (parentControl as IGrid).ApplyStyleSheet(styleSheetName);
            }
            else
            {
                foreach (Control child in parentControl.Controls)
                {
                    ResetGridStyle(child, styleSheetName);
                }
            }
        }

        private void RemoveIfExistInHistory(MenuInfo menuInfo)
        {
            foreach (ToolStripMenuItem i in tsmHistory.DropDownItems)
            {
                if (menuInfo.Name == (i.Tag as MenuHistoryInfo).MenuName)
                {
                    tsmHistory.DropDownItems.Remove(i);
                    break;
                }
            }
        }
        private void AddToHistoryMenuItems(MenuInfo srcMenu)
        {
            if (tsmHistory.DropDownItems.Count > 10)
            {
                tsmHistory.DropDownItems.RemoveAt(tsmHistory.DropDownItems.Count - 1);
            }
            RemoveIfExistInHistory(srcMenu);

            ToolStripMenuItem menuItem = new ToolStripMenuItem();

            StringBuilder sb = new StringBuilder();
            MenuInfo i = srcMenu;
            sb.Append(i.Text);
            while (i.ParentMenu != null)
            {
                sb.Insert(0, " - ");
                sb.Insert(0, i.ParentMenu.Text);
                i = i.ParentMenu;
            }
            string hisMenuText = sb.ToString();

            menuItem.Text = hisMenuText;
            menuItem.Click += new EventHandler(HistoryMenuItem_Click);
            //menuItem.Image = form.Icon;

            MenuHistoryInfo menuHistoryInfo = new MenuHistoryInfo();
            menuHistoryInfo.Name = hisMenuText;
            menuHistoryInfo.MenuName = srcMenu.Name;
            menuItem.Tag = menuHistoryInfo;

            tsmHistory.DropDownItems.Insert(0, menuItem);

            SaveHistoryMenuItems();
        }

        void HistoryMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            MenuInfo info = ADInfoBll.Instance.GetMenuInfo((item.Tag as MenuHistoryInfo).MenuName);
            ExecuteMenu(info);

            AddToHistoryMenuItems(info);
        }

        private void LoadHistoryMenuItems()
        {
            tsmHistory.DropDownItems.Clear();

            AMS.Profile.IProfile profile = SystemProfileFile.DefaultUserProfile;
            string history = profile.GetValue("MainWindow", "History", string.Empty);
            if (!string.IsNullOrEmpty(history))
            {
                string[] ss = history.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string s in ss)
                {
                    MenuHistoryInfo menuHistoryInfo = null;
                    try
                    {
                        menuHistoryInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<MenuHistoryInfo>(s);
                    }
                    catch(Exception)
                    {
                    }
                    if (menuHistoryInfo == null)
                        continue;

                    MenuInfo info = ADInfoBll.Instance.GetMenuInfo(menuHistoryInfo.MenuName);
                    if (info == null)
                        continue;

                    ToolStripMenuItem menuItem = new ToolStripMenuItem();

                    menuItem.Text = menuHistoryInfo.Name;
                    menuItem.Click += new EventHandler(HistoryMenuItem_Click);

                    //if (!string.IsNullOrEmpty(info.ImageName))
                    //{
                    //    menuItem.Image = Feng.Windows.ImageResource.Get("Icons." + info.ImageName + ".png").Reference;
                    //}
                    //if (menuItem.Image == null)
                    //{
                    //    menuItem.Image = GetDefaultImage(info);
                    //}

                    menuItem.Visible = Authority.AuthorizeByRule(info.Visible);

                    menuItem.Tag = menuHistoryInfo;

                    tsmHistory.DropDownItems.Add(menuItem);
                }
            }
        }
        private void SaveHistoryMenuItems()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ToolStripItem i in tsmHistory.DropDownItems)
            {
                MenuHistoryInfo menuHistoryInfo = i.Tag as MenuHistoryInfo;
                sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(menuHistoryInfo));
                sb.Append(Environment.NewLine);
            }
            if (sb.Length > 0)
            {
                SystemProfileFile.DefaultUserProfile.SetValue("MainWindow", "History", sb.ToString());
            }
        }

        private void AddBookmarkMenuItems(BookmarkInfo parentInfo, ToolStripMenuItem parentItem)
        {
            foreach (BookmarkInfo subInfo in parentInfo.Childs)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.MouseDown += new MouseEventHandler(BookmarkMenuItem_MouseDown);

                menuItem.Image = subInfo.IsFolder ? Feng.Windows.ImageResource.Get("Feng", "Icons.iconFolder.png").Reference :
                    Feng.Windows.ImageResource.Get("Feng", "Icons.iconBookmark.png").Reference;
                menuItem.Text = subInfo.Name;
                menuItem.Click += new EventHandler(BookmarkMenuItem_Click);

                //menuItem.Visible = Authority.AuthorizeByRule(info.Visible);
                string address = subInfo.Address;
                //menuItem.ToolTipText = address;
                menuItem.Tag = subInfo;

                parentItem.DropDownItems.Add(menuItem);

                AddBookmarkMenuItems(subInfo, menuItem);
            }
        }

        private BookmarkInfo m_rootBookmarkInfo;
        private void LoadBookmarkMenuItems()
        {
            while(tsmBookmark.DropDownItems.Count > 4)
                tsmBookmark.DropDownItems.RemoveAt(4);

            string fileName = SystemDirectory.UserDataDirectory + "\\bookmark.xml";
            if (!System.IO.File.Exists(fileName))
                return;

            m_rootBookmarkInfo = BookmarkManagerForm.LoadBookmark(fileName);

            if (m_rootBookmarkInfo != null)
            {
                AddBookmarkMenuItems(m_rootBookmarkInfo, tsmBookmark);
            }

            //AMS.Profile.IProfile profile = SystemProfileFile.DefaultUserProfile;
            //string history = profile.GetValue("MainWindow", "Bookmark", string.Empty);
            //if (!string.IsNullOrEmpty(history))
            //{
            //    string[] ss = history.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (string s in ss)
            //    {
            //        //Dictionary<string, string> bm = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
            //        BookmarkInfo bookmarkInfo = null;
            //        try
            //        {
            //            bookmarkInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<BookmarkInfo>(s);
            //        }
            //        catch (Exception)
            //        {
            //        }

            //        if (bookmarkInfo == null)
            //            continue;

            //        ToolStripMenuItem menuItem = new ToolStripMenuItem();
            //        menuItem.MouseDown += new MouseEventHandler(BookmarkMenuItem_MouseDown);

            //        menuItem.Text = bookmarkInfo.Name;
            //        menuItem.Click += new EventHandler(BookmarkMenuItem_Click);

            //        //if (!string.IsNullOrEmpty(info.ImageName))
            //        //{
            //        //    menuItem.Image = Feng.Windows.ImageResource.Get("Icons." + info.ImageName + ".png").Reference;
            //        //}
            //        //if (menuItem.Image == null)
            //        //{
            //        //    menuItem.Image = GetDefaultImage(info);
            //        //}

            //        //menuItem.Visible = Authority.AuthorizeByRule(info.Visible);
            //        string address = bookmarkInfo.Address;
            //        menuItem.ToolTipText = address;
            //        menuItem.Tag = bookmarkInfo;

            //        tsmBookmark.DropDownItems.Add(menuItem);
            //    }
            //}
        }
        private void SaveBookmarkMenuItems()
        {
            //StringBuilder sb = new StringBuilder();
            //foreach (ToolStripItem i in tsmBookmark.DropDownItems)
            //{
            //    if (i == tsmAddToBookmark || i == tsmBookmarkManage || i == tsmBookmarkSeparator)
            //        continue;

            //    BookmarkInfo bookmarkInfo = i.Tag as BookmarkInfo;
            //    sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(bookmarkInfo));
            //    sb.Append(Environment.NewLine);
            //}
            //if (sb.Length > 0)
            //{
            //    SystemProfileFile.DefaultUserProfile.SetValue("MainWindow", "Bookmark", sb.ToString());
            //}

            m_rootBookmarkInfo = new BookmarkInfo();
            m_rootBookmarkInfo.Name = "书签";
            m_rootBookmarkInfo.Childs = new List<BookmarkInfo>();
            foreach (ToolStripItem i in tsmBookmark.DropDownItems)
            {
                if (i == tsmAddToBookmark || i == tsmBookmarkManage || i == tsmBookmarkSeparator)
                    continue;

                BookmarkInfo bookmarkInfo = i.Tag as BookmarkInfo;
                m_rootBookmarkInfo.Childs.Add(bookmarkInfo);

                SaveBookmarkMenuItems(bookmarkInfo, i);
            }

            string fileName = SystemDirectory.UserDataDirectory + "\\bookmark.xml";
            BookmarkManagerForm.SaveBookmark(m_rootBookmarkInfo, fileName);
        }
        private void SaveBookmarkMenuItems(BookmarkInfo parentInfo, ToolStripItem parentItem)
        {
            ToolStripMenuItem parentMenuItem = parentItem as ToolStripMenuItem;
            if (parentMenuItem == null)
                return;

            parentInfo.Childs.Clear();
            foreach (ToolStripItem subItem in parentMenuItem.DropDownItems)
            {
                BookmarkInfo subInfo = subItem.Tag as BookmarkInfo;
                parentInfo.Childs.Add(subInfo);

                SaveBookmarkMenuItems(subInfo, subItem);
            }
        }

        private void tsm设置为主页_Click(object sender, EventArgs e)
        {
            string address = GetAddress();
            UserConfigurationHelper.UpdateStartFormofConfiguration(SystemConfiguration.UserName, address);
        }

        private void tsmAddToBookmark_Click(object sender, EventArgs e)
        {
            string address = GetAddress();
            if (!string.IsNullOrEmpty(address))
            {
                BookmarkInfo bookmarkInfo = new BookmarkInfo();
                bookmarkInfo.Name = this.ActiveChildMdiForm.Text;
                bookmarkInfo.Address = address;
                bookmarkInfo.Childs = new List<BookmarkInfo>();
                m_rootBookmarkInfo.Childs.Add(bookmarkInfo);

                ToolStripMenuItem menuItem = new ToolStripMenuItem();
                menuItem.MouseDown += new MouseEventHandler(BookmarkMenuItem_MouseDown);
                menuItem.Text = bookmarkInfo.Name;
                menuItem.Click += new EventHandler(BookmarkMenuItem_Click);

                //if (!string.IsNullOrEmpty(info.ImageName))
                //{
                //    menuItem.Image = Feng.Windows.ImageResource.Get("Icons." + info.ImageName + ".png").Reference;
                //}
                //if (menuItem.Image == null)
                //{
                //    menuItem.Image = GetDefaultImage(info);
                //}

                //menuItem.Visible = Authority.AuthorizeByRule(info.Visible);
                menuItem.ToolTipText = address;
                menuItem.Tag = bookmarkInfo;
                tsmBookmark.DropDownItems.Add(menuItem);

                SaveBookmarkMenuItems();
            }
            else
            {
                MessageForm.ShowWarning("无当前页！");
            }
        }
        void BookmarkMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            this.NavigateTo((item.Tag as BookmarkInfo).Address);
        }

        private ToolStripMenuItem m_contextBookmarkMenuItem = null;
        private void BookmarkMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) 
                return;
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            tsmBookmark.DropDown.AutoClose = false;
            m_contextBookmarkMenuItem = item;
            contextMenuStripBookmark.Show(item.Owner, new Point(item.Bounds.Left + e.X, item.Bounds.Top + e.Y));
        }
        private void tsmDeleteBookmark_Click(object sender, EventArgs e)
        {
            if (m_contextBookmarkMenuItem != null)
            {
                tsmBookmark.DropDownItems.Remove(m_contextBookmarkMenuItem);
                m_contextBookmarkMenuItem = null;
                tsmBookmark.DropDown.AutoClose = true;
                tsmBookmark.DropDown.Close();
            }
        }
        private void tsmBookmarkProperty_Click(object sender, EventArgs e)
        {
            if (m_contextBookmarkMenuItem != null)
            {
                tsmBookmark.DropDown.AutoClose = true;
                tsmBookmark.DropDown.Close();

                BookmarkInfo bookmarkInfo = m_contextBookmarkMenuItem.Tag as BookmarkInfo;
                BookmarkPropertyForm form = new BookmarkPropertyForm(bookmarkInfo);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    m_contextBookmarkMenuItem.Text = bookmarkInfo.Name;
                    m_contextBookmarkMenuItem.ToolTipText = bookmarkInfo.Address;
                }
            }
        }
        private void tsmBookmarkManage_Click(object sender, EventArgs e)
        {
            BookmarkManagerForm form = new BookmarkManagerForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadBookmarkMenuItems();
            }
        }

        private void LoadMdiFormSettings()
        {
            AMS.Profile.IProfile profile = SystemProfileFile.DefaultUserProfile;
            bool showViewAddress = profile.GetValue("MainWindow", "showViewAddress", false);
            tsAddress.Visible = tsmAddressTextBox.Checked = showViewAddress;

            bool showStatusBar = profile.GetValue("MainWindow", "showStatusBar", true);
            this.statusStrip1.Visible = this.tsmStatusStrip.Checked = showStatusBar;

            string gridStyleSheet = profile.GetValue("MainWindow", "gridStyleSheet", null);
            foreach (ToolStripMenuItem item in tsmGridStyleSheets.DropDownItems)
            {
                if ((string)item.Tag == gridStyleSheet)
                {
                    item.PerformClick();
                }
            }
        }

        private void SaveMdiFormSettings()
        {
            AMS.Profile.IProfile profile = SystemProfileFile.DefaultUserProfile;
            profile.SetValue("MainWindow", "showViewAddress", tsAddress.Visible);
            profile.SetValue("MainWindow", "showStatusBar", this.statusStrip1.Visible);
            profile.SetValue("MainWindow", "gridStyleSheet", GridSetting.CurrentStyleSheet);
        }

        private void tsmAddress_Click(object sender, EventArgs e)
        {
            tsAddress.Visible = tsmAddressTextBox.Checked;
            if (tsAddress.Visible)
            {
                tsbGetAddress_Click(tsbGetAddress, System.EventArgs.Empty);
            }
        }

        private void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)System.Windows.Forms.Keys.Enter)
            {
                this.NavigateTo(txtAddress.Text);
            }
        }

        private void tsbGetAddress_Click(object sender, EventArgs e)
        {
            this.txtAddress.Text = GetAddress();
        }
        private string GetAddress()
        {
            string address = null;
            if (this.ActiveChildMdiForm != null)
            {
                string actionInfo = m_actionInfos.ContainsKey(this.ActiveChildMdiForm.Name) ? m_actionInfos[this.ActiveChildMdiForm.Name].Name : null;
                if (!string.IsNullOrEmpty(actionInfo))
                {
                    if (this.ActiveChildMdiForm != null)
                    {
                        if (this.ActiveChildMdiForm is IDisplayManagerContainer)
                        {
                            IDisplayManager dm = (this.ActiveChildMdiForm as IDisplayManagerContainer).DisplayManager;
                            address = ApplicationExtention.GetNavigatorAddress(actionInfo, dm);
                        }
                        else
                        {
                            address = ApplicationExtention.GetNavigatorAddress(actionInfo);
                        }
                    }
                }
            }
            return address;
        }
        #endregion 

        #region "Admin"
        private void tsmSetupPermission_Click(object sender, EventArgs e)
        {
            using (MembershipForm form = new MembershipForm())
            {
                form.ShowDialog(this);
            }
        }
        private void tsmGotoCurrentWindowADInfo_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                MessageForm.ShowError("未存在当前窗口！");
                return;
            }
            if (this.ActiveMdiChild is IWindowNamesContainer)
            {
                string[] names = (this.ActiveMdiChild as IWindowNamesContainer).WindowNames;
                if (names != null && names.Length > 0)
                {
                    IDisplayManagerContainer form = ExecuteAction("AD_Window_Tab") as IDisplayManagerContainer;
                    form.DisplayManager.SearchManager.LoadData(SearchExpression.InG("Window.Name", names), null);
                }
            }
            else
            {
                string name = this.ActiveMdiChild.Name;
                if (this.ActiveMdiChild is IControlManagerContainer)
                {
                    name = (this.ActiveMdiChild as IControlManagerContainer).ControlManager.Name;
                }
                else if (this.ActiveMdiChild is IDisplayManagerContainer)
                {
                    name = (this.ActiveMdiChild as IDisplayManagerContainer).DisplayManager.Name;
                }
                IDisplayManagerContainer form = ExecuteAction("AD_Window_Tab") as IDisplayManagerContainer;
                form.DisplayManager.SearchManager.LoadData(SearchExpression.Like("Window.Name", name), null);
            }
        }

        private void tsmGotoCurrentGridADInfo_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
            {
                MessageForm.ShowError("未存在当前窗口！");
                return;
            }
            //if (this.ActiveMdiChild is IGridContainer)
            //{
            //    string toGridId = (this.ActiveMdiChild as IGridContainer).GridName;
            //    if (!string.IsNullOrEmpty(toGridId))
            //    {
            //        IDisplayManagerContainer form = ExecuteAction("AD_Grid_Column") as IDisplayManagerContainer;
            //        form.DisplayManager.SearchManager.LoadData(SearchExpression.Eq("GridName", toGridId), null);
            //    }
            //}
            if (this.ActiveMdiChild is IGridNamesContainer)
            {
                string[] toGridId = (this.ActiveMdiChild as IGridNamesContainer).GridNames;
                if (toGridId != null && toGridId.Length > 0)
                {
                    IDisplayManagerContainer form = ExecuteAction("AD_Grid_Column") as IDisplayManagerContainer;
                    form.DisplayManager.SearchManager.LoadData(SearchExpression.InG("GridName", toGridId), null);
                }
            }
        }

        
        private void 根据当前配置更新GridColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!MessageForm.ShowYesNo("是否确认更新？"))
                return;

            var f = this.ActiveMdiChild as IGridNamesContainer;
            if (f != null)
            {
                string[] toGridId = f.GridNames;
                foreach (var gridName in toGridId)
                {
                    var g = UserControlHelper.SearchChildControl<IGrid>(this, (grid) =>
                        {
                            if (grid != null && grid.GridName == gridName)
                                return true;
                            else
                                return false;
                        });
                    if (g == null)
                        continue;

                    using (var rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<GridColumnInfo>())
                    {
                        try
                        {
                            rep.BeginTransaction();
                            foreach (Xceed.Grid.Column c in g.Columns)
                            {
                                var columnInfo = c.Tag as GridColumnInfo;
                                if (columnInfo == null)
                                    continue;
                                //columnInfo.SeqNo = c.VisibleIndex;
                                //columnInfo.ColumnWidth = c.Width * 1024 / g.Width;
                                //columnInfo.ColumnFixed = c.Fixed;
                                //columnInfo.ColumnVisible = c.Visible;

                                rep.Update(columnInfo);
                            }
                            rep.CommitTransaction();
                        }
                        catch (Exception)
                        {
                            rep.RollbackTransaction();
                        }

                    }
                }
            }
        }

        private void tsmConnectionStringAdmin_Click(object sender, EventArgs e)
        {
            using (ConnectionStringModifyForm form = new ConnectionStringModifyForm())
            {
                form.ShowDialog();
            }
        }
        #endregion

        #region"Development"
        private void tsmTestAllMenu_Click(object sender, EventArgs e)
        {
            TestUtility.TestAll();
        }

        private void tsmTestCurrent_Click(object sender, EventArgs e)
        {
            TestUtility.TestWindow(this.ActiveMdiChild as MyForm);
        }
        private void GCMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void tsmClearMemory_Click(object sender, EventArgs e)
        {
            long before = GC.GetTotalMemory(true);

            GCMemory();
            SetWorkingSet(750000, 300000);

            long after = GC.GetTotalMemory(true);
            MessageForm.ShowInfo("Before: " + before + System.Environment.NewLine + "After:  " + after);
        }
        private static void SetWorkingSet(int lnMaxSize, int lnMinSize)
        {
            System.Diagnostics.Process loProcess = System.Diagnostics.Process.GetCurrentProcess();

            loProcess.MaxWorkingSet = (IntPtr)lnMaxSize;
            loProcess.MinWorkingSet = (IntPtr)lnMinSize;

            //long lnValue = loProcess.WorkingSet; // see what the actual value
        }

        private void tsmRunIronPython_Click(object sender, EventArgs e)
        {
            PythonScriptForm form = new PythonScriptForm();
            form.Show();
            PythonHost.Instance.Scope.SetVariable("mdiForm", this);
            PythonHost.Instance.Scope.SetVariable("masterForm", this.ActiveMdiChild);
        }
        private void tsmRunIronPythonCode_Click(object sender, EventArgs e)
        {
            PythonCodeForm form = new PythonCodeForm();
            form.Show();
            PythonHost.Instance.Scope.SetVariable("mdiForm", this);
            PythonHost.Instance.Scope.SetVariable("masterForm", this.ActiveMdiChild);
        }

        private void tsmRunIronPython2_Click(object sender, EventArgs e)
        {
            PythonConsoleHost host = PythonConsoleHost.OpenPythonConsole(this);
            host.ScriptScope.SetVariable("mdiForm", this);
            host.ScriptScope.SetVariable("masterForm", this.ActiveMdiChild);
        }

        private void tsmRunQuery_Click(object sender, EventArgs e)
        {
            QueryForm form = new QueryForm();
            form.Show();
        }
        private void tsmSearchInAdDb_Click(object sender, EventArgs e)
        {
            var form = new Feng.Windows.Utils.AdInfoSearchForm();
            ShowChildForm(form);
        }
        private void tsmRunCSharpCode_Click(object sender, EventArgs e)
        {
            CSharpCodeForm form = new CSharpCodeForm();
            form.Show();
        }

        private void tsmOpenLog_Click(object sender, EventArgs e)
        {
            log4netLogger.FlushAllAppenders();
            string logFile = ".\\log.txt";
            ProcessHelper.ExecuteApplication(logFile);
        }
        private void tsmOpenConfig_Click(object sender, EventArgs e)
        {
            string fileName = System.Reflection.Assembly.GetEntryAssembly().Location + ".config";
            var process = ProcessHelper.ExecuteApplication(fileName);
            if (process != null)
            {
                process.WaitForExit();
                log4net.Config.XmlConfigurator.Configure();
                // re-init ConfigurationManager
                var constructorInfo = typeof(System.Configuration.ConfigurationManager).GetConstructor(
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new Type[0], null);
                constructorInfo.Invoke(null, null);
            }
        }
        #endregion

        private void tsmDevelopment_DropDownOpening(object sender, EventArgs e)
        {
            tsmRunPy.DropDownItems.Clear();
            string dir = ServiceProvider.GetService<IApplicationDirectory>().GetMainDirectory() + "\\AdminScript";
            if (!System.IO.Directory.Exists(dir))
                return;
            foreach (string s in System.IO.Directory.GetFiles(dir))
            {
                if (s.Contains("_"))
                    continue;
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = System.IO.Path.GetFileName(s);
                item.Tag = s;
                item.Click += new EventHandler(tsmRunPythonFile_Click);
                tsmRunPy.DropDownItems.Add(item);
            }
        }

        void tsmRunPythonFile_Click(object sender, EventArgs e)
        {
            string file = (sender as ToolStripMenuItem).Tag.ToString();
            PythonHelper.ExecutePythonFile(file, new Dictionary<string, object> { { "mdiForm", this }, { "masterForm", this.ActiveMdiChild } });
        }

        private void tsmUpgradeDb_Click(object sender, EventArgs e)
        {
            if (!MessageForm.ShowYesNo("可能会出现意想不到的错误，建议先备份数据库。是否继续？"))
                return;

            TestUtility.UpgradeAdDb();
        }

        private void tsmRunAdInfoUtility_Click(object sender, EventArgs e)
        {
            string exeFileName = "ADInfosUtil.exe";
            string configFileName = string.Format("{0}\\{1}.config", SystemDirectory.WorkDirectory, exeFileName);
            if (!System.IO.File.Exists(configFileName) || !Authority.IsDeveloper())
            {
                System.IO.File.Copy(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config", configFileName, true);
            }
            var process = ProcessHelper.ExecuteApplication(exeFileName);
            if (process != null)
            {
                process.WaitForExit();
            }
            if (System.IO.File.Exists(configFileName) && !Authority.IsDeveloper())
            {
                System.IO.File.Delete(configFileName);
            }
        }

        #region "Old Static func"
        //private static TabbedMdiForm s_tabbedMdiForm;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static TabbedMdiForm GetMainForm()
        //{
        //    if (s_tabbedMdiForm == null)
        //    {
        //        TabbedMdiForm mdiForm = null;
        //        foreach (System.Windows.Forms.Form form in Application.OpenForms)
        //        {
        //            if (form.IsMdiContainer)
        //            {
        //                mdiForm = form as TabbedMdiForm;
        //                if (mdiForm != null)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        s_tabbedMdiForm = mdiForm;
        //    }
        //    return s_tabbedMdiForm;
        //}

        ///// <summary>
        ///// 在主窗体上打开菜单中的窗口
        ///// </summary>
        ///// <param name="menuName"></param>
        ///// <returns></returns>
        //public static Form ShowMenuFormInMdi(string menuName)
        //{
        //    TabbedMdiForm mainForm = GetMainForm();
        //    MenuInfo menuInfo = ADInfoBll.Instance.GetMenuInfo(menuName);
        //    if (menuInfo != null)
        //    {
        //        return mainForm.ShowMenuForm(menuInfo);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 在Mdi窗体中显示
        ///// </summary>
        ///// <param name="form"></param>
        ///// <returns></returns>
        //public static Form ShowChildFormInMdi(MyChildForm form)
        //{
        //    TabbedMdiForm mainForm = GetMainForm();

        //    return mainForm.ShowChildForm(form);
        //}
        #endregion
    }
}