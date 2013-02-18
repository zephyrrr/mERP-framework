//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Xml;

//namespace Feng.NH
//{
//    /// <summary>
//    /// Mapping信息配置区
//    ///  <section name="repositoryMappings" type="Feng.NH.NHibernateRepositoryMappingSection, Feng.Base" />
//    ///  <repositoryMappings>
//    ///   <assemblies>
//    ///   <clearAssemblies/>
//    ///   <assembly name="Feng.Application" type="attribute" />
//    ///   <assembly name="Feng.Model" type="attribute"  factoryConfigPath="example.config"/>
//    ///   </assemblies>
//    ///  </repositoryMappings>
//    /// </summary>
//    public class NHibernateRepositoryMappingSection : ConfigurationSection
//    {
//        /// <summary>
//        /// Mapping信息集合
//        /// </summary>
//        [ConfigurationProperty("assemblies", IsDefaultCollection = false)]
//        [ConfigurationCollection(typeof(SessionFactoriesConfigSectionCollection), AddItemName = "assembly",
//            ClearItemsName = "clearAssemblies")]
//        public NHibernateRepositoryMappingCollection MappingAssemblies
//        {
//            get
//            {
//                NHibernateRepositoryMappingCollection mappingAssemblies =
//                    (NHibernateRepositoryMappingCollection)base["assemblies"];
//                return mappingAssemblies;
//            }
//        }
//    }

//    /// <summary>
//    /// 配置文件中的SessionFactory中的Mapping文件配置
//    /// </summary>
//    public class NHibernateRepositoryMappingElement : ConfigurationElement
//    {
//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public NHibernateRepositoryMappingElement() { }

//        /// <summary>
//        /// Consturctor
//        /// </summary>
//        /// <param name="name"></param>
//        /// <param name="type"></param>
//        public NHibernateRepositoryMappingElement(string name, string type)
//        {
//            Name = name;
//            Type = type;
//        }

//        /// <summary>
//        /// 根据不同类型定义的Mapping信息文件名
//        /// </summary>
//        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "Not Supplied")]
//        public string Name
//        {
//            get { return (string)this["name"]; }
//            set { this["name"] = value; }
//        }

//        /// <summary>
//        /// 类型，有Assembly，Resource，Attribute，hbmFile 4种
//        /// </summary>
//        [ConfigurationProperty("type", IsRequired = true, DefaultValue = "Not Supplied")]
//        public string Type
//        {
//            get { return (string)this["type"]; }
//            set { this["type"] = value; }
//        }

//        /// <summary>
//        /// 对应的SessionFactory。如对应多个，用','分隔
//        /// 程序可以有多个SessionFactory，每个SessionFactory读入不同的Mapping信息
//        /// </summary>
//        [ConfigurationProperty("factoryConfigPath", IsRequired = false, DefaultValue = "")]
//        public string FactoryConfigPath
//        {
//            get { return (string)this["factoryConfigPath"]; }
//            set { this["factoryConfigPath"] = value; }
//        }
//    }

//    /// <summary>
//    /// <see cref="NHibernateRepositoryMappingElement"/>信息集合
//    /// </summary>
//    [ConfigurationCollection(typeof(NHibernateRepositoryMappingElement))]
//    public sealed class NHibernateRepositoryMappingCollection : ConfigurationElementCollection
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public NHibernateRepositoryMappingCollection()
//        {
//            NHibernateRepositoryMappingElement sessionFactory = (NHibernateRepositoryMappingElement)CreateNewElement();
//            Add(sessionFactory);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public override ConfigurationElementCollectionType CollectionType
//        {
//            get
//            {
//                return ConfigurationElementCollectionType.AddRemoveClearMap;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        protected override ConfigurationElement CreateNewElement()
//        {
//            return new NHibernateRepositoryMappingElement();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="element"></param>
//        /// <returns></returns>
//        protected override object GetElementKey(ConfigurationElement element)
//        {
//            return ((NHibernateRepositoryMappingElement)element).Name;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="index"></param>
//        /// <returns></returns>
//        public NHibernateRepositoryMappingElement this[int index]
//        {
//            get
//            {
//                return (NHibernateRepositoryMappingElement)BaseGet(index);
//            }
//            set
//            {
//                if (BaseGet(index) != null)
//                {
//                    BaseRemoveAt(index);
//                }

