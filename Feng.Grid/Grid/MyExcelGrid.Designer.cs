namespace Feng.Grid
{
    partial class MyExcelGrid
    {
        //<summary>
        //必需的设计器变量。
        //</summary>
        private System.ComponentModel.IContainer components = null;

        //<summary>
        //清理所有正在使用的资源。
        //</summary>
        //<param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        //<summary>
        //设计器支持所需的方法 - 不要
        //使用代码编辑器修改此方法的内容。
        //</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAddNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSetRowsInColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCut,
            this.tsmCopy,
            this.tsmPaste,
            this.tsmSetRowsInColumn,
            this.toolStripSeparator1,
            this.tsmInsert,
            this.tsmDelete,
            this.tsmAddNew});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 142);
            // 
            // tsmCut
            // 
            this.tsmCut.Name = "tsmCut";
            this.tsmCut.Size = new System.Drawing.Size(117, 22);
            this.tsmCut.Text = "剪切(&T)";
            this.tsmCut.Click += new System.EventHandler(this.tsmCut_Click);
            // 
            // tsmCopy
            // 
            this.tsmCopy.Name = "tsmCopy";
            this.tsmCopy.Size = new System.Drawing.Size(117, 22);
            this.tsmCopy.Text = "复制(&C)";
            this.tsmCopy.Click += new System.EventHandler(this.tsmCopy_Click);
            // 
            // tsmPaste
            // 
            this.tsmPaste.Name = "tsmPaste";
            this.tsmPaste.Size = new System.Drawing.Size(117, 22);
            this.tsmPaste.Text = "粘贴(&P)";
            this.tsmPaste.Click += new System.EventHandler(this.tsmPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(114, 6);
            // 
            // tsmInsert
            // 
            this.tsmInsert.Name = "tsmInsert";
            this.tsmInsert.Size = new System.Drawing.Size(117, 22);
            this.tsmInsert.Text = "插入(&I)";
            this.tsmInsert.Click += new System.EventHandler(this.tsmInsert_Click);
            // 
            // tsmDelete
            // 
            this.tsmDelete.Name = "tsmDelete";
            this.tsmDelete.Size = new System.Drawing.Size(117, 22);
            this.tsmDelete.Text = "删除(&D)";
            this.tsmDelete.Click += new System.EventHandler(this.tsmDelete_Click);
            // 
            // tsmAddNew
            // 
            this.tsmAddNew.Name = "tsmAddNew";
            this.tsmAddNew.Size = new System.Drawing.Size(117, 22);
            this.tsmAddNew.Text = "新增(&A)";
            this.tsmAddNew.Click += new System.EventHandler(tsmAddNew_Click);
            // 
            // tsmSetRowsInColumn
            // 
            this.tsmSetRowsInColumn.Name = "tsmSetRowsInColumn";
            this.tsmSetRowsInColumn.Size = new System.Drawing.Size(117, 22);
            this.tsmSetRowsInColumn.Text = "默认本列(&B)";
            this.tsmSetRowsInColumn.Click += new System.EventHandler(tsmSetRowsInColumn_Click);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmCut;
        private System.Windows.Forms.ToolStripMenuItem tsmCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmInsert;
        private System.Windows.Forms.ToolStripMenuItem tsmDelete;
        private System.Windows.Forms.ToolStripMenuItem tsmAddNew;
        private System.Windows.Forms.ToolStripMenuItem tsmSetRowsInColumn;
    }
}
