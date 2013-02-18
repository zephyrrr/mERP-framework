namespace Feng.Windows.Forms
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
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdGridColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataRowTemplate1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnManagerRow1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnCancel.Location = new System.Drawing.Point(183, 263);
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
            this.btnOk.ImeMode = System.Windows.Forms.ImeMode.On;
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
            this.grdGridColumns.Size = new System.Drawing.Size(300, 250);
            this.grdGridColumns.TabIndex = 6;
            // 
            // btnResetGrid
            // 
            this.btnResetGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetGrid.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnResetGrid.Location = new System.Drawing.Point(340, 263);
            this.btnResetGrid.Name = "btnResetGrid";
            this.btnResetGrid.Size = new System.Drawing.Size(79, 40);
            this.btnResetGrid.TabIndex = 7;
            this.btnResetGrid.Text = "重置";
            this.btnResetGrid.UseVisualStyleBackColor = true;
            this.btnResetGrid.Click += new System.EventHandler(this.btnResetGrid_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDown.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnMoveDown.Location = new System.Drawing.Point(344, 157);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 14;
            this.btnMoveDown.Text = "下移(D)";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUp.ImeMode = System.Windows.Forms.ImeMode.On;
            this.btnMoveUp.Location = new System.Drawing.Point(344, 104);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 13;
            this.btnMoveUp.Text = "上移(U)";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // MyGridSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 312);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
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
        private Feng.Grid.MyGrid grdGridColumns;
        private Xceed.Grid.DataRow dataRowTemplate1;
        private Xceed.Grid.ColumnManagerRow columnManagerRow1;
        private Feng.Windows.Forms.MyButton btnResetGrid;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
    }
}