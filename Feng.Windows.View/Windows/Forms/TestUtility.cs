using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public class TestUtility
    {
        private static StringExceptionProcess m_stringExceptionProcess = new StringExceptionProcess();
        private static EmptyMessageBox m_emptyMessageBox = new EmptyMessageBox();
        public static void TestAll()
        {
            SystemConfiguration.UseMultiThread = false;

            //if (MessageForm.ShowYesNo("Operate Data?"))
            {
                operateData = true;
            }

            m_stringExceptionProcess.Clear();

            var instance = DefaultServiceProvider.Instance;
            IExceptionProcess defaultEp = null;
            IMessageBox defaultMb = null;
            if (instance != null)
            {
                defaultEp = ServiceProvider.GetService<IExceptionProcess>();
                instance.SetDefaultService<IExceptionProcess>(m_stringExceptionProcess);
                defaultMb = ServiceProvider.GetService<IMessageBox>();
                instance.SetDefaultService<IMessageBox>(m_emptyMessageBox);
                ServiceProvider.Clear();
            }

            IList<MenuInfo> menuInfos = ADInfoBll.Instance.GetTopMenuInfos();
            foreach (MenuInfo info in menuInfos)
            {
                TestMenu(info);
            }

            if (instance != null)
            {
                instance.SetDefaultService<IExceptionProcess>(defaultEp);
                instance.SetDefaultService<IMessageBox>(defaultMb);
                ServiceProvider.Clear();
            }

            SystemConfiguration.UseMultiThread = true;

            ErrorReport errorReport = new ErrorReport("测试中的异常，请见详细内容。", "测试信息", MessageImage.Information, m_stringExceptionProcess.Exceptions);
            errorReport.ShowDialog();
        }

        private static void TestMenu(MenuInfo menuInfo)
        {
            if (menuInfo.Name.StartsWith("System") || menuInfo.Name.StartsWith("报表"))
            {
                return;
            }
            m_stringExceptionProcess.AddLog("Now execute menu of " + menuInfo.Name);

            if (menuInfo.ChildMenus.Count == 0)
            {
                if (menuInfo.Action != null)
                {
                    ActionInfo actionInfo = ADInfoBll.Instance.GetActionInfo(menuInfo.Action.Name);
                    TestAction(actionInfo);
                }
            }
            else
            {
                foreach (MenuInfo subMenu in menuInfo.ChildMenus)
                {
                    TestMenu(subMenu);
                }
            }
        }
        private static bool operateData = false;
        private static bool haveTestStandartTsb = false;
        private static void TestAction(ActionInfo actionInfo)
        {
            switch (actionInfo.ActionType)
            {
                case ActionType.Window:
                    {
                        WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(actionInfo.Window.Name);
                        if (windowInfo == null)
                        {
                            throw new ArgumentException("Invalid WindowID in menuInfo");
                        }
                        MyChildForm form = null;
                        try
                        {
                            form = ServiceProvider.GetService<IWindowFactory>().CreateWindow(windowInfo) as MyChildForm;
                            form.WindowState = FormWindowState.Maximized;
                            var mdiForm = ServiceProvider.GetService<IApplication>() as TabbedMdiForm;
                            //form.Show();
                            mdiForm.ShowChildForm(form);
                            TestWindow(form);
                        }
                        catch (Exception ex)
                        {
                            ExceptionProcess.ProcessWithNotify(ex);
                        }
                        finally
                        {
                            if (form != null)
                            {
                                form.Close();
                            }
                        }
                    }
                    break;
                case ActionType.Form:
                    {
                        FormInfo formInfo = null;
                        if (actionInfo.Form != null)
                        {
                            formInfo = ADInfoBll.Instance.GetFormInfo(actionInfo.Form.Name);
                        }

                        if (formInfo == null)
                        {
                            throw new ArgumentException("Invalid FormInfo in menuInfo");
                        }
                        Form form = null;
                        try
                        {
                            form = ArchiveFormFactory.CreateForm(formInfo);
                            form.WindowState = FormWindowState.Maximized;
                            var mdiForm = ServiceProvider.GetService<IApplication>() as TabbedMdiForm;
                            //form.Show();
                            if (form is MyChildForm)
                            {
                                mdiForm.ShowChildForm(form as MyChildForm);
                            }
                            else
                            {
                                form.Show();
                            }
                            TestForm(form);
                        }
                        catch (Exception ex)
                        {
                            ExceptionProcess.ProcessWithNotify(ex);
                        }
                        finally
                        {
                            if (form != null)
                            {
                                form.Close();
                            }
                        }
                    }
                    break;
                case ActionType.Process:
                    break;
                case ActionType.Url:
                    break;
                default:
                    throw new ArgumentException("menuInfo's MenuAction is Invalid");
            }
        }
        internal static void TestFormControl(Control parent)
        {
            if (parent is Button)
            {
                (parent as Button).PerformClick();
            }
            else
            {
                foreach (Control i in parent.Controls)
                {
                    TestFormControl(i);
                }
            }
        }
        internal static void TestForm(Form form)
        {
            TestFormControl(form);
        }
        internal static void TestWindow(MyForm form)
        {
            if (form == null)
                return;

            if (form is ArchiveSeeForm)
            {
                var opForm = form as IArchiveMasterForm;

                opForm.DisplayManager.SearchManager.MaxResult = 1;

                SetSearchControlsValues(opForm.DisplayManager.SearchManager);
                opForm.DisplayManager.SearchManager.LoadDataAccordSearchControls();
                opForm.DisplayManager.SearchManager.WaitLoadData();

                SetSearchControlsValues(opForm.DisplayManager.SearchManager, true);
                opForm.DisplayManager.SearchManager.LoadDataAccordSearchControls();
                opForm.DisplayManager.SearchManager.WaitLoadData();

                if (opForm.DisplayManager.Count > 0 && opForm.ArchiveDetailForm != null)
                {
                    ArchiveSeeForm.DoView(opForm);
                }

                foreach (Xceed.Grid.DataRow row in opForm.MasterGrid.DataRows)
                {
                    ExpandDataRow(row);
                }

                if (form is ArchiveOperationForm)
                {
                    var opForm2 = form as ArchiveOperationForm;
                    if (opForm2 != null && opForm2.ArchiveDetailForm != null)
                    {
                        if (operateData)
                        {
                            if (opForm2.ArchiveDetailForm is ArchiveDetailForm)
                            {
                                if (opForm2.ControlManager.AllowInsert)
                                {
                                    opForm2.DoAdd();
                                    (opForm2.ArchiveDetailForm as ArchiveDetailForm).DoCancel();
                                }

                                if (opForm2.DisplayManager.Count > 0)
                                {
                                    if (opForm2.ControlManager.AllowEdit)
                                    {
                                        opForm2.DisplayManager.Position = 0;
                                        opForm2.DoEdit();
                                        (opForm2.ArchiveDetailForm as ArchiveDetailForm).DoCancel();

                                        opForm2.DoEdit();
                                        (opForm2.ArchiveDetailForm as ArchiveDetailForm).DoSave();
                                    }
                                }
                            }

                            if (opForm2.DisplayManager.Count > 0)
                            {
                                if (opForm2.ControlManager.AllowDelete)
                                {
                                    opForm2.DisplayManager.Position = 0;
                                    opForm2.DoDelete();
                                }
                            }
                        }
                    }
                }
            }
            else if (form is ArchiveDataSetReportForm)
            {
                var opForm = form as ArchiveDataSetReportForm;

                opForm.SearchManager.MaxResult = 1;

                SetSearchControlsValues(opForm.SearchManager);
                opForm.SearchManager.LoadDataAccordSearchControls();
                opForm.SearchManager.WaitLoadData();

                SetSearchControlsValues(opForm.SearchManager, true);
                opForm.SearchManager.LoadDataAccordSearchControls();
                opForm.SearchManager.WaitLoadData();
            }
            else if (form is ArchiveDatabaseReportForm)
            {
                var opForm = form as ArchiveDatabaseReportForm;
                opForm.SearchManager.MaxResult = 1;

                SetSearchControlsValues(opForm.SearchManager);
                opForm.SearchManager.LoadDataAccordSearchControls();
                opForm.SearchManager.WaitLoadData();

                SetSearchControlsValues(opForm.SearchManager, true);
                opForm.SearchManager.LoadDataAccordSearchControls();
                opForm.SearchManager.WaitLoadData();
            }

            foreach (ToolStripItem i in form.ToolStrip.Items)
            {
                if (i.Name == "tsbSearch" || i.Name == "tsbFind" || i.Name == "tsbRelatedInfo"
                    || i.Name == "tsbAttachment" || i.Name == "tsbExportExcel" || i.Name == "tsbPrintPreview"
                    || i.Name == "tsbGenerateReport" || i.Name == "tsbSetup" || i.Name == "tsbRefresh"
                    || i.Name == "tsbView" || i.Name == "tsbFilter" || i.Name == "tsbGroup"
                    || i.Name.StartsWith("tss") || i.Name.StartsWith("binding"))
                {
                    if (haveTestStandartTsb)
                        continue;
                }
                i.PerformClick();
            }
            haveTestStandartTsb = true;
        }

        private static void ExpandDataRow(Xceed.Grid.DataRow row)
        {
            if (row.DetailGrids.Count > 0)
            {
                foreach (Xceed.Grid.DetailGrid detailGrid in row.DetailGrids)
                {
                    detailGrid.Expand();
                    foreach (Xceed.Grid.DataRow subRow in detailGrid.DataRows)
                    {
                        ExpandDataRow(subRow);
                    }
                }
            }
        }
        private static void SetSearchControlsValues(ISearchManager sm, bool allEmpty = false)
        {
            foreach (ISearchControl sc in sm.SearchControls)
            {
                if (allEmpty)
                {
                    sc.SelectedDataValues = null;
                }
                else
                {
                    IWindowControl wc = sc as IWindowControl;
                    if (wc != null)
                    {
                        MyComboBox c = wc.Control as MyComboBox;
                        if (c != null && c.Items.Count > 0)
                        {
                            c.SelectedIndex = 0;
                            continue;
                        }
                        else
                        {
                            MyOptionPicker op = wc.Control as MyOptionPicker;
                            if (op != null && op.DropDownControl.DataRows.Count > 0)
                            {
                                op.DropDownControl.DataRows[0].Cells[Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName].Value = true;
                                continue;
                            }
                        }
                    }
                    System.Collections.ArrayList arr = new System.Collections.ArrayList { };
                    if (sc.ResultType == typeof(DateTime))
                    {
                        arr.Add(System.DateTime.Today);
                    }
                    else if (sc.ResultType == typeof(int))
                    {
                        arr.Add(1);
                    }
                    else if (sc.ResultType == typeof(long))
                    {
                        arr.Add(1L);
                    }
                    else if (sc.ResultType == typeof(string))
                    {
                        arr.Add("1");
                    }
                    else if (sc.ResultType == typeof(double))
                    {
                        arr.Add(1.1d);
                    }
                    else if (sc.ResultType == typeof(decimal))
                    {
                        arr.Add(1.1m);
                    }
                    else if (sc.ResultType == typeof(bool))
                    {
                        arr.Add(true);
                    }

                    sc.SelectedDataValues = arr;
                }
            }
        }

        public static void OpenAllMenus()
        {
            TabbedMdiForm mdiForm = ServiceProvider.GetService<IApplication>() as TabbedMdiForm; 
            if (mdiForm == null)
                return;

            try
            {
                foreach (ToolStripMenuItem menuItem in mdiForm.MainMenuStrip.Items)
                {
                    if (menuItem.Text == "功能(&F)")
                    {
                        //Thread t = new Thread(new ParameterizedThreadStart(PerformClickMenu));
                        //t.Start(menuItem);
                        PerformClickMenu(menuItem);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }

        private static void PerformClickMenu(object o)
        {
            ToolStripMenuItem menuItem = o as ToolStripMenuItem;
            if (menuItem.Text.Contains("忽略"))
                return;

            menuItem.PerformClick();

            TabbedMdiForm mdiForm = ServiceProvider.GetService<IApplication>() as TabbedMdiForm; 
            try
            {
                foreach (Form form in mdiForm.MdiChildren)
                {
                    if (form is ArchiveSeeForm)
                    {
                        (form as ArchiveSeeForm).DisplayManager.SearchManager.LoadDataAccordSearchControls();
                        if ((form as ArchiveSeeForm).DisplayManager.Count > 0
                            && (form as ArchiveSeeForm).ArchiveDetailForm != null)
                        {
                            (form as ArchiveSeeForm).DoView();
                        }
                    }
                    //if (form is ArchiveOperationForm)
                    //{
                    //    (form as ArchiveOperationForm).DoAdd();
                    //}
                }

                foreach (Form form in mdiForm.MdiChildren)
                {
                    form.Close();
                    form.Dispose();
                }

                if (menuItem is ToolStripDropDownItem)
                {
                    foreach (ToolStripMenuItem subItem in ((ToolStripDropDownItem)menuItem).DropDownItems)
                    {
                        PerformClickMenu(subItem);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(new ArgumentException(menuItem.Text + "出错", ex));
            }
        }

        internal static void UpgradeAdDb()
        {
            // ControlManager, DisplayManager, SearchManager, Dao
            DaoHelper.DoInRepository( (rep) =>
                {
                    var list = rep.List<WindowTabInfo>();
                    foreach (var i in list)
                    {
                        if (!string.IsNullOrEmpty(i.ControlManagerClassName))
                        {
                            if (i.ControlManagerClassName.Contains("Feng.Windows.Forms.ControlManager,"))
                                i.ControlManagerClassName = "UNTYPED";
                            else if (i.ControlManagerClassName.Contains("Feng.Windows.Forms.ControlManager`1[["))
                            {
                                int idx = i.ControlManagerClassName.IndexOf("[[");
                                int idx2 = i.ControlManagerClassName.IndexOf("]]", idx);
                                i.ModelClassName = i.ControlManagerClassName.Substring(idx + 2, idx2 - idx - 2);
                                i.ControlManagerClassName = "TYPED";
                            }
                        }

                        if (!string.IsNullOrEmpty(i.DisplayManagerClassName))
                        {
                            if (i.DisplayManagerClassName.Contains("Feng.Windows.Forms.DisplayManager,"))
                                i.DisplayManagerClassName = "UNTYPED";
                            else if (i.DisplayManagerClassName.Contains("Feng.Windows.Forms.DisplayManager`1[["))
                            {
                                int idx = i.DisplayManagerClassName.IndexOf("[[");
                                int idx2 = i.DisplayManagerClassName.IndexOf("]]", idx);
                                i.ModelClassName = i.DisplayManagerClassName.Substring(idx + 2, idx2 - idx - 2);
                                i.DisplayManagerClassName = "TYPED";
                            }
                        }

                        if (!string.IsNullOrEmpty(i.BusinessLayerClassName))
                        {
                            if (i.BusinessLayerClassName.Contains("DataTableDao"))
                                i.BusinessLayerClassName = "Feng.Data.DataTableDao, Feng.Windows.Controller";
                            else
                                i.BusinessLayerClassName = i.BusinessLayerClassName.Replace(", Feng.Controller", ", Feng.Dao");
                        }

                        if (!string.IsNullOrEmpty(i.SearchManagerClassName))
                        {
                            if (i.SearchManagerClassName.Contains("Feng.Data.SearchManager,"))
                                i.SearchManagerClassName = "UNTYPED";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerProcedure"))
                                i.SearchManagerClassName = "UNTYPEDPROCEDURE";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerFunction"))
                                i.SearchManagerClassName = "UNTYPEDFUNCTION";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerGroupByDetail"))
                                i.SearchManagerClassName = "UNTYPEDGROUPBYDETAIL";
                            else if (i.SearchManagerClassName.Contains("Feng.NH.SearchManager`1[["))
                            {
                                int idx = i.SearchManagerClassName.IndexOf("[[");
                                int idx2 = i.SearchManagerClassName.IndexOf("]]", idx);
                                i.ModelClassName = i.SearchManagerClassName.Substring(idx + 2, idx2 - idx - 2);
                                i.SearchManagerClassName = "TYPED";
                            }
                            else
                                i.SearchManagerClassName = i.SearchManagerClassName.Replace(", Feng.Controller", ", Feng.Windows.Controller");
                        }
                        rep.Update(i);
                    }
                });
            DaoHelper.DoInRepository( (rep) =>
                {
                    var list2 = rep.List<ReportDataInfo>();
                    foreach (var i in list2)
                    {
                        if (!string.IsNullOrEmpty(i.SearchManagerClassName))
                        {
                            if (i.SearchManagerClassName.Contains("Feng.Data.SearchManager,"))
                                i.SearchManagerClassName = "UNTYPED";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerProcedure"))
                                i.SearchManagerClassName = "UNTYPEDPROCEDURE";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerFunction"))
                                i.SearchManagerClassName = "UNTYPEDFUNCTION";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerGroupByDetail"))
                                i.SearchManagerClassName = "UNTYPEDGROUPBYDETAIL";
                            else
                                i.SearchManagerClassName = i.SearchManagerClassName.Replace(", Feng.Controller", ", Feng.Windows.Controller");
                            rep.Update(i);
                        }
                    }
                });

            DaoHelper.DoInRepository( (rep) =>
                {
                    var list3 = rep.List<TaskInfo>();
                    foreach (var i in list3)
                    {
                        if (!string.IsNullOrEmpty(i.SearchManagerClassName))
                        {
                            if (i.SearchManagerClassName.Contains("Feng.Data.SearchManager,"))
                                i.SearchManagerClassName = "UNTYPED";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerProcedure"))
                                i.SearchManagerClassName = "UNTYPEDPROCEDURE";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerFunction"))
                                i.SearchManagerClassName = "UNTYPEDFUNCTION";
                            else if (i.SearchManagerClassName.Contains("Feng.Data.SearchManagerGroupByDetail"))
                                i.SearchManagerClassName = "UNTYPEDGROUPBYDETAIL";
                            else
                                i.SearchManagerClassName = i.SearchManagerClassName.Replace(", Feng.Controller", ", Feng.Windows.Controller");
                            rep.Update(i);
                        }
                    }
                });

            // Entity expression
            DaoHelper.DoInRepository((rep) =>
            {
                var list1 = rep.List<GridRowInfo>();
                foreach (var i in list1)
                {
                    i.Visible = ReplaceExpression(i.Visible);
                    i.ReadOnly = ReplaceExpression(i.ReadOnly);
                    i.AllowDelete = ReplaceExpression(i.AllowDelete);
                    i.AllowEdit = ReplaceExpression(i.AllowEdit);
                    i.DetailGridReadOnly = ReplaceExpression(i.DetailGridReadOnly);
                    i.DetailGridAllowInsert = ReplaceExpression(i.DetailGridAllowInsert);      
                    rep.Update(i);
                }
                var list2 = rep.List<GridCellInfo>();
                foreach (var i in list2)
                {
                    i.NotNull = ReplaceExpression(i.NotNull);
                    i.ReadOnly = ReplaceExpression(i.ReadOnly);
                    rep.Update(i);
                }
                var list3 = rep.List<GridInfo>();
                foreach (var i in list3)
                {
                    i.VisibleAsDetail = ReplaceExpression(i.VisibleAsDetail);
                    rep.Update(i);
                }
                var list4 = rep.List<GridRelatedInfo>();
                foreach (var i in list4)
                {
                    i.Visible = ReplaceExpression(i.Visible);
                    rep.Update(i);
                }
                var list5 = rep.List<GridRelatedAddressInfo>();
                foreach (var i in list5)
                {
                    i.Visible = ReplaceExpression(i.Visible);
                    rep.Update(i);
                }
                var list6 = rep.List<WindowMenuInfo>();
                foreach (var i in list6)
                {
                    i.Visible = ReplaceExpression(i.Visible);
                    i.Enable = ReplaceExpression(i.Enable);
                    rep.Update(i);
                }
                var list7 = rep.List<ParamCreatorInfo>();
                foreach (var i in list7)
                {
                    i.ParamValue = ReplaceExpression(i.ParamValue);
                    rep.Update(i);
                }
            });
        }
        private static string ReplaceExpression(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            return s.Replace(" = ", " == ").Replace("%=\"", "% == \"").Replace("<>", "!=")
                            .Replace("\"False\"", "False").Replace("\"True\"", "True").Replace("\"\"", "None")
                            .Replace("AND", "and").Replace("OR", "or").Replace("NOT", "not")
                            .Replace("%转关标志%", "str(%转关标志%)").Replace("str(str(%转关标志%))", "str(%转关标志%)")
                            .Replace("%凭证类别%", "str(%凭证类别%)").Replace("str(str(%凭证类别%))", "str(%凭证类别%)")
                            .Replace("%自动手工标志%", "str(%自动手工标志%)").Replace("str(str(%自动手工标志%))", "str(%自动手工标志%)")
                            .Replace("%收付标志%", "str(%收付标志%)").Replace("str(str(%收付标志%))", "str(%收付标志%)")
                            .Replace("%开票类别%", "str(%开票类别%)").Replace("str(str(%开票类别%))", "str(%开票类别%)")
                            .Replace("%领用方式%", "str(%领用方式%)").Replace("str(str(%领用方式%))", "str(%领用方式%)")
                            .Replace("%开票类别%", "str(%开票类别%)").Replace("str(str(%开票类别%))", "str(%开票类别%)")
                            .Replace("str", "unicode");
        }
    }
}
