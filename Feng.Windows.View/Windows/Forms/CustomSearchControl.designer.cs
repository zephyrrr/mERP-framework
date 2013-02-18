namespace Feng.Windows.Forms
{
	partial class CustomSearchControl
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
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsm隐藏 = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCaption = new Feng.Windows.Forms.MyLabel();
            this.ContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm隐藏});
            this.ContextMenuStrip1.Name = "tsm隐藏";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // tsm隐藏
            // 
            this.tsm隐藏.Name = "tsm隐藏";
            this.tsm隐藏.Size = new System.Drawing.Size(100, 22);
            this.tsm隐藏.Text = "隐藏";
            this.tsm隐藏.Click += new System.EventHandler(this.tsm隐藏_Click);
            // 
            // lblCaption
            // 
            this.lblCaption.ContextMenuStrip = this.ContextMenuStrip1;
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.ReadOnly = true;
            this.lblCaption.Size = new System.Drawing.Size(60, 24);
            this.lblCaption.TabIndex = 4;
            this.lblCaption.Text = "状态";
            // 
            // CustomSearchControl
            // 
            this.Controls.Add(this.lblCaption);
            this.Name = "CustomSearchControl";
            this.Size = new System.Drawing.Size(172, 24);
            this.ContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm隐藏;
        private MyLabel lblCaption;
	}
}
