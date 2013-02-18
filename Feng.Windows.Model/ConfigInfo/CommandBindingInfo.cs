using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 命令绑定信息
    /// </summary>
    [Class(0, Name = "Feng.CommandBindingInfo", Table = "AD_Command_Binding", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class CommandBindingInfo : BaseADEntity
    {
        /// <summary>
        /// 要执行的Process
        /// </summary>
        [ManyToOne(ForeignKey = "FK_CommandBinding_Process", NotNull = true)]
        public virtual ProcessInfo Command
        {
            get;
            set;
        }
    }
}
