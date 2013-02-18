using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Runtime.InteropServices;
using System.Net;
using Feng.UserManager;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MembershipSetupForm
    /// </summary>
    public partial class MembershipForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipForm()
        {
            InitializeComponent();
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        private bool m_webHttp;
        private bool m_webServiceProvider;
        private string m_userManagerAddress;
        private void Form_Load(object sender, EventArgs e)
        {
            m_webHttp = false;
            m_webServiceProvider = false;

            m_userManagerAddress = null;// ServiceProvider.GetService<IDefinition>().TryGetValue(DefinitionString.UserManagerAddress);
            if (string.IsNullOrEmpty(m_userManagerAddress))
            {
                m_userManagerAddress = SystemConfiguration.Server + "/" + SystemConfiguration.ApplicationName + "/Membership/MembershipManage.aspx";
            }
            using (var pm = new ProviderManager())
            {
                ProviderBase provider = pm.DefaultProvider;
                if (provider is WebServiceProvider && !string.IsNullOrEmpty(m_userManagerAddress))
                {
                    m_webHttp = true;
                }
                if (!m_webHttp)
                {
                    if (!System.IO.File.Exists(SystemDirectory.WorkDirectory + "\\CredentialsManager.exe"))
                    {
                        m_webHttp = true;
                    }
                }

                if (m_webHttp)
                {
                    string url = m_userManagerAddress + "?ApplicationName=" +
                                     SystemConfiguration.ApplicationName;

                    ProviderBase wsProvider = pm.Providers["WebServiceUserManager"];
                    if (wsProvider != null)
                    {
                        m_webServiceProvider = true;
                        WebServiceManager wsm = wsProvider.CreateUserManager() as WebServiceManager;
                        wsm.ReAuthenticate();

                        // ´´½¨Cookie
                        string cookieName = UserManagerHelper.UserAuthenticationCookieName;

                        string ticket = wsm.GetFormsAuthenticationTicket();
                        HttpCookie authCookie = new HttpCookie(cookieName, ticket);
                        InternetSetCookie(url, cookieName, authCookie.Value);
                    }
                    webBrowser1.Navigate(url);
                }
                else
                {
                    try
                    {
                        this.Close();

                        Feng.Async.AsyncHelper.Start(() =>
                            {
                                string exeFileName = "CredentialsManager.exe";
                                string configFileName = string.Format("{0}\\{1}.config", SystemDirectory.WorkDirectory, exeFileName);
                                if (!System.IO.File.Exists(configFileName) || !Authority.IsDeveloper())
                                {
                                    System.IO.File.Copy(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config", configFileName, true);
                                }
                                var process = ProcessHelper.ExecuteApplication(exeFileName, "-applicationName " + SystemConfiguration.ApplicationName);
                                if (process != null)
                                {
                                    process.WaitForExit();
                                }
                                if (System.IO.File.Exists(configFileName) && !Authority.IsDeveloper())
                                {
                                    System.IO.File.Delete(configFileName);
                                }
                            });
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithNotify(ex);
                    }
                }
            }
        }

        //void process_Exited(object sender, EventArgs e)
        //{
        //    //+= new EventHandler((sender1, e1) => System.IO.File.Delete(configFileName));
        //    System.IO.File.Delete("");
        //}

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            if (m_webHttp && m_webServiceProvider)
            {
                string cookieName = ".ASPXFORMSAUTH";
                string url = m_userManagerAddress + "?ApplicationName=" +
                             SystemConfiguration.ApplicationName;

                InternetSetCookie(url, cookieName, null);
            }
        }
    }
}