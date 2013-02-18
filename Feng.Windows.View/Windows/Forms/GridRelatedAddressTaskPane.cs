using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.SmartUI;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class GridRelatedAddressTaskPane : Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane
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

                var archiveSeeForm = m_parentForm;
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
        /// <param name="addressTextBox"></param>
        public GridRelatedAddressTaskPane(string gridName, IDisplayManager dm, IArchiveMasterForm parentForm, LinkLabel addressTextBox)
        {
            XceedUtility.SetUIStyle(this);

            m_parentForm = parentForm;
            m_dm = dm;
            m_addressTextBox = addressTextBox;

            LoadMenus(gridName);
            if (m_items1.Count != 0)
            {
                var archiveSeeForm = parentForm;
                if (archiveSeeForm != null)
                {
                    archiveSeeForm.MasterGrid.GridControl.CurrentRowChanged += new EventHandler(MasterGrid_CurrentRowChanged);

                    MasterGrid_CurrentRowChanged(archiveSeeForm.DisplayManager, System.EventArgs.Empty);
                }

                if (m_addressTextBox != null)
                {
                    m_addressTextBox.Text = null;
                }
            }
            else
            {
                if (m_addressTextBox != null)
                {
                    m_addressTextBox.Visible = false;
                }
            }
        }
        void MasterGrid_CurrentRowChanged(object sender, EventArgs e)
        {
            var currentRow = m_parentForm.MasterGrid.CurrentRow;
            string currentLevel = currentRow == null ? null : Feng.Grid.MyGrid.GetGridLevel(currentRow.ParentGrid);

            object entity = currentRow == null ? null : m_parentForm.MasterGrid.CurrentRow.Tag;

            foreach (SmartItem item in m_items1)
            {
                GridRelatedAddressInfo info = item.Tag as GridRelatedAddressInfo;
                if (info.RelatedType == GridRelatedType.ByRows)
                {
                    if (entity == null || (info.GridLevel != currentLevel && !string.IsNullOrEmpty(info.GridLevel))) //Feng.Utils.ReflectionHelper.GetTypeFromName(info.EntityType) != entity.GetType())
                    {
                        item.Visible = false;
                    }
                    else
                    {
                        item.Visible = Permission.AuthorizeByRule(info.Visible, entity);
                    }
                }
            }
        }

        private LinkLabel m_addressTextBox;
        private IArchiveMasterForm m_parentForm;
        private IDisplayManager m_dm;
        private List<SmartItem> m_items1 = new List<SmartItem>();

        private void LoadGridRelated(string gridName)
        {
            IList<GridRelatedAddressInfo> list = ADInfoBll.Instance.GetGridRelatedAddressInfo(gridName);
            if (list == null || list.Count == 0)
            {
                return;
            }

            int groupIdx1 = -1;
            int groupIdx3 = -1;

            foreach (GridRelatedAddressInfo info in list)
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
                                if (groupIdx1 == -1)
                                {
                                    groupIdx1 = this.Items.Add("生成地址-按选定行");
                                }

                                int childIdx1 = this.Items[groupIdx1].Items.Add(info.Name);
                                this.Items[groupIdx1].Items[childIdx1].Text = info.Text;
                                this.Items[groupIdx1].Items[childIdx1].ToolTipText = info.Help;
                                this.Items[groupIdx1].Items[childIdx1].Click += new Xceed.SmartUI.SmartItemClickEventHandler(GridGotoFormTaskPane_Click);
                                this.Items[groupIdx1].Items[childIdx1].Tag = info;
                                m_items1.Add(this.Items[groupIdx1].Items[childIdx1]);
                            }
                            break;
                        case GridRelatedType.BySearchExpression:
                            {
                                // 不跟选定行相关，所以Visible是通用的，无参数
                                if (!Authority.AuthorizeByRule(info.Visible))
                                    continue;

                                if (groupIdx3 == -1)
                                {
                                    groupIdx3 = this.Items.Add("生成地址-按搜索条件");
                                }
                                int childIdx1 = this.Items[groupIdx3].Items.Add(info.Name);
                                this.Items[groupIdx3].Items[childIdx1].Text = info.Text;
                                this.Items[groupIdx3].Items[childIdx1].ToolTipText = info.Help;
                                this.Items[groupIdx3].Items[childIdx1].Click += new Xceed.SmartUI.SmartItemClickEventHandler(GridGotoFormTaskPane_Click);
                                this.Items[groupIdx3].Items[childIdx1].Tag = info;
                            }
                            break;
                        default:
                            throw new NotSupportedException("Invalid GridRelatedAddressInfo info of " + info.Name);
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
            GridRelatedAddressInfo info = item.Tag as GridRelatedAddressInfo;

            ISearchExpression newSe = null;
            switch (info.RelatedType)
            {
                case GridRelatedType.ByRows:
                    {
                        newSe = GridGotoFormTaskPane.GetSearchExpressionFromGrid(m_parentForm, info, false);
                    }
                    break;
                case GridRelatedType.BySearchExpression:
                    {
                        SearchHistoryInfo his = m_dm.SearchManager.GetHistory(0);

                        if (string.IsNullOrEmpty(his.Expression))
                        {
                            MessageForm.ShowWarning("还未有搜索条件！");
                            return;
                        }
                        string newSearchExpression = info.SearchExpression;
                        ISearchExpression oldSe = SearchExpression.Parse(his.Expression);
                        Dictionary<string, object> dict = GridGotoFormTaskPane.GetSearchExpreesionValues(oldSe);

                        ISearchExpression se = SearchExpression.Parse(newSearchExpression);

                        newSe = GridGotoFormTaskPane.ReplaceSearchExpreesionValues(newSe, dict);
                    }
                    break;
                default:
                    throw new NotSupportedException("Invalid GridRelatedInfo's RelatedType of " + info.Name);
            }

            if (newSe != null)
            {
                string s = string.Format(info.Address, newSe.ToString());

                ClipboardHelper.CopyTextToClipboard(s);
                if (m_addressTextBox != null)
                {
                    m_addressTextBox.Text = s;
                }
            }
        }

    }
}
