using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

namespace Feng.Web
{
    public class MyHttpClient
    {
        public MyHttpClient(bool useAuth = true)
        {
            m_useAuth = useAuth;
        }
        private bool m_useAuth = false;
        private string m_authTicket = null;
        public string GetSync(string addr)
        {
            HttpClientHandler handler = new HttpClientHandler();
            if (m_useAuth)
            {
                handler.CookieContainer = new System.Net.CookieContainer();
                if (string.IsNullOrEmpty(m_authTicket))
                {
                    try
                    {
                        m_authTicket = Feng.UserManager.UserManagerHelper.GetFormsAuthenticationTicket();
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        m_useAuth = false;
                    }
                }
                handler.CookieContainer.Add(new Uri(SystemConfiguration.Server),
                    new System.Net.Cookie(Feng.UserManager.UserManagerHelper.UserAuthenticationCookieName, m_authTicket));
            }
            HttpClient client = new HttpClient(handler);
            client.MaxResponseContentBufferSize = 5242880;

            var t = client.GetAsync(addr).ContinueWith((requestTask) =>
                    {
                        System.Net.Http.HttpResponseMessage response = requestTask.Result;
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            throw new InvalidOperationException("Invalid Rest Service with StatusCode of " + response.StatusCode);
                        }
                        response.EnsureSuccessStatusCode();
                        var t2 = response.Content.ReadAsStringAsync().ContinueWith((readTask) =>
                            {
                                string s = readTask.Result;
                                return s;
                            });
                        return t2.Result;
                    });
            t.Wait();
            return t.Result;
        }
    }
}
