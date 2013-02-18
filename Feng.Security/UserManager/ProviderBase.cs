using System;
using System.Configuration.Provider;
using Feng;

namespace Feng.UserManager
{
    /// <summary>
    /// UserManagerProviderBase
    /// </summary>
    public abstract class ProviderBase : System.Configuration.Provider.ProviderBase, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected virtual void Dispose(bool isDispose)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected string m_name;

        /// <summary>
        /// Name
        /// </summary>
        public override string Name
        {
            get { return m_name; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string m_description;

        /// <summary>
        /// Description
        /// </summary>
        public override string Description
        {
            get { return m_description; }
        }

        /// <summary>
        /// CreateMembershipManager
        /// </summary>
        /// <returns></returns>
        public abstract IMembershipManager CreateMembershipManager();

        /// <summary>
        /// CreateUserManager
        /// </summary>
        /// <returns></returns>
        public abstract IUserManager CreateUserManager();

        /// <summary>
        /// CreatePasswordManager
        /// </summary>
        /// <returns></returns>
        public abstract IPasswordManager CreatePasswordManager();

        /// <summary>
        /// CreateApplicationManager
        /// </summary>
        /// <returns></returns>
        public abstract IApplicationManager CreateApplicationManager();

        /// <summary>
        /// CreateRoleManager
        /// </summary>
        /// <returns></returns>
        public abstract IRoleManager CreateRoleManager();
    }
}