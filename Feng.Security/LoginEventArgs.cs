//Questions? Comments? go to 
//http://www.idesign.net
using System;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;

namespace Feng
{
    /// <summary>
    /// LoginFailType
    /// </summary>
    public enum LoginFailType
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = -1,
        /// <summary>
        /// Fail
        /// </summary>
        Fail = 0,
        /// <summary>
        /// AlreadyLogin
        /// </summary>
        AlreadyLogin = 1,
        /// <summary>
        /// Exception
        /// </summary>
        Exception = 2,
        /// <summary>
        /// 
        /// </summary>
        UIError = 3
    }

    /// <summary>
    /// LoginEventArgs
    /// </summary>
    public class LoginEventArgs : EventArgs
    {
        /// <summary>
        /// LoginEventArgs
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public LoginEventArgs(System.Security.Principal.IPrincipal principal, LoginFailType type, string message)
        {
            m_principal = principal;
            m_loginFailType = type;
            m_message = message;
        }

        private System.Security.Principal.IPrincipal m_principal;

        /// <summary>
        /// Authenticated
        /// </summary>
        public System.Security.Principal.IPrincipal Principal
        {
            get { return m_principal; }
        }

        private string m_message;

        /// <summary>
        /// Authenticated
        /// </summary>
        public string Message
        {
            get { return m_message; }
        }

        private LoginFailType m_loginFailType;

        /// <summary>
        /// µ«¬º¥ÌŒÛ¿‡–Õ
        /// </summary>
        public LoginFailType LoginFailType
        {
            get { return m_loginFailType; }
            internal set { m_loginFailType = value; }
        }
    }
}