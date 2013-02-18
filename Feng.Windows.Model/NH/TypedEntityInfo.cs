using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
//using NHibernate.Mapping.Attributes;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace Feng.NH
{
    /// <summary>
    /// 实体类信息
    /// </summary>
    public class TypedEntityMetadata : IEntityMetadata
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <param name="type"></param>
        private TypedEntityMetadata(ISessionFactory sessionFactory, Type type)
        {
            m_sessionFactory = sessionFactory;
            m_persistentClass = type;
            Load();
        }

        private ISessionFactory m_sessionFactory;
        private Type m_persistentClass;
        /// <summary>
        /// 实体类类型
        /// </summary>
        public Type EntityType
        {
            get { return m_persistentClass; }
        }

        private static Dictionary<Type, TypedEntityMetadata> m_entityInfos = new Dictionary<Type, TypedEntityMetadata>();
        /// <summary>
        /// 生成EntityInfo信息
        /// </summary>
        /// <param name="sessionFactory"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypedEntityMetadata GenerateEntityInfo(ISessionFactory sessionFactory, Type type)
        {
            if (!m_entityInfos.ContainsKey(type))
            {
                m_entityInfos[type] = new TypedEntityMetadata(sessionFactory, type);
            }
            return m_entityInfos[type];
        }

        private void Load()
        {
            IClassMetadata classMetadata = m_sessionFactory.GetClassMetadata(m_persistentClass);
            if (classMetadata == null)
                return;

            Configuration conf = NHibernateHelper.GetSessionFactoryManager().GetConfigurationBySessionFactory(m_sessionFactory);
            var persistentClass = conf.GetClassMapping(m_persistentClass);

            m_idProperty = persistentClass.GetProperty(classMetadata.IdentifierPropertyName);
            this.TableName = persistentClass.Table.Name;

            for (int i = 0; i < classMetadata.PropertyNames.Length; ++i)
            {
                string s = classMetadata.PropertyNames[i];
                NHibernate.Mapping.Property property = persistentClass.GetProperty(s);
                try
                {
                    var it = property.ColumnIterator.GetEnumerator();
                    it.MoveNext();
                    if (it.Current != null && it.Current is NHibernate.Mapping.Column)
                    {
                        int length = ((NHibernate.Mapping.Column)it.Current).Length;
                        bool nullability = classMetadata.PropertyNullability[i];
                        m_properties[s] = new EntityPropertyMetadata { Name = s, Length = length, NotNull = !nullability };
                    }
                }
                catch (System.InvalidOperationException)
                {
                }
            }

            //if (Attribute.IsDefined(m_persistentClass, typeof(NHibernate.Mapping.Attributes.ClassAttribute)))
            //{
            //    Attribute[] attrs = Attribute.GetCustomAttributes(m_persistentClass, typeof(NHibernate.Mapping.Attributes.ClassAttribute));
            //    if (attrs.Length > 0)
            //    {
            //        ClassAttribute attr = attrs[0] as ClassAttribute;
            //        m_tableName = attr.Table;
            //    }
            //}

            //m_propertyAttributes.Clear();
            //PropertyInfo[] pInfos = m_persistentClass.GetProperties();
            //foreach (PropertyInfo pInfo in pInfos)
            //{
            //    if (Attribute.IsDefined(pInfo, typeof(NHibernate.Mapping.Attributes.PropertyAttribute)))
            //    {
            //        Attribute[] attrs = Attribute.GetCustomAttributes(pInfo, typeof(NHibernate.Mapping.Attributes.PropertyAttribute));
            //        foreach(var i in attrs)
            //        {
            //            PropertyAttribute attr = i as PropertyAttribute;
            //            m_propertyAttributes.Add(attr.Name == null ? pInfo.Name : attr.Name, attr);
            //        }
            //    }
            //    else if (Attribute.IsDefined(pInfo, typeof(NHibernate.Mapping.Attributes.IdAttribute)))
            //    {
            //        Attribute[] attrs = Attribute.GetCustomAttributes(pInfo, typeof(NHibernate.Mapping.Attributes.IdAttribute));
            //        if (attrs.Length > 0)
            //        {
            //            m_idAttribute = attrs[0] as IdAttribute;
            //        }
            //    }
            //}
        }

        private NHibernate.Mapping.Property m_idProperty;
        /// <summary>
        /// Id 名称
        /// </summary>
        public string IdName
        {
            get { return m_idProperty == null ? null : m_idProperty.Name; }
        }
        /// <summary>
        /// Id 对应数据库ColumnName
        /// </summary>
        public string IdColumnName
        {
            get 
            {
                if (m_idProperty == null)
                    return null;

                var it = m_idProperty.ColumnIterator.GetEnumerator();
                it.MoveNext();
                return ((NHibernate.Mapping.Column)it.Current).Name;
            }
        }
        /// <summary>
        /// 主键长度
        /// </summary>
        public int IdLength
        {
            get
            {
                if (m_idProperty == null)
                    return 0;
                var it = m_idProperty.ColumnIterator.GetEnumerator();
                it.MoveNext();
                return ((NHibernate.Mapping.Column)it.Current).Length;
            }
        }

        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName
        {
            get;
            private set;
        }

        private Dictionary<string, IPropertyMetadata> m_properties = new Dictionary<string, IPropertyMetadata>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IPropertyMetadata GetPropertMetadata(string propertyName)
        {
            if (m_properties.ContainsKey(propertyName))
                return m_properties[propertyName];
            else
                return null;
        }

        //private NHibernate.Mapping.Attributes.IdAttribute m_idAttribute;
        ///// <summary>
        ///// 实体类中的Id属性的Attribute
        ///// </summary>
        //public NHibernate.Mapping.Attributes.IdAttribute IdAttribute
        //{
        //    get { return m_idAttribute; }
        //}
        ///// <summary>
        ///// Id 名称
        ///// </summary>
        //public string IdName
        //{
        //    get { return m_idAttribute.Name; }
        //}
        ///// <summary>
        ///// Id 对应数据库ColumnName
        ///// </summary>
        //public string IdColumnName
        //{
        //    get { return m_idAttribute.Column; }
        //}
        ///// <summary>
        ///// 主键长度
        ///// </summary>
        //public int IdLength
        //{
        //    get { return m_idAttribute.Length; }
        //}
        //private string m_tableName;
        ///// <summary>
        ///// 数据表名
        ///// </summary>
        //public string TableName
        //{
        //    get { return m_tableName; }
        //}


        //private Dictionary<string, NHibernate.Mapping.Attributes.PropertyAttribute> m_propertyAttributes = new Dictionary<string, NHibernate.Mapping.Attributes.PropertyAttribute>();
        ///// <summary>
        ///// 获得实体类Property的属性
        ///// </summary>
        ///// <param name="proeprtyName"></param>
        ///// <returns></returns>
        //public IPropertyMetadata GetPropertMetadata(string proeprtyName)
        //{
        //    NHibernate.Mapping.Attributes.PropertyAttribute p = GetPropertyAttribute(null, proeprtyName);
        //    if (p == null)
        //        return null;
        //    else
        //        return new EntityPropertyInfo { Name = p.Name, NotNull = p.NotNull, Length = p.Length };
        //}

        ///// <summary>
        ///// 获得实体类Property的属性
        ///// </summary>
        ///// <param name="navigator"></param>
        ///// <param name="propertyName"></param>
        ///// <returns></returns>
        //public NHibernate.Mapping.Attributes.PropertyAttribute GetPropertyAttribute(string navigator, string propertyName)
        //{
        //    // Todo: 读取带有Navigator的Attribute
        //    if (!string.IsNullOrEmpty(navigator))
        //    {
        //        return null;
        //    }

        //    if (string.IsNullOrEmpty(propertyName))
        //    {
        //        return null;
        //    }
        //    if (m_propertyAttributes.ContainsKey(propertyName))
        //    {
        //        return m_propertyAttributes[propertyName];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
