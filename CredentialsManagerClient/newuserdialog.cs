// ?2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using CredentialsManagerClient.Properties;
using Feng.UserManager;

namespace CredentialsManagerClient
{
   partial class CreateUserDialog : Form
   {
      string m_Url;
      string m_Application;
      List<string> m_Users = new List<string>();

      public string[] Users
      {
         get
         {
            return m_Users.ToArray();
         }
         set
         {
            m_Users = new List<string>(value);
         }
      }

      public CreateUserDialog(string url,string application)
      {
         InitializeComponent();

         m_Url = url;
         m_Application = application;
         m_CreatedUsersListView.SmallImageList = new ImageList();
         m_CreatedUsersListView.SmallImageList.Images.Add(Resources.User);
      }
      void OnCreateUser(object sender,EventArgs e)
      {
          IMembershipManager membershipManager = UserManagerProviderFactory.CreateMembershipManager(); // new AspNetSqlProviderService(m_Url);
         string[] users = membershipManager.GetAllUsers(m_Application);
         Predicate<string> exists = delegate(string user)
                                    {
                                       return user == m_UserNameTextBox.Text;
                                    };
         if(Array.Exists(users,exists))
         {
            m_Validator.SetError(m_UserNameTextBox,"User name already exists");
            return;
         }
         m_Validator.Clear();

         if(m_PasswordTextbox.Text == String.Empty)
         {
            m_Validator.SetError(m_PasswordTextbox,"Password cannot be empty");
            return;
         }
         m_Validator.Clear();

         if(m_PasswordTextbox.Text != m_ConfirmedPasswordTextBox.Text)
         {
            m_Validator.SetError(m_ConfirmedPasswordTextBox,"Confirmed password does not match");
            return;
         }
         m_Validator.Clear();

         if(m_UserNameTextBox.Text == String.Empty)
         {
            m_Validator.SetError(m_UserNameTextBox,"User name cannot be empty");
            return;
         }
         m_Validator.Clear();

         if(m_EmailTextBox.Text == String.Empty)
         {
            m_Validator.SetError(m_EmailTextBox,"Email cannot be empty");
            return;
         }
         m_Validator.Clear();

         if(m_SecurityQuestionTextBox.Text == String.Empty)
         {
            m_Validator.SetError(m_SecurityQuestionTextBox,"Security question cannot be empty");
            return;
         }
         m_Validator.Clear();

         if(m_SecurityAnswerTextbox.Text == String.Empty)
         {
            m_Validator.SetError(m_SecurityAnswerTextbox,"Security question cannot be empty");
            return;
         }
         m_Validator.Clear();

         MembershipCreateStatus status = membershipManager.CreateUser(m_Application,m_UserNameTextBox.Text,m_PasswordTextbox.Text,m_EmailTextBox.Text,m_SecurityQuestionTextBox.Text,m_SecurityAnswerTextbox.Text,m_ActiveUserCheckBox.Checked);
         if(status != MembershipCreateStatus.Success)
         {
            MessageBox.Show(status.ToString(),"Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
         }
         else
         {
            m_Users.Add(m_UserNameTextBox.Text);
            m_CreatedUsersListView.AddItem(m_UserNameTextBox.Text,true);
            m_UserNameTextBox.Text = String.Empty;
            m_UserNameTextBox.Focus();
            m_GeneratePasswordCheckBox.Checked = false;
            m_PasswordTextbox.Text = String.Empty;
            m_ConfirmedPasswordTextBox.Text = String.Empty;
         }
      }

      void OnClose(object sender,EventArgs e)
      {
         Close();
      }

      void OnCheckedChanged(object sender,EventArgs e)
      {
         if(m_GeneratePasswordCheckBox.Checked)
         {
             IPasswordManager passwordManager = UserManagerProviderFactory.CreatePasswordManager(); // new AspNetSqlProviderService(m_Url);
            int length = passwordManager.GetMinRequiredPasswordLength(m_Application);
            int nonAlphaNumeric = passwordManager.GetMinRequiredNonAlphanumericCharacters(m_Application);
            string password = passwordManager.GeneratePassword(m_Application,length,nonAlphaNumeric);
            m_PasswordTextbox.Text = password;
            m_ConfirmedPasswordTextBox.Text = password;

            m_PasswordTextbox.PasswordChar = '\0';
            m_ConfirmedPasswordTextBox.PasswordChar = '\0';
         }
         else
         {
            m_PasswordTextbox.PasswordChar = '*';
            m_ConfirmedPasswordTextBox.PasswordChar = '*';
         }
      }

      private void CreateUserDialog_Load(object sender,EventArgs e)
      {

      }
   }
}