using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Feng;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CustomSearchToolStripItem : ToolStripDropDownButton
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomSearchToolStripItem()
        {
            InitializeComponent();
        }

        private ADInfoBll m_bll = ADInfoBll.Instance;

        private ISearchManager m_sm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fm"></param>
        public void LoadMenus(ISearchManager sm, string name)
        {
            IList<CustomSearchInfo> list;

            list = m_bll.GetCustomSearchInfo(name);
            if (list == null || list.Count == 0)
            {
                this.Visible = false;
                return;
            }

            foreach (CustomSearchInfo info in list)
            {
                if (!Authority.AuthorizeByRule(info.Visible))
                    continue;

                ToolStripItem item;
                if (info.SearchExpression.ToUpper() == "SEPARATOR")
                {
                    item = new ToolStripSeparator();
                }
                else
                {
                    item = new ToolStripMenuItem();
                    item.Name = info.Name + "ToolStripItem";
                    item.Size = new System.Drawing.Size(172, 22);
                    item.Text = info.Text;
                    item.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
                    item.Tag = info;
                }

                this.DropDownItems.Add(item);
            }

            m_sm = sm;

            if (list.Count > 0)
            {
                ToolStripMenuItem_Click(this.DropDownItems[0], System.EventArgs.Empty);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ToolStripItemClicked;


        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                CustomSearchInfo info = item.Tag as CustomSearchInfo;

                m_sm.LoadData(SearchExpression.Parse(info.SearchExpression), null);

                if (ToolStripItemClicked != null)
                {
                    ToolStripItemClicked(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }
    }
}