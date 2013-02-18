using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// SetupInfoDal
    /// SetCacheable（后面读取的语句也要加SetCacheable，系统才能知道从Cache读取）
    /// </summary>
    internal class ADInfoDal : Feng.ADInfoDal
    {
        //internal class AdInfoRepositoryConsumer : IRepositoryConsumer
        //{
        //    public string RepositoryCfgName 
        //    {
        //        get { return "ADBuffer"; }
        //        set { }
        //    }
        //}
        //private static AdInfoRepositoryConsumer m_consumer = new AdInfoRepositoryConsumer();

        //private NHibernate.ISession session = (new Repository()).Session;

        private static NH.INHibernateStatelessRepository GenerateStatelessRepository<T>()
        {
            //return ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(m_consumer);
            return new NH.StatelessRepository();
        }

        private static NH.INHibernateRepository GenerateStateRepository<T>()
        {
            //return ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(m_consumer);
            return new NH.Repository(string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal override IList<T> GetInfos<T>(string queryString = null, Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                queryString = string.Format("from {0} where IsActive = true", typeof(T).FullName);
            }
            else
            {
                if (!queryString.StartsWith("from"))
                {
                    queryString = string.Format("from {0} where {1} and IsActive = true", typeof(T).FullName, queryString);
                }
            }
            if (!queryString.Contains("order by"))
            {
                Type type = typeof(T);
                if (type.GetProperty("SeqNo") != null)
                {
                    queryString += " order by SeqNo";
                }
            }

            using (var rep = GenerateStatelessRepository<T>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var query = rep.Session.CreateQuery(queryString);
                if (parameters != null)
                {
                    foreach (var i in parameters)
                    {
                        query.SetParameter(i.Key, i.Value);
                    }
                }
                var ret = query.SetCacheable(true)
                    .List<T>();

                //rep.CommitTransaction();
                return ret;
            }
        }
        //internal IList<PluginInfo> GetPluginInfo()
        //{
        //    using (var rep = GenerateRepository<PluginInfo>())
        //    {
        //        //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        //        var ret = rep.Session.CreateCriteria(typeof(PluginInfo))
        //            .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
        //            .Add(NHibernate.Criterion.Expression.Or(
        //                NHibernate.Criterion.Expression.Eq("EnableUser", SystemConfiguration.UserName),
        //                NHibernate.Criterion.Expression.In("EnableRole", SystemConfiguration.Roles)))
        //            .List<PluginInfo>();

        //        //rep.CommitTransaction();

        //        return ret;
        //    }
        //}

        #region "Menu"
        internal IList<MenuInfo> GetAllMenuInfos()
        {
            IList<MenuInfo> listAll;
            using (var rep = GenerateStatelessRepository<MenuInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                /*NHibernate.Criterion.DetachedCriteria.For<MenuInfo>()
                        .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                        .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo")));*/

                listAll = rep.Session.CreateCriteria(typeof(MenuInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .List<MenuInfo>();

                //rep.CommitTransaction();
            }
            return listAll;
        }

        internal MenuInfo GetMenuInfo(string name)
        {
            MenuInfo ret;
            using (var rep = GenerateStatelessRepository<MenuInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                /*NHibernate.Criterion.DetachedCriteria.For<MenuInfo>()
                        .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                        .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo")));*/

                ret = rep.Session.CreateCriteria(typeof(MenuInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("ID", name))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .UniqueResult<MenuInfo>();

                //rep.CommitTransaction();
            }
            return ret;
        }
        #endregion

        #region "Grid"
        internal IList<GridInfo> GetGridInfos(string gridName)
        {
            using (var rep = GenerateStatelessRepository<GridInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<GridInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<GridColumnInfo> GetGridColumnInfos(string gridName)
        {
            using (var rep = GenerateStatelessRepository<GridColumnInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridColumnInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<GridColumnInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<GridRowInfo> GetGridRowInfos(string gridName)
        {
            using (var rep = GenerateStatelessRepository<GridRowInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridRowInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<GridRowInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<GridCellInfo> GetGridCellInfos(string gridName)
        {
            using (var rep = GenerateStatelessRepository<GridCellInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridCellInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<GridCellInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<GridGroupInfo> GetGridGroupInfos(string gridName)
        {
            using (var rep = GenerateStatelessRepository<GridGroupInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridGroupInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<GridGroupInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }
        #endregion

        #region "Others"
        internal IList<GridRelatedInfo> GetGridRelatedInfo(string gridName)
        {
            using (var rep = GenerateStatelessRepository<GridRelatedInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridRelatedInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<GridRelatedInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        //internal IList<MenuPropertyInfo> GetMenuPropertyInfo(string name)
        //{
        //    using (var rep = CreateRepository<MenuPropertyInfo>())
        //    using (var tx = rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) 
        //    {
        //        var ret = rep.Session.CreateCriteria(typeof(MenuPropertyInfo))
        //            .Add(NHibernate.Criterion.Expression.Eq("ID", name))
        //            .SetCacheable(true)
        //            .List<MenuPropertyInfo>();

        //        //rep.CommitTransaction();
        //        return ret;
        //    }
        //}

        internal IList<GridFilterInfo> GetGridFilterInfos(string gridName)
        {
            using (var rep = GenerateStatelessRepository<GridFilterInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridFilterInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GridName", gridName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<GridFilterInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        //internal IList<CustomSearchControlInfo> GetCustomSearchControlInfo(string groupName)
        //{
        //    using (var rep = CreateRepository<CustomSearchControlInfo>())
        //    using (var tx = rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) 
        //    {
        //        var ret = rep.Session.CreateCriteria(typeof(CustomSearchControlInfo))
        //            .Add(NHibernate.Criterion.Expression.Eq("groupName", groupName))
        //            .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
        //            .SetCacheable(true)
        //            .List<CustomSearchControlInfo>();

        //        //rep.CommitTransaction();
        //        return ret;
        //    }
        //}

        internal IList<CustomSearchInfo> GetCustomSearchInfo(string groupName)
        {
            using (var rep = GenerateStatelessRepository<CustomSearchInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(CustomSearchInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GroupName", groupName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<CustomSearchInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<WindowSelectInfo> GetWindowSelectInfo(string groupName)
        {
            using (var rep = GenerateStatelessRepository<WindowSelectInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WindowSelectInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GroupName", groupName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<WindowSelectInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<TaskInfo> GetTaskInfo(string groupName)
        {
            using (var rep = GenerateStatelessRepository<TaskInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(TaskInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GroupName", groupName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<TaskInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<GridColumnWarningInfo> GetWarningInfo(string groupName)
        {
            using (var rep = GenerateStatelessRepository<GridColumnWarningInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(GridColumnWarningInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("GroupName", groupName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<GridColumnWarningInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<ServerTaskScheduleInfo> GetTaskScheduleInfo()
        {
            using (var rep = GenerateStatelessRepository<ServerTaskScheduleInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(ServerTaskScheduleInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<ServerTaskScheduleInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<WebServiceInfo> GetWebServiceInfos()
        {
            using (var rep = GenerateStatelessRepository<WebServiceInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WebServiceInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<WebServiceInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<ParamCreatorInfo> GetParamCreatorInfos(string paramName)
        {
            using (var rep = GenerateStatelessRepository<ParamCreatorInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(ParamCreatorInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .Add(NHibernate.Criterion.Expression.Eq("ParamName", paramName))
                    .SetCacheable(true)
                    .List<ParamCreatorInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }
        #endregion

        #region "Window"
        internal IList<ActionInfo> GetActionInfos()
        {
            using (var rep = GenerateStatelessRepository<ActionInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(ActionInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<ActionInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<WindowInfo> GetWindowInfos()
        {
            using (var rep = GenerateStatelessRepository<WindowInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WindowInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<WindowInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal WindowInfo GetWindowInfo(string Name)
        {
            using (var rep = GenerateStatelessRepository<WindowInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WindowInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("ID", Name))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .FutureValue<WindowInfo>().Value;


                //rep.CommitTransaction();

                return ret;
            }
        }

        internal WindowInfo GetWindowTabInfo(string Name)
        {
            using (var rep = GenerateStatelessRepository<WindowInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WindowInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("ID", Name))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .UniqueResult<WindowInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        internal IList<WindowTabInfo> GetWindowTabInfos()
        {
            using (var rep = GenerateStatelessRepository<WindowTabInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WindowTabInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<WindowTabInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal IList<WindowTabInfo> GetTabInfo(string windowId)
        {
            using (var rep = GenerateStatelessRepository<WindowTabInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WindowTabInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("Window.Name", windowId))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .Add(NHibernate.Criterion.Expression.IsNull("ParentId"))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<WindowTabInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal FormInfo GetFormInfo(string Name)
        {
            using (var rep = GenerateStatelessRepository<FormInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(FormInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("ID", Name))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<FormInfo>();

                //rep.CommitTransaction();

                if (ret.Count > 0)
                {
                    return ret[0];
                }
                else
                {
                    return null;
                }
            }
        }

        internal ProcessInfo GetProcessInfo(string Name)
        {
            using (var rep = GenerateStatelessRepository<ProcessInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(ProcessInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("ID", Name))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<ProcessInfo>();

                //rep.CommitTransaction();

                if (ret.Count > 0)
                {
                    return ret[0];
                }
                else
                {
                    return null;
                }
            }
        }

        internal IList<EventProcessInfo> GetEventProcessInfos(string eventName)
        {
            using (var rep = GenerateStatelessRepository<EventProcessInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(EventProcessInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("EventName", eventName))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<EventProcessInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        internal IList<WindowMenuInfo> GetWindowMenuInfo(string windowId)
        {
            using (var rep = GenerateStatelessRepository<WindowMenuInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(WindowMenuInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("Window.Name", windowId))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .AddOrder(NHibernate.Criterion.Order.Asc("SeqNo"))
                    .SetCacheable(true)
                    .List<WindowMenuInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        internal NameValueMappingInfo GetNameValueMappingInfo(string Name)
        {
            using (var rep = GenerateStatelessRepository<NameValueMappingInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(NameValueMappingInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("ID", Name))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .UniqueResult<NameValueMappingInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        internal IList<AlertRuleInfo> GetAlertRuleInfo()
        {
            using (var rep = GenerateStatelessRepository<AlertRuleInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(AlertRuleInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .SetCacheable(true)
                    .List<AlertRuleInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        internal IList<AlertInfo> GetAlertInfo()
        {
            using (var rep = GenerateStatelessRepository<AlertInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(AlertInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .Add(NHibernate.Criterion.Expression.Eq("IsFixed", false))
                    .Add(NHibernate.Criterion.Expression.Or(
                        NHibernate.Criterion.Expression.Eq("RecipientUser", SystemConfiguration.UserName),
                        NHibernate.Criterion.Expression.In("RecipientRole", SystemConfiguration.Roles)))
                    .List<AlertInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        #endregion

        #region "Report"
        internal ReportInfo GetReportInfo(string Name)
        {
            using (var rep = GenerateStatelessRepository<ReportInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(ReportInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .Add(NHibernate.Criterion.Expression.Eq("ID", Name))
                    .UniqueResult<ReportInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        internal IList<ReportDataInfo> GetReportDataInfo(string reportId)
        {
            using (var rep = GenerateStatelessRepository<ReportDataInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria(typeof(ReportDataInfo))
                    .Add(NHibernate.Criterion.Expression.Eq("IsActive", true))
                    .Add(NHibernate.Criterion.Expression.Eq("Report.Name", reportId))
                    .List<ReportDataInfo>();

                //rep.CommitTransaction();

                return ret;
            }
        }

        #endregion

        #region "ConfigData"
        internal UserConfigurationInfo GetUserConfigurationInfo(string userName)
        {
            using (var rep = GenerateStatelessRepository<UserConfigurationInfo>())
            {
                //rep.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var ret = rep.Session.CreateCriteria<UserConfigurationInfo>()
                    .Add(NHibernate.Criterion.Expression.Eq("UserName", userName))
                    .UniqueResult<UserConfigurationInfo>();

                //rep.CommitTransaction();
                return ret;
            }
        }

        internal override void GetUserConfigurationData(UserConfigurationInfo userInfo)
        {
            using (var rep = GenerateStateRepository<UserConfigurationInfo>())
            {
                rep.Session.Lock("Feng.UserConfigurationInfo", userInfo, NHibernate.LockMode.Read);
                byte[] b = userInfo.UserData;

                Logger.Debug("Retrive UserData. length is " + b.Length.ToString());
            }
        }

        internal override void SaveOrUpdateUserConfigurationInfo(UserConfigurationInfo userInfo)
        {
            using (var rep = GenerateStateRepository<UserConfigurationInfo>())
            {
                if (userInfo.Version == 0)
                {
                    userInfo.Created = System.DateTime.Now;
                    userInfo.CreatedBy = SystemConfiguration.UserName;
                }
                else
                {
                    userInfo.Updated = System.DateTime.Now;
                    userInfo.UpdatedBy = SystemConfiguration.UserName;
                }

                try
                {
                    rep.BeginTransaction();
                    rep.SaveOrUpdate(userInfo);
                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                    throw;
                }
            }
        }
        #endregion
    }
}