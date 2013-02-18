using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.UserManager
{
    public class MockProvider : ProviderBase
    {
        public MockProvider()
        {
            this.m_name = "mockProvider";
        }

        private MockManager m_manager;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if ((config == null))
            {
                throw new ArgumentNullException("You must supply a valid configuration parameters.");
            }

            this.m_name = name;

            string userName = null, roles = null, password = null;
            if (string.IsNullOrEmpty(config["description"]))
            {
                //throw new System.Configuration.Provider.ProviderException("You must specify a description attribute.");
            }
            else
            {
                this.m_description = config["description"];
                config.Remove("description");
            }

            if (string.IsNullOrEmpty(config["userName"]))
            {
                throw new System.Configuration.Provider.ProviderException("The userName is invalid.");
            }
            else
            {
                userName = config["userName"];
                config.Remove("userName");
            }
            if (string.IsNullOrEmpty(config["password"]))
            {
                //throw new System.Configuration.Provider.ProviderException("The password is invalid.");
            }
            else
            {
                password = config["password"];
                config.Remove("password");
            }

            if (string.IsNullOrEmpty(config["roles"]))
            {
                throw new System.Configuration.Provider.ProviderException("The roles is invalid.");
            }
            else
            {
                roles = config["roles"];
                config.Remove("roles");
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

            m_manager = new MockManager(userName, password, roles);
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
        /// CreateApplicationManager
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
