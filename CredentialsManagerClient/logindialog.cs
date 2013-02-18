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
   public partial class LoginDialog : Form
   {
      string m_Url;

      public LoginDialog()
      {
         InitializeComponent();
      }
      public LoginDialog(string url,string application,string user) : this()
      {
         m_Url = url;
         m_ApplicationTextBox.Text = application;
         m_UserNameTextBox.Text = user;
      }

      void OnLogin(object sender,EventArgs e)
      {
          IUserManager userManager = UserManagerProviderFactory.CreateUserManager(); // new AspNetSqlProviderService(m_Url);
         bool authenticated = false;

         authenticated = userManager.Authenticate(m_ApplicationTextBox.Text,m_UserNameTextBox.Text,m_PasswordTextBox.Text);
         if(authenticated)
         {
            MessageBox.Show(this,"Successful Authentication","Credentials Manager",MessageBoxButtons.OK);
         }
         else
         {
            MessageBox.Show(this,"Incorrect Credentials","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Hand);
         }
      }

      void OnClose(object sender,EventArgs e)
      {
         Close();
      }
   }
}