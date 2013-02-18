using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 注销类
    /// </summary>
    public class LogoutForm
    {
        private AMS.Profile.IProfile m_profile = SystemProfileFile.DefaultUserProfile;

        /// <summary>
        /// 注销，使不自动登录，重新执行程序
        /// </summary>
        public LogoutForm()
        {
            m_profile.SetValue("Login", "AutoLogin", false);

            ProcessHelper.ExecuteApplication(Application.ExecutablePath, "/username:anonymous /password:nowandfuture");

            Application.Exit();
        }
    }
}