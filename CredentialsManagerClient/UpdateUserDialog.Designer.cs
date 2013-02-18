using System.Windows.Forms;
namespace CredentialsManagerClient
{
   partial class UpdateUserDialog 
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
         System.Windows.Forms.Label newAnswerLabel;
         System.Windows.Forms.Label newQuestionLabel;
         System.Windows.Forms.Label passwordAnswer;
         System.Windows.Forms.Button updateUserButton;
         System.Windows.Forms.Label passwordQuestionLabel;
         System.Windows.Forms.Label emailLabel;
         System.Windows.Forms.Label userNameLabel;
         this.m_NewAnswerTextBox = new System.Windows.Forms.TextBox();
         this.m_NewQuestionTextBox = new System.Windows.Forms.TextBox();
         this.m_LcokedOutCheckBox = new System.Windows.Forms.CheckBox();
         this.m_OldAnswerTextBox = new System.Windows.Forms.TextBox();
         this.m_ActiveUserCheckbox = new System.Windows.Forms.CheckBox();
         this.m_OldQuestionTextBox = new System.Windows.Forms.TextBox();
         this.m_EmailTextBox = new System.Windows.Forms.TextBox();
         this.m_UserNameTextBox = new System.Windows.Forms.TextBox();
         this.m_Validator = new System.Windows.Forms.ErrorProvider(this.components);
         userGroup = new System.Windows.Forms.GroupBox();
         newAnswerLabel = new System.Windows.Forms.Label();
         newQuestionLabel = new System.Windows.Forms.Label();
         passwordAnswer = new System.Windows.Forms.Label();
         updateUserButton = new System.Windows.Forms.Button();
         passwordQuestionLabel = new System.Windows.Forms.Label();
         emailLabel = new System.Windows.Forms.Label();
         userNameLabel = new System.Windows.Forms.Label();
         userGroup.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.m_Validator)).BeginInit();
         this.SuspendLayout();
         // 
         // userGroup
         // 
         userGroup.Controls.Add(this.m_NewAnswerTextBox);
         userGroup.Controls.Add(newAnswerLabel);
         userGroup.Controls.Add(this.m_NewQuestionTextBox);
         userGroup.Controls.Add(newQuestionLabel);
         userGroup.Controls.Add(this.m_LcokedOutCheckBox);
         userGroup.Controls.Add(this.m_OldAnswerTextBox);
         userGroup.Controls.Add(passwordAnswer);
         userGroup.Controls.Add(this.m_ActiveUserCheckbox);
         userGroup.Controls.Add(updateUserButton);
         userGroup.Controls.Add(this.m_OldQuestionTextBox);
         userGroup.Controls.Add(passwordQuestionLabel);
         userGroup.Controls.Add(this.m_EmailTextBox);
         userGroup.Controls.Add(emailLabel);
         userGroup.Controls.Add(this.m_UserNameTextBox);
         userGroup.Controls.Add(userNameLabel);
         userGroup.Location = new System.Drawing.Point(6,11);
         userGroup.Name = "userGroup";
         userGroup.Size = new System.Drawing.Size(231,266);
         userGroup.TabIndex = 1;
         userGroup.TabStop = false;
         userGroup.Text = "User Account:";
         // 
         // m_NewAnswerTextBox
         // 
         this.m_NewAnswerTextBox.Location = new System.Drawing.Point(7,237);
         this.m_NewAnswerTextBox.Name = "m_NewAnswerTextBox";
         this.m_NewAnswerTextBox.Size = new System.Drawing.Size(100,20);
         this.m_NewAnswerTextBox.TabIndex = 20;
         // 
         // newAnswerLabel
         // 
         newAnswerLabel.AutoSize = true;
         newAnswerLabel.Location = new System.Drawing.Point(6,221);
         newAnswerLabel.Name = "newAnswerLabel";
         newAnswerLabel.Size = new System.Drawing.Size(66,13);
         newAnswerLabel.TabIndex = 19;
         newAnswerLabel.Text = "New Answer:";
         // 
         // m_NewQuestionTextBox
         // 
         this.m_NewQuestionTextBox.Location = new System.Drawing.Point(7,195);
         this.m_NewQuestionTextBox.Name = "m_NewQuestionTextBox";
         this.m_NewQuestionTextBox.Size = new System.Drawing.Size(100,20);
         this.m_NewQuestionTextBox.TabIndex = 18;
         // 
         // newQuestionLabel
         // 
         newQuestionLabel.AutoSize = true;
         newQuestionLabel.Location = new System.Drawing.Point(6,179);
         newQuestionLabel.Name = "newQuestionLabel";
         newQuestionLabel.Size = new System.Drawing.Size(73,13);
         newQuestionLabel.TabIndex = 17;
         newQuestionLabel.Text = "New Question:";
         // 
         // m_LcokedOutCheckBox
         // 
         this.m_LcokedOutCheckBox.AutoSize = true;
         this.m_LcokedOutCheckBox.Location = new System.Drawing.Point(138,52);
         this.m_LcokedOutCheckBox.Name = "m_LcokedOutCheckBox";
         this.m_LcokedOutCheckBox.Size = new System.Drawing.Size(78,17);
         this.m_LcokedOutCheckBox.TabIndex = 16;
         this.m_LcokedOutCheckBox.Text = "Locked Out";
         // 
         // m_OldAnswerTextBox
         // 
         this.m_OldAnswerTextBox.Location = new System.Drawing.Point(7,155);
         this.m_OldAnswerTextBox.Name = "m_OldAnswerTextBox";
         this.m_OldAnswerTextBox.Size = new System.Drawing.Size(100,20);
         this.m_OldAnswerTextBox.TabIndex = 15;
         // 
         // passwordAnswer
         // 
         passwordAnswer.AutoSize = true;
         passwordAnswer.Location = new System.Drawing.Point(6,139);
         passwordAnswer.Name = "passwordAnswer";
         passwordAnswer.Size = new System.Drawing.Size(60,13);
         passwordAnswer.TabIndex = 14;
         passwordAnswer.Text = "Old Answer:";
         // 
         // m_ActiveUserCheckbox
         // 
         this.m_ActiveUserCheckbox.AutoSize = true;
         this.m_ActiveUserCheckbox.Location = new System.Drawing.Point(138,29);
         this.m_ActiveUserCheckbox.Name = "m_ActiveUserCheckbox";
         this.m_ActiveUserCheckbox.Size = new System.Drawing.Size(77,17);
         this.m_ActiveUserCheckbox.TabIndex = 13;
         this.m_ActiveUserCheckbox.Text = "Active User";
         // 
         // updateUserButton
         // 
         updateUserButton.Location = new System.Drawing.Point(138,75);
         updateUserButton.Name = "updateUserButton";
         updateUserButton.Size = new System.Drawing.Size(88,23);
         updateUserButton.TabIndex = 4;
         updateUserButton.Text = "Update User";
         updateUserButton.Click += new System.EventHandler(this.OnUpdateUser);
         // 
         // m_OldQuestionTextBox
         // 
         this.m_OldQuestionTextBox.Enabled = false;
         this.m_OldQuestionTextBox.Location = new System.Drawing.Point(7,113);
         this.m_OldQuestionTextBox.Name = "m_OldQuestionTextBox";
         this.m_OldQuestionTextBox.Size = new System.Drawing.Size(100,20);
         this.m_OldQuestionTextBox.TabIndex = 9;
         // 
         // passwordQuestionLabel
         // 
         passwordQuestionLabel.AutoSize = true;
         passwordQuestionLabel.Location = new System.Drawing.Point(6,97);
         passwordQuestionLabel.Name = "passwordQuestionLabel";
         passwordQuestionLabel.Size = new System.Drawing.Size(67,13);
         passwordQuestionLabel.TabIndex = 8;
         passwordQuestionLabel.Text = "Old Question:";
         // 
         // m_EmailTextBox
         // 
         this.m_EmailTextBox.Location = new System.Drawing.Point(7,70);
         this.m_EmailTextBox.Name = "m_EmailTextBox";
         this.m_EmailTextBox.Size = new System.Drawing.Size(100,20);
         this.m_EmailTextBox.TabIndex = 7;
         // 
         // emailLabel
         // 
         emailLabel.AutoSize = true;
         emailLabel.Location = new System.Drawing.Point(6,54);
         emailLabel.Name = "emailLabel";
         emailLabel.Size = new System.Drawing.Size(34,13);
         emailLabel.TabIndex = 6;
         emailLabel.Text = "E-mail:";
         // 
         // m_UserNameTextBox
         // 
         this.m_UserNameTextBox.Location = new System.Drawing.Point(6,29);
         this.m_UserNameTextBox.Name = "m_UserNameTextBox";
         this.m_UserNameTextBox.ReadOnly = true;
         this.m_UserNameTextBox.Size = new System.Drawing.Size(100,20);
         this.m_UserNameTextBox.TabIndex = 1;
         // 
         // userNameLabel
         // 
         userNameLabel.AutoSize = true;
         userNameLabel.Location = new System.Drawing.Point(5,13);
         userNameLabel.Name = "userNameLabel";
         userNameLabel.Size = new System.Drawing.Size(59,13);
         userNameLabel.TabIndex = 0;
         userNameLabel.Text = "User Name:";
         // 
         // m_Validator
         // 
         this.m_Validator.ContainerControl = this;
         // 
         // UpdateUserDialog
         // 
         this.AcceptButton = updateUserButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(242,280);
         this.Controls.Add(userGroup);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "UpdateUserDialog";
         this.ShowIcon = false;
         this.Text = "Update User Dialog";
         userGroup.ResumeLayout(false);
         userGroup.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.m_Validator)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TextBox m_EmailTextBox;
      private System.Windows.Forms.TextBox m_UserNameTextBox;
      private System.Windows.Forms.ErrorProvider m_Validator;
      private CheckBox m_ActiveUserCheckbox;
      private TextBox m_OldAnswerTextBox;
      private TextBox m_OldQuestionTextBox;
      private CheckBox m_LcokedOutCheckBox;
      private TextBox m_NewAnswerTextBox;
      private TextBox m_NewQuestionTextBox;
   }
}