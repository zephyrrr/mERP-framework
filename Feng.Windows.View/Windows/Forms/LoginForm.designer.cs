namespace Feng.Windows.Forms
{
	partial class LoginForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.dataPictureBox1 = new System.Windows.Forms.PictureBox();
            this.grpDl = new System.Windows.Forms.GroupBox();
            this.ckbAutoLogin = new System.Windows.Forms.CheckBox();
            this.aspNetLoginControl1 = new Feng.Windows.Forms.LoginControl();
            this.ckbPwdRemember = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataPictureBox1)).BeginInit();
            this.grpDl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(22, 118);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(72, 21);
            this.btnSetup.TabIndex = 2;
            this.btnSetup.Text = "设置(&S)";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLogin.Location = new System.Drawing.Point(178, 118);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(72, 21);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "登录(&L)";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // dataPictureBox1
            // 
            this.dataPictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataPictureBox1.Location = new System.Drawing.Point(4, 11);
            this.dataPictureBox1.Name = "dataPictureBox1";
            this.dataPictureBox1.Size = new System.Drawing.Size(279, 66);
            this.dataPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.dataPictureBox1.TabIndex = 7;
            this.dataPictureBox1.TabStop = false;
            // 
            // grpDl
            // 
            this.grpDl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDl.Controls.Add(this.ckbAutoLogin);
            this.grpDl.Controls.Add(this.aspNetLoginControl1);
            this.grpDl.Controls.Add(this.btnSetup);
            this.grpDl.Controls.Add(this.ckbPwdRemember);
            this.grpDl.Controls.Add(this.btnLogin);
            this.grpDl.Location = new System.Drawing.Point(4, 83);
            this.grpDl.Name = "grpDl";
            this.grpDl.Size = new System.Drawing.Size(278, 151);
            this.grpDl.TabIndex = 8;
            this.grpDl.TabStop = false;
            // 
            // ckbAutoLogin
            // 
            this.ckbAutoLogin.AutoSize = true;
            this.ckbAutoLogin.Location = new System.Drawing.Point(178, 96);
            this.ckbAutoLogin.Name = "ckbAutoLogin";
            this.ckbAutoLogin.Size = new System.Drawing.Size(90, 16);
            this.ckbAutoLogin.TabIndex = 4;
            this.ckbAutoLogin.Text = "自动登录(&O)";
            this.ckbAutoLogin.UseVisualStyleBackColor = true;
            // 
            // aspNetLoginControl1
            // 
            this.aspNetLoginControl1.Location = new System.Drawing.Point(22, 10);
            this.aspNetLoginControl1.Name = "aspNetLoginControl1";
            this.aspNetLoginControl1.Password = "";
            this.aspNetLoginControl1.Size = new System.Drawing.Size(220, 86);
            this.aspNetLoginControl1.TabIndex = 0;
            this.aspNetLoginControl1.UserName = "";
            this.aspNetLoginControl1.LoginEvent += new System.EventHandler<Feng.LoginEventArgs>(this.LoginControl_LoginEvent);
            // 
            // ckbPwdRemember
            // 
            this.ckbPwdRemember.AutoSize = true;
            this.ckbPwdRemember.Location = new System.Drawing.Point(22, 96);
            this.ckbPwdRemember.Name = "ckbPwdRemember";
            this.ckbPwdRemember.Size = new System.Drawing.Size(90, 16);
            this.ckbPwdRemember.TabIndex = 3;
            this.ckbPwdRemember.Text = "记住密码(&R)";
            this.ckbPwdRemember.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 244);
            this.Controls.Add(this.dataPictureBox1);
            this.Controls.Add(this.grpDl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登录";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_Login_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataPictureBox1)).EndInit();
            this.grpDl.ResumeLayout(false);
            this.grpDl.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

		private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.PictureBox dataPictureBox1;
        private System.Windows.Forms.GroupBox grpDl;
        private System.Windows.Forms.CheckBox ckbAutoLogin;
        private LoginControl aspNetLoginControl1;
        private System.Windows.Forms.CheckBox ckbPwdRemember;
    }
}