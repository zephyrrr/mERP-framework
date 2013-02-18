using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 用户配置信息
    /// </summary>
    [Class(0, Name = "Feng.UserConfigurationInfo", Table = "SD_User_Configuration", OptimisticLock = OptimisticLockMode.Version)]
    public class UserConfigurationInfo : BaseDataEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Property(Length = 100, NotNull = true, Unique = true)]
        public virtual string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 用户数据
        /// </summary>
        [Property(Length = int.MaxValue, NotNull = false, Lazy = true)]
        public virtual byte[] UserData
        {
            get;
            set;
        }

        /// <summary>
        /// 用户自定义起始窗体
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string StartForm
        {
            get;
            set;
        }

        /// <summary>
        /// 数据长度
        /// </summary>
        [Property(NotNull = true)]
        public virtual int UserDataLength
        {
            get;
            set;
        }
    }
}
