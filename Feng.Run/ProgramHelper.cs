using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Utils;

namespace Feng.Run
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProgramHelper
    {
        private static bool m_init;

        private static void SetSpringServiceLocator()
        {
            Spring.Context.IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();

            DefaultServiceProvider.Instance.EnableAll();
            var sp = new ComboServiceProvider(
                        new Microsoft.Practices.ServiceLocation.SpringAdapter.SpringServiceLocatorAdapter(ctx),
                        DefaultServiceProvider.Instance);
            
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(new Microsoft.Practices.ServiceLocation.ServiceLocatorProvider(
                delegate()
                {
                    return sp;
                }));
        }
        private static void SetDefaultServiceLocator()
        {
            var sp =  DefaultServiceProvider.Instance;
            DefaultServiceProvider.Instance.EnableAll();
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(new Microsoft.Practices.ServiceLocation.ServiceLocatorProvider(
                delegate()
                {
                    return sp;
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

                //if (Microsoft.Practices.ServiceLocation.ServiceLocator.Current == null) // throw null
                {
                    if (!SystemConfiguration.LiteMode)
                    {
                        SetSpringServiceLocator();
                    }
                    else
                    {
                        SetDefaultServiceLocator();
                    }
                }

                XceedUtility.SetXceedLicense();
            }
        }
    }
}
