using System.Windows.Forms;
namespace CredentialsManagerClient
{
   partial class ResetWithQuestionDialog 
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
         System.Windows.Forms.Label passwordAnswer;
         System.Windows.Forms.Button resetPasswordButton;
         System.Windows.Forms.Label passwordQuestionLabel;
         System.Windows.Forms.Label userNameLabel;
         this.m_PasswordAnswerTextBox = new System.Windows.Forms.TextBox();
         this.m_PasswordQuestionTextBox = new System.Windows.Forms.TextBox();
         this.m_UserNameTextBox = new System.Windows.Forms.TextBox();
         this.m_Validator = new System.Windows.Forms.ErrorProvider(this.components);
         userGroup = new System.Windows.Forms.GroupBox();
         passwordAnswer = new System.Windows.Forms.Label();
         resetPasswordButton = new System.Windows.Forms.Button();
         passwordQuestionLabel = new System.Windows.Forms.Label();
         userNameLabel = new System.Windows.Forms.Label();
         userGroup.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.m_Validator)).BeginInit();
         this.SuspendLayout();
         // 
         // userGroup
         // 
         userGroup.Controls.Add(this.m_PasswordAnswerTextBox);
         userGroup.Controls.Add(passwordAnswer);
         userGroup.Controls.Add(resetPasswordButton);
         userGroup.Controls.Add(this.m_PasswordQuestionTextBox);
         userGroup.Controls.Add(passwordQuestionLabel);
         userGroup.Controls.Add(this.m_UserNameTextBox);
         userGroup.Controls.Add(userNameLabel);
         userGroup.Location = new System.Drawing.Point(6,11);
         userGroup.Name = "userGroup";
         userGroup.Size = new System.Drawing.Size(216,146);
         userGroup.TabIndex = 1;
         userGroup.TabStop = false;
         userGroup.Text = "User Account:";
         // 
         // m_PasswordAnswerTextBox
         // 
         this.m_PasswordAnswerTextBox.Location = new System.Drawing.Point(7,116);
         this.m_PasswordAnswerTextBox.Name = "m_PasswordAnswerTextBox";
         this.m_PasswordAnswerTextBox.Size = new System.Drawing.Size(100,20);
         this.m_PasswordAnswerTextBox.TabIndex = 15;
         // 
         // passwordAnswer
         // 
         passwordAnswer.AutoSize = true;
         passwordAnswer.Location = new System.Drawing.Point(6,100);
         passwordAnswer.Name = "passwordAnswer";
         passwordAnswer.Size = new System.Drawing.Size(90,13);
         passwordAnswer.TabIndex = 14;
         passwordAnswer.Text = "Password Answer:";
         // 
         // resetPasswordButton
         // 
         resetPasswordButton.Location = new System.Drawing.Point(121,32);
         resetPasswordButton.Name = "resetPasswordButton";
         resetPasswordButton.Size = new System.Drawing.Size(88,23);
         resetPasswordButton.TabIndex = 4;
         resetPasswordButton.Text = "Reset";
         resetPasswordButton.Click += new System.EventHandler(this.OnReset);
         // 
         // m_PasswordQuestionTextBox
         // 
         this.m_PasswordQuestionTextBox.Enabled = false;
         this.m_PasswordQuestionTextBox.Location = new System.Drawing.Point(7,74);
         this.m_PasswordQuestionTextBox.Name = "m_PasswordQuestionTextBox";
         this.m_PasswordQuestionTextBox.Size = new System.Drawing.Size(100,20);
         this.m_PasswordQuestionTextBox.TabIndex = 9;
         // 
         // passwordQuestionLabel
         // 
         passwordQuestionLabel.AutoSize = true;
         passwordQuestionLabel.Location = new System.Drawing.Point(6,58);
         passwordQuestionLabel.Name = "passwordQuestionLabel";
         passwordQuestionLabel.Size = new System.Drawing.Size(89,13);
         passwordQuestionLabel.TabIndex = 8;
         passwordQuestionLabel.Text = "Security Question:";
         // 
         // m_UserNameTextBox
         // 
         this.m_UserNameTextBox.Enabled = false;
         this.m_UserNameTextBox.Location = new System.Drawing.Point(6,35);
         this.m_UserNameTextBox.Name = "m_UserNameTextBox";
         this.m_UserNameTextBox.ReadOnly = true;
         this.m_UserNameTextBox.Size = new System.Drawing.Size(100,20);
         this.m_UserNameTextBox.TabIndex = 1;
         // 
         // userNameLabel
         // 
         userNameLabel.AutoSize = true;
         userNameLabel.Location = new System.Drawing.Point(5,19);
         userNameLabel.Name = "userNameLabel";
         userNameLabel.Size = new System.Drawing.Size(59,13);
         userNameLabel.TabIndex = 0;
         userNameLabel.Text = "User Name:";
         // 
         // m_Validator
         // 
         this.m_Validator.ContainerControl = this;
         // 
         // ResetWithQuestionDialog
         // 
         this.AcceptButton = resetPasswordButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(227,162);
         this.Controls.Add(userGroup);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ResetWithQuestionDialog";
         this.ShowIcon = false;
         this.Text = "Reset Password Dialog";
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
   }
}