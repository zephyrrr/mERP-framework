using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Feng;
using Feng.Grid;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public class MyTemplateForm : MyChildForm, IGridNamesContainer, IWindowNamesContainer, IControlManagerContainer, IDisplayManagerContainer
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisableSearchProgressForm(null);

                foreach (var sm in m_progressInvalidate.Keys)
                {
                    sm.DataLoaded -= new EventHandler<DataLoadedEventArgs>(searchManager2_DataLoaded);
                }

                MenuWindowExtention.DisposeWindowMenu(this);
            }
            base.Dispose(disposing);
        }

        private List<string> m_gridNames = new List<string>();
        public string[] GridNames
        {
            get 
            {
                return m_gridNames.ToArray(); 
            }
        }
        private List<string> m_windowNames = new List<string>();
        public string[] WindowNames
        {
            get
            {
                return m_windowNames.ToArray();
            }
        }

        private IControlManager m_cm;
        public IControlManager ControlManager 
        {
            get 
            {
                if (this.ActiveControl == this || this.ActiveControl == null)
                    return null;

                var d = UserControlHelper.SearchParentControl<IControlManagerContainer>(this.ActiveControl);
                if (d != null && !(d is MyTemplateForm))
                    return d.ControlManager;
                return m_cm;
            }
        }

        private IDisplayManager m_dm;
        public IDisplayManager DisplayManager
        {
            get
            {
                if (this.ActiveControl == this || this.ActiveControl == null)
                    return null;

                var d = UserControlHelper.SearchParentControl<IDisplayManagerContainer>(this.ActiveControl);
                if (d != null && !(d is MyTemplateForm))
                    return d.DisplayManager;

                if (m_dm != null)
                    return m_dm;
                if (m_cm != null)
                    return m_cm.DisplayManager;
                return null;
            }
        }

        public static void AddControl(Control container, Control child)
        {
            //if (!(container is FlowLayoutPanel))
            {
                child.Dock = DockStyle.Fill;
            }
            container.Controls.Add(child);
        }

        private void TryAddButtons(MyButton btn, MyChildForm menuWindow)
        {
            IList<IButton> tsbs = menuWindow.GetCustomProperty(MenuWindowExtention.s_windowMenuButtonName) as IList<IButton>;
            if (tsbs == null)
            {
                tsbs = new List<IButton>();
                menuWindow.SetCustomProperty(MenuWindowExtention.s_windowMenuButtonName, tsbs);
            }
            tsbs.Add(btn);
        }

        public void AssociateButton(MyButton btn, IDisplayManager dm, string windowMenuName)
        {
            var windowMenuInfo = ADInfoBll.Instance.GetInfos<WindowMenuInfo>("Name = '" + windowMenuName + "'")[0];
            if (windowMenuInfo == null)
            {
                throw new ArgumentException("There is no WindowMenu of " + windowMenuName);
            }
            TryAddButtons(btn, this);

            btn.Text = windowMenuInfo.Text;
            btn.Tag = windowMenuInfo;
            btn.Click += new EventHandler((sender, e) =>
            {
                Button tsb = sender as Button;
                WindowMenuInfo info = tsb.Tag as WindowMenuInfo;

                MenuWindowExtention.OnButtonClick(info, tsb.FindForm());
            });

            dm.PositionChanged += new EventHandler((sender, e) =>
            {
                IDisplayManager dm2 = sender as IDisplayManager;

                if (dm2 == dm)
                {
                    MenuWindowExtention.SetMenuState(this);
                }
            });
        }

        public void AssociateButton(MyButton btn, IControlManager cm, string windowMenuName)
        {
            var windowMenuInfo = ADInfoBll.Instance.GetInfos<WindowMenuInfo>("Name = '" + windowMenuName + "'")[0];
            if (windowMenuInfo == null)
            {
                throw new ArgumentException("There is no WindowMenu of " + windowMenuName);
            }
            TryAddButtons(btn, this);
            btn.Text = windowMenuInfo.Text;
            btn.Tag = windowMenuInfo;
            btn.Click += new EventHandler((sender, e) =>
                {
                    Button tsb = sender as Button;
                    WindowMenuInfo info = tsb.Tag as WindowMenuInfo;

                    MenuWindowExtention.OnButtonClick(info, tsb.FindForm());
                    //UpdateContent(cm, gridName);
                });

            cm.DisplayManager.PositionChanged += new EventHandler((sender, e) =>
                {
                    IDisplayManager dm2 = sender as IDisplayManager;

                    if (dm2 == cm.DisplayManager)
                    {
                        MenuWindowExtention.SetMenuState(this);
                    }
                });
            cm.StateChanged += new EventHandler((sender, e) =>
            {
                IControlManager cm2 = sender as IControlManager;

                if (cm2 == cm)
                {
                    MenuWindowExtention.SetMenuState(this);
                }
            });

            cm.EditEnded += new EventHandler((sender, e) =>
            {
                IControlManager cm2 = sender as IControlManager;

                if (cm2 == cm)
                {
                    if (cm2.DisplayManager.InBatchOperation)
                    {
                        return;
                    }

                    MenuWindowExtention.SetMenuState(this);
                }
            });
        }

        static void DisplayManager_PositionChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public static ArchiveSeeForm AssociateSeeForm(Control container, string windowName)
        {
            return AssociateSeeForm(container, windowName, true);
        }
        public static ArchiveOperationForm AssociateOperationForm(Control container, string windowName)
        {
            return AssociateOperationForm(container, windowName, true);
        }

        public static ArchiveSeeForm AssociateSeeForm(Control container, string windowName, bool asDetail)
        {
            WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(windowName);
            if (windowInfo == null)
                return null;

            ArchiveSeeForm form = new GeneratedArchiveSeeForm(windowInfo);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Visible = true;

            if (asDetail)
            {
                form.MenuStrip.Visible = false;
                Feng.Utils.ReflectionHelper.SetObjectValue(Feng.Utils.ReflectionHelper.GetObjectValue(form, "tsbView"), "Visible", false);
            }

            AddControl(container, form);

            return form;
        }

        public static ArchiveOperationForm AssociateOperationForm(Control container, string windowName, bool asDetail)
        {
            WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(windowName);
            if (windowInfo == null)
                return null;

            ArchiveOperationForm form = new GeneratedArchiveOperationForm(windowInfo);
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Visible = true;

            if (asDetail)
            {
                form.MenuStrip.Visible = false;
                Feng.Utils.ReflectionHelper.SetObjectValue(Feng.Utils.ReflectionHelper.GetObjectValue(form, "tsbView"), "Visible", false);
            }

            AddControl(container, form);

            return form;
        }

        public static IBoundGrid AssociateBoundGridS(Control container, string windowTabName)
        {
            var info = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            if (info == null)
                return null;

            GeneratedDataUnboundGrid grid = new GeneratedDataUnboundGrid(info);
            AddControl(container, grid);
            grid.LoadLayout();

            return grid;
        }

        public IBoundGrid AssociateBoundGrid(Control container, string windowTabName)
        {
            var grid = MyTemplateForm.AssociateBoundGridS(container, windowTabName);
            if (grid == null)
                return null;
            m_windowNames.Add(windowTabName);
            m_gridNames.Add(grid.GridName);
            m_dm = grid.DisplayManager;
            return grid;
        }

        public static IArchiveGrid AssociateArchiveGridS(Control container, string windowTabName)
        {
            var info = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            if (info == null)
                return null;

            GeneratedArchiveUnboundGrid grid = new GeneratedArchiveUnboundGrid(info);
            AddControl(container, grid);
            grid.LoadLayout();

            return grid;
        }

        public IArchiveGrid AssociateArchiveGrid(Control container, string windowTabName)
        {
            var grid = AssociateArchiveGridS(container, windowTabName);
            if (grid == null)
                return null;
            m_windowNames.Add(windowTabName);
            m_gridNames.Add(grid.GridName);
            m_cm = grid.ControlManager;

            return grid;
        }

        public static IArchiveGrid AssociateArchiveDetailGrid(Control container, string windowTabName, IControlManager cmParent, IRelationalDao daoParent)
        {
            WindowTabInfo windowTabInfo = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            if (windowTabName == null)
                return null;

            GeneratedArchiveUnboundGrid grid = new GeneratedArchiveUnboundGrid(windowTabInfo, cmParent);

            IWindowControlManager subCm = grid.ControlManager as IWindowControlManager;
            //ISearchManager subSm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(windowTabInfo, cmParent.DisplayManager);
            //IWindowControlManager subCm = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(windowTabInfo, subSm) as IWindowControlManager;
            //subCm.Name = windowTabInfo.Name;
            //grid.SetControlManager(subCm, windowTabInfo.GridName);
            //ManagerFactory.GenerateBusinessLayer(daoParent, windowTabInfo);

            int i = 0;
            IBaseDao subDao = daoParent.GetRelationalDao(i);
            if (subDao is IMemoriedRelationalDao)
            {
                IMemoryDao subMemoryDao = ((IMemoriedRelationalDao)daoParent.GetRelationalDao(i)).DetailMemoryDao;
                subCm.Dao = subMemoryDao;

                //subMemoryDao.AddSubDao(new MasterDetailMemoryDao<>(cmParent));
                ((IMemoriedRelationalDao)daoParent.GetRelationalDao(i)).AddRelationToMemoryDao(cmParent.DisplayManager);
            }
            else
            {
                subCm.Dao = subDao;
            }

            AddControl(container, grid);
            grid.LoadLayout();

            grid.IsInDetailMode = true;
            cmParent.StateControls.Add(grid);
            cmParent.CheckControls.Add(grid);

            return grid;
        }

        public static IBoundGrid AssociateDataDetailGrid(Control container, string windowTabName, IDisplayManager dmParent)
        {
            WindowTabInfo windowTabInfo = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            if (windowTabName == null)
                return null;

            GeneratedDataUnboundGrid grid = new GeneratedDataUnboundGrid(windowTabInfo, dmParent);

            //ISearchManager subSm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(windowTabInfo, dmParent);
            //IDisplayManager subDm = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(windowTabInfo, subSm);
            //subDm.Name = windowTabInfo.Name;
            //grid.SetDisplayManager(subDm, windowTabInfo.GridName);

            AddControl(container, grid);
            grid.LoadLayout();
            return grid;
        }

        public static Control[] GenerateControlsInFlowLaoutPanel(System.Windows.Forms.FlowLayoutPanel flowLayoutPanel, string windowTabName)
        {
            var wintabInfo = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            IList<GridColumnInfo> infos = ADInfoBll.Instance.GetGridColumnInfos(wintabInfo.GridName);
            var controlContainers = new List<Control>();
            for (int i = 0; i < infos.Count; ++i)
            {
                if (string.IsNullOrEmpty(infos[i].DataControlType))
                    continue;

                if (infos[i].GridColumnType == GridColumnType.NoColumn
                    || infos[i].GridColumnType == GridColumnType.SplitColumn)
                    continue;

                var p = new Panel();
                //p.AutoSize = true;
                //p.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                p.Name = "pnl" + infos[i].GridColumnName;
                p.Size = new System.Drawing.Size(112 + 52 + 16, 24);
                flowLayoutPanel.Controls.Add(p);
                //var p = flowLayoutPanel;
                //p.Name = "pnlAll";
                controlContainers.Add(p);
            }
            return controlContainers.ToArray();
        }

        public static void AssociateDataControls(Control[] containers, IDisplayManager dm, string gridName, bool includeLabel, Control parentControl, Dictionary<string, Label> labels)
        {
            foreach (Control c in containers)
            {
                string name = c.Name;
                if (!name.StartsWith("pnl"))
                {
                    throw new NotSupportedException("only panel support default associate!");
                }
                AssociateDataControl(c, dm, gridName, name.Substring(3), includeLabel);   //remove pnl
            }

            if (containers.Length > 0 && labels != null && parentControl != null)
            {
                NotNullControls(dm, parentControl, labels);
            }
        }
        public static IDataControl AssociateDataControl(Control container, IDisplayManager dm, string gridName, string gridColumnName, bool includeLabel = false)
        {
            var dc = GetDataControl(dm, gridName, gridColumnName, includeLabel);
            if (dc == null)
            {
                throw new ArgumentException(string.Format("there is no {0}!", gridColumnName));
            }
            AddControl(container, dc as Control);

            return dc;
        }

        private static IDataControl GetDataControl(IDisplayManager dm, string gridName, string gridColumnName, bool includeLabel = false)
        {
            if (gridName == null)
            {
                throw new ArgumentNullException("gridName");
            }

            IList<GridColumnInfo> infos = ADInfoBll.Instance.GetGridColumnInfos(gridName);
            string infoName = gridName + "_" + gridColumnName;
            foreach (GridColumnInfo info in infos)
            {
                if (info.GridColumnName == gridColumnName
                    || info.Name == infoName)
                {
                    IDataControl dc = null;
                    if (dm != null)
                    {
                        if (includeLabel)
                        {
                            dc = ControlFactory.GetDataControl(info, dm.Name);
                        }
                        else
                        {
                            dc = ControlFactory.GetDataControlWrapper(info, dm.Name);
                        }
                        dm.DataControls.Add(dc);
                    }
                    else
                    {
                        if (includeLabel)
                        {
                            dc = ControlFactory.GetDataControl(info, info.GridName);
                        }
                        else
                        {
                            dc = ControlFactory.GetDataControlWrapper(info, info.GridName);
                        }
                    }

                    dc.ReadOnly = true;

                    //dc.AvailableChanged += new EventHandler((sender, e) =>
                    //    {
                    //        IDataControl dcc = sender as IDataControl;
                    //        (dcc as Control).Visible = dcc.Available;
                    //    });
                    return dc;
                }
            }
            return null;
        }

        public static IDataControl[] GetDataControls(IDisplayManager dm, string gridName, string[] gridColumnNames, bool includeLabel)
        {
            List<IDataControl> ret = new List<IDataControl>();

            for (int i = 0; i < gridColumnNames.Length; ++i)
            {
                var dc = GetDataControl(dm, gridName, gridColumnNames[i], includeLabel);
                ret.Add(dc);
            }

            return ret.ToArray();
        }

        public static IDataControl[] AssociateDataControlGroup(Control container, IDisplayManager dm, string gridName, string[] gridColumnNames)
        {
            if (gridColumnNames.Length != 2)
            {
                throw new ArgumentException("gridColumnNames", "Now only support 2 controls!");
            }

            IDataControl[] dcs = GetDataControls(dm, gridName, gridColumnNames, false);
            DataControlGroup dcGroup = new DataControlGroup();
            dcGroup.SetInnerControls(dcs[0] as Control, dcs[1] as Control);

            return dcs;
        }

        //protected void AssociateDefaultDataControls(Control[] containers)
        //{
        //    IDisplayManager dm = null;
        //    if (this is IDisplayManagerContainer)
        //    {
        //        dm = (this as IDisplayManagerContainer).DisplayManager;
        //    }
        //    else if (this is IControlManagerContainer)
        //    {
        //        dm = (this as IControlManagerContainer).ControlManager.DisplayManager;
        //    }
        //    string gridName = null;
        //    if (this is IGridContainer)
        //    {
        //        gridName = (this as IGridContainer).Grid.GridName;
        //    }
        //    AssociateDataControls(containers, dm, gridName);
        //}

        public IControlManager AssociateDataControlsInControlManager(Control[] containers, string windowTabName)
        {
            var wintabInfo = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            var cm = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(wintabInfo, null);
            AssociateDataControls(containers, cm.DisplayManager, wintabInfo.GridName, false);

            if (cm is IWindowControlManager)
            {
                cm.EditBegun += new EventHandler((sender, e) =>
                    {
                        UpdateContent(cm as IWindowControlManager, wintabInfo.GridName);
                    });
            }
            return cm;
        }
        public IDisplayManager AssociateDataControlsInDisplayManager(Control[] containers, string windowTabName)
        {
            var wintabInfo = ADInfoBll.Instance.GetWindowTabInfo(windowTabName);
            var dm = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(wintabInfo, null);
            AssociateDataControls(containers, dm, wintabInfo.GridName, false);
            return dm;
        }

        public void AssociateDataControls(Control[] containers, IDisplayManager dm, string gridName, bool includeLabel = false)
        {
            AssociateDataControls(containers, dm, gridName, includeLabel, this, m_labels);
            m_dm = dm;
            m_gridNames.Add(gridName);
        }

        private Dictionary<ISearchManager, Control> m_progressInvalidate = new Dictionary<ISearchManager, Control>();
        public void EnableInvalidateAfterSearch(ISearchManager sm, Control invalidateControl)
        {
            sm.DataLoaded += new EventHandler<DataLoadedEventArgs>(searchManager2_DataLoaded);
            m_progressInvalidate[sm] = invalidateControl;
        }
        private void searchManager2_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            ISearchManager sm = sender as ISearchManager;
            if (m_progressInvalidate.ContainsKey(sm))
            {
                Control c = m_progressInvalidate[sm];
                c.Invalidate();
                c.Update();
            }
        }

        //private Dictionary<ISearchManager, ProgressForm> m_progressForms = new Dictionary<ISearchManager,ProgressForm>();
        private List<ISearchManager> m_progressSms = new List<ISearchManager>();
        private int m_progressDoings = 0;
        ProgressForm progressForm;
        public void EnableSearchProgressForm(ISearchManager sm)
        {
            sm.DataLoaded += new EventHandler<DataLoadedEventArgs>(searchManager_DataLoaded);
            sm.DataLoading += new EventHandler<DataLoadingEventArgs>(searchManager_DataLoading);

            m_progressSms.Add(sm);
        }
        public void DisableSearchProgressForm(ISearchManager sm)
        {
            if (sm == null)
            {
                foreach (var i in m_progressSms)
                {
                    sm = i as ISearchManager;
                    if (sm != null)
                    {
                        sm.DataLoaded -= new EventHandler<DataLoadedEventArgs>(searchManager_DataLoaded);
                        sm.DataLoading -= new EventHandler<DataLoadingEventArgs>(searchManager_DataLoading);
                    }
                }
                m_progressSms.Clear();

                if (progressForm != null)
                {
                    progressForm.ProgressStopped -= new EventHandler(progressForm_ProgressStopped);
                    progressForm.Stop();
                    progressForm.Dispose();
                    progressForm = null;
                }
            }
            else
            {
                sm.DataLoaded -= new EventHandler<DataLoadedEventArgs>(searchManager_DataLoaded);
                sm.DataLoading -= new EventHandler<DataLoadingEventArgs>(searchManager_DataLoading);
                m_progressSms.Remove(sm);
            }
        }

        private void searchManager_DataLoading(object sender, DataLoadingEventArgs e)
        {
            if (e.Cancel)
                return;

            ISearchManager sm = sender as ISearchManager;
            if (progressForm == null)
            {
                progressForm = new ProgressForm();
                progressForm.ProgressStopped += new EventHandler(progressForm_ProgressStopped);
            }
            System.Threading.Interlocked.Increment(ref m_progressDoings);
            if (!progressForm.IsActive)
            {
                progressForm.Start(this, "查找");
            }
        }
        private void searchManager_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            ISearchManager sm = sender as ISearchManager;

            System.Threading.Interlocked.Decrement(ref m_progressDoings);
            if (m_progressDoings == 0 && progressForm != null)
            {
                progressForm.ProgressStopped -= new EventHandler(progressForm_ProgressStopped);
                progressForm.Stop();
            }
        }
        void progressForm_ProgressStopped(object sender, EventArgs e)
        {
            foreach (var i in m_progressSms)
            {
                i.StopLoadData();
            }
        }

        //protected void AssociateButtonAdd(Button button, IControlManager cm)
        //{
        //    cm.StateControls.Add(new StateControl(button, true));
        //    button.Click += new EventHandler((sender, e) =>
        //    {
        //        cm.AddNew();
        //        UpdateContent(m_cm, m_controlGroupName);

        //        (m_cm.DisplayManager.CurrentItem as 任务).任务来源 = 任务来源.手工;
        //        (m_cm.DisplayManager.CurrentItem as 任务).IsActive = true;
        //    });
        //}

        public static void RestrictToUserAccess(ISearchManager sm, string adminName)
        {
            if (!Authority.AuthorizeByRule("R:" + adminName))
            {
                string userRestriction = string.Format("CreatedBy = {0}", SystemConfiguration.UserName);
                if (string.IsNullOrEmpty(sm.AdditionalSearchExpression))
                {
                    sm.AdditionalSearchExpression = userRestriction;
                }
                else
                {
                    sm.AdditionalSearchExpression = "(" + sm.AdditionalSearchExpression + ")"
                        + " AND " + userRestriction;
                }
            }
        }

        private Dictionary<string, Label> m_labels = null;
        private static void SearchLabels(Control parentControl, Dictionary<string, Label> labels)
        {
            Label lbl = parentControl as Label;
            if (lbl != null)
            {
                const string labelPrefix = "lbl";
                if (lbl.Name.StartsWith(labelPrefix))
                {
                    labels[lbl.Name.Substring(labelPrefix.Length)] = lbl;
                }
            }
            else
            {
                foreach (Control child in parentControl.Controls)
                {
                    SearchLabels(child, labels);
                }
            }
        }

        public void NotNullControls(IDisplayManager dm)
        {
            NotNullControls(dm, this, m_labels);
        }
        public static void NotNullControls(IDisplayManager dm, Control parentControl, Dictionary<string, Label> labels)
        {
            if (labels == null)
            {
                labels = new Dictionary<string, Label>();
                SearchLabels(parentControl, labels);
            }

            if (!string.IsNullOrEmpty(Const.LabelNotNullPreFix))
            {
                foreach (var dc in dm.DataControls)
                {
                    if (dc != null && dc.NotNull)
                    {
                        if (labels.ContainsKey(dc.Name))
                        {
                            if (!labels[dc.Name].Text.StartsWith(Const.LabelNotNullPreFix))
                            {
                                labels[dc.Name].Text = Const.LabelNotNullPreFix + labels[dc.Name].Text;
                            }
                        }
                    }
                }
            }
        }
        public void ReadOnlyControls(IDisplayManager dm, string[] controlsName, bool readOnly)
        {
            ReadOnlyControls(dm, controlsName, readOnly, this, m_labels);
        }

        public static void ReadOnlyControls(IDisplayManager dm, string[] controlsName, bool readOnly, Control parentControl, Dictionary<string, Label> labels)
        {
            if (labels == null)
            {
                labels = new Dictionary<string, Label>();
                SearchLabels(parentControl, labels);
            }
            foreach (string i in controlsName)
            {
                IDataControl dc = dm.DataControls[i];
                if (dc != null)
                {
                    dc.ReadOnly = readOnly;
                    if (readOnly)
                    {
                        dc.SelectedDataValue = null;
                    }
                }

                if (labels.ContainsKey(i))
                {
                    labels[i].ForeColor = readOnly ? System.Drawing.SystemColors.GrayText : System.Drawing.SystemColors.ControlText;
                }
                else
                {
                    throw new ArgumentException("There is no Label " + i);
                }
            }
        }

        public static void UpdateContent(IWindowControlManager cm, string gridName)
        {
            ArchiveDetailForm.ResetStatusDataControl(cm);
            ArchiveDetailForm.UpdateStatusDataControl(cm, gridName);

            //m_masterCm.DisplayManager.OnPositionChanged(System.EventArgs.Empty);

            if (cm != null)
            {
                if (cm.State == StateType.Add)
                {
                    cm.DisplayManager.DataControls.FocusFirstInsertableControl();

                    ArchiveDetailForm.SetDataControlDefaultValues(cm);
                }
                else if (cm.State == StateType.Edit)
                {
                    cm.DisplayManager.DataControls.FocusFirstEditableControl();
                }
            }
        }
    }
}
