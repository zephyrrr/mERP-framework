using System.Windows.Forms;
namespace Feng.Windows.Forms
{
   partial class LoginControl
   {
      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
          this.components = new System.ComponentModel.Container();
          this.m_UserNameBox = new System.Windows.Forms.TextBox();
          this.m_PasswordBox = new System.Windows.Forms.TextBox();
          this.m_PasswordLabel = new System.Windows.Forms.Label();
          this.m_UserNameLabel = new System.Windows.Forms.Label();
          this.m_ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
          ((System.ComponentModel.ISupportInitialize)(this.m_ErrorProvider)).BeginInit();
          this.SuspendLayout();
          // 
          // m_UserNameBox
          // 
          this.m_UserNameBox.Location = new System.Drawing.Point(80, 10);
          this.m_UserNameBox.Name = "m_UserNameBox";
          this.m_UserNameBox.Size = new System.Drawing.Size(100, 21);
          this.m_UserNameBox.TabIndex = 1;
          // 
          // m_PasswordBox
          // 
          this.m_PasswordBox.AcceptsReturn = true;
          this.m_PasswordBox.Location = new System.Drawing.Point(80, 53);
          this.m_PasswordBox.Name = "m_PasswordBox";
          this.m_PasswordBox.PasswordChar = '*';
          this.m_PasswordBox.Size = new System.Drawing.Size(100, 21);
          this.m_PasswordBox.TabIndex = 2;
          // 
          // m_PasswordLabel
          // 
          this.m_PasswordLabel.AutoSize = true;
          this.m_PasswordLabel.Location = new System.Drawing.Point(9, 53);
          this.m_PasswordLabel.Name = "m_PasswordLabel";
          this.m_PasswordLabel.Size = new System.Drawing.Size(53, 12);
          this.m_PasswordLabel.TabIndex = 3;
          this.m_PasswordLabel.Text = "密码(&P):";
          // 
          // m_UserNameLabel
          // 
          this.m_UserNameLabel.AutoSize = true;
          this.m_UserNameLabel.Location = new System.Drawing.Point(9, 10);
          this.m_UserNameLabel.Name = "m_UserNameLabel";
          this.m_UserNameLabel.Size = new System.Drawing.Size(65, 12);
          this.m_UserNameLabel.TabIndex = 4;
          this.m_UserNameLabel.Text = "用户名(&U):";
          // 
          // m_ErrorProvider
          // 
          this.m_ErrorProvider.ContainerControl = this;
          // 
          // LoginControl
          // 
          this.Controls.Add(this.m_UserNameLabel);
          this.Controls.Add(this.m_PasswordLabel);
          this.Controls.Add(this.m_PasswordBox);
          this.Controls.Add(this.m_UserNameBox);
          this.Name = "LoginControl";
          this.Size = new System.Drawing.Size(220, 85);
          ((System.ComponentModel.ISupportInitialize)(this.m_ErrorProvider)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

      }

      #endregion

	  TextBox m_UserNameBox;
      TextBox m_PasswordBox;
      Label m_PasswordLabel;
      Label m_UserNameLabel;
      ErrorProvider m_ErrorProvider;
      private System.ComponentModel.IContainer components;
   }
}
