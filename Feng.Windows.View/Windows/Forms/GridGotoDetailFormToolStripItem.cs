using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GridGotoDetailFormToolStripItem : ToolStripDropDownButton
    {
        /// <summary>
        /// 
        /// </summary>
        public GridGotoDetailFormToolStripItem()
        {
            InitializeComponent();
        }

        private ADInfoBll m_bll = ADInfoBll.Instance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void LoadMenus(string name)
        {
            this.DropDownItems.Clear();

            IList<GridRelatedInfo> list = m_bll.GetGridRelatedInfo(name);
            if (list == null || list.Count == 0)
            {
                this.Visible = false;
                return;
            }

            foreach (GridRelatedInfo info in list)
            {
                ToolStripItem item;
                if (info.Name.ToUpper() == "SEPARATOR")
                {
                    item = new ToolStripSeparator();
                }
                else
                {
                    item = new ToolStripMenuItem();
                    item.Name = info.Name + "toolStripMenuItem";
                    item.Size = new System.Drawing.Size(172, 22);
                    item.Text = info.Text;
                    item.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
                    item.Tag = info;
                }

                this.DropDownItems.Add(item);
            }

            this.DropDownOpened -= new EventHandler(GridGotoDetailFormToolStripItem_DropDownOpened);
            this.DropDownOpened += new EventHandler(GridGotoDetailFormToolStripItem_DropDownOpened);
        }

        void GridGotoDetailFormToolStripItem_DropDownOpened(object sender, EventArgs e)
        {
            IDisplayManagerContainer parentForm = this.Parent.FindForm() as IDisplayManagerContainer;
            if (parentForm != null)
            {
                object entity = parentForm.DisplayManager.Position == -1 ? null : parentForm.DisplayManager.CurrentItem;

                foreach (ToolStripItem item in this.DropDownItems)
                {
                    GridRelatedInfo info = item.Tag as GridRelatedInfo;
                    item.Visible = Permission.AuthorizeByRule(info.Visible, entity);
                }
            }
        }


        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var childForm = this.Parent.FindForm();
            if (childForm == null)
            {
                MessageForm.ShowError("未能找到父窗体！");
                return;
            }
            var masterForm = childForm.ParentForm as IArchiveMasterForm;

            ToolStripMenuItem item = sender as ToolStripMenuItem;
            GridRelatedInfo info = item.Tag as GridRelatedInfo;

            if (info.RelatedType == GridRelatedType.ByRows)
            {
                GridGotoFormTaskPane.ShowFormFromGrid(masterForm, info, true);
            }
            else
            {
                throw new NotSupportedException("Not Supported now!");
            }
        }
    }
}
