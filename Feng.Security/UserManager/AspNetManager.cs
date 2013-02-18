using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web.Security;
using System.Diagnostics;

namespace Feng.UserManager
{
    /// <summary>
    /// 
    /// </summary>
    public class AspNetManager : IMembershipManager, IUserManager, IPasswordManager, IApplicationManager, IRoleManager
    {
        /// <summary>
        /// 
        /// </summary>
        public AspNetManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }

        private static T[] ToArray<T>(IEnumerable collection, Converter<object, T> converter, int count)
        {
            List<T> list = new List<T>(count);
            foreach (object item in collection)
            {
                list.Add(converter(item));
            }
            return list.ToArray();
        }

        private static T[] ToArray<T>(IEnumerable collection, Converter<object, T> converter)
        {
            int count = 0;
            foreach (object item in collection)
            {
                count++;
            }
            return ToArray(collection, converter, count);
        }

        string[] IApplicationManager.GetApplications()
        {
            UserManager.AspNetDbDataSet dataSet = new UserManager.AspNetDbDataSet();

            UserManager.AspNetDbDataSetTableAdapters.aspnet_ApplicationsTableAdapter adapter =
                new UserManager.AspNetDbDataSetTableAdapters.aspnet_ApplicationsTableAdapter();
            adapter.Fill(dataSet.aspnet_Applications);

            Converter<object, string> converter = delegate(object obj)
                                                  {
                                                      UserManager.AspNetDbDataSet.aspnet_ApplicationsRow application =
                                                          obj as UserManager.AspNetDbDataSet.aspnet_ApplicationsRow;
                                                      return application.ApplicationName;
                                                  };
            string[] applications = ToArray(dataSet.aspnet_Applications, converter);
            if (applications == null)
            {
                applications = new string[] {};
            }
            return applications;
        }

        void IApplicationManager.DeleteAllApplications()
        {
            AspNetDbTablesAdapter aspNetDbTablesAdapter = new AspNetDbTablesAdapter();
            aspNetDbTablesAdapter.DeleteAllApplications();
        }

        void IApplicationManager.DeleteApplication(string application)
        {
            AspNetDbTablesAdapter aspNetDbTablesAdapter = new AspNetDbTablesAdapter();
            aspNetDbTablesAdapter.DeleteApplication(application);
        }

        void IMembershipManager.DeleteAllUsers(string application, bool deleteAllRelatedData)
        {
            IMembershipManager membershipManager = this;
            string[] users = membershipManager.GetAllUsers(application);

            Action<string> deleteUser =
                delegate(string user) { membershipManager.DeleteUser(application, user, deleteAllRelatedData); };
            Array.ForEach(users, deleteUser);
        }

        MembershipCreateStatus IMembershipManager.CreateUser(string application, string userName, string password,
                                                             string email, string passwordQuestion,
                                                             string passwordAnswer, bool isApproved)
        {
            System.Web.Security.MembershipCreateStatus status = System.Web.Security.MembershipCreateStatus.UserRejected;

            Membership.ApplicationName = application;
            Membership.CreateUser(userName, password, email, passwordQuestion, passwordAnswer, isApproved, out status);

            return (MembershipCreateStatus) status;
        }

        bool IMembershipManager.DeleteUser(string application, string userName, bool deleteAllRelatedData)
        {
            Membership.ApplicationName = application;
            return Membership.DeleteUser(userName, deleteAllRelatedData);
        }

