using System;
using System.Collections.Generic;
using System.Text;
using Spring.Context;
using Spring.Context.Support;

namespace Feng.Server
{
    public static class ProgramHelper
    {
        static ProgramHelper()
        {
            SystemConfiguration.LiteMode = true;
        }

        private static bool m_init;

        private static void SetSpringServiceLocator()
        {
            Spring.Context.IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(new Microsoft.Practices.ServiceLocation.ServiceLocatorProvider(
                delegate()
                {
                    return new Microsoft.Practices.ServiceLocation.SpringAdapter.SpringServiceLocatorAdapter(ctx);
                }));
        }

        /// <summary>
        /// 初始化程序（设置ServiceLocator，读取IDataBuffer）
        /// </summary>
        public static void InitProgram()
        {
            if (!m_init)
            {
                m_init = true;

                Feng.Utils.XceedUtility.SetXceedLicense();
                log4net.Config.XmlConfigurator.Configure();
                //HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize(); 

                //if (Microsoft.Practices.ServiceLocation.ServiceLocator.Current == null) // throw null
                {
                    if (!SystemConfiguration.LiteMode)
                    {
                        SetSpringServiceLocator();
                    }

                    ServiceProvider.SetDefaultService<ILogger>(new log4netLogger());
                    ServiceProvider.SetDefaultService<IExceptionProcess>(new LoggerExceptionProcess());

                    ServiceProvider.SetDefaultService<Feng.NH.ISessionFactoryManager>(new Feng.NH.NHibernateSessionFactoryManager());
                    ServiceProvider.SetDefaultService<IRepositoryFactory>(new NH.RepositoryFactory());

                    IPersistentCache c = new PersistentHashtableCache();
                    ServiceProvider.SetDefaultService<ICache>(c);
                    ServiceProvider.SetDefaultService<IPersistentCache>(c);

                    Feng.DBDef def = new Feng.DBDef();
                    ServiceProvider.SetDefaultService<IDefinition>(def);

                    IDataBuffer buf = new Feng.DBDataBuffer();
                    ServiceProvider.SetDefaultService<IDataBuffer>(buf);

                    IDataBuffers bufs = new DataBuffers();
                    bufs.AddDataBuffer(new Cache());
                    bufs.AddDataBuffer(buf);
                    bufs.AddDataBuffer(def);
                    ServiceProvider.SetDefaultService<IDataBuffers>(bufs);

                    IEntityScript script = new PythonandEvalutionEngineScript();
                    ServiceProvider.SetDefaultService<IScript>(script);
                    ServiceProvider.SetDefaultService<IEntityScript>(script);

                    ServiceProvider.SetDefaultService<IMessageBox>(new EmptyMessageBox());

                    ServiceProvider.SetDefaultService<IControlCollectionFactory>(new Feng.ControlCollectionFactoryBase());

                    ServiceProvider.SetDefaultService<IManagerFactory>(new Feng.Utils.ManagerFactory());

                    IDataBuffer db = ServiceProvider.GetService<IDataBuffer>();
                    if (db != null)
                    {
                        db.LoadData();
                    }
                }
            }
        }
    }
}
