namespace Feng.Windows.Forms
{
	partial class MyFileBox
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnBrowser = new System.Windows.Forms.Button();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemModify = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnBrowser
			// 
			this.btnBrowser.ContextMenuStrip = this.contextMenuStrip1;
			this.btnBrowser.Location = new System.Drawing.Point(0, 0);
			this.btnBrowser.Name = "btnBrowser";
			this.btnBrowser.Size = new System.Drawing.Size(50, 21);
			this.btnBrowser.TabIndex = 0;
			this.btnBrowser.Text = "浏览";
			this.btnBrowser.UseVisualStyleBackColor = true;
			this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemModify,
            this.toolStripMenuItemClear,
            this.toolStripMenuItemSave,
            this.toolStripMenuItemOpen});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(95, 70);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// toolStripMenuItemModify
			// 
			this.toolStripMenuItemModify.Name = "toolStripMenuItemModify";
			this.toolStripMenuItemModify.Size = new System.Drawing.Size(94, 22);
			this.toolStripMenuItemModify.Text = "修改";
			this.toolStripMenuItemModify.Click += new System.EventHandler(this.toolStripMenuItemModify_Click);
			// 
			// toolStripMenuItemClear
			// 
			this.toolStripMenuItemClear.Name = "toolStripMenuItemClear";
			this.toolStripMenuItemClear.Size = new System.Drawing.Size(94, 22);
			this.toolStripMenuItemClear.Text = "清空";
			this.toolStripMenuItemClear.Click += new System.EventHandler(this.toolStripMenuItemClear_Click);
			// 
			// toolStripMenuItemSave
			// 
			this.toolStripMenuItemSave.Name = "toolStripMenuItemSave";
			this.toolStripMenuItemSave.Size = new System.Drawing.Size(94, 22);
			this.toolStripMenuItemSave.Text = "保存";
			this.toolStripMenuItemSave.Click += new System.EventHandler(this.toolStripMenuItemSave_Click);
            // 
            // toolStripMenuItemOpen
            // 
            this.toolStripMenuItemOpen.Name = "toolStripMenuItemOpen";
            this.toolStripMenuItemOpen.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItemOpen.Text = "打开";
            this.toolStripMenuItemOpen.Click += new System.EventHandler(this.toolStripMenuItemOpen_Click);
			// 
			// MyFileBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnBrowser);
			this.Name = "MyFileBox";
			this.Size = new System.Drawing.Size(50, 21);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnBrowser;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemModify;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClear;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpen;
	}
}
