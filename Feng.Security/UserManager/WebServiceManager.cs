// ?2005 IDesign Inc. All rights reserved 
//Questions? Comments? go to 
//http://www.idesign.net

using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System;
using System.Xml.Serialization;
using System.Net;

namespace Feng.UserManager
{
    /// <summary>
    /// 
    /// </summary>
    //[DebuggerStepThrough]
    [ToolboxItem(false)]
    [DesignerCategory("code")]
    [WebServiceBinding(Name = "AspNetSqlProviderService", Namespace = "http://CredentialsServices.com")]
    public class WebServiceManager : SoapHttpClientProtocol, IMembershipManager, IUserManager, IPasswordManager,
                                     IApplicationManager, IRoleManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public WebServiceManager(string url)
        {
            CookieContainer = new CookieContainer();
            Credentials = CredentialCache.DefaultCredentials;
            PreAuthenticate = true; //Optional

            Url = url;
        }

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
        [SoapDocumentMethod("http://CredentialsServices.com/GetAppConfig")]
        public string GetAppConfig(string applicationName)
        {
            object[] results = Invoke("GetAppConfig", new object[]
                                                      {
                                                          applicationName
                                                      });
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetFormsAuthenticationTicket")]
        public string GetFormsAuthenticationTicket()
        {
            object[] results = Invoke("GetFormsAuthenticationTicket", new object[] {});
            return ((string) (results[0]));
        }

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
        [SoapDocumentMethod("http://CredentialsServices.com/CreateUser")]
        public MembershipCreateStatus CreateUser(string application, string userName, string password, string email,
                                                 string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            object[] results = Invoke("CreateUser", new object[]
                                                    {
                                                        application,
                                                        userName,
                                                        password,
                                                        email,
                                                        passwordQuestion,
                                                        passwordAnswer,
                                                        isApproved
                                                    });
            return ((MembershipCreateStatus) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/DeleteUser")]
        public bool DeleteUser(string application, string userName, bool deleteAllRelatedData)
        {
            object[] results = Invoke("DeleteUser", new object[]
                                                    {
                                                        application,
                                                        userName,
                                                        deleteAllRelatedData
                                                    });
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deleteAllRelatedData"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/DeleteAllUsers")]
        public void DeleteAllUsers(string application, bool deleteAllRelatedData)
        {
            Invoke("DeleteAllUsers", new object[] {application, deleteAllRelatedData});
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
        [SoapDocumentMethod("http://CredentialsServices.com/UpdateUser")]
        public void UpdateUser(string application, string userName, string email, string oldAnswer, string newQuestion,
                               string newAnswer, bool isApproved, bool isLockedOut)
        {
            Invoke("UpdateUser", new object[]
                                 {
                                     application,
                                     userName,
                                     email,
                                     oldAnswer,
                                     newQuestion,
                                     newAnswer,
                                     isApproved,
                                     isLockedOut
                                 });
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
        [SoapDocumentMethod("http://CredentialsServices.com/UpdateUserWithPassword")]
        public void UpdateUserWithPassword(string application, string userName, string email, string password,
                                           string newQuestion, string newAnswer, bool isApproved, bool isLockedOut)
        {
            Invoke("UpdateUserWithPassword", new object[]
                                             {
                                                 application,
                                                 userName,
                                                 email,
                                                 password,
                                                 newQuestion,
                                                 newAnswer,
                                                 isApproved,
                                                 isLockedOut
                                             });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetUserInfo")]
        public UserInfo GetUserInfo(string application, string userName)
        {
            object[] results = Invoke("GetUserInfo", new string[] {application, userName});
            return ((UserInfo) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetUserNameByEmail")]
        public string GetUserNameByEmail(string application, string email)
        {
            object[] results = Invoke("GetUserNameByEmail", new string[] {application, email});
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetAllUsers")]
        public string[] GetAllUsers(string application)
        {
            object[] results = Invoke("GetAllUsers", new string[] {application});
            return ((string[]) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetNumberOfUsersOnline")]
        public int GetNumberOfUsersOnline(string application)
        {
            object[] results = Invoke("GetNumberOfUsersOnline", new string[] {application});
            return ((int) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/UserIsOnlineTimeWindow")]
        public int UserIsOnlineTimeWindow(string application)
        {
            object[] results = Invoke("UserIsOnlineTimeWindow", new string[] {application});
            return ((int) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/Authenticate")]
        public bool Authenticate(string applicationName, string userName, string password)
        {
            object[] results = Invoke("Authenticate", new object[]
                                                      {
                                                          applicationName,
                                                          userName,
                                                          password
                                                      });
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/IsInRole")]
        public bool IsInRole(string applicationName, string userName, string role)
        {
            object[] results = Invoke("IsInRole", new string[] {applicationName, userName, role});
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetRoles")]
        public string[] GetRoles(string applicationName, string userName)
        {
            object[] results = Invoke("GetRoles", new string[] {applicationName, userName});
            return ((string[]) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/EnablePasswordReset")]
        public bool EnablePasswordReset(string application)
        {
            object[] results = Invoke("EnablePasswordReset", new string[] {application});
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/EnablePasswordRetrieval")]
        public bool EnablePasswordRetrieval(string application)
        {
            object[] results = Invoke("EnablePasswordRetrieval", new string[] {application});
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="length"></param>
        /// <param name="numberOfNonAlphanumericCharacters"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GeneratePassword")]
        public string GeneratePassword(string application, int length, int numberOfNonAlphanumericCharacters)
        {
            object[] results = Invoke("GeneratePassword", new object[]
                                                          {
                                                              application,
                                                              length,
                                                              numberOfNonAlphanumericCharacters
                                                          });
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetMaxInvalidPasswordAttempts")]
        public int GetMaxInvalidPasswordAttempts(string application)
        {
            object[] results = Invoke("GetMaxInvalidPasswordAttempts", new string[] {application});
            return ((int) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetMinRequiredNonAlphanumericCharacters")]
        public int GetMinRequiredNonAlphanumericCharacters(string application)
        {
            object[] results = Invoke("GetMinRequiredNonAlphanumericCharacters", new string[] {application});
            return ((int) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetMinRequiredPasswordLength")]
        public int GetMinRequiredPasswordLength(string application)
        {
            object[] results = Invoke("GetMinRequiredPasswordLength", new string[] {application});
            return ((int) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetPasswordAttemptWindow")]
        public int GetPasswordAttemptWindow(string application)
        {
            object[] results = Invoke("GetPasswordAttemptWindow", new string[] {application});
            return ((int) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetPasswordStrengthRegularExpression")]
        public string GetPasswordStrengthRegularExpression(string application)
        {
            object[] results = Invoke("GetPasswordStrengthRegularExpression", new string[] {application});
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/RequiresQuestionAndAnswer")]
        public bool RequiresQuestionAndAnswer(string application)
        {
            object[] results = Invoke("RequiresQuestionAndAnswer", new string[] {application});
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/ResetPassword")]
        public string ResetPassword(string application, string userName)
        {
            object[] results = Invoke("ResetPassword", new string[] {application, userName});
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/ResetPasswordWithQuestionAndAnswer")]
        public string ResetPasswordWithQuestionAndAnswer(string application, string userName, string passwordAnswer)
        {
            object[] results = Invoke("ResetPasswordWithQuestionAndAnswer",
                                      new string[] {application, userName, passwordAnswer});
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetPassword")]
        public string GetPassword(string application, string userName, string passwordAnswer)
        {
            object[] results = Invoke("GetPassword", new string[] {application, userName, passwordAnswer});
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetPasswordQuestion")]
        public string GetPasswordQuestion(string application, string userName)
        {
            object[] results = Invoke("GetPasswordQuestion", new string[] {application, userName});
            return ((string) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/ChangePassword")]
        public void ChangePassword(string application, string userName, string newPassword)
        {
            Invoke("ChangePassword", new string[] {application, userName, newPassword});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="newPassword"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/ChangePasswordWithAnswer")]
        public void ChangePasswordWithAnswer(string application, string userName, string passwordAnswer,
                                             string newPassword)
        {
            Invoke("ChangePasswordWithAnswer", new string[] {application, userName, passwordAnswer, newPassword});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetApplications")]
        public string[] GetApplications()
        {
            object[] results = Invoke("GetApplications", new object[0]);
            return ((string[]) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        [SoapDocumentMethod("http://CredentialsServices.com/DeleteAllApplications")]
        public void DeleteAllApplications()
        {
            Invoke("DeleteAllApplications", new object[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/DeleteApplication")]
        public void DeleteApplication(string application)
        {
            Invoke("DeleteApplication", new string[] {application});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetAllRoles")]
        public string[] GetAllRoles(string application)
        {
            object[] results = Invoke("GetAllRoles", new string[] {application});
            return ((string[]) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetRolesForUser")]
        public string[] GetRolesForUser(string application, string userName)
        {
            object[] results = Invoke("GetRolesForUser", new string[] {application, userName});
            return ((string[]) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/GetUsersInRole")]
        public string[] GetUsersInRole(string application, string role)
        {
            object[] results = Invoke("GetUsersInRole", new string[] {application, role});
            return ((string[]) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/RoleExists")]
        public bool RoleExists(string application, string role)
        {
            object[] results = Invoke("RoleExists", new string[] {application, role});
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userNames"></param>
        /// <param name="role"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/AddUsersToRole")]
        public void AddUsersToRole(string application, string[] userNames, string role)
        {
            Invoke("AddUsersToRole", new object[] {application, userNames, role});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userNames"></param>
        /// <param name="roles"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/AddUsersToRoles")]
        public void AddUsersToRoles(string application, string[] userNames, string[] roles)
        {
            Invoke("AddUsersToRoles", new object[] {application, userNames, roles});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/AddUserToRole")]
        public void AddUserToRole(string application, string userName, string role)
        {
            Invoke("AddUserToRole", new string[] {application, userName, role});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/AddUserToRoles")]
        public void AddUserToRoles(string application, string userName, string[] roles)
        {
            Invoke("AddUserToRoles", new object[] {application, userName, roles});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/CreateRole")]
        public void CreateRole(string application, string role)
        {
            Invoke("CreateRole", new string[] {application, role});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="role"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/DeleteRole")]
        public bool DeleteRole(string application, string role, bool throwOnPopulatedRole)
        {
            object[] results = Invoke("DeleteRole", new object[]
                                                    {
                                                        application,
                                                        role,
                                                        throwOnPopulatedRole
                                                    });
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="throwOnPopulatedRole"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/DeleteAllRoles")]
        public void DeleteAllRoles(string application, bool throwOnPopulatedRole)
        {
            Invoke("DeleteAllRoles", new object[] {application, throwOnPopulatedRole});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/IsRolesEnabled")]
        public bool IsRolesEnabled(string application)
        {
            object[] results = Invoke("IsRolesEnabled", new string[] {application});
            return ((bool) (results[0]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/RemoveUserFromRole")]
        public void RemoveUserFromRole(string application, string userName, string roleName)
        {
            Invoke("RemoveUserFromRole", new string[] {application, userName, roleName});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="users"></param>
        /// <param name="roleName"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/RemoveUsersFromRole")]
        public void RemoveUsersFromRole(string application, string[] users, string roleName)
        {
            Invoke("RemoveUsersFromRole", new object[] {application, users, roleName});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/RemoveUserFromRoles")]
        public void RemoveUserFromRoles(string application, string user, string[] roles)
        {
            Invoke("RemoveUserFromRoles", new object[] {application, user, roles});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="users"></param>
        /// <param name="roles"></param>
        [SoapDocumentMethod("http://CredentialsServices.com/RemoveUsersFromRoles")]
        public void RemoveUsersFromRoles(string application, string[] users, string[] roles)
        {
            Invoke("RemoveUsersFromRoles", new object[] {application, users, roles});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [SoapDocumentMethod("http://CredentialsServices.com/ChangePasswordWithOldPassword")]
        public bool ChangePasswordWithOldPassword(string applicationName, string userName, string oldPassword,
                                                  string newPassword)
        {
            object[] results = Invoke("ChangePasswordWithOldPassword",
                                      new string[] {applicationName, userName, oldPassword, newPassword});
            return ((bool) (results[0]));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace = "http://CredentialsServices.com")]
    public enum MembershipCreateStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Success,
        /// <summary>
        /// 
        /// </summary>
        InvalidUserName,
        /// <summary>
        /// 
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// 
        /// </summary>
        InvalidQuestion,
        /// <summary>
        /// 
        /// </summary>
        InvalidAnswer,
        /// <summary>
        /// 
        /// </summary>
        InvalidEmail,
        /// <summary>
        /// 
        /// </summary>
        DuplicateUserName,
        /// <summary>
        /// 
        /// </summary>
        DuplicateEmail,
        /// <summary>
        /// 
        /// </summary>
        UserRejected,
        /// <summary>
        /// 
        /// </summary>
        InvalidProviderUserKey,
        /// <summary>
        /// 
        /// </summary>
        DuplicateProviderUserKey,
        /// <summary>
        /// 
        /// </summary>
        ProviderError,
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public partial class UserInfo
    {
        private string m_Email;
        private string m_PasswordQuestion;
        private bool m_IsApproved;
        private bool m_IsLockedOut;
        private string m_Name;

        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PasswordQuestion
        {
            get { return m_PasswordQuestion; }
            set { m_PasswordQuestion = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsApproved
        {
            get { return m_IsApproved; }
            set { m_IsApproved = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLockedOut
        {
            get { return m_IsLockedOut; }
            set { m_IsLockedOut = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="isApproved"></param>
        /// <param name="isLockedOut"></param>
        public UserInfo(string name, string email, string passwordQuestion, bool isApproved, bool isLockedOut)
        {
            Name = name;
            Email = email;
            PasswordQuestion = passwordQuestion;
            IsApproved = isApproved;
            IsLockedOut = isLockedOut;
        }

        /// <summary>
        /// 
        /// </summary>
        public UserInfo()
        {
        }
    }
}