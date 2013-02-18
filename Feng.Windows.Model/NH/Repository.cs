using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.NH
{
    public class Repository : SimpleRepository
    {
        public Repository(string sessionFactoryConfigName)
            : base(sessionFactoryConfigName)
        {
        }

        private Dictionary<object, object> m_updatedEntities = new Dictionary<object, object>();

        public override void Update(object entity)
        {
            base.Update(entity);

            if (entity is IVersionedEntity)
            {
                m_updatedEntities[entity] = (entity as IVersionedEntity).Version;
            }
            else if (entity is ICloneable)
            {
                m_updatedEntities[entity] = (entity as ICloneable).Clone();
            }
        }

        public override void BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            base.BeginTransaction(isolationLevel);

            m_updatedEntities.Clear();
            //return _transaction;
        }

        public override void CommitTransaction()
        {
            base.CommitTransaction();

            m_updatedEntities.Clear();
        }

        /// <summary>
        /// Rolls back an NHibernate transaction.
        /// </summary>
        public override void RollbackTransaction()
        {
            base.RollbackTransaction();

            foreach (KeyValuePair<object, object> kvp in m_updatedEntities)
            {
                if (kvp.Key is IVersionedEntity)
                {
                    (kvp.Key as IVersionedEntity).Version = (int)kvp.Value;
                    //Feng.Utils.ReflectionHelper.SetObjectValue(kvp.Key, "Version", Feng.Utils.ReflectionHelper.GetObjectValue(kvp.Value, "Version"));
                }
            }
        }
    }
}
