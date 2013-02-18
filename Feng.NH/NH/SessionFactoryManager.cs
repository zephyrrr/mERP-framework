using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;

namespace Feng.NH
{
    /// <summary>
    /// SessionFactory管理器
    /// 程序支持多SessionFactory，默认是根据实体类类型（命名空间）来创建不同SessionFactory。如果找不到相应的SessionFactory，用默认SessionFactory。
    /// (Obsolete: 如果有特殊需要，可用Feng.RepositoryConfigNameAttribute)
    /// 如有特殊需要，采用WindowTabInfo中的RepositoryConfigName
    /// </summary>
    public class SessionFactoryManager : ISessionFactoryManager, ICacheConfigurationManager //Singleton<NHibernateSessionFactoryManager>
    {
        private Dictionary<string, ISessionFactory> m_sessionFactories = new Dictionary<string, ISessionFactory>();
        private Dictionary<string, Configuration> m_configurations = new Dictionary<string, Configuration>();

        /// <summary>
        /// GetConfigurationBySessionFactory
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <returns></returns>
        public Configuration GetConfigurationBySessionFactory(ISessionFactory sessionFactory)
        {
            foreach (KeyValuePair<string, ISessionFactory> kvp in m_sessionFactories)
            {
                if (kvp.Value == sessionFactory)
                {
                    if (m_configurations.ContainsKey(kvp.Key))
                        return m_configurations[kvp.Key];
                }
            }
            return null;
        }

        private const string s_appSessionFactoryConfigPath = "app.config";

        private static string s_defaultSessionFactoryName;
        /// <summary>
        /// 默认SessionFactory名称
        /// </summary>
        public string DefaultSessionFactoryName
        {
            get 
            {
                if (string.IsNullOrEmpty(s_defaultSessionFactoryName))
                {
                    RetriveDefaultSessionFactoryName();
                }
                return s_defaultSessionFactoryName;
            }
        }

        protected virtual SessionFactoriesConfigSection GetSessionFactoriesConfigSection()
        {
            return System.Configuration.ConfigurationManager.GetSection("nhibernateSettings") as SessionFactoriesConfigSection;
        }

        private void RetriveDefaultSessionFactoryName()
        {
            SessionFactoriesConfigSection sfcs = GetSessionFactoriesConfigSection();

            if (sfcs == null)
            {
                s_defaultSessionFactoryName = s_appSessionFactoryConfigPath;
                return;
            }

            foreach (SessionFactoriesConfigSectionElement element in sfcs.SessionFactories)
            {
                if (element.IsDefault)
                {
                    s_defaultSessionFactoryName = element.Name;
                    return;
                }
            }
        }

        /// <summary>
        /// SessionFactory列表
        /// </summary>
        public Dictionary<string, ISessionFactory> SessionFactories
        {
            get { return m_sessionFactories; }
        }

        /// <summary>
        /// 根据名称得到SessionFactory
        /// sessionFactoryName=repCfgName, 可为空（此时返回默认)
        /// </summary>
        /// <param name="sessionFactoryName"></param>
        /// <returns></returns>
        public ISessionFactory GetSessionFactory(string sessionFactoryName)
        {
            if (string.IsNullOrEmpty(sessionFactoryName))
            {
                return GetSessionFactoryByConfigPath(this.DefaultSessionFactoryName);
            }

            SessionFactoriesConfigSection sfcs = GetSessionFactoriesConfigSection();
            if (sfcs == null)
            {
                return GetSessionFactoryByConfigPath(s_appSessionFactoryConfigPath);
            }
            else if (sfcs.SessionFactories[sessionFactoryName] == null)
            {
                return GetSessionFactoryByConfigPath(sfcs.SessionFactories[DefaultSessionFactoryName].FactoryConfigPath);
            }
            else
            {
                return GetSessionFactoryByConfigPath(sfcs.SessionFactories[sessionFactoryName].FactoryConfigPath);
            }
        }

