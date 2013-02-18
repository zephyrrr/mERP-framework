using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceSessionFactoryManager : Feng.NH.SessionFactoryManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceSessionFactoryManager()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Feng.NH.SessionFactoriesConfigSection GetSessionFactoriesConfigSection()
        {
            Feng.NH.SessionFactoriesConfigSection fileConfig = base.GetSessionFactoriesConfigSection();
            if (fileConfig != null)
                return fileConfig;

            ResourceContent rc = Feng.Windows.Utils.ResourceInfoHelper.ResolveResource("sessionfactory.config", ResourceType.Config, true);
            if (rc != null)
            {
                switch (rc.Type)
                {
                    case ResourceContentType.File:
                        ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = rc.Content.ToString() };
                        return System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None).GetSection("nhibernateSettings") as Feng.NH.SessionFactoriesConfigSection;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactoryConfigPath"></param>
        /// <returns></returns>
        protected override NHibernate.Cfg.Configuration BuildConfig(string sessionFactoryConfigPath)
        {
            if (!base.IsConstConfigPath(sessionFactoryConfigPath) && !System.IO.File.Exists(sessionFactoryConfigPath))
            {
                ResourceContent rc = Feng.Windows.Utils.ResourceInfoHelper.ResolveResource(sessionFactoryConfigPath, ResourceType.Config);
                if (rc != null && rc.Type == ResourceContentType.File)
                {
                    sessionFactoryConfigPath = rc.Content.ToString();
                }
            }

            return base.BuildConfig(sessionFactoryConfigPath);
        }
    }
}
