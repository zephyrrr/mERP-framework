namespace Feng.Grid.Columns
{
    partial class CheckColumnHelper
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        ///// <summary>
        ///// 清理所有正在使用的资源。
        ///// </summary>
        ///// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsm全选 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm选择项选中 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsm全不选 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm当前组全不选 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm选择项全不选 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm当前组全选 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm全选,
            this.tsm当前组全选,
            this.tsm选择项选中,
            this.toolStripSeparator1,
            this.tsm全不选,
            this.tsm当前组全不选,
            this.tsm选择项全不选});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 142);
            // 
            // 全选ToolStripMenuItem
            // 
            this.tsm全选.Name = "全选ToolStripMenuItem";
            this.tsm全选.Size = new System.Drawing.Size(142, 22);
            this.tsm全选.Text = "全选";
            this.tsm全选.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 当前组全选ToolStripMenuItem
            // 
            this.tsm当前组全选.Name = "当前组全选ToolStripMenuItem";
            this.tsm当前组全选.Size = new System.Drawing.Size(142, 22);
            this.tsm当前组全选.Text = "当前组全选";
            this.tsm当前组全选.Click += new System.EventHandler(this.当前组全选ToolStripMenuItem_Click);
            // 
            // 选择项选中ToolStripMenuItem
            // 
            this.tsm选择项选中.Name = "选择项选中ToolStripMenuItem";
            this.tsm选择项选中.Size = new System.Drawing.Size(142, 22);
            this.tsm选择项选中.Text = "选择项选中";
            this.tsm选择项选中.Click += new System.EventHandler(this.选择项选中ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // 全不选ToolStripMenuItem
            // 
            this.tsm全不选.Name = "全不选ToolStripMenuItem";
            this.tsm全不选.Size = new System.Drawing.Size(142, 22);
            this.tsm全不选.Text = "清空";
            this.tsm全不选.Click += new System.EventHandler(this.全不选ToolStripMenuItem_Click);
            // 
            // tsm当前组全不选
            // 
            this.tsm当前组全不选.Name = "tsm当前组全不选";
            this.tsm当前组全不选.Size = new System.Drawing.Size(142, 22);
            this.tsm当前组全不选.Text = "当前组全不选";
            this.tsm当前组全不选.Click += new System.EventHandler(tsm当前组全不选_Click);
            // 
            // tsm选择项全不选
            // 
            this.tsm选择项全不选.Name = "tsm选择项不选";
            this.tsm选择项全不选.Size = new System.Drawing.Size(142, 22);
            this.tsm选择项全不选.Text = "选择项不选";
            this.tsm选择项全不选.Click += new System.EventHandler(tsm选择项全不选_Click);
            this.contextMenuStrip1.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm全选;
        private System.Windows.Forms.ToolStripMenuItem tsm全不选;
        private System.Windows.Forms.ToolStripMenuItem tsm选择项选中;
        private System.Windows.Forms.ToolStripMenuItem tsm当前组全选;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsm当前组全不选;
        private System.Windows.Forms.ToolStripMenuItem tsm选择项全不选;
    }
}
