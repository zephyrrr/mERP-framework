using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using Feng.UserManager;

namespace Feng
{
    public static class LoginHelper
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static IPrincipal Login(string userName, string password)
        {
            using (var pm = new ProviderManager())
            {
                IUserManager um = pm.DefaultProvider.CreateUserManager();

                bool authenticate = um.Authenticate(SystemConfiguration.ApplicationName, userName, password);
                if (authenticate)
                {
                    IIdentity identity = new GenericIdentity(userName, pm.DefaultProvider.Name);
                    Debug.Assert(identity.IsAuthenticated);

                    string[] roles = um.GetRoles(SystemConfiguration.ApplicationName, userName);
                    IPrincipal principal = new GenericPrincipal(identity, roles);

                    SystemConfiguration.UserName = userName;
                    SystemConfiguration.Roles = roles;
                    SystemConfiguration.Password = password;
                    // Todo: Change to correct OrgId and ClientId
                    SystemConfiguration.ClientId = 0;
                    SystemConfiguration.OrgId = 0;

                    return principal;
                    //AppDomain.CurrentDomain.SetThreadPrincipal(principal);

                    //IToken token = SecurityCacheFactory.GetSecurityCacheProvider().SaveIdentity(identity);
                    //IIdentity t1 = SecurityCacheFactory.GetSecurityCacheProvider().GetIdentity(new GuidToken(new Guid("9958226e-0491-4c50-b189-affc0eb0ca81")));
                }
                else
                {
                    SystemConfiguration.UserName = null;
                    SystemConfiguration.Roles = new string[0];
                    SystemConfiguration.Password = string.Empty;
                }
                return null;
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        /// <summary>
        /// Logout
        /// </summary>
        public static void Logout()
        {
            Thread.CurrentPrincipal = null;
            SystemConfiguration.UserName = null;
            SystemConfiguration.Roles = null;
            //SecurityCacheFactory.GetSecurityCacheProvider().ExpireIdentity(m_token);
        }
    }
}
