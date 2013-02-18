namespace Feng.Grid.Print
{
    partial class MyGridExportSaveFileDialog
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
            this.ckbIncludeDetailGrid = new Feng.Windows.Forms.MyCheckBox();
            this.SuspendLayout();
            // 
            // ckbIncludeDetailGrid
            // 
            this.ckbIncludeDetailGrid.Location = new System.Drawing.Point(25, 19);
            this.ckbIncludeDetailGrid.Name = "ckbIncludeDetailGrid";
            this.ckbIncludeDetailGrid.ReadOnly = false;
            this.ckbIncludeDetailGrid.Size = new System.Drawing.Size(90, 17);
            this.ckbIncludeDetailGrid.TabIndex = 0;
            this.ckbIncludeDetailGrid.Text = "包含子表格";
            this.ckbIncludeDetailGrid.UseVisualStyleBackColor = true;
            this.ckbIncludeDetailGrid.CheckedChanged += new System.EventHandler(this.ckbIncludeDetailGrid_CheckedChanged);
            // 
            // MyGridExportSaveFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ckbIncludeDetailGrid);
            this.FileDlgStartLocation = Feng.Windows.Forms.AddonWindowLocation.Bottom;
            this.Name = "MyGridExportSaveFileDialog";
            this.Size = new System.Drawing.Size(255, 56);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Feng.Windows.Forms.MyCheckBox ckbIncludeDetailGrid;
    }
}