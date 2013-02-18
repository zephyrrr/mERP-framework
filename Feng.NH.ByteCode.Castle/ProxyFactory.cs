using System;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Proxy;
using log4net;
using Castle.DynamicProxy;
using Feng;

namespace Feng.NH.ByteCode.CacheCastle
{
    public class ProxyFactory : AbstractProxyFactory
    {
        private static readonly Castle.DynamicProxy.ProxyGenerator ProxyGenerator = new Castle.DynamicProxy.ProxyGenerator();
        protected static readonly ILog log = LogManager.GetLogger(typeof(NHibernate.ByteCode.Castle.ProxyFactory));

        protected static ProxyGenerator DefaultProxyGenerator
        {
            get
            {
                return ProxyGenerator;
            }
        }

        //public override object GetFieldInterceptionProxy()
        //{
        //    ProxyGenerationOptions options = new ProxyGenerationOptions();
        //    NHibernate.ByteCode.Castle.LazyFieldInterceptor instance = new NHibernate.ByteCode.Castle.LazyFieldInterceptor();
        //    options.AddMixinInstance(instance);
        //    return ProxyGenerator.CreateClassProxy(this.PersistentClass, options, new Castle.DynamicProxy.IInterceptor[] { instance });
        //}

        public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
        {
            INHibernateProxy proxy;
            try
            {
                NHibernate.ByteCode.Castle.LazyInitializer initializer = new NHibernate.ByteCode.Castle.LazyInitializer(this.EntityName, this.PersistentClass, id, this.GetIdentifierMethod, this.SetIdentifierMethod, this.ComponentIdType, session);
                object obj2 = base.IsClassProxy ? ProxyGenerator.CreateClassProxy(this.PersistentClass, this.Interfaces, new Castle.DynamicProxy.IInterceptor[] { initializer }) : ProxyGenerator.CreateInterfaceProxyWithoutTarget(this.Interfaces[0], this.Interfaces, new Castle.DynamicProxy.IInterceptor[] { initializer });
                initializer._constructed = true;
                proxy = (INHibernateProxy)obj2;

                object target = TryGetCachedTarget(this.PersistentClass, id);
                if (target != null)
                {
                    initializer.SetImplementation(target);
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
