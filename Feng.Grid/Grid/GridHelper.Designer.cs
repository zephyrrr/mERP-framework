namespace Feng.Grid
{
    partial class GridHelper
    {
        //<summary>
        //必需的设计器变量。
        //</summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows 窗体设计器生成的代码

        //<summary>
        //设计器支持所需的方法 - 不要
        //使用代码编辑器修改此方法的内容。
        //</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStripForCell = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmShowCellContent = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripForCell.SuspendLayout();
            // 
            // contextMenuStripForCell
            // 
            this.contextMenuStripForCell.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCopy,
            this.tsmShowCellContent});
            this.contextMenuStripForCell.Name = "contextMenuStripForCell";
            this.contextMenuStripForCell.Size = new System.Drawing.Size(117, 26);
            // 
            // tsmCopy
            // 
            this.tsmCopy.Name = "tsmCopy";
            this.tsmCopy.Size = new System.Drawing.Size(116, 22);
            this.tsmCopy.Text = "复制(&C)";
            this.tsmCopy.Click += new System.EventHandler(tsmCopy_Click);
            // 
            // tsmShowCellContent
            // 
            this.tsmShowCellContent.Name = "tsmShowCellContent";
            this.tsmShowCellContent.Size = new System.Drawing.Size(116, 22);
            this.tsmShowCellContent.Text = "显示详细";
            this.tsmShowCellContent.Click += new System.EventHandler(tsmShowCellContent_Click);
            this.contextMenuStripForCell.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStripForCell;
        private System.Windows.Forms.ToolStripMenuItem tsmCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmShowCellContent;
    }
}
