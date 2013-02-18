using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.UserManager
{
    public static class UserManagerHelper
    {
        public const string UserAuthenticationCookieName = ".ASPXFORMSAUTH";
        public static string GetFormsAuthenticationTicket()
        {
            using (var pm = new ProviderManager())
            {
                ProviderBase wsProvider = pm.Providers["WebServiceUserManager"];
                if (wsProvider != null)
                {
                    WebServiceManager wsm = wsProvider.CreateUserManager() as WebServiceManager;
                    wsm.ReAuthenticate();
                    string ticket = wsm.GetFormsAuthenticationTicket();
                    return ticket;
                }
            }
            return null;
        }
    }
}
