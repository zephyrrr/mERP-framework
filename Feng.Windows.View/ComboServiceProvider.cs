using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Feng
{
    public class ComboServiceProvider : ServiceLocatorImplBase
    {
        public ComboServiceProvider(params ServiceLocatorImplBase[] list)
        {
            foreach (var i in list)
                m_serviceProviders.Add(i);
        }

        private List<ServiceLocatorImplBase> m_serviceProviders = new List<ServiceLocatorImplBase>();
        public void Add(ServiceLocatorImplBase provider)
        {
            m_serviceProviders.Add(provider);
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            foreach (var i in m_serviceProviders)
            {
                var r = i.GetInstance(serviceType, key);
                if (r != null)
                    return r;
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
            foreach (var i in m_serviceProviders)
            {
                var r = i.GetAllInstances(serviceType);
                if (r != null)
                    return r;
            }
            return null;
        }
    }
}