//                BaseAdd(index, value);
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        new public NHibernateRepositoryMappingElement this[string name]
//        {
//            get
//            {
//                return (NHibernateRepositoryMappingElement)BaseGet(name);
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="sessionFactory"></param>
//        /// <returns></returns>
//        public int IndexOf(NHibernateRepositoryMappingElement sessionFactory)
//        {
//            return BaseIndexOf(sessionFactory);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="sessionFactory"></param>
//        public void Add(NHibernateRepositoryMappingElement sessionFactory)
//        {
//            BaseAdd(sessionFactory);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="element"></param>
//        protected override void BaseAdd(ConfigurationElement element)
//        {
//            BaseAdd(element, false);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="sessionFactory"></param>
//        public void Remove(NHibernateRepositoryMappingElement sessionFactory)
//        {
//            if (BaseIndexOf(sessionFactory) >= 0)
//            {
//                BaseRemove(sessionFactory.Name);
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="index"></param>
//        public void RemoveAt(int index)
//        {
//            BaseRemoveAt(index);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="name"></param>
//        public void Remove(string name)
//        {
//            BaseRemove(name);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void Clear()
//        {
//            BaseClear();
//        }
//    }

    

//    ///// <summary>
//    ///// This is the configuration section handler for NHibernateRepository.
//    ///// </summary>
//    //public class NHibernateConfigSectionHandler : IConfigurationSectionHandler
//    //{
//    //    #region IConfigurationSectionHandler Members
//    //    /// <summary>
//    //    /// Create
//    //    /// </summary>
//    //    /// <param name="parent"></param>
//    //    /// <param name="configContext"></param>
//    //    /// <param name="section"></param>
//    //    /// <returns></returns>
//    //    public object Create(object parent, object configContext, XmlNode section)
//    //    {
//    //        Dictionary<string, string> assemblies = GetAssembliesConfiguration(section);

//    //        NHibernateConfig config = new NHibernateConfig(assemblies);

//    //        return config;
//    //    }

//    //    #endregion

//    //    private static Dictionary<string, string> GetAssembliesConfiguration(XmlNode section)
//    //    {
//    //        XmlNodeList assemblies = section.SelectNodes("assemblies/assembly");
//    //        if (assemblies == null)
//    //        {
//    //            throw new ConfigurationErrorsException(
//    //                "The repository config section must contain assemblies/assembly node.");
//    //        }

//    //        Dictionary<string, string> dict = new Dictionary<string, string>();

//    //        for (int i = 0; i < assemblies.Count; i++)
//    //        {
//    //            XmlAttribute assemblyName = assemblies[i].Attributes["name"];
//    //            XmlAttribute assemblyType = assemblies[i].Attributes["type"];

//    //            if (string.IsNullOrEmpty(assemblyName.Value)
//    //                || string.IsNullOrEmpty(assemblyType.Value))
//    //            {
//    //                throw new ConfigurationErrorsException("The assembly name and type attribute cannot be empty.");
//    //            }

//    //            dict[assemblyName.Value] = assemblyType.Value;
//    //        }

//    //        return dict;
//    //    }
//    //}

//    ///// <summary>
//    ///// Used to hold configuration information for the NHibernateRepository.
//    ///// </summary>
//    //public class NHibernateConfig
//    //{
//    //    /// <summary>
//    //    /// Consturctor
//    //    /// </summary>
//    //    public NHibernateConfig(Dictionary<string, string> dict)
//    //    {
//    //        _assemblies = new Dictionary<string, string>();
//    //        foreach (KeyValuePair<string, string> kvp in dict)
//    //        {
//    //            _assemblies.Add(kvp.Key, kvp.Value);
//    //        }
//    //    }

//    //    private Dictionary<string, string> _assemblies;

//    //    /// <summary>
//    //    /// Gets a collection of assemblies specified in the NHibernateRepository configuration.
//    //    /// </summary>
//    //    public Dictionary<string, string> Assemblies
//    //    {
//    //        get { return _assemblies; }
//    //    }
//    //}
//}