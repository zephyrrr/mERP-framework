using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.UserManager;

namespace CredentialsManagerClient
{
   public partial class AuthorizationDialog : Form
   {
      string m_Url;

      public AuthorizationDialog()
      {
         InitializeComponent();
      }
      public AuthorizationDialog(string url,string application,string user) : this()
      {
         m_Url = url;
         m_ApplicationTextBox.Text = application;
         m_UserComboBox.Text = user;

         IRoleManager roleManager = UserManagerProviderFactory.CreateRoleManager();
         string[] roles = roleManager.GetRolesForUser(application,user);

         m_RoleComboBox.Items.AddRange(roles);
         if(roles.Length > 0)
         {
            m_RoleComboBox.Text = roles[roles.Length-1];
         }

         IMembershipManager membershipManager = roleManager as IMembershipManager;

         string[] users = membershipManager.GetAllUsers(application);

         m_UserComboBox.Items.AddRange(users);
         if(users.Length > 0)
         {
            m_UserComboBox.Text = users[users.Length-1];
         }
      }

      void OnLogin(object sender,EventArgs e)
      {
          IUserManager userManager = UserManagerProviderFactory.CreateUserManager(); // new AspNetSqlProviderService(m_Url);
         bool isInRole = false;

         isInRole = userManager.IsInRole(m_ApplicationTextBox.Text,m_UserComboBox.Text,m_RoleComboBox.Text);
         if(isInRole)
         {
            MessageBox.Show(this,"Successful Authorization","Credentials Manager",MessageBoxButtons.OK);
         }
         else
         {
            MessageBox.Show(this,"User is not a member of the specified role","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Hand);
         }
      }

      void OnClose(object sender,EventArgs e)
      {
         Close();
      }
   }
}