        private void SetDefaultProperties(Configuration cfg)
        {
            cfg.DataBaseIntegration(db =>
                {
                    db.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();
                    db.Dialect<NHibernate.Dialect.MsSql2008Dialect>();
                    db.Driver<NHibernate.Driver.SqlClientDriver>();
                    db.ConnectionStringName = "DataConnectionString";
                    db.ExceptionConverter<MsSqlExceptionConverter>();
                    db.IsolationLevel = System.Data.IsolationLevel.ReadCommitted;
                    db.LogSqlInConsole = false;
                    db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    db.Timeout = 255;
                    //db.SchemaAction = null;
                });
            cfg.SetProperty("hbm2ddl.auto", "none");
            //cfg.SetProperty("use_proxy_validator", "false");

            cfg.Cache(cache =>
                {
                    cache.Provider<NHibernate.Cache.HashtableCacheProvider>();
                    cache.UseQueryCache = false;
                });
            cfg.SetProperty("cache.use_second_level_cache", "false");

            cfg.Proxy(proxy =>
                {
                    //proxy.ProxyFactoryFactory<Feng.NH.ByteCode.CacheLinFu.ProxyFactoryFactory>();
                    proxy.ProxyFactoryFactory<NHibernate.Bytecode.DefaultProxyFactoryFactory>();
                    proxy.Validation = false;
                });
            //cfg.SetProperty("proxyfactory.factory_class", "Feng.NH.ByteCode.CacheCastle.ProxyFactoryFactory, Feng.NH.ByteCode.CacheCastle");
            //cfg.SetProperty("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");

            cfg.SetProperty("generate_statistics", "false");

            cfg.LinqToHqlGeneratorsRegistry<LinqToHqlGeneratorsRegistry>();

          //<property name="adonet.batch_size">10</property>
          // <property name="command_timeout">60</property>
          //<property name="query.substitutions">true 1, false 0, yes 'Y', no 'N'</property>
          //<property name="cache.provider_class">NHibernate.Caches.SysCache.SysCacheProvider, NHibernate.Caches.SysCache, Version=2.0.0.1001</property>
          //max_fetch_depth=1, 则ManyToOne只取得一层，对于继承的，只取得基类信息。所以有用的信息放在基类。
          //<property name="max_fetch_depth">3</property>
          
          //<property name="format_sql">true</property>
          //<property name="use_sql_comments">true</property>
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactoryConfigPath"></param>
        /// <returns></returns>
        protected virtual Configuration BuildConfig(string sessionFactoryConfigPath)
        {
            Configuration cfg = LoadConfigurationFromCache(sessionFactoryConfigPath);
            if (cfg == null)
            {
                cfg = new Configuration();
                if (string.IsNullOrEmpty(cfg.GetProperty("connection.provider")))
                {
                    SetDefaultProperties(cfg);
                }

                //if (sessionFactoryConfigPath == s_emptySessionFactoryConfigPath)
                //{
                //    AddDefaultMappings(cfg);
                //}
                //else
                {
                    if (sessionFactoryConfigPath == s_appSessionFactoryConfigPath)
                    {
                        IHibernateConfiguration section = System.Configuration.ConfigurationManager.GetSection("hibernate-configuration") as IHibernateConfiguration;
                        if (section != null)
                        {
                            cfg.Configure();
                        }
                    }
                    else
                    {
                        cfg.Configure(ServiceProvider.GetService<IApplicationDirectory>().GetFullPath(sessionFactoryConfigPath));
                    }
                    //AddMappings(sessionFactoryConfigPath, cfg);
                    AddDefaultMappings(cfg);
                }

                SaveConfigurationToCache(sessionFactoryConfigPath, cfg);
            }

            m_configurations[sessionFactoryConfigPath] = cfg;

            cfg.SetProperty("Name", sessionFactoryConfigPath);
            return cfg;
        }

        private void AddDefaultMappings(Configuration cfg)
        {
            try
            {
                cfg.AddResource("Feng.Domain.hbm.xml", Assembly.Load("Feng.Windows.Model"));
                //cfg.AddResource("Feng.Gps.Domain.hbm.xml", Assembly.Load("Feng.Gps"));
            }
            catch (Exception)
            {
            }
        }

        //private void AddMappings(string sessionFactoryConfigPath, Configuration cfg)
        //{
        //    NHibernateRepositoryMappingSection repositoryConfig = System.Configuration.ConfigurationManager
        //        .GetSection("repository") as NHibernateRepositoryMappingSection;

        //    if (repositoryConfig != null)
        //    {
        //        foreach (NHibernateRepositoryMappingElement mappingElement in repositoryConfig.MappingAssemblies)
        //        {
        //            if (!string.IsNullOrEmpty(mappingElement.FactoryConfigPath)
        //                && !mappingElement.FactoryConfigPath.Contains(sessionFactoryConfigPath))
        //                continue;

        //            switch (mappingElement.Type.ToUpper())
        //            {
        //                case "ASSEMBLY":
        //                    cfg.AddAssembly(mappingElement.Name);
        //                    break;
        //                case "RESOURCE":
        //                    string[] ss = mappingElement.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //                    cfg.AddResource(ss[0], Assembly.Load(ss[1]));
        //                    break;
        //                case "ATTRIBUTE":
        //                    NHibernateHelper.MemorizeMappingAttribute(cfg, mappingElement.Name);
        //                    break;
        //                case "HBMFILE":
        //                    cfg.AddFile(mappingElement.Name);
        //                    break;
        //                default:
        //                    throw new NotSupportedException("Invalid nhibernate assembly type");
        //            }
        //        }
        //        //throw new ConfigurationErrorsException("Repository configuration cannot be null.");
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public void DeleteSessionFactoryCache()
        {
            m_sessionFactories.Clear();
        }

        /// <summary>
        /// DeleteConfigurationCache
        /// </summary>
        public void DeleteConfigurationCaches()
        {
            foreach(var file in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory()))
            {
                if (file.EndsWith(SerializedConfiguration))
                {
                    System.IO.File.Delete(file);
                }
            }
            
            m_configurations.Clear();
        }


