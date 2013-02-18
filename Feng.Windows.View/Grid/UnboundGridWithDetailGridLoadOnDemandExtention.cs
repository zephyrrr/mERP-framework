using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Forms;

namespace Feng.Grid
{
    /// <summary>
    /// UnboundGridWithDetailGridLoadOnDemandExtention
    /// </summary>
    public static class UnboundGridWithDetailGridLoadOnDemandExtention
    {
        /// <summary>
        /// AddDetailGrid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="detailGrid"></param>
        public static void AddDetailGrid(this IBoundGridWithDetailGridLoadOnDemand grid, DataBoundDetailGrid detailGrid)
        {
            GridAddDataRowExtention.AddDetailGrid(grid, detailGrid);

            detailGrid.CollapsedChanged -= new EventHandler(detailGrid_CollapsedChanged);
            detailGrid.CollapsedChanged += new EventHandler(detailGrid_CollapsedChanged);
        }

        private static void detailGrid_CollapsedChanged(object sender, EventArgs e)
        {
            DataBoundDetailGrid detailGrid = sender as DataBoundDetailGrid;
            if (detailGrid == null)
                return;

            if (!detailGrid.Collapsed)
            {
                // Buffer
                if (detailGrid.DataRows.Count > 0)
                    return;

                //detailGrid.GridControl.CurrentRow = detailGrid.ParentDataRow;

                // detailGrid.ParentGrid is Xceed.Grid.MasterGrid
                // detailGrid.GridControl is IboundGrid 当时不适用于多层情况
                //IBoundGrid parentGrid = detailGrid.ParentGrid as IBoundGrid;  
                //if (parentGrid == null)
                //{
                //    throw new ArgumentException("Parent Grid should be IBoundGrid!");
                //}

                ISearchManagerWithParent smp = detailGrid.DisplayManager.SearchManager as ISearchManagerWithParent;
                if (smp == null)
                {
                    throw new ArgumentException("detailGrid.DisplayManager.SearchManager should be ISearchManagerWithParent!");
                }
                IDisplayManager dmParent = smp.ParentDisplayManager;
                try
                {
                    dmParent.BeginBatchOperation();

                    int oldPos = dmParent.Position;
                    if (oldPos != detailGrid.ParentDataRow.Index)
                    {
                        dmParent.Position = detailGrid.ParentDataRow.Index;
                    }
                    if (detailGrid is ArchiveUnboundDetailGrid)
                    {
                        ((ArchiveUnboundDetailGrid)detailGrid).ArchiveGridHelper.Initialize();
                    }

                    GridInfo gridInfo = ADInfoBll.Instance.GetGridInfo(detailGrid.GridName);

                    bool gridVisible = true;
                    if (!string.IsNullOrEmpty(gridInfo.VisibleAsDetail))
                    {
                        gridVisible = Permission.AuthorizeByRule(gridInfo.VisibleAsDetail, dmParent.CurrentItem);
                    }
                    if (!gridVisible)
                    {
                        detailGrid.Visible = false;
                    }
                    else
                    {
                        detailGrid.Visible = true;

                        // 可能有多个DetailGrid，只处理第一个
                        if (m_detailGridsState.Count == 0)
                        {
                            m_detailGridsState = new ArrayList { detailGrid, detailGrid.GridControl.Enabled, oldPos };

                            detailGrid.GridControl.Enabled = false;
                            detailGrid.DisplayManager.SearchManager.DataLoaded -= new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);
                            detailGrid.DisplayManager.SearchManager.DataLoaded += new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);
                        }

                        detailGrid.DisplayManager.SearchManager.LoadDataAccordSearchControls();
                    }

                    // 不能直接设置，需要在查找完成后设置
                    //dmParent.Position = oldPos;
                }
                finally
                {
                    dmParent.EndBatchOperation();
                    //detailGrid.Visible = (detailGrid.DataRows.Count != 0);
                }
            }
        }

        private static ArrayList m_detailGridsState = new ArrayList();
        static void SearchManager_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            ISearchManager sm = sender as ISearchManager;
            if (m_detailGridsState.Count > 0)
            {
                Xceed.Grid.DetailGrid detailGrid = m_detailGridsState[0] as Xceed.Grid.DetailGrid;

                int level = 0;
                Xceed.Grid.DetailGrid nowGrid = detailGrid;
                while (nowGrid != null)
                {
                    nowGrid = nowGrid.ParentGrid;
                    level++;
                }
                (detailGrid.GridControl as MyGrid).LoadLayout(level - 1);

                detailGrid.GridControl.Enabled = (bool)m_detailGridsState[1];

                ISearchManagerWithParent smp = sm as ISearchManagerWithParent;
                IDisplayManager dmParent = smp.ParentDisplayManager;
                dmParent.Position = (int)m_detailGridsState[2];

                m_detailGridsState.Clear();
            }
        }
    }
}
