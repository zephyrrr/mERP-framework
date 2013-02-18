namespace Feng.Map
{
    partial class MapForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUseSatellite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmReadGpsData = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm显示路线点 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm显示理论路径 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm显示重要地点 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm显示路线点地址 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem,
            this.tsmUseSatellite,
            this.tsmReadGpsData,
            this.tsm显示路线点,
            this.tsm显示理论路径,
            this.tsm显示重要地点,
            this.tsm显示路线点地址});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 180);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // tsmUseSatellite
            // 
            this.tsmUseSatellite.CheckOnClick = true;
            this.tsmUseSatellite.Name = "tsmUseSatellite";
            this.tsmUseSatellite.Size = new System.Drawing.Size(154, 22);
            this.tsmUseSatellite.Text = "卫星图";
            this.tsmUseSatellite.Click += new System.EventHandler(this.tsmUseSatellite_Click);
            // 
            // tsmReadGpsData
            // 
            this.tsmReadGpsData.Name = "tsmReadGpsData";
            this.tsmReadGpsData.Size = new System.Drawing.Size(154, 22);
            this.tsmReadGpsData.Text = "读入GPS数据";
            this.tsmReadGpsData.Click += new System.EventHandler(this.tsmReadGpsData_Click);
            // 
            // tsm显示路线点
            // 
            this.tsm显示路线点.CheckOnClick = true;
            this.tsm显示路线点.Name = "tsm显示路线点";
            this.tsm显示路线点.Size = new System.Drawing.Size(154, 22);
            this.tsm显示路线点.Text = "显示路线点";
            this.tsm显示路线点.Click += new System.EventHandler(this.tsmShowGpsPoint_Click);
            // 
            // tsm显示理论路径
            // 
            this.tsm显示理论路径.CheckOnClick = true;
            this.tsm显示理论路径.Name = "tsm显示理论路径";
            this.tsm显示理论路径.Size = new System.Drawing.Size(154, 22);
            this.tsm显示理论路径.Text = "显示理论路径";
            // 
            // tsm显示重要地点
            // 
            this.tsm显示重要地点.CheckOnClick = true;
            this.tsm显示重要地点.Name = "tsm显示重要地点";
            this.tsm显示重要地点.Size = new System.Drawing.Size(154, 22);
            this.tsm显示重要地点.Text = "显示重要地点";
            // 
            // tsm显示路线点地址
            // 
            this.tsm显示路线点地址.CheckOnClick = true;
            this.tsm显示路线点地址.Name = "tsm显示路线点地址";
            this.tsm显示路线点地址.Size = new System.Drawing.Size(154, 22);
            this.tsm显示路线点地址.Text = "显示路线点地址";
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 449);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "MapForm";
            this.Text = "地图";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmUseSatellite;
        private System.Windows.Forms.ToolStripMenuItem tsmReadGpsData;
        private System.Windows.Forms.ToolStripMenuItem tsm显示路线点;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsm显示理论路径;
        private System.Windows.Forms.ToolStripMenuItem tsm显示重要地点;
        private System.Windows.Forms.ToolStripMenuItem tsm显示路线点地址;


    }
}

