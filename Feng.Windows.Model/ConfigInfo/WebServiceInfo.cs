using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 自动创建的Web服务的种类
    /// </summary>
    public enum WebServiceType
    {
        /// <summary>
        /// 通过WinTab创建查找服务，传输的是显示数据，而非实际数据
        /// </summary>
        DataSearchView = 1,
        /// <summary>
        /// 无类型查找服务
        /// </summary>
        DataSearchViewAll = 2,
        /// <summary>
        /// 实际类型
        /// </summary>
        DataSearch = 3,
        /// <summary>
        /// 
        /// </summary>
        DataSearchAll = 4,
        /// <summary>
        /// 查找ADInfo信息
        /// </summary>
        ADInfoSearch = 5,
        /// <summary>
        /// NameValueMapping查找
        /// </summary>
        NameValueMapping = 6,
        /// <summary>
        /// 认证
        /// </summary>
        AuthenticationService = 11,
        /// <summary>
        /// 
        /// </summary>
        RoleService = 12,
        /// <summary>
        /// 
        /// </summary>
        ProfileService = 13

    }

    /// <summary>
    /// 自动创建的Web服务的配置信息
    /// </summary>
    [Class(0, Name = "Feng.WebServiceInfo", Table = "AD_Web_Service", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class WebServiceInfo : BaseADEntity
    {
        /// <summary>
        /// Rest服务（XML，json）地址（相对地址）
        /// 可为空，为空时不创建
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string RestAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Soap地址（相对地址）
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string SoapAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 参数。
        /// WebServiceType = SearchWinTab时，为WinTab名称
        /// </summary>
        [Property(Length = 50, NotNull = false)]
        public virtual string ExecuteParam
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual WebServiceType Type
        {
            get;
            set;
        }
    }
}
