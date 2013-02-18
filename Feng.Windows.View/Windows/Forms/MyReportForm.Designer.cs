namespace Feng.Windows.Forms
{
    partial class MyReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crystalReportViewer1 = new Feng.Windows.Forms.MyCrystalReportViewer();
            this.lblHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.Size = new System.Drawing.Size(786, 441);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.TemplateDataSet = null;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // lblHint
            // 
            this.lblHint.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHint.ForeColor = System.Drawing.Color.Blue;
            this.lblHint.Location = new System.Drawing.Point(360, 168);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(301, 80);
            this.lblHint.TabIndex = 12;
            this.lblHint.Text = "正在加载报表模板，请稍候.....";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHint.Visible = false;
            // 
            // MyReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 441);
            this.Controls.Add(this.crystalReportViewer1);
            this.Name = "MyReportForm";
            this.Text = "报表打印预览";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MyReportForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Feng.Windows.Forms.MyCrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.Label lblHint;
    }
}