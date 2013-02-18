using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityBuffer : IEnumerable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        object Get(object Id);

        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// 
        /// </summary>
        string EntityName
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Type PersistentClass
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityBuffer: IEntityBuffer
    {
        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            if (m_buffers == null)
            {
                Load();
            }
            return m_buffers.Values.GetEnumerator();
        }

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="persistentClass"></param>
        public EntityBuffer(string entityName, Type persistentClass)
            : this(persistentClass)
        {
            m_entityName = entityName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="persistentClass"></param>
        public EntityBuffer(Type persistentClass)
        {
            m_persistentClass = persistentClass;
        }
        private Type m_persistentClass;

        /// <summary>
        /// 
        /// </summary>
        public Type PersistentClass
        {
            get { return m_persistentClass; }
        }

        private string m_entityName;
        /// <summary>
        /// 名称
        /// </summary>
        public string EntityName
        {
            get
            {
                if (!string.IsNullOrEmpty(m_entityName))
                {
                    return m_entityName;
                }
                else
                {
                    return PersistentClass.Name;
                }
            }
        }

        private System.Collections.IDictionary m_buffers;

        /// <summary>
        /// 根据Id得到实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public object Get(object Id)
        {
            if (Id == null)
                throw new ArgumentNullException("Id should not be null!");

            if (m_buffers == null)
            {
                Load();
            }

            if (m_buffers.Contains(Id))
            {
                return m_buffers[Id];
            }
            else
            {
                //throw new ArgumentException("There is no " + this.PersistentClass + " with id = " + Id);
                return null;
            }
        }

        private void Load()
        {
            m_buffers = new System.Collections.Generic.Dictionary<object, object>();

            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(m_persistentClass))
            {
                rep.BeginTransaction();
                IList list = rep.List(m_persistentClass);
                rep.CommitTransaction();
                
                foreach (object i in list)
                {
                    object id = EntityScript.GetPropertyValue(i, ServiceProvider.GetService<IEntityMetadataGenerator>().GenerateEntityMetadata(m_persistentClass).IdName);
                    m_buffers[id] = i;
                }
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            if (m_buffers != null)
            {
                m_buffers.Clear();
                m_buffers = null;
            }
        }
    }

    /// <summary>
    /// 实体缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public sealed class EntityBuffer<T, S> : IEntityBuffer, IEnumerable<T>
        where T: class, IEntity, IIdEntity<S>, new()
    {
        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (m_buffers == null)
            {
                Load();
            }
            return m_buffers.Values.GetEnumerator();
        }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityName"></param>
        public EntityBuffer(string entityName)
        {
            m_entityName = entityName;
        }

        /// <summary>
        /// 
        /// </summary>
        public Type PersistentClass
        {
            get { return typeof(T); }
        }

        private string m_entityName;
        /// <summary>
        /// 名称
        /// </summary>
        public string EntityName
        {
            get
            {
                if (!string.IsNullOrEmpty(m_entityName))
                {
                    return m_entityName;
                }
                else
                {
                    return PersistentClass.Name;
                }
            }
        }

        private Dictionary<S, T> m_buffers;

        /// <summary>
        /// 根据Id得到实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T Get(S Id)
        {
            if (Id == null)
                throw new ArgumentNullException("Id should not be null!");

            if (m_buffers == null)
            {
                Load();
            }

            if (m_buffers.ContainsKey(Id))
            {
                return m_buffers[Id];
            }
            else
            {
                //throw new ArgumentException("There is no " + PersistentClass.ToString() + " with id = " + Id);
                return null;
            }
        }

        /// <summary>
        /// 根据Id得到实体类
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public object Get(object Id)
        {
            if (Id == null)
                throw new ArgumentNullException("Id should not be null!");

            S sid = (S)Id;
            

            return Get(sid);
        }

        private void Load()
        {
            m_buffers = new Dictionary<S, T>();
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(typeof(T)))
            {
                rep.BeginTransaction();
                IList<T> list = rep.List<T>();
                rep.CommitTransaction();

                foreach (T i in list)
                {
                    m_buffers[i.Identity] = i;
                }
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            if (m_buffers != null)
            {
                m_buffers.Clear();
                m_buffers = null;
            }
        }
    }
}
