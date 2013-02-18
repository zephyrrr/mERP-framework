namespace Feng.Windows.Forms
{
    partial class SearchControlContainer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        
        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmPresetLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLoadLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmResetSearchControls = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmPresetLayout,
            this.tsmLoadLayout,
            this.tsmSaveLayout,
            this.tsmResetSearchControls,
            this.toolStripSeparator1,
            this.tsmSetup});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 142);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // tsmPresetLayout
            // 
            this.tsmPresetLayout.Name = "tsmPresetLayout";
            this.tsmPresetLayout.Size = new System.Drawing.Size(152, 22);
            this.tsmPresetLayout.Text = "预定义设置";
            // 
            // tsmLoadLayout
            // 
            this.tsmLoadLayout.Name = "tsmLoadLayout";
            this.tsmLoadLayout.Size = new System.Drawing.Size(152, 22);
            this.tsmLoadLayout.Text = "读取设置";
            this.tsmLoadLayout.Click += new System.EventHandler(this.tsmLoadLayout_Click);
            // 
            // tsmSaveLayout
            // 
            this.tsmSaveLayout.Name = "tsmSaveLayout";
            this.tsmSaveLayout.Size = new System.Drawing.Size(152, 22);
            this.tsmSaveLayout.Text = "保存设置";
            this.tsmSaveLayout.Click += new System.EventHandler(this.tsmSaveLayout_Click);
            // 
            // tsmResetSearchControls
            // 
            this.tsmResetSearchControls.Name = "tsmResetSearchControls";
            this.tsmResetSearchControls.Size = new System.Drawing.Size(152, 22);
            this.tsmResetSearchControls.Text = "复位";
            this.tsmResetSearchControls.Click += new System.EventHandler(this.tsmResetSearchControls_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmSetup
            // 
            this.tsmSetup.Name = "tsmSetup";
            this.tsmSetup.Size = new System.Drawing.Size(152, 22);
            this.tsmSetup.Text = "设置";
            this.tsmSetup.Click += new System.EventHandler(this.tsmSetup_Click);
            // 
            // SearchControlContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScroll = true;
            this.ContextMenuStrip = this.contextMenuStrip2;
            this.DoubleBuffered = true;
            this.Name = "SearchControlContainer";
            this.Size = new System.Drawing.Size(220, 522);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tsmPresetLayout;
        private System.Windows.Forms.ToolStripMenuItem tsmLoadLayout;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveLayout;
        private System.Windows.Forms.ToolStripMenuItem tsmResetSearchControls;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmSetup;

    }
}
