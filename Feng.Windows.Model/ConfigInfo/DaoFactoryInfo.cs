//using System;
//using System.Collections.Generic;
//using System.Text;
//using NHibernate.Mapping.Attributes;

//namespace Feng
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [Class(0, Name = "Feng.DaoFactoryInfo", Table = "AD_DaoFactory", OptimisticLock = OptimisticLockMode.Version)]
//    [Cache(1, Usage = CacheUsage.NonStrictReadWrite)]
//    [Serializable]
//    public class DaoFactoryInfo : BaseADEntity
//    {
//        /// <summary>
//        /// Entity's Type
//        /// </summary>
//        [Property(Length = 255, NotNull = true)]
//        public virtual string EntityType
//        {
//            get;
//            set;
//        }

//        /// <summary>
//        /// Entity Dao's Type
//        /// </summary>
//        [Property(Length = 255, NotNull = true)]
//        public virtual string DaoType
//        {
//            get;
//            set;
//        }
//    }
//}
