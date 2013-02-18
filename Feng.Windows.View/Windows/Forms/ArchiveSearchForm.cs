using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xceed.SmartUI;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ArchiveSearchForm : UserControl, IStateControl, IReadOnlyControl, IProfileLayoutControl, ILayoutControl
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                this.PerformLayout();   // remove cachedLayoutEventArgs

                if (this.searchControlContainer1 != null)
                {
                    this.searchControlContainer1.Dispose();
                    this.searchControlContainer1 = null;
                }

                if (m_progressForm != null)
                {
                    m_progressForm.Dispose();
                }

                tabControl1.SelectedIndexChanged -= new EventHandler(tabControl1_SelectedIndexChanged);

                if (m_sm != null)
                {
                    m_sm.DataLoaded -= new EventHandler<DataLoadedEventArgs>(searchManager_DataLoaded);
                    m_sm.DataLoading -= new EventHandler<DataLoadingEventArgs>(searchManager_DataLoading);

                    m_sm.Dispose();
                    m_sm = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        private ArchiveSearchForm()
        {
            InitializeComponent();

            XceedUtility.SetUIStyle(historyPane);
            XceedUtility.SetUIStyle(customSearchPane);

            lblResult.Text = "";

            if (!Authority.IsAdministrators())
            {
                tabControl1.TabPages.Remove(tabPage4);
            }

            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);

            this.EnableProgressForm = true;
        }

        private Form m_parentForm;

        /// <summary>
        /// 
        /// </summary>
        public Form ContainerForm
        {
            get { return m_parentForm; }
        }

        //// <summary>
        ///// DefaultDataDirectory
        ///// </summary>
        //public virtual string DefaultDataDirectory
        //{
        //    get
        //    {
        //        string dirPath = SystemConfiguration.UserDataDirectory + m_sm.Name + "\\";
        //        return dirPath;
        //    }
        //}

        private static string m_layoutDefaultFileName = "system.xmls.default";
        public string LayoutFilePath
        {
            get
            {
                if (m_sm != null)
                {
                    return this.m_sm.Name + "\\" +  m_layoutDefaultFileName;
                }                                   
                else
                {
                    return null;
                }
            }
        }

        public bool LoadLayout()
        {
            return LayoutControlExtention.LoadLayout(this);
        }

        public bool SaveLayout()
        {
            return LayoutControlExtention.SaveLayout(this);
        }

        public bool LoadLayout(AMS.Profile.IProfile profile)
        {
            int r = profile.GetValue("SearchManager." + m_sm.Name, "MaxResult", -1);
            if (r != -1)
            {
                m_sm.MaxResult = r;
            }

            string history = profile.GetValue("SearchManager." + m_sm.Name, "History", string.Empty);
            if (!string.IsNullOrEmpty(history))
            {
                string[] ss = history.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                for(int i=ss.Length -1; i>=0; --i)
                {
                    string s = ss[i];
                    if (string.IsNullOrEmpty(s))
                        continue;
                    string[] sss = s.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sss.Length == 0)
                        continue;
                    ISearchExpression se = null;
                    try
                    {
                        se = SearchExpression.Parse(sss[0]);
                    }
                    catch (Exception)
                    {
                    }

                    if (se == null)
                        continue;
                    IList<ISearchOrder> so = null;
                    if (sss.Length > 1)
                    {
                        so = SearchOrder.Parse(sss[1]);
                    }
                    SearchHistoryInfo his = m_sm.SetHistory(se, so);
                    his.IsCurrentSession = false;
                }
            }

            bool ret = this.searchControlContainer1.LoadLayout(profile);
            return ret;
        }

        public bool SaveLayout(AMS.Profile.IProfile profile)
        {
            if (m_sm == null)
                return false;
            if (m_sm.MaxResult != SearchManagerDefaultValue.MaxResult)
            {
                profile.SetValue("SearchManager." + m_sm.Name, "MaxResult", m_sm.MaxResult);
            }

            StringBuilder sb = new StringBuilder();
            int idx = 0;
            while (true)
            {
                SearchHistoryInfo his = m_sm.GetHistory(idx);
                if (!string.IsNullOrEmpty(his.Expression))
                {
                    sb.Append(his.Expression);
                    if (!string.IsNullOrEmpty(his.Order))
                    {
                        sb.Append(";");
                        sb.Append(his.Order);
                    }
                    sb.Append(Environment.NewLine);

                    idx++;
                }
                else
                {
                    break;
                }
            }
            profile.SetValue("SearchManager." + m_sm.Name, "History", sb.ToString());

            bool ret = this.searchControlContainer1.SaveLayout(profile);
            return ret;
        }

        private WindowTabInfo m_winTabInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="archiveSeeForm"></param>
        /// <param name="sm"></param>
        /// <param name="winTabInfo"></param>
        public ArchiveSearchForm(Form parentForm, ISearchManager sm, WindowTabInfo winTabInfo)
            : this()
        {
            m_parentForm = parentForm;

            m_sm = sm;
            m_winTabInfo = winTabInfo;

            m_sm.DataLoaded += new EventHandler<DataLoadedEventArgs>(searchManager_DataLoaded);
            m_sm.DataLoading += new EventHandler<DataLoadingEventArgs>(searchManager_DataLoading);

            searchControlContainer1.SetSearchManager(sm);
            searchControlContainer1.LoadSearchControls(winTabInfo.GridName, this.flowLayoutPanelSearchControlNormal, this.flowLayoutPanelSearchControlHidden);

            //tabControl1.SetTabPageBackColor();

            LoadCustomSearch(winTabInfo.GridName);

            LoadLayout();
        }

        void searchManager_DataLoading(object sender, DataLoadingEventArgs e)
        {
            if (e.Cancel)
                return;

            if (!m_sm.UseStreamLoad)
            {
                this.StartLoadData();
            }
        }
        private void searchManager_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            if (!m_sm.UseStreamLoad)
            {
                this.StopLoadData();
            }
        }

        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                historyPane.Items.Clear();

                int groupIdx1 = historyPane.Items.Add("历史");

                int idx = 0;
                while(true)
                {
                    SearchHistoryInfo his = m_sm.GetHistory(idx);

                    // 如果为null，则还不存在记录。如果为空，是全部
                    if (his.Expression == null)
                        break;

                    string text;
                    if (string.IsNullOrEmpty(his.Expression))
                    {
                        text = "全部";
                    }
                    else
                    {
                        text = his.Expression;
                    }
                    if (!string.IsNullOrEmpty(his.Order))
                    {
                        text += ", ORDER BY " + his.Order;
                    }

                    text = text.Substring(0, Math.Min(text.Length, 100));
                    int childIdx1 = historyPane.Items[groupIdx1].Items.Add(text);
                    historyPane.Items[groupIdx1].Items[childIdx1].Text = text;
                    historyPane.Items[groupIdx1].Items[childIdx1].ToolTipText = text;
                    historyPane.Items[groupIdx1].Items[childIdx1].Click += new SmartItemClickEventHandler(HistoryPaneItem_Click);
                    historyPane.Items[groupIdx1].Items[childIdx1].Tag = his;
                    historyPane.Items[groupIdx1].Items[childIdx1].MouseDown += new MouseEventHandler(ArchiveSearchForm_MouseDown);
                    idx++;
                }
            }
        }

        private SmartItem m_currentContextItem;
        void ArchiveSearchForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_currentContextItem = sender as SmartItem;
                contextMenuStrip1.Show(m_currentContextItem.ParentSmartControl.PointToScreen(new Point(e.X, e.Y)));
            }
        }
        private void tsmCopyHistory_Click(object sender, EventArgs e)
        {
            if (m_currentContextItem == null)
                return;

            SearchHistoryInfo his = m_currentContextItem.Tag as SearchHistoryInfo;

            ClipboardHelper.CopyTextToClipboard(his.Expression);

            m_currentContextItem = null;
        }

        void HistoryPaneItem_Click(object sender, SmartItemClickEventArgs e)
        {
            Xceed.SmartUI.SmartItem item = sender as Xceed.SmartUI.SmartItem;
            SearchHistoryInfo his = item.Tag as SearchHistoryInfo;
            m_sm.FirstResult = 0;
            m_sm.LoadData(SearchExpression.Parse(his.Expression), SearchOrder.Parse(his.Order));
        }

        private void LoadCustomSearch(string gridName)
        {
            IList<CustomSearchInfo> list = ADInfoBll.Instance.GetCustomSearchInfo(gridName);
            if (list == null || list.Count == 0)
            {
                return;
            }

            int groupIdx1 = customSearchPane.Items.Add("预定义");

            foreach (CustomSearchInfo info in list)
            {
                if (!Authority.AuthorizeByRule(info.Visible))
                    continue;

                int childIdx1 = customSearchPane.Items[groupIdx1].Items.Add(info.Name);
                customSearchPane.Items[groupIdx1].Items[childIdx1].Text = info.Text;
                customSearchPane.Items[groupIdx1].Items[childIdx1].ToolTipText = info.Help;
                customSearchPane.Items[groupIdx1].Items[childIdx1].Click += new SmartItemClickEventHandler(CustomSearchItem_Click);
                customSearchPane.Items[groupIdx1].Items[childIdx1].Tag = info;
            }
        }

        void CustomSearchItem_Click(object sender, SmartItemClickEventArgs e)
        {
            Xceed.SmartUI.SmartItem item = sender as Xceed.SmartUI.SmartItem;
            CustomSearchInfo info = item.Tag as CustomSearchInfo;

            m_sm.FirstResult = 0;
            m_sm.LoadData(SearchExpression.Parse(info.SearchExpression), null);
        }

        private ISearchManager m_sm;

        private bool m_readOnly;

        /// <summary>
        /// ReadOnly
        /// </summary>
        public bool ReadOnly
        {
            get { return m_readOnly; }
            set
            {
                if (m_readOnly != value)
                {
                    m_readOnly = value;

                    this.btnSearch.Enabled = !value;
                    this.historyPane.Enabled = !value;
                    this.customSearchPane.Enabled = !value;
                    if (ReadOnlyChanged != null)
                    {
                        ReadOnlyChanged(this, System.EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReadOnlyChanged;

        /// <summary>
        /// SetState
        /// </summary>
        /// <param name="state"></param>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state, false);
        }

        private void FormatLabelResult()
        {
            lblResult.Visible = false;
            if (m_sm.MaxResult == 0)
            {
                lblResult.Text = "";
            }
            else
            {
                string str = "共" + m_sm.Count + "条 每页" + m_sm.MaxResult + "条" +
                             System.Environment.NewLine
                             + "第" + (m_sm.FirstResult / m_sm.MaxResult + 1) + "页/共"
                             + ((m_sm.Count - 1) / m_sm.MaxResult + 1) + "页";
                lblResult.Text = str;
            }
        }

        private void 下一页NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int next = m_sm.FirstResult + m_sm.MaxResult;
            if (next < m_sm.Count)
            {
                m_sm.FirstResult = next;
                m_sm.ReloadData();
            }
        }

        private void 上一页PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int prev = m_sm.FirstResult - m_sm.MaxResult;
            if (prev >= 0)
            {
                m_sm.FirstResult = prev;
                m_sm.ReloadData();
            }
        }

        private void 第一页FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_sm.FirstResult > 0)
            {
                m_sm.FirstResult = 0;
                m_sm.ReloadData();
            }
        }

        private void 最后一页LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int last = m_sm.MaxResult == 0
                           ? 0
                           : ((m_sm.Count - 1) / m_sm.MaxResult) * m_sm.MaxResult;
            if (m_sm.FirstResult != last)
            {
                m_sm.FirstResult = last;
                m_sm.ReloadData();
            }
        }

        private void 设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchWindowSetupForm form = new SearchWindowSetupForm(m_sm);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                m_sm.ReloadData();
                FormatLabelResult();

                SaveLayout();
            }
            form.Dispose();
        }

        private bool m_isLoading;

        /// <summary>
        /// 搜索动作执行后
        /// </summary>
        //public event CancelEventHandler SearchExecuting;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (!m_isLoading)
                {
                    //CancelEventArgs arg = new CancelEventArgs();
                    //if (SearchExecuting != null)
                    //{
                    //    SearchExecuting(sender, arg);
                    //}

                    //if (!arg.Cancel)
                    {
                        m_sm.FirstResult = 0;
                        m_sm.LoadDataAccordSearchControls();
                    }
                }
                else
                {
                    //m_sm.StopLoadData();

                    //StopLoadData();
                    // Nothing
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                m_sm.FirstResult = 0;
                if (ckbUseHql.Checked)
                {
                    if (!string.IsNullOrEmpty(txtSearchExpression.Text))
                    {
                        m_sm.LoadData(new Feng.Search.QueryExpression(txtSearchExpression.Text), SearchOrder.Parse(txtSearchOrder.Text));
                    }
                    else
                    {
                        m_sm.LoadData(null, null);
                    }
                }
                else
                {
                    m_sm.LoadData(SearchExpression.Parse(txtSearchExpression.Text), SearchOrder.Parse(txtSearchOrder.Text));
                }
            }
        }

        public bool EnableProgressForm
        {
            get;
            set;
        }

        private ProgressForm m_progressForm;
        private void StartLoadData()
        {
            if (this.EnableProgressForm)
            {
                if (m_progressForm == null)
                {
                    m_progressForm = new ProgressForm();
                    m_progressForm.ProgressStopped += new EventHandler(progressForm_ProgressStopped);
                }

                m_progressForm.Start(this.ContainerForm, "查找");
            }

            //this.btnSearch.Text = "停止";
            this.Enabled = false;
            m_isLoading = true;
        }

        private void StopLoadData()
        {
            if (this.EnableProgressForm)
            {
                if (m_progressForm != null)
                {
                    m_progressForm.ProgressStopped -= new EventHandler(progressForm_ProgressStopped);
                    m_progressForm.Stop();
                }
            }

            this.Enabled = true;
            m_isLoading = false;
        }

        void progressForm_ProgressStopped(object sender, EventArgs e)
        {
            if (m_sm != null)
            {
                m_sm.StopLoadData();
            }
        }

    }
}