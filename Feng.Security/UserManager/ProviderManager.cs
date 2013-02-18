using System;
using System.Configuration;
using System.Web.Configuration;

namespace Feng.UserManager
{
    /// <summary>
    /// ProviderManager
    /// </summary>
    public class ProviderManager : IDisposable
    {
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_providerCollection != null)
                {
                    foreach (var i in m_providerCollection)
                    {
                        (i as ProviderBase).Dispose();
                    }
                    m_providerCollection.Clear();
                }
            }
        }


        //Initialization related variables and logic
        private bool isInitialized;
        //private static Exception initializationException;
        //private static object initializationLock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public ProviderManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                
                m_providerCollection = new ProviderCollection();
                ProviderConfigurationSection qc = null;

                bool defaultMode = false;
                //Get the feature's configuration info
                qc = (ProviderConfigurationSection)ConfigurationManager.GetSection("UserManagerProvider");
                if (qc == null)
                {
                    defaultMode = true;
                }
                if (defaultMode)
                {
                    if (SystemConfiguration.ApplicationName == "Example")
                    {
                        m_defaultProvider = new MockProvider();
                    }
                    else
                    {
                        m_defaultProvider = new AspNetProvider();
                    }
                    m_providerCollection.Add(m_defaultProvider);
                }
                else
                {
                    if (qc.DefaultProvider == null || qc.Providers == null || qc.Providers.Count < 1)
                    {
                        throw new System.Configuration.Provider.ProviderException("You must specify a valid default provider.");
                    }

                    //Instantiate the providers
                    ProvidersHelper.InstantiateProviders(qc.Providers, m_providerCollection, typeof(ProviderBase));
                    m_defaultProvider = m_providerCollection[qc.DefaultProvider];
                    if (m_defaultProvider == null)
                    {
                        throw new ConfigurationErrorsException("You must specify a default provider for the feature.",
                            qc.ElementInformation.Properties["defaultProvider"].Source,
                            qc.ElementInformation.Properties["defaultProvider"].LineNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                //initializationException = ex;
                isInitialized = false;
                throw ex;
            }

            isInitialized = true; //error-free initialization
        }

        //Public feature API
        private ProviderBase m_defaultProvider;
        private ProviderCollection m_providerCollection;

        /// <summary>
        /// 
        /// </summary>
        public ProviderBase DefaultProvider
        {
            get
            {
                if (!isInitialized)
                {
                    Initialize();
                }
                return m_defaultProvider;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ProviderCollection Providers
        {
            get
            {
                if (!isInitialized)
                {
                    Initialize();
                }
                return m_providerCollection;
            }
        }
    }
}