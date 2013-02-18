// ?2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Feng.UserManager;

namespace CredentialsManagerClient
{
   partial class UpdateUserDialog : Form
   {
      string m_Url;
      string m_Application;

      public UpdateUserDialog(string url,string application,string user)
      {
         InitializeComponent();

         m_Url = url;
         m_Application = application;
         m_UserNameTextBox.Text = user;

         IMembershipManager membershipManager = UserManagerProviderFactory.CreateMembershipManager(); // new AspNetSqlProviderService(m_Url);
         UserInfo info = membershipManager.GetUserInfo(m_Application,m_UserNameTextBox.Text);
         m_EmailTextBox.Text = info.Email;
         m_ActiveUserCheckbox.Checked = info.IsApproved;
         m_LcokedOutCheckBox.Checked = info.IsLockedOut;

         m_NewQuestionTextBox.Text = m_OldQuestionTextBox.Text = info.PasswordQuestion;

         IPasswordManager passwordManager = membershipManager as IPasswordManager;

         m_OldAnswerTextBox.Enabled = passwordManager.EnablePasswordRetrieval(application);
         m_NewQuestionTextBox.Enabled = m_NewAnswerTextBox.Enabled = m_OldAnswerTextBox.Enabled;
      }
      void OnUpdateUser(object sender,EventArgs e)
      {
          IMembershipManager membershipManager = UserManagerProviderFactory.CreateMembershipManager();// new AspNetSqlProviderService(m_Url);
         string[] users = membershipManager.GetAllUsers(m_Application);
         Predicate<string> exists = delegate(string user)
                                    {
                                       return user == m_UserNameTextBox.Text;
                                    };
         if(Array.Exists(users,exists) == false)
         {
            m_Validator.SetError(m_UserNameTextBox,"User name does not exist");
            return;
         }
         m_Validator.Clear();

         if(m_EmailTextBox.Text == String.Empty)
         {
            m_Validator.SetError(m_EmailTextBox,"Email cannot be empty");
            return;
         }
         m_Validator.Clear();
         if(m_OldAnswerTextBox.Text == String.Empty && m_OldAnswerTextBox.Enabled)
         {
            m_Validator.SetError(m_OldAnswerTextBox,"Old answer cannot be empty");
            return;
         }
         m_Validator.Clear();
         if(m_NewQuestionTextBox.Text == String.Empty && m_NewQuestionTextBox.Enabled)
         {
            m_Validator.SetError(m_NewQuestionTextBox,"New question cannot be empty");
            return;
         }
         m_Validator.Clear();
         if(m_NewAnswerTextBox.Text == String.Empty && m_NewAnswerTextBox.Enabled)
         {
            m_Validator.SetError(m_NewAnswerTextBox,"New answer cannot be empty");
            return;
         }
         m_Validator.Clear();

         membershipManager.UpdateUser(m_Application,m_UserNameTextBox.Text,m_EmailTextBox.Text,m_OldAnswerTextBox.Text,m_NewQuestionTextBox.Text,m_NewAnswerTextBox.Text,m_ActiveUserCheckbox.Checked,m_LcokedOutCheckBox.Checked);

         Close();
      }
   }
}