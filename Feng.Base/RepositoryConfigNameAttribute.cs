using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// SessionFactory配置属性。
    /// 默认为从实体类类型信息（命名空间）得到SessionFactory，但如果有特殊需要，可手工写Attribute指明用哪个SessionFactory
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    //[Obsolete("use repositoryConfigName in WindowTabInfo")]
    internal class RepositoryConfigNameAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configName"></param>
        public RepositoryConfigNameAttribute(string configName)
        {
            if (configName == null)
            {
                throw new ArgumentNullException("configName");
            }
            this._configName = configName;
        }


        private string _configName;

        /// <summary>
        /// 配置名称，见<see cref="P:SessionFactoryElement.Name"/>
        /// </summary>
        public string ConfigName
        {
            get
            {
                return this._configName;
            }
        }
    }
}
