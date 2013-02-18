using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityBufferCollection : Singleton<EntityBufferCollection>
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityBufferCollection()
        {
        }

        private Dictionary<string, IEntityBuffer> m_buffers = new Dictionary<string, IEntityBuffer>();

        /// <summary>
        /// 加入一个EntityBuffer
        /// </summary>
        /// <param name="eb">IEntityBuffer</param>
        public void Add(IEntityBuffer eb)
        {
            if (eb == null)
            {
                throw new ArgumentNullException("eb");
            }

            if (m_buffers.ContainsKey(eb.EntityName))
            {
                return;
            }

            m_buffers.Add(eb.EntityName, eb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public IEntityBuffer this[string entityName]
        {
            get
            {
                if (!m_buffers.ContainsKey(entityName))
                {
                    //throw new ArgumentException("there is no " + name + " in EntityBufferCollection!");
                    return null;
                }
                return m_buffers[entityName];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEntityBuffer this[Type type]
        {
            get
            {
                string entityName = type.Name;
                return this[entityName];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(object id)
        {
            return (T)Get(typeof(T), id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(Type type, object id)
        {
            IEntityBuffer eb = this[type];
            if (eb == null)
            {
                throw new ArgumentException(string.Format("There is no EntityBuffer with {0}!", type));
            }
            return eb.Get(id);
        }

        private void AddTypedEntityBuffer(Type type)
        {
            EntityBuffer eb = new EntityBuffer(type);
            Add(eb);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (IEntityBuffer i in this.m_buffers.Values)
            {
                i.Clear();
            }
            m_buffers.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            foreach (IEntityBuffer i in this.m_buffers.Values)
            {
                i.Clear();
            }
        }
    }
}
