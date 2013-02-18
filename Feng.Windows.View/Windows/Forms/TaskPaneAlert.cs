using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xceed.SmartUI;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public class TaskPaneAlert : Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane 
    {
        public TaskPaneAlert()
        {
            InitializeComponent();

            XceedUtility.SetUIStyle(this);

            //LoadMenus();

            this.KeyDown += new System.Windows.Forms.KeyEventHandler(TaskPane_KeyDown);
        }

        private void tsmFixIt_Click(object sender, EventArgs e)
        {

        }

        private LogEntityDao<AlertInfo> m_bll;

        private void FixIt(AlertInfo alertInfo)
        {
            if (m_bll == null)
            {
                m_bll = new LogEntityDao<AlertInfo>();
            }
            alertInfo.IsFixed = true;

            try
            {
                m_bll.Update(alertInfo);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }

        public void ReloadData()
        {
            this.Items.Clear();
            LoadItems();
        }

        private void tsmRefresh_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        void TaskPane_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.F5)
            {
                tsmRefresh_Click(tsmRefresh, System.EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void LoadItems()
        {
            IList<AlertInfo> list = ADInfoBll.Instance.GetAlertInfo();
            if (list == null || list.Count == 0)
            {
                return;
            }

            Dictionary<string, int> groupIdxs = new Dictionary<string, int>();
            foreach (AlertInfo info in list)
            {
                ActionInfo actionInfo = ADInfoBll.Instance.GetActionInfo(info.Action.Name);
                if (Authority.AuthorizeByRule(actionInfo.Access))
                {
                    if (!groupIdxs.ContainsKey("警示"))
                    {
                        groupIdxs["警示"] = this.Items.Add("警示");
                    }
                    int groupIdx = groupIdxs["警示"];

                    int childIdx = this.Items[groupIdx].Items.Add(info.Description);
                    this.Items[groupIdx].Items[childIdx].Click += new SmartItemClickEventHandler(TaskPaneAlert_Click);
                    this.Items[groupIdx].Items[childIdx].MouseDown += new System.Windows.Forms.MouseEventHandler(TaskPaneAlert_MouseDown);
                    this.Items[groupIdx].Items[childIdx].Tag = info;
                }
            }
        }

        void TaskPaneAlert_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //SmartItem item = sender as SmartItem;
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    contextMenuStrip2.Show(this.PointToScreen(new System.Drawing.Point(e.X, e.Y)));
            //}
        }

        void TaskPaneAlert_Click(object sender, Xceed.SmartUI.SmartItemClickEventArgs e)
        {
            Xceed.SmartUI.SmartItem item = sender as Xceed.SmartUI.SmartItem;
            AlertInfo info = item.Tag as AlertInfo;

            IArchiveMasterForm seeForm = ServiceProvider.GetService<IApplication>().ExecuteAction(info.Action.Name) as IArchiveMasterForm;
            if (seeForm == null)
            {
                MessageForm.ShowError("未能创建目标窗体！");
                return;
            }

            ISearchManager sm = seeForm.DisplayManager.SearchManager;
            sm.LoadData(SearchExpression.Parse(info.SearchExpression), null);

            FixIt(info);
        }

        #region "Windows 窗体设计器生成的代码"
        private System.Windows.Forms.ToolStripMenuItem tsmRefresh;
        private System.Windows.Forms.ToolStripMenuItem tsmFixIt;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFixIt = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmFixIt});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 48);
            // 
            // tsmRefresh
            // 
            this.tsmRefresh.Name = "tsmRefresh";
            this.tsmRefresh.Size = new System.Drawing.Size(152, 22);
            this.tsmRefresh.Text = "刷新(&R)";
            this.tsmRefresh.Click += new System.EventHandler(this.tsmRefresh_Click);
            // 
            // tsmFixIt
            // 
            this.tsmFixIt.Name = "tsmFixIt";
            this.tsmFixIt.Size = new System.Drawing.Size(152, 22);
            this.tsmFixIt.Text = "确认(&O)";
            this.tsmFixIt.Click += new System.EventHandler(this.tsmFixIt_Click);
            // 
            // TaskPane
            // 
            this.ContextMenuStrip = contextMenuStrip1;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "TaskPaneAlert";
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.ComponentModel.IContainer components;
        #endregion
    }
}
