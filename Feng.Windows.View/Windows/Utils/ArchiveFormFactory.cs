using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Feng.Grid;
using Feng.Windows.Forms;

namespace Feng.Windows.Utils
{
    public class ArchiveFormFactory : IWindowFactory
    {
        public event EventHandler WindowCreated;

        /// <summary>
        /// CreateWindow
        /// </summary>
        /// <param name="windowInfo"></param>
        /// <returns></returns>
        public object CreateWindow(WindowInfo windowInfo)
        {
            MyForm returnForm = null;
            switch (windowInfo.WindowType)
            {
                case WindowType.Maintain:
                case WindowType.Transaction:
                    {
                        returnForm = new GeneratedArchiveOperationForm(windowInfo);
                    }
                    break;
                case WindowType.Query:
                    {
                        // decide gridType
                        IList<WindowTabInfo> tabInfos = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowInfo.Name);
                        IList<GridColumnInfo> gridColumns = ADInfoBll.Instance.GetGridColumnInfos(tabInfos[0].GridName);
                        foreach (GridColumnInfo info in gridColumns)
                        {
                            if (!string.IsNullOrEmpty(info.ParentPropertyName))
                            {
                                returnForm = new GeneratedArchiveSeeForm(windowInfo, DataGridType.DataUnboundGridLoadOnce);
                            }
                        }
                        returnForm = new GeneratedArchiveSeeForm(windowInfo);
                    }
                    break;
                case WindowType.QueryBound:
                    {
                        returnForm = new GeneratedArchiveSeeForm(windowInfo, DataGridType.DataBoundGridLoadOnDemand);
                    }
                    break;
                case WindowType.TransactionBound:
                    {
                        returnForm = new GeneratedArchiveOperationForm(windowInfo, ArchiveGridType.ArchiveBoundGrid);
                    }
                    break;
                case WindowType.Select:
                    {
                        returnForm = new GeneratedArchiveCheckForm(windowInfo);
                    }
                    break;
                case WindowType.DatabaseReport:
                    {
                        returnForm = new GeneratedArchiveDatabaseReportForm(windowInfo);
                    }
                    break;
                case WindowType.DataSetReport:
                    {
                        returnForm = new GeneratedArchiveDataSetReportForm(windowInfo);
                    }
                    break;
                case WindowType.SelectWindow:
                    {
                        using (ArchiveSelectForm selectForm = new ArchiveSelectForm(windowInfo.Name))
                        {
                            if (selectForm.ShowDialog() == DialogResult.OK)
                            {
                                returnForm = selectForm.SelectedForm;
                            }
                            else
                            {
                                returnForm = null;
                            }
                        }
                    }
                    break;
                case WindowType.DetailTransaction:
                    {
                        ArchiveDetailForm form = ArchiveFormFactory.GenerateArchiveDetailForm(ADInfoBll.Instance.GetWindowInfo(windowInfo.Name), null);
                        form.Load += new EventHandler(delegate(object sender, System.EventArgs e)
                        {
                            form.UpdateContent();
                            form.SetMenuState();
                        });
                        form.SetAsMdiChild();
                        //// 创建TaskPane
                        //GridRelatedControl gridRelatedControl = null;
                        //form.SetGridRelatedPanel(() =>
                        //    {
                        //        if (gridRelatedControl == null)
                        //        {
                        //            gridRelatedControl = new GridRelatedControl(form.GridName, form.DisplayManager, form);
                        //        }
                        //        return gridRelatedControl;
                        //    });

                        form.GenerateWindowMenu(ADInfoBll.Instance.GetWindowMenuInfo(windowInfo.Name));
                        form.Disposed += new EventHandler(delegate(object sender, System.EventArgs e)
                        {
                            form.DisposeWindowMenu();
                        });
                        returnForm = form;
                    }
                    break;
                case WindowType.DataControl:
                    {
                        returnForm = new GeneratedArchiveDataControlForm(windowInfo);
                    }
                    break;
                case WindowType.ExcelOperation:
                    {
                        returnForm = new GeneratedArchiveExcelForm(windowInfo);
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid WindowType in WindowInfo");
            }

            if (WindowCreated != null)
            {
                WindowCreated(returnForm, System.EventArgs.Empty);
            }

            return returnForm;
        }

