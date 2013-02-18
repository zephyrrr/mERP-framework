namespace Feng.Windows.Forms
{
    partial class SearchWindowSetupForm
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
            this.txtMaxResult = new Feng.Windows.Forms.MyIntegerTextBox();
            this.myLabel1 = new Feng.Windows.Forms.MyLabel();
            this.btnOk = new Feng.Windows.Forms.MyButton();
            this.btnCancel = new Feng.Windows.Forms.MyButton();
            this.SuspendLayout();
            // 
            // txtMaxResult
            // 
            this.txtMaxResult.Location = new System.Drawing.Point(89, 25);
            this.txtMaxResult.Name = "txtMaxResult";
            this.txtMaxResult.Size = new System.Drawing.Size(88, 21);
            this.txtMaxResult.TabIndex = 0;
            this.txtMaxResult.Text = "50";
            // 
            // myLabel1
            // 
            this.myLabel1.AutoSize = true;
            this.myLabel1.Location = new System.Drawing.Point(13, 25);
            this.myLabel1.Name = "myLabel1";
            this.myLabel1.Size = new System.Drawing.Size(53, 12);
            this.myLabel1.TabIndex = 1;
            this.myLabel1.Text = "每页条数";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(12, 78);
            this.btnOk.Name = "btnOk";
            this.btnOk.ReadOnly = false;
            this.btnOk.Size = new System.Drawing.Size(72, 21);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(105, 78);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ReadOnly = false;
            this.btnCancel.Size = new System.Drawing.Size(72, 21);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FindToolWindowSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 114);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.myLabel1);
            this.Controls.Add(this.txtMaxResult);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FindToolWindowSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MyIntegerTextBox txtMaxResult;
        private MyLabel myLabel1;
        private MyButton btnOk;
        private MyButton btnCancel;

    }
}