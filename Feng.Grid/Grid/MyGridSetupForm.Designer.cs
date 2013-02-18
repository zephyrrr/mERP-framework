namespace Feng.Grid
{
    partial class MyGridSetupForm
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
            this.btnCancel = new Feng.Windows.Forms.MyButton();
            this.btnOk = new Feng.Windows.Forms.MyButton();
            this.grdGridColumns = new Feng.Grid.MyGrid();
            this.dataRowTemplate1 = new Xceed.Grid.DataRow();
            this.columnManagerRow1 = new Xceed.Grid.ColumnManagerRow();
            this.btnResetGrid = new Feng.Windows.Forms.MyButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdGridColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(148, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 40);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(32, 263);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(79, 40);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grdGridColumns
            // 
            this.grdGridColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdGridColumns.DataRowTemplate = this.dataRowTemplate1;
            this.grdGridColumns.FixedHeaderRows.Add(this.columnManagerRow1);
            this.grdGridColumns.Location = new System.Drawing.Point(12, 12);
            this.grdGridColumns.Name = "grdGridColumns";
            this.grdGridColumns.Size = new System.Drawing.Size(357, 245);
            this.grdGridColumns.TabIndex = 6;
            // 
            // btnResetGrid
            // 
            this.btnResetGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetGrid.Location = new System.Drawing.Point(262, 263);
            this.btnResetGrid.Name = "btnResetGrid";
            this.btnResetGrid.Size = new System.Drawing.Size(79, 40);
            this.btnResetGrid.TabIndex = 7;
            this.btnResetGrid.Text = "重置";
            this.btnResetGrid.UseVisualStyleBackColor = true;
            this.btnResetGrid.Click += new System.EventHandler(this.btnResetGrid_Click);
            // 
            // MyGridSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 312);
            this.Controls.Add(this.grdGridColumns);
            this.Controls.Add(this.btnResetGrid);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MyGridSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.MyGridSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdGridColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Feng.Windows.Forms.MyButton btnCancel;
        private Feng.Windows.Forms.MyButton btnOk;
        private MyGrid grdGridColumns;
        private Xceed.Grid.DataRow dataRowTemplate1;
        private Xceed.Grid.ColumnManagerRow columnManagerRow1;
        private Feng.Windows.Forms.MyButton btnResetGrid;
    }
}