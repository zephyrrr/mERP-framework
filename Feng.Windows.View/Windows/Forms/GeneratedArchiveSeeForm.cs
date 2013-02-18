using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Forms;
using Feng.Windows.Utils;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    public enum DataGridType
    {
        DataUnboundGridLoadOnDemand = 11,
        DataBoundGridLoadOnDemand = 12,
        DataUnboundGridLoadOnce = 13
    }

    public class GeneratedArchiveSeeForm : ArchiveSeeForm
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_detailForm != null)
                {
                    m_detailForm.VisibleChanged -= new EventHandler(m_detailForm_VisibleChanged);
                   
                    IArchiveDetailFormWithDetailGrids dfdg = m_detailForm as IArchiveDetailFormWithDetailGrids;
                    if (dfdg != null)
                    {
                        foreach (IBoundGrid grid in dfdg.DetailGrids)
                        {
                            if (grid.DisplayManager != null)
                            {
                            }
                        }
                    }
                    m_detailForm = null;
                }

                this.DisposeWindowMenu();
                if (m_detailForm != null)
                {
                    m_detailForm.DisposeWindowMenu();
                }
            }
            base.Dispose(disposing);
        }

        //private System.Windows.Forms.ToolStrip m_toolStripMaster, m_toolStripDetail;
        //private List<System.Windows.Forms.ToolStripItem> m_tsbsMaster = new List<System.Windows.Forms.ToolStripItem>();
        //private List<System.Windows.Forms.ToolStripItem> m_tsbsDetail = new List<System.Windows.Forms.ToolStripItem>();

        public GeneratedArchiveSeeForm(WindowInfo windowInfo, DataGridType dataGridType)
            : base(dataGridType == DataGridType.DataUnboundGridLoadOnDemand ? (MyGrid)(new DataUnboundWithDetailGridLoadOnDemand()) : 
            (dataGridType == DataGridType.DataBoundGridLoadOnDemand ? (MyGrid)(new DataBoundWithDetailGridLoadOnDemand()) :
            (dataGridType == DataGridType.DataUnboundGridLoadOnce ? (MyGrid)(new DataUnboundWithDetailGridLoadonce()) : null)))
        {
            Initialize(windowInfo);
        }

        public GeneratedArchiveSeeForm(WindowInfo windowInfo)
            : this(windowInfo, DataGridType.DataUnboundGridLoadOnDemand)
        {
        }

        private void Initialize(WindowInfo windowInfo)
        {
            this.Name = windowInfo.Name;
            this.Text = windowInfo.Text;

            IList<WindowTabInfo> tabInfos = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowInfo.Name);
            if (tabInfos == null)
            {
                throw new ArgumentException("there is no windowTab with windowId of " + windowInfo.Name);
            }
            if (tabInfos.Count == 0)
            {
                throw new ArgumentException("There should be at least one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }
            if (tabInfos.Count > 1)
            {
                throw new ArgumentException("There should be at most one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }

            ISearchManager smMaster = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfos[0], null);

            IDisplayManager dmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(tabInfos[0], smMaster);

            (base.MasterGrid as IBoundGrid).SetDisplayManager(dmMaster, tabInfos[0].GridName);

            if (base.MasterGrid is IBoundGridWithDetailGridLoadOnDemand)
            {
                ArchiveFormFactory.GenerateDetailGrids((IBoundGridWithDetailGridLoadOnDemand)base.MasterGrid, tabInfos[0]);
            }

            // Load Additional Menus
            IList<WindowMenuInfo> windowMenuInfos = ADInfoBll.Instance.GetWindowMenuInfo(windowInfo.Name);
            IList<WindowMenuInfo> masterWindowMenuInfos;
            IList<WindowMenuInfo> detailWindowMenuInfos;
            GeneratedArchiveSeeForm.SplitWindowMenu(windowMenuInfos, out masterWindowMenuInfos, out detailWindowMenuInfos);
            if (masterWindowMenuInfos.Count > 0)
            {
                this.GenerateWindowMenu(masterWindowMenuInfos);
            }

            if (windowInfo.GenerateDetailForm)
            {
                // 自定义窗体
                if (windowInfo.DetailForm != null)
                {
                    m_detailForm = ArchiveFormFactory.CreateForm(ADInfoBll.Instance.GetFormInfo(windowInfo.DetailForm.Name)) as IArchiveDetailForm;
                    if (windowInfo.DetailWindow == null)
                    {
                        ArchiveFormFactory.GenerateArchiveDetailForm(windowInfo, null, null, dmMaster, m_detailForm);
                    }
                }
                // 跟主DisplayManager无关的DetailForm
                else if (windowInfo.DetailWindow != null)
                {
                    WindowInfo detailWindowInfo = ADInfoBll.Instance.GetWindowInfo(windowInfo.DetailWindow.Name);
                    m_detailForm = ServiceProvider.GetService<IWindowFactory>().CreateWindow(detailWindowInfo) as IArchiveDetailForm;
                    var searchWindow = m_detailForm.GetCustomProperty(MyChildForm.SearchPanelName) as ArchiveSearchForm;
                    if (searchWindow != null)
                    {
                        searchWindow.EnableProgressForm = false;
                    }
                }
                else
                {
                    // 当DetailFormId有值的时候，不一定是DetailForm，而只是其中的一部分
                    // 和主表一致的明细窗体
                    m_detailForm = ArchiveFormFactory.GenerateArchiveDetailForm(windowInfo, dmMaster);
                }
                if (m_detailForm != null)
                {
                    //m_detailWindow.ParentArchiveForm = this;
                    //m_detailWindow = m_detailWindow;

                    // Generate DetailForm's Menu
                    if (detailWindowMenuInfos.Count > 0)
                    {
                        m_detailForm.GenerateWindowMenu(detailWindowMenuInfos);
                        m_detailForm.VisibleChanged += new EventHandler(m_detailForm_VisibleChanged);
                    }
                }
            }

            ArchiveSearchForm searchForm = null;
            this.SetSearchPanel(() =>
            {
                if (searchForm == null)
                {
                    searchForm = new ArchiveSearchForm(this, smMaster, tabInfos[0]);
                }
                return searchForm;
            });

            m_attachmentForm = CreateAttachmentWindow(this, windowInfo);

            GeneratedArchiveSeeForm.InitializeWindowProcess(windowInfo, this);

            m_windowInfo = windowInfo;
        }

        private WindowInfo m_windowInfo;

        private IArhiveOperationMasterForm m_attachmentForm;
        public override IArhiveOperationMasterForm AttachmentForm
        {
            get
            {
                return m_attachmentForm;
            }
        }

        internal static IArhiveOperationMasterForm CreateAttachmentWindow(MyChildForm masterForm, WindowInfo masterWindowInfo)
        {
            WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo("SD_Attachment");

            MyForm form = null;
            if (windowInfo != null && !string.IsNullOrEmpty(masterWindowInfo.AttachmentId))
            {
                form = ServiceProvider.GetService<IWindowFactory>().CreateWindow(windowInfo) as MyForm;
                masterForm.SetCustomProperty("AttachmentEntityIdExp", masterWindowInfo.AttachmentId);

                if (form != null)
                {
                    form.FormClosing += new FormClosingEventHandler(delegate(object sender1, FormClosingEventArgs e1)
                    {
                        e1.Cancel = true;
                        form.Visible = false;
                    });
                }
                //if (form != null)
                //{
                    //form.Text = masterForm.Text + "的附件";
                    //form.Show();
                    //form.DisplayManager.SearchManager.LoadData(SearchExpression.And(
                    //    SearchExpression.Eq("EntityName", entityName), SearchExpression.Eq("EntityId", entityId)), null);
                    //(form.ControlManager.Dao as BaseDao<AttachmentInfo>).EntityOperating += new EventHandler<OperateArgs<AttachmentInfo>>(delegate(object sender1, OperateArgs<AttachmentInfo> e1)
                    //{
                    //    e1.Entity.EntityName = entityName;
                    //    e1.Entity.EntityId = entityId;
                    //});
                //}
            }
            return form as IArhiveOperationMasterForm;
        }

        internal static void InitializeWindowProcess(WindowInfo windowInfo, MyChildForm masterForm)
        {
            if (windowInfo.AutoProcess != null)
            {
                ProcessInfoHelper.ExecuteProcess(ADInfoBll.Instance.GetProcessInfo(windowInfo.AutoProcess.Name),
                   new Dictionary<string, object> { { "masterForm", masterForm } });
            }
            if (!string.IsNullOrEmpty(windowInfo.EventInitialized))
            {
                EventProcessUtils.ExecuteEventProcess(ADInfoBll.Instance.GetEventProcessInfos(windowInfo.EventInitialized), masterForm, System.EventArgs.Empty);
            }
        }

        internal static void SplitWindowMenu(IList<WindowMenuInfo> windowMenuInfos, out IList<WindowMenuInfo> masterWindowMenuInfos, out IList<WindowMenuInfo> detailWindowMenuInfos)
        {
            masterWindowMenuInfos = new List<WindowMenuInfo>();
            detailWindowMenuInfos = new List<WindowMenuInfo>();
            foreach (WindowMenuInfo windowMenuInfo in windowMenuInfos)
            {
                if (windowMenuInfo.InMasterWindow)
                {
                    masterWindowMenuInfos.Add(windowMenuInfo);
                }
                else
                {
                    detailWindowMenuInfos.Add(windowMenuInfo);
                }
            }
        }

        void m_detailForm_VisibleChanged(object sender, EventArgs e)
        {
            if (m_detailForm.Visible)
            {
                m_detailForm.SetMenuState();
            }
        }

        protected override void Form_Load(object sender, EventArgs e)
        {
            this.SetWindowState(m_windowInfo.WindowState, m_windowInfo.ShowMenu);

            base.Form_Load(sender, e);

            // set tsb enable
            this.SetMenuState();
            if (m_detailForm != null)
            {
                m_detailForm.SetMenuState();
            }
        }

        //private ArchiveDetailForm m_detailForm;
        private IArchiveDetailForm m_detailForm;
        public override IArchiveDetailForm ArchiveDetailForm
        {
            get
            {
                if (m_detailForm != null)
                    return m_detailForm;
                else
                    return m_detailForm;
            }
        }
    }
}
