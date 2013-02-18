namespace Feng.Windows.Forms
{
    partial class ArchiveSetupForm
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
            this.myTabControl1 = new Feng.Windows.Forms.MyTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdGridColumns = new Feng.Grid.MyGrid();
            this.dataRowTemplate1 = new Xceed.Grid.DataRow();
            this.columnManagerRow1 = new Xceed.Grid.ColumnManagerRow();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grdSearchControls = new Feng.Grid.MyGrid();
            this.dataRowTemplate2 = new Xceed.Grid.DataRow();
            this.columnManagerRow2 = new Xceed.Grid.ColumnManagerRow();
            this.btnResetGrid = new Feng.Windows.Forms.MyButton();
            this.btnCancel = new Feng.Windows.Forms.MyButton();
            this.btnOk = new Feng.Windows.Forms.MyButton();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.myTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGridColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearchControls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow2)).BeginInit();
            this.SuspendLayout();
            // 
            // myTabControl1
            // 
            this.myTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.myTabControl1.Controls.Add(this.tabPage1);
            this.myTabControl1.Controls.Add(this.tabPage2);
            this.myTabControl1.Location = new System.Drawing.Point(0, 0);
            this.myTabControl1.Name = "myTabControl1";
            this.myTabControl1.ReadOnly = false;
            this.myTabControl1.SelectedIndex = 0;
            this.myTabControl1.Size = new System.Drawing.Size(368, 296);
            this.myTabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdGridColumns);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(426, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "表格";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdGridColumns
            // 
            this.grdGridColumns.DataRowTemplate = this.dataRowTemplate1;
            this.grdGridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGridColumns.FixedHeaderRows.Add(this.columnManagerRow1);
            this.grdGridColumns.Location = new System.Drawing.Point(3, 3);
            this.grdGridColumns.Name = "grdGridColumns";
            this.grdGridColumns.Size = new System.Drawing.Size(420, 264);
            this.grdGridColumns.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grdSearchControls);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(360, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "搜索栏";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grdSearchControls
            // 
            this.grdSearchControls.DataRowTemplate = this.dataRowTemplate2;
            this.grdSearchControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSearchControls.FixedHeaderRows.Add(this.columnManagerRow2);
            this.grdSearchControls.Location = new System.Drawing.Point(3, 3);
            this.grdSearchControls.Name = "grdSearchControls";
            this.grdSearchControls.Size = new System.Drawing.Size(354, 264);
            this.grdSearchControls.TabIndex = 1;
            // 
            // btnResetGrid
            // 
            this.btnResetGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetGrid.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnResetGrid.Location = new System.Drawing.Point(273, 313);
            this.btnResetGrid.Name = "btnResetGrid";
            this.btnResetGrid.Size = new System.Drawing.Size(79, 40);
            this.btnResetGrid.TabIndex = 10;
            this.btnResetGrid.Text = "重置(&R)";
            this.btnResetGrid.UseVisualStyleBackColor = true;
            this.btnResetGrid.Click += new System.EventHandler(this.btnResetGrid_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnCancel.Location = new System.Drawing.Point(162, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 40);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消(C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnOk.Location = new System.Drawing.Point(52, 313);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(79, 40);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUp.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnMoveUp.Location = new System.Drawing.Point(374, 102);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 11;
            this.btnMoveUp.Text = "上移(U)";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDown.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnMoveDown.Location = new System.Drawing.Point(374, 155);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 12;
            this.btnMoveDown.Text = "下移(D)";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // ArchiveSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 365);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnResetGrid);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.myTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ArchiveSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.myTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGridColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSearchControls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MyTabControl myTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Feng.Grid.MyGrid grdGridColumns;
        private Xceed.Grid.DataRow dataRowTemplate1;
        private Xceed.Grid.ColumnManagerRow columnManagerRow1;
        private System.Windows.Forms.TabPage tabPage2;
        private Feng.Grid.MyGrid grdSearchControls;
        private Xceed.Grid.DataRow dataRowTemplate2;
        private Xceed.Grid.ColumnManagerRow columnManagerRow2;
        private MyButton btnResetGrid;
        private MyButton btnCancel;
        private MyButton btnOk;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
    }
}