        void IMembershipManager.UpdateUser(string application, string userName, string email, string oldAnswer,
                                           string newQuestion, string newAnswer, bool isApproved, bool isLockedOut)
        {
            Membership.ApplicationName = application;
            MembershipUser membershipUser = Membership.GetUser(userName);

            if (isLockedOut == false)
            {
                membershipUser.UnlockUser();
            }

            if (Membership.EnablePasswordRetrieval)
            {
                string password = membershipUser.GetPassword(oldAnswer);
                membershipUser.ChangePasswordQuestionAndAnswer(password, newQuestion, newAnswer);
            }

            membershipUser.Email = email;
            membershipUser.IsApproved = isApproved;
            Membership.UpdateUser(membershipUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="newQuestion"></param>
        /// <param name="newAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="isLockedOut"></param>
        void IMembershipManager.UpdateUserWithPassword(string application, string userName, string email,
                                                       string password, string newQuestion, string newAnswer,
                                                       bool isApproved, bool isLockedOut)
        {
            Membership.ApplicationName = application;
            MembershipUser membershipUser = Membership.GetUser(userName);

            if (isLockedOut == false)
            {
                membershipUser.UnlockUser();
            }

            if (Membership.EnablePasswordRetrieval)
            {
                membershipUser.ChangePasswordQuestionAndAnswer(password, newQuestion, newAnswer);
            }

            membershipUser.Email = email;
            membershipUser.IsApproved = isApproved;
            Membership.UpdateUser(membershipUser);
        }

        UserInfo IMembershipManager.GetUserInfo(string application, string userName)
        {
            Membership.ApplicationName = application;
            MembershipUser membershipUser = Membership.GetUser(userName);

            return new UserInfo(membershipUser.UserName, membershipUser.Email, membershipUser.PasswordQuestion,
                                membershipUser.IsApproved, membershipUser.IsLockedOut);
        }

        string[] IMembershipManager.GetAllUsers(string application)
        {
            Membership.ApplicationName = application;
            MembershipUserCollection collection = Membership.GetAllUsers();
            Converter<object, string> converter = delegate(object obj)
                                                  {
                                                      MembershipUser membershipUser = obj as MembershipUser;
                                                      return membershipUser.UserName;
                                                  };
            return ToArray(collection, converter);
        }

        int IMembershipManager.GetNumberOfUsersOnline(string application)
        {
            Membership.ApplicationName = application;
            return Membership.GetNumberOfUsersOnline();
        }

        int IMembershipManager.UserIsOnlineTimeWindow(string application)
        {
            Membership.ApplicationName = application;
            return Membership.UserIsOnlineTimeWindow;
        }

        string IMembershipManager.GetUserNameByEmail(string application, string email)
        {
            Membership.ApplicationName = application;
            return Membership.GetUserNameByEmail(email);
        }

        bool IPasswordManager.EnablePasswordReset(string application)
        {
            Membership.ApplicationName = application;
            return Membership.EnablePasswordReset;
        }

        string IPasswordManager.ResetPassword(string application, string userName)
        {
            Membership.ApplicationName = application;
            if (Membership.EnablePasswordReset && !Membership.RequiresQuestionAndAnswer)
            {
                MembershipUser membershipUser = Membership.GetUser(userName);
                return membershipUser.ResetPassword();
            }
            return String.Empty;
        }

        string IPasswordManager.ResetPasswordWithQuestionAndAnswer(string application, string userName,
                                                                   string passwordAnswer)
        {
            Membership.ApplicationName = application;
            if (Membership.EnablePasswordReset)
            {
                MembershipUser membershipUser = Membership.GetUser(userName);
                return membershipUser.ResetPassword(passwordAnswer);
            }
            return String.Empty;
        }

        string IPasswordManager.GetPassword(string application, string userName, string passwordAnswer)
        {
            Membership.ApplicationName = application;
            Debug.Assert(Membership.EnablePasswordRetrieval);

            MembershipUser membershipUser = Membership.GetUser(userName);
            return membershipUser.GetPassword(passwordAnswer);
        }

        string IPasswordManager.GetPasswordQuestion(string application, string userName)
        {
            Membership.ApplicationName = application;
            MembershipUser membershipUser = Membership.GetUser(userName);

            return membershipUser.PasswordQuestion;
        }

        void IPasswordManager.ChangePassword(string application, string userName, string newPassword)
        {
            Membership.ApplicationName = application;
            Debug.Assert(Membership.EnablePasswordRetrieval && !Membership.RequiresQuestionAndAnswer);

            MembershipUser membershipUser = Membership.GetUser(userName);
            membershipUser.ChangePassword(membershipUser.GetPassword(), newPassword);
        }

        void IPasswordManager.ChangePasswordWithAnswer(string application, string userName, string passwordAnswer,
                                                       string newPassword)
        {
            Membership.ApplicationName = application;
            Debug.Assert(Membership.EnablePasswordRetrieval);

            MembershipUser membershipUser = Membership.GetUser(userName);
            membershipUser.ChangePassword(membershipUser.GetPassword(passwordAnswer), newPassword);
        }

        bool IPasswordManager.EnablePasswordRetrieval(string application)
        {
            Membership.ApplicationName = application;
            return Membership.EnablePasswordRetrieval;
        }

        string IPasswordManager.GeneratePassword(string application, int length, int numberOfNonAlphanumericCharacters)
        {
            Membership.ApplicationName = application;
            return Membership.GeneratePassword(length, numberOfNonAlphanumericCharacters);
        }

        int IPasswordManager.GetMaxInvalidPasswordAttempts(string application)
        {
            Membership.ApplicationName = application;
            return Membership.MaxInvalidPasswordAttempts;
        }

        int IPasswordManager.GetMinRequiredNonAlphanumericCharacters(string application)
        {
            Membership.ApplicationName = application;
            return Membership.MinRequiredNonAlphanumericCharacters;
        }

        int IPasswordManager.GetMinRequiredPasswordLength(string application)
        {
            Membership.ApplicationName = application;
            return Membership.MinRequiredPasswordLength;
        }

        int IPasswordManager.GetPasswordAttemptWindow(string application)
        {
            Membership.ApplicationName = application;
            return Membership.PasswordAttemptWindow;
        }

        string IPasswordManager.GetPasswordStrengthRegularExpression(string application)
        {
            Membership.ApplicationName = application;
            return Membership.PasswordStrengthRegularExpression;
        }

        bool IPasswordManager.RequiresQuestionAndAnswer(string application)
        {
            Membership.ApplicationName = application;
            return Membership.RequiresQuestionAndAnswer;
        }

        void IRoleManager.DeleteAllRoles(string application, bool throwOnPopulatedRole)
        {
            IRoleManager roleManager = this;
            string[] roles = roleManager.GetAllRoles(application);

            Action<string> deleteRole =
                delegate(string role) { roleManager.DeleteRole(application, role, throwOnPopulatedRole); };
            Array.ForEach(roles, deleteRole);
        }

        string[] IRoleManager.GetAllRoles(string application)
        {
            Roles.ApplicationName = application;
            return Roles.GetAllRoles();
        }

        string[] IRoleManager.GetRolesForUser(string application, string userName)
        {
            Roles.ApplicationName = application;
            return Roles.GetRolesForUser(userName);
        }

        string[] IRoleManager.GetUsersInRole(string application, string role)
        {
            Roles.ApplicationName = application;
            return Roles.GetUsersInRole(role);
        }

        bool IRoleManager.RoleExists(string application, string role)
        {
            Roles.ApplicationName = application;
            return Roles.RoleExists(role);
        }

        void IRoleManager.AddUsersToRole(string application, string[] userNames, string role)
        {
            Roles.ApplicationName = application;
            Roles.AddUsersToRole(userNames, role);
        }

        void IRoleManager.AddUsersToRoles(string application, string[] userNames, string[] roles)
        {
            Roles.ApplicationName = application;
            Roles.AddUsersToRoles(userNames, roles);
        }

        void IRoleManager.AddUserToRole(string application, string userName, string role)
        {
            Roles.ApplicationName = application;
            Roles.AddUserToRole(userName, role);
        }

        void IRoleManager.AddUserToRoles(string application, string userName, string[] roles)
        {
            Roles.ApplicationName = application;
            Roles.AddUserToRoles(userName, roles);
        }

        void IRoleManager.CreateRole(string application, string role)
        {
            Roles.ApplicationName = application;
            Roles.CreateRole(role);
        }

        bool IRoleManager.DeleteRole(string application, string role, bool throwOnPopulatedRole)
        {
            Roles.ApplicationName = application;
            return Roles.DeleteRole(role, throwOnPopulatedRole);
        }

        bool IRoleManager.IsRolesEnabled(string application)
        {
            Roles.ApplicationName = application;
            return Roles.Enabled;
        }

        void IRoleManager.RemoveUserFromRole(string application, string userName, string roleName)
        {
            Roles.ApplicationName = application;
            Roles.RemoveUserFromRole(userName, roleName);
        }

        void IRoleManager.RemoveUserFromRoles(string application, string userName, string[] roles)
        {
            Roles.ApplicationName = application;
            Roles.RemoveUserFromRoles(userName, roles);
        }

        void IRoleManager.RemoveUsersFromRole(string application, string[] users, string roleName)
        {
            Roles.ApplicationName = application;
            Roles.RemoveUsersFromRole(users, roleName);
        }

        void IRoleManager.RemoveUsersFromRoles(string application, string[] users, string[] roles)
        {
            Roles.ApplicationName = application;
            Roles.RemoveUsersFromRoles(users, roles);
        }

        bool IUserManager.Authenticate(string application, string userName, string password)
        {
            Membership.ApplicationName = application;
            return Membership.ValidateUser(userName, password);
        }

        bool IUserManager.IsInRole(string application, string userName, string role)
        {
            Roles.ApplicationName = application;
            return Roles.IsUserInRole(userName, role);
        }

        string[] IUserManager.GetRoles(string application, string userName)
        {
            IRoleManager roleManager = this;
            return roleManager.GetRolesForUser(application, userName);
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        bool IPasswordManager.ChangePasswordWithOldPassword(string application, string userName, string oldPassword,
                                                            string newPassword)
        {
            MembershipUser user = Membership.GetUser(userName);
            if (user != null)
            {
                return user.ChangePassword(oldPassword, newPassword);
            }
            else
            {
                return false;
            }
        }
    }
}