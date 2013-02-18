using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 插件信息
    /// </summary>
    [Class(0, Name = "Feng.PluginInfo", Table = "AD_Plugin", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class PluginInfo : BaseADEntity
    {
        /// <summary>
        /// 要执行的Process
        /// </summary>
        [ManyToOne(ForeignKey = "FK_Plugin_Process")]
        public virtual ProcessInfo Process
        {
            get;
            set;
        }

        /// <summary>
        /// 权限
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Permission
        {
            get;
            set;
        }
    }
}
