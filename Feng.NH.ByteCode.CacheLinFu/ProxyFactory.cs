using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Proxy;
using log4net;
using LinFu.DynamicProxy;
using Feng;

namespace Feng.NH.ByteCode.CacheLinFu
{
    public class ProxyFactory : AbstractProxyFactory
    {
        private static readonly LinFu.DynamicProxy.ProxyFactory factory = new LinFu.DynamicProxy.ProxyFactory();
        protected static readonly ILog log = LogManager.GetLogger(typeof(NHibernate.ByteCode.LinFu.ProxyFactory));

        public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
        {
            INHibernateProxy proxy;
            try
            {
                NHibernate.ByteCode.LinFu.LazyInitializer interceptor = new NHibernate.ByteCode.LinFu.LazyInitializer(this.EntityName, this.PersistentClass, id, this.GetIdentifierMethod, this.SetIdentifierMethod, this.ComponentIdType, session);
                //CacheLazyInitializer interceptor = new CacheLazyInitializer(this.EntityName, this.PersistentClass, id, this.GetIdentifierMethod, this.SetIdentifierMethod, this.ComponentIdType, session);
                object obj2 = base.IsClassProxy ? factory.CreateProxy(this.PersistentClass, interceptor, this.Interfaces) : factory.CreateProxy(this.Interfaces[0], interceptor, this.Interfaces);
                proxy = (INHibernateProxy)obj2;

                object target = TryGetCachedTarget(this.PersistentClass, id);
                if (target != null)
                {
                    interceptor.SetImplementation(target);
                }
            }
            catch (Exception exception)
            {
                log.Error("Creating a proxy instance failed", exception);
                throw new HibernateException("Creating a proxy instance failed", exception);
            }
            return proxy;
        }

        private object TryGetCachedTarget(Type persistentClass, object id)
        {
            IEntityBuffer eb = EntityBufferCollection.Instance[persistentClass];
            if (eb == null)
            {
                return null;
            }
            return eb.Get(id);
        }
    }
}
