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
   partial class CreateRoleDialog : Form
   {
      string m_Url;
      string m_Application;
      List<string> m_Roles = new List<string>();

      public string[] Roles
      {
         get
         {
            return m_Roles.ToArray();
         }
         set
         {
            m_Roles = new List<string>(value);
         }
      }

      public CreateRoleDialog(string url,string application)
      {
         InitializeComponent();

         m_Url = url;
         m_Application = application;
         m_RoleTextBox.Focus();
         m_CreatedRolesListView.SmallImageList = new ImageList();
         m_CreatedRolesListView.SmallImageList.Images.Add(Resources.Role);
      }
      void OnCreateRole(object sender,EventArgs e)
      {
          IRoleManager roleManager = UserManagerProviderFactory.CreateRoleManager(); // new AspNetSqlProviderService(m_Url);
         string[] roles = roleManager.GetAllRoles(m_Application);
         Predicate<string> exists = delegate(string role)
                                    {
                                       return role == m_RoleTextBox.Text;
                                    };
         if(Array.Exists(roles,exists))
         {
            m_RoleValidator.SetError(m_RoleTextBox,"Role already exists");
            return;
         }
         m_RoleValidator.Clear();
         if(m_RoleTextBox.Text == String.Empty)
         {
            m_RoleValidator.SetError(m_RoleTextBox,"Role cannot be empty");
            return;
         }
         m_RoleValidator.Clear();
         roleManager.CreateRole(m_Application,m_RoleTextBox.Text);
         m_Roles.Add(m_RoleTextBox.Text);
         m_CreatedRolesListView.AddItem(m_RoleTextBox.Text,true);
         m_RoleTextBox.Focus();
         m_RoleTextBox.Text = String.Empty;
      }
      void OnClosed(object sender,EventArgs e)
      {
         Close();
      }
   }
}