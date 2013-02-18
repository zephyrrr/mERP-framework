using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRepositoryFactory
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //IRepository GenerateRepository();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="consumer"></param>
        ///// <returns></returns>
        //IRepository GenerateRepository(IRepositoryConsumer consumer);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="persistentClass"></param>
        ///// <returns></returns>
        //IRepository GenerateRepository(Type persistentClass);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //IRepository GenerateRepository<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repCfgName"></param>
        /// <returns></returns>
        IRepository GenerateRepository(string repCfgName = null);

        IRepository GetCurrentRepository();
    }

    /// <summary>
    /// 
    /// </summary>
    public static class RepositoryFactoryExtention
    {
        /// <summary>
        /// 根据IRepositoryConsumer生成Repository
        /// </summary>
        /// <param name="rf"></param>
        /// <param name="consumer"></param>
        /// <returns></returns>
        public static IRepository GenerateRepository(this IRepositoryFactory rf, IRepositoryConsumer consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException("consumer");

            return rf.GenerateRepository(consumer.RepositoryCfgName);
        }

        /// <summary>
        /// 根据类型生成Repository
        /// 根据类型所在的Assembly名称对应RepositoryConfigName
        /// </summary>
        /// <param name="rf"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRepository GenerateRepository<T>(this IRepositoryFactory rf)
        {
            string cfgName = Utils.RepositoryHelper.GetConfigNameFromType(typeof(T));
            return rf.GenerateRepository(cfgName);
        }

        /// <summary>
        /// 根据类型生成Repository
        /// </summary>
        /// <param name="rf"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IRepository GenerateRepository(this IRepositoryFactory rf, Type type)
        {
            string cfgName = Utils.RepositoryHelper.GetConfigNameFromType(type);
            return rf.GenerateRepository(cfgName);
        }
    }
}
