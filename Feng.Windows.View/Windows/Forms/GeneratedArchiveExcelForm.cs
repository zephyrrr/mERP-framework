using System;
using System.Collections.Generic;
using System.Text;
using Feng.Grid;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public class GeneratedArchiveExcelForm : ArchiveExcelForm
    {
        public GeneratedArchiveExcelForm(WindowInfo windowInfo)
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

            ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfos[0], null);
            IBaseDao daoParent = ServiceProvider.GetService<IManagerFactory>().GenerateBusinessLayer(tabInfos[0]);
            IWindowControlManager cmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(tabInfos[0], sm) as IWindowControlManager;
            cmMaster.Dao = daoParent;
            base.ControlManager = cmMaster;

            this.MasterGrid.GridName = tabInfos[0].GridName;
            this.ExcelGrid.BeginInitialize();
            CreateGridColumns(this.MasterGrid);
            this.ExcelGrid.EndInitialize();

            this.ExcelGrid.SetColumnManagerRowHorizontalAlignment();

            // 不需要。和ArchiveOperationForm同一个WindowInfo，会出错
            //GeneratedArchiveSeeForm.InitializeWindowProcess(windowInfo, this);
        }

        private void CreateGridColumns(IGrid grid)
        {
            try
            {
                grid.BeginInit();

                grid.Columns.Clear();

                foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(grid.GridName))
                {
                    // 有些列是要设置值但不可见的，例如Id
                    //if (!Authority.AuthorizeByRule(info.ColumnVisible))
                    //    continue;

                    switch (info.GridColumnType)
                    {
                        case GridColumnType.Normal:
                            {
                                Xceed.Grid.Column column;
                                if (grid.Columns[info.GridColumnName] != null)
                                {
                                    throw new ArgumentException("there have already exist column " + info.GridColumnName);
                                }
                                else
                                {
                                    column = new Xceed.Grid.Column(info.GridColumnName, GridColumnInfoHelper.CreateType(info));
                                }

                                UnBoundGridExtention.SetColumnProperties(column, info, grid);

                                GridFactory.CreateCellViewerManager(column, info, this.ControlManager.DisplayManager);
                               
                                bool readOnly = Authority.AuthorizeByRule(info.ReadOnly);
                                if (!readOnly)
                                {
                                    GridFactory.CreateCellEditorManager(column, info, this.ControlManager.DisplayManager);
                                }

                                grid.Columns.Add(column);
                            }
                            break;
                        default:
                            break;
                        //default:
                        //    throw new InvalidOperationException("Invalide gridcolumnType of " + info.GridColumnType + " in " + info.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
            finally
            {
                grid.EndInit();
            }
        }
    }
}
