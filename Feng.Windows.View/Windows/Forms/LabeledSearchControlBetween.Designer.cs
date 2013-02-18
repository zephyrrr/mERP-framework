namespace Feng.Windows.Forms
{
    partial class LabeledSearchControlBetween
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsm为空 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm取反 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsc顺序 = new System.Windows.Forms.ToolStripComboBox();
            this.tsm隐藏 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm为空,
            this.tsm取反,
            this.toolStripSeparator1,
            this.tsc顺序,
            this.tsm隐藏});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 122);
            // 
            // tsm为空
            // 
            this.tsm为空.Name = "tsm为空";
            this.tsm为空.Size = new System.Drawing.Size(152, 22);
            this.tsm为空.Text = "为空";
            this.tsm为空.Click += new System.EventHandler(this.tsm为空_Click);
            // 
            // tsm取反
            // 
            this.tsm取反.Name = "tsm取反";
            this.tsm取反.Size = new System.Drawing.Size(152, 22);
            this.tsm取反.Text = "取反";
            this.tsm取反.Click += new System.EventHandler(this.tsm取反_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsc顺序
            // 
            this.tsc顺序.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tsc顺序.Items.AddRange(new object[] {
            "无序",
            "升序",
            "逆序"});
            this.tsc顺序.Name = "tsc顺序";
            this.tsc顺序.Size = new System.Drawing.Size(75, 20);
            this.tsc顺序.Text = "无序";
            this.tsc顺序.SelectedIndexChanged += new System.EventHandler(this.tsc顺序_SelectedIndexChanged);
            // 
            // tsm隐藏
            // 
            this.tsm隐藏.Name = "tsm隐藏";
            this.tsm隐藏.Size = new System.Drawing.Size(152, 22);
            this.tsm隐藏.Text = "隐藏";
            this.tsm隐藏.Click += new System.EventHandler(this.tsm隐藏_Click);
            // 
            // LabeledFindControlBetween
            // 
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "LabeledFindControlBetween";
            this.Size = new System.Drawing.Size(172, 45);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm为空;
        private System.Windows.Forms.ToolStripMenuItem tsm取反;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox tsc顺序;
        private System.Windows.Forms.ToolStripMenuItem tsm隐藏;
    }
}
