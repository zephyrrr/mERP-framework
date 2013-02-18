using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Proxy;
using NHibernate.Collection;

namespace Feng.NH
{
    /// <summary>
    /// NHibernate的帮助类
    /// </summary>
    public static class NHibernateHelper
    {
        #region "Metadata"
        private static ISessionFactoryManager m_defaultSfm;
        public static ISessionFactoryManager GetSessionFactoryManager()
        {
            var sfm = ServiceProvider.GetService<ISessionFactoryManager>();
            if (sfm != null)
                return sfm;
            m_defaultSfm = new Feng.NH.SessionFactoryManager();
            return m_defaultSfm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistType"></param>
        /// <returns></returns>
        public static ISessionFactory GetSessionFactory(Type persistType)
        {
            var sfm = GetSessionFactoryManager();
            foreach (var i in sfm.SessionFactories.Values)
            {
                if (i.GetClassMetadata(persistType) != null)
                    return i;
            }
            return null;
        }

        /// <summary>
        /// 获得Property的长度
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <param name="type"></param>
        /// <param name="fullPropertyName"></param>
        /// <returns></returns>
        public static int? GetPropertyLength(ISessionFactory sessionFactory, Type type, string fullPropertyName)
        {
             if (string.IsNullOrEmpty(fullPropertyName))
                return null;
            string dictKey = type.ToString() + "#" + fullPropertyName;

            int? ret = null;
            if (!s_propertyLengths.ContainsKey(dictKey))
            {
                Configuration conf = GetSessionFactoryManager().GetConfigurationBySessionFactory(sessionFactory);
                if (conf != null)
                {
                    try
                    {
                        int idx = fullPropertyName.LastIndexOf(':');
                        if (idx == -1)
                        {
                            var persistentClass = conf.GetClassMapping(type);
                            if (persistentClass != null)
                            {
                                NHibernate.Mapping.Property property = persistentClass.GetProperty(fullPropertyName);
                                if (property != null)
                                {
                                    var it = property.ColumnIterator.GetEnumerator();
                                    it.MoveNext();
                                    int length = ((NHibernate.Mapping.Column)it.Current).Length;
                                    ret = length;
                                }
                            }
                        }
                        else
                        {
                            string left = fullPropertyName.Substring(0, idx);
                            bool hasCollection;
                            Type leftType = GetPropertyType(sessionFactory, type, left, out hasCollection);
                            ret = GetPropertyLength(sessionFactory, leftType, fullPropertyName.Substring(idx + 1));
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                s_propertyLengths[dictKey] = ret;
            }

            return s_propertyLengths[dictKey];
        }

        /// <summary>
        /// GetId
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object GetId(ISessionFactory sessionFactory, object entity)
        {
            if (entity == null)
            {
                return null;
            }
            Type entityType = entity.GetType();

            NHibernate.Metadata.IClassMetadata entityMeta = sessionFactory.GetClassMetadata(entityType);
            if (entityMeta != null)
            {
                return entityMeta.GetIdentifier(entity, EntityMode.Map);
            }
            else
            {
                return null;
            }
        }

        public static char? TryRemoveJoinTypeChar(ref string s)
        {
            if (!string.IsNullOrEmpty(s) && s.Length > 1 && (s[0] == 'L' || s[0] == 'R' || s[0] == 'F'))
            {
                // Todo: maybe wrong, "ReadOnly"
                if (char.GetUnicodeCategory(s[1]) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    char ret = s[0];
                    s = s.Substring(1);
                    return ret;
                }
            }
            return null;
        }

        private static Dictionary<string, int?> s_propertyLengths = new Dictionary<string, int?>();
        private static Dictionary<string, Type> s_propertyTypes = new Dictionary<string, Type>();
        private static Dictionary<string, bool> s_propertyTypesHasCollection = new Dictionary<string, bool>();
        /// <summary>
        /// 根据Metadata得到属性类型(Path以：或者.分割，如果是Collection，则返回Collection的内部Type）
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <param name="type"></param>
        /// <param name="fullPropertyName"></param>
        /// <param name="hasCollection"></param>
        /// <returns></returns>
        public static Type GetPropertyType(ISessionFactory sessionFactory, Type type, string fullPropertyName, out bool hasCollection)
        {
            hasCollection = false;
            if (string.IsNullOrEmpty(fullPropertyName))
                return null;
            string dictKey = type.ToString() + "#" + fullPropertyName;

            lock (s_propertyTypesHasCollection)
            {
                if (!s_propertyTypes.ContainsKey(dictKey))
                {
                    NHibernate.Metadata.IClassMetadata classMetadata = sessionFactory.GetClassMetadata(type);
                    if (classMetadata == null)
                    {
                        throw new NotSupportedException("There is no Metadata of type " + type.ToString());
                    }
                    NHibernate.Type.IType destType = null;
                    Type innerType = null;

                    NHibernate.Type.ComponentType componentType = null;
                    string[] ss = fullPropertyName.Split(new char[] { ':', '.' });
                    for (int i = 0; i < ss.Length; ++i)
                    {
                        TryRemoveJoinTypeChar(ref ss[i]);
                        if (componentType == null)
                        {
                            destType = classMetadata.GetPropertyType(ss[i]);
                        }
                        else
                        {
                            for (int j = 0; j < componentType.PropertyNames.Length; ++j)
                            {
                                if (componentType.PropertyNames[j] == ss[i])
                                {
                                    destType = componentType.Subtypes[j];
                                    break;
                                }
                            }
                        }
                        componentType = destType as NHibernate.Type.ComponentType;

                        if (componentType == null)
                        {
                            if (destType.IsCollectionType)
                            {
                                //System.Collections.Generic.IList`1[[Hd.Model.Jk2.进口其他业务箱, Hd.Model.Jk2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]
                                innerType = Feng.Utils.ReflectionHelper.GetGenericUnderlyingType(destType.ReturnedClass);
                                classMetadata = sessionFactory.GetClassMetadata(innerType);

                                hasCollection = true;
                            }
                            else
                            {
                                classMetadata = sessionFactory.GetClassMetadata(destType.ReturnedClass);
                            }
                        }
                    }

                    if (!destType.IsCollectionType)
                    {
                        s_propertyTypes[dictKey] = destType.ReturnedClass;
                    }
                    else
                    {
                        s_propertyTypes[dictKey] = innerType;
                    }

                    s_propertyTypesHasCollection[dictKey] = hasCollection;
                }

                hasCollection = s_propertyTypesHasCollection[dictKey];
                return s_propertyTypes[dictKey];
            }
        }

        private static Dictionary<Type, Dictionary<Type, string>> s_collectionPropertyNames = new Dictionary<Type, Dictionary<Type, string>>();

        /// <summary>
        /// 获得MasterDetail下的Detail Property Name
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <param name="masterType"></param>
        /// <param name="detailType"></param>
        /// <returns></returns>
        public static string GetOnetoManyPropertyName(ISessionFactory sessionFactory, Type masterType, Type detailType)
        {
            if (!s_collectionPropertyNames.ContainsKey(detailType))
            {
                s_collectionPropertyNames[detailType] = new Dictionary<Type, string>();
            }
            if (!s_collectionPropertyNames[detailType].ContainsKey(masterType))
            {
                NHibernate.Metadata.IClassMetadata metadata = sessionFactory.GetClassMetadata(detailType.FullName);

                for (int i = 0; i < metadata.PropertyTypes.Length; ++i)
                {
                    NHibernate.Type.ManyToOneType manyToOneType = metadata.PropertyTypes[i] as NHibernate.Type.ManyToOneType;
                    if (manyToOneType != null && manyToOneType.ReturnedClass == masterType)
                    {
                        s_collectionPropertyNames[detailType][masterType] = metadata.PropertyNames[i];
                        break;
                    }

                    NHibernate.Type.OneToOneType oneToOneType = metadata.PropertyTypes[i] as NHibernate.Type.OneToOneType;
                    if (oneToOneType != null && oneToOneType.ReturnedClass == masterType)
                    {
                        s_collectionPropertyNames[detailType][masterType] = metadata.PropertyNames[i];
                        break;
                    }
                }
            }

            return s_collectionPropertyNames[detailType][masterType];
        }

        #endregion

        #region "Proxy"
        /// <summary>
        /// IsProxy
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static bool IsProxy(object proxy)
        {
            return (proxy is INHibernateProxy || proxy is IPersistentCollection);
        }


        /// <summary>
        /// Initialize Collection or Proxy
        /// 从外部Initialize，默认LockMode=Write。如果为None，不会写数据库。Lock后要先Evict
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="owner"></param>
        /// <param name="session"></param>
        internal static void Initialize(object proxy, object owner, NHibernate.ISession session)
        {
            if (null == proxy)
                return;

            if (!NHibernateUtil.IsInitialized(proxy))
            {
                if (proxy is INHibernateProxy)
                {
                    session.Lock(proxy, NHibernate.LockMode.None);
                    NHibernateUtil.Initialize(proxy);
                    session.Evict(proxy);
                }
                else if (proxy is IPersistentCollection)
                {
                    bool b = session.Contains(owner);
                    if (!b)
                    {
                        session.Lock(owner, NHibernate.LockMode.None);
                    }
                    NHibernateUtil.Initialize(proxy);
                    if (!b)
                    {
                        session.Evict(owner);
                    }
                }

                //if (proxy is INHibernateProxy || proxy is IPersistentCollection)
                //{
                //    if (proxy is INHibernateProxy) session.Lock(proxy, NHibernate.LockMode.None);
                //    else if (!session.Contains(owner)) session.Lock(owner, NHibernate.LockMode.None);

                //    NHibernateUtil.Initialize(proxy);
                //}
            }
        }

        ///// <summary>
        ///// Initialize Collection or Proxy
        ///// </summary>
        ///// <param name="proxy"></param>
        ///// <param name="owner"></param>
        //public static void Initialize(object proxy, object owner)
        //{
        //    if (null == proxy)
        //        return;

        //    if (!NHibernateUtil.IsInitialized(proxy))
        //    {
        //        using (var rep = RepositoryFactory.GenerateRepository(owner.GetType()))
        //        {
        //            Initialize(proxy, owner, rep.Session);
        //        }
        //    }
        //}
        #endregion

        #region "Schema"
        /// <summary>
        /// 根据实体类信息，输出数据库框架
        /// </summary>
        public static string ExportSchema(string outputFileName)
        {
            Configuration config = GetSessionFactoryManager().GetDefaultConfiguration();

            ExportSchema(config, outputFileName, true, false, false);

            return outputFileName;
        }

        /// <summary>
        /// 根据实体类信息，输出数据库框架
        /// </summary>
        public static string ExportSchema()
        {
            return ExportSchema("SchemaExport.sql");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="outputFileName"></param>
        /// <param name="script"></param>
        /// <param name="export"></param>
        /// <param name="justDrop"></param>
        public static void ExportSchema(Configuration cfg, string outputFileName, bool script, bool export, bool justDrop)
        {
            SchemaExport exporter = new SchemaExport(cfg);
            //exporter.SetOutputFile(outputFileName);
            exporter.SetDelimiter(";");

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFileName, false, Encoding.UTF8))
            {
                Console.SetOut(sw);
                exporter.Execute(script, export, justDrop);
            }

            //exporter.Drop(true, false);  ==  == exporter.Execute(true, false, true, true)
            //exporter.Create(true, false); == exporter.Execute(true, false, false, true)
        }

        /// <summary>
        /// 根据实体类信息，更新数据库框架
        /// </summary>
        /// <param name="outputFileName"></param>
        public static string UpdateSchema(string outputFileName)
        {
            Configuration config = GetSessionFactoryManager().GetDefaultConfiguration();

            SchemaUpdate updater = new SchemaUpdate(config);
            //updater.(fileName);

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(outputFileName, false, Encoding.UTF8))
            {
                Console.SetOut(sw);
                updater.Execute(true, false);
            }
            return outputFileName;
        }

        /// <summary>
        /// 根据实体类信息，更新数据库框架
        /// </summary>
        public static string UpdateSchema()
        {
            return UpdateSchema("SchemaExportUpdate.sql");
        }

        #endregion
    }
}