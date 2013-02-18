using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 警示信息规则定义，用于产生警示信息<see cref="AlertInfo"/>。
    /// 系统会定时或手工根据此规则产生警示信息。
    /// </summary>
    [Class(0, Name = "Feng.AlertRuleInfo", Table = "AD_AlertRule", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class AlertRuleInfo : BaseADEntity
    {
        /// <summary>
        /// Sql语句
        /// 其生成的内容必须有 <see cref="P:AlertInfo.Action.Name"/>，<see cref="AlertInfo.SearchExpression"/>，<see cref="AlertInfo.Description"/> 3个字段
        /// </summary>
        [Property(Length = 2000, NotNull = false)]
        public virtual string Sql
        {
            get;
            set;
        }

        /// <summary>
        /// 警示信息接收用户，填充<see cref="AlertInfo"/>中的<see cref="AlertInfo.RecipientUser"/>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string RecipientUser
        {
            get;
            set;
        }

        /// <summary>
        /// 警示信息接收组，填充<see cref="AlertInfo"/>中的<see cref="AlertInfo.RecipientRole"/>
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string RecipientRole
        {
            get;
            set;
        }
    }
}
