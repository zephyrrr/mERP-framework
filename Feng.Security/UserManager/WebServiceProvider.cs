using System;
using System.Configuration;
using Feng;

namespace Feng.UserManager
{
    /// <summary>
    /// WebServiceUserManagerProvider
    /// </summary>
    public class WebServiceProvider : ProviderBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceAddress">http://" + serverIp + ":" + serverPort + serviceAddress</param>
        public WebServiceProvider(string serviceAddress)
        {
            this.m_name = "webServiceProvider";
            m_manager = new WebServiceManager(serviceAddress);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebServiceProvider()
        {
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

        private WebServiceManager m_manager;

        public string WebServiceAddress
        {
            get { return m_manager.Url; }
            set
            {
                m_manager.Url = value;
            }
        }
        ///// <summary>
        ///// SetWebServiceAddress
        ///// </summary>
        ///// <param name="serverIp"></param>
        ///// <param name="serverPort"></param>
        //public void SetWebServiceAddress(string serverIp, short serverPort)
        //{
        //    string serviceAddress = m_webServiceAddress;
        //    string httpName = "http://";
        //    if (m_webServiceAddress.StartsWith(httpName))
        //    {
        //        int idx = serviceAddress.IndexOf('/', httpName.Length);
        //        serviceAddress = serviceAddress.Substring(idx);
        //    }
        //    m_manager = new WebServiceManager("http://" + serverIp + ":" + serverPort + serviceAddress);
        //}

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if ((config == null) || (config.Count == 0))
            {
                throw new ArgumentNullException("You must supply a valid configuration parameters.", "config");
            }

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
            string webServiceAddress;
            if (string.IsNullOrEmpty(config["webServiceAddress"]))
            {
                webServiceAddress = string.Format("{0}/{1}/WebLogin.asmx", SystemConfiguration.Server, SystemConfiguration.ApplicationName);
                //throw new System.Configuration.Provider.ProviderException("The webServiceAddress is invalid.");
            }
            else
            {
                webServiceAddress = config["webServiceAddress"];
            }
            config.Remove("webServiceAddress");

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

            //ConnectionStringsSection cs =
            //    (ConnectionStringsSection)ConfigurationManager.GetSection("connectionStrings");
            //if (cs == null)
            //    throw new ProviderException("An error occurred retrieving the connection strings section.");
            //if (cs.ConnectionStrings[connectionString] == null)
            //    throw new ProviderException("The connection string could not be found in the connection strings section.");
            //else
            //    ConnectionString = cs.ConnectionStrings[connectionString].ConnectionString;

            m_manager = new WebServiceManager(webServiceAddress);
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