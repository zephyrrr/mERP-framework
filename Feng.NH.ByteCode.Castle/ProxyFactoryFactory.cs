using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Bytecode;
using NHibernate.Proxy;

namespace Feng.NH.ByteCode.CacheCastle
{
    /// <summary>
    /// 不做检查。
    /// 如果做检查，方法属性必须是Virtual，Virtual则除了Id其他的都会调用LazyInitialize.Intercept。
    /// 现在改成不做检查，则如果方法属性是sealed，不会重载，不会调用LazyInitialize.Intercept。
    /// 例如BaseEntity.ToString()，不需要Lazylod，则变成sealed
    /// </summary>
    public class NoneProxyTypeValidator : NHibernate.Proxy.IProxyValidator
    {
        public bool IsProxeable(System.Reflection.MethodInfo method)
        {
            return true;
        }
        public ICollection<string> ValidateType(Type type)
        {
            return null;
        }
    }

    public class ProxyFactoryFactory : IProxyFactoryFactory
    {
        public IProxyFactory BuildProxyFactory()
        {
            return new ProxyFactory();
        }

        public IProxyValidator ProxyValidator
        {
            get
            {
                return new DynProxyTypeValidator();
            }
        }

        public bool IsInstrumented(Type entityClass)
        {
            // 必须要true，否则不能property Lazyload
            return true;
        }

        public bool IsProxy(object entity)
        {
            return (entity is INHibernateProxy);
        }
    }
}
