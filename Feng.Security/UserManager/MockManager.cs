using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.UserManager
{
    public class MockManager : IMembershipManager, IUserManager, IPasswordManager,
                                     IApplicationManager, IRoleManager
    {
        public MockManager(string userName, string password, string roles)
        {
            m_userName = userName;
            m_password = password;
            m_roles = roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            // SystemConfiguration.AdministratorsRoleName
        }

        private string m_userName, m_password;
        private string[] m_roles;

        /// <summary>
        /// ReAuthenticate
        /// </summary>
        public void ReAuthenticate()
        {
            Authenticate(SystemConfiguration.ApplicationName, SystemConfiguration.UserName, SystemConfiguration.Password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public string GetAppConfig(string applicationName)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public string GetFormsAuthenticationTicket()
        //{
        //    throw new InvalidOperationException("Invalid Operation in MockManager!");
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public MembershipCreateStatus CreateUser(string application, string userName, string password, string email,
                                                 string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public bool DeleteUser(string application, string userName, bool deleteAllRelatedData)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deleteAllRelatedData"></param>
        public void DeleteAllUsers(string application, bool deleteAllRelatedData)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="oldAnswer"></param>
        /// <param name="newQuestion"></param>
        /// <param name="newAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="isLockedOut"></param>
        public void UpdateUser(string application, string userName, string email, string oldAnswer, string newQuestion,
                               string newAnswer, bool isApproved, bool isLockedOut)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
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
        public void UpdateUserWithPassword(string application, string userName, string email, string password,
                                           string newQuestion, string newAnswer, bool isApproved, bool isLockedOut)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserInfo GetUserInfo(string application, string userName)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetUserNameByEmail(string application, string email)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public string[] GetAllUsers(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public int GetNumberOfUsersOnline(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public int UserIsOnlineTimeWindow(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Authenticate(string applicationName, string userName, string password)
        {
            if (string.IsNullOrEmpty(m_userName) || m_userName == userName)
            {
                if (string.IsNullOrEmpty(m_password) || m_password == password)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string applicationName, string userName, string role)
        {
            return Array.IndexOf<string>(m_roles, role) != -1;
            //throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string[] GetRoles(string applicationName, string userName)
        {
            return m_roles;
            //return new string[] { SystemConfiguration.AdministratorsRoleName };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool EnablePasswordReset(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool EnablePasswordRetrieval(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="length"></param>
        /// <param name="numberOfNonAlphanumericCharacters"></param>
        /// <returns></returns>
        public string GeneratePassword(string application, int length, int numberOfNonAlphanumericCharacters)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public int GetMaxInvalidPasswordAttempts(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public int GetMinRequiredNonAlphanumericCharacters(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public int GetMinRequiredPasswordLength(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public int GetPasswordAttemptWindow(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public string GetPasswordStrengthRegularExpression(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool RequiresQuestionAndAnswer(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string ResetPassword(string application, string userName)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <returns></returns>
        public string ResetPasswordWithQuestionAndAnswer(string application, string userName, string passwordAnswer)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <returns></returns>
        public string GetPassword(string application, string userName, string passwordAnswer)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetPasswordQuestion(string application, string userName)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        public void ChangePassword(string application, string userName, string newPassword)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="newPassword"></param>
        public void ChangePasswordWithAnswer(string application, string userName, string passwordAnswer,
                                             string newPassword)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] GetApplications()
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteAllApplications()
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public void DeleteApplication(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public string[] GetAllRoles(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string[] GetRolesForUser(string application, string userName)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public string[] GetUsersInRole(string application, string role)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool RoleExists(string application, string role)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userNames"></param>
        /// <param name="role"></param>
        public void AddUsersToRole(string application, string[] userNames, string role)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userNames"></param>
        /// <param name="roles"></param>
        public void AddUsersToRoles(string application, string[] userNames, string[] roles)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        public void AddUserToRole(string application, string userName, string role)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        public void AddUserToRoles(string application, string userName, string[] roles)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        public void CreateRole(string application, string role)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns></returns>
        public bool DeleteRole(string application, string role, bool throwOnPopulatedRole)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="throwOnPopulatedRole"></param>
        public void DeleteAllRoles(string application, bool throwOnPopulatedRole)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public bool IsRolesEnabled(string application)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        public void RemoveUserFromRole(string application, string userName, string roleName)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="users"></param>
        /// <param name="roleName"></param>
        public void RemoveUsersFromRole(string application, string[] users, string roleName)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        public void RemoveUserFromRoles(string application, string user, string[] roles)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        public void RemoveUsersFromRoles(string application, string[] users, string[] roles)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePasswordWithOldPassword(string applicationName, string userName, string oldPassword,
                                                  string newPassword)
        {
            throw new InvalidOperationException("Invalid Operation in MockManager!");
        }
    }
}
