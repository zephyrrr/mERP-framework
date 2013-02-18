using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xceed.SmartUI;
using Feng.Scripts;

namespace Feng.Windows.Forms
{
    public class TaskPane : Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane 
    {
        private string m_name;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        public TaskPane(string name)
        {
            InitializeComponent();

            Feng.Windows.Utils.XceedUtility.SetUIStyle(this);

            m_name = name;

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(TaskPane_KeyDown);
        }

        private void tsmRefresh_Click(object sender, EventArgs e)
        {
            RefreshItem();
        }

        public void RefreshItem()
        {
            this.Items.Clear();
            LoadMenus(m_name);
        }

        void TaskPane_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                RefreshItem();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void LoadMenus(string groupName)
        {
            IList<TaskInfo> list = ADInfoBll.Instance.GetTaskInfo(groupName);
            if (list == null || list.Count == 0)
            {
                return;
            }

            //ProgressForm progressForm = new ProgressForm();
            //progressForm.Start(this.FindForm(), "计算");

            Dictionary<string, int> groupIdxs = new Dictionary<string, int>();
            foreach (TaskInfo info in list)
            {
                ActionInfo menuInfo = ADInfoBll.Instance.GetActionInfo(info.Action.Name);
                if (Authority.AuthorizeByRule(menuInfo.Access))
                {
                    if (Authority.AuthorizeByRule(info.Visible))
                    {
                        if (!groupIdxs.ContainsKey(info.ParentName))
                        {
                            groupIdxs[info.ParentName] = this.Items.Add(info.ParentName);
                        }
                        int groupIdx = groupIdxs[info.ParentName];
                        int childIdx = this.Items[groupIdx].Items.Add(info.Name);
                        this.Items[groupIdx].Items[childIdx].Text = info.Text;
                        this.Items[groupIdx].Items[childIdx].Click += new SmartItemClickEventHandler(TaskPane_Click);
                        this.Items[groupIdx].Items[childIdx].Tag = info;
                        //this.Items[groupIdx].Items[childIdx].Visible = Authority.AuthorizeByRule(info.Visible);

                        //if (this.Items[groupIdx].Items[childIdx].Visible)
                        {
                            RepalceParameterizedText(groupIdx, childIdx);
                        }
                    }
                }
            }

            //progressForm.Stop();
        }

        private System.Windows.Forms.ToolStripMenuItem tsmRefresh;

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

        private static Regex s_regexOwnData = new Regex(@"\%(.*?)(:(.*?))?\%", RegexOptions.Compiled);
        private void RepalceParameterizedText(int groupIdx, int childIdx)
        {
            TaskInfo info = this.Items[groupIdx].Items[childIdx].Tag as TaskInfo;

            Feng.Async.AsyncHelper asyncHelper = new Feng.Async.AsyncHelper(
                new Feng.Async.AsyncHelper.DoWork(delegate()
                {
                    lock (this)
                    {
                        string result = GetVariableText(info);
                        return result;
                    }
                }),
                new Feng.Async.AsyncHelper.WorkDone(delegate(object result)
                {
                    if (result != null)
                    {
                        this.Items[groupIdx].Items[childIdx].Text = (string)result;
                    }
                }));
        }

        private string GetVariableText(TaskInfo info)
        {
            string text = info.Text;
            MatchCollection mc = s_regexOwnData.Matches(text);
            foreach (Match m in mc)
            {
                string s = string.Empty;
                switch (m.Groups[1].Value.ToUpper())
                {
                    case "COUNT":
                        if (!string.IsNullOrEmpty(info.SearchManagerClassName))
                        {
                            ISearchManager searchManager = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(info.SearchManagerClassName, info.SearchManagerClassParams);
                            s = searchManager.GetCount(SearchExpression.Parse(EntityHelper.ReplaceExpression(info.SearchExpression))).ToString();

                            m_sms[info.Name] = searchManager;
                        }
                        break;
                    case "EXP":
                        string exp = m.Groups[3].Value.Trim();
                        object ret = PythonHelper.ExecutePythonExpression(exp, null);
                        s = ret == null ? string.Empty : ret.ToString();
                        break;
                    default:
                        throw new NotSupportedException("Invalid text of " + text);
                }
                text = text.Replace(m.Groups[0].Value, s);
            }
            return text;
        }
        private Dictionary<string, ISearchManager> m_sms = new Dictionary<string, ISearchManager>();
        void TaskPane_Click(object sender, Xceed.SmartUI.SmartItemClickEventArgs e)
        {
            Xceed.SmartUI.SmartItem item = sender as Xceed.SmartUI.SmartItem;
            TaskInfo info = item.Tag as TaskInfo;

            IArchiveMasterForm seeForm = ServiceProvider.GetService<IApplication>().ExecuteAction(info.Action.Name) as IArchiveMasterForm;
            ArchiveOperationForm operForm = seeForm as ArchiveOperationForm;
            if (seeForm == null)
            {
                MessageForm.ShowError("未能创建目标窗体！");
                return;
            }
            switch(info.TaskType)
            {
                case TaskType.Add:
                    operForm.DoAdd();
                    break;
                case TaskType.Edit:
                    ISearchManager smSrc;
                    if (m_sms.ContainsKey(info.Name))
                    {
                        smSrc = m_sms[info.Name];
                    }
                    else
                    {
                        smSrc = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(info.SearchManagerClassName, info.SearchManagerClassParams);
                    }

                    ISearchManager smDest = seeForm.DisplayManager.SearchManager;
                    if (smSrc.GetType() == smDest.GetType())
                    {
                        smDest.LoadData(SearchExpression.Parse(EntityHelper.ReplaceExpression(info.SearchExpression)), null);
                    }
                    else
                    {
                        smSrc.EnablePage = false;
                        object dataSource = smSrc.GetData(SearchExpression.Parse(EntityHelper.ReplaceExpression(info.SearchExpression)), null);

                        IDisplayManager dm = seeForm.DisplayManager;
                        dm.SetDataBinding(dataSource, string.Empty);
                        dm.SearchManager.OnDataLoaded(new DataLoadedEventArgs(dataSource, 0));
                    }
                    break;
                default:
                    throw new NotSupportedException("Invalide TaskType");
            }
        }

        #region "Windows 窗体设计器生成的代码"
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // tsmRefresh
            // 
            this.tsmRefresh.Name = "tsmRefresh";
            this.tsmRefresh.Size = new System.Drawing.Size(152, 22);
            this.tsmRefresh.Text = "刷新(&R)";
            this.tsmRefresh.Click += new System.EventHandler(this.tsmRefresh_Click);
            // 
            // TaskPane
            // 
            this.ContextMenuStrip = contextMenuStrip1;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "TaskPane";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.ComponentModel.IContainer components;
        #endregion

    }
}
