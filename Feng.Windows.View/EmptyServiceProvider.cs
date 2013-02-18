using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Feng
{
    public class EmptyServiceProvider : ServiceLocatorImplBase
    {
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return null;
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return null;
        }
    }
}
