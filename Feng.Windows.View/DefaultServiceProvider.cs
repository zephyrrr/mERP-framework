using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    public class DefaultServiceProvider : MemoryServiceProvider
    {
        private MemoryServiceProvider m_sp = null;

        private static DefaultServiceProvider s_instance;
        public static DefaultServiceProvider Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new DefaultServiceProvider();

                return s_instance;
            }
        }

        private DefaultServiceProvider()
            : this(false)
        {
        }

        private DefaultServiceProvider(bool enableAll)
        {
            m_sp = this;

            m_sp.SetDefaultService<IApplicationDirectory>(new Feng.Windows.WindowsDirectory());
            m_sp.SetDefaultService<IExceptionProcess>(new Feng.Windows.Forms.WinFormExceptionProcess());
            m_sp.SetDefaultService<IMessageBox>(new Feng.Windows.Forms.MyMessageBox());

            if (enableAll)
            {
                EnableAll();
            }
        }

        public void EnableAll()
        {
            this.EnableLog();
            if (System.Configuration.ConfigurationManager.ConnectionStrings[Feng.Windows.Utils.SecurityHelper.DataConnectionStringName] == null)
            {
                this.EnableWS();
            }
            else
            {
                this.EnableNHibernate();
            }
            this.EnableScript();
            this.EnableCache();
            this.EnableApplication();
        }

        public void EnableWS()
        {
            m_sp.SetDefaultService<IRepositoryFactory>(new Feng.Windows.Net.AuthRepositoryFactory());
        }

        public void EnableNHibernate()
        {
            m_sp.SetDefaultService<Feng.NH.ISessionFactoryManager>(new Feng.NH.SessionFactoryManager());
            m_sp.SetDefaultService<IRepositoryFactory>(new Feng.NH.RepositoryFactory());

            m_sp.SetDefaultService<IEntityMetadataGenerator>(new Feng.NH.NHDataEntityMetadataGenerator());

            //ServiceProvider.SetDefaultService<NHibernate.IInterceptor>(new Feng.NH.DeleteLogInterceptor());
            //ServiceProvider.SetDefaultService<Feng.NH.ISessionFactoryManager>(new Feng.ResourceSessionFactoryManager());
        }

        public void EnableLog()
        {
            log4net.Config.XmlConfigurator.Configure();
            //HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize(); 
            m_sp.SetDefaultService<ILogger>(new Feng.Windows.log4netLogger());
        }

        public void EnableCache()
        {
            IPersistentCache c = new Feng.Windows.PersistentHashtableCache();
            m_sp.SetDefaultService<ICache>(c);
            m_sp.SetDefaultService<IPersistentCache>(c);

            m_sp.SetDefaultService<IDefinition>(DBDef.Instance);

            m_sp.SetDefaultService<IDataBuffer>(DBDataBuffer.Instance);

            IDataBuffers bufs = new DataBuffers();
            bufs.AddDataBuffer(new Cache());
            bufs.AddDataBuffer(DBDataBuffer.Instance);
            bufs.AddDataBuffer(DBDef.Instance);
            m_sp.SetDefaultService<IDataBuffers>(bufs);
        }

        public void EnableApplication()
        {
            m_sp.SetDefaultService<IControlCollectionFactory>(new Feng.Windows.Forms.ControlCollectionFactoryWindows());
            m_sp.SetDefaultService<IManagerFactory>(new Feng.Windows.Utils.WinFormManagerFactory());
            m_sp.SetDefaultService<IWindowFactory>(new Feng.Windows.Utils.ArchiveFormFactory());

            if (SystemConfiguration.ApplicationName.ToLower() == "jkhd"
                || SystemConfiguration.ApplicationName.ToLower() == "hd"
                || SystemConfiguration.ApplicationName.ToLower() == "cd")
            {
                m_sp.SetDefaultService<IApplication>(new Feng.Windows.Forms.TabbedMdiForm());
            }
            else if (SystemConfiguration.ApplicationName.ToLower() == "zkzx")
            {
                m_sp.SetDefaultService<IApplication>(new Feng.Windows.Forms.TabbedMdiForm2());
            }
            else
            {
                m_sp.SetDefaultService<IApplication>(new Feng.Windows.Forms.TabbedMdiForm());
            }
        }

        public void EnableScript(bool? usePython = null)
        {
            if (!usePython.HasValue)
            {
                if (SystemConfiguration.ApplicationName.ToLower() == "jkhd"
                    || SystemConfiguration.ApplicationName.ToLower() == "hd"
                    || SystemConfiguration.ApplicationName.ToLower() == "cd")
                {
                    usePython = false;
                }
                else
                {
                    usePython = true;
                }
            }

            PythonEntityScript pythonScript = new PythonEntityScript();
            m_sp.SetDefaultService<IFileScript>(pythonScript);
            if (!usePython.Value)
            {
                IEntityScript script = new PythonandEvalutionEngineEntityScript();
                m_sp.SetDefaultService<IScript>(script);
                m_sp.SetDefaultService<IEntityScript>(script);
            }
            else
            {
                m_sp.SetDefaultService<IScript>(pythonScript);
                m_sp.SetDefaultService<IEntityScript>(pythonScript);
            }
        }
    }
}
