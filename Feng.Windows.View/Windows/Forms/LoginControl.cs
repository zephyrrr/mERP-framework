using System;
using System.Collections;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.ComponentModel;
using System.Web.Security;
using System.Web;
using Feng.UserManager;

namespace Feng.Windows.Forms
{
    using LoginEventHandler = EventHandler<LoginEventArgs>;

    /// <summary>
    /// LoginControl
    /// </summary>
    [DefaultEvent("LoginEvent")]
    public partial class LoginControl : UserControl
    {
        /// <summary>
        /// LoginEvent
        /// </summary>
        public event LoginEventHandler LoginEvent = delegate { };

        /// <summary>
        /// Constructor
        /// </summary>
        public LoginControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// UseName
        /// </summary>
        public string UserName
        {
            get { return m_UserNameBox.Text; }
            set { m_UserNameBox.Text = value; }
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get { return m_PasswordBox.Text; }
            set { m_PasswordBox.Text = value; }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            try
            {
                LoginHelper.Logout();

                string userName = m_UserNameBox.Text.Trim();
                string password = m_PasswordBox.Text.Trim();
                
                if (string.IsNullOrEmpty(userName))
                {
                    m_ErrorProvider.SetError(m_UserNameBox, "请输入用户名");
                    LoginEvent(this, new LoginEventArgs(null, LoginFailType.UIError, null));
                    return false;
                }
                else
                {
                    m_ErrorProvider.SetError(m_UserNameBox, String.Empty);
                }
                if (string.IsNullOrEmpty(password))
                {
                    m_ErrorProvider.SetError(m_PasswordBox, "请输入密码");
                    LoginEvent(this, new LoginEventArgs(null, LoginFailType.UIError, null));
                    return false;
                }
                else
                {
                    m_ErrorProvider.SetError(m_PasswordBox, String.Empty);
                }

                System.Security.Principal.IPrincipal p = LoginHelper.Login(userName, password);

                if (p == null)
                {
                    LoginEvent(this, new LoginEventArgs(null, LoginFailType.Fail, null));
                }
                else
                {
                    LoginEvent(this, new LoginEventArgs(p, LoginFailType.Success, null));
                }
            }
            catch (Exception ex)
            {
                LoginEvent(this, new LoginEventArgs(null, LoginFailType.Exception, ex.Message));
            }

            return true;
        }
    }
}