using System;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Impl;
using NHibernate.Engine;
using NHibernate.Cfg;
using NHibernate.Type;
using NHibernate.Criterion;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

namespace Feng.NH
{
    public static class SessionExtensions
    {
        public static IEntityPersister GetEntityPersister(this ISession session, Object entity, out EntityEntry oldEntry)
        {
            ISessionImplementor sessionImpl = session.GetSessionImplementation();
            IPersistenceContext persistenceContext = sessionImpl.PersistenceContext;
            oldEntry = persistenceContext.GetEntry(entity);

            if ((oldEntry == null) && (entity is INHibernateProxy))
            {
                INHibernateProxy proxy = entity as INHibernateProxy;
                Object obj = sessionImpl.PersistenceContext.Unproxy(proxy);
                oldEntry = sessionImpl.PersistenceContext.GetEntry(obj);
            }
            string className;
            if (oldEntry != null)
            {
                className = oldEntry.EntityName;
            }
            else
            {
                className = sessionImpl.Factory.GetClassMetadata(entity.GetType()).EntityName;
            }
            IEntityPersister persister = sessionImpl.Factory.GetEntityPersister(className);
            return persister;
        }

        public static Boolean IsDirtyEntity(this ISession session, Object entity)
        {
            EntityEntry oldEntry;
            IEntityPersister persister = GetEntityPersister(session, entity, out oldEntry);

            Object[] oldState = oldEntry.LoadedState;
            Object[] currentState = persister.GetPropertyValues(entity, session.GetSessionImplementation().EntityMode);

            Int32[] dirtyProps = oldState.Select((o, i) => (oldState[i] == currentState[i]) ? -1 : i).Where(x => x >= 0).ToArray();
            return (dirtyProps != null);
        }

        public static Boolean IsDirtyProperty(this ISession session, Object entity, String propertyName)
        {
            EntityEntry oldEntry;
            IEntityPersister persister = GetEntityPersister(session, entity, out oldEntry);

            Object[] oldState = oldEntry.LoadedState;
            Object[] currentState = persister.GetPropertyValues(entity, session.GetSessionImplementation().EntityMode);
            Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, session.GetSessionImplementation());

            Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);
            Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;
            return (isDirty);
        }

        public static bool HasProxy(this ISession session, Object entity)
        {
            EntityEntry oldEntry;
            IEntityPersister persister = GetEntityPersister(session, entity, out oldEntry);

            Object[] currentState = persister.GetPropertyValues(entity, session.GetSessionImplementation().EntityMode);

            foreach (var i in currentState)
            {
                // NHibernateUtil.IsInitialized
                if (NHibernateHelper.IsProxy(i))
                    return true;
            }
            return false;
        }

        public static Object GetOriginalEntityProperty(this ISession session, Object entity, String propertyName)
        {
            EntityEntry oldEntry;
            IEntityPersister persister = GetEntityPersister(session, entity, out oldEntry);

            Object[] oldState = oldEntry.LoadedState;
            Object[] currentState = persister.GetPropertyValues(entity, session.GetSessionImplementation().EntityMode);

            Int32[] dirtyProps = persister.FindDirty(currentState, oldState, entity, session.GetSessionImplementation());
            Int32 index = Array.IndexOf(persister.PropertyNames, propertyName);

            Boolean isDirty = (dirtyProps != null) ? (Array.IndexOf(dirtyProps, index) != -1) : false;
            return ((isDirty == true) ? oldState[index] : currentState[index]);
        }

    }
}
