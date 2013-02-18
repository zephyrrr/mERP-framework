namespace Feng.Windows.Forms
{
    partial class ArchiveSearchForm
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.历史1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.历史2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.历史3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.历史4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.历史5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.分页ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下一页NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上一页PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.第一页FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.最后一页LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new Feng.Windows.Forms.MyTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelSearchControlNormal = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelSearchControlHidden = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.historyPane = new Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane(this.components);
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.customSearchPane = new Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane(this.components);
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.ckbUseHql = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchOrder = new System.Windows.Forms.TextBox();
            this.txtSearchExpression = new System.Windows.Forms.TextBox();
            this.lblResult = new Feng.Windows.Forms.MyLabel();
            this.btnSearch = new Feng.Windows.Forms.MyButton();
            this.searchControlContainer1 = new Feng.Windows.Forms.SearchControlContainer();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historyPane)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customSearchPane)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCopy});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // tsmCopy
            // 
            this.tsmCopy.Name = "tsmCopy";
            this.tsmCopy.Size = new System.Drawing.Size(100, 22);
            this.tsmCopy.Text = "复制(&C)";
            this.tsmCopy.Click += new System.EventHandler(this.tsmCopyHistory_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.历史1ToolStripMenuItem,
            this.历史2ToolStripMenuItem,
            this.历史3ToolStripMenuItem,
            this.历史4ToolStripMenuItem,
            this.历史5ToolStripMenuItem,
            this.toolStripSeparator1,
            this.分页ToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip1";
            this.contextMenuStrip3.Size = new System.Drawing.Size(108, 142);
            // 
            // 历史1ToolStripMenuItem
            // 
            this.历史1ToolStripMenuItem.Name = "历史1ToolStripMenuItem";
            this.历史1ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.历史1ToolStripMenuItem.Text = "历史1";
            // 
            // 历史2ToolStripMenuItem
            // 
            this.历史2ToolStripMenuItem.Name = "历史2ToolStripMenuItem";
            this.历史2ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.历史2ToolStripMenuItem.Text = "历史2";
            // 
            // 历史3ToolStripMenuItem
            // 
            this.历史3ToolStripMenuItem.Name = "历史3ToolStripMenuItem";
            this.历史3ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.历史3ToolStripMenuItem.Text = "历史3";
            // 
            // 历史4ToolStripMenuItem
            // 
            this.历史4ToolStripMenuItem.Name = "历史4ToolStripMenuItem";
            this.历史4ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.历史4ToolStripMenuItem.Text = "历史4";
            // 
            // 历史5ToolStripMenuItem
            // 
            this.历史5ToolStripMenuItem.Name = "历史5ToolStripMenuItem";
            this.历史5ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.历史5ToolStripMenuItem.Text = "历史5";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(104, 6);
            // 
            // 分页ToolStripMenuItem
            // 
            this.分页ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.下一页NToolStripMenuItem,
            this.上一页PToolStripMenuItem,
            this.第一页FToolStripMenuItem,
            this.最后一页LToolStripMenuItem,
            this.设置SToolStripMenuItem});
            this.分页ToolStripMenuItem.Name = "分页ToolStripMenuItem";
            this.分页ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.分页ToolStripMenuItem.Text = "分页";
            // 
            // 下一页NToolStripMenuItem
            // 
            this.下一页NToolStripMenuItem.Name = "下一页NToolStripMenuItem";
            this.下一页NToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.下一页NToolStripMenuItem.Text = "下一页(&N)";
            // 
            // 上一页PToolStripMenuItem
            // 
            this.上一页PToolStripMenuItem.Name = "上一页PToolStripMenuItem";
            this.上一页PToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.上一页PToolStripMenuItem.Text = "上一页(&P)";
            // 
            // 第一页FToolStripMenuItem
            // 
            this.第一页FToolStripMenuItem.Name = "第一页FToolStripMenuItem";
            this.第一页FToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.第一页FToolStripMenuItem.Text = "第一页(&F)";
            // 
            // 最后一页LToolStripMenuItem
            // 
            this.最后一页LToolStripMenuItem.Name = "最后一页LToolStripMenuItem";
            this.最后一页LToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.最后一页LToolStripMenuItem.Text = "最后一页(&L)";
            // 
            // 设置SToolStripMenuItem
            // 
            this.设置SToolStripMenuItem.Name = "设置SToolStripMenuItem";
            this.设置SToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.设置SToolStripMenuItem.Text = "设置(&S)";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.ReadOnly = false;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(229, 488);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Controls.Add(this.searchControlContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(221, 462);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "搜索";
            // 
            // tabControl2
            // 
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(215, 456);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage5.Controls.Add(this.flowLayoutPanelSearchControlNormal);
            this.tabPage5.Location = new System.Drawing.Point(4, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(207, 430);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "常用";
            // 
            // flowLayoutPanelSearchControlNormal
            // 
            this.flowLayoutPanelSearchControlNormal.AutoScroll = true;
            this.flowLayoutPanelSearchControlNormal.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanelSearchControlNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelSearchControlNormal.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelSearchControlNormal.Name = "flowLayoutPanelSearchControlNormal";
            this.flowLayoutPanelSearchControlNormal.Size = new System.Drawing.Size(201, 424);
            this.flowLayoutPanelSearchControlNormal.TabIndex = 170;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage6.Controls.Add(this.flowLayoutPanelSearchControlHidden);
            this.tabPage6.Location = new System.Drawing.Point(4, 4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(207, 430);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "隐藏";
            // 
            // flowLayoutPanelSearchControlHidden
            // 
            this.flowLayoutPanelSearchControlHidden.AutoScroll = true;
            this.flowLayoutPanelSearchControlHidden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelSearchControlHidden.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelSearchControlHidden.Name = "flowLayoutPanelSearchControlHidden";
            this.flowLayoutPanelSearchControlHidden.Size = new System.Drawing.Size(201, 424);
            this.flowLayoutPanelSearchControlHidden.TabIndex = 171;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.historyPane);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(221, 462);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "历史";
            // 
            // historyPane
            // 
            this.historyPane.BackColor = System.Drawing.SystemColors.Control;
            this.historyPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyPane.Location = new System.Drawing.Point(3, 3);
            this.historyPane.Name = "historyPane";
            this.historyPane.Size = new System.Drawing.Size(215, 456);
            this.historyPane.TabIndex = 1;
            this.historyPane.Text = "smartOfficeTaskPane1";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.customSearchPane);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(221, 462);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "预定义";
            // 
            // customSearchPane
            // 
            this.customSearchPane.BackColor = System.Drawing.SystemColors.Control;
            this.customSearchPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customSearchPane.Location = new System.Drawing.Point(3, 3);
            this.customSearchPane.Name = "customSearchPane";
            this.customSearchPane.Size = new System.Drawing.Size(215, 456);
            this.customSearchPane.TabIndex = 0;
            this.customSearchPane.Text = "smartOfficeTaskPane2";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.ckbUseHql);
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.txtSearchOrder);
            this.tabPage4.Controls.Add(this.txtSearchExpression);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(221, 462);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "复杂";
            // 
            // ckbUseHql
            // 
            this.ckbUseHql.AutoSize = true;
            this.ckbUseHql.Location = new System.Drawing.Point(104, 16);
            this.ckbUseHql.Name = "ckbUseHql";
            this.ckbUseHql.Size = new System.Drawing.Size(72, 16);
            this.ckbUseHql.TabIndex = 4;
            this.ckbUseHql.Text = "HQL(SQL)";
            this.ckbUseHql.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "查询排序";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "查询条件";
            // 
            // txtSearchOrder
            // 
            this.txtSearchOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchOrder.Location = new System.Drawing.Point(6, 303);
            this.txtSearchOrder.Multiline = true;
            this.txtSearchOrder.Name = "txtSearchOrder";
            this.txtSearchOrder.Size = new System.Drawing.Size(209, 129);
            this.txtSearchOrder.TabIndex = 1;
            // 
            // txtSearchExpression
            // 
            this.txtSearchExpression.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchExpression.Location = new System.Drawing.Point(6, 32);
            this.txtSearchExpression.Multiline = true;
            this.txtSearchExpression.Name = "txtSearchExpression";
            this.txtSearchExpression.Size = new System.Drawing.Size(209, 223);
            this.txtSearchExpression.TabIndex = 0;
            // 
            // lblResult
            // 
            this.lblResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(93, 494);
            this.lblResult.Name = "lblResult";
            this.lblResult.ReadOnly = true;
            this.lblResult.Size = new System.Drawing.Size(113, 24);
            this.lblResult.TabIndex = 170;
            this.lblResult.Text = "共39630条 每页15条\r\n第1页/共2642页";
            this.lblResult.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.Location = new System.Drawing.Point(12, 494);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(56, 25);
            this.btnSearch.TabIndex = 169;
            this.btnSearch.Text = "搜索";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // searchControlContainer1
            // 
            this.searchControlContainer1.AutoScroll = true;
            this.searchControlContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchControlContainer1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.searchControlContainer1.Location = new System.Drawing.Point(3, 3);
            this.searchControlContainer1.Name = "searchControlContainer1";
            this.searchControlContainer1.Size = new System.Drawing.Size(215, 456);
            this.searchControlContainer1.TabIndex = 0;
            // 
            // ArchiveSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnSearch);
            this.Name = "ArchiveSearchForm";
            this.Size = new System.Drawing.Size(229, 522);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.historyPane)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customSearchPane)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Feng.Windows.Forms.MyTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane historyPane;
        private Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane customSearchPane;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSearchControlNormal;
        private SearchControlContainer searchControlContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmCopy;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchOrder;
        private System.Windows.Forms.TextBox txtSearchExpression;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem 历史1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 分页ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下一页NToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上一页PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 第一页FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最后一页LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置SToolStripMenuItem;
        private MyButton btnSearch;
        protected MyLabel lblResult;
        private System.Windows.Forms.CheckBox ckbUseHql;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSearchControlHidden;
    }
}