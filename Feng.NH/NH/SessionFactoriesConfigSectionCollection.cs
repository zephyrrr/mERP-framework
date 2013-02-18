using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 配置文件中多SessionFactory配置信息集合，包含<see cref="SessionFactoriesConfigSectionElement"/>信息
    /// </summary>
    [ConfigurationCollection(typeof(SessionFactoriesConfigSectionElement))]
    public sealed class SessionFactoriesConfigSectionCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SessionFactoriesConfigSectionCollection()
        {
            SessionFactoriesConfigSectionElement sessionFactory = (SessionFactoriesConfigSectionElement)CreateNewElement();
            Add(sessionFactory);
        }

        /// <summary>
        /// CollectionType
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new SessionFactoriesConfigSectionElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SessionFactoriesConfigSectionElement)element).Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SessionFactoriesConfigSectionElement this[int index]
        {
            get
            {
                return (SessionFactoriesConfigSectionElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        new public SessionFactoriesConfigSectionElement this[string name]
        {
            get
            {
                return (SessionFactoriesConfigSectionElement)BaseGet(name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <returns></returns>
        public int IndexOf(SessionFactoriesConfigSectionElement sessionFactory)
        {
            return BaseIndexOf(sessionFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        public void Add(SessionFactoriesConfigSectionElement sessionFactory)
        {
            BaseAdd(sessionFactory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactory"></param>
        public void Remove(SessionFactoriesConfigSectionElement sessionFactory)
        {
            if (BaseIndexOf(sessionFactory) >= 0)
            {
                BaseRemove(sessionFactory.Name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            BaseRemove(name);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }
    }
}
