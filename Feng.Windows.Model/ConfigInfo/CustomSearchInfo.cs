using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 自定义查找信息，显示在程序查找页的自定义查找中
    /// </summary>
    [Class(0, Name = "Feng.CustomSearchInfo", Table = "AD_Search_Custom", OptimisticLock = OptimisticLockMode.Version)]
    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
    [Serializable]
    public class CustomSearchInfo : BaseADEntity
    {
        /// <summary>
        /// 分组名称，读入和<see cref="T:Feng.ISearchManager.Name"/>一致的信息
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public virtual string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Text
        {
            get;
            set;
        }

        /// <summary>
        /// 搜索条件，格式见<see cref="Feng.SearchExpression"/>
        /// </summary>
        [Property(Length = 255, NotNull = true)]
        public virtual string SearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Property(NotNull = true)]
        public virtual int SeqNo
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示，格式见<see cref="T:Feng.Authority"/>
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public virtual string Visible
        {
            get;
            set;
        }
    }
}