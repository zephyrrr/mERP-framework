using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 警示信息，显示在程序警示窗口中。根据<see cref="AlertRuleInfo"/>自动生成。
    /// </summary>
    [Class(0, Name = "Feng.AlertInfo", Table = "SD_Alert", OptimisticLock = OptimisticLockMode.Version)]
    [Serializable]
    public class AlertInfo : BaseDataEntity
    {
        /// <summary>
        /// 警示信息描述
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 对此警示信息可见的用户，其与<see cref="RecipientRole"/>是Or关系，即只要有其中一个符合就可见
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string RecipientUser
        {
            get;
            set;
        }

        /// <summary>
        /// 对此警示信息可见的用户组
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string RecipientRole
        {
            get;
            set;
        }

        /// <summary>
        /// 此警示所关联的动作，即点击警示信息后会产生的动作
        /// </summary>
        [ManyToOne(ForeignKey = "FK_AlertInfo_Action")]
        public virtual ActionInfo Action
        {
            get;
            set;
        }

        ///// <summary>
        ///// 此警示所关联的动作，即点击警示信息后会产生的动作，见<see cref="ActionInfo"/>
        ///// </summary>
        //[Property(Column = "ActionId", NotNull = true)]
        //public virtual long ActionId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 如果相关动作为窗体(ActionType=Window, Form, Report)时的默认查找条件表达式。
        /// 表达式格式见<see cref="Feng.SearchExpression"/>
        /// </summary>
        [Property(Length = 200, NotNull = true)]
        public virtual string SearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已处理。新生成警示为未处理，处理过（默认为点击操作）后为已处理，不再显示
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool IsFixed
        {
            get;
            set;
        }
    }
}
