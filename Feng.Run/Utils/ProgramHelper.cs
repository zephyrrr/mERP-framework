using System;
using System.Collections.Generic;
using System.Text;
using Spring.Context;
using Spring.Context.Support;

namespace Feng.Utils
{
    public class ProgramHelper
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
                if (Microsoft.Practices.ServiceLocation.ServiceLocator.Current == null)
                {
                    IApplicationContext ctx = ContextRegistry.GetContext();
                    Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(new Microsoft.Practices.ServiceLocation.ServiceLocatorProvider(
                        delegate()
                        {
                            return new Microsoft.Practices.ServiceLocation.SpringAdapter.SpringServiceLocatorAdapter(ctx);
                        }));

                    // Spring.net 默认是Singleton的
                    IDataBuffer db = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IDataBuffer>();
                    if (db != null)
                    {
                        db.LoadData();
                    }
                }
            }
        }
    }
}
