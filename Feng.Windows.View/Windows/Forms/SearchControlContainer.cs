using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SearchControlContainer : MyUserControl, ILayoutControl, IProfileLayoutControl
    {
        #region "Constructor"
        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                if (m_sm != null)
                {
                    m_sm.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchControlContainer()
        {
            InitializeComponent();

            this.tsmResetSearchControls.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconReset.png").Reference;
            this.tsmPresetLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPreset.png").Reference;
            this.tsmLoadLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconOpen.png").Reference;
            this.tsmSaveLayout.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSave.png").Reference;
        }

        #endregion

        #region "ISearchManager"

        /// <summary>
        /// 
        /// </summary>
        private ISearchManager m_sm;

        /// <summary>
        /// SetSearchManager
        /// </summary>
        public void SetSearchManager(ISearchManager sm)
        {
            m_sm = sm;

            //this.Size = new Size(this.Size.Width, flowLayoutPanel.Size.Height + 60);
        }

        ///// <summary>
        ///// 根据<see cref="GridColumnInfo"/>数据自动创建搜索控件，自动创建FlowLayoutPanel
        ///// </summary>
        ///// <param name="infos"></param>
        //public void LoadSearchControls(IList<GridColumnInfo> infos)
        //{
        //    System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        //    flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //                | System.Windows.Forms.AnchorStyles.Left)
        //                | System.Windows.Forms.AnchorStyles.Right)));
        //    flowLayoutPanel1.AutoScroll = true;
        //    flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        //    flowLayoutPanel1.Name = "flowLayoutPanel1";
        //    flowLayoutPanel1.Size = new System.Drawing.Size(this.Width - 6, this.Height - 56);

        //    this.Controls.Add(flowLayoutPanel1);

        //    LoadSearchControls(infos, flowLayoutPanel1);
        //}

        private string m_gridName;

        private System.Windows.Forms.FlowLayoutPanel m_flpNormal, m_flpHidden;
        /// <summary>
        /// 根据<see cref="GridColumnInfo"/>数据自动创建搜索控件
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="flowLayoutPanelNormal"></param>
        /// <param name="flowLayoutPanelHidden"></param>
        public void LoadSearchControls(string gridName, System.Windows.Forms.FlowLayoutPanel flowLayoutPanelNormal, System.Windows.Forms.FlowLayoutPanel flowLayoutPanelHidden)
        {
            m_flpNormal = flowLayoutPanelNormal;
            m_flpHidden = flowLayoutPanelHidden;
            m_gridName = gridName; 

            if (!string.IsNullOrEmpty(gridName))
            {
                IList<GridColumnInfo> infos = ADInfoBll.Instance.GetGridColumnInfos(gridName);
                foreach (GridColumnInfo info in infos)
                {
                    if (string.IsNullOrEmpty(info.SearchControlType))
                    {
                        continue;
                    }

                    ISearchControl sc = ControlFactory.GetSearchControl(info, m_sm);
                    m_sm.SearchControls.Add(sc);
                    sc.AvailableChanged += new EventHandler(sc_AvailableChanged);

                    Control c = sc as Control;
                    if (c != null)
                    {
                        flowLayoutPanelNormal.Controls.Add(c);
                    }
                    else
                    {
                        throw new ArgumentException("In SearchControlConainer only Windows.Control is permitted!");
                    }
                }
            }
        }

        private Dictionary<Control, int> m_controlIndex = new Dictionary<Control, int>();
        void sc_AvailableChanged(object sender, EventArgs e)
        {
            ISearchControl sc = sender as ISearchControl;
            Control c = sender as Control;
            if (sc.Available)
            {
                if (!m_flpNormal.Controls.Contains(c))
                {
                    m_flpHidden.Controls.Remove(c);
                    m_flpNormal.Controls.Add(c);

                    if (m_controlIndex.ContainsKey(c))
                    {
                        m_flpNormal.Controls.SetChildIndex(c, m_controlIndex[c]);
                    }
                }
            }
            else
            {
                int n = m_flpNormal.Controls.IndexOf(c);
                if (n != -1)
                {
                    m_controlIndex[c] = n;
                    m_flpNormal.Controls.Remove(c);
                    m_flpHidden.Controls.Add(c);

                    m_flpHidden.Controls.SetChildIndex(c, n);
                }
            }
        }

        #endregion

        #region "Layout"

        public bool LoadLayout(string fileName)
        {
            AMS.Profile.IProfile profile = new AMS.Profile.Xml(fileName);
            return LoadLayout(profile);
        }

        // <summary>
        /// DefaultDataDirectory
        /// </summary>
        public virtual string DefaultDataDirectory
        {
            get
            {
                string dirPath = SystemDirectory.UserDataDirectory + (m_sm.Name + "\\") + m_gridName + "\\";
                return dirPath;
            }
        }

        private static string m_layoutDefaultFileName = "system.xmls.default";
        public string LayoutFilePath
        {
            get
            {
                if (m_sm != null)
                {
                    return this.m_sm.Name + "\\" + m_layoutDefaultFileName;
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

        /// <summary>
        /// 保存查找控件信息
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        public bool LoadLayout(AMS.Profile.IProfile profile)
        {
            string sectionName = "SearchControlContainer." + "." + m_sm.Name + ".Layout";

            string s = profile.GetValue(sectionName, "SearchControls", "");
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            string[] columns = s.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string columnName in columns)
            {
                string[] ss = columnName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length != 3)
                {
                    continue;
                }

                ISearchControl sc = m_sm.SearchControls[ss[0]];
                if (sc == null)
                {
                    continue;
                }

                GridColumnInfo info = sc.Tag as GridColumnInfo;
                if (info == null || (!string.IsNullOrEmpty(info.SearchControlType)
                        && Authority.AuthorizeByRule(info.SearchControlVisible)))
                {
                    sc.Available = Convert.ToBoolean(ss[1]);
                }
                else
                {
                    m_sm.SearchControls[ss[0]].Available = false;
                }

                sc.Index = Convert.ToInt32(ss[2]);
            }
            return true;
        }

        /// <summary>
        /// 保存Column布局
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sectionName"></param>
        public bool SaveLayout(AMS.Profile.IProfile profile)
        {
            if (m_sm == null)
                return false;
            string sectionName = "SearchControlContainer." + "." + m_sm.Name + ".Layout";

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_sm.SearchControls.Count; ++i)
            {
                sb.Append(m_sm.SearchControls[i].Name);
                sb.Append(",");
                sb.Append(m_sm.SearchControls[i].Available);
                sb.Append(",");
                sb.Append(m_sm.SearchControls[i].Index);
                sb.Append(System.Environment.NewLine);
            }
            profile.SetValue(sectionName, "SearchControls", sb.ToString());

            return true;
        }

        private void tsmLoadLayout_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = DefaultDataDirectory;
            openFileDialog1.Filter = "配置文件 (*.xmls)|*.xmls";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.LoadLayout(openFileDialog1.FileName);
            }
            openFileDialog1.Dispose();
        }

        private void tsmSaveLayout_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = DefaultDataDirectory;
            saveFileDialog1.Filter = "配置文件 (*.xmls)|*.xmls";
            //saveFileDialog1.Title = "保存";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AMS.Profile.IProfile profile = new AMS.Profile.Xml(saveFileDialog1.FileName);
                this.SaveLayout(profile);
            }
            saveFileDialog1.Dispose();
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tsmPresetLayout.DropDownItems.Count == 0)
            {
                string[] folders = new string[] { DefaultDataDirectory };

                foreach (string folder in folders)
                {
                    if (!System.IO.Directory.Exists(folder))
                    {
                        continue;
                    }
                    foreach (string fileName in System.IO.Directory.GetFiles(folder, "*.xmls"))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = System.IO.Path.GetFileName(fileName).Replace(".xmls", "");
                        item.Tag = fileName;
                        item.Click += new EventHandler(tsmPresetSubitem_Click);
                        tsmPresetLayout.DropDownItems.Add(item);
                    }
                }
                if (tsmPresetLayout.DropDownItems.Count == 0)
                {
                    tsmPresetLayout.Visible = false;
                }
            }
        }
        private void tsmPresetSubitem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string fileName = item.Tag.ToString();
            this.LoadLayout(fileName);
        }

        private void tsmResetSearchControls_Click(object sender, EventArgs e)
        {
            ArchiveSetupForm.ResetSearchControlInfos(null, m_sm.DisplayManager);

            this.SaveLayout();
        }

        private void tsmSetup_Click(object sender, EventArgs e)
        {
            using (ArchiveSetupForm form = new ArchiveSetupForm(null, m_sm.DisplayManager))
            {
                form.ShowDialog();
            }
        }

        #endregion

        

        
    }
}