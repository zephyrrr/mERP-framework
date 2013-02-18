using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Security;

namespace Feng.Server.Utils
{
    public static class AuthHelper
    {
        public static bool EnableAuth = false;
        public static bool CheckAuthentication()
        {
            if (!EnableAuth)
                return true;

            var p = Feng.Server.Utils.AuthHelper.GetCurrentPrincipal();
            System.Threading.Thread.CurrentPrincipal = p;
            return p != null;
        }
        private static readonly Regex m_regexFormTicket = new Regex(string.Format(@"{0}=(\w+)", Feng.UserManager.UserManagerHelper.UserAuthenticationCookieName));
        private static System.Security.Principal.IPrincipal GetCurrentPrincipal()
        {
            var cookieHeader = WebOperationContext.Current.IncomingRequest.Headers[System.Net.HttpRequestHeader.Cookie];
            if (!String.IsNullOrEmpty(cookieHeader))
            {
                Match match = m_regexFormTicket.Match(cookieHeader);
                if (match.Success)
                {
                    var s = match.Groups[1].Value;
                    var ticket = FormsAuthentication.Decrypt(s);
                    if (!ticket.Expired)
                    {
                        string userName = ticket.Name;
                        Roles.ApplicationName = ticket.UserData;
                        string[] roles = Roles.GetRolesForUser(userName);
                        return new System.Security.Principal.GenericPrincipal(
                            new System.Security.Principal.GenericIdentity(userName), roles);
                    }
                }
            }
            return null;
        }
    }
}
