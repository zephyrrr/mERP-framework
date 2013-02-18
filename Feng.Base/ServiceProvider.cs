using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// ServiceProvider
    /// </summary>
    public static class ServiceProvider
    {
        private static Dictionary<Type, object> s_serviceExist = new Dictionary<Type, object>();
        /// <summary>
        /// Clear Inner Data
        /// </summary>
        public static void Clear()
        {
            s_serviceExist.Clear();
        }

        private static Microsoft.Practices.ServiceLocation.IServiceLocator serviceLocator;

        static ServiceProvider()
        {
            try
            {
                serviceLocator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// GetService
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
            where T: class
        {
            Type type = typeof(T);
            if (s_serviceExist.ContainsKey(type) && s_serviceExist[type] != null)
            {
                return (T)s_serviceExist[type];
            }

            T ret = null;

            if (serviceLocator != null)
            {
                // maybe not defined, it's ok
                try
                {
                    // singleton
                    ret = serviceLocator.GetInstance<T>();
                }
                catch (Exception)
                {
                }
                s_serviceExist[type] = ret;
            }
            if (ret != null)
            {
                return ret;
            }

//#if DEBUG
//            throw new InvalidOperationException(string.Format("Service {0} is not configured!", typeof(T).Name));
//#endif
            return null;
        }

        

        ///// <summary>
        ///// SetDefaultServiceType
        ///// 不同与SetDefaultService，此函数只有到获取的时候才会真正创建实例
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="defaultServiceType"></param>
        //public static void SetDefaultServiceType<T>(Type defaultServiceType)
        //{
        //    if (defaultServiceType.IsSubclassOf(typeof(T)))
        //    {
        //        throw new ArgumentException(string.Format("{0} should be subclass of {1}", defaultServiceType.ToString(), typeof(T).ToString()));
        //    }
        //    s_defaultServiceType[typeof(T)] = defaultServiceType;
        //}
    }
}
