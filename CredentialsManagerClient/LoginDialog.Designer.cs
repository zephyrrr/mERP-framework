namespace CredentialsManagerClient
{
   partial class LoginDialog
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
         if(disposing && (components != null))
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
         System.Windows.Forms.Button loginButton;
         System.Windows.Forms.Label applicationLabel;
         System.Windows.Forms.Label userNameLabel;
         System.Windows.Forms.Label passwordLabel;
         this.m_ApplicationTextBox = new System.Windows.Forms.TextBox();
         this.m_UserNameTextBox = new System.Windows.Forms.TextBox();
         this.m_PasswordTextBox = new System.Windows.Forms.TextBox();
         loginButton = new System.Windows.Forms.Button();
         applicationLabel = new System.Windows.Forms.Label();
         userNameLabel = new System.Windows.Forms.Label();
         passwordLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // loginButton
         // 
         loginButton.Location = new System.Drawing.Point(137,22);
         loginButton.Name = "loginButton";
         loginButton.Size = new System.Drawing.Size(75,23);
         loginButton.TabIndex = 0;
         loginButton.Text = "Login";
         loginButton.Click += new System.EventHandler(this.OnLogin);
         // 
         // applicationLabel
         // 
         applicationLabel.AutoSize = true;
         applicationLabel.Location = new System.Drawing.Point(12,9);
         applicationLabel.Name = "applicationLabel";
         applicationLabel.Size = new System.Drawing.Size(58,13);
         applicationLabel.TabIndex = 2;
         applicationLabel.Text = "Application:";
         // 
         // userNameLabel
         // 
         userNameLabel.AutoSize = true;
         userNameLabel.Location = new System.Drawing.Point(12,48);
         userNameLabel.Name = "userNameLabel";
         userNameLabel.Size = new System.Drawing.Size(59,13);
         userNameLabel.TabIndex = 3;
         userNameLabel.Text = "User Name:";
         // 
         // passwordLabel
         // 
         passwordLabel.AutoSize = true;
         passwordLabel.Location = new System.Drawing.Point(12,90);
         passwordLabel.Name = "passwordLabel";
         passwordLabel.Size = new System.Drawing.Size(52,13);
         passwordLabel.TabIndex = 6;
         passwordLabel.Text = "Password:";
         // 
         // m_ApplicationTextBox
         // 
         this.m_ApplicationTextBox.Location = new System.Drawing.Point(13,25);
         this.m_ApplicationTextBox.Name = "m_ApplicationTextBox";
         this.m_ApplicationTextBox.Size = new System.Drawing.Size(100,20);
         this.m_ApplicationTextBox.TabIndex = 1;
         // 
         // m_UserNameTextBox
         // 
         this.m_UserNameTextBox.Location = new System.Drawing.Point(13,64);
         this.m_UserNameTextBox.Name = "m_UserNameTextBox";
         this.m_UserNameTextBox.Size = new System.Drawing.Size(100,20);
         this.m_UserNameTextBox.TabIndex = 4;
         // 
         // m_PasswordTextBox
         // 
         this.m_PasswordTextBox.Location = new System.Drawing.Point(13,106);
         this.m_PasswordTextBox.Name = "m_PasswordTextBox";
         this.m_PasswordTextBox.PasswordChar = '*';
         this.m_PasswordTextBox.Size = new System.Drawing.Size(100,20);
         this.m_PasswordTextBox.TabIndex = 5;
         // 
         // LoginDialog
         // 
         this.AcceptButton = loginButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(215,134);
         this.Controls.Add(passwordLabel);
         this.Controls.Add(this.m_PasswordTextBox);
         this.Controls.Add(this.m_UserNameTextBox);
         this.Controls.Add(userNameLabel);
         this.Controls.Add(applicationLabel);
         this.Controls.Add(this.m_ApplicationTextBox);
         this.Controls.Add(loginButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "LoginDialog";
         this.Text = "Test Login Credenials";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox m_ApplicationTextBox;
      private System.Windows.Forms.TextBox m_UserNameTextBox;
      private System.Windows.Forms.TextBox m_PasswordTextBox;
   }
}