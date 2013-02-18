using System;
using System.Collections.Generic;
using System.Text;
using Spring.Context;
using Spring.Context.Support;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProgramHelper
    {
        private static bool m_init;

        /// <summary>
        /// 初始化程序（设置ServiceLocator，读取IDataBuffer）
        /// </summary>
        public static void InitProgram()
        {
            if (!m_init)
            {
                m_init = true;

                var p = DefaultServiceProvider.Instance;
                p.EnableNHibernate();

                //p.SetDefaultService<ICache>(new HashtableCache());

                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(new Microsoft.Practices.ServiceLocation.ServiceLocatorProvider(
                delegate()
                {
                    return p;
                }));

                //ServiceProvider.SetDefaultService<IControlCollectionFactory>(new Feng.Windows.Forms.ControlCollectionFactoryWindows());

                //ServiceProvider.SetDefaultService<IManagerFactory>(new Feng.Utils.WinFormManagerFactory());

                XceedUtility.SetXceedLicense();
            }
        }
    }
}
