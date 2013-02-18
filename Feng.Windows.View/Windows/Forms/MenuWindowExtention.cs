using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using Feng.Windows.Utils;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    public static class MenuWindowExtention
    {
        public static void GenerateWindowMenu(this IChildMdiForm menuWindow, IList<WindowMenuInfo> windowMenuInfos)
        {
            // WindowMenu
            if (windowMenuInfos != null && windowMenuInfos.Count > 0)
            {
                IList<IButton> tsbs = new List<IButton>();
                ToolStrip toolStripDetail = GenerateWindowMenu(windowMenuInfos, tsbs, GetExistToolStripItems(menuWindow.ToolStrip));
                menuWindow.MergeToolStrip(toolStripDetail);

                menuWindow.SetCustomProperty(s_windowMenuButtonName, tsbs);
                menuWindow.SetCustomProperty("WindowMenuGeneratedToolStrip", toolStripDetail);

                IControlManagerContainer cmContainer = menuWindow as IControlManagerContainer;
                if (cmContainer != null && cmContainer.ControlManager != null)
                {
                    cmContainer.ControlManager.DisplayManager.PositionChanged -= new EventHandler(cmContainer.ControlManager_PositionChanged);
                    cmContainer.ControlManager.EditEnded -= new EventHandler(cmContainer.ControlManager_EditEnded);
                    cmContainer.ControlManager.StateChanged -= new EventHandler(cmContainer.ControlManager_StateChanged);

                    cmContainer.ControlManager.DisplayManager.PositionChanged += new EventHandler(cmContainer.ControlManager_PositionChanged);
                    cmContainer.ControlManager.EditEnded += new EventHandler(cmContainer.ControlManager_EditEnded);
                    cmContainer.ControlManager.StateChanged += new EventHandler(cmContainer.ControlManager_StateChanged);
                }
                else
                {
                    IDisplayManagerContainer dmContainer = menuWindow as IDisplayManagerContainer;
                    if (dmContainer != null && dmContainer.DisplayManager != null)
                    {
                        // only one. DetailWindow.DisplayManager = MasterWindow.DisplayManager. 当时也可能不一致，-=无效。 
                        // // 可能和MasterWindow不是同一个ControlManager
                        dmContainer.DisplayManager.PositionChanged -= new EventHandler(dmContainer.DisplayManager_PositionChanged);
                        dmContainer.DisplayManager.PositionChanged += new EventHandler(dmContainer.DisplayManager_PositionChanged);
                    }
                }
            }
        }

        public static void DisposeWindowMenu(this IChildMdiForm menuWindow)
        {
            IList<IButton> tsbs = menuWindow.GetCustomProperty(s_windowMenuButtonName) as IList<IButton>;
            if (tsbs != null)
            {
                foreach (IButton tsb in tsbs)
                {
                    tsb.Click -= new EventHandler(tsb_Click);
                }
                tsbs.Clear();
            }

            ToolStrip toolStrip = menuWindow.GetCustomProperty("WindowMenuGeneratedToolStrip") as ToolStrip;
            if (toolStrip != null)
            {
                menuWindow.RevertMergeToolStrip(toolStrip);
            }

            IControlManagerContainer cmContainer = menuWindow as IControlManagerContainer;
            if (cmContainer != null && cmContainer.ControlManager != null)
            {
                cmContainer.ControlManager.DisplayManager.PositionChanged -= new EventHandler(cmContainer.ControlManager_PositionChanged);
                cmContainer.ControlManager.EditEnded -= new EventHandler(cmContainer.ControlManager_EditEnded);
                cmContainer.ControlManager.StateChanged -= new EventHandler(cmContainer.ControlManager_StateChanged);
            }
            else
            {
                IDisplayManagerContainer dmContainer = menuWindow as IDisplayManagerContainer;
                if (dmContainer != null && dmContainer.DisplayManager != null)
                {
                    dmContainer.DisplayManager.PositionChanged -= new EventHandler(dmContainer.DisplayManager_PositionChanged);
                }
            }
        }

        internal const string s_windowMenuButtonName = "WindowMenuButtons";
        public static void SetMenuState(this IChildMdiForm menuWindow)
        {
            if (menuWindow == null)
            {
                return;
            }
            IList<IButton> tsbs = menuWindow.GetCustomProperty(s_windowMenuButtonName) as IList<IButton>;
            if (tsbs != null)
            {
                IControlManagerContainer cmContainer = menuWindow as IControlManagerContainer;
                if (cmContainer != null)
                {
                    if (cmContainer != null && cmContainer.ControlManager != null)
                    {
                        foreach (IButton tsb in tsbs)
                        {
                            SetTsbState(tsb, cmContainer.ControlManager);
                        }
                    }
                }
                else
                {
                    IDisplayManagerContainer dmContainer = menuWindow as IDisplayManagerContainer;
                    if (dmContainer != null && dmContainer.DisplayManager != null)
                    {
                        foreach (IButton tsb in tsbs)
                        {
                            SetTsbState(tsb, dmContainer.DisplayManager);
                        }
                    }
                }
            }
        }
        private static void SetTsbState(IButton tsb, IDisplayManager dm)
        {
            object entity = dm.CurrentItem;

            WindowMenuInfo info = tsb.Tag as WindowMenuInfo;
            tsb.Visible = Permission.AuthorizeByRule(info.Visible, entity);
            //if (!tsb.Visible)
            //    continue;

            bool enable = true;
            if (enable)
            {
                enable = Permission.AuthorizeByRule(info.Enable, entity);
            }
            tsb.Enabled = enable;
        }
        private static void SetTsbState(IButton tsb, IControlManager cm)
        {
            // first set enable according state
            string state = cm.State.ToString();

            object entity = cm.DisplayManager.CurrentItem;

            WindowMenuInfo info = tsb.Tag as WindowMenuInfo;
            tsb.Visible = Permission.AuthorizeByRule(info.Visible, entity);
            //if (!tsb.Visible)
            //    continue;

            bool enable = true;
            if (!string.IsNullOrEmpty(info.EnableState))
            {
                enable = info.EnableState.Contains(state);
            }
            if (enable)
            {
                enable = Permission.AuthorizeByRule(info.Enable, entity);
            }
            tsb.Enabled = enable;
        }

        private static System.Windows.Forms.ToolStrip GenerateWindowMenu(IList<WindowMenuInfo> windowMenuInfos, IList<IButton> tsbs, Dictionary<string, ToolStripItem> haveTsbs)
        {
            System.Windows.Forms.ToolStrip toolStrip2 = new System.Windows.Forms.ToolStrip();
            foreach (WindowMenuInfo windowMenuInfo in windowMenuInfos)
            {
                if (windowMenuInfo.Type == WindowMenuType.Separator)
                {
                    toolStrip2.Items.Add(new System.Windows.Forms.ToolStripSeparator());
                }
                else
                {
                    if (string.IsNullOrEmpty(windowMenuInfo.OriginalName))
                    {
                        MyToolStripButton tsb = new MyToolStripButton();
                        tsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                        if (!string.IsNullOrEmpty(windowMenuInfo.ImageName))
                        {
                            tsb.Image = Feng.Windows.ImageResource.Get("Icons." + windowMenuInfo.ImageName + ".png").Reference;
                        }
                        if (tsb.Image == null)
                        {
                            tsb.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconProcess.png").Reference;
                        }
                        tsb.Name = "tsb" + windowMenuInfo.Name;
                        tsb.Text = windowMenuInfo.Text;
                        tsb.ToolTipText = windowMenuInfo.Description;
                        
                        tsb.Tag = windowMenuInfo;
                        tsb.Click += new EventHandler(tsb_Click);
                        toolStrip2.Items.Add(tsb);

                        tsbs.Add(tsb);
                    }
                    else
                    {
                        if (haveTsbs.ContainsKey(windowMenuInfo.OriginalName))
                        {
                            MyToolStripButton tsb = haveTsbs[windowMenuInfo.OriginalName] as MyToolStripButton;
                            if (tsb == null)
                                continue;
                            tsb.DisplayStyle = ToolStripItemDisplayStyle.Image;

                            if (!string.IsNullOrEmpty(windowMenuInfo.ImageName))
                            {
                                tsb.Image = Feng.Windows.ImageResource.Get("Icons." + windowMenuInfo.ImageName + ".png").Reference;
                            }
                            if (tsb.Image == null)
                            {
                                tsb.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconProcess.png").Reference;
                            }
                            tsb.Name = "tsb" + windowMenuInfo.Name;
                            tsb.Text = windowMenuInfo.Text;
                            tsb.ToolTipText = windowMenuInfo.Description;
                            
                            tsb.Tag = windowMenuInfo;

                            EventSuppressor es = new EventSuppressor(tsb);
                            es.Suppress();

                            tsb.Click += new EventHandler(tsb_Click);

                            tsbs.Add(tsb);
                        }
                        else
                        {
                            throw new ArgumentException("There is no ToolStripItem of " + windowMenuInfo.OriginalName + "!");
                        }
                    }
                }
            }
            return toolStrip2;
        }
        

        private static void DisplayManager_PositionChanged(this IDisplayManagerContainer dmContainer, object sender, EventArgs e)
        {
            IDisplayManager dm = sender as IDisplayManager;

            if (dm == dmContainer.DisplayManager)
            {
                SetMenuState(dmContainer as IChildMdiForm);
            }
            //if (this.ArchiveDetailForm != null && dm == this.ArchiveDetailForm.DisplayManager)
            //{
            //    foreach (IButton tsb in m_tsbsDetail)
            //    {
            //        SetTsbState(tsb, this.ArchiveDetailForm.DisplayManager);
            //    }
            //}
        }
        private static void ControlManager_PositionChanged(this IControlManagerContainer cmContainer, object sender, EventArgs e)
        {
            IDisplayManager dm = sender as IDisplayManager;
            if (dm == cmContainer.ControlManager.DisplayManager)
            {
                SetMenuState(cmContainer as IChildMdiForm);
            }
            //if (this.ArchiveDetailForm != null && dm == this.ArchiveDetailForm.DisplayManager)
            //{
            //    foreach (IButton tsb in m_tsbsDetail)
            //    {
            //        SetTsbState(tsb, this.ArchiveDetailForm.ControlManager);
            //    }
            //}
        }

        private static void ControlManager_StateChanged(this IControlManagerContainer cmContainer, object sender, EventArgs e)
        {
            IControlManager cm = sender as IControlManager;
            if (cm == cmContainer.ControlManager)
            {
                SetMenuState(cmContainer as IChildMdiForm);
            }
            //if (this.ArchiveDetailForm != null && cm == this.ArchiveDetailForm.ControlManager)
            //{
            //    foreach (IButton tsb in m_tsbsDetail)
            //    {
            //        SetTsbState(tsb, this.ArchiveDetailForm.ControlManager);
            //    }
            //}
        }

        private static void ControlManager_EditEnded(this IControlManagerContainer cmContainer, object sender, EventArgs e)
        {
            // Todo: Maybe should in ControlManager
            if (cmContainer.ControlManager.DisplayManager.InBatchOperation)
            {
                return;
            }

            IControlManager cm = sender as IControlManager;
            if (cm == cmContainer.ControlManager)
            {
                SetMenuState(cmContainer as IChildMdiForm);
            }
        }

        internal static void OnButtonClick(WindowMenuInfo windowMenuInfo, System.Windows.Forms.Form parentForm)
        {
            IArchiveDetailFormWithDetailGrids addg = parentForm as IArchiveDetailFormWithDetailGrids;
            if (addg != null)
            {
                foreach (IBoundGrid i in addg.DetailGrids)
                {
                    MyGrid.CancelEditCurrentDataRow(i);
                }
            }
            IGridContainer agf = parentForm as IGridContainer;
            if (agf != null)
            {
                MyGrid.CancelEditCurrentDataRow(agf.MasterGrid);
            }

            IControlManagerContainer cmc = parentForm as IControlManagerContainer;
            if (cmc != null)
            {
                InternalExecuteWindowMenu(cmc.ControlManager, windowMenuInfo, parentForm);
            }
            else
            {
                IDisplayManagerContainer dmc = parentForm as IDisplayManagerContainer;
                if (dmc != null)
                {
                    InternalExecuteWindowMenu(dmc.DisplayManager, windowMenuInfo, parentForm);
                    //InternalExecuteWindowMenu((parentForm as ArchiveDetailForm).ParentArchiveForm, windowMenuInfo);
                }
            }
        }
        private static void tsb_Click(object sender, EventArgs e)
        {
            IButton tsb = sender as IButton;
            WindowMenuInfo windowMenuInfo = tsb.Tag as WindowMenuInfo;

            System.Windows.Forms.Form parentForm = null;
            if (tsb is MyToolStripButton)
            {
                parentForm = (tsb as MyToolStripButton).Owner.FindForm();
            }
            else if (tsb is MyButton)
            {
                parentForm = (tsb as MyButton).FindForm();
            }
            else
            {
                throw new NotSupportedException("not supported ibutton.");
            }
            while (!(parentForm.ParentForm is TabbedMdiForm))
            {
                parentForm = parentForm.ParentForm;
            }
            OnButtonClick(windowMenuInfo, parentForm);
        }

        private static Dictionary<string, ToolStripItem> GetExistToolStripItems(ToolStrip toolStrip)
        {
            Dictionary<string, ToolStripItem> ret = new Dictionary<string, ToolStripItem>();
            foreach (ToolStripItem item in toolStrip.Items)
            {
                ret[item.Name] = item;
                foreach (var i in GetExistToolStripItems(item))
                {
                    ret.Add(i.Key, i.Value);
                }
            }
            return ret;
        }
        private static Dictionary<string, ToolStripItem> GetExistToolStripItems(ToolStripItem item)
        {
            Dictionary<string, ToolStripItem> ret = new Dictionary<string, ToolStripItem>();
            ToolStripDropDownItem dropDownItem = item as ToolStripDropDownItem;
            if (dropDownItem != null)
            {
                foreach (ToolStripItem subItem in dropDownItem.DropDownItems)
                {
                    ret[subItem.Name] = subItem;
                    foreach (var i in GetExistToolStripItems(subItem))
                    {
                        ret.Add(i.Key, i.Value);
                    }
                }
            }
            return ret;
        }
        public static bool InternalExecuteWindowMenu(IControlManager cm, WindowMenuInfo info, Form parentForm)
        {
            bool b = InternalExecuteWindowMenu2(cm, info, parentForm);
            if (!b)
            {
                return InternalExecuteWindowMenu(cm.DisplayManager, info, parentForm);
            }
            return true;
        }

        public static bool InternalExecuteWindowMenu(IDisplayManager dm, WindowMenuInfo info, Form parentForm)
        {
            object entity = dm.CurrentItem;
            int pos = dm.Position;
            //ArchiveOperationForm opForm = masterForm as ArchiveOperationForm;
            switch (info.Type)
            {
                case WindowMenuType.ReportSingle:
                    {
                        if (entity == null)
                        {
                            MessageForm.ShowError("请选择要打印的项！");
                            break;
                        }

                        ProgressAsyncHelper asyncHelper = new ProgressAsyncHelper(
                            new Feng.Async.AsyncHelper.DoWork(delegate()
                            {
                                MyReportForm form = new MyReportForm(info.ExecuteParam);
                                form.FillDataSet(entity);
                                return form;
                            }),
                            new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                            {
                                if (result != null)
                                {
                                    MyReportForm form = result as MyReportForm;
                                    form.Show(parentForm);
                                }
                            }),
                            parentForm, "生成");
                    }
                    break;
                case WindowMenuType.ReportMulti:
                    {
                        if (dm.Count == 0)
                        {
                            MessageForm.ShowError("请选择要打印的项！");
                            break;
                        }
                        object[] entities = new object[dm.Count];
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            entities[i] = dm.Items[i];
                        }
                        ProgressAsyncHelper asyncHelper = new ProgressAsyncHelper(
                            new Feng.Async.AsyncHelper.DoWork(delegate()
                            {
                                MyReportForm form = new MyReportForm(info.ExecuteParam);
                                form.FillDataSet(entities);
                                return form;
                            }),
                            new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                            {
                                if (result != null)
                                {
                                    MyReportForm form = result as MyReportForm;
                                    form.Show(parentForm);
                                }
                            }),
                            parentForm, "生成");
                    }
                    break;
                case WindowMenuType.MsReportSingle:
                    {
                        if (entity == null)
                        {
                            MessageForm.ShowError("请选择要打印的项！");
                            break;
                        }

                        ProgressAsyncHelper asyncHelper = new ProgressAsyncHelper(
                            new Feng.Async.AsyncHelper.DoWork(delegate()
                            {
                                MsReportForm form = new MsReportForm(info.ExecuteParam);
                                form.FillDataSet(entity);
                                return form;
                            }),
                            new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                            {
                                if (result != null)
                                {
                                    MsReportForm form = result as MsReportForm;
                                    form.Show(parentForm);

                                    // 焦点会转变到其他程序，只能这样
                                    form.FormClosed += new FormClosedEventHandler(delegate(object sender, FormClosedEventArgs e)
                                    {
                                        parentForm.ParentForm.Activate();
                                    });
                                }
                            }),
                            parentForm, "生成");
                    }
                    break;
                case WindowMenuType.MsReportMulti:
                    {
                        if (dm.Count == 0)
                        {
                            MessageForm.ShowError("请选择要打印的项！");
                            break;
                        }
                        object[] entities = new object[dm.Count];
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            entities[i] = dm.Items[i];
                        }

                        ProgressAsyncHelper asyncHelper = new ProgressAsyncHelper(
                            new Feng.Async.AsyncHelper.DoWork(delegate()
                            {
                                MsReportForm form = new MsReportForm(info.ExecuteParam);
                                form.FillDataSet(entities);
                                return form;
                            }),
                            new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                            {
                                if (result != null)
                                {
                                    MsReportForm form = result as MsReportForm;
                                    form.Show(parentForm);

                                    form.FormClosed += new FormClosedEventHandler(delegate(object sender, FormClosedEventArgs e)
                                    {
                                        parentForm.ParentForm.Activate();
                                    });
                                }

                            }),
                            parentForm, "生成");
                    }
                    break;
                case WindowMenuType.DatabaseCommand:
                    {
                        ProgressAsyncHelper asyncHelper = new ProgressAsyncHelper(
                            new Feng.Async.AsyncHelper.DoWork(delegate()
                            {
                                Feng.Data.DbHelper.Instance.ExecuteNonQuery(info.ExecuteParam);
                                return null;
                            }),
                            new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                            {
                            }),
                            parentForm, "执行");
                    }
                    break;
                case WindowMenuType.DatabaseCommandMulti:
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < dm.Count; ++i)
                        {
                            string s = EntityHelper.ReplaceEntity(info.ExecuteParam, dm.Items[i]);
                            sb.Append(s);
                            sb.Append(System.Environment.NewLine);
                        }
                        ProgressAsyncHelper asyncHelper = new ProgressAsyncHelper(
                            new Feng.Async.AsyncHelper.DoWork(delegate()
                            {
                                Feng.Data.DbHelper.Instance.ExecuteNonQuery(sb.ToString());
                                return null;
                            }),
                            new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                            {
                            }),
                            parentForm, "执行");
                    }
                    break;
                case WindowMenuType.DatabaseCommandMultiParam:
                    {
                        object[] entities = new object[dm.Count];
                        for (int i = 0; i < dm.Count; ++i)
                        {
                            entities[i] = dm.Items[i];
                        }
                        string s = EntityHelper.ReplaceEntities(info.ExecuteParam, entities, '\'');

                        ProgressAsyncHelper asyncHelper = new ProgressAsyncHelper(
                            new Feng.Async.AsyncHelper.DoWork(delegate()
                            {
                                Feng.Data.DbHelper.Instance.ExecuteNonQuery(s);
                                return null;
                            }),
                            new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                            {
                            }),
                            parentForm, "执行");

                    }
                    break;
                case WindowMenuType.Process:
                    {
                        ProcessInfoHelper.ExecuteProcess(info.ExecuteParam, new Dictionary<string, object> { { "masterForm", parentForm } });
                    }
                    break;
                case WindowMenuType.Action:
                    {
                        ServiceProvider.GetService<IApplication>().ExecuteAction(info.ExecuteParam);
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        private static bool InternalExecuteWindowMenu2(IControlManager cm, WindowMenuInfo info, Form parentForm)
        {
            object entity = cm.DisplayManager.CurrentItem;
            int pos = cm.DisplayManager.Position;
            //ArchiveOperationForm opForm = masterForm as ArchiveOperationForm;
            switch (info.Type)
            {
                case WindowMenuType.Add:
                    {
                        ArchiveOperationForm.DoAddS(cm);
                    }
                    break;
                case WindowMenuType.Edit:
                    {
                        ArchiveOperationForm.DoEditS(cm, (parentForm as IGridNamesContainer).GridNames[0]);
                    }
                    break;
                case WindowMenuType.Delete:
                    {
                        ArchiveOperationForm.DoDeleteS(cm, (parentForm as IGridNamesContainer).GridNames[0]);
                    }
                    break;
                case WindowMenuType.Confirm:
                    {
                        parentForm.ValidateChildren();
                        ArchiveDetailForm.DoSaveS(cm);
                    }
                    break;
                case WindowMenuType.Cancel:
                    {
                        ArchiveDetailForm.DoCancelS(cm);
                    }
                    break;
                case WindowMenuType.Submit:
                    {
                        if (entity == null)
                        {
                            MessageForm.ShowError("请选择要提交的项！");
                            return true;
                        }
                        if (!MessageForm.ShowYesNo("是否确认提交？"))
                            return true;

                        ISubmittedEntity se = entity as ISubmittedEntity;
                        if (se == null)
                        {
                            throw new ArgumentException("Submit Entity should be ISubmittedEntity!");
                        }
                        if (string.IsNullOrEmpty(info.ExecuteParam))
                        {
                            cm.EditCurrent();
                            se.Submitted = true;
                            cm.EndEdit();
                        }
                        else
                        {
                            ISubmittedEntityDao dao = Feng.Utils.ReflectionHelper.CreateInstanceFromName(info.ExecuteParam) as ISubmittedEntityDao;
                            if (dao == null)
                            {
                                throw new ArgumentException("Submit windowMenuType's ExecuteParam should be ISubmittedEntityDao!");
                            }

                            using (IRepository rep = dao.GenerateRepository())
                            {
                                try
                                {
                                    se.Submitted = true;
                                    rep.BeginTransaction();
                                    dao.Submit(rep, entity);
                                    rep.CommitTransaction();
                                    cm.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
                                }
                                catch (Exception ex)
                                {
                                    se.Submitted = false;
                                    rep.RollbackTransaction();
                                    ExceptionProcess.ProcessWithNotify(ex);
                                }
                            }
                        }
                    }
                    break;
                case WindowMenuType.Unsubmit:
                    {
                        if (entity == null)
                        {
                            MessageForm.ShowError("请选择要撤销提交的项！");
                            return true;
                        }
                        if (!MessageForm.ShowYesNo("是否确认撤销提交？", "确认", true))
                            return true;

                        ISubmittedEntity se = entity as ISubmittedEntity;
                        if (se == null)
                        {
                            throw new ArgumentException("Submit Entity should be ISubmittedEntity!");
                        }
                        if (string.IsNullOrEmpty(info.ExecuteParam))
                        {
                            cm.EditCurrent();
                            se.Submitted = false;
                            cm.EndEdit();
                        }
                        else
                        {
                            ISubmittedEntityDao dao = Feng.Utils.ReflectionHelper.CreateInstanceFromName(info.ExecuteParam) as ISubmittedEntityDao;
                            if (dao == null)
                            {
                                throw new ArgumentException("Submit windowMenuType's ExecuteParam should be ISubmittedEntityDao!");
                            }

                            using (IRepository rep = dao.GenerateRepository())
                            {
                                try
                                {
                                    se.Submitted = false;
                                    rep.BeginTransaction();
                                    dao.Unsubmit(rep, entity);
                                    rep.CommitTransaction();
                                    cm.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
                                }
                                catch (Exception ex)
                                {
                                    se.Submitted = true;
                                    rep.RollbackTransaction();
                                    ExceptionProcess.ProcessWithNotify(ex);
                                }
                            }
                        }
                    }
                    break;
                case WindowMenuType.SubmitMulti:
                    {
                        if (cm.DisplayManager.Count == 0)
                        {
                            MessageForm.ShowError("请选择要提交的项！");
                            return true;
                        }
                        if (!MessageForm.ShowYesNo("是否确认提交（当前全部）？"))
                            return true;

                        ISubmittedEntity se = entity as ISubmittedEntity;
                        if (se == null)
                        {
                            throw new ArgumentException("Submit Entity should be ISubmittedEntity!");
                        }
                        if (string.IsNullOrEmpty(info.ExecuteParam))
                        {
                            IBatchDao batchDao = cm.Dao as IBatchDao;
                            if (batchDao != null)
                            {
                                batchDao.SuspendOperation();
                            }

                            for (int i = 0; i < cm.DisplayManager.Count; ++i)
                            {
                                cm.DisplayManager.Position = i;
                                cm.EditCurrent();
                                (cm.DisplayManager.Items[i] as ISubmittedEntity).Submitted = true;
                                cm.EndEdit();
                            }

                            if (batchDao != null)
                            {
                                batchDao.ResumeOperation();
                            }
                        }
                        else
                        {
                            ISubmittedEntityDao dao = Feng.Utils.ReflectionHelper.CreateInstanceFromName(info.ExecuteParam) as ISubmittedEntityDao;
                            if (dao == null)
                            {
                                throw new ArgumentException("Submit windowMenuType's ExecuteParam should be ISubmittedEntityDao!");
                            }

                            using (IRepository rep = dao.GenerateRepository())
                            {
                                try
                                {
                                    rep.BeginTransaction();
                                    for (int i = 0; i < cm.DisplayManager.Count; ++i)
                                    {
                                        (cm.DisplayManager.Items[i] as ISubmittedEntity).Submitted = true;
                                        dao.Submit(rep, cm.DisplayManager.Items[i]);
                                    }
                                    rep.CommitTransaction();
                                    cm.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
                                }
                                catch (Exception ex)
                                {
                                    se.Submitted = false;
                                    rep.RollbackTransaction();
                                    ExceptionProcess.ProcessWithNotify(ex);
                                }
                            }
                        }
                    }
                    break;
                case WindowMenuType.Cancellate:
                    {
                        if (entity == null)
                        {
                            MessageForm.ShowError("请选择要作废的项！");
                            return true;
                        }
                        if (!MessageForm.ShowYesNo("是否确认作废？", "确认", true))
                            return true;

                        ICancellateDao dao = Feng.Utils.ReflectionHelper.CreateInstanceFromName(info.ExecuteParam) as ICancellateDao;
                        if (dao == null)
                        {
                            throw new ArgumentException("Submit windowMenuType's ExecuteParam should be ICancellateDao!");
                        }
                        using (IRepository rep = dao.GenerateRepository())
                        {
                            try
                            {
                                rep.BeginTransaction();
                                dao.Cancellate(rep, entity);
                                rep.CommitTransaction();
                                cm.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
                            }
                            catch (Exception ex)
                            {
                                rep.RollbackTransaction();
                                ExceptionProcess.ProcessWithNotify(ex);
                            }
                        }
                    }
                    break;
                case WindowMenuType.DaoProcess:
                    {
                        if (entity == null)
                        {
                            MessageForm.ShowError("请选择要操作的项！");
                            return true;
                        }
                        if (!MessageForm.ShowYesNo("是否要执行" + info.Text + "？"))
                            return true;

                        string[] ss = info.ExecuteParam.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length != 2)
                        {
                            throw new ArgumentException("DaoProcess windowMenuType's ExecuteParam should be IDao;MethodName!");
                        }
                        IRepositoryDao dao = Feng.Utils.ReflectionHelper.CreateInstanceFromName(ss[0].Trim()) as IRepositoryDao;
                        if (dao == null)
                        {
                            throw new ArgumentException("DaoProcess windowMenuType's ExecuteParam's first part should be IDao!");
                        }

                        using (IRepository rep = dao.GenerateRepository())
                        {
                            try
                            {
                                rep.BeginTransaction();
                                Feng.Utils.ReflectionHelper.RunInstanceMethod(ss[0].Trim(), ss[1].Trim(), dao, new object[] { rep, entity });
                                rep.CommitTransaction();
                                cm.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, pos));
                            }
                            catch (Exception ex)
                            {
                                rep.RollbackTransaction();
                                ExceptionProcess.ProcessWithNotify(ex);
                            }
                        }
                    }
                    break;
                case WindowMenuType.Select:
                    {
                        string[] ss = info.ExecuteParam.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length == 0)
                        {
                            throw new ArgumentException("WindowMenu's ExecuteParam is Invalid of WindowMenu " + info.Name);
                        }
                        ArchiveCheckForm form = ServiceProvider.GetService<IWindowFactory>().CreateWindow(ADInfoBll.Instance.GetWindowInfo(ss[0])) as ArchiveCheckForm;
                        if (ss.Length > 1)
                        {
                            string exp = ss[1];
                            exp = EntityHelper.ReplaceEntity(exp, new EntityHelper.GetReplaceValue(delegate(string paramName)
                            {
                                Tuple<string, object> t = EventProcessUtils.GetDataControlValue(paramName, cm.DisplayManager);
                                if (t.Item2 == null)
                                {
                                    throw new InvalidUserOperationException(string.Format("请先填写{0}!", paramName));
                                }
                                cm.DisplayManager.DataControls[t.Item1].ReadOnly = true;
                                // save controlValue to entity because readonly will not save
                                EntityScript.SetPropertyValue(cm.DisplayManager.CurrentItem, t.Item1, t.Item2);

                                return t.Item2;
                            }));
                            if (string.IsNullOrEmpty(form.DisplayManager.SearchManager.AdditionalSearchExpression))
                            {
                                form.DisplayManager.SearchManager.AdditionalSearchExpression = exp;
                            }
                            else
                            {
                                form.DisplayManager.SearchManager.AdditionalSearchExpression = "(" + form.DisplayManager.SearchManager.AdditionalSearchExpression + ") and " + exp;
                            }
                        }

                        int detailGridIdx = 0;
                        if (ss.Length > 2)
                        {
                            detailGridIdx = Feng.Utils.ConvertHelper.ToInt(ss[2]).Value;
                        }
                        if (form.ShowDialog(parentForm) == System.Windows.Forms.DialogResult.OK)
                        {
                            IControlManager detailCm = (((IArchiveDetailFormWithDetailGrids)(parentForm as IArchiveMasterForm).ArchiveDetailForm).DetailGrids[detailGridIdx] as IArchiveGrid).ControlManager;

                            var nowList = detailCm.DisplayManager.Items;
                            foreach (object i in form.SelectedEntites)
                            {
                                if (nowList.Contains(i))
                                    continue;

                                detailCm.AddNew();
                                detailCm.DisplayManager.Items[detailCm.DisplayManager.Position] = i;
                                detailCm.EndEdit();
                            }
                        }
                        form.Dispose();
                    }
                    break;
                case WindowMenuType.Input:
                    throw new NotSupportedException("Not supported now!");
                case WindowMenuType.ManyToOneWindow:
                    {
                        if (cm.DisplayManager.CurrentItem == null)
                        {
                            MessageForm.ShowInfo("无当前项，不能操作！");
                            return true;
                        }
                        string[] ss = info.ExecuteParam.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length < 2)
                        {
                            throw new ArgumentException("WindowMenu's ExecuteParam is Invalid of WindowMenu " + info.Name);
                        }
                        ArchiveDetailForm selectForm = ServiceProvider.GetService<IWindowFactory>().CreateWindow(ADInfoBll.Instance.GetWindowInfo(ss[0])) as ArchiveDetailForm;
                        string propertyName = ss[1];
                        object masterEntity = EntityScript.GetPropertyValue(cm.DisplayManager.CurrentItem, propertyName);
                        if (masterEntity == null)
                        {
                            ArchiveOperationForm.DoAddS(selectForm.ControlManager);
                            selectForm.UpdateContent();
                            if (selectForm.ShowDialog(parentForm) == System.Windows.Forms.DialogResult.OK)
                            {
                                cm.EditCurrent();
                                EntityScript.SetPropertyValue(cm.DisplayManager.CurrentItem, propertyName, selectForm.DisplayManager.CurrentItem);
                                cm.EndEdit();

                                cm.OnCurrentItemChanged();
                            }
                        }
                        else
                        {
                            selectForm.ControlManager.AddNew();
                            selectForm.DisplayManager.Items[selectForm.DisplayManager.Position] = masterEntity;
                            selectForm.ControlManager.EndEdit(false);
                            ArchiveOperationForm.DoEditS(selectForm.ControlManager, selectForm.GridName);
                            selectForm.UpdateContent();

                            if (selectForm.ShowDialog(parentForm) == System.Windows.Forms.DialogResult.OK)
                            {
                                ((parentForm as IArchiveMasterForm).MasterGrid as IBoundGrid).ReloadData();
                            }
                        }
                        selectForm.Dispose();
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}
