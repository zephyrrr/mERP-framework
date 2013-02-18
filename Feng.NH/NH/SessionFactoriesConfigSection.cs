using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// Encapsulates a section of Web/App.config to declare which session factories are to be created.
    /// Huge kudos go out to http://msdn2.microsoft.com/en-us/library/system.configuration.configurationcollectionattribute.aspx
    /// for this technique - it was by far the best overview of the subject.
    /// </summary>
    public class SessionFactoriesConfigSection : ConfigurationSection
    {
        /// <summary>
        /// SessionFactory集合
        /// </summary>
        [ConfigurationProperty("sessionFactories", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SessionFactoriesConfigSectionCollection), AddItemName = "sessionFactory",
            ClearItemsName = "clearFactories")]
        public SessionFactoriesConfigSectionCollection SessionFactories
        {
            get
            {
                SessionFactoriesConfigSectionCollection sessionFactoriesCollection =
                    (SessionFactoriesConfigSectionCollection)base["sessionFactories"];
                return sessionFactoriesCollection;
            }
        }
    }
}
