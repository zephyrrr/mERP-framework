using System.Windows.Forms;
namespace CredentialsManagerClient
{
   partial class ChangePasswordDialog 
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
         this.components = new System.ComponentModel.Container();
         System.Windows.Forms.GroupBox userGroup;
         System.Windows.Forms.Label newPassword;
         System.Windows.Forms.Label passwordAnswer;
         System.Windows.Forms.Button changePasswordButton;
         System.Windows.Forms.Label passwordQuestionLabel;
         System.Windows.Forms.Label userNameLabel;
         this.m_NewPasswordTextBox = new System.Windows.Forms.TextBox();
         this.m_PasswordAnswerTextBox = new System.Windows.Forms.TextBox();
         this.m_PasswordQuestionTextBox = new System.Windows.Forms.TextBox();
         this.m_UserNameTextBox = new System.Windows.Forms.TextBox();
         this.m_Validator = new System.Windows.Forms.ErrorProvider(this.components);
         userGroup = new System.Windows.Forms.GroupBox();
         newPassword = new System.Windows.Forms.Label();
         passwordAnswer = new System.Windows.Forms.Label();
         changePasswordButton = new System.Windows.Forms.Button();
         passwordQuestionLabel = new System.Windows.Forms.Label();
         userNameLabel = new System.Windows.Forms.Label();
         userGroup.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.m_Validator)).BeginInit();
         this.SuspendLayout();
         // 
         // userGroup
         // 
         userGroup.Controls.Add(this.m_NewPasswordTextBox);
         userGroup.Controls.Add(newPassword);
         userGroup.Controls.Add(this.m_PasswordAnswerTextBox);
         userGroup.Controls.Add(passwordAnswer);
         userGroup.Controls.Add(changePasswordButton);
         userGroup.Controls.Add(this.m_PasswordQuestionTextBox);
         userGroup.Controls.Add(passwordQuestionLabel);
         userGroup.Controls.Add(this.m_UserNameTextBox);
         userGroup.Controls.Add(userNameLabel);
         userGroup.Location = new System.Drawing.Point(6,11);
         userGroup.Name = "userGroup";
         userGroup.Size = new System.Drawing.Size(223,191);
         userGroup.TabIndex = 1;
         userGroup.TabStop = false;
         userGroup.Text = "User Account:";
         // 
         // m_NewPasswordTextBox
         // 
         this.m_NewPasswordTextBox.Location = new System.Drawing.Point(7,160);
         this.m_NewPasswordTextBox.Name = "m_NewPasswordTextBox";
         this.m_NewPasswordTextBox.Size = new System.Drawing.Size(100,20);
         this.m_NewPasswordTextBox.TabIndex = 17;
         // 
         // newPassword
         // 
         newPassword.AutoSize = true;
         newPassword.Location = new System.Drawing.Point(6,144);
         newPassword.Name = "newPassword";
         newPassword.Size = new System.Drawing.Size(77,13);
         newPassword.TabIndex = 16;
         newPassword.Text = "New Password:";
         // 
         // m_PasswordAnswerTextBox
         // 
         this.m_PasswordAnswerTextBox.Location = new System.Drawing.Point(7,115);
         this.m_PasswordAnswerTextBox.Name = "m_PasswordAnswerTextBox";
         this.m_PasswordAnswerTextBox.Size = new System.Drawing.Size(100,20);
         this.m_PasswordAnswerTextBox.TabIndex = 15;
         // 
         // passwordAnswer
         // 
         passwordAnswer.AutoSize = true;
         passwordAnswer.Location = new System.Drawing.Point(6,99);
         passwordAnswer.Name = "passwordAnswer";
         passwordAnswer.Size = new System.Drawing.Size(90,13);
         passwordAnswer.TabIndex = 14;
         passwordAnswer.Text = "Password Answer:";
         // 
         // changePasswordButton
         // 
         changePasswordButton.Location = new System.Drawing.Point(140,31);
         changePasswordButton.Name = "changePasswordButton";
         changePasswordButton.Size = new System.Drawing.Size(75,23);
         changePasswordButton.TabIndex = 4;
         changePasswordButton.Text = "Change";
         changePasswordButton.Click += new System.EventHandler(this.OnChange);
         // 
         // m_PasswordQuestionTextBox
         // 
         this.m_PasswordQuestionTextBox.Enabled = false;
         this.m_PasswordQuestionTextBox.Location = new System.Drawing.Point(7,73);
         this.m_PasswordQuestionTextBox.Name = "m_PasswordQuestionTextBox";
         this.m_PasswordQuestionTextBox.Size = new System.Drawing.Size(100,20);
         this.m_PasswordQuestionTextBox.TabIndex = 9;
         // 
         // passwordQuestionLabel
         // 
         passwordQuestionLabel.AutoSize = true;
         passwordQuestionLabel.Location = new System.Drawing.Point(6,57);
         passwordQuestionLabel.Name = "passwordQuestionLabel";
         passwordQuestionLabel.Size = new System.Drawing.Size(89,13);
         passwordQuestionLabel.TabIndex = 8;
         passwordQuestionLabel.Text = "Security Question:";
         // 
         // m_UserNameTextBox
         // 
         this.m_UserNameTextBox.Enabled = false;
         this.m_UserNameTextBox.Location = new System.Drawing.Point(7,34);
         this.m_UserNameTextBox.Name = "m_UserNameTextBox";
         this.m_UserNameTextBox.ReadOnly = true;
         this.m_UserNameTextBox.Size = new System.Drawing.Size(100,20);
         this.m_UserNameTextBox.TabIndex = 1;
         // 
         // userNameLabel
         // 
         userNameLabel.AutoSize = true;
         userNameLabel.Location = new System.Drawing.Point(6,18);
         userNameLabel.Name = "userNameLabel";
         userNameLabel.Size = new System.Drawing.Size(59,13);
         userNameLabel.TabIndex = 0;
         userNameLabel.Text = "User Name:";
         // 
         // m_Validator
         // 
         this.m_Validator.ContainerControl = this;
         // 
         // ChangePasswordDialog
         // 
         this.AcceptButton = changePasswordButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(233,207);
         this.Controls.Add(userGroup);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ChangePasswordDialog";
         this.ShowIcon = false;
         this.Text = "Change Password Dialog";
         userGroup.ResumeLayout(false);
         userGroup.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.m_Validator)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TextBox m_UserNameTextBox;
      private System.Windows.Forms.ErrorProvider m_Validator;
      private TextBox m_PasswordQuestionTextBox;
      private TextBox m_PasswordAnswerTextBox;
      private TextBox m_NewPasswordTextBox;
   }
}