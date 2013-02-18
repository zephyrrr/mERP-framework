using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    public interface IArchiveMasterForm : IChildMdiForm, IDisposable, IGridContainer, IDisplayManagerContainer
    {
        bool SelfVisible { get; set; }

        IArchiveDetailForm ArchiveDetailForm { get; }

        void ShowArchiveDetailForm();

        bool DoView();
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ArchiveSeeForm : ArchiveGridForm, IArchiveMasterForm, IGridNamesContainer, IDisplayManagerContainer, IArchiveDetailForm, ILayoutControl
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                GridRelatedControl taskPane = this.GetCustomProperty(MyChildForm.GridRelatedPanelName, false) as GridRelatedControl;
                if (taskPane != null)
                {
                    taskPane.Dispose();
                }

                if (this.m_masterGrid != null)
                {
                    this.m_masterGrid.Dispose();
                    this.m_masterGrid = null;
                }
                
                if (this.ArchiveDetailForm != null)
                {
                    this.ArchiveDetailForm.Dispose();
                }

                base.RevertMergeMenu(this.menuStrip1);
                base.RevertMergeToolStrip(this.pageNavigator1);
                base.RevertMergeToolStrip(this.toolStrip1);
            }

            base.Dispose(disposing);
        }

        public ArchiveSeeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public ArchiveSeeForm(MyGrid masterGrid)
        {
            InitializeComponent();

            base.MergeMenu(this.menuStrip1);
            base.MergeToolStrip(this.toolStrip1);
            base.MergeToolStrip(this.pageNavigator1);

            m_masterGrid = masterGrid;
            if (m_masterGrid != null)
            {
                m_masterGrid.Dock = DockStyle.Fill;
                m_masterGrid.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());
                this.splitContainer1.Panel1.Controls.Add(m_masterGrid);
            }

            this.tsbView.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconDetail.png").Reference;
            this.tsbRefresh.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRefresh.png").Reference;
            this.tsbSearch.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSearch.png").Reference;
            this.tsbFilter.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFilter.png").Reference;
            this.tsbGroup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconGroup.png").Reference;
            this.tsbFind.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFind.png").Reference;
            this.tsbRelatedInfo.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExternalLink.png").Reference;

            this.tsbAttachment.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconAttach.png").Reference;
            this.tsbExportExcel.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExportExcel.png").Reference;
            this.tsbPrintPreview.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPrint.png").Reference;
            this.tsbGenerateReport.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNewReport.png").Reference;
            this.tsbSetup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSetup.png").Reference;

            this.tsmView.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconDetail.png").Reference;
            this.tsmRefresh.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRefresh.png").Reference;
            this.tsmSearch.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSearch.png").Reference;
            this.tsmFilter.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFilter.png").Reference;
            this.tsmGroup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconGroup.png").Reference;
            this.tsmFind.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFind.png").Reference;
            this.tsmRelatedInfo.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExternalLink.png").Reference;

            this.tsmExportExcel.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExportExcel.png").Reference;
            this.tsmPrintPreview.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPrint.png").Reference;
            this.tsmGenerateReport.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNewReport.png").Reference;
            this.tsmSetup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSetup.png").Reference;

            this.bindingNavigatorMoveLastItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconLast.png").Reference;
            this.bindingNavigatorMoveNextItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNext.png").Reference;
            this.bindingNavigatorMovePreviousItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPrevious.png").Reference;
            this.bindingNavigatorMoveFirstItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFirst.png").Reference;

            if (Feng.Utils.ReflectionHelper.GetObjectValue(typeof(TabbedMdiForm), ServiceProvider.GetService<IApplication>(), "m_twReletedInfos") != null)
            {
                this.tsbRelatedInfo.Visible = this.tsmRelatedInfo.Visible = false;
            }
            else
            {
                this.tsbRelatedInfo.Click += new EventHandler((sender, e) =>
                    {
                        GridRelatedControl taskPane = this.GetGridRelatedPanel() as GridRelatedControl;
                        if (taskPane != null)
                        {
                            PositionPersistForm searchForm = new PositionPersistForm();
                            searchForm.Name = taskPane.Name;
                            searchForm.Text = tsbRelatedInfo.Text;

                            searchForm.Controls.Add(taskPane);
                            taskPane.Dock = DockStyle.Fill;
                            searchForm.ShowDialog();

                            searchForm.Controls.Remove(taskPane);
                            searchForm.Dispose();
                        }
                    });
            }
        }

        //private ArchiveSeeForm m_parentArchiveSeeForm;
        ///// <summary>
        ///// Parent ArchiveSeeForm
        ///// </summary>
        //public ArchiveSeeForm ParentArchiveForm
        //{
        //    get { return m_parentArchiveSeeForm; }
        //    set { m_parentArchiveSeeForm = value; }
        //}

        private WindowMasterDetailState? m_windowMasterDetailState;
        public void SetWindowState(WindowMasterDetailState? windowState, bool? showMenu)
        {
            if (windowState.HasValue)
            {
                m_windowMasterDetailState = windowState;

                tsbView.Visible = (windowState & WindowMasterDetailState.EnableViewDetail) == WindowMasterDetailState.EnableViewDetail;
                if ((windowState & WindowMasterDetailState.EnableViewMaster) != WindowMasterDetailState.EnableViewMaster)
                {
                    if (this.ArchiveDetailForm != null)
                    {
                        tsbView_Click(tsbView, System.EventArgs.Empty);

                        this.splitContainer1.Panel1Collapsed = true;
                        this.ToolStrip.Visible = false;

                        if (this.ArchiveDetailForm is ArchiveDetailForm)
                        {
                            Feng.Utils.ReflectionHelper.SetObjectValue(
                                Feng.Utils.ReflectionHelper.GetObjectValue(typeof(ArchiveDetailForm), this.ArchiveDetailForm, "tsbViewHideMasterWindow"),
                                "Visible", false);
                        }
                    }
                }
            }

            if (showMenu.HasValue && !showMenu.Value)
            {
                base.MainMenuStrip.Visible = false;
                base.ToolStrip.Visible = false;
            }
        }
        
        private MyGrid m_masterGrid;
        /// <summary>
        /// 
        /// </summary>
        public override IGrid MasterGrid
        {
            get
            {
                return m_masterGrid;
            }
        }

        /// <summary>
        /// GridName
        /// 虽然这里没有Grid，但一些配置以GridName为准。同读入数据控件时的ControlGroupName
        /// </summary>
        public IBoundGrid Grid
        {
            get
            {
                IBoundGrid grid = m_masterGrid as IBoundGrid;
                return grid;
            }
        }

        public string[] GridNames
        {
            get
            {
                var grid = this.Grid;
                if (grid != null)
                    return new string[] { grid.GridName };
                else
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IDisplayManager DisplayManager
        {
            get
            {
                IBoundGrid grid = m_masterGrid as IBoundGrid;
                return grid == null ? null : grid.DisplayManager;
            }
        }

        /// <summary>
        /// 详细编辑窗体
        /// </summary>
        public virtual IArchiveDetailForm ArchiveDetailForm
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void OnDisplayManagerChanged()
        {
            if (this.DisplayManager == null || this.DisplayManager.SearchManager == null)
            {
                pageNavigator1.Enabled = false;
                //tsbSearchConditions.Enabled = false;
            }
            else
            {
                pageNavigator1.BindingSource = new PageBindingSource(this.DisplayManager.SearchManager);
                pageNavigator1.Enabled = this.DisplayManager.SearchManager.EnablePage;
 
                //tsbSearchConditions.LoadMenus(this.DisplayManager.SearchManager, this.Text);
            }

            IWinFormApplication mdiForm = ServiceProvider.GetService<IApplication>() as IWinFormApplication; 
            if (mdiForm != null)
            {
                mdiForm.OnChildFormShow(this);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //protected virtual void OnActiveGridChanged()
        //{
        //    this.tsbFilter.Checked = this.MasterGrid.GetFilterRowVisible();
        //}

        ///// <summary>
        ///// ToolStripItemSearchConditions
        ///// </summary>
        //protected CustomFindToolStripItem ToolStripItemSearchConditions
        //{
        //    get { return tsbSearchConditions; }
        //}

        private AMS.Profile.IProfile m_profile = SystemProfileFile.DefaultUserProfile;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, System.EventArgs e)
        {
            if (base.DesignMode)
            {
                return;
            }

            if (this.ArchiveDetailForm == null)
            {
                this.tsbView.Visible = false;
                this.tsmView.Visible = false;
            }
            else
            {
                LoadLayout();
            }

            bool check = m_profile.GetValue("ArchiveSeeForm." + this.Name, "tsbFilterChecked", false);
            if (check)
            {
                tsbFilter_Click(tsbFilter, System.EventArgs.Empty);
            }
            this.tsbFilter.Checked = this.MasterGrid.GetFilterRowVisible();

            // 创建TaskPane
            GridRelatedControl gridRelatedControl = null;
            this.SetGridRelatedPanel(() =>
            {
                if (gridRelatedControl == null)
                {
                    gridRelatedControl = new GridRelatedControl(this.MasterGrid.GridName, this.DisplayManager, this);
                }
                return gridRelatedControl;
            });

            this.OnDisplayManagerChanged();

            LoadAttachmentInfo();

            base.Form_Load(sender, e);
        }

        #region "Menus&Tsbs"
        /// <summary>
        /// 详细编辑窗体
        /// </summary>
        public virtual IArhiveOperationMasterForm AttachmentForm
        {
            get { return null; }
        }

        private void LoadAttachmentInfo()
        {
            m_attachmentEntityIdExp = (string)this.GetCustomProperty("AttachmentEntityIdExp");
            if (string.IsNullOrEmpty(m_attachmentEntityIdExp))
            {
                tsbAttachment.Visible = false;
                tsmAttachment.Visible = false;
            }
        }

        private void tsbAttachment_Click(object sender, EventArgs e)
        {
            if (this.AttachmentForm == null)
                return;
       
            object entity = this.DisplayManager.CurrentItem;
            if (entity == null)
            {
                MessageForm.ShowWarning("请选择当前行！");
                return;
            }

            Type entityType = entity.GetType();
            var sm = ServiceProvider.GetService<Feng.NH.ISessionFactoryManager>();
            if (sm == null)
                return;
            NHibernate.ISessionFactory sessionFactory = sm.GetSessionFactory(Feng.Utils.RepositoryHelper.GetConfigNameFromType(entityType));
            if (sessionFactory == null)
                return;

            bool hasCollection;

            int idx = m_attachmentEntityIdExp.LastIndexOf('.');
            if (idx != -1)
            {
                string navi = m_attachmentEntityIdExp.Substring(0, idx);
                entityType = Feng.NH.NHibernateHelper.GetPropertyType(sessionFactory, entityType, navi, out hasCollection);
            }

            NHibernate.Metadata.IClassMetadata metaData = sessionFactory.GetClassMetadata(entityType);
            string entityName = metaData.EntityName;
            object id = EntityScript.GetPropertyValue(entity, m_attachmentEntityIdExp);
            string entityId;

            //object id = EntityHelper.GetPropertyValue(entity, metaData.IdentifierPropertyName);
            if (id != null)
            {
                entityId = id.ToString();
            }
            else
            {
                MessageForm.ShowWarning("当前行无Id！");
                return;
            }

            string formText = entityName + ":" + entityId;
            IArhiveOperationMasterForm form = this.AttachmentForm;
            var form2 = form as ArchiveSeeForm;
            form2.tsbSearch.Visible = false;
            form.Text = formText + " 的附件";
            form.Show();
            form.DoView();
            
            form.DisplayManager.SearchManager.LoadData(SearchExpression.And(
                SearchExpression.Eq("EntityName", entityName), SearchExpression.Eq("EntityId", entityId)), null);

            m_attachmentEntityName = entityName;
            m_attachmentEntityId = entityId;
            //(form.ControlManager.Dao as BaseDao<AttachmentInfo>).EntityOperating += new EventHandler<OperateArgs<AttachmentInfo>>(delegate(object sender1, OperateArgs<AttachmentInfo> e1)
            //{
            //    e1.Entity.EntityName = entityName;
            //    e1.Entity.EntityId = entityId;
            //});

            (form.ControlManager.Dao as BaseDao<AttachmentInfo>).EntityOperating -= new EventHandler<OperateArgs<AttachmentInfo>>(AttachmentForm_EntityOperating);
            (form.ControlManager.Dao as BaseDao<AttachmentInfo>).EntityOperating += new EventHandler<OperateArgs<AttachmentInfo>>(AttachmentForm_EntityOperating);
        }

        private string m_attachmentEntityName, m_attachmentEntityId, m_attachmentEntityIdExp;
        void AttachmentForm_EntityOperating(object sender, OperateArgs<AttachmentInfo> e)
        {
            e.Entity.EntityName = m_attachmentEntityName;
            e.Entity.EntityId = m_attachmentEntityId;
        }

        private void tsbGenerateReport_Click(object sender, EventArgs e)
        {
            if (this.m_masterGrid == null)
            {
                MessageForm.ShowError("未能找到相应表格！");
                return;
            }

            this.m_masterGrid.GenerateReport();
        }

        private void tsbPrintPreview_Click(object sender, EventArgs e)
        {
            if (this.m_masterGrid == null)
            {
                MessageForm.ShowError("未能找到相应表格！");
                return;
            }
            this.m_masterGrid.PrintPriviewGrid();
        }

        private void tsbExportExcel_Click(object sender, EventArgs e)
        {
            if (this.m_masterGrid == null)
            {
                MessageForm.ShowError("未能找到相应表格！");
                return;
            }

            MyGrid.ExportToExcelCommand.Execute(this.m_masterGrid, ExecutedEventArgs.Empty);
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            TabbedMdiForm mdiForm = ServiceProvider.GetService<IApplication>() as TabbedMdiForm;
            bool ret = false;
            if (mdiForm != null && this.IsMdiChild)
            {
                ret = mdiForm.ShowSearchToolWindow();
            }
            if (!ret)
            {
                ShowSearchDialog(this.GetSearchPanel(), this.DisplayManager.SearchManager, this.Name);
            }
        }

        /// <summary>
        /// 用对话框显示查找窗口
        /// </summary>
        /// <param name="searchPanel"></param>
        /// <param name="sm"></param>
        /// <param name="formName"></param>
        public static void ShowSearchDialog(Control searchPanel, ISearchManager sm, string formName)
        {
            if (searchPanel != null)
            {
                PositionPersistForm searchForm = new PositionPersistForm();
                searchForm.Name = formName;
                searchForm.Text = "查找";

                searchForm.Controls.Add(searchPanel);
                searchPanel.Dock = DockStyle.Fill;

                sm.DataLoaded += new EventHandler<DataLoadedEventArgs>(searchManager_DataLoaded);
                m_searchForms[sm.Name] = searchForm;
                searchForm.ShowDialog();

                searchForm.Controls.Remove(searchPanel);
                searchForm.Dispose();
            }
        }
        private static Dictionary<string, PositionPersistForm> m_searchForms = new Dictionary<string, PositionPersistForm>();

        static void searchManager_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            ISearchManager sm = sender as ISearchManager;
            sm.DataLoaded -= new EventHandler<DataLoadedEventArgs>(searchManager_DataLoaded);
            if (m_searchForms.ContainsKey(sm.Name))
            {
                m_searchForms[sm.Name].Close();
            }
        }


        private void tsbFind_Click(object sender, EventArgs e)
        {
            if (this.MasterGrid != null)
            {
                Feng.Windows.Forms.FindForm.Instance.Show(this);
                Feng.Windows.Forms.FindForm.Instance.ToFindGrid = this.MasterGrid.GridControl;
            }
        }

        private void tsbFilter_Click(object sender, EventArgs e)
        {
            if (this.MasterGrid != null)
            {
                tsbFilter.Checked = !tsbFilter.Checked;
                this.MasterGrid.SetFilterRowVisible(tsbFilter.Checked);
            }
        }

        private void tsbGroup_Click(object sender, EventArgs e)
        {
            if (this.MasterGrid != null)
            {
                tsbGroup.Checked = !tsbGroup.Checked;

                if (tsbGroup.Checked)
                {
                    this.MasterGrid.FixedHeaderRows.Insert(0, new BoundGridGroupByRow());
                }
                else
                {
                    MyGroupByRow row = this.MasterGrid.GetGroupByRow();
                    if (row != null)
                    {
                        this.MasterGrid.FixedHeaderRows.Remove(row);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新数据，并且保持当前条位置
        /// </summary>
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            (m_masterGrid as IBoundGrid).ReloadData();
        }

        public static bool DoView(IArchiveMasterForm masterForm)
        {
            if (masterForm.ArchiveDetailForm == null)
            {
                MessageForm.ShowError("您只能通过表格来查看数据！");
                return false;
            }

            //if (this.DisplayManager.CurrentItem == null)
            //{
            //    MessageForm.ShowError("请选择要查看的记录！");
            //    return false;
            //}

            masterForm.ShowArchiveDetailForm();

            return true;
        }

        /// <summary>
        /// 参看记录操作
        /// </summary>
        public virtual bool DoView()
        {
            return DoView(this);
        }

        private void tsbView_Click(object sender, EventArgs e)
        {
            if (this.ArchiveDetailForm != null)
            {
                if (!tsbView.Checked)
                {
                    if (DoView())
                    {
                    }
                }
                else
                {
                    this.ArchiveDetailForm.Close(false);
                }
            }
        }

        /// <summary>
        /// ShowArchiveDetailForm
        /// </summary>
        /// <param name="detailForm"></param>
        public void ShowArchiveDetailForm()
        {
            IArchiveDetailForm detailForm = this.ArchiveDetailForm;
            Form form = detailForm as Form;
            if (form == null)
            {
                return;
            }

            // 需要变换DetailForm状态
            //if (form.Visible)
            //{
            //    return;
            //}

            //TabbedMdiForm mdiForm = TabbedMdiForm.GetMainForm();
            //if (mdiForm != null)
            {
                form.Text = this.Text + "_详细";

                //// show in mdi
                //mdiForm.AddFormToGroup(form, 1);
                //form.InvisibleOnClosing = true;

                form.FormBorderStyle = FormBorderStyle.None;
                form.TopLevel = false;
                form.Dock = DockStyle.Fill;
                this.splitContainer1.Panel2.Controls.Add(form);
                // 如果是最大化，里面的控件不能随着Panel2大小改变而改变
                //form.WindowState = FormWindowState.Maximized;
                // 在还没Show之前，SplitterDistance是按照原大小来的，此时改变窗体大小，SplitterDistance会按比例改变。
                // 所以应该先改变大小，再读入SplitterDistance
                form.Size = new Size(this.Width, this.Height - this.splitContainer1.SplitterDistance);

                this.splitContainer1.Panel2Collapsed = false;

                if (detailForm.MenuStrip != null)
                {
                    detailForm.MenuStrip.Visible = false;
                }
            }
            //else
            //{
            //    form.TopLevel = true;
            //}

            form.VisibleChanged -= new EventHandler(detailForm_VisibleChanged);
            form.VisibleChanged += new EventHandler(detailForm_VisibleChanged);

            form.Show();

            detailForm.UpdateContent();
            //if (detailForm is ArchiveDetailForm)
            //{
            //    (detailForm as ArchiveDetailForm).UpdateContent();
            //}
            //else if (detailForm is ArchiveSeeForm)
            //{
            //    (detailForm as ArchiveSeeForm).UpdateContent();
            //}
            // 如果DisplayManager是BindingSource类型的，如果Cancel会导致 PositionChanged，所以只能Disable Grid
            //  && form.ControlManager != null
            //    && (form.ControlManager.State == StateType.Add || form.ControlManager.State == StateType.Edit)
            // 还不是很好，有问题。Todo
            IDisplayManagerContainer dmc = detailForm as IDisplayManagerContainer;
            if (this.MasterGrid != null && dmc != null && dmc.DisplayManager == this.DisplayManager)
            {
                this.MasterGrid.ReadOnly = true;
            }
        }

        private void detailForm_VisibleChanged(object sender, EventArgs e)
        {
            Form form = sender as Form;
            if (!form.Visible)
            {
                if (this.MasterGrid != null)
                {
                    this.MasterGrid.ReadOnly = false;
                }
                this.splitContainer1.Panel2Collapsed = true;
                this.tsbView.Checked = false;
            }
            else
            {
                this.tsbView.Checked = true;
            }
        }


        private void tsbSetup_Click(object sender, EventArgs e)
        {
            using (ArchiveSetupForm form = new ArchiveSetupForm(this.MasterGrid, this.DisplayManager))
            {
                form.ShowDialog(this);
            }
        }

        //internal Feng.Windows.Forms.MySplitContainer SplitContainer
        //{
        //    get { return this.splitContainer1; }
        //}

        public bool SelfVisible
        {
            get
            {
                return !this.splitContainer1.Panel1Collapsed;
            }
            set
            {
                if (!value)
                {
                    this.splitContainer1.Panel1Collapsed = true;
                    this.ToolStrip.Visible = false;
                }
                else
                {
                    this.splitContainer1.Panel1Collapsed = false;
                    this.ToolStrip.Visible = true;
                }
            }
        }
        /// <summary>
        /// Form_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            try
            {
                SaveLayout();

                this.splitContainer1.Panel2.Controls.Clear();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }

            base.Form_Closing(sender, e);
        }

        protected override void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.ArchiveDetailForm != null)
            {
                this.ArchiveDetailForm.Close(true);
            }
        }
        #endregion


        #region "Menu"
        /// <summary>
        /// 
        /// </summary>
        protected override void AssociateMenuToToolStrip()
        {
            m_assoMenu2ToolStrip.Associate(tsmMovePrev, pageNavigator1.MovePreviousItem);
            m_assoMenu2ToolStrip.Associate(tsmMoveNext, pageNavigator1.MoveNextItem);
            m_assoMenu2ToolStrip.Associate(tsmMoveLast, pageNavigator1.MoveLastItem);
            m_assoMenu2ToolStrip.Associate(tsmMoveFirst, pageNavigator1.MoveFirstItem);

            m_assoMenu2ToolStrip.Associate(tsmAttachment, tsbAttachment);
            m_assoMenu2ToolStrip.Associate(tsmExportExcel, tsbExportExcel);
            m_assoMenu2ToolStrip.Associate(tsmGenerateReport, tsbGenerateReport);
            m_assoMenu2ToolStrip.Associate(tsmPrintPreview, tsbPrintPreview);
            

            m_assoMenu2ToolStrip.Associate(tsmSearch, tsbSearch);
            m_assoMenu2ToolStrip.Associate(tsmFind, tsbFind);
            m_assoMenu2ToolStrip.Associate(tsmRelatedInfo, tsbRelatedInfo);

            m_assoMenu2ToolStrip.Associate(tsmFilter, tsbFilter);
            m_assoMenu2ToolStrip.Associate(tsmRefresh, tsbRefresh);
            m_assoMenu2ToolStrip.Associate(tsmView, tsbView);
            m_assoMenu2ToolStrip.Associate(tsmSetup, tsbSetup);

            //m_assoMenu2ToolStrip.Associate(tsmSearchConditions, tsbSearchConditions);

            base.AssociateMenuToToolStrip();
        }

        #endregion

        public void UpdateContent()
        {
            this.tsbSearch.Visible = false;
        }

        public void Close(bool force)
        {
            if (force)
            {
                this.Close();
            }
            else
            {
                this.Hide();
            }
        }

        bool ILayoutControl.LoadLayout()
        {
            try
            {
                if (!m_windowMasterDetailState.HasValue)
                {
                    bool showDetail = !m_profile.GetValue("ArchiveSeeForm." + this.Name, "Panel2Collapsed", true);
                    if (showDetail)
                    {
                        DoView();
                    }
                    try
                    {
                        bool panel1Collapsed = m_profile.GetValue("ArchiveSeeForm." + this.Name, "Panel1Collapsed", false);
                        if (panel1Collapsed)
                        {
                            this.splitContainer1.Panel1Collapsed = true;
                            this.ToolStrip.Visible = false;
                        }

                        // todo：不知道为啥每次会自动增大24
                        // fixed?
                        var x = this.Width * m_profile.GetValue("ArchiveSeeForm." + this.Name, "SplitterDistance", this.Height - 300)
                            / m_profile.GetValue("ArchiveSeeForm." + this.Name, "SplitterWidth", this.Width);
                        if (x > 0)
                        {
                            this.splitContainer1.SplitterDistance = x;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ILayoutControl.SaveLayout()
        {
            try
            {
                m_profile.SetValue("ArchiveSeeForm." + this.Name, "tsbFilterChecked", tsbFilter.Checked);

                if (!m_windowMasterDetailState.HasValue)
                {
                    if (this.ArchiveDetailForm != null)
                    {
                        m_profile.SetValue("ArchiveSeeForm." + this.Name, "SplitterDistance", this.splitContainer1.SplitterDistance);
                        m_profile.SetValue("ArchiveSeeForm." + this.Name, "SplitterWidth", this.splitContainer1.Width);

                        m_profile.SetValue("ArchiveSeeForm." + this.Name, "Panel1Collapsed", this.splitContainer1.Panel1Collapsed);
                        m_profile.SetValue("ArchiveSeeForm." + this.Name, "Panel2Collapsed", this.splitContainer1.Panel2Collapsed);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            return true;
        }
    }
}