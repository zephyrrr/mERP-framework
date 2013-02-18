using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// PythonSource，py（=1）
        /// </summary>
        PythonSource = 1,
        /// <summary>
        /// Report，rpt（=2）
        /// </summary>
        Report = 2,
        /// <summary>
        /// Dataset，cs（=3）
        /// </summary>
        Dataset = 3,
        /// <summary>
        /// File（=4）
        /// </summary>
        File = 4,
        /// <summary>
        /// MsReport，rdlc（=5）
        /// </summary>
        MsReport = 5,
        /// <summary>
        /// Config, .config（=6）
        /// </summary>
        Config = 6
    }

    

    /// <summary>
    /// 资源信息
    /// </summary>
    [Class(0, Name = "Feng.ResourceInfo", Table = "AD_Resource", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class ResourceInfo : BaseADEntity
    {
        /// <summary>
        /// 资源类型
        /// </summary>
        [Property(NotNull = true)]
        public virtual ResourceType ResourceType
        {
            get;
            set;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        [Property(Length = 255, NotNull = true, Unique = true)]
        public virtual string ResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 参数
        /// </summary>
        [Property(Length = int.MaxValue, NotNull = true)]
        public virtual string Content
        {
            get;
            set;
        }

        ///// <summary>
        ///// 参数类型(System.String or Sytem.Byte[])
        ///// </summary>
        //[Property(Length = 255, NotNull = true)]
        //public virtual string ContentType
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Md5校验值
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string Md5
        {
            get;
            set;
        }

        /// <summary>
        /// 是否保存到本地
        /// </summary>
        [Property(NotNull = true)]
        public virtual bool PersistLocal
        {
            get;
            set;
        }
    }
}
