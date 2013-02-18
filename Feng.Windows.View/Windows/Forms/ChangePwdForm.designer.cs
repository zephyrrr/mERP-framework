namespace Feng.Windows.Forms
{
	partial class ChangePwdForm
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
            this.components = new System.ComponentModel.Container();
            this.m_UserNameLabel = new System.Windows.Forms.Label();
            this.m_PasswordLabel = new System.Windows.Forms.Label();
            this.m_PasswordBox = new System.Windows.Forms.TextBox();
            this.m_PasswordBox0 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_PasswordBox2 = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.m_ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // m_UserNameLabel
            // 
            this.m_UserNameLabel.AutoSize = true;
            this.m_UserNameLabel.Location = new System.Drawing.Point(37, 29);
            this.m_UserNameLabel.Name = "m_UserNameLabel";
            this.m_UserNameLabel.Size = new System.Drawing.Size(47, 16);
            this.m_UserNameLabel.TabIndex = 8;
            this.m_UserNameLabel.Text = "旧密码:";
            // 
            // m_PasswordLabel
            // 
            this.m_PasswordLabel.AutoSize = true;
            this.m_PasswordLabel.Location = new System.Drawing.Point(37, 87);
            this.m_PasswordLabel.Name = "m_PasswordLabel";
            this.m_PasswordLabel.Size = new System.Drawing.Size(47, 16);
            this.m_PasswordLabel.TabIndex = 7;
            this.m_PasswordLabel.Text = "新密码:";
            // 
            // m_PasswordBox
            // 
            this.m_PasswordBox.Location = new System.Drawing.Point(120, 83);
            this.m_PasswordBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_PasswordBox.Name = "m_PasswordBox";
            this.m_PasswordBox.PasswordChar = '*';
            this.m_PasswordBox.Size = new System.Drawing.Size(116, 23);
            this.m_PasswordBox.TabIndex = 6;
            // 
            // m_PasswordBox0
            // 
            this.m_PasswordBox0.Location = new System.Drawing.Point(120, 25);
            this.m_PasswordBox0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_PasswordBox0.Name = "m_PasswordBox0";
            this.m_PasswordBox0.PasswordChar = '*';
            this.m_PasswordBox0.Size = new System.Drawing.Size(116, 23);
            this.m_PasswordBox0.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "新密码:";
            // 
            // m_PasswordBox2
            // 
            this.m_PasswordBox2.Location = new System.Drawing.Point(120, 132);
            this.m_PasswordBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_PasswordBox2.Name = "m_PasswordBox2";
            this.m_PasswordBox2.PasswordChar = '*';
            this.m_PasswordBox2.Size = new System.Drawing.Size(116, 23);
            this.m_PasswordBox2.TabIndex = 9;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(40, 188);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(84, 28);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "确认(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(192, 188);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 28);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // m_ErrorProvider
            // 
            this.m_ErrorProvider.ContainerControl = this;
            // 
            // ChangePwdForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(331, 240);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_PasswordBox2);
            this.Controls.Add(this.m_UserNameLabel);
            this.Controls.Add(this.m_PasswordLabel);
            this.Controls.Add(this.m_PasswordBox);
            this.Controls.Add(this.m_PasswordBox0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePwdForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改密码";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_ChangePwd_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.m_ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label m_UserNameLabel;
		private System.Windows.Forms.Label m_PasswordLabel;
		private System.Windows.Forms.TextBox m_PasswordBox;
		private System.Windows.Forms.TextBox m_PasswordBox0;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_PasswordBox2;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ErrorProvider m_ErrorProvider;
	}
}