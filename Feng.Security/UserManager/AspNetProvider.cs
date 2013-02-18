using System;
using System.Configuration;

namespace Feng.UserManager
{
    /// <summary>
    /// AspNetUserManagerProvider
    /// </summary>
    public class AspNetProvider : ProviderBase
    {
        public AspNetProvider()
        {
            this.m_name = "aspnetProvider";
            if (System.Configuration.ConfigurationManager.ConnectionStrings["LoginConnectionString"] != null)
            {
                Properties.Settings.Default["LoginConnectionString"] = System.Configuration.ConfigurationManager.ConnectionStrings["LoginConnectionString"].ConnectionString;
            }
        }

        protected override void Dispose(bool isDispose)
        {
            if (isDispose)
            {
                if (m_manager != null)
                {
                    m_manager.Dispose();
                }
            }
        }

        private AspNetManager m_manager = new AspNetManager();

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            this.m_name = name;
            if (string.IsNullOrEmpty(config["description"]))
            {
                //throw new System.Configuration.Provider.ProviderException("You must specify a description attribute.");
            }
            else
            {
                this.m_description = config["description"];
                config.Remove("description");
            }

            if (config.Count > 0)
            {
                string extraAttribute = config.GetKey(0);
                if (!String.IsNullOrEmpty(extraAttribute))
                {
                    throw new System.Configuration.Provider.ProviderException(
                        "The following unrecognized attribute was found in " + Name + "'s configuration: '" +
                        extraAttribute + "'");
                }
                else
                {
                    throw new System.Configuration.Provider.ProviderException(
                        "An unrecognized attribute was found in the provider's configuration.");
                }
            }
        }

        /// <summary>
        /// CreateMembershipManager
        /// </summary>
        /// <returns></returns>
        public override IMembershipManager CreateMembershipManager()
        {
            return m_manager;
        }

        /// <summary>
        /// CreateUserManager
        /// </summary>
        /// <returns></returns>
        public override IUserManager CreateUserManager()
        {
            return m_manager;
        }

        /// <summary>
        /// CreatePasswordManager
        /// </summary>
        /// <returns></returns>
        public override IPasswordManager CreatePasswordManager()
        {
            return m_manager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IApplicationManager CreateApplicationManager()
        {
            return m_manager;
        }

        /// <summary>
        /// CreateRoleManager
        /// </summary>
        /// <returns></returns>
        public override IRoleManager CreateRoleManager()
        {
            return m_manager;
        }
    }
}