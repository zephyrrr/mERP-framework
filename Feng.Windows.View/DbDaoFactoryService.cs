//using System;
//using System.Collections.Generic;
//using System.Text;
//using Feng.Windows.Utils;

//namespace Feng
//{
//    /// <summary>
//    /// DaoFactory Service according DbSetup
//    /// </summary>
//    public class DbDaoFactoryService : IDaoFactoryService
//    {
//        private Dictionary<string, string> m_daoTypes = new Dictionary<string, string>();
//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public DbDaoFactoryService()
//        {
//            IList<DaoFactoryInfo> list = ADInfoBll.Instance.GetDaoFactoryInfo();
//            if (list != null)
//            {
//                foreach (DaoFactoryInfo i in list)
//                {
//                    m_daoTypes[i.EntityType] = i.DaoType;
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="entityType"></param>
//        /// <returns></returns>
//        public IBaseDao CreateDao(Type entityType)
//        {
//            if (entityType == null)
//            {
//                throw new ArgumentNullException("entityType");
//            }
//            if (!typeof(IEntity).IsAssignableFrom(entityType))
//            {
//                throw new ArgumentNullException("entityType should be IEntity!");
//            }

//            if (m_daoTypes.ContainsKey(entityType.FullName))
//            {
//                IBaseDao dao = Feng.Utils.ReflectionHelper.CreateInstanceFromName(m_daoTypes[entityType.FullName]) as IBaseDao;
//                if (dao == null)
//                {
//                    throw new ArgumentNullException("DaoType should be IDao!");
//                }
//                return dao;
//            }
//            else
//            {
//                if (typeof(IMultiOrgEntity).IsAssignableFrom(entityType))
//                {
//                    return Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.MultiOrgEntityDao`1[[" + entityType + "]], Feng.Controller") as IBaseDao;
//                }
//                else if (typeof(ILogEntity).IsAssignableFrom(entityType))
//                {
//                    return Feng.Utils.ReflectionHelper.CreateInstanceFromName("Feng.LogEntityDao`1[[" + entityType + "]], Feng.Controller") as IBaseDao;
//                }
//                else
//                {
//                    throw new ArgumentException("Not supported entityType of " + entityType.FullName);
//                }
//            }
//        }
//    }
//}
