// ?2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CredentialsManagerClient.Properties;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.Threading;
using Feng.UserManager;

namespace CredentialsManagerClient
{
   partial class CredentialsManagerForm 
   {
      bool m_EnablePasswordReset;
      bool m_EnablePasswordRetrieval;
      bool m_RequiresQuestionAndAnswer;
      ManualResetEvent m_DownloadCompletedEvent;
      string m_ServiceAddress;

      bool RequiresQuestionAndAnswer
      {
         get
         {
            return m_RequiresQuestionAndAnswer;
         }
         set
         {
            m_RequiresQuestionAndAnswer = value;
         }
      }
      bool EnablePasswordReset
      {
         get
         {
            return m_EnablePasswordReset;
         }
         set
         {
            m_EnablePasswordReset = value;
         }
      }
      bool EnablePasswordRetrieval
      {
         get
         {
            return m_EnablePasswordRetrieval;
         }
         set
         {
            m_EnablePasswordRetrieval = value;
         }
      }
            
      string ServiceAddress
      {
         get
         {
            return m_ServiceAddress;
         }
         set
         {
            if(! value.StartsWith("http://"))
            {
               value = "http://" + value;
            }
            m_ServiceAddress = value;
         }
      }

      internal string m_specialApplicationName;
      string ApplicationName
      {
          get
          {
              if (string.IsNullOrEmpty(m_specialApplicationName))
                  return m_ApplicationListView.CurrentListViewItem;
              else
                  return m_specialApplicationName;
          }
      }
      string UserName
      {
         get
         {
            return m_UsersListView.CurrentListViewItem;
         }
      }
      string UserToAssign
      {
         get
         {
            return m_UsersToAssignListView.CurrentListViewItem;
         }
      }
      string RoleName
      {
         get
         {
            return m_RolesListView.CurrentListViewItem;
         }
      }
      string[] Applications
      {
         get
         {
            return m_ApplicationListView.ToArray();
         }
      }
      string[] Users
      {
         get
         {
            return m_UsersListView.ToArray();
         }
      }
      string[] Roles
      {
         get
         {
            return m_UsersListView.ToArray();
         }
      }
      void RefreshApplicationsPage()
      {
         RefreshApplicationListView();
         RefreshApplicationButtons();
      }
      void RefreshUsersPage()
      {
         RefreshUsersListView();
         RefreshUserStatus();
         RefreshUsersPageButtonsAndMenuItems();
      }
      void RefreshApplicationButtons()
      {
         m_DeleteApplicationMenuItem.Enabled = m_DeleteApplicationButton.Enabled = m_ApplicationListView.Items.Count > 0;
         m_DeleteAllApplicationsMenuItem.Enabled = m_DeleteAllApplicationsButton.Enabled = m_DeleteApplicationButton.Enabled;
      }
      void RefreshUsersPageButtonsAndMenuItems()
      {
         m_CreateUserMenuItem.Enabled = m_CreateUserButton.Enabled = !(ApplicationName == String.Empty);
         m_UpdateUserMenuItem.Enabled = m_UpdateUser.Enabled       = m_UsersListView.Items.Count > 0;
         m_RelatedDataCheckBox.Enabled = m_DeleteUserMenuItem.Enabled = m_DeleteUserButton.Enabled = m_UpdateUser.Enabled;
         m_DeleteAllUsersMenuItem.Enabled = m_DeleteAllUsersButton.Enabled = m_UpdateUser.Enabled;

         m_RefreshUsersStatusMenuItem.Enabled = m_UsersStatusRefresh.Enabled = ApplicationName != String.Empty && m_UpdateUser.Enabled;

         m_ResetPasswordMenuItem.Enabled = m_ResetPasswordButton.Enabled = EnablePasswordReset && m_DeleteUserButton.Enabled;
         m_ChangePasswordMenuItem.Enabled = m_ChangePasswordButton.Enabled = EnablePasswordRetrieval && m_DeleteUserButton.Enabled;
      }
      void RefreshRolesPage()
      {
         RefreshRolesListView();
         RefreshUsersToAssignListView();
         RefreshRolesForUserComboBox();
         RefreshUsersForRoleComboBox();
         RefreshRolePageButtons();
      }
      void RefreshRolePageButtons()
      {
         m_AssignUsertoRoleMenuItem.Enabled = m_AssignButton.Enabled = m_UsersToAssignListView.HasSelection && m_RolesListView.HasSelection;
         m_RemoveUserFromRoleMenuItem.Enabled =  m_RemoveUserFromRoleButton.Enabled = m_RolesForUserComboBox.Enabled;
         m_RemoveUserFromAllRolesMenuItem.Enabled = m_RemoveUserFromAllRolesButton.Enabled = m_RolesForUserComboBox.Enabled;

         m_CreateRoleMenuItem.Enabled = m_CreateRoleButton.Enabled = !(ApplicationName == String.Empty);
         m_PopulatedLabel.Enabled = m_ThrowIfPopulatedCheckBox.Enabled = m_DeleteRoleMenuItem.Enabled = m_DeleteRoleButton.Enabled = m_RolesListView.Items.Count > 0;
         m_DeleteAllRolesMenuItem.Enabled = m_DeleteAllRolesButton.Enabled = m_DeleteRoleButton.Enabled;
      }
      void RefreshPasswordsPage()
      {
         if(ApplicationName == String.Empty)
         {
            m_PasswordReset.Text = "-";
            m_PasswordRetrieval.Text = "-";
            m_MaxInvalidAttempts.Text = "-";
            m_MinNonAlphanumeric.Text = "-";
            m_MinLength.Text = "-";
            m_AttemptWindow.Text = "-";
            m_PasswordRegularExpression.Text = "-";
            m_RequiresQuestionAndAnswerLabel.Text = "-";
            m_LengthTextBox.Text = String.Empty;
            m_NonAlphanumericTextBox.Text = String.Empty;
            m_GeneratePasswordMenuItem.Enabled = m_GeneratePassword.Enabled = false;
            return;
         }
         m_GeneratePasswordMenuItem.Enabled = m_GeneratePassword.Enabled = true;

         IPasswordManager passwordManager = UserManagerProviderFactory.CreatePasswordManager(); // new AspNetSqlProviderService(ServiceAddress);
         if(passwordManager.EnablePasswordReset(ApplicationName))
         {
            EnablePasswordReset = true;
            m_PasswordReset.Text = "Yes";
         }
         else
         {
            EnablePasswordReset = false;
            m_PasswordReset.Text = "No";
         }
         if(passwordManager.EnablePasswordRetrieval(ApplicationName))
         {
            EnablePasswordRetrieval = true;
            m_PasswordRetrieval.Text = "Yes";
         }
         else
         {
            EnablePasswordRetrieval = false;
            m_PasswordRetrieval.Text = "No";
         }
         m_MaxInvalidAttempts.Text = passwordManager.GetMaxInvalidPasswordAttempts(ApplicationName).ToString();
         m_MinNonAlphanumeric.Text = passwordManager.GetMinRequiredNonAlphanumericCharacters(ApplicationName).ToString();
         m_MinLength.Text = passwordManager.GetMinRequiredPasswordLength(ApplicationName).ToString();
         m_AttemptWindow.Text = passwordManager.GetPasswordAttemptWindow(ApplicationName).ToString();
         m_PasswordRegularExpression.Text = passwordManager.GetPasswordStrengthRegularExpression(ApplicationName);
         if(passwordManager.RequiresQuestionAndAnswer(ApplicationName))
         {
            RequiresQuestionAndAnswer = true;
            m_RequiresQuestionAndAnswerLabel.Text = "Yes";
         }
         else
         {
            RequiresQuestionAndAnswer = false;
            m_RequiresQuestionAndAnswerLabel.Text = "No";
         }

         m_LengthTextBox.Text = m_MinLength.Text;
         m_NonAlphanumericTextBox.Text = m_MinNonAlphanumeric.Text;
      }
      void RefreshServicePage()
      {
         if(ServiceAddress == String.Empty)
         {
            m_ViewMenuItem.Enabled = m_ViewButton.Enabled = false;
         }
         else
         {
            m_ViewMenuItem.Enabled = m_ViewButton.Enabled = true;
            OnViewService(this, EventArgs.Empty);
            RefreshSelectButton();
         }
     }
      void RefreshApplicationListView()
      {
         m_ApplicationListView.ClearItems();
         string[] applications = new string[] { };
         if(ValidAddress)
         {
             IApplicationManager applicationManager = UserManagerProviderFactory.CreateApplicationManager(); // new AspNetSqlProviderService(ServiceAddress);
            applications = applicationManager.GetApplications();
         }
         m_ApplicationListView.AddItems(applications,true);
         SelectedApplicationChanged();
      }
      void RefreshUsersListView()
      {
         m_UsersListView.ClearItems();
         if(ApplicationName == String.Empty)
         {
            return;
         }
         IMembershipManager membershipManager = UserManagerProviderFactory.CreateMembershipManager(); // new AspNetSqlProviderService(ServiceAddress);

         string[] users = membershipManager.GetAllUsers(ApplicationName);
         m_UsersListView.AddItems(users,true);
      }
      void RefreshUserStatus()
      {
         if(ApplicationName == String.Empty)
         {
            m_UsersOnline.Text = "-";
            m_OnlineTimeWindow.Text = "-";
            return;
         }
         IMembershipManager membershipManager = UserManagerProviderFactory.CreateMembershipManager(); // new AspNetSqlProviderService(ServiceAddress);

         m_UsersOnline.Text = membershipManager.GetNumberOfUsersOnline(ApplicationName).ToString();
         m_OnlineTimeWindow.Text = membershipManager.UserIsOnlineTimeWindow(ApplicationName).ToString();
      }
      void RefreshRolesListView()
      {
         m_RolesListView.ClearItems();
         if(ApplicationName == String.Empty)
         {
            return;
         }
         IRoleManager roleManager = UserManagerProviderFactory.CreateRoleManager(); // new AspNetSqlProviderService(ServiceAddress);
         string[] roles = roleManager.GetAllRoles(ApplicationName);

         m_RolesListView.AddItems(roles,true);
      }
      
