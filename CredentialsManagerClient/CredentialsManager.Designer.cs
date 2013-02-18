// ?2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System.Windows.Forms;
using CredentialsManagerClient.Properties;
namespace CredentialsManagerClient
{
   partial class CredentialsManagerForm
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialsManagerForm));
         this.m_PasswordReset = new System.Windows.Forms.Label();
         this.m_RequiresQuestionAndAnswerLabel = new System.Windows.Forms.Label();
         this.m_PasswordRetrieval = new System.Windows.Forms.Label();
         this.m_PasswordRegularExpression = new System.Windows.Forms.Label();
         this.m_MaxInvalidAttempts = new System.Windows.Forms.Label();
         this.m_AttemptWindow = new System.Windows.Forms.Label();
         this.m_MinNonAlphanumeric = new System.Windows.Forms.Label();
         this.m_MinLength = new System.Windows.Forms.Label();
         this.m_LengthTextBox = new System.Windows.Forms.TextBox();
         this.m_NonAlphanumericTextBox = new System.Windows.Forms.TextBox();
         this.m_GeneratePassword = new System.Windows.Forms.Button();
         this.m_RolesForUserComboBox = new ComboBoxEx();
         this.m_UsersToAssignListView = new CredentialsManagerClient.ListViewEx();
         this.m_RemoveUserFromAllRolesButton = new System.Windows.Forms.Button();
         this.m_RemoveUserFromRoleButton = new System.Windows.Forms.Button();
         this.m_AssignButton = new System.Windows.Forms.Button();
         this.m_UsersInRoleComboBox = new ComboBoxEx();
         this.m_RolesListView = new CredentialsManagerClient.ListViewEx();
         this.m_PopulatedLabel = new System.Windows.Forms.Label();
         this.m_ThrowIfPopulatedCheckBox = new System.Windows.Forms.CheckBox();
         this.m_DeleteAllRolesButton = new System.Windows.Forms.Button();
         this.m_CreateRoleButton = new System.Windows.Forms.Button();
         this.m_DeleteRoleButton = new System.Windows.Forms.Button();
         this.m_UsersStatusRefresh = new System.Windows.Forms.Button();
         this.m_OnlineTimeWindow = new System.Windows.Forms.Label();
         this.m_UsersOnline = new System.Windows.Forms.Label();
         this.m_UsersListView = new CredentialsManagerClient.ListViewEx();
         this.m_ChangePasswordButton = new System.Windows.Forms.Button();
         this.m_ResetPasswordButton = new System.Windows.Forms.Button();
         this.m_RelatedDataCheckBox = new System.Windows.Forms.CheckBox();
         this.m_DeleteAllUsersButton = new System.Windows.Forms.Button();
         this.m_UpdateUser = new System.Windows.Forms.Button();
         this.m_DeleteUserButton = new System.Windows.Forms.Button();
         this.m_CreateUserButton = new System.Windows.Forms.Button();
         this.m_ApplicationListView = new CredentialsManagerClient.ListViewEx();
         this.m_DeleteAllApplicationsButton = new System.Windows.Forms.Button();
         this.m_CreateApplicationButton = new System.Windows.Forms.Button();
         this.m_DeleteApplicationButton = new System.Windows.Forms.Button();
         this.m_SelectButton = new System.Windows.Forms.Button();
         this.m_WebBrowser = new System.Windows.Forms.WebBrowser();
         this.m_AddressTextbox = new System.Windows.Forms.TextBox();
         this.m_ViewButton = new System.Windows.Forms.Button();
         this.m_CreateApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_DeleteApplicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_DeleteAllApplicationsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_CreateUserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_UpdateUserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_DeleteUserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_DeleteAllUsersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_ChangePasswordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_ResetPasswordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_RefreshUsersStatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_CreateRoleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_DeleteRoleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_DeleteAllRolesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_AssignUsertoRoleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_RemoveUserFromRoleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_RemoveUserFromAllRolesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_GeneratePasswordMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_ViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_SelectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_LogOnMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_AuthorizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.helpContentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_AddressLabel = new System.Windows.Forms.Label();
         this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
         passwordSetupGroupBox = new System.Windows.Forms.GroupBox();
         passwordResetLabel = new System.Windows.Forms.Label();
         passwordRetrievalLabel = new System.Windows.Forms.Label();
         requiresQuestionAndAnswerLabel = new System.Windows.Forms.Label();
         maxInvalidLabel = new System.Windows.Forms.Label();
         passwordRegularExpressionLabel = new System.Windows.Forms.Label();
         minNonAlpha = new System.Windows.Forms.Label();
         attemptWindowLabel = new System.Windows.Forms.Label();
         minLengthLabel = new System.Windows.Forms.Label();
         generatePassorgGroupBox = new System.Windows.Forms.GroupBox();
         nonAlphanumericLabel = new System.Windows.Forms.Label();
         lengthLabel = new System.Windows.Forms.Label();
         usersGroupBox = new System.Windows.Forms.GroupBox();
         userToassignHeader = new System.Windows.Forms.ColumnHeader();
         rolesForUserLabel = new System.Windows.Forms.Label();
         usersToAssignHeader = new System.Windows.Forms.ColumnHeader();
         rolesGroupBox = new System.Windows.Forms.GroupBox();
         rolesHeader = new System.Windows.Forms.ColumnHeader();
         usersInRoleLabel = new System.Windows.Forms.Label();
         usersStatus = new System.Windows.Forms.GroupBox();
         onlineTimeWindowLabel = new System.Windows.Forms.Label();
         onlineUsersLabel = new System.Windows.Forms.Label();
         usersGoupBox = new System.Windows.Forms.GroupBox();
         usersHeader = new System.Windows.Forms.ColumnHeader();
         applicationsGroupBox = new System.Windows.Forms.GroupBox();
         applicationsHeader = new System.Windows.Forms.ColumnHeader();
         columnApplications = new System.Windows.Forms.ColumnHeader();
         addressGroupBox = new System.Windows.Forms.GroupBox();
         mainMenu = new System.Windows.Forms.MenuStrip();
         applicationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         usersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         usersSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         usersSeparator2 = new System.Windows.Forms.ToolStripSeparator();
         rolesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         rolesSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         passwordsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         serviceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         testMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         passwordsPage = new System.Windows.Forms.TabPage();
         rolesPage = new System.Windows.Forms.TabPage();
         usersPage = new System.Windows.Forms.TabPage();
         applicationsTab = new System.Windows.Forms.TabPage();
         applicationPictureBox = new System.Windows.Forms.PictureBox();
         tabControl = new System.Windows.Forms.TabControl();
         servicePage = new System.Windows.Forms.TabPage();
         passwordSetupGroupBox.SuspendLayout();
         generatePassorgGroupBox.SuspendLayout();
         usersGroupBox.SuspendLayout();
         rolesGroupBox.SuspendLayout();
         usersStatus.SuspendLayout();
         usersGoupBox.SuspendLayout();
         applicationsGroupBox.SuspendLayout();
         addressGroupBox.SuspendLayout();
         mainMenu.SuspendLayout();
         passwordsPage.SuspendLayout();
         rolesPage.SuspendLayout();
         usersPage.SuspendLayout();
         applicationsTab.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(applicationPictureBox)).BeginInit();
         tabControl.SuspendLayout();
         servicePage.SuspendLayout();
         this.SuspendLayout();
         // 
         // passwordSetupGroupBox
         // 
         passwordSetupGroupBox.Controls.Add(passwordResetLabel);
         passwordSetupGroupBox.Controls.Add(this.m_PasswordReset);
         passwordSetupGroupBox.Controls.Add(this.m_RequiresQuestionAndAnswerLabel);
         passwordSetupGroupBox.Controls.Add(passwordRetrievalLabel);
         passwordSetupGroupBox.Controls.Add(requiresQuestionAndAnswerLabel);
         passwordSetupGroupBox.Controls.Add(this.m_PasswordRetrieval);
         passwordSetupGroupBox.Controls.Add(this.m_PasswordRegularExpression);
         passwordSetupGroupBox.Controls.Add(maxInvalidLabel);
         passwordSetupGroupBox.Controls.Add(passwordRegularExpressionLabel);
         passwordSetupGroupBox.Controls.Add(this.m_MaxInvalidAttempts);
         passwordSetupGroupBox.Controls.Add(this.m_AttemptWindow);
         passwordSetupGroupBox.Controls.Add(minNonAlpha);
         passwordSetupGroupBox.Controls.Add(attemptWindowLabel);
         passwordSetupGroupBox.Controls.Add(this.m_MinNonAlphanumeric);
         passwordSetupGroupBox.Controls.Add(this.m_MinLength);
         passwordSetupGroupBox.Controls.Add(minLengthLabel);
         passwordSetupGroupBox.Location = new System.Drawing.Point(6,11);
         passwordSetupGroupBox.Name = "passwordSetupGroupBox";
         passwordSetupGroupBox.Size = new System.Drawing.Size(258,368);
         passwordSetupGroupBox.TabIndex = 18;
         passwordSetupGroupBox.TabStop = false;
         passwordSetupGroupBox.Text = "Setup";
         // 
         // passwordResetLabel
         // 
         passwordResetLabel.AutoSize = true;
         passwordResetLabel.Location = new System.Drawing.Point(19,21);
         passwordResetLabel.Name = "passwordResetLabel";
         passwordResetLabel.Size = new System.Drawing.Size(119,13);
         passwordResetLabel.TabIndex = 0;
         passwordResetLabel.Text = "Password reset enabled:";
         // 
         // m_PasswordReset
         // 
         this.m_PasswordReset.AutoSize = true;
         this.m_PasswordReset.Location = new System.Drawing.Point(188,21);
         this.m_PasswordReset.Name = "m_PasswordReset";
         this.m_PasswordReset.Size = new System.Drawing.Size(21,13);
         this.m_PasswordReset.TabIndex = 1;
         this.m_PasswordReset.Text = "Yes";
         // 
         // m_RequiresQuestionAndAnswerLabel
         // 
         this.m_RequiresQuestionAndAnswerLabel.AutoSize = true;
         this.m_RequiresQuestionAndAnswerLabel.Location = new System.Drawing.Point(188,269);
         this.m_RequiresQuestionAndAnswerLabel.Name = "m_RequiresQuestionAndAnswerLabel";
         this.m_RequiresQuestionAndAnswerLabel.Size = new System.Drawing.Size(21,13);
         this.m_RequiresQuestionAndAnswerLabel.TabIndex = 15;
         this.m_RequiresQuestionAndAnswerLabel.Text = "Yes";
         // 
         // passwordRetrievalLabel
         // 
         passwordRetrievalLabel.AutoSize = true;
         passwordRetrievalLabel.Location = new System.Drawing.Point(19,54);
         passwordRetrievalLabel.Name = "passwordRetrievalLabel";
         passwordRetrievalLabel.Size = new System.Drawing.Size(133,13);
         passwordRetrievalLabel.TabIndex = 2;
         passwordRetrievalLabel.Text = "Password retrieval enabled:";
         // 
         // requiresQuestionAndAnswerLabel
         // 
         requiresQuestionAndAnswerLabel.AutoSize = true;
         requiresQuestionAndAnswerLabel.Location = new System.Drawing.Point(19,269);
         requiresQuestionAndAnswerLabel.Name = "requiresQuestionAndAnswerLabel";
         requiresQuestionAndAnswerLabel.Size = new System.Drawing.Size(149,13);
         requiresQuestionAndAnswerLabel.TabIndex = 14;
         requiresQuestionAndAnswerLabel.Text = "Requires question and answer:";
         // 
         // m_PasswordRetrieval
         // 
         this.m_PasswordRetrieval.AutoSize = true;
         this.m_PasswordRetrieval.Location = new System.Drawing.Point(188,54);
         this.m_PasswordRetrieval.Name = "m_PasswordRetrieval";
         this.m_PasswordRetrieval.Size = new System.Drawing.Size(21,13);
         this.m_PasswordRetrieval.TabIndex = 3;
         this.m_PasswordRetrieval.Text = "Yes";
         // 
         // m_PasswordRegularExpression
         // 
         this.m_PasswordRegularExpression.AutoSize = true;
         this.m_PasswordRegularExpression.Location = new System.Drawing.Point(188,232);
         this.m_PasswordRegularExpression.Name = "m_PasswordRegularExpression";
         this.m_PasswordRegularExpression.Size = new System.Drawing.Size(7,13);
         this.m_PasswordRegularExpression.TabIndex = 13;
         this.m_PasswordRegularExpression.Text = "*";
         // 
         // maxInvalidLabel
         // 
         maxInvalidLabel.AutoSize = true;
         maxInvalidLabel.Location = new System.Drawing.Point(19,89);
         maxInvalidLabel.Name = "maxInvalidLabel";
         maxInvalidLabel.Size = new System.Drawing.Size(136,13);
         maxInvalidLabel.TabIndex = 4;
         maxInvalidLabel.Text = "Max invalid attempt allowed:";
         // 
         // passwordRegularExpressionLabel
         // 
         passwordRegularExpressionLabel.AutoSize = true;
         passwordRegularExpressionLabel.Location = new System.Drawing.Point(19,232);
         passwordRegularExpressionLabel.Name = "passwordRegularExpressionLabel";
         passwordRegularExpressionLabel.Size = new System.Drawing.Size(140,13);
         passwordRegularExpressionLabel.TabIndex = 12;
         passwordRegularExpressionLabel.Text = "Password regular expression:";
         // 
         // m_MaxInvalidAttempts
         // 
         this.m_MaxInvalidAttempts.AutoSize = true;
         this.m_MaxInvalidAttempts.Location = new System.Drawing.Point(188,89);
         this.m_MaxInvalidAttempts.Name = "m_MaxInvalidAttempts";
         this.m_MaxInvalidAttempts.Size = new System.Drawing.Size(9,13);
         this.m_MaxInvalidAttempts.TabIndex = 5;
         this.m_MaxInvalidAttempts.Text = "0";
         // 
         // m_AttemptWindow
         // 
         this.m_AttemptWindow.AutoSize = true;
         this.m_AttemptWindow.Location = new System.Drawing.Point(188,197);
         this.m_AttemptWindow.Name = "m_AttemptWindow";
         this.m_AttemptWindow.Size = new System.Drawing.Size(9,13);
         this.m_AttemptWindow.TabIndex = 11;
         this.m_AttemptWindow.Text = "0";
         // 
         // minNonAlpha
         // 
         minNonAlpha.AutoSize = true;
         minNonAlpha.Location = new System.Drawing.Point(19,125);
         minNonAlpha.Name = "minNonAlpha";
         minNonAlpha.Size = new System.Drawing.Size(163,13);
         minNonAlpha.TabIndex = 6;
         minNonAlpha.Text = "Min non-alphanumeric characters:";
         // 
         // attemptWindowLabel
         // 
         attemptWindowLabel.AutoSize = true;
         attemptWindowLabel.Location = new System.Drawing.Point(19,197);
         attemptWindowLabel.Name = "attemptWindowLabel";
         attemptWindowLabel.Size = new System.Drawing.Size(81,13);
         attemptWindowLabel.TabIndex = 10;
         attemptWindowLabel.Text = "Attempt window:";
         // 
         // m_MinNonAlphanumeric
         // 
         this.m_MinNonAlphanumeric.AutoSize = true;
         this.m_MinNonAlphanumeric.Location = new System.Drawing.Point(188,125);
         this.m_MinNonAlphanumeric.Name = "m_MinNonAlphanumeric";
         this.m_MinNonAlphanumeric.Size = new System.Drawing.Size(9,13);
         this.m_MinNonAlphanumeric.TabIndex = 7;
         this.m_MinNonAlphanumeric.Text = "0";
         // 
         // m_MinLength
         // 
         this.m_MinLength.AutoSize = true;
         this.m_MinLength.Location = new System.Drawing.Point(188,161);
         this.m_MinLength.Name = "m_MinLength";
         this.m_MinLength.Size = new System.Drawing.Size(9,13);
         this.m_MinLength.TabIndex = 9;
         this.m_MinLength.Text = "0";
         // 
         // minLengthLabel
         // 
         minLengthLabel.AutoSize = true;
         minLengthLabel.Location = new System.Drawing.Point(19,161);
         minLengthLabel.Name = "minLengthLabel";
         minLengthLabel.Size = new System.Drawing.Size(96,13);
         minLengthLabel.TabIndex = 8;
         minLengthLabel.Text = "Min required length:";
         // 
         // generatePassorgGroupBox
         // 
         generatePassorgGroupBox.Controls.Add(nonAlphanumericLabel);
         generatePassorgGroupBox.Controls.Add(lengthLabel);
         generatePassorgGroupBox.Controls.Add(this.m_LengthTextBox);
         generatePassorgGroupBox.Controls.Add(this.m_NonAlphanumericTextBox);
         generatePassorgGroupBox.Controls.Add(this.m_GeneratePassword);
         generatePassorgGroupBox.Location = new System.Drawing.Point(270,11);
         generatePassorgGroupBox.Name = "generatePassorgGroupBox";
         generatePassorgGroupBox.Size = new System.Drawing.Size(258,368);
         generatePassorgGroupBox.TabIndex = 17;
         generatePassorgGroupBox.TabStop = false;
         generatePassorgGroupBox.Text = "Generate Password";
         // 
         // nonAlphanumericLabel
         // 
         nonAlphanumericLabel.AutoSize = true;
         nonAlphanumericLabel.Location = new System.Drawing.Point(14,65);
         nonAlphanumericLabel.Name = "nonAlphanumericLabel";
         nonAlphanumericLabel.Size = new System.Drawing.Size(92,13);
         nonAlphanumericLabel.TabIndex = 20;
         nonAlphanumericLabel.Text = "Non-alphanumeric:";
         // 
         // lengthLabel
         // 
         lengthLabel.AutoSize = true;
         lengthLabel.Location = new System.Drawing.Point(15,19);
         lengthLabel.Name = "lengthLabel";
         lengthLabel.Size = new System.Drawing.Size(39,13);
         lengthLabel.TabIndex = 19;
         lengthLabel.Text = "Length:";
         // 
         // m_LengthTextBox
         // 
         this.m_LengthTextBox.Location = new System.Drawing.Point(15,34);
         this.m_LengthTextBox.Name = "m_LengthTextBox";
         this.m_LengthTextBox.Size = new System.Drawing.Size(100,20);
         this.m_LengthTextBox.TabIndex = 18;
         this.m_LengthTextBox.Text = "6";
         // 
         // m_NonAlphanumericTextBox
         // 
         this.m_NonAlphanumericTextBox.Location = new System.Drawing.Point(15,81);
         this.m_NonAlphanumericTextBox.Name = "m_NonAlphanumericTextBox";
         this.m_NonAlphanumericTextBox.Size = new System.Drawing.Size(100,20);
         this.m_NonAlphanumericTextBox.TabIndex = 17;
         this.m_NonAlphanumericTextBox.Text = "1";
         // 
         // m_GeneratePassword
         // 
         this.m_GeneratePassword.Location = new System.Drawing.Point(177,34);
         this.m_GeneratePassword.Name = "m_GeneratePassword";
         this.m_GeneratePassword.Size = new System.Drawing.Size(75,23);
         this.m_GeneratePassword.TabIndex = 16;
         this.m_GeneratePassword.Text = "Generate";
         this.m_GeneratePassword.Click += new System.EventHandler(this.OnGeneratePassword);
         // 
         // usersGroupBox
         // 
         usersGroupBox.Controls.Add(this.m_RolesForUserComboBox);
         usersGroupBox.Controls.Add(this.m_UsersToAssignListView);
         usersGroupBox.Controls.Add(this.m_RemoveUserFromAllRolesButton);
         usersGroupBox.Controls.Add(rolesForUserLabel);
         usersGroupBox.Controls.Add(this.m_RemoveUserFromRoleButton);
         usersGroupBox.Controls.Add(this.m_AssignButton);
         usersGroupBox.Location = new System.Drawing.Point(6,11);
         usersGroupBox.Name = "usersGroupBox";
         usersGroupBox.Size = new System.Drawing.Size(258,368);
         usersGroupBox.TabIndex = 1;
         usersGroupBox.TabStop = false;
         usersGroupBox.Text = "Users";
         // 
         // m_RolesForUserComboBox
         // 
         this.m_RolesForUserComboBox.FormattingEnabled = true;
         this.m_RolesForUserComboBox.ImageList = null;
         this.m_RolesForUserComboBox.Location = new System.Drawing.Point(6,341);
         this.m_RolesForUserComboBox.Name = "m_RolesForUserComboBox";
         this.m_RolesForUserComboBox.Size = new System.Drawing.Size(165,21);
         this.m_RolesForUserComboBox.TabIndex = 4;
         // 
         // m_UsersToAssignListView
         // 
         this.m_UsersToAssignListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            userToassignHeader});
         this.m_UsersToAssignListView.FullRowSelect = true;
         this.m_UsersToAssignListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.m_UsersToAssignListView.HideSelection = false;
         this.m_UsersToAssignListView.Location = new System.Drawing.Point(7,19);
         this.m_UsersToAssignListView.MultiSelect = false;
         this.m_UsersToAssignListView.Name = "m_UsersToAssignListView";
         this.m_UsersToAssignListView.ShowGroups = false;
         this.m_UsersToAssignListView.Size = new System.Drawing.Size(165,290);
         this.m_UsersToAssignListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
         this.m_UsersToAssignListView.TabIndex = 2;
         this.m_UsersToAssignListView.View = System.Windows.Forms.View.SmallIcon;
         this.m_UsersToAssignListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OnSelectedUserAssignChanged);
         // 
         // userToassignHeader
         // 
         userToassignHeader.Width = 300;
         // 
         // m_RemoveUserFromAllRolesButton
         // 
         this.m_RemoveUserFromAllRolesButton.Location = new System.Drawing.Point(177,77);
         this.m_RemoveUserFromAllRolesButton.Name = "m_RemoveUserFromAllRolesButton";
         this.m_RemoveUserFromAllRolesButton.Size = new System.Drawing.Size(75,23);
         this.m_RemoveUserFromAllRolesButton.TabIndex = 2;
         this.m_RemoveUserFromAllRolesButton.Text = "Remove All";
         this.m_RemoveUserFromAllRolesButton.Click += new System.EventHandler(this.OnRemoveUsersFromAllRoles);
         // 
         // rolesForUserLabel
         // 
         rolesForUserLabel.AutoSize = true;
         rolesForUserLabel.Location = new System.Drawing.Point(6,325);
         rolesForUserLabel.Name = "rolesForUserLabel";
         rolesForUserLabel.Size = new System.Drawing.Size(73,13);
         rolesForUserLabel.TabIndex = 5;
         rolesForUserLabel.Text = "Roles for User:";
         // 
         // m_RemoveUserFromRoleButton
         // 
         this.m_RemoveUserFromRoleButton.Location = new System.Drawing.Point(177,48);
         this.m_RemoveUserFromRoleButton.Name = "m_RemoveUserFromRoleButton";
         this.m_RemoveUserFromRoleButton.Size = new System.Drawing.Size(75,23);
         this.m_RemoveUserFromRoleButton.TabIndex = 3;
         this.m_RemoveUserFromRoleButton.Text = "Remove";
         this.m_RemoveUserFromRoleButton.Click += new System.EventHandler(this.OnRemoveUserFromRole);
         // 
         // m_AssignButton
         // 
         this.m_AssignButton.Location = new System.Drawing.Point(177,19);
         this.m_AssignButton.Name = "m_AssignButton";
         this.m_AssignButton.Size = new System.Drawing.Size(75,23);
         this.m_AssignButton.TabIndex = 1;
         this.m_AssignButton.Text = "Assign";
         this.m_AssignButton.Click += new System.EventHandler(this.OnAssignUserToRole);
         // 
         // usersToAssignHeader
         // 
         usersToAssignHeader.Text = "";
         usersToAssignHeader.Width = 186;
         // 
         // rolesGroupBox
         // 
         rolesGroupBox.Controls.Add(this.m_UsersInRoleComboBox);
         rolesGroupBox.Controls.Add(this.m_RolesListView);
         rolesGroupBox.Controls.Add(this.m_PopulatedLabel);
         rolesGroupBox.Controls.Add(this.m_ThrowIfPopulatedCheckBox);
         rolesGroupBox.Controls.Add(this.m_DeleteAllRolesButton);
         rolesGroupBox.Controls.Add(usersInRoleLabel);
         rolesGroupBox.Controls.Add(this.m_CreateRoleButton);
         rolesGroupBox.Controls.Add(this.m_DeleteRoleButton);
         rolesGroupBox.Location = new System.Drawing.Point(270,11);
         rolesGroupBox.Name = "rolesGroupBox";
         rolesGroupBox.Size = new System.Drawing.Size(258,368);
         rolesGroupBox.TabIndex = 0;
         rolesGroupBox.TabStop = false;
         rolesGroupBox.Text = "Roles";
         // 
         // m_UsersInRoleComboBox
         // 
         this.m_UsersInRoleComboBox.FormattingEnabled = true;
         this.m_UsersInRoleComboBox.ImageList = null;
         this.m_UsersInRoleComboBox.Location = new System.Drawing.Point(6,341);
         this.m_UsersInRoleComboBox.Name = "m_UsersInRoleComboBox";
         this.m_UsersInRoleComboBox.Size = new System.Drawing.Size(165,21);
         this.m_UsersInRoleComboBox.TabIndex = 5;
         // 
         // m_RolesListView
         // 
         this.m_RolesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            rolesHeader});
         this.m_RolesListView.FullRowSelect = true;
         this.m_RolesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.m_RolesListView.HideSelection = false;
         this.m_RolesListView.Location = new System.Drawing.Point(6,19);
         this.m_RolesListView.MultiSelect = false;
         this.m_RolesListView.Name = "m_RolesListView";
         this.m_RolesListView.ShowGroups = false;
         this.m_RolesListView.Size = new System.Drawing.Size(165,290);
         this.m_RolesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
         this.m_RolesListView.TabIndex = 6;
         this.m_RolesListView.View = System.Windows.Forms.View.SmallIcon;
         this.m_RolesListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OnSelectedRoleChanged);
         // 
         // rolesHeader
         // 
         rolesHeader.Width = 300;
         // 
         // m_PopulatedLabel
         // 
         this.m_PopulatedLabel.AutoSize = true;
         this.m_PopulatedLabel.Location = new System.Drawing.Point(197,97);
         this.m_PopulatedLabel.Name = "m_PopulatedLabel";
         this.m_PopulatedLabel.Size = new System.Drawing.Size(51,13);
         this.m_PopulatedLabel.TabIndex = 7;
         this.m_PopulatedLabel.Text = "Populated";
         // 
         // m_ThrowIfPopulatedCheckBox
         // 
         this.m_ThrowIfPopulatedCheckBox.AutoSize = true;
         this.m_ThrowIfPopulatedCheckBox.Location = new System.Drawing.Point(177,77);
         this.m_ThrowIfPopulatedCheckBox.Name = "m_ThrowIfPopulatedCheckBox";
         this.m_ThrowIfPopulatedCheckBox.Size = new System.Drawing.Size(46,17);
         this.m_ThrowIfPopulatedCheckBox.TabIndex = 2;
         this.m_ThrowIfPopulatedCheckBox.Text = "Fail if";
         // 
         // m_DeleteAllRolesButton
         // 
         this.m_DeleteAllRolesButton.Location = new System.Drawing.Point(177,286);
         this.m_DeleteAllRolesButton.Name = "m_DeleteAllRolesButton";
         this.m_DeleteAllRolesButton.Size = new System.Drawing.Size(75,23);
         this.m_DeleteAllRolesButton.TabIndex = 6;
         this.m_DeleteAllRolesButton.Text = "Delete All";
         this.m_DeleteAllRolesButton.Click += new System.EventHandler(this.OnDeleteAllRoles);
         // 
         // usersInRoleLabel
         // 
         usersInRoleLabel.AutoSize = true;
         usersInRoleLabel.Location = new System.Drawing.Point(6,325);
         usersInRoleLabel.Name = "usersInRoleLabel";
         usersInRoleLabel.Size = new System.Drawing.Size(69,13);
         usersInRoleLabel.TabIndex = 4;
         usersInRoleLabel.Text = "Users in Role:";
         // 
         // m_CreateRoleButton
         // 
         this.m_CreateRoleButton.Location = new System.Drawing.Point(177,19);
         this.m_CreateRoleButton.Name = "m_CreateRoleButton";
         this.m_CreateRoleButton.Size = new System.Drawing.Size(75,23);
         this.m_CreateRoleButton.TabIndex = 2;
         this.m_CreateRoleButton.Text = "Create";
         this.m_CreateRoleButton.Click += new System.EventHandler(this.OnCreateRole);
         // 
         // m_DeleteRoleButton
         // 
         this.m_DeleteRoleButton.Location = new System.Drawing.Point(177,48);
         this.m_DeleteRoleButton.Name = "m_DeleteRoleButton";
         this.m_DeleteRoleButton.Size = new System.Drawing.Size(75,23);
         this.m_DeleteRoleButton.TabIndex = 1;
         this.m_DeleteRoleButton.Text = "Delete";
         this.m_DeleteRoleButton.Click += new System.EventHandler(this.OnDeleteRole);
         // 
         // usersStatus
         // 
         usersStatus.Controls.Add(this.m_UsersStatusRefresh);
         usersStatus.Controls.Add(this.m_OnlineTimeWindow);
         usersStatus.Controls.Add(onlineTimeWindowLabel);
         usersStatus.Controls.Add(this.m_UsersOnline);
         usersStatus.Controls.Add(onlineUsersLabel);
         usersStatus.Location = new System.Drawing.Point(298,11);
         usersStatus.Name = "usersStatus";
         usersStatus.Size = new System.Drawing.Size(230,368);
         usersStatus.TabIndex = 8;
         usersStatus.TabStop = false;
         usersStatus.Text = "Users Status";
         // 
         // m_UsersStatusRefresh
         // 
         this.m_UsersStatusRefresh.Location = new System.Drawing.Point(139,135);
         this.m_UsersStatusRefresh.Name = "m_UsersStatusRefresh";
         this.m_UsersStatusRefresh.Size = new System.Drawing.Size(75,23);
         this.m_UsersStatusRefresh.TabIndex = 13;
         this.m_UsersStatusRefresh.Text = "Refresh";
         this.m_UsersStatusRefresh.Click += new System.EventHandler(this.OnUserStatusRefresh);
         // 
         // m_OnlineTimeWindow
         // 
         this.m_OnlineTimeWindow.AutoSize = true;
         this.m_OnlineTimeWindow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.m_OnlineTimeWindow.Location = new System.Drawing.Point(138,62);
         this.m_OnlineTimeWindow.Name = "m_OnlineTimeWindow";
         this.m_OnlineTimeWindow.Size = new System.Drawing.Size(9,13);
         this.m_OnlineTimeWindow.TabIndex = 12;
         this.m_OnlineTimeWindow.Text = "0";
         // 
         // onlineTimeWindowLabel
         // 
         onlineTimeWindowLabel.AutoSize = true;
         onlineTimeWindowLabel.Location = new System.Drawing.Point(12,62);
         onlineTimeWindowLabel.Name = "onlineTimeWindowLabel";
         onlineTimeWindowLabel.Size = new System.Drawing.Size(97,13);
         onlineTimeWindowLabel.TabIndex = 11;
         onlineTimeWindowLabel.Text = "Online time window:";
         // 
         // m_UsersOnline
         // 
         this.m_UsersOnline.AutoSize = true;
         this.m_UsersOnline.Location = new System.Drawing.Point(138,33);
         this.m_UsersOnline.Name = "m_UsersOnline";
         this.m_UsersOnline.Size = new System.Drawing.Size(9,13);
         this.m_UsersOnline.TabIndex = 10;
         this.m_UsersOnline.Text = "0";
         this.m_UsersOnline.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // onlineUsersLabel
         // 
         onlineUsersLabel.AutoSize = true;
         onlineUsersLabel.Location = new System.Drawing.Point(12,33);
         onlineUsersLabel.Name = "onlineUsersLabel";
         onlineUsersLabel.Size = new System.Drawing.Size(114,13);
         onlineUsersLabel.TabIndex = 9;
         onlineUsersLabel.Text = "Number of users online:";
         // 
         // usersGoupBox
         // 
         usersGoupBox.Controls.Add(this.m_UsersListView);
         usersGoupBox.Controls.Add(this.m_ChangePasswordButton);
         usersGoupBox.Controls.Add(this.m_ResetPasswordButton);
         usersGoupBox.Controls.Add(this.m_RelatedDataCheckBox);
         usersGoupBox.Controls.Add(this.m_DeleteAllUsersButton);
         usersGoupBox.Controls.Add(this.m_UpdateUser);
         usersGoupBox.Controls.Add(this.m_DeleteUserButton);
         usersGoupBox.Controls.Add(this.m_CreateUserButton);
         usersGoupBox.Location = new System.Drawing.Point(6,11);
         usersGoupBox.Name = "usersGoupBox";
         usersGoupBox.Size = new System.Drawing.Size(286,368);
         usersGoupBox.TabIndex = 4;
         usersGoupBox.TabStop = false;
         usersGoupBox.Text = "Users";
         // 
         // m_UsersListView
         // 
         this.m_UsersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            usersHeader});
         this.m_UsersListView.FullRowSelect = true;
         this.m_UsersListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.m_UsersListView.HideSelection = false;
         this.m_UsersListView.Location = new System.Drawing.Point(6,19);
         this.m_UsersListView.MultiSelect = false;
         this.m_UsersListView.Name = "m_UsersListView";
         this.m_UsersListView.ShowGroups = false;
         this.m_UsersListView.Size = new System.Drawing.Size(165,342);
         this.m_UsersListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
         this.m_UsersListView.TabIndex = 10;
         this.m_UsersListView.View = System.Windows.Forms.View.SmallIcon;
         // 
         // usersHeader
         // 
         usersHeader.Width = 300;
         // 
         // m_ChangePasswordButton
         // 
         this.m_ChangePasswordButton.Location = new System.Drawing.Point(177,77);
         this.m_ChangePasswordButton.Name = "m_ChangePasswordButton";
         this.m_ChangePasswordButton.Size = new System.Drawing.Size(103,23);
         this.m_ChangePasswordButton.TabIndex = 12;
         this.m_ChangePasswordButton.Text = "Change Password";
         this.m_ChangePasswordButton.Click += new System.EventHandler(this.OnChangePassword);
         // 
         // m_ResetPasswordButton
         // 
         this.m_ResetPasswordButton.Location = new System.Drawing.Point(177,106);
         this.m_ResetPasswordButton.Name = "m_ResetPasswordButton";
         this.m_ResetPasswordButton.Size = new System.Drawing.Size(103,23);
         this.m_ResetPasswordButton.TabIndex = 11;
         this.m_ResetPasswordButton.Text = "Reset Password";
         this.m_ResetPasswordButton.Click += new System.EventHandler(this.OnResetPassword);
         // 
         // m_RelatedDataCheckBox
         // 
         this.m_RelatedDataCheckBox.AutoSize = true;
         this.m_RelatedDataCheckBox.Checked = true;
         this.m_RelatedDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
         this.m_RelatedDataCheckBox.Location = new System.Drawing.Point(177,164);
         this.m_RelatedDataCheckBox.Name = "m_RelatedDataCheckBox";
         this.m_RelatedDataCheckBox.Size = new System.Drawing.Size(59,17);
         this.m_RelatedDataCheckBox.TabIndex = 9;
         this.m_RelatedDataCheckBox.Text = "All Data";
         // 
         // m_DeleteAllUsersButton
         // 
         this.m_DeleteAllUsersButton.Location = new System.Drawing.Point(177,338);
         this.m_DeleteAllUsersButton.Name = "m_DeleteAllUsersButton";
         this.m_DeleteAllUsersButton.Size = new System.Drawing.Size(103,23);
         this.m_DeleteAllUsersButton.TabIndex = 8;
         this.m_DeleteAllUsersButton.Text = "Delete All";
         this.m_DeleteAllUsersButton.Click += new System.EventHandler(this.OnDeleteAllUsers);
         // 
         // m_UpdateUser
         // 
         this.m_UpdateUser.Location = new System.Drawing.Point(177,48);
         this.m_UpdateUser.Name = "m_UpdateUser";
         this.m_UpdateUser.Size = new System.Drawing.Size(103,23);
         this.m_UpdateUser.TabIndex = 7;
         this.m_UpdateUser.Text = "Update";
         this.m_UpdateUser.Click += new System.EventHandler(this.OnUpdateUser);
         // 
         // m_DeleteUserButton
         // 
         this.m_DeleteUserButton.Location = new System.Drawing.Point(177,135);
         this.m_DeleteUserButton.Name = "m_DeleteUserButton";
         this.m_DeleteUserButton.Size = new System.Drawing.Size(103,23);
         this.m_DeleteUserButton.TabIndex = 4;
         this.m_DeleteUserButton.Text = "Delete";
         this.m_DeleteUserButton.Click += new System.EventHandler(this.OnDeleteUser);
         // 
         // m_CreateUserButton
         // 
         this.m_CreateUserButton.Location = new System.Drawing.Point(177,19);
         this.m_CreateUserButton.Name = "m_CreateUserButton";
         this.m_CreateUserButton.Size = new System.Drawing.Size(103,23);
         this.m_CreateUserButton.TabIndex = 4;
         this.m_CreateUserButton.Text = "Create...";
         this.m_CreateUserButton.Click += new System.EventHandler(this.OnCreateUser);
         // 
         // applicationsGroupBox
         // 
         applicationsGroupBox.Controls.Add(this.m_ApplicationListView);
         applicationsGroupBox.Controls.Add(this.m_DeleteAllApplicationsButton);
         applicationsGroupBox.Controls.Add(this.m_CreateApplicationButton);
         applicationsGroupBox.Controls.Add(this.m_DeleteApplicationButton);
         applicationsGroupBox.Location = new System.Drawing.Point(6,11);
         applicationsGroupBox.Name = "applicationsGroupBox";
         applicationsGroupBox.Size = new System.Drawing.Size(258,368);
         applicationsGroupBox.TabIndex = 11;
         applicationsGroupBox.TabStop = false;
         applicationsGroupBox.Text = "Applications";
         // 
         // m_ApplicationListView
         // 
         this.m_ApplicationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            applicationsHeader});
         this.m_ApplicationListView.FullRowSelect = true;
         this.m_ApplicationListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.m_ApplicationListView.HideSelection = false;
         this.m_ApplicationListView.Location = new System.Drawing.Point(6,19);
         this.m_ApplicationListView.MultiSelect = false;
         this.m_ApplicationListView.Name = "m_ApplicationListView";
         this.m_ApplicationListView.ShowGroups = false;
         this.m_ApplicationListView.Size = new System.Drawing.Size(165,342);
         this.m_ApplicationListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
         this.m_ApplicationListView.TabIndex = 12;
         this.m_ApplicationListView.View = System.Windows.Forms.View.SmallIcon;
         this.m_ApplicationListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OnSelectedApplicationChanged);
         // 
         // applicationsHeader
         // 
         applicationsHeader.Width = 390;
         // 
         // m_DeleteAllApplicationsButton
         // 
         this.m_DeleteAllApplicationsButton.Location = new System.Drawing.Point(177,338);
         this.m_DeleteAllApplicationsButton.Name = "m_DeleteAllApplicationsButton";
         this.m_DeleteAllApplicationsButton.Size = new System.Drawing.Size(75,23);
         this.m_DeleteAllApplicationsButton.TabIndex = 11;
         this.m_DeleteAllApplicationsButton.Text = "Delete All";
         this.m_DeleteAllApplicationsButton.Click += new System.EventHandler(this.OnDeleteAllApplications);
         // 
         // m_CreateApplicationButton
         // 
         this.m_CreateApplicationButton.Location = new System.Drawing.Point(177,19);
         this.m_CreateApplicationButton.Name = "m_CreateApplicationButton";
         this.m_CreateApplicationButton.Size = new System.Drawing.Size(75,23);
         this.m_CreateApplicationButton.TabIndex = 4;
         this.m_CreateApplicationButton.Text = "Create";
         this.m_CreateApplicationButton.Click += new System.EventHandler(this.OnCreateApplication);
         // 
         // m_DeleteApplicationButton
         // 
         this.m_DeleteApplicationButton.Location = new System.Drawing.Point(177,48);
         this.m_DeleteApplicationButton.Name = "m_DeleteApplicationButton";
         this.m_DeleteApplicationButton.Size = new System.Drawing.Size(75,23);
         this.m_DeleteApplicationButton.TabIndex = 7;
         this.m_DeleteApplicationButton.Text = "Delete";
         this.m_DeleteApplicationButton.Click += new System.EventHandler(this.OnDeleteApplication);
         // 
         // columnApplications
         // 
         columnApplications.Text = "Select Application:";
         columnApplications.Width = 186;
         // 
         // addressGroupBox
         // 
         addressGroupBox.Controls.Add(this.m_SelectButton);
         addressGroupBox.Controls.Add(this.m_WebBrowser);
         addressGroupBox.Controls.Add(this.m_AddressTextbox);
         addressGroupBox.Controls.Add(this.m_ViewButton);
         addressGroupBox.Location = new System.Drawing.Point(6,11);
         addressGroupBox.Name = "addressGroupBox";
         addressGroupBox.Size = new System.Drawing.Size(549,368);
         addressGroupBox.TabIndex = 5;
         addressGroupBox.TabStop = false;
         addressGroupBox.Text = "Credentials Web Service Address:";
         // 
         // m_SelectButton
         // 
         this.m_SelectButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.m_SelectButton.Location = new System.Drawing.Point(450,17);
         this.m_SelectButton.Name = "m_SelectButton";
         this.m_SelectButton.Size = new System.Drawing.Size(75,23);
         this.m_SelectButton.TabIndex = 5;
         this.m_SelectButton.Text = "Select";
         this.m_SelectButton.Click += new System.EventHandler(this.OnSelectService);
         // 
         // m_WebBrowser
         // 
         this.m_WebBrowser.Location = new System.Drawing.Point(6,45);
         this.m_WebBrowser.Name = "m_WebBrowser";
         this.m_WebBrowser.Size = new System.Drawing.Size(519,317);
         this.m_WebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.OnDownloadCompleted);
         // 
         // m_AddressTextbox
         // 
         this.m_AddressTextbox.Location = new System.Drawing.Point(6,19);
         this.m_AddressTextbox.Name = "m_AddressTextbox";
         this.m_AddressTextbox.Size = new System.Drawing.Size(357,20);
         this.m_AddressTextbox.TabIndex = 3;
         this.m_AddressTextbox.Text = "http://localhost/CredentialsService/AspNetSqlProviderService.asmx";
         // 
         // m_ViewButton
         // 
         this.m_ViewButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.m_ViewButton.Location = new System.Drawing.Point(369,17);
         this.m_ViewButton.Name = "m_ViewButton";
         this.m_ViewButton.Size = new System.Drawing.Size(75,23);
         this.m_ViewButton.TabIndex = 4;
         this.m_ViewButton.Text = "View";
         this.m_ViewButton.Click += new System.EventHandler(this.OnViewService);
         // 
         // mainMenu
         // 
         mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            applicationMenuItem,
            usersMenuItem,
            rolesMenuItem,
            passwordsMenuItem,
            serviceMenuItem,
            testMenuItem,
            helpMenuItem});
         mainMenu.Location = new System.Drawing.Point(0,0);
         mainMenu.Name = "mainMenu";
         mainMenu.Size = new System.Drawing.Size(542,24);
         mainMenu.TabIndex = 1;
         mainMenu.Text = "m_MainMenu";
         // 
         // applicationMenuItem
         // 
         applicationMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_CreateApplicationMenuItem,
            this.m_DeleteApplicationMenuItem,
            this.m_DeleteAllApplicationsMenuItem});
         applicationMenuItem.Name = "applicationMenuItem";
         applicationMenuItem.Text = "Application";
         // 
         // m_CreateApplicationMenuItem
         // 
         this.m_CreateApplicationMenuItem.Image = CredentialsManagerClient.Properties.Resources.CreateIApplication;
         this.m_CreateApplicationMenuItem.Name = "m_CreateApplicationMenuItem";
         this.m_CreateApplicationMenuItem.Text = "Create";
         this.m_CreateApplicationMenuItem.Click += new System.EventHandler(this.OnCreateApplication);
         // 
         // m_DeleteApplicationMenuItem
         // 
         this.m_DeleteApplicationMenuItem.Image = CredentialsManagerClient.Properties.Resources.DeleteApplication;
         this.m_DeleteApplicationMenuItem.Name = "m_DeleteApplicationMenuItem";
         this.m_DeleteApplicationMenuItem.Text = "Delete";
         this.m_DeleteApplicationMenuItem.Click += new System.EventHandler(this.OnDeleteApplication);
         // 
         // m_DeleteAllApplicationsMenuItem
         // 
         this.m_DeleteAllApplicationsMenuItem.Image = CredentialsManagerClient.Properties.Resources.DeleteAllApplications;
         this.m_DeleteAllApplicationsMenuItem.Name = "m_DeleteAllApplicationsMenuItem";
         this.m_DeleteAllApplicationsMenuItem.Text = "Delete All";
         this.m_DeleteAllApplicationsMenuItem.Click += new System.EventHandler(this.OnDeleteAllApplications);
         // 
         // usersMenuItem
         // 
         usersMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_CreateUserMenuItem,
            this.m_UpdateUserMenuItem,
            this.m_DeleteUserMenuItem,
            this.m_DeleteAllUsersMenuItem,
            usersSeparator1,
            this.m_ChangePasswordMenuItem,
            this.m_ResetPasswordMenuItem,
            usersSeparator2,
            this.m_RefreshUsersStatusMenuItem});
         usersMenuItem.Name = "usersMenuItem";
         usersMenuItem.Text = "Users";
         // 
         // m_CreateUserMenuItem
         // 
         this.m_CreateUserMenuItem.Image = CredentialsManagerClient.Properties.Resources.CreateIUser;
         this.m_CreateUserMenuItem.Name = "m_CreateUserMenuItem";
         this.m_CreateUserMenuItem.Text = "Create User";
         this.m_CreateUserMenuItem.Click += new System.EventHandler(this.OnCreateUser);
         // 
         // m_UpdateUserMenuItem
         // 
         this.m_UpdateUserMenuItem.Image = CredentialsManagerClient.Properties.Resources.Update;
         this.m_UpdateUserMenuItem.Name = "m_UpdateUserMenuItem";
         this.m_UpdateUserMenuItem.Text = "Update User";
         this.m_UpdateUserMenuItem.Click += new System.EventHandler(this.OnUpdateUser);
         // 
         // m_DeleteUserMenuItem
         // 
         this.m_DeleteUserMenuItem.Image = CredentialsManagerClient.Properties.Resources.DeleteUser;
         this.m_DeleteUserMenuItem.Name = "m_DeleteUserMenuItem";
         this.m_DeleteUserMenuItem.Text = "Delete User";
         this.m_DeleteUserMenuItem.Click += new System.EventHandler(this.OnDeleteUser);
         // 
         // m_DeleteAllUsersMenuItem
         // 
         this.m_DeleteAllUsersMenuItem.Image = CredentialsManagerClient.Properties.Resources.DeleteAllUsers;
         this.m_DeleteAllUsersMenuItem.Name = "m_DeleteAllUsersMenuItem";
         this.m_DeleteAllUsersMenuItem.Text = "Delete All Users";
         this.m_DeleteAllUsersMenuItem.Click += new System.EventHandler(this.OnDeleteAllUsers);
         // 
         // usersSeparator1
         // 
         usersSeparator1.Name = "usersSeparator1";
         // 
         // m_ChangePasswordMenuItem
         // 
         this.m_ChangePasswordMenuItem.Image = CredentialsManagerClient.Properties.Resources.ChangePassword;
         this.m_ChangePasswordMenuItem.Name = "m_ChangePasswordMenuItem";
         this.m_ChangePasswordMenuItem.Text = "Change Password";
         this.m_ChangePasswordMenuItem.Click += new System.EventHandler(this.OnChangePassword);
         // 
         // m_ResetPasswordMenuItem
         // 
         this.m_ResetPasswordMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("m_ResetPasswordMenuItem.Image")));
         this.m_ResetPasswordMenuItem.Name = "m_ResetPasswordMenuItem";
         this.m_ResetPasswordMenuItem.Text = "Reset Password";
         this.m_ResetPasswordMenuItem.Click += new System.EventHandler(this.OnResetPassword);
         // 
         // usersSeparator2
         // 
         usersSeparator2.Name = "usersSeparator2";
         // 
         // m_RefreshUsersStatusMenuItem
         // 
         this.m_RefreshUsersStatusMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("m_RefreshUsersStatusMenuItem.Image")));
         this.m_RefreshUsersStatusMenuItem.Name = "m_RefreshUsersStatusMenuItem";
         this.m_RefreshUsersStatusMenuItem.Text = "Refresh Users Status";
         this.m_RefreshUsersStatusMenuItem.Click += new System.EventHandler(this.OnUserStatusRefresh);
         // 
         // rolesMenuItem
         // 
         rolesMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_CreateRoleMenuItem,
            this.m_DeleteRoleMenuItem,
            this.m_DeleteAllRolesMenuItem,
            rolesSeparator1,
            this.m_AssignUsertoRoleMenuItem,
            this.m_RemoveUserFromRoleMenuItem,
            this.m_RemoveUserFromAllRolesMenuItem});
         rolesMenuItem.Name = "rolesMenuItem";
         rolesMenuItem.Text = "Roles";
         // 
         // m_CreateRoleMenuItem
         // 
         this.m_CreateRoleMenuItem.Image = CredentialsManagerClient.Properties.Resources.CreateIRole;
         this.m_CreateRoleMenuItem.Name = "m_CreateRoleMenuItem";
         this.m_CreateRoleMenuItem.Text = "Create Role";
         this.m_CreateRoleMenuItem.Click += new System.EventHandler(this.OnCreateRole);
         // 
         // m_DeleteRoleMenuItem
         // 
         this.m_DeleteRoleMenuItem.Image = CredentialsManagerClient.Properties.Resources.DeleteRole;
         this.m_DeleteRoleMenuItem.Name = "m_DeleteRoleMenuItem";
         this.m_DeleteRoleMenuItem.Text = "Delete Role";
         this.m_DeleteRoleMenuItem.Click += new System.EventHandler(this.OnDeleteRole);
         // 
         // m_DeleteAllRolesMenuItem
         // 
         this.m_DeleteAllRolesMenuItem.Image = CredentialsManagerClient.Properties.Resources.DeleteAllRoles;
         this.m_DeleteAllRolesMenuItem.Name = "m_DeleteAllRolesMenuItem";
         this.m_DeleteAllRolesMenuItem.Text = "Delete All Roles";
         this.m_DeleteAllRolesMenuItem.Click += new System.EventHandler(this.OnDeleteAllRoles);
         // 
         // rolesSeparator1
         // 
         rolesSeparator1.Name = "rolesSeparator1";
         // 
         // m_AssignUsertoRoleMenuItem
         // 
         this.m_AssignUsertoRoleMenuItem.Image = CredentialsManagerClient.Properties.Resources.Assign;
         this.m_AssignUsertoRoleMenuItem.Name = "m_AssignUsertoRoleMenuItem";
         this.m_AssignUsertoRoleMenuItem.Text = "Assign User to Role";
         this.m_AssignUsertoRoleMenuItem.Click += new System.EventHandler(this.OnAssignUserToRole);
         // 
         // m_RemoveUserFromRoleMenuItem
         // 
         this.m_RemoveUserFromRoleMenuItem.Image = CredentialsManagerClient.Properties.Resources.Remove;
         this.m_RemoveUserFromRoleMenuItem.Name = "m_RemoveUserFromRoleMenuItem";
         this.m_RemoveUserFromRoleMenuItem.Text = "Remove User from Role";
         this.m_RemoveUserFromRoleMenuItem.Click += new System.EventHandler(this.OnRemoveUserFromRole);
         // 
         // m_RemoveUserFromAllRolesMenuItem
         // 
         this.m_RemoveUserFromAllRolesMenuItem.Image = CredentialsManagerClient.Properties.Resources.RemoveAll;
         this.m_RemoveUserFromAllRolesMenuItem.Name = "m_RemoveUserFromAllRolesMenuItem";
         this.m_RemoveUserFromAllRolesMenuItem.Text = "Remove User from All Roles";
         this.m_RemoveUserFromAllRolesMenuItem.Click += new System.EventHandler(this.OnRemoveUsersFromAllRoles);
         // 
         // passwordsMenuItem
         // 
         passwordsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_GeneratePasswordMenuItem});
         passwordsMenuItem.Name = "passwordsMenuItem";
         passwordsMenuItem.Text = "Passwords";
         // 
         // m_GeneratePasswordMenuItem
         // 
         this.m_GeneratePasswordMenuItem.Image = CredentialsManagerClient.Properties.Resources.GeneratePassword;
         this.m_GeneratePasswordMenuItem.Name = "m_GeneratePasswordMenuItem";
         this.m_GeneratePasswordMenuItem.Text = "Generate Password";
         this.m_GeneratePasswordMenuItem.Click += new System.EventHandler(this.OnGeneratePassword);
         // 
         // serviceMenuItem
         // 
         serviceMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_ViewMenuItem,
            this.m_SelectMenuItem});
         serviceMenuItem.Name = "serviceMenuItem";
         serviceMenuItem.Text = "Service";
         serviceMenuItem.Click += new System.EventHandler(this.OnViewService);
         // 
         // m_ViewMenuItem
         // 
         this.m_ViewMenuItem.Image = CredentialsManagerClient.Properties.Resources.Service;
         this.m_ViewMenuItem.Name = "m_ViewMenuItem";
         this.m_ViewMenuItem.Text = "View";
         // 
         // m_SelectMenuItem
         // 
         this.m_SelectMenuItem.Image = CredentialsManagerClient.Properties.Resources.SelectService;
         this.m_SelectMenuItem.Name = "m_SelectMenuItem";
         this.m_SelectMenuItem.Text = "Select";
         // 
         // testMenuItem
         // 
         testMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_LogOnMenuItem,
            this.m_AuthorizeMenuItem});
         testMenuItem.Name = "testMenuItem";
         testMenuItem.Text = "Test";
         // 
         // m_LogOnMenuItem
         // 
         this.m_LogOnMenuItem.Image = CredentialsManagerClient.Properties.Resources.Authenticate;
         this.m_LogOnMenuItem.Name = "m_LogOnMenuItem";
         this.m_LogOnMenuItem.Text = "Authenticate";
         this.m_LogOnMenuItem.Click += new System.EventHandler(this.OnAuthenticate);
         // 
         // m_AuthorizeMenuItem
         // 
         this.m_AuthorizeMenuItem.Image = CredentialsManagerClient.Properties.Resources.Authorize;
         this.m_AuthorizeMenuItem.Name = "m_AuthorizeMenuItem";
         this.m_AuthorizeMenuItem.Text = "Authorize";
         this.m_AuthorizeMenuItem.Click += new System.EventHandler(this.OnAuthorize);
         // 
         // helpMenuItem
         // 
         helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpContentMenuItem,
            aboutMenuItem});
         helpMenuItem.Name = "helpMenuItem";
         helpMenuItem.Text = "Help";
         // 
         // helpContentMenuItem
         // 
         this.helpContentMenuItem.Name = "helpContentMenuItem";
         this.helpContentMenuItem.Text = "Content";
         this.helpContentMenuItem.Click += new System.EventHandler(this.Content);
         // 
         // aboutMenuItem
         // 
         aboutMenuItem.Name = "aboutMenuItem";
         aboutMenuItem.Text = "About";
         aboutMenuItem.Click += new System.EventHandler(this.OnAbout);
         // 
         // passwordsPage
         // 
         passwordsPage.Controls.Add(passwordSetupGroupBox);
         passwordsPage.Controls.Add(generatePassorgGroupBox);
         passwordsPage.Location = new System.Drawing.Point(4,22);
         passwordsPage.Name = "passwordsPage";
         passwordsPage.Size = new System.Drawing.Size(534,387);
         passwordsPage.TabIndex = 3;
         passwordsPage.Text = "Passwords";
         // 
         // rolesPage
         // 
         rolesPage.Controls.Add(usersGroupBox);
         rolesPage.Controls.Add(rolesGroupBox);
         rolesPage.Location = new System.Drawing.Point(4,22);
         rolesPage.Name = "rolesPage";
         rolesPage.Size = new System.Drawing.Size(534,387);
         rolesPage.TabIndex = 2;
         rolesPage.Text = "Roles";
         // 
         // usersPage
         // 
         usersPage.Controls.Add(usersStatus);
         usersPage.Controls.Add(usersGoupBox);
         usersPage.Location = new System.Drawing.Point(4,22);
         usersPage.Name = "usersPage";
         usersPage.Padding = new System.Windows.Forms.Padding(3);
         usersPage.Size = new System.Drawing.Size(534,387);
         usersPage.TabIndex = 1;
         usersPage.Text = "Users";
         // 
         // applicationsTab
         // 
         applicationsTab.Controls.Add(applicationPictureBox);
         applicationsTab.Controls.Add(applicationsGroupBox);
         applicationsTab.Location = new System.Drawing.Point(4,22);
         applicationsTab.Name = "applicationsTab";
         applicationsTab.Padding = new System.Windows.Forms.Padding(3);
         applicationsTab.Size = new System.Drawing.Size(534,387);
         applicationsTab.TabIndex = 0;
         applicationsTab.Text = "Applications";
         // 
         // applicationPictureBox
         // 
         applicationPictureBox.AutoSize = true;
         applicationPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         applicationPictureBox.Image = CredentialsManagerClient.Properties.Resources.Security;
         applicationPictureBox.Location = new System.Drawing.Point(447,15);
         applicationPictureBox.Name = "applicationPictureBox";
         applicationPictureBox.Size = new System.Drawing.Size(79,79);
         applicationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         applicationPictureBox.TabIndex = 12;
         applicationPictureBox.TabStop = false;
         // 
         // tabControl
         // 
         tabControl.Controls.Add(applicationsTab);
         tabControl.Controls.Add(usersPage);
         tabControl.Controls.Add(rolesPage);
         tabControl.Controls.Add(passwordsPage);
         tabControl.Controls.Add(servicePage);
         tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
         tabControl.Location = new System.Drawing.Point(0,24);
         tabControl.Name = "tabControl";
         tabControl.SelectedIndex = 0;
         tabControl.Size = new System.Drawing.Size(542,413);
         tabControl.TabIndex = 0;
         // 
         // servicePage
         // 
         servicePage.Controls.Add(addressGroupBox);
         servicePage.Controls.Add(this.m_AddressLabel);
         servicePage.Location = new System.Drawing.Point(4,22);
         servicePage.Name = "servicePage";
         servicePage.Size = new System.Drawing.Size(534,387);
         servicePage.TabIndex = 4;
         servicePage.Text = "Credentials Service";
         // 
         // m_AddressLabel
         // 
         this.m_AddressLabel.AutoSize = true;
         this.m_AddressLabel.Location = new System.Drawing.Point(8,11);
         this.m_AddressLabel.Name = "m_AddressLabel";
         this.m_AddressLabel.Size = new System.Drawing.Size(0,0);
         this.m_AddressLabel.TabIndex = 2;
         // 
         // columnHeader1
         // 
         this.columnHeader1.Text = "";
         this.columnHeader1.Width = 186;
         // 
         // CredentialsManagerForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
         this.ClientSize = new System.Drawing.Size(542,437);
         this.Controls.Add(tabControl);
         this.Controls.Add(mainMenu);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MainMenuStrip = mainMenu;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "CredentialsManagerForm";
         this.Text = " IDesign ASP.NET Credentials Manager";
         this.Load += new System.EventHandler(this.OnLoad);
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
         passwordSetupGroupBox.ResumeLayout(false);
         passwordSetupGroupBox.PerformLayout();
         generatePassorgGroupBox.ResumeLayout(false);
         generatePassorgGroupBox.PerformLayout();
         usersGroupBox.ResumeLayout(false);
         usersGroupBox.PerformLayout();
         rolesGroupBox.ResumeLayout(false);
         rolesGroupBox.PerformLayout();
         usersStatus.ResumeLayout(false);
         usersStatus.PerformLayout();
         usersGoupBox.ResumeLayout(false);
         usersGoupBox.PerformLayout();
         applicationsGroupBox.ResumeLayout(false);
         addressGroupBox.ResumeLayout(false);
         addressGroupBox.PerformLayout();
         mainMenu.ResumeLayout(false);
         passwordsPage.ResumeLayout(false);
         rolesPage.ResumeLayout(false);
         usersPage.ResumeLayout(false);
         applicationsTab.ResumeLayout(false);
         applicationsTab.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(applicationPictureBox)).EndInit();
         tabControl.ResumeLayout(false);
         servicePage.ResumeLayout(false);
         servicePage.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ToolStripMenuItem m_CreateApplicationMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_DeleteApplicationMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_CreateUserMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_DeleteUserMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_UpdateUserMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_CreateRoleMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_DeleteRoleMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_GeneratePasswordMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_LogOnMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_AuthorizeMenuItem;
      private System.Windows.Forms.Label m_AddressLabel;
      private System.Windows.Forms.TextBox m_AddressTextbox;
      private System.Windows.Forms.Button m_CreateApplicationButton;
      private System.Windows.Forms.Button m_DeleteApplicationButton;
      private System.Windows.Forms.Button m_CreateUserButton;
      private System.Windows.Forms.Button m_DeleteUserButton;
      private System.Windows.Forms.Button m_UpdateUser;
      private System.Windows.Forms.Label m_UsersOnline;
      private System.Windows.Forms.Label m_OnlineTimeWindow;
      private System.Windows.Forms.Button m_UsersStatusRefresh;
      private System.Windows.Forms.Button m_DeleteRoleButton;
      private System.Windows.Forms.Button m_CreateRoleButton;
      private System.Windows.Forms.Button m_AssignButton;
      private System.Windows.Forms.Button m_RemoveUserFromRoleButton;
      private System.Windows.Forms.Label m_PasswordReset;
      private System.Windows.Forms.Label m_PasswordRetrieval;
      private System.Windows.Forms.Label m_MaxInvalidAttempts;
      private System.Windows.Forms.Label m_MinNonAlphanumeric;
      private System.Windows.Forms.Label m_MinLength;
      private System.Windows.Forms.Label m_AttemptWindow;
      private System.Windows.Forms.Label m_PasswordRegularExpression;
      private System.Windows.Forms.Label m_RequiresQuestionAndAnswerLabel;
      private System.Windows.Forms.Button m_GeneratePassword;
      private System.Windows.Forms.TextBox m_LengthTextBox;
      private System.Windows.Forms.TextBox m_NonAlphanumericTextBox;
      private System.Windows.Forms.Button m_DeleteAllUsersButton;
      private System.Windows.Forms.Button m_RemoveUserFromAllRolesButton;
      private System.Windows.Forms.ToolStripMenuItem m_DeleteAllUsersMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_AssignUsertoRoleMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_RemoveUserFromRoleMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_RemoveUserFromAllRolesMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_RefreshUsersStatusMenuItem;
      private System.Windows.Forms.Button m_DeleteAllApplicationsButton;
      private System.Windows.Forms.ToolStripMenuItem m_DeleteAllApplicationsMenuItem;
      private System.Windows.Forms.Button m_DeleteAllRolesButton;
      private System.Windows.Forms.ToolStripMenuItem m_DeleteAllRolesMenuItem;
      private System.Windows.Forms.ToolStripMenuItem m_ViewMenuItem;
      private System.Windows.Forms.WebBrowser m_WebBrowser;
      private System.Windows.Forms.CheckBox m_RelatedDataCheckBox;
      private System.Windows.Forms.CheckBox m_ThrowIfPopulatedCheckBox;
      private System.Windows.Forms.Label m_PopulatedLabel;
      private System.Windows.Forms.Button m_ViewButton;
      private System.Windows.Forms.ColumnHeader columnHeader1;    
      private Button m_ChangePasswordButton;
      private Button m_ResetPasswordButton;
      private ToolStripMenuItem m_ChangePasswordMenuItem;
      private ToolStripMenuItem m_ResetPasswordMenuItem;
      private ListViewEx m_UsersToAssignListView;
      private ListViewEx m_RolesListView;
      private ListViewEx m_UsersListView;
      private ListViewEx m_ApplicationListView;
      private ComboBoxEx m_RolesForUserComboBox;
      private ComboBoxEx m_UsersInRoleComboBox;
      private ToolStripMenuItem m_SelectMenuItem;
      private Button m_SelectButton;
      private ToolStripMenuItem helpContentMenuItem;

      System.Windows.Forms.GroupBox passwordSetupGroupBox;
      System.Windows.Forms.Label passwordResetLabel;
      System.Windows.Forms.Label passwordRetrievalLabel;
      System.Windows.Forms.Label requiresQuestionAndAnswerLabel;
      System.Windows.Forms.Label maxInvalidLabel;
      System.Windows.Forms.Label passwordRegularExpressionLabel;
      System.Windows.Forms.Label minNonAlpha;
      System.Windows.Forms.Label attemptWindowLabel;
      System.Windows.Forms.Label minLengthLabel;
      System.Windows.Forms.GroupBox generatePassorgGroupBox;
      System.Windows.Forms.Label nonAlphanumericLabel;
      System.Windows.Forms.Label lengthLabel;
      System.Windows.Forms.GroupBox usersGroupBox;
      System.Windows.Forms.ColumnHeader userToassignHeader;
      System.Windows.Forms.Label rolesForUserLabel;
      System.Windows.Forms.ColumnHeader usersToAssignHeader;
      System.Windows.Forms.GroupBox rolesGroupBox;
      System.Windows.Forms.ColumnHeader rolesHeader;
      System.Windows.Forms.Label usersInRoleLabel;
      System.Windows.Forms.GroupBox usersStatus;
      System.Windows.Forms.Label onlineTimeWindowLabel;
      System.Windows.Forms.Label onlineUsersLabel;
      System.Windows.Forms.GroupBox usersGoupBox;
      System.Windows.Forms.ColumnHeader usersHeader;
      System.Windows.Forms.GroupBox applicationsGroupBox;
      System.Windows.Forms.ColumnHeader applicationsHeader;
      System.Windows.Forms.ColumnHeader columnApplications;
      System.Windows.Forms.GroupBox addressGroupBox;
      System.Windows.Forms.MenuStrip mainMenu;
      System.Windows.Forms.ToolStripMenuItem applicationMenuItem;
      System.Windows.Forms.ToolStripMenuItem usersMenuItem;
      System.Windows.Forms.ToolStripSeparator usersSeparator1;
      System.Windows.Forms.ToolStripSeparator usersSeparator2;
      System.Windows.Forms.ToolStripMenuItem rolesMenuItem;
      System.Windows.Forms.ToolStripSeparator rolesSeparator1;
      System.Windows.Forms.ToolStripMenuItem passwordsMenuItem;
      System.Windows.Forms.ToolStripMenuItem serviceMenuItem;
      System.Windows.Forms.ToolStripMenuItem testMenuItem;
      System.Windows.Forms.ToolStripMenuItem helpMenuItem;
      System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
      System.Windows.Forms.TabPage passwordsPage;
      System.Windows.Forms.TabPage rolesPage;
      System.Windows.Forms.TabPage usersPage;
      System.Windows.Forms.TabPage applicationsTab;
      System.Windows.Forms.PictureBox applicationPictureBox;
      System.Windows.Forms.TabControl tabControl;
      System.Windows.Forms.TabPage servicePage;
   }
}

