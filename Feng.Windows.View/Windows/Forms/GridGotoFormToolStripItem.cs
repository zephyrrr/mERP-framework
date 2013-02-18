using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Feng.Windows.Utils;
using Feng.Search;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GridGotoFormToolStripItem : ToolStripDropDownButton
    {
        /// <summary>
        /// 
        /// </summary>
        public GridGotoFormToolStripItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void LoadMenus(string name)
        {
            this.DropDownItems.Clear();

            IList<GridRelatedInfo> list = ADInfoBll.Instance.GetGridRelatedInfo(name);
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
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            GridRelatedInfo info = item.Tag as GridRelatedInfo;

            GridGotoFormTaskPane.ShowFormFromGrid(this.Parent.FindForm() as IArchiveMasterForm, info, false);
        }

        //private static ISearchExpression GetAndFindCondition(ArrayList[] selected, int idx, string[] toColumnNames)
        //{
        //    ISearchExpression findCondition = SearchExpression.Eq(toColumnNames[0], selected[0][idx]);
        //    for (int i = 1; i < selected.Length; ++i)
        //    {
        //        findCondition = SearchExpression.And(findCondition, SearchExpression.Eq(toColumnNames[i], selected[i][idx]));
        //    }
        //    return findCondition;
        //}
    }
}