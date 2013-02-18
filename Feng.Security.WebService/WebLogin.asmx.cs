using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Security;
using System.Configuration;
using System.Security.Permissions;
using System.Web.SessionState;
using System.IO;

namespace Feng.Security.WebService
{
    /// <summary>
    /// WebLogin 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://CredentialsServices.com", Description = "Implements the <a href=\"IUserManager.asmx\">IUserManager</a> interface. Wraps with a web service the ASP.NET provider model. This web service should be accessed over https.")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class WebLogin : LogInWebService, Feng.UserManager.IUserManager
    {
        //private void SaveLoginData(bool login, string applicationName, string userName)
        //{
        //    SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["LabChipsConnectionString1"].ConnectionString);
        //    SqlCommand cmd = new SqlCommand("INSERT Data_LogIn (Application, UserName, IP, Time, Action) VALUES (@Application, @UserName, @IP, @Time, @Action)", conn);
        //    cmd.Parameters.AddWithValue("@Application", applicationName);
        //    cmd.Parameters.AddWithValue("@UserName", userName);
        //    cmd.Parameters.AddWithValue("@IP", HttpContext.Current.Request.UserHostAddress);
        //    cmd.Parameters.AddWithValue("@Time", System.DateTime.Now);
        //    cmd.Parameters.AddWithValue("@Action", login ? "Login" : "Logout");
        //    conn.Open();
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //}

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public bool ChangePasswordWithOldPassword(string applicationName, string userName, string oldPassword, string newPassword)
        {
            Membership.ApplicationName = applicationName;
            MembershipUser user = Membership.GetUser(userName);
            if (user != null)
                return user.ChangePassword(oldPassword, newPassword);
            else
                return false;
        }

        [WebMethod(EnableSession = true)]
        public bool Authenticate(string applicationName, string userName, string password)
        {
            //if (HttpContext.Current.Request.IsSecureConnection == false)
            //{
            //    HttpContext.Current.Trace.Warn("You should use HTTPS to avoid sending passwords in clear text");
            //}

            Membership.ApplicationName = applicationName;

            if (ConfigurationManager.AppSettings["AllowSameUserLogin"] == "false")
            {
                // Check isOnline?
                if (Membership.GetUser(userName).IsOnline)
                {
                    return false;
                }
            }

            bool ok = Membership.ValidateUser(userName, password);
            //base.IsAuthenticated = ok;
            if (ok)
            {
                // 保存登录数据
                //SaveLoginData(true, applicationName, userName);
                //FormsAuthentication.SetAuthCookie(userName, false);
                // 创建身份验证票证
                base.IsAuthenticated = true;
                base.UserName = userName;
                return true;
            }
            else
            {
                base.UserName = null;
                return false;
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public bool IsInRole(string applicationName, string userName, string role)
        {
            Roles.ApplicationName = applicationName;
            return Roles.IsUserInRole(userName, role);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public string[] GetRoles(string applicationName, string userName)
        {
            Roles.ApplicationName = applicationName;
            return Roles.GetRolesForUser(userName);
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public string GetAppConfig(string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName))
                return null;
            string fileName = this.Context.Server.MapPath("config\\" + applicationName + ".exe.config");
            if (!File.Exists(fileName))
                return null;

            using (StreamReader sr = new StreamReader(fileName))
            {
                string s = sr.ReadToEnd();
                return s;
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public string GetFormsAuthenticationTicket()
        {
            if (string.IsNullOrEmpty(base.UserName))
                return null;
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, // version
                            base.UserName,
                            DateTime.Now, // creation
                            DateTime.Now.AddDays(1),// Expiration
                            false, // Persistent
                            Membership.ApplicationName); // User data

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            return encryptedTicket;
        }

        //[PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        //[WebMethod(EnableSession = true)]
        //public bool Logout(string applicationName, string userName)
        //{
        //    MembershipUser user = Membership.GetUser(userName);
        //    if (!user.IsOnline)
        //        return false;

        //    if (ConfigurationManager.AppSettings["AllowSameUserLogin"] == "false")
        //    {
        //        user.LastActivityDate = user.LastActivityDate.AddMinutes(-Membership.UserIsOnlineTimeWindow);
        //        Membership.UpdateUser(user);
        //    }

        //    //SaveLoginData(false, applicationName, userName);

        //    base.UserName = String.Empty;
        //    base.IsAuthenticated = false;

        //    return true;
        //}

        ///// <summary>
        ///// 更新用户状态（LastActivityDate）
        ///// </summary>
        ///// <param name="applicationName"></param>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //public bool UpdateState(string applicationName, string userName)
        //{
        //    MembershipUser user = Membership.GetUser(userName, true);
        //    return true;
        //}
    }
}
