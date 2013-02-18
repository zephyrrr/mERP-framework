namespace Feng.Grid
{
    partial class ArchiveGridHelper
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components;

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tsm默认本列 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDeleteBatch = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripForInsertionRowSelector = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStripForInsertionRowSelector.SuspendLayout();
            // 
            // tsm默认本列
            // 
            this.tsm默认本列.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.tsm默认本列.MergeIndex = 0;
            this.tsm默认本列.Name = "tsm默认本列";
            this.tsm默认本列.Size = new System.Drawing.Size(154, 22);
            this.tsm默认本列.Text = "默认本列(&B)";
            this.tsm默认本列.ToolTipText = "以当前行数据为准，更改选中行当前列的值为当前行当前列数据";
            this.tsm默认本列.Click += new System.EventHandler(this.tsm默认本列_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmDelete,
            this.tsmDeleteBatch,
            this.tsm默认本列});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 92);
            // 
            // tsmDelete
            // 
            this.tsmDelete.Name = "tsmDelete";
            this.tsmDelete.Size = new System.Drawing.Size(118, 22);
            this.tsmDelete.Text = "删除";
            this.tsmDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // tsmDeleteBatch
            // 
            this.tsmDeleteBatch.Name = "tsmDeleteBatch";
            this.tsmDeleteBatch.Size = new System.Drawing.Size(118, 22);
            this.tsmDeleteBatch.Text = "删除(&D)";
            this.tsmDeleteBatch.Click += new System.EventHandler(this.tsmDeleteBatch_Click);
            // 
            // contextMenuStripForInsertionRowSelector
            // 
            this.contextMenuStripForInsertionRowSelector.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmPaste});
            this.contextMenuStripForInsertionRowSelector.Name = "contextMenuStripForInsertionRowSelector";
            this.contextMenuStripForInsertionRowSelector.Size = new System.Drawing.Size(101, 26);
            this.contextMenuStripForInsertionRowSelector.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripForInsertionRowSelector_Opening);
            // 
            // tsmPaste
            // 
            this.tsmPaste.Name = "tsmPaste";
            this.tsmPaste.Size = new System.Drawing.Size(100, 22);
            this.tsmPaste.Text = "粘贴";
            this.tsmPaste.Click += new System.EventHandler(tsmPaste_Click);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStripForInsertionRowSelector.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmDeleteBatch;
        private System.Windows.Forms.ToolStripMenuItem tsm默认本列;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripForInsertionRowSelector;
        private System.Windows.Forms.ToolStripMenuItem tsmPaste;
    }
}