        /// <summary>
        /// CreateForm
        /// </summary>
        /// <param name="formInfo"></param>
        /// <returns></returns>
        public static Form CreateForm(FormInfo formInfo)
        {
            if (formInfo == null)
            {
                throw new ArgumentNullException("formInfo");
            }
            if (string.IsNullOrEmpty(formInfo.ClassName))
            {
                throw new ArgumentException("FormInfo of " + formInfo.Name + " 's ClassName must not be null!");
            }

            Form form = null;
            if (formInfo.ClassName.EndsWith(".py"))
            {
                form = ServiceProvider.GetService<IFileScript>().ExecuteFile(formInfo.ClassName, 
                    new Dictionary<string, object> { { "Params", formInfo.Params } }) as Form;
            }
            else
            {
                if (!string.IsNullOrEmpty(formInfo.Params))
                {
                    form =
                        Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(formInfo.ClassName),
                                                                formInfo.Params.Split(new char[] { ';' })) as Form;
                }
                else
                {
                    form =
                        Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(formInfo.ClassName)) as Form;
                }
            }
            return form;
        }
        
        //public static ArchiveDetailForm GenerateArchiveDetailForm(WindowInfo windowInfo)
        //{
        //    return GenerateArchiveDetailForm(windowInfo, null);
        //}

        public static ArchiveDetailForm GenerateArchiveDetailForm(WindowInfo windowInfo)
        {
            ArchiveDetailForm ret = null;
            IList<WindowTabInfo> tabInfos = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowInfo.Name);

            if (tabInfos.Count == 0)
            {
                throw new ArgumentException("there should be winTab infos in window of " + windowInfo.Name);
            }
            ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfos[0], null);

            IWindowControlManager cmParent = null;
            if (string.IsNullOrEmpty(tabInfos[0].ControlManagerClassName))
            {
                IDisplayManager dmParent = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(tabInfos[0], sm);
                ret = GenerateArchiveDetailForm(windowInfo, dmParent);
            }
            else
            {
                cmParent = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(tabInfos[0], sm) as IWindowControlManager;

                IBaseDao daoParent = ServiceProvider.GetService<IManagerFactory>().GenerateBusinessLayer(tabInfos[0]);
                cmParent.Dao = daoParent;

                ret = GenerateArchiveDetailForm(windowInfo, cmParent, daoParent as IRelationalDao);
            }

            ret.Text = windowInfo.Text;
            ret.Name = windowInfo.Name;

            ArchiveSearchForm searchForm = null;
            ret.SetSearchPanel(() =>
                {
                    if (searchForm == null)
                    {
                        searchForm = new ArchiveSearchForm(ret, sm, tabInfos[0]);
                        if (cmParent != null)
                        {
                            cmParent.StateControls.Add(searchForm);
                        }
                    }
                    return searchForm;
                });

            return ret;
        }

        public static ArchiveDetailForm GenerateArchiveDetailForm(WindowInfo windowInfo, IDisplayManager dmParent)
        {
            return GenerateArchiveDetailForm(windowInfo, null, null, dmParent);
        }

        public static ArchiveDetailForm GenerateArchiveDetailForm(WindowInfo windowInfo, IWindowControlManager cmParent, IRelationalDao daoParent)
        {
            return GenerateArchiveDetailForm(windowInfo, cmParent, daoParent, null);
        }

        //private static void AddManualDetailForm(System.Windows.Forms.Form form, IArchiveDetailFormAuto detailFormAuto)
        //{
        //    form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        //    form.TopLevel = false;

        //    detailFormAuto.ReplaceFlowLayoutPanel(form);
        //}

        public static ArchiveDetailForm GenerateArchiveDetailForm(WindowInfo windowInfo, IWindowControlManager cmParent, IBaseDao daoParent, IDisplayManager dmParent, IArchiveDetailForm originalDetailForm = null)
        {
            ArchiveDetailForm detailForm = originalDetailForm as ArchiveDetailForm;
            IList<WindowTabInfo> tabInfos = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowInfo.Name);

            IList<WindowTabInfo> detailFormTabInfos = new List<WindowTabInfo>();
            IList<WindowTabInfo> detailFormTabInfos2 = new List<WindowTabInfo>();
            foreach (WindowTabInfo subTabInfo in tabInfos[0].ChildTabs)
            {
                if (subTabInfo.IsInDetailForm)
                {
                    detailFormTabInfos.Add(subTabInfo);
                }
                else
                {
                    detailFormTabInfos2.Add(subTabInfo);
                }
            }

            //if (detailStyleForm == null && windowInfo.DetailForm != null)
            //{
            //    FormInfo formInfo = ADInfoBll.Instance.GetFormInfo(windowInfo.DetailForm.Name);
            //    if (formInfo == null)
            //    {
            //        throw new ArgumentException("There is no FormInfo with Name of " + windowInfo.DetailForm.Name);
            //    }
            //    detailStyleForm = CreateForm(formInfo);
            //}
            //if (detailStyleForm != null)
            //{
            //    ret = detailStyleForm as ArchiveDetailForm;
            //}

            // 当第二层的任何一个ControlManager为空时，DetailForm作为不可编辑的。
            bool isControlManagerEnable = cmParent != null;
            if (isControlManagerEnable)
            {
                for (int i = 0; i < detailFormTabInfos.Count; ++i)
                {
                    if (string.IsNullOrEmpty(detailFormTabInfos[i].ControlManagerClassName))
                    {
                        dmParent = cmParent.DisplayManager;
                        isControlManagerEnable = false;
                        break;
                    }
                }
            }

            if (detailForm == null)
            {
                if (detailFormTabInfos.Count == 0)
                {
                    if (isControlManagerEnable)
                    {
                        detailForm = new ArchiveDetailFormAuto(cmParent, tabInfos[0].GridName);
                    }
                    else
                    {
                        detailForm = new ArchiveDetailFormAuto(dmParent, tabInfos[0].GridName);
                    }
                }
                else if (detailFormTabInfos.Count == 1)
                {
                    if (isControlManagerEnable)
                    {
                        detailForm = new ArchiveDetailFormAutoWithDetailGrid(cmParent, tabInfos[0].GridName);
                    }
                    else
                    {
                        detailForm = new ArchiveDetailFormAutoWithDetailGrid(dmParent, tabInfos[0].GridName);
                    }
                }
                else
                {
                    string[] texts = new string[detailFormTabInfos.Count];
                    for (int i = 0; i < detailFormTabInfos.Count; ++i)
                    {
                        texts[i] = detailFormTabInfos[i].Text;
                    }

                    if (isControlManagerEnable)
                    {
                        detailForm = new ArchiveDetailFormAutoWithMultiDetailGrid(cmParent, tabInfos[0].GridName, detailFormTabInfos.Count, texts);
                    }
                    else
                    {
                        detailForm = new ArchiveDetailFormAutoWithMultiDetailGrid(dmParent, tabInfos[0].GridName, detailFormTabInfos.Count, texts);
                    }
                }

                //IArchiveDetailFormAuto detailFormAuto = ret as IArchiveDetailFormAuto;
                //if (detailStyleForm != null && detailFormAuto != null)
                //{
                //    AddManualDetailForm(detailStyleForm, detailFormAuto);
                //}
            }
            else
            {
                // Dao 在cmParent处已经设置了
                if (isControlManagerEnable)
                {
                    detailForm.SetControlMananger(cmParent, tabInfos[0].GridName);
                }
                else
                {
                    detailForm.SetDisplayManager(dmParent, tabInfos[0].GridName);
                }
            }

            detailForm.Name = windowInfo.Name;
            detailForm.Text = windowInfo.Text;

            // 只有2层可编辑。主层（控件）-第二层（grid）。再下去就是第二层grid的DetailGrid，不可编辑。
            IArchiveDetailFormWithDetailGrids detailFormWithGrids = detailForm as IArchiveDetailFormWithDetailGrids;
            if (detailFormWithGrids != null)
            {
                for (int i = 0; i < detailFormTabInfos.Count; ++i)
                {
                    if (i >= detailFormWithGrids.DetailGrids.Count)
                        break;
                    // 主是ControlManager，并不一定子也是ControlManager。
                    // 主可以在grid编辑，不能通过DetailForm编辑。此时DetailForm是另外显示的东西。
                    if (isControlManagerEnable)
                    {
                        var daoRelational = daoParent as IRelationalDao;
                        if (daoRelational == null)
                        {
                            throw new ArgumentException("IArchiveDetailFormWithDetailGrids must has IRelationalDao.");
                        }

                        ISearchManager subSm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(detailFormTabInfos[i], cmParent.DisplayManager);
                        IWindowControlManager subCm = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(detailFormTabInfos[i], subSm) as IWindowControlManager;
                        subCm.Name = detailFormTabInfos[i].Name;
                        ((IArchiveGrid)detailFormWithGrids.DetailGrids[i]).SetControlManager(subCm, detailFormTabInfos[i].GridName);
                       
                        ManagerFactory.GenerateBusinessLayer(daoParent as IRelationalDao, detailFormTabInfos[i]);

                        IBaseDao subDao = daoRelational.GetRelationalDao(i);
                        if (subDao is IMemoriedRelationalDao)
                        {
                            IMemoryDao subMemoryDao = ((IMemoriedRelationalDao)daoRelational.GetRelationalDao(i)).DetailMemoryDao;
                            subCm.Dao = subMemoryDao;

                            //subMemoryDao.AddSubDao(new MasterDetailMemoryDao<>(cmParent));
                            ((IMemoriedRelationalDao)daoRelational.GetRelationalDao(i)).AddRelationToMemoryDao(cmParent.DisplayManager);
                        }
                        else
                        {
                            subCm.Dao = subDao;
                        }
                    }
                    else
                    {
                        ISearchManager subSm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(detailFormTabInfos[i], dmParent);
                        IDisplayManager subDm = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(detailFormTabInfos[i], subSm);
                        subDm.Name = detailFormTabInfos[i].Name;

                        detailFormWithGrids.DetailGrids[i].SetDisplayManager(subDm, detailFormTabInfos[i].GridName);
                    }
                    GenerateDetailGrids(detailFormWithGrids.DetailGrids[i], detailFormTabInfos[i]);
                }
            }

            if (isControlManagerEnable)
            {
                // Generate Other Daos
                for (int i = 0; i < detailFormTabInfos2.Count; ++i)
                {
                    if (!string.IsNullOrEmpty(detailFormTabInfos2[i].BusinessLayerClassName))
                    {
                        ManagerFactory.GenerateBusinessLayer(daoParent as IRelationalDao, detailFormTabInfos2[i]);
                    }
                }
            }

            // if Master Tab's IsInDetailForm=false, Invisible it
            if (!tabInfos[0].IsInDetailForm)
            {
                if (detailForm is IArchiveDetailFormAuto)
                {
                    (detailForm as IArchiveDetailFormAuto).RemoveControls();
                }
            }

            return detailForm;
        }

        // 必定是DisplayManager接管，不可编辑
        internal static void GenerateDetailGrids(IBoundGrid masterGrid, WindowTabInfo masterTabInfo)
        {
            if (masterTabInfo.ChildTabs.Count == 0)
                return;

            if (!(masterGrid is IBoundGridWithDetailGridLoadOnDemand))
            {
                throw new ArgumentException("Grid With DetailGrid must be IBoundGridWithDetailGridLoadOnDemand");
            }

            IDisplayManager masterDm = masterGrid.DisplayManager;
            foreach (WindowTabInfo subTabInfo in masterTabInfo.ChildTabs)
            {
                if (subTabInfo.IsInGrid)
                {
                    ISearchManager subSm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(subTabInfo, masterDm);

                    IDisplayManager subDm;
                    if (!string.IsNullOrEmpty(subTabInfo.DisplayManagerClassName))
                    {
                        subDm = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(subTabInfo, subSm);
                    }
                    else
                    {
                        subDm = (ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(subTabInfo, subSm)).DisplayManager;
                    }
                    subDm.Name = subTabInfo.Name;

                    // IBoundGridWithDetailGridLoadOnDemand
                    DataUnboundDetailGrid detailGrid = new DataUnboundDetailGrid();
                    detailGrid.SetDisplayManager(subDm, subTabInfo.GridName);
                    ((IBoundGridWithDetailGridLoadOnDemand)masterGrid).AddDetailGrid(detailGrid);

                    GenerateDetailGrids(detailGrid, subTabInfo);
                }
            }
        }

        /// <summary>
        /// 按照WindowTabInfo生成Grid
        /// </summary>
        /// <param name="masterTabInfo"></param>
        /// <returns></returns>
        public static DataUnboundGrid GenerateDataUnboundGrid(WindowTabInfo masterTabInfo)
        {
            DataUnboundGrid ret;
            if (masterTabInfo.ChildTabs.Count == 0)
            {
                ret = new DataUnboundGrid();
            }
            else
            {
                ret = new DataUnboundWithDetailGridLoadOnDemand();
            }
            SetupDataUnboundGrid(ret, masterTabInfo);

            return ret;
        }

        /// <summary>
        /// 按照WindowTabInfo配置Grid
        /// </summary>
        /// <param name="masterGrid"></param>
        /// <param name="masterTabInfo"></param>
        public static void SetupDataUnboundGrid(DataUnboundGrid masterGrid, WindowTabInfo masterTabInfo)
        {
            ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(masterTabInfo, null);
            masterGrid.SetDisplayManager(ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(masterTabInfo, sm), 
                masterTabInfo.GridName);

            if (masterTabInfo.ChildTabs.Count > 0)
            {
                GenerateDetailGrids(masterGrid, masterTabInfo);
            }
        }

        
    }
}
