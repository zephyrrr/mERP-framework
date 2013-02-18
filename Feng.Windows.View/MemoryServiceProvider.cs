using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Feng
{
    public class MemoryServiceProvider : ServiceLocatorImplBase
    {
        
        private Dictionary<Type, object> s_defaultService = new Dictionary<Type, object>();
        private Dictionary<Type, Type> s_defaultServiceType = new Dictionary<Type, Type>();

        /// <summary>
        /// SetDefaultService
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultService"></param>
        public void SetDefaultService<T>(T defaultService)
        {
            s_defaultService[typeof(T)] = defaultService;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            Type type = serviceType;

            if (s_defaultService.ContainsKey(type) && s_defaultService[type] != null)
            {
                return s_defaultService[type];
            }
            if (s_defaultServiceType.ContainsKey(type))
            {
                object newInstance = Feng.Utils.ReflectionHelper.CreateInstanceFromType(s_defaultServiceType[type]);
                if (newInstance != null)
                {
                    s_defaultService[type] = newInstance;
                    return newInstance;
                }
            }

            return null;
        }

        /// <summary>
        /// Resolves service instances by type.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects matching the <paramref name="serviceType"/>.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotSupportedException("Not supported in MemoryServiceProvider now.");
        }
    }
}
