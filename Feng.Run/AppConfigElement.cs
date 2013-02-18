using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Feng.Run
{
    public class AppConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "Feng.Run", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("serverPath")]
        public string ServerPath
        {
            get
            {
                return (string)this["serverPath"];
            }
            set
            {
                this["serverPath"] = value;
            }
        }

        [ConfigurationProperty("version", IsRequired = false)]
        public string versionString
        {
            get
            {
                return (string)this["version"];
            }
            set
            {
                this["version"] = value;
            }
        }

        public Version CurrentAppVersion
        {
            get
            {
                return new Version(versionString);
            }
            set
            {
                versionString = value.ToString();
            }
        }

        [ConfigurationProperty("product")]
        public string ProductName
        {
            get
            {
                return (string)this["product"];
            }
            set
            {
                this["product"] = value;
            }
        }

        [ConfigurationProperty("company")]
        public string CompanyName
        {
            get
            {
                return (string)this["company"];
            }
            set
            {
                this["company"] = value;
            }
        }

        [ConfigurationProperty("resource")]
        public string ResoucrceAssembly
        {
            get
            {
                return (string)this["resource"];
            }
            set
            {
                this["resource"] = value;
            }
        }

        [ConfigurationProperty("litemode", DefaultValue = true)]
        public bool LiteMode
        {
            get
            {
                return (bool)this["litemode"];
            }
        }

        //[ConfigurationProperty("applicationmode")]
        //public string ApplicationMode
        //{
        //    get
        //    {
        //        return (string)this["applicationmode"];
        //    }
        //}
        [ConfigurationProperty("debugmode", DefaultValue = false)]
        public bool DebugMode
        {
            get
            {
                return (bool)this["debugmode"];
            }
        }
    }
}
