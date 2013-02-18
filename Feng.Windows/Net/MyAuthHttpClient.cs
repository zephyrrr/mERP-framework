using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Windows.Net
{
    public class MyAuthHttpClient : Feng.Net.MyHttpClient
    {
        private string m_authTicket = null;
        private bool m_useAuth = true;
        protected override System.Net.Http.HttpClientHandler CreateHandle()
        {
            var handler = base.CreateHandle();
            if (m_useAuth)
            {
                handler.CookieContainer = new System.Net.CookieContainer();
                if (string.IsNullOrEmpty(m_authTicket))
                {
                    try
                    {
                        m_authTicket = Feng.UserManager.UserManagerHelper.GetFormsAuthenticationTicket();
                        handler.CookieContainer.Add(new Uri(SystemConfiguration.Server),
                            new System.Net.Cookie(Feng.UserManager.UserManagerHelper.UserAuthenticationCookieName, m_authTicket));
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        m_useAuth = false;
                    }
                }
            }
            return handler;
        }
    }
}
