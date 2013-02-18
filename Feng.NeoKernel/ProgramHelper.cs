using System;
using System.Collections.Generic;
using System.Text;
using Spring.Context;
using Spring.Context.Support;

namespace Feng.NeoKernel
{
    public static class ProgramHelper
    {
        static ProgramHelper()
        {
            SystemConfiguration.LiteMode = true;
        }

        private static bool m_init;

        /// <summary>
        /// 初始化程序（设置ServiceLocator，读取IDataBuffer）
        /// </summary>
        public static void InitProgram()
        {
            if (!m_init)
            {
#if DEBUG
                Console.WriteLine("Please press any key to continue...");
                Console.ReadLine();
#endif
                m_init = true;

                var p = DefaultServiceProvider.Instance;
                p.EnableLog();
                p.EnableNHibernate();
                p.EnableCache();
                p.EnableScript(false);

                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(new Microsoft.Practices.ServiceLocation.ServiceLocatorProvider(
                delegate()
                {
                    return p;
                }));


                IDataBuffer db = ServiceProvider.GetService<IDataBuffer>();
                if (db != null)
                {
                    db.LoadData();
                }
            }
        }
    }
}
