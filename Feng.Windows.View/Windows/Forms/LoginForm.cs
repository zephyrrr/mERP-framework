using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Feng.UserManager;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// LoginForm
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        private bool m_bAutoLogin = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="autoLogin"></param>
        public LoginForm(bool autoLogin)
        {
            m_bAutoLogin = autoLogin;

            InitializeComponent();
        }

        /// <summary>
        /// OnLogin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void LoginControl_LoginEvent(object sender, LoginEventArgs e)
        {
            if (e.Principal != null)
            {
                System.Threading.Thread.CurrentPrincipal = e.Principal;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                switch (e.LoginFailType)
                {
                    case LoginFailType.Fail:
                        MessageForm.ShowError("用户名或密码错误。");
                        break;
                    case LoginFailType.AlreadyLogin:
                        MessageForm.ShowError("您已在其他地方登录。请先登出或等待一段时间。");
                        break;
                    case LoginFailType.Exception:
                        MessageForm.ShowError("出现未知错误，" + e.Message);
                        break;
                    case LoginFailType.UIError:
                        break;
                    default:
                        throw new ArgumentException("Invalid LoginEventArgs");
                }

                this.DialogResult = DialogResult.Retry;
            }
        }

        private AMS.Profile.IProfile m_profile = SystemProfileFile.DefaultGlobalProfile;

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            if (ckbAutoLogin.Checked)
            {
                ckbPwdRemember.Checked = true;
            }

            string key = Feng.Cryptographer.GenerateKey();

            m_profile.SetValue("Login", "Key", key);
            m_profile.SetValue("Login", "UserId", Feng.Cryptographer.EncryptSymmetric(aspNetLoginControl1.UserName, key));
            m_profile.SetValue("Login", "UserPwd",
                               ckbPwdRemember.Checked ? Feng.Cryptographer.EncryptSymmetric(aspNetLoginControl1.Password, key) : "");
            m_profile.SetValue("Login", "RememberPwd", ckbPwdRemember.Checked);
            m_profile.SetValue("Login", "AutoLogin", ckbAutoLogin.Checked);
        }

        /// <summary>
        /// 读入配置
        /// </summary>
        private void LoadConfig()
        {
            string key =  m_profile.GetValue("Login", "Key", "");
            if (!string.IsNullOrEmpty(key))
            {
                aspNetLoginControl1.UserName = Feng.Cryptographer.DecryptSymmetric(m_profile.GetValue("Login", "UserId", ""), key);
                aspNetLoginControl1.Password = Feng.Cryptographer.DecryptSymmetric(m_profile.GetValue("Login", "UserPwd", ""), key);
            }
            ckbPwdRemember.Checked = m_profile.GetValue("Login", "RememberPwd", false);
            ckbAutoLogin.Checked = m_profile.GetValue("Login", "AutoLogin", false);

            //SetUserManagerAddress();
        }

        //private void SetUserManagerAddress()
        //{
        //    if (string.IsNullOrEmpty(txtIp.Text))
        //    {
        //        return;
        //    }

        //    short port = 80;
        //    if (!string.IsNullOrEmpty(txtPort.Text))
        //    {
        //        Int16.TryParse(txtPort.Text, out port);
        //    }

        //    ProviderBase provider = ProviderManager.Instance.DefaultProvider;
        //    UserManager.WebServiceProvider webServiceProvider = provider as UserManager.WebServiceProvider;
        //    if (webServiceProvider != null)
        //    {
        //        webServiceProvider.SetWebServiceAddress(txtIp.Text, port);
        //    }
        //    else
        //    {
        //        throw new NotSupportedException("Only WebService UserManager Provider support custom address!");
        //    }

        //    SystemConfiguration.ServerIP = txtIp.Text;
        //    SystemConfiguration.ServerPort = port;
        //}

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SaveConfig();

            bool res = false;
            try
            {
                res = aspNetLoginControl1.Login();
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                MessageForm.ShowWarning("不能登录，请稍后再试。");
            }
        }

        //private bool m_detailMode = true;
        /// <summary>
        /// 显示和隐藏服务器连接设置表单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetup_Click(object sender, EventArgs e)
        {
            string fileName = SystemDirectory.WorkDirectory + "\\connectionStrings.config";
            if (!File.Exists(fileName))
            {
                MessageForm.ShowError("您不具有设置权限！");
            }
            else
            {
                using (ConnectionStringModifyForm form = new ConnectionStringModifyForm(fileName))
                {
                    form.ShowDialog();
                }
            }
            //using (LoginSetupForm form = new LoginSetupForm())
            //{
            //    form.ShowDialog(this);
            //}
        }

        /// <summary>
        /// 得到Logo
        /// </summary>
        /// <returns></returns>
        protected virtual Image GetLogImage()
        {
            return ImageResource.TryGet("Images.Logo.jpg");
        }

        /// <summary>
        /// OnFormLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void LoginForm_Load(object sender, EventArgs e)
        {
            dataPictureBox1.BackgroundImage = GetLogImage();
            dataPictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            LoadConfig();
            //btnSetup_Click(btnSetup, System.EventArgs.Empty);

            if (m_bAutoLogin && ckbAutoLogin.Checked)
            {
                btnLogin_Click(btnLogin, System.EventArgs.Empty);
            }
        }

        private void frm_Login_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK
                && this.DialogResult != DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ckbAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAutoLogin.Checked)
            {
                ckbPwdRemember.Checked = true;
            }
        }

        //private void btnTest_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtIp.Text))
        //    {
        //        MessageBox.Show("请输入服务器地址！");
        //        return;
        //    }
        //    short port = 80;
        //    if (!string.IsNullOrEmpty(txtPort.Text))
        //    {
        //        Int16.TryParse(txtPort.Text, out port);
        //    }
        //    try
        //    {
        //        //StringBuilder sb = new StringBuilder();
        //        //if (txtIp.Text.StartsWith("http://"))
        //        //    sb.Append(txtIp.Text);
        //        //else
        //        //    sb.Append("http://" + txtIp.Text);
        //        //if (!string.IsNullOrEmpty(txtPort.Text))
        //        //{
        //        //    sb.Append(":" + txtPort.Text);
        //        //}

        //        IPHostEntry hostEntry = Dns.GetHostEntry(txtIp.Text);

        //        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        //        // an exception that occurs when the host IP Address is not compatible with the address family
        //        // (typical in the IPv6 case).
        //        foreach (IPAddress address in hostEntry.AddressList)
        //        {
        //            IPEndPoint ipe = new IPEndPoint(address, port);
        //            Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        //            tempSocket.Connect(ipe);
        //            if (tempSocket.Connected)
        //            {
        //                m_profile.SetValue("Login", "ServerIp", txtIp.Text);
        //                m_profile.SetValue("Login", "ServerPort", txtPort.Text);
        //                MessageBox.Show("连接成功！");

        //                SetUserManagerAddress();

        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionProcess.ProcessWithResume(ex);
        //        MessageBox.Show("连接失败！");
        //    }
        //}
    }
}