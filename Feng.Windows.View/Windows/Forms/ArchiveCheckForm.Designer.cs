namespace Feng.Windows.Forms
{
    partial class ArchiveCheckForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new Feng.Windows.Forms.MyToolStripButton();
            this.tsbSearch = new Feng.Windows.Forms.MyToolStripButton();
            this.tsbFind = new Feng.Windows.Forms.MyToolStripButton();
            this.tsbFilter = new Feng.Windows.Forms.MyToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSetup = new Feng.Windows.Forms.MyToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOk = new Feng.Windows.Forms.MyToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmBasicOperation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFind = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer1 = new Feng.Windows.Forms.MySplitContainer();
            this.pageNavigator1 = new Feng.Windows.Forms.PageNavigator();
            this.bindingNavigatorAllCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPageNameItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorPageCountItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorMoveLastItem = new Feng.Windows.Forms.MyToolStripButton();
            this.bindingNavigatorMoveNextItem = new Feng.Windows.Forms.MyToolStripButton();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMovePreviousItem = new Feng.Windows.Forms.MyToolStripButton();
            this.bindingNavigatorMoveFirstItem = new Feng.Windows.Forms.MyToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageNavigator1)).BeginInit();
            this.pageNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.tsbSearch,
            this.tsbFind,
            this.tsbFilter,
            this.toolStripSeparator4,
            this.tsbSetup,
            this.toolStripSeparator2,
            this.tsbOk});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(882, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbRefresh.Text = "刷新数据";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbSearch
            // 
            this.tsbSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Size = new System.Drawing.Size(23, 22);
            this.tsbSearch.Text = "搜索";
            this.tsbSearch.Click += new System.EventHandler(this.tsbSearch_Click);
            // 
            // tsbFind
            // 
            this.tsbFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Size = new System.Drawing.Size(23, 22);
            this.tsbFind.Text = "查找";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // tsbFilter
            // 
            this.tsbFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFilter.Name = "tsbFilter";
            this.tsbFilter.Size = new System.Drawing.Size(23, 22);
            this.tsbFilter.Text = "筛选";
            this.tsbFilter.Click += new System.EventHandler(this.tsbFilter_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSetup
            // 
            this.tsbSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSetup.Name = "tsbSetup";
            this.tsbSetup.Size = new System.Drawing.Size(23, 22);
            this.tsbSetup.Text = "设置(&O)";
            this.tsbSetup.Click += new System.EventHandler(this.tsbSetup_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbOk
            // 
            this.tsbOk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOk.Name = "tsbOk";
            this.tsbOk.Size = new System.Drawing.Size(23, 22);
            this.tsbOk.Text = "确定";
            this.tsbOk.Click += new System.EventHandler(this.tsbOk_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmBasicOperation});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(882, 25);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmBasicOperation
            // 
            this.tsmBasicOperation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRefresh,
            this.toolStripSeparator3,
            this.tsmSearch,
            this.tsmFind,
            this.tsmFilter,
            this.toolStripSeparator6});
            this.tsmBasicOperation.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.tsmBasicOperation.MergeIndex = 2;
            this.tsmBasicOperation.Name = "tsmBasicOperation";
            this.tsmBasicOperation.Size = new System.Drawing.Size(60, 21);
            this.tsmBasicOperation.Text = "查看(&V)";
            // 
            // tsmRefresh
            // 
            this.tsmRefresh.Name = "tsmRefresh";
            this.tsmRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.tsmRefresh.Size = new System.Drawing.Size(185, 22);
            this.tsmRefresh.Text = "刷新数据(&R)";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
            // 
            // tsmSearch
            // 
            this.tsmSearch.Name = "tsmSearch";
            this.tsmSearch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmSearch.Size = new System.Drawing.Size(185, 22);
            this.tsmSearch.Text = "搜索(&S)";
            // 
            // tsmFind
            // 
            this.tsmFind.Name = "tsmFind";
            this.tsmFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.tsmFind.Size = new System.Drawing.Size(185, 22);
            this.tsmFind.Text = "查找(&F)";
            // 
            // tsmFilter
            // 
            this.tsmFilter.Name = "tsmFilter";
            this.tsmFilter.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.F)));
            this.tsmFilter.Size = new System.Drawing.Size(185, 22);
            this.tsmFilter.Text = "筛选(&I)";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(182, 6);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 50);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(882, 397);
            this.splitContainer1.SplitterDistance = 636;
            this.splitContainer1.TabIndex = 7;
            // 
            // pageNavigator1
            // 
            this.pageNavigator1.AddNewItem = null;
            this.pageNavigator1.AllCountItem = this.bindingNavigatorAllCountItem;
            this.pageNavigator1.BindingSource = null;
            this.pageNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.pageNavigator1.CountItemFormat = "/{0}页";
            this.pageNavigator1.DeleteItem = null;
            this.pageNavigator1.Dock = System.Windows.Forms.DockStyle.None;
            this.pageNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPageNameItem,
            this.bindingNavigatorPageCountItem,
            this.bindingNavigatorAllCountItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorMoveFirstItem});
            this.pageNavigator1.Location = new System.Drawing.Point(527, 25);
            this.pageNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.pageNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.pageNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.pageNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.pageNavigator1.Name = "pageNavigator1";
            this.pageNavigator1.PageCountItem = this.bindingNavigatorPageCountItem;
            this.pageNavigator1.PageNameItem = this.bindingNavigatorPageNameItem;
            this.pageNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.pageNavigator1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.pageNavigator1.Size = new System.Drawing.Size(324, 25);
            this.pageNavigator1.TabIndex = 8;
            this.pageNavigator1.Text = "pageNavigator1";
            // 
            // bindingNavigatorAllCountItem
            // 
            this.bindingNavigatorAllCountItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorAllCountItem.Name = "bindingNavigatorAllCountItem";
            this.bindingNavigatorAllCountItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigatorAllCountItem.Size = new System.Drawing.Size(68, 22);
            this.bindingNavigatorAllCountItem.Text = "共0条/每页";
            this.bindingNavigatorAllCountItem.ToolTipText = "总条数";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(40, 22);
            this.bindingNavigatorCountItem.Text = "/{0}页";
            this.bindingNavigatorCountItem.ToolTipText = "总页数";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPageNameItem
            // 
            this.bindingNavigatorPageNameItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorPageNameItem.Name = "bindingNavigatorPageNameItem";
            this.bindingNavigatorPageNameItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigatorPageNameItem.Size = new System.Drawing.Size(20, 22);
            this.bindingNavigatorPageNameItem.Text = "条";
            this.bindingNavigatorPageNameItem.ToolTipText = "总条数";
            // 
            // bindingNavigatorPageCountItem
            // 
            this.bindingNavigatorPageCountItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorPageCountItem.Name = "bindingNavigatorPageCountItem";
            this.bindingNavigatorPageCountItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigatorPageCountItem.Size = new System.Drawing.Size(40, 25);
            this.bindingNavigatorPageCountItem.Text = "25";
            this.bindingNavigatorPageCountItem.ToolTipText = "每页条数";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "移到最后一条记录";
            this.bindingNavigatorMoveLastItem.ToolTipText = "移到最后一页";
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "移到下一条记录";
            this.bindingNavigatorMoveNextItem.ToolTipText = "移到下一页";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "位置";
            this.bindingNavigatorPositionItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(30, 25);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "当前位置";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "移到上一条记录";
            this.bindingNavigatorMovePreviousItem.ToolTipText = "移到上一页";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "移到第一条记录";
            this.bindingNavigatorMoveFirstItem.ToolTipText = "移到第一页";
            // 
            // ArchiveCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 447);
            this.Controls.Add(this.pageNavigator1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ArchiveCheckForm";
            this.Text = "ArchiveCheckForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pageNavigator1)).EndInit();
            this.pageNavigator1.ResumeLayout(false);
            this.pageNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private Feng.Windows.Forms.MyToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private Feng.Windows.Forms.MyToolStripButton tsbSearch;
        private Feng.Windows.Forms.MyToolStripButton tsbFind;
        private Feng.Windows.Forms.MyToolStripButton tsbFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmBasicOperation;
        private System.Windows.Forms.ToolStripMenuItem tsmRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmSearch;
        private System.Windows.Forms.ToolStripMenuItem tsmFind;
        private System.Windows.Forms.ToolStripMenuItem tsmFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private Feng.Windows.Forms.MyToolStripButton tsbOk;
        private Feng.Windows.Forms.MyToolStripButton tsbSetup;
        private Feng.Windows.Forms.MySplitContainer splitContainer1;
        private PageNavigator pageNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorAllCountItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorPageNameItem;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPageCountItem;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMoveLastItem;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMovePreviousItem;
        private Feng.Windows.Forms.MyToolStripButton bindingNavigatorMoveFirstItem;
    }
}