using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 带Grid的Form
    /// </summary>
    public partial class ArchiveGridForm : MyChildForm, IGridContainer
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
                if (this.MasterGrid != null)
                {
                    this.MasterGrid.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveGridForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 主窗体上的Grid
        /// </summary>
        public virtual IGrid MasterGrid
        {
            get { return null; }
        }

        ///// <summary>
        ///// 当前Grid（包括DetailForm上的grid）
        ///// </summary>
        //public virtual IBoundGrid CurrentGrid
        //{
        //    get { return null; }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            if (this.ToolStrip != null)
            {
                SetToolStripClickEvent(this.ToolStrip, true);
            }
            if (this.MenuStrip != null)
            {
                SetMenuStripClickEvent(this.MenuStrip, true);
            }

            base.Form_Load(sender, e);
        }

        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.ToolStrip != null)
            {
                SetToolStripClickEvent(this.ToolStrip, false);
            }
            if (this.MenuStrip != null)
            {
                SetMenuStripClickEvent(this.MenuStrip, false);
            }

            base.Form_Closing(sender, e);
        }
        #region "Cancel Grid Edit"

        private void SetToolStripClickEvent(ToolStrip toolStrip, bool set)
        {
            foreach (ToolStripItem item in toolStrip.Items)
            {
                if (set)
                {
                    item.MouseDown += new MouseEventHandler(menuStripItem_Click);
                }
                else
                {
                    item.MouseDown -= new MouseEventHandler(menuStripItem_Click);
                }
            }
        }

        private void SetMenuStripClickEvent(MenuStrip menuStrip, bool set)
        {
            foreach (ToolStripItem item in menuStrip.Items)
            {
                ToolStripMenuItem mItem = item as ToolStripMenuItem;
                if (mItem == null)
                {
                    continue;
                }

                mItem.MouseDown += new MouseEventHandler(menuStripItem_Click);
                foreach (ToolStripItem item2 in mItem.DropDownItems)
                {
                    ToolStripMenuItem mItem2 = item2 as ToolStripMenuItem;
                    if (mItem2 == null)
                    {
                        continue;
                    }

                    if (set)
                    {
                        mItem2.MouseDown += new MouseEventHandler(menuStripItem_Click);
                    }
                    else
                    {
                        mItem2.MouseDown -= new MouseEventHandler(menuStripItem_Click);
                    }
                }
            }
        }

        private void menuStripItem_Click(object sender, MouseEventArgs e)
        {
            if (this.MasterGrid != null)
            {
                MyGrid.CancelEditCurrentDataRow(this.MasterGrid);
            }
        }

        #endregion
    }
}
