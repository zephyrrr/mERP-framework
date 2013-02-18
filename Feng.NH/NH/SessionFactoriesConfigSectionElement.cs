using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 配置文件中的SessionFactory信息
    /// 程序支持多SessionFactory，默认是根据实体类类型（命名空间）来创建不同SessionFactory。如果找不到相应的SessionFactory，用默认SessionFactory
    /// </summary>
    public class SessionFactoriesConfigSectionElement : ConfigurationElement
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SessionFactoriesConfigSectionElement() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configPath"></param>
        public SessionFactoriesConfigSectionElement(string name, string configPath) {
            Name = name;
            FactoryConfigPath = configPath;
        }

        /// <summary>
        /// SessionFactory名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        // <summary>
        /// 类型，有Assembly，Resource，Attribute，hbmFile 4种
        /// </summary>
        [ConfigurationProperty("type", IsRequired = false, DefaultValue = "")]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// SessionFactory的配置文件路径
        /// </summary>
        [ConfigurationProperty("factoryConfigPath", IsRequired = true, DefaultValue = "")]
        public string FactoryConfigPath {
            get { return (string)this["factoryConfigPath"]; }
            set { this["factoryConfigPath"] = value; }
        }

        /// <summary>
        /// 是否是默认SessionFactory
        /// </summary>
        [ConfigurationProperty("isDefault", IsRequired = false, DefaultValue = "false")]
        public bool IsDefault
        {
            get { return (bool)this["isDefault"]; }
            set { this["isDefault"] = value; }
        }
    }
}
