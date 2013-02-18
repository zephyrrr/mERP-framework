using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Feng.Run
{
    public class AppConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("name", DefaultValue = "AppConfigSection", IsRequired = true, IsKey = false)]
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


        // Declare a simple element of the type
        // UrlConfigElement. In the configuration
        // file it corresponds to <config .... />.
        [ConfigurationProperty("product")]
        public AppConfigElement Product
        {
            get
            {
                AppConfigElement url = (AppConfigElement)base["product"];
                return url;
            }
        }

    }
}