        public Configuration LoadConfigurationFromCache(string sessionFactoryConfigPath)
        {
            string fileName = GetConfigCacheFileName(sessionFactoryConfigPath);
            if (!System.IO.File.Exists(fileName))
                return null;
            if (IsConfigurationFileValid(sessionFactoryConfigPath, fileName) == false)
            {
                System.IO.File.Delete(fileName);
                return null;
            }
            try
            {
                using (var file = File.Open(fileName, FileMode.Open))
                {
                    var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return bf.Deserialize(file) as Configuration;
                }
            }
            catch (Exception)
            {
                System.IO.File.Delete(fileName);
                return null;
            }
        }

        private string GetConfigCacheFileName(string sessionFactoryConfigPath)
        {
            string fileName = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), 
                string.Format("{0}_{1}.{2}", sessionFactoryConfigPath, SystemConfiguration.ApplicationName, SerializedConfiguration));
            return fileName;
        }
        public void SaveConfigurationToCache(string sessionFactoryConfigPath, Configuration configuration)
        {
            try
            {
                string fileName = GetConfigCacheFileName(sessionFactoryConfigPath);
                using (var file = File.Open(fileName, FileMode.Create))
                {
                    var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    bf.Serialize(file, configuration);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        private const string SerializedConfiguration = "configurtion.serialized";
        private bool IsConfigurationFileValid(string sessionFactoryConfigPath, string serializeFileName)
        {
            var ass = Assembly.GetCallingAssembly();
            if (ass.Location == null)
                return false;
            var configInfo = new FileInfo(serializeFileName);
            var assInfo = new FileInfo(ass.Location);
            var configFileInfo = new FileInfo(sessionFactoryConfigPath);
            if (configInfo.LastWriteTime < assInfo.LastWriteTime)
                return false;
            if (configInfo.LastWriteTime < configFileInfo.LastWriteTime)
                return false;
            return true;
        }

        //private const string s_emptySessionFactoryConfigPath = "empty.config";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactoryConfigPath"></param>
        /// <returns></returns>
        protected bool IsConstConfigPath(string sessionFactoryConfigPath)
        {
            return string.IsNullOrEmpty(sessionFactoryConfigPath) 
                //|| sessionFactoryConfigPath == s_emptySessionFactoryConfigPath
                || sessionFactoryConfigPath == s_appSessionFactoryConfigPath;
        }

        /// <summary>
        /// This method attempts to find a session factory stored in <see cref="m_sessionFactories" />
        /// via its name; if it can't be found it creates a new one and adds it the hashtable.
        /// </summary>
        /// <param name="sessionFactoryConfigPath">Path location of the factory config</param>
        public ISessionFactory GetSessionFactoryByConfigPath(string sessionFactoryConfigPath)
        {
            //Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
            //    "sessionFactoryConfigPath may not be null nor empty");

            //  Attempt to retrieve a stored SessionFactory from the hashtable.
            ISessionFactory sessionFactory = null;

            lock (m_sessionFactories)
            {
                if (string.IsNullOrEmpty(sessionFactoryConfigPath))
                {
                    sessionFactoryConfigPath = this.DefaultSessionFactoryName;
                }

                //  Failed to find a matching SessionFactory so make a new one.
                if (!m_sessionFactories.ContainsKey(sessionFactoryConfigPath))
                {
                    //Check.Require(File.Exists(sessionFactoryConfigPath),
                    //    "The config file at '" + sessionFactoryConfigPath + "' could not be found");

                    Configuration cfg = BuildConfig(sessionFactoryConfigPath);
                    
                    //  Now that we have our Configuration object, create a new SessionFactory
                    sessionFactory = cfg.BuildSessionFactory();

                    if (sessionFactory == null)
                    {
                        throw new ApplicationException("cfg.BuildSessionFactory() returned null.");
                    }

                    m_sessionFactories.Add(sessionFactoryConfigPath, sessionFactory);
                }
            }

            return m_sessionFactories[sessionFactoryConfigPath];
        }

        

        ///// <summary>
        ///// 根据类型得到SessionFactory
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public ISessionFactory GetSessionFactory(Type type)
        //{
        //    string s = RepositoryHelper.GetConfigNameFromType(type);
        //    ISessionFactory sessionFactory = GetSessionFactory(s);
        //    return sessionFactory;
        //}

        /// <summary>
        /// 根据配置文件路径得到Configuration
        /// </summary>
        /// <param name="sessionFactoryConfigPath"></param>
        /// <returns></returns>
        public Configuration GetConfiguration(string sessionFactoryConfigPath)
        {
            if (m_configurations.ContainsKey(sessionFactoryConfigPath))
                return m_configurations[sessionFactoryConfigPath];
            else
            {
                BuildConfig(sessionFactoryConfigPath);
                return m_configurations[sessionFactoryConfigPath];
            }
        }

        /// <summary>
        /// 得到默认Configuration
        /// </summary>
        /// <returns></returns>
        public Configuration GetDefaultConfiguration()
        {
            return GetConfiguration(DefaultSessionFactoryName);
        }
    }
}
