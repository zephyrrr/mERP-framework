using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng;
using Feng.UserManager;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// ChangePwdForm
    /// </summary>
    public partial class ChangePwdForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChangePwdForm()
        {
            InitializeComponent();
        }

        private bool m_success = true;

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_success = false;

            string password0 = m_PasswordBox0.Text;
            string password = m_PasswordBox.Text;
            string password2 = m_PasswordBox2.Text;

            if (password0 == String.Empty)
            {
                m_ErrorProvider.SetError(m_PasswordBox0, "请输入旧密码");
                return;
            }
            else
            {
                m_ErrorProvider.SetError(m_PasswordBox0, String.Empty);
            }
            if (password == String.Empty)
            {
                m_ErrorProvider.SetError(m_PasswordBox, "请输入新密码");
                return;
            }
            else
            {
                m_ErrorProvider.SetError(m_PasswordBox, String.Empty);
            }
            if (password.Length < 6)
            {
                m_ErrorProvider.SetError(m_PasswordBox, "密码长度小于6位");
                return;
            }
            else
            {
                m_ErrorProvider.SetError(m_PasswordBox, String.Empty);
            }
            if (password != password2)
            {
                m_ErrorProvider.SetError(m_PasswordBox2, "两次输入的新密码不匹配");
                return;
            }
            else
            {
                m_ErrorProvider.SetError(m_PasswordBox2, String.Empty);
            }

            try
            {
                using (var pm = new ProviderManager())
                {
                    IUserManager userManager = pm.DefaultProvider.CreateUserManager();
                    WebServiceManager webServiceManager = userManager as WebServiceManager;
                    if (webServiceManager != null)
                    {
                        webServiceManager.ReAuthenticate();
                    }

                    bool res = pm.DefaultProvider.CreatePasswordManager().ChangePasswordWithOldPassword(
                            SystemConfiguration.ApplicationName, SystemConfiguration.UserName,
                            password0, password);
                    if (res)
                    {
                        m_success = res;
                        MessageForm.ShowInfo("修改密码成功");
                    }
                    else
                    {
                        MessageForm.ShowInfo("修改密码失败");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                MessageForm.ShowError("修改密码失败");
            }

            if (m_success)
            {
                this.Close();
            }
        }

        private void frm_ChangePwd_FormClosing(object sender, FormClosingEventArgs e)
        {
            //    if (!m_success)
            //    {
            //        e.Cancel = true;
            //    }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_success = true;
            this.Close();
        }
    }
}