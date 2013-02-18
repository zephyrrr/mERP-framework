using System;
using System.Collections.Generic;
using System.Text;
using Xceed.SmartUI;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class GridGotoFormTaskPane : Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane //Xceed.SmartUI.Controls.ExplorerTaskPane.SmartExplorerTaskPane
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (SmartItem item in this.Items)
                {
                    foreach (SmartItem subItem in item.Items)
                    {
                        subItem.Click -= new Xceed.SmartUI.SmartItemClickEventHandler(GridGotoFormTaskPane_Click);
                    }
                }
                this.Items.Clear();

                IArchiveMasterForm archiveSeeForm = m_parentForm as IArchiveMasterForm;
                if (archiveSeeForm != null)
                {
                    archiveSeeForm.MasterGrid.GridControl.CurrentRowChanged -= new EventHandler(MasterGrid_CurrentRowChanged);
                }
                m_parentForm = null;

                if (m_dm != null)
                {
                    m_dm.Dispose();
                    m_dm = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="dm"></param>
        /// <param name="parentForm"></param>
        public GridGotoFormTaskPane(string gridName, IDisplayManager dm, IArchiveMasterForm parentForm)
        {
            XceedUtility.SetUIStyle(this);

            LoadMenus(gridName);
            m_parentForm = parentForm;
            m_dm = dm;

            IArchiveMasterForm archiveSeeForm = parentForm;
            if (archiveSeeForm != null)
            {
                archiveSeeForm.MasterGrid.GridControl.CurrentRowChanged += new EventHandler(MasterGrid_CurrentRowChanged);

                MasterGrid_CurrentRowChanged(archiveSeeForm.DisplayManager, System.EventArgs.Empty);
            }
        }
        void MasterGrid_CurrentRowChanged(object sender, EventArgs e)
        {
            var currentRow = m_parentForm.MasterGrid.CurrentRow;
            string currentLevel = currentRow == null ? null : Feng.Grid.MyGrid.GetGridLevel(currentRow.ParentGrid);

            object entity = currentRow == null ? null : m_parentForm.MasterGrid.CurrentRow.Tag;

            foreach (SmartItem item in m_items1)
            {
                GridRelatedInfo info = item.Tag as GridRelatedInfo;
                if (info.RelatedType == GridRelatedType.ByRows)
                {
                    if (entity == null ||  (!string.IsNullOrEmpty(info.GridLevel) && info.GridLevel != currentLevel)) // Feng.Utils.ReflectionHelper.GetTypeFromName(info.EntityType) != entity.GetType())
                    {
                        item.Visible = false;
                    }
                    else
                    {
                        item.Visible = Permission.AuthorizeByRule(info.Visible, entity);
                    }
                }
            }
            //foreach (SmartItem item in m_items2)
            //{
            //    GridRelatedInfo info = item.Tag as GridRelatedInfo;
            //    if (entity == null || Feng.Utils.ReflectionHelper.GetTypeFromName(info.EntityType) != entity.GetType())
            //    {
            //        item.Visible = false;
            //    }
            //    else
            //    {
            //        item.Visible = Permission.AuthorizeByRule(info.Visible, entity);
            //    }
            //}
        }

        private IArchiveMasterForm m_parentForm;
        private IDisplayManager m_dm;
        private List<SmartItem> m_items1 = new List<SmartItem>();
        //private List<SmartItem> m_items2 = new List<SmartItem>();
        //private List<SmartItem> m_items3 = new List<SmartItem>();
        //private List<SmartItem> m_items4 = new List<SmartItem>();

        private void LoadGridRelated(string gridName)
        {
            IList<GridRelatedInfo> list = ADInfoBll.Instance.GetGridRelatedInfo(gridName);
            if (list == null || list.Count == 0)
            {
                return;
            }

            int groupIdx1 = -1;
            //int groupIdx2 = this.Items.Add("详细信息");
            int groupIdx3 = -1;
            int groupIdx4 = -1;

            foreach (GridRelatedInfo info in list)
            {
                if (info.Name.ToUpper() == "SEPARATOR")
                {
                }
                else
                {
                    switch (info.RelatedType)
                    {
                        case GridRelatedType.ByRows:
                            {
                                ActionInfo actionInfo = ADInfoBll.Instance.GetActionInfo(info.Action.Name);
                                if (actionInfo == null)
                                    continue;
                                if (Authority.AuthorizeByRule(actionInfo.Access))
                                {
                                    if (groupIdx1 == -1)
                                    {
                                        groupIdx1 = this.Items.Add("按选定行");
                                    }

                                    int childIdx1 = this.Items[groupIdx1].Items.Add(info.Name);
                                    this.Items[groupIdx1].Items[childIdx1].Text = info.Text;
                                    this.Items[groupIdx1].Items[childIdx1].ToolTipText = info.Help;
                                    this.Items[groupIdx1].Items[childIdx1].Click += new Xceed.SmartUI.SmartItemClickEventHandler(GridGotoFormTaskPane_Click);
                                    this.Items[groupIdx1].Items[childIdx1].Tag = info;
                                    m_items1.Add(this.Items[groupIdx1].Items[childIdx1]);
                                    //if (!string.IsNullOrEmpty(info.ToDetailForm))
                                    //{
                                    //    int childIdx2 = this.Items[groupIdx2].Items.Add(info.Text);
                                    //    this.Items[groupIdx2].Items[childIdx2].Click += new Xceed.SmartUI.SmartItemClickEventHandler(GridGotoFormTaskPane_Click);
                                    //    this.Items[groupIdx2].Items[childIdx2].Tag = info;
                                    //    m_items2.Add(this.Items[groupIdx2].Items[childIdx2]);
                                    //}
                                }
                            }
                            break;
                        case GridRelatedType.BySearchExpression:
                            {
                                // 不跟选定行相关，所以Visible是通用的，无参数
                                if (!Authority.AuthorizeByRule(info.Visible))
                                    continue;

                                if (groupIdx3 == -1)
                                {
                                    groupIdx3 = this.Items.Add("按搜索条件");
                                }
                                ActionInfo actionInfo = ADInfoBll.Instance.GetActionInfo(info.Action.Name);
                                if (actionInfo == null)
                                    continue;
                                if (Authority.AuthorizeByRule(actionInfo.Access))
                                {
                                    int childIdx1 = this.Items[groupIdx3].Items.Add(info.Name);
                                    this.Items[groupIdx3].Items[childIdx1].Text = info.Text;
                                    this.Items[groupIdx3].Items[childIdx1].ToolTipText = info.Help;
                                    this.Items[groupIdx3].Items[childIdx1].Click += new Xceed.SmartUI.SmartItemClickEventHandler(GridGotoFormTaskPane_Click);
                                    this.Items[groupIdx3].Items[childIdx1].Tag = info;
                                    //m_items3.Add(this.Items[groupIdx3].Items[childIdx1]);
                                }
                            }
                            break;
                        case GridRelatedType.ByDataControl:
                        case GridRelatedType.ByNone:
                            {
                                if (!Authority.AuthorizeByRule(info.Visible))
                                    continue;

                                if (groupIdx4 == -1)
                                {
                                    groupIdx4 = this.Items.Add("按数据控件");
                                }
                                ActionInfo actionInfo = ADInfoBll.Instance.GetActionInfo(info.Action.Name);
                                if (Authority.AuthorizeByRule(actionInfo.Access))
                                {
                                    int childIdx1 = this.Items[groupIdx4].Items.Add(info.Name);
                                    this.Items[groupIdx4].Items[childIdx1].Text = info.Text;
                                    this.Items[groupIdx4].Items[childIdx1].ToolTipText = info.Help;
                                    this.Items[groupIdx4].Items[childIdx1].Click += new Xceed.SmartUI.SmartItemClickEventHandler(GridGotoFormTaskPane_Click);
                                    this.Items[groupIdx4].Items[childIdx1].Tag = info;
                                    //m_items4.Add(this.Items[groupIdx3].Items[childIdx1]);
                                }
                            }
                            break;
                        default:
                            throw new NotSupportedException("Invalid GridRelatedInfo info of " + info.Name);
                    }

                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void LoadMenus(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            LoadGridRelated(name);
        }

        void GridGotoFormTaskPane_Click(object sender, Xceed.SmartUI.SmartItemClickEventArgs e)
        {
            Xceed.SmartUI.SmartItem item = sender as Xceed.SmartUI.SmartItem;
            GridRelatedInfo info = item.Tag as GridRelatedInfo;

            switch (info.RelatedType)
            {
                case GridRelatedType.ByRows:
                    {
                        if (item.ParentItem.Text == "按选定行")
                        {
                            ShowFormFromGrid(m_parentForm, info, false);
                        }
                        else
                        {
                            ShowFormFromGrid(m_parentForm, info, true);
                        }
                    }
                    break;
                case GridRelatedType.BySearchExpression:
                    {
                        SearchHistoryInfo his = m_dm.SearchManager.GetHistory(0);

                        if (string.IsNullOrEmpty(his.Expression))
                        {
                            MessageForm.ShowInfo("还未有搜索条件！");
                            return;
                        }
                        string newSearchExpression = info.SearchExpression;
                        ISearchExpression oldSe = SearchExpression.Parse(his.Expression);
                        Dictionary<string, object> dict = GetSearchExpreesionValues(oldSe);

                        ISearchExpression newSe = SearchExpression.Parse(newSearchExpression);

                        newSe = ReplaceSearchExpreesionValues(newSe, dict);

                        ShowFrom(info.Action.Name, newSe, false);
                    }
                    break;
                case GridRelatedType.ByDataControl:
                    {
                        Dictionary<string, object> dict = m_dm.Copy();
                        string exp = EntityHelper.ReplaceEntity(info.SearchExpression, new EntityHelper.GetReplaceValue(delegate(string paramName)
                        {
                            return dict[paramName];
                        }));
                        ShowFrom(info.Action.Name, SearchExpression.Parse(exp), false);
                    }
                    break;
                case GridRelatedType.ByNone:
                    {
                        ShowFrom(info.Action.Name, null, false);
                    }
                    break;
                default:
                    throw new NotSupportedException("Invalid GridRelatedInfo's RelatedType of " + info.Name);
            }
        }

        internal static ISearchExpression ReplaceSearchExpreesionValues(ISearchExpression se, Dictionary<string, object> dict)
        {
            Feng.Search.LogicalExpression le = se as Feng.Search.LogicalExpression;
            if (le != null)
            {
                ISearchExpression l = le.LeftHandSide;
                l = ReplaceSearchExpreesionValues(l, dict);

                ISearchExpression r = le.RightHandSide;
                r = ReplaceSearchExpreesionValues(r, dict);

                if (l == null)
                    return r;
                else if (r == null)
                    return l;
                else 
                    return new Feng.Search.LogicalExpression(l, r, le.LogicOperator);
            }
            else
            {
                Feng.Search.SimpleExpression ce = se as Feng.Search.SimpleExpression;
                string s = ce.Values.ToString();
                if (s[0] == '%' && s[s.Length - 1] == '%')
                {
                    string s1 = s.Substring(1, s.Length - 2);
                    if (dict.ContainsKey(s1))
                    {
                        ce.Values = dict[s1];
                    }
                    else
                    {
                        ce = null;
                    }
                }
                return ce;
            }
        }

        internal static Dictionary<string, object> GetSearchExpreesionValues(ISearchExpression se)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            Feng.Search.LogicalExpression le = se as Feng.Search.LogicalExpression;
            if (le != null)
            {
                Dictionary<string, object> r = GetSearchExpreesionValues(le.LeftHandSide);
                foreach (KeyValuePair<string, object> kvp in r)
                {
                    ret[kvp.Key] = kvp.Value;
                }
                r = GetSearchExpreesionValues(le.RightHandSide);
                foreach (KeyValuePair<string, object> kvp in r)
                {
                    ret[kvp.Key] = kvp.Value;
                }
            }
            else
            {
                Feng.Search.SimpleExpression ce = se as Feng.Search.SimpleExpression;
                ret[ce.FullPropertyName + ce.Operator.ToString()] = ce.Values;
            }
            return ret;
        }

        internal static ISearchExpression GetSearchExpressionFromGrid(IArchiveMasterForm sourceForm, GridRelatedInfo info, bool onlyFirstOne)
        {
            if (sourceForm == null)
            {
                throw new ArgumentException("未能找到父窗体！", "sourceForm");
            }

            if (info.RelatedType == GridRelatedType.ByRows)
            {
                if (sourceForm.MasterGrid == null)
                {
                    throw new ArgumentException("未能找到主表格！", "sourceForm");
                }

                List<object> selectedEntities = new List<object>();
                if (sourceForm.MasterGrid.GridControl.SelectedRows.Count == 0)
                {
                    Xceed.Grid.Row row = sourceForm.MasterGrid.CurrentRow;
                    Xceed.Grid.DataRow dataRow = row as Xceed.Grid.DataRow;
                    if (dataRow != null && dataRow.Visible 
                        && (info.GridLevel == Feng.Grid.MyGrid.GetGridLevel(dataRow.ParentGrid) || string.IsNullOrEmpty(info.GridLevel)))
                    {
                        selectedEntities.Add(dataRow.Tag);
                    }
                }
                else
                {
                    foreach (Xceed.Grid.Row row in sourceForm.MasterGrid.GridControl.SelectedRows)
                    {
                        if (!row.Visible)
                        {
                            continue;
                        }

                        Xceed.Grid.DataRow dataRow = row as Xceed.Grid.DataRow;
                        if (dataRow == null)
                        {
                            continue;
                        }
                        if (info.GridLevel == Feng.Grid.MyGrid.GetGridLevel(dataRow.ParentGrid)
                            || string.IsNullOrEmpty(info.GridLevel))
                        {
                            selectedEntities.Add(dataRow.Tag);
                        }
                        else
                        {
                            foreach(Xceed.Grid.DetailGrid dg in dataRow.DetailGrids)
                            {
                                GetDetailGridRows(info, dg, selectedEntities);
                            }
                        }

                        if (onlyFirstOne && selectedEntities.Count > 0)
                        {
                            break;
                        }
                    }
                }

                if (selectedEntities.Count == 0)
                {
                    throw new InvalidOperationException("请选择表格行！");
                }

                Dictionary<string, string> exps = new Dictionary<string, string>();
                foreach (object entity in selectedEntities)
                {
                    //if (entity.GetType() != Feng.Utils.ReflectionHelper.GetTypeFromName(info.EntityType))
                    //    continue;

                    string exp = EntityHelper.ReplaceEntity(info.SearchExpression, entity);
                    exps[exp] = exp;
                }
                ISearchExpression se = null;
                foreach (string exp in exps.Keys)
                {
                    ISearchExpression subSearch = SearchExpression.Parse(exp);
                    if (se == null)
                    {
                        se = subSearch;
                    }
                    else
                    {
                        se = SearchExpression.Or(se, subSearch);
                    }
                }
                return se;

                //string[] fromColumns = info.FromColumnName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //for (int i = 0; i < fromColumns.Length; ++i)
                //{
                //    fromColumns[i] = fromColumns[i].Replace(":", ".");
                //}
                //int count = fromColumns.Length;
                //string[] toColumns = info.ToColumnName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //Debug.Assert(count == toColumns.Length, "FromColumnName必须和ToColumnName内容个数相同");

                //ArrayList[] selected = new ArrayList[count];

                //for (int i = 0; i < count; ++i)
                //{
                //    selected[i] = new ArrayList();
                //    string columnName = fromColumns[i];

                //    foreach (object entity in selectedEntities)
                //    {
                //        object o = EntityHelper.GetPropertyValue(entity, columnName);
                //        //if (o != null && !string.IsNullOrEmpty(o.ToString()))
                //        {
                //            selected[i].Add(o);
                //        }
                //    }
                //}
            }
            else
            {
                throw new NotSupportedException("Not Supported now!");
            }
        }

        internal static ISearchExpression GetSearchExpressionFromGrid(IArchiveMasterForm sourceForm, GridRelatedAddressInfo info, bool onlyFirstOne)
        {
            if (sourceForm == null)
            {
                throw new ArgumentException("未能找到父窗体！", "sourceForm");
            }

            if (info.RelatedType == GridRelatedType.ByRows)
            {
                if (sourceForm.MasterGrid == null)
                {
                    throw new ArgumentException("未能找到主表格！", "sourceForm");
                }

                List<object> selectedEntities = new List<object>();
                if (sourceForm.MasterGrid.GridControl.SelectedRows.Count == 0)
                {
                    Xceed.Grid.Row row = sourceForm.MasterGrid.CurrentRow;
                    Xceed.Grid.DataRow dataRow = row as Xceed.Grid.DataRow;
                    if (dataRow != null && dataRow.Visible 
                        && (Feng.Grid.MyGrid.GetGridLevel(dataRow.ParentGrid) == info.GridLevel || string.IsNullOrEmpty(info.GridLevel)))
                    {
                        selectedEntities.Add(dataRow.Tag);
                    }
                }
                else
                {
                    foreach (Xceed.Grid.Row row in sourceForm.MasterGrid.GridControl.SelectedRows)
                    {
                        if (!row.Visible)
                        {
                            continue;
                        }

                        Xceed.Grid.DataRow dataRow = row as Xceed.Grid.DataRow;
                        if (dataRow == null)
                        {
                            continue;
                        }

                        if (Feng.Grid.MyGrid.GetGridLevel(dataRow.ParentGrid) == info.GridLevel
                            || string.IsNullOrEmpty(info.GridLevel))
                        {
                            selectedEntities.Add(dataRow.Tag);
                        }
                        else 
                        {
                            foreach (Xceed.Grid.DetailGrid dg in dataRow.DetailGrids)
                            {
                                GetDetailGridRows(info, dataRow.DetailGrids[0], selectedEntities);
                            }
                        }

                        if (onlyFirstOne && selectedEntities.Count > 0)
                        {
                            break;
                        }
                    }
                }

                if (selectedEntities.Count == 0)
                {
                    throw new InvalidOperationException("请选择表格行！");
                }


                Dictionary<string, string> exps = new Dictionary<string, string>();
                foreach (object entity in selectedEntities)
                {
                    //if (entity.GetType() != Feng.Utils.ReflectionHelper.GetTypeFromName(info.EntityType))
                    //    continue;

                    string exp = EntityHelper.ReplaceEntity(info.SearchExpression, entity);
                    exps[exp] = exp;
                }
                ISearchExpression se = null;
                foreach (string exp in exps.Keys)
                {
                    ISearchExpression subSearch = SearchExpression.Parse(exp);
                    if (se == null)
                    {
                        se = subSearch;
                    }
                    else
                    {
                        se = SearchExpression.Or(se, subSearch);
                    }
                }
                return se;

                //string[] fromColumns = info.FromColumnName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //for (int i = 0; i < fromColumns.Length; ++i)
                //{
                //    fromColumns[i] = fromColumns[i].Replace(":", ".");
                //}
                //int count = fromColumns.Length;
                //string[] toColumns = info.ToColumnName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //Debug.Assert(count == toColumns.Length, "FromColumnName必须和ToColumnName内容个数相同");

                //ArrayList[] selected = new ArrayList[count];

                //for (int i = 0; i < count; ++i)
                //{
                //    selected[i] = new ArrayList();
                //    string columnName = fromColumns[i];

                //    foreach (object entity in selectedEntities)
                //    {
                //        object o = EntityHelper.GetPropertyValue(entity, columnName);
                //        //if (o != null && !string.IsNullOrEmpty(o.ToString()))
                //        {
                //            selected[i].Add(o);
                //        }
                //    }
                //}
            }
            else
            {
                throw new NotSupportedException("Not Supported now!");
            }
        }

        internal static void ShowFormFromGrid(IArchiveMasterForm sourceForm, GridRelatedInfo info, bool asDetailDialog)
        {
            try
            {
                ISearchExpression se = GetSearchExpressionFromGrid(sourceForm, info, asDetailDialog);

                ShowFrom(info.Action.Name, se, asDetailDialog);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }


        internal static void ShowFrom(string actionId, ISearchExpression se, bool asDetailDialog)
        {
            IDisplayManagerContainer seeForm = null;
            ArchiveDetailForm detailForm = null;
            ISearchManager sm = null;
            if (!asDetailDialog)
            {
                seeForm = (ServiceProvider.GetService<IApplication>() as IWinFormApplication).ExecuteAction(actionId) as IDisplayManagerContainer;
                if (seeForm == null)
                {
                    throw new InvalidOperationException("未能创建目标窗体！");
                }

                sm = seeForm.DisplayManager.SearchManager;
            }
            else
            {
                //detailForm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(info.ToDetailForm)) as ArchiveDetailForm;
                //if (detailForm == null)
                //{
                //    MessageForm.ShowError("未能创建目标窗体！");
                //    return;
                //}
                //detailForm.UpdateContent();
                //findWindow = detailForm.DisplayManager.SearchManager;
            }

            if (sm == null)
            {
                throw new InvalidOperationException("未能找到目标窗体的查找窗口！");
            }

            //List<ISearchExpression> findList = new List<ISearchExpression>();
            //ISearchExpression findCondition = GetAndFindCondition(selected, 0, toColumns);
            //for (int i = 1; i < selected[0].Count; ++i)
            //{
            //    findCondition = new LogicalExpression(findCondition, GetAndFindCondition(selected, i, toColumns), LogicalOperator.Or);
            //}
            //findList.Add(findCondition);
            sm.LoadData(se, null);

            if (asDetailDialog)
            {
                if (detailForm.ControlManager.DisplayManager.Count == 0)
                {
                    ServiceProvider.GetService<IMessageBox>().ShowWarning("未能找到相应记录！");
                    return;
                }

                detailForm.ControlManager.DisplayManager.Position = 0;

                detailForm.ShowDialog();

                detailForm.Dispose();
            }
        }

        private static void GetDetailGridRows(GridRelatedInfo info, Xceed.Grid.DetailGrid detailGrid, List<object> selectedEntities)
        {
            foreach (Xceed.Grid.DataRow dataRow in detailGrid.DataRows)
            {
                if (info.GridLevel == Feng.Grid.MyGrid.GetGridLevel(dataRow.ParentGrid)
                    || string.IsNullOrEmpty(info.GridLevel))
                {
                    selectedEntities.Add(dataRow.Tag);
                }
                else 
                {
                    foreach(Xceed.Grid.DetailGrid dg in dataRow.DetailGrids)
                    {
                        GetDetailGridRows(info, dg, selectedEntities);
                    }
                }
            }
        }

        private static void GetDetailGridRows(GridRelatedAddressInfo info, Xceed.Grid.DetailGrid detailGrid, List<object> selectedEntities)
        {
            foreach (Xceed.Grid.DataRow dataRow in detailGrid.DataRows)
            {
                if (info.GridLevel == Feng.Grid.MyGrid.GetGridLevel(dataRow.ParentGrid)
                    || string.IsNullOrEmpty(info.GridLevel))
                {
                    selectedEntities.Add(dataRow.Tag);
                }
                else
                {
                    foreach (Xceed.Grid.DetailGrid dg in dataRow.DetailGrids)
                    {
                        GetDetailGridRows(info, dg, selectedEntities);
                    }
                }
            }
        }
    }
}
