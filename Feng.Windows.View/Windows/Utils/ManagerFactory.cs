using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// SearchManager, DisplayManager, SearchManager Factory
    /// ControlManager: Typed, Untyped
    /// DisplayManager: Typed, Untyped
    /// SearchManager: Typed, Untyped, UntypedProcedure, UntypedFunction, 
    /// </summary>
    public class ManagerFactory : IManagerFactory
    {
        /// <summary>
        /// CreateSearchManagerEagerFetchs
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="gridName"></param>
        public static void CreateSearchManagerEagerFetchs(ISearchManager fm, string gridName)
        {
            Dictionary<string, bool> eagerFetchs = new Dictionary<string, bool>();
            foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(gridName))
            {
                if (info.GridColumnType == GridColumnType.Normal)
                {
                    string toEagerFetch = null;
                    if (info.CellViewerManager == "Object")
                    {
                        if (string.IsNullOrEmpty(info.Navigator))
                        {
                            toEagerFetch = info.PropertyName;
                        }
                        else
                        {
                            toEagerFetch = info.Navigator + "." + info.PropertyName;
                        }
                    }
                    else if (!string.IsNullOrEmpty(info.Navigator))
                    {
                        toEagerFetch = info.Navigator;
                    }
                    if (!string.IsNullOrEmpty(toEagerFetch))
                    {
                        string[] ss = toEagerFetch.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < ss.Length; ++i)
                        {
                            sb.Append(ss[i]);
                            eagerFetchs[sb.ToString()] = true;
                            sb.Append(".");
                        }
                    }
                }
            }
            foreach (string s in eagerFetchs.Keys)
            {
                fm.EagerFetchs.Add(s);
            }
        }

        /// <summary>
        /// 生成子Dao（继续生成subsubDao)
        /// </summary>
        /// <param name="daoParent"></param>
        /// <param name="tabInfo"></param>
        public static void GenerateBusinessLayer(IRelationalDao daoParent, WindowTabInfo tabInfo)
        {
            if (string.IsNullOrEmpty(tabInfo.BusinessLayerClassName))
            {
                throw new ArgumentException("WindowTabInfo of " + tabInfo.Name + " 's BusinessLayerClassName must not be null!");
            }

            if (!string.IsNullOrEmpty(tabInfo.BusinessLayerClassParams))
            {
                IRelationalDao subDao = Feng.Utils.ReflectionHelper.CreateInstanceFromName(tabInfo.BusinessLayerClassParams) as IRelationalDao;

                IEventDao subRelationDao = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.BusinessLayerClassName), subDao) as IEventDao;
                daoParent.AddRelationalDao(subRelationDao);

                foreach (WindowTabInfo childTab in tabInfo.ChildTabs)
                {
                    GenerateBusinessLayer(subDao, childTab);
                }
            }
            else
            {
                IEventDao subDao = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.BusinessLayerClassName)) as IEventDao;
                daoParent.AddRelationalDao(subDao);

                foreach (WindowTabInfo childTab in tabInfo.ChildTabs)
                {
                    GenerateBusinessLayer(subDao, childTab);
                }
            }
        }

        /// <summary>
        /// 生成主Dao(不生成subDao)
        /// </summary>
        /// <param name="tabInfo"></param>
        public virtual IBaseDao GenerateBusinessLayer(WindowTabInfo tabInfo)
        {
            if (string.IsNullOrEmpty(tabInfo.BusinessLayerClassName))
            {
                throw new ArgumentException("WindowTabInfo of " + tabInfo.Identity + " 's BusinessLayerClassName must not be null!");
            }
            IBaseDao daoParent;
            if (string.IsNullOrEmpty(tabInfo.BusinessLayerClassParams))
            {
                daoParent = Feng.Utils.ReflectionHelper.CreateInstanceFromName(tabInfo.BusinessLayerClassName) as IBaseDao;
            }
            else
            {
                string[] s = Feng.Utils.StringHelper.Split(tabInfo.BusinessLayerClassParams, ',');
                object[] args = new object[s.Length];
                for (int i = 0; i < s.Length; ++i)
                {
                    args[i] = s[i];
                }
                daoParent = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.BusinessLayerClassName), args) as IBaseDao;
            }
            if (daoParent is IRepositoryConsumer)
            {
                (daoParent as IRepositoryConsumer).RepositoryCfgName = tabInfo.RepositoryConfigName;
            }
            return daoParent;
        }


        private static void SetGridPermissions(WindowTabInfo tabInfo, IControlManager cm)
        {
            GridInfo info = ADInfoBll.Instance.GetGridInfo(tabInfo.GridName);

            if (!string.IsNullOrEmpty(info.AllowInsert))
            {
                cm.AllowInsert = Authority.AuthorizeByRule(info.AllowInsert);
            }
            if (!string.IsNullOrEmpty(info.AllowEdit))
            {
                cm.AllowEdit = Authority.AuthorizeByRule(info.AllowEdit);
            }
            if (!string.IsNullOrEmpty(info.AllowDelete))
            {
                cm.AllowDelete = Authority.AuthorizeByRule(info.AllowDelete);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        public virtual IControlManager GenerateControlManager(WindowTabInfo tabInfo, ISearchManager sm)
        {
            // maybe null, saved in outer space
            //if (string.IsNullOrEmpty(tabInfo.BusinessLayerClassName))
            //{
            //    throw new ArgumentException("WindowTabInfo of " + tabInfo.Name + " 's BusinessLayerClassName must not be null!");
            //}
            if (string.IsNullOrEmpty(tabInfo.ControlManagerClassName))
            {
                throw new ArgumentException("WindowTabInfo of " + tabInfo.Identity + " 's ControlManagerClassName must not be null!");
            }
            IControlManager cm = null;
            switch (tabInfo.ControlManagerClassName.ToUpper())
            {
                case "TYPED":
                    cm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.CreateGenericType(
                        Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Windows.Forms.ControlManager`1, Feng.Windows.Controller"), new Type[] { Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.ModelClassName) }),
                        sm) as IControlManager;
                    break;
                case "UNTYPED":
                    cm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Windows.Forms.ControlManager, Feng.Windows.Controller"),
                        sm) as IControlManager;
                    break;
                default:
                    cm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.ControlManagerClassName), sm) as IControlManager;
                    break;
            }
            if (cm == null)
            {
                throw new ArgumentException("WindowTabInfo of " + tabInfo.Identity + " 's ControlManagerClassName is wrong!");
            }
            //cm.Name = windowInfo == null ? tabInfo.Name : windowInfo.Name;
            cm.Name = tabInfo.Identity;
            cm.DisplayManager.Name = tabInfo.Identity;
            
            SetGridPermissions(tabInfo, cm);

            return cm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        public virtual IDisplayManager GenerateDisplayManager(WindowTabInfo tabInfo, ISearchManager sm)
        {
            IDisplayManager dm;
            if (!string.IsNullOrEmpty(tabInfo.DisplayManagerClassName))
            {
                switch (tabInfo.DisplayManagerClassName.ToUpper())
                {
                    case "TYPED":
                        dm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.CreateGenericType(
                            Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Windows.Forms.DisplayManager`1, Feng.Windows.Controller"), new Type[] { Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.ModelClassName) }),
                            sm) as IDisplayManager;
                        break;
                    case "UNTYPED":
                        dm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Windows.Forms.DisplayManager, Feng.Windows.Controller"),
                            sm) as IDisplayManager;
                        break;
                    default:
                        dm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.DisplayManagerClassName), sm) as IDisplayManager;
                        break;
                }
                if (dm == null)
                {
                    throw new ArgumentException("WindowTabInfo of " + tabInfo.Identity + " 's DisplayManagerClassName is wrong!");
                }
            }
            else if (!string.IsNullOrEmpty(tabInfo.ControlManagerClassName))
            {
                dm = GenerateControlManager(tabInfo, sm).DisplayManager;
            }
            else
            {
                throw new ArgumentException("WindowTabInfo of " + tabInfo.Identity + " 's DisplayManagerClassName or ControlManagerClassName must not be null!");
            }
            // Why?
            //dm.Name = windowInfo == null ? tabInfo.Name : windowInfo.Name;
            dm.Name = tabInfo.Identity;

            return dm;
        }

        /// <summary>
        /// GenerateSearchManager
        /// </summary>
        /// <param name="searchManagerClassName"></param>
        /// <param name="searchManagerClassParams"></param>
        /// <returns></returns>
        public virtual ISearchManager GenerateSearchManager(string searchManagerClassName, string searchManagerClassParams = null)
        {
            if (string.IsNullOrEmpty(searchManagerClassName))
            {
                throw new ArgumentNullException("searchManagerClassName");
            }

            ISearchManager sm = null;

            if (string.IsNullOrEmpty(searchManagerClassParams))
            {
                sm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(GenerateSearchManagerType(searchManagerClassName)) as ISearchManager;
            }
            else
            {
                string[] s = Feng.Utils.StringHelper.Split(searchManagerClassParams, ',');
                sm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(GenerateSearchManagerType(searchManagerClassName), s) as ISearchManager;
            }

            sm.Name = searchManagerClassName + "." + searchManagerClassParams;

            return sm;
        }

        private static Type GenerateSearchManagerType(string searchManagerClassName)
        {
            Type sm;
            switch (searchManagerClassName.ToUpper())
            {
                case "UNTYPED":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManager, Feng.Windows.Controller");
                    break;
                case "UNTYPEDPROCEDURE":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerProcedure, Feng.Windows.Controller");
                    break;
                case "UNTYPEDFUNCTION":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerFunction, Feng.Windows.Controller");
                    break;
                case "UNTYPEDGROUPBYDETAIL":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerGroupByDetail, Feng.Windows.Controller");
                    break;
                case "UNTYPEDDETAILINMASTER":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerProxyDetailInMaster, Feng.Windows.Controller");
                    break;
                default:
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName(searchManagerClassName);
                    break;
            }
            return sm;
        }

        private Type GenerateSearchManagerType(WindowTabInfo tabInfo)
        {
            Type sm;
            switch (tabInfo.SearchManagerClassName.ToUpper())
            {
                case "TYPED":
                    sm = Feng.Utils.ReflectionHelper.CreateGenericType(Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.NH.SearchManager`1, Feng.Windows.Controller"), 
                        new Type[] { Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.ModelClassName) });
                    break;
                case "TYPEDDETAIL":
                    sm = Feng.Utils.ReflectionHelper.CreateGenericType(Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.NH.SearchManagerProxyDetail`1, Feng.Windows.Controller"),
                        new Type[] { Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.ModelClassName) });
                    break;
                case "TYPEDONETOSAME":
                    sm = Feng.Utils.ReflectionHelper.CreateGenericType(Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.NH.SearchManagerProxyOneToSame`1, Feng.Windows.Controller"),
                        new Type[] { Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.ModelClassName) });
                    break;
                case "UNTYPED":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManager, Feng.Windows.Controller");
                    break;
                case "UNTYPEDPROCEDURE":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerProcedure, Feng.Windows.Controller");
                    break;
                case "UNTYPEDFUNCTION":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerFunction, Feng.Windows.Controller");
                    break;
                case "UNTYPEDGROUPBYDETAIL":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerGroupByDetail, Feng.Windows.Controller");
                    break;
                case "UNTYPEDDETAILINMASTER":
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.Data.SearchManagerProxyDetailInMaster, Feng.Windows.Controller");
                    break;
                default:
                    sm = Feng.Utils.ReflectionHelper.GetTypeFromName(tabInfo.SearchManagerClassName);
                    break;
            }
            return sm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <param name="dmParent"></param>
        /// <returns></returns>
        public virtual ISearchManager GenerateSearchManager(WindowTabInfo tabInfo, IDisplayManager dmParent)
        {
            if (string.IsNullOrEmpty(tabInfo.SearchManagerClassName))
            {
                throw new ArgumentException("WindowTabInfo of " + tabInfo.Identity + " 's SearchManagerClassName must not be null!");
            }

            ISearchManager sm = null;
            if (dmParent != null)
            {
                if (string.IsNullOrEmpty(tabInfo.SearchManagerClassParams))
                {
                    sm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(GenerateSearchManagerType(tabInfo), dmParent) as ISearchManager;
                }
                else
                {
                    string[] s = Feng.Utils.StringHelper.Split(tabInfo.SearchManagerClassParams, ',');
                    object[] args = new object[s.Length + 1];
                    args[0] = dmParent;
                    for (int i = 0; i < s.Length; ++i)
                    {
                        args[i + 1] = s[i];
                    }
                    sm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(GenerateSearchManagerType(tabInfo), args) as ISearchManager;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(tabInfo.SearchManagerClassParams))
                {
                    sm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(GenerateSearchManagerType(tabInfo)) as ISearchManager;
                }
                else
                {
                    string[] s = Feng.Utils.StringHelper.Split(tabInfo.SearchManagerClassParams, ',');
                    sm = Feng.Utils.ReflectionHelper.CreateInstanceFromType(GenerateSearchManagerType(tabInfo), s) as ISearchManager;
                }
            }


            sm.Name = tabInfo.Identity;

            if (sm is IRepositoryConsumer)
            {
                (sm as IRepositoryConsumer).RepositoryCfgName = tabInfo.RepositoryConfigName;
            }

            sm.AdditionalSearchExpression = (string)ParamCreatorHelper.TryGetParam(tabInfo.WhereClause);
            sm.AdditionalSearchOrder = (string)ParamCreatorHelper.TryGetParam(tabInfo.OrderByClause);

            return sm;
        }
    }
}
