using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Forms;
using Feng.Windows.Utils;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    public enum ArchiveGridType
    {
        ArchiveUnboundGridLoadOnDemand = 11,
        ArchiveBoundGrid = 2
    }

    public class GeneratedArchiveOperationForm : ArchiveOperationForm
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeWindowMenu();
                if (m_detailForm != null)
                {
                    m_detailForm.DisposeWindowMenu();
                }

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
            }
            base.Dispose(disposing);
        }

        public GeneratedArchiveOperationForm(WindowInfo windowInfo, ArchiveGridType gridType)
            : base(gridType == ArchiveGridType.ArchiveUnboundGridLoadOnDemand ? (MyGrid)(new ArchiveUnboundWithDetailGridLoadOnDemand()) :
            (gridType == ArchiveGridType.ArchiveBoundGrid ? (MyGrid)(new ArchiveBoundGrid()) : null))
        {
            Initialize(windowInfo);
        }


        public GeneratedArchiveOperationForm(WindowInfo windowInfo)
            : this(windowInfo, ArchiveGridType.ArchiveUnboundGridLoadOnDemand)
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
            IWindowControlManager cmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(tabInfos[0], smMaster) as IWindowControlManager;

            IBaseDao daoParent = ServiceProvider.GetService<IManagerFactory>().GenerateBusinessLayer(tabInfos[0]);
            cmMaster.Dao = daoParent;

            ((IArchiveGrid)base.MasterGrid).SetControlManager(cmMaster, tabInfos[0].GridName);

            // daoParent's subDao is inserted in detailForm
            if (base.MasterGrid is IBoundGridWithDetailGridLoadOnDemand)
            {
                ArchiveFormFactory.GenerateDetailGrids(base.MasterGrid as IBoundGridWithDetailGridLoadOnDemand, tabInfos[0]);
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
                if (windowInfo.DetailForm != null)
                {
                    m_detailForm = ArchiveFormFactory.CreateForm(ADInfoBll.Instance.GetFormInfo(windowInfo.DetailForm.Name)) as IArchiveDetailForm;
                    if (windowInfo.DetailWindow == null)
                    {
                        ArchiveFormFactory.GenerateArchiveDetailForm(windowInfo, cmMaster, daoParent, null, m_detailForm);
                    }
                }
                // 和主窗体不关联
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
                    m_detailForm = ArchiveFormFactory.GenerateArchiveDetailForm(windowInfo, cmMaster, daoParent as IRelationalDao);
                }

                if (m_detailForm != null)
                {
                    //m_detailWindow.ParentArchiveForm = this;
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
                    if (cmMaster != null)
                    {
                        cmMaster.StateControls.Add(searchForm);
                    }
                }
                return searchForm;
            });

            m_attachmentForm = GeneratedArchiveSeeForm.CreateAttachmentWindow(this, windowInfo);

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
