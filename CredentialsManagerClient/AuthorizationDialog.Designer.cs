namespace CredentialsManagerClient
{
   partial class AuthorizationDialog
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
         System.Windows.Forms.Button verifyButton;
         System.Windows.Forms.Label applicationLabel;
         System.Windows.Forms.Label userNameLabel;
         System.Windows.Forms.Label roleLabel;
         System.Windows.Forms.Button closeButton;
         this.m_ApplicationTextBox = new System.Windows.Forms.TextBox();
         this.m_RoleComboBox = new System.Windows.Forms.ComboBox();
         this.m_UserComboBox = new System.Windows.Forms.ComboBox();
         verifyButton = new System.Windows.Forms.Button();
         applicationLabel = new System.Windows.Forms.Label();
         userNameLabel = new System.Windows.Forms.Label();
         roleLabel = new System.Windows.Forms.Label();
         closeButton = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // verifyButton
         // 
         verifyButton.Location = new System.Drawing.Point(179,23);
         verifyButton.Name = "verifyButton";
         verifyButton.Size = new System.Drawing.Size(75,23);
         verifyButton.TabIndex = 0;
         verifyButton.Text = "Verify";
         verifyButton.Click += new System.EventHandler(this.OnLogin);
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
         userNameLabel.Location = new System.Drawing.Point(12,47);
         userNameLabel.Name = "userNameLabel";
         userNameLabel.Size = new System.Drawing.Size(140,13);
         userNameLabel.TabIndex = 3;
         userNameLabel.Text = "Type or Select a User Name:";
         // 
         // roleLabel
         // 
         roleLabel.AutoSize = true;
         roleLabel.Location = new System.Drawing.Point(12,85);
         roleLabel.Name = "roleLabel";
         roleLabel.Size = new System.Drawing.Size(109,13);
         roleLabel.TabIndex = 6;
         roleLabel.Text = "Type or Select a Role:";
         // 
         // closeButton
         // 
         closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         closeButton.Location = new System.Drawing.Point(178,101);
         closeButton.Name = "closeButton";
         closeButton.Size = new System.Drawing.Size(75,23);
         closeButton.TabIndex = 7;
         closeButton.Text = "Close";
         closeButton.Click += new System.EventHandler(this.OnClose);
         // 
         // m_ApplicationTextBox
         // 
         this.m_ApplicationTextBox.Location = new System.Drawing.Point(13,25);
         this.m_ApplicationTextBox.Name = "m_ApplicationTextBox";
         this.m_ApplicationTextBox.Size = new System.Drawing.Size(100,20);
         this.m_ApplicationTextBox.TabIndex = 1;
         // 
         // m_RoleComboBox
         // 
         this.m_RoleComboBox.FormattingEnabled = true;
         this.m_RoleComboBox.Location = new System.Drawing.Point(13,101);
         this.m_RoleComboBox.Name = "m_RoleComboBox";
         this.m_RoleComboBox.Size = new System.Drawing.Size(141,21);
         this.m_RoleComboBox.TabIndex = 8;
         // 
         // m_UserComboBox
         // 
         this.m_UserComboBox.FormattingEnabled = true;
         this.m_UserComboBox.Location = new System.Drawing.Point(13,63);
         this.m_UserComboBox.Name = "m_UserComboBox";
         this.m_UserComboBox.Size = new System.Drawing.Size(141,21);
         this.m_UserComboBox.TabIndex = 9;
         // 
         // AuthorizationDialog
         // 
         this.AcceptButton = verifyButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = closeButton;
         this.ClientSize = new System.Drawing.Size(265,131);
         this.Controls.Add(this.m_UserComboBox);
         this.Controls.Add(this.m_RoleComboBox);
         this.Controls.Add(closeButton);
         this.Controls.Add(roleLabel);
         this.Controls.Add(userNameLabel);
         this.Controls.Add(applicationLabel);
         this.Controls.Add(this.m_ApplicationTextBox);
         this.Controls.Add(verifyButton);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "AuthorizationDialog";
         this.Text = "Test Role Membership";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox m_ApplicationTextBox;
      private System.Windows.Forms.ComboBox m_RoleComboBox;
      private System.Windows.Forms.ComboBox m_UserComboBox;
   }
}