      void RefreshUsersToAssignListView()
      {
         m_UsersToAssignListView.ClearItems();
         if(ApplicationName == String.Empty)
         {
            return;
         }
         m_UsersToAssignListView.AddItems(Users,true);
      }
      void RefreshRolesForUserComboBox()
      {
         string[] roles = null;
         if(String.IsNullOrEmpty(UserToAssign) == false)
         {
             IRoleManager roleManager = UserManagerProviderFactory.CreateRoleManager(); // new AspNetSqlProviderService(ServiceAddress);
            roles = roleManager.GetRolesForUser(ApplicationName,UserToAssign);
         }
         m_RolesForUserComboBox.RefreshComboBox(UserToAssign,roles);        
      }
      void RefreshUsersForRoleComboBox()
      {
         string[] users = null;
         
         if(String.IsNullOrEmpty(RoleName) == false)
         {
             IRoleManager roleManager = UserManagerProviderFactory.CreateRoleManager(); // new AspNetSqlProviderService(ServiceAddress);
            users = roleManager.GetUsersInRole(ApplicationName,RoleName);
         }
         m_UsersInRoleComboBox.RefreshComboBox(RoleName,users);
      }
      void RefreshSelectButton()
      {
         m_SelectMenuItem.Enabled = m_SelectButton.Enabled = false;

         if(ServiceAddress == String.Empty)
         {
            return;
         }
         if(!ServiceAddress.EndsWith("asmx?wsdl") && !ServiceAddress.EndsWith("asmx"))
         {
            return;
         }
         string content = m_WebBrowser.DocumentText;
         if(!ContainsInterface(content,typeof(IApplicationManager)))
         {
            return;
         }
         if(!ContainsInterface(content,typeof(IMembershipManager)))
         {
            return;
         }
         if(!ContainsInterface(content,typeof(IPasswordManager)))
         {
            return;
         }
         if(!ContainsInterface(content,typeof(IRoleManager)))
         {
            return;
         }
         if(!ContainsInterface(content,typeof(IUserManager)))
         {
            return;
         }
         m_SelectMenuItem.Enabled = m_SelectButton.Enabled = true;
      }
      bool ContainsInterface(string content,Type interfaceType)
      {
         if(! interfaceType.IsInterface)
         {
            return false;
         }
         MethodInfo[] methodInfos = interfaceType.GetMethods();
         Predicate<MethodInfo> contains = delegate(MethodInfo methodInfo)
                                          {
                                             return content.Contains(methodInfo.Name);
                                          };
         return Array.TrueForAll(methodInfos,contains);
      }
      bool ValidAddress
      {
         get
         {
             return true;
            //return m_SelectButton.Enabled;
         }
      }
   }
}