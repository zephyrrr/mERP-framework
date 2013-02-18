using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISessionFactoryManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <returns></returns>
        NHibernate.Cfg.Configuration GetConfigurationBySessionFactory(NHibernate.ISessionFactory sessionFactory);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        NHibernate.Cfg.Configuration GetDefaultConfiguration();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactoryName"></param>
        /// <returns></returns>
        NHibernate.ISessionFactory GetSessionFactory(string sessionFactoryName);

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, NHibernate.ISessionFactory> SessionFactories
        {
            get;
        }

        void DeleteSessionFactoryCache();
    }

    public interface ICacheConfigurationManager
    {
        void DeleteConfigurationCaches();

        NHibernate.Cfg.Configuration LoadConfigurationFromCache(string sessionFactoryConfigPath);

        void SaveConfigurationToCache(string sessionFactoryConfigPath, NHibernate.Cfg.Configuration configuration);
    }
}
