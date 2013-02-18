using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Application Dictionary Info Bll
    /// </summary>
    public class ADInfoBll : Singleton<ADInfoBll>
    {
        public ADInfoBll()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings[Feng.Windows.Utils.SecurityHelper.DataConnectionStringName] == null)
            {
                m_dal = new ADInfoDal();
                m_useWSWindowTab = true;
            }
            else
            {
                m_dal = new Feng.NH.ADInfoDal();
                m_useWSWindowTab = false;
            }
        }
        private ADInfoDal m_dal;

        private string GetCacheKey<T>(string Name, string otherKey)
             where T : class
        {
            if (string.IsNullOrEmpty(otherKey))
            {
                return string.Format("ApplicationInfo, {0}:{1}", typeof(T).FullName, Name);
            }
            else
            {
                return string.Format("ApplicationInfo, {0}:{1}, {2}", typeof(T).FullName, Name, otherKey);
            }
        }

        private string GetCacheKey<T>(string Name)
            where T : class
        {
            return GetCacheKey<T>(Name, null);
        }

        #region "Get Common Info"
        internal T GetInfo<T>(string idName)
            where T : class, new()
        {
            try
            {
                return m_dal.GetInfo<T>(idName);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetInfos<T>()
            where T : class, new()
        {
            try
            {
                return Cache.TryGetCache<IList<T>>(GetCacheKey<T>("All"), new Func<IList<T>>(delegate()
                {
                    return m_dal.GetInfos<T>();
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            return EmptyInstance.GetEmpty<List<T>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IList<T> GetInfos<T>(string queryString)
            where T : class, new()
        {
            try
            {
                return Cache.TryGetCache<IList<T>>(GetCacheKey<T>(queryString), new Func<IList<T>>(delegate()
                {
                    return m_dal.GetInfos<T>(queryString);
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            return EmptyInstance.GetEmpty<List<T>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<T> GetInfos<T>(string queryString, Dictionary<string, object> parameters)
             where T : class, new()
        {
            try
            {
                return m_dal.GetInfos<T>(queryString, parameters);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            return EmptyInstance.GetEmpty<List<T>>();
        }
        #endregion

        #region "BufferData"
        /// <summary>
        /// GetWindowTabEventInfos
        /// </summary>
        /// <param name="windowTabId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IList<WindowTabEventInfo> GetWindowTabEventInfos(string windowTabId, WindowTabEventManagerType type)
        {
            return Cache.TryGetCache<IList<WindowTabEventInfo>>(GetCacheKey<WindowTabEventInfo>(windowTabId, type.ToString()), new Func<IList<WindowTabEventInfo>>(delegate()
            {
                IList<WindowTabEventInfo> list = GetInfos<WindowTabEventInfo>("ParentWindowTab.ID = :windowTabId and ManagerType = :type",
                new Dictionary<string, object> { { "windowTabId", windowTabId }, { "type", type } });

                return list;
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public ResourceInfo GetResourceInfo(string resourceName, ResourceType resourceType)
        {
            return Cache.TryGetCache<ResourceInfo>(GetCacheKey<ResourceInfo>(resourceName), new Func<ResourceInfo>(delegate()
            {
                IList<ResourceInfo> list = GetInfos<ResourceInfo>("ResourceName = :resourceName and ResourceType = :resourceType",
                new Dictionary<string, object> { { "resourceName", resourceName }, { "resourceType", resourceType } });

                System.Diagnostics.Debug.Assert(list.Count <= 1, "Too much resource with same name!");
                if (list.Count == 0)
                    return null;
                else
                    return list[0];
            }));
        }


        /// <summary>
        /// 得到顶级<see cref="MenuInfo"/>数(Parent is null)
        /// </summary>
        /// <returns>顶级菜单信息</returns>
        public IList<MenuInfo> GetTopMenuInfos()
        {
            try
            {
                IList<MenuInfo> listAll = null;
                listAll = Cache.TryGetCache<IList<MenuInfo>>(GetCacheKey<MenuInfo>("All"), new Func<IList<MenuInfo>>(delegate()
                {
                    try
                    {
                        return GetInfos<MenuInfo>();
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        return null;
                    }
                }));

                if (listAll == null)
                    return null;

                ICache c = ServiceProvider.GetService<ICache>();
                if (c != null)
                {
                    foreach (MenuInfo info in listAll)
                    {
                        c.Put(GetCacheKey<MenuInfo>(info.ID), info);
                    }
                }

                IList<MenuInfo> listTop = new List<MenuInfo>();
                foreach (MenuInfo menu in listAll)
                {
                    if (menu.ParentMenu == null)
                    {
                        listTop.Add(menu);
                        SearchMenuChilds(menu, listAll);
                    }
                }

                return listTop;
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        private void SearchMenuChilds(MenuInfo parent, IList<MenuInfo> listAll)
        {
            parent.ChildMenus = new List<MenuInfo>();
            foreach (MenuInfo menu in listAll)
            {
                if (menu.ParentMenu != null
                    && menu.ParentMenu.ID == parent.ID)
                {
                    menu.ParentMenu = parent;
                    parent.ChildMenus.Add(menu);

                    SearchMenuChilds(menu, listAll);
                }
            }
        }

        /// <summary>
        /// 根据<see cref="P:Feng.MenuInfo.Name"/>得到<see cref="MenuInfo"/>数据
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns>菜单配置信息</returns>
        public MenuInfo GetMenuInfo(string menuName)
        {
            return Cache.TryGetCache<MenuInfo>(GetCacheKey<MenuInfo>(menuName), new Func<MenuInfo>(delegate()
            {
                return GetInfo<MenuInfo>(menuName);
            }));
        }

        private const string m_defaultName = "Default";
        private GridInfo m_defaultGridInfo = new GridInfo
        {
            ID = "defaultGridInfo",
            AllowInsert = "TRUE",
            AllowEdit = "TRUE",
            AllowDelete = "TRUE",
            AllowOperationInsert = "TRUE",
            AllowOperationEdit = "TRUE",
            AllowOperationDelete = "TRUE",
            AllowInnerInsert = "TRUE",
            AllowInnerEdit = "TRUE",
            AllowInnerDelete = "TRUE",
            Visible = "TRUE",
            AllowExcelOperation = "FALSE",
            AllowInnerFilter = null,
            AllowInnerMenu = null,
            AllowInnerSearch = null,
            AllowInnerTextFilter = null
        };

        /// <summary>
        /// 根据<see cref="GridInfo.GridName"/>得到<see cref="GridInfo"/>数据
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns>表格配置信息</returns>
        public GridInfo GetGridInfo(string gridName)
        {
            return Cache.TryGetCache<GridInfo>(GetCacheKey<GridInfo>(gridName), new Func<GridInfo>(delegate()
                {
                    try
                    {
                        IList<GridInfo> gridInfos = GetInfos<GridInfo>(string.Format("GridName = '{0}'", gridName));
                        if (gridInfos.Count > 0)
                        {
                            return gridInfos[0];
                        }
                        else
                        {
                            gridInfos = GetInfos<GridInfo>(string.Format("GridName = '{0}'", m_defaultName));
                            if (gridInfos.Count > 0)
                            {
                                return gridInfos[0];
                            }
                            else
                            {
                                return m_defaultGridInfo;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        return null;
                    }
                }));
        }

        /// <summary>
        /// 根据<see cref="GridColumnInfo.GridName"/>得到<see cref="GridColumnInfo"/>数据
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns>表格列配置信息</returns>
        public IList<GridColumnInfo> GetGridColumnInfos(string gridName)
        {
            return Cache.TryGetCache<IList<GridColumnInfo>>(GetCacheKey<GridColumnInfo>(gridName), new Func<IList<GridColumnInfo>>(delegate()
                {
                    try
                    {
                        IList<GridColumnInfo> ret = GetInfos<GridColumnInfo>(string.Format("GridName = '{0}'", gridName));
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        return null;
                    }
                }));
        }

        private GridRowInfo m_defaultGridRowInfo = new GridRowInfo
        {
            ID = "defaultGridRowInfo",
            ReadOnly = "FALSE",
            AllowDelete = "TRUE",
            Visible = "TRUE",
            DetailGridReadOnly = null
        };

        /// <summary>
        /// 根据<see cref="GridRowInfo.GridName"/>得到<see cref="GridRowInfo"/>数据
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns>表格行配置信息</returns>
        public GridRowInfo GetGridRowInfo(string gridName)
        {
            return Cache.TryGetCache<GridRowInfo>(GetCacheKey<GridRowInfo>(gridName), new Func<GridRowInfo>(delegate()
                {
                    try
                    {
                        IList<GridRowInfo> ret = GetInfos<GridRowInfo>(string.Format("GridName = '{0}'", gridName));
                        if (ret.Count > 0)
                        {
                            return ret[0];
                        }
                        else
                        {
                            ret = GetInfos<GridRowInfo>(string.Format("GridName = '{0}'", m_defaultName));
                            if (ret.Count > 0)
                            {
                                return ret[0];
                            }
                            else
                            {
                                return m_defaultGridRowInfo;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        return null;
                    }
                }));
        }

        /// <summary>
        /// 根据<see cref="GridCellInfo.GridName"/>得到<see cref="GridCellInfo"/>数据
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns>表格单元格配置信息</returns>
        public IList<GridCellInfo> GetGridCellInfos(string gridName)
        {
            return Cache.TryGetCache<IList<GridCellInfo>>(GetCacheKey<GridCellInfo>(gridName), new Func<IList<GridCellInfo>>(delegate()
                {
                    try
                    {
                        IList<GridCellInfo> ret = GetInfos<GridCellInfo>(string.Format("GridName = '{0}'", gridName));
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithResume(ex);
                        return null;
                    }
                }));
        }


        /// <summary>
        /// 根据<see cref="GridRelatedInfo.GridName"/>得到<see cref="GridRelatedInfo"/>数据
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns>表格相关信息配置信息</returns>
        public IList<GridRelatedInfo> GetGridRelatedInfo(string gridName)
        {
            if (string.IsNullOrEmpty(gridName))
            {
                return null;
            }

            return Cache.TryGetCache<IList<GridRelatedInfo>>(GetCacheKey<GridRelatedInfo>(gridName), new Func<IList<GridRelatedInfo>>(delegate()
            {
                try
                {
                    IList<GridRelatedInfo> ret = GetInfos<GridRelatedInfo>(string.Format("GridName = '{0}'", gridName));
                    return ret;
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns></returns>
        public IList<GridRelatedAddressInfo> GetGridRelatedAddressInfo(string gridName)
        {
            if (string.IsNullOrEmpty(gridName))
            {
                return null;
            }

            return Cache.TryGetCache<IList<GridRelatedAddressInfo>>(GetCacheKey<GridRelatedAddressInfo>(gridName), new Func<IList<GridRelatedAddressInfo>>(delegate()
            {
                try
                {
                    return GetInfos<GridRelatedAddressInfo>(string.Format("GridName = '{0}'", gridName));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }

        /// <summary>
        /// 根据<see cref="GridFilterInfo.GridName"/>得到<see cref="GridFilterInfo"/>数据
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns>表格筛选配置信息</returns>
        public IList<GridFilterInfo> GetGridFilterInfos(string gridName)
        {
            return Cache.TryGetCache<IList<GridFilterInfo>>(GetCacheKey<GridFilterInfo>(gridName), new Func<IList<GridFilterInfo>>(delegate()
            {
                try
                {
                    return GetInfos<GridFilterInfo>(string.Format("GridName = '{0}'", gridName));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }

        /// <summary>
        /// 根据<see cref="GridGroupInfo.GridName"/>得到<see cref="GridGroupInfo"/>数据
        /// </summary>
        /// <param name="gridName"></param>
        /// <returns>表格分组配置信息</returns>
        public IList<GridGroupInfo> GetGridGroupInfos(string gridName)
        {
            if (string.IsNullOrEmpty(gridName))
            {
                return null;
            }

            return Cache.TryGetCache<IList<GridGroupInfo>>(GetCacheKey<GridGroupInfo>(gridName), new Func<IList<GridGroupInfo>>(delegate()
            {
                try
                {
                    return GetInfos<GridGroupInfo>(string.Format("GridName = '{0}'", gridName));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }

        /// <summary>
        /// 根据<see cref="TaskInfo.GroupName"/>得到<see cref="TaskInfo"/>数据
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>任务配置信息</returns>
        public IList<TaskInfo> GetTaskInfo(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return null;
            }

            return Cache.TryGetCache<IList<TaskInfo>>(GetCacheKey<TaskInfo>(groupName), new Func<IList<TaskInfo>>(delegate()
            {
                try
                {
                    return GetInfos<TaskInfo>(string.Format("GroupName = '{0}'", groupName));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }

        /// <summary>
        /// 根据<see cref="GridColumnWarningInfo.GroupName"/>得到<see cref="GridColumnWarningInfo"/>数据
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>表格警示单元格配置信息</returns>
        public IList<GridColumnWarningInfo> GetWarningInfo(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return null;
            }

            return Cache.TryGetCache<IList<GridColumnWarningInfo>>(GetCacheKey<GridColumnWarningInfo>(groupName), new Func<IList<GridColumnWarningInfo>>(delegate()
            {
                try
                {
                    return GetInfos<GridColumnWarningInfo>(string.Format("GroupName = '{0}'", groupName));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }


        ///// <summary>
        ///// 根据<see cref="P:Feng.MenuPropertyInfo.Name"/>得到<see cref="MenuPropertyInfo"/>数据
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns>菜单工具栏按钮配置信息</returns>
        //public MenuPropertyInfo GetMenuPropertyInfo(string name)
        //{
        //    try
        //    {
        //        IList<MenuPropertyInfo> list = GetMenuPropertyInfo(name);
        //        if (list.Count > 0)
        //        {
        //            return list[0];
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionProcess.ProcessWithResume(ex);
        //        return null;
        //    }
        //}

        

        /// <summary>
        /// 根据<see cref="WindowSelectInfo.GroupName"/>得到<see cref="CustomSearchInfo"/>数据
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>自定义查找配置信息</returns>
        public IList<CustomSearchInfo> GetCustomSearchInfo(string groupName)
        {
            return Cache.TryGetCache<IList<CustomSearchInfo>>(GetCacheKey<CustomSearchInfo>(groupName), new Func<IList<CustomSearchInfo>>(delegate()
            {
                try
                {
                    return GetInfos<CustomSearchInfo>(string.Format("GroupName = '{0}'", groupName));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }

        /// <summary>
        /// 根据<see cref="WindowSelectInfo.GroupName"/>得到<see cref="WindowSelectInfo"/>数据
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>选择窗体配置信息</returns>
        public IList<WindowSelectInfo> GetWindowSelectInfo(string groupName)
        {
            return Cache.TryGetCache<IList<WindowSelectInfo>>(GetCacheKey<WindowSelectInfo>(groupName), new Func<IList<WindowSelectInfo>>(delegate()
            {
                try
                {
                    return GetInfos<WindowSelectInfo>(string.Format("GroupName = '{0}'", groupName));
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithResume(ex);
                    return null;
                }
            }));
        }

        /// <summary>
        /// 根据<see cref="P:Feng.WindowInfo.Name"/>得到<see cref="WindowInfo"/>数据
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns>普通窗口配置信息</returns>
        public WindowInfo GetWindowInfo(string windowName)
        {
            return Cache.TryGetCache<WindowInfo>(GetCacheKey<WindowInfo>(windowName), new Func<WindowInfo>(delegate()
            {
                WindowInfo r = null;
                ICache c = ServiceProvider.GetService<ICache>();
                IEnumerable<WindowInfo> list = GetInfos<WindowInfo>();
                foreach (WindowInfo info in list)
                {
                    if (c != null)
                    {
                        c.Put(GetCacheKey<WindowInfo>(info.ID), info);
                    }

                    if (info.ID == windowName)
                    {
                        r = info;
                    }
                }
                return r;
            }));

        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IList<WindowTabInfo> GetWindowTabInfos()
        //{
        //    try
        //    {
        //        return GetWindowTabInfos();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionProcess.ProcessWithResume(ex);
        //        return null;
        //    }
        //}

        private bool m_useWSWindowTab = false;
        public WindowTabInfo GetWindowTabInfo(string windowTabId)
        {
            if (m_useWSWindowTab && !windowTabId.StartsWith("WS_"))
            {
                var r = GetWindowTabInfoReal("WS_" + windowTabId);
                if (r == null)
                {
                    r = GetWindowTabInfoReal("WSV_" + windowTabId);
                }
                if (r != null)
                    return r;
            }
            return GetWindowTabInfoReal(windowTabId);
        }

        /// <summary>
        /// 根据<see cref="P:Feng.WindowTabInfo.Name"/>得到<see cref="WindowTabInfo"/>数据
        /// </summary>
        /// <param name="windowTabId"></param>
        /// <returns></returns>
        private WindowTabInfo GetWindowTabInfoReal(string windowTabId)
        {
            return Cache.TryGetCache<WindowTabInfo>(GetCacheKey<WindowTabInfo>(windowTabId), new Func<WindowTabInfo>(delegate()
            {
                WindowTabInfo r = null;
                ICache c = ServiceProvider.GetService<ICache>();

                IEnumerable<WindowTabInfo> listAll = GetInfos<WindowTabInfo>();
                foreach (WindowTabInfo info in listAll)
                {
                    if (info.Parent == null)
                    {
                        SearchTabChilds(info, listAll);
                    }

                    if (c != null)
                    {
                        c.Put(GetCacheKey<WindowTabInfo>(info.ID), info);
                    }

                    if (info.ID == windowTabId)
                    {
                        r = info;
                    }
                }
                return r;
            }));
        }

        /// <summary>
        /// 根据<see cref="P:Feng.TabInfo.WindowId"/>得到<see cref="WindowTabInfo"/>数据
        /// </summary>
        /// <param name="windowId"></param>
        /// <returns>普通窗口分级配置信息</returns>
        public IList<WindowTabInfo> GetWindowTabInfosByWindowId(string windowId)
        {
            return Cache.TryGetCache<IList<WindowTabInfo>>(GetCacheKey<WindowTabInfo>(windowId, "ListForWindowId"), new Func<IList<WindowTabInfo>>(delegate()
            {
                IList<WindowTabInfo> r = null;
                ICache c = ServiceProvider.GetService<ICache>();

                IEnumerable<WindowTabInfo> listAll = GetInfos<WindowTabInfo>();
                foreach (WindowTabInfo info in listAll)
                {
                    if (info.Window == null)
                        continue;
                    if (info.Parent == null)
                    {
                        SearchTabChilds(info, listAll);

                        IList<WindowTabInfo> list = new List<WindowTabInfo> { info };

                        if (c != null)
                        {
                            c.Put(GetCacheKey<WindowTabInfo>(info.Window.ID, "ListForWindowId"), list);
                        }

                        if (info.Window.ID == windowId
                            && (windowId.StartsWith("WS_")
                                || (!m_useWSWindowTab && !info.ID.StartsWith("WS_"))
                                || (m_useWSWindowTab && info.ID.StartsWith("WS_"))))
                        {
                            r = list;
                        }
                    }
                }
                return r;
            }));
        }

        private void SearchTabChilds(WindowTabInfo parent, IEnumerable<WindowTabInfo> listAll)
        {
            parent.ChildTabs = new List<WindowTabInfo>();
            foreach (WindowTabInfo menu in listAll)
            {
                if (menu.Parent != null
                    && menu.Parent.ID == parent.ID)
                {
                    menu.Parent = parent;
                    parent.ChildTabs.Add(menu);

                    SearchTabChilds(menu, listAll);
                }
            }
        }

        /// <summary>
        /// 根据<see cref="P:Feng.FormInfo.Name"/>得到<see cref="FormInfo"/>数据
        /// </summary>
        /// <param name="formName"></param>
        /// <returns>特殊窗口配置信息</returns>
        public FormInfo GetFormInfo(string formName)
        {
            return Cache.TryGetCache<FormInfo>(GetCacheKey<FormInfo>(formName), new Func<FormInfo>(delegate()
            {
                return GetInfo<FormInfo>(formName);
            }));
        }

        /// <summary>
        /// 根据<see cref="P:Feng.ProcessInfo.Name"/>得到<see cref="ProcessInfo"/>数据
        /// </summary>
        /// <param name="processName"></param>
        /// <returns>过程配置信息</returns>
        public ProcessInfo GetProcessInfo(string processName)
        {
            try
            {
                return Cache.TryGetCache<ProcessInfo>(GetCacheKey<ProcessInfo>(processName), new Func<ProcessInfo>(delegate()
                {
                    return GetInfo<ProcessInfo>(processName);
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据<see cref="P:Feng.EventProcessInfo.EventName"/>得到<see cref="EventProcessInfo"/>数据
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>过程配置信息</returns>
        public IList<EventProcessInfo> GetEventProcessInfos(string eventName)
        {
            try
            {
                return Cache.TryGetCache<IList<EventProcessInfo>>(GetCacheKey<EventProcessInfo>(eventName), new Func<IList<EventProcessInfo>>(delegate()
                {
                    return GetInfos<EventProcessInfo>(string.Format("EventName = '{0}'", eventName));
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }


        /// <summary>
        /// 根据<see cref="P:WindowMenuInfo.Name"/>得到<see cref="WindowMenuInfo"/>数据
        /// </summary>
        /// <param name="windowName"></param>
        /// <returns>普通窗口工具栏按钮配置信息</returns>
        public IList<WindowMenuInfo> GetWindowMenuInfo(string windowName)
        {
            try
            {
                return Cache.TryGetCache<IList<WindowMenuInfo>>(GetCacheKey<WindowMenuInfo>(windowName), new Func<IList<WindowMenuInfo>>(delegate()
                {
                    return GetInfos<WindowMenuInfo>(string.Format("Window.ID = '{0}'", windowName));
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据<see cref="P:Feng.NameValueMappingInfo.Name"/>得到<see cref="NameValueMappingInfo"/>数据
        /// </summary>
        /// <param name="nvName"></param>
        /// <returns>系统数据表缓存信息</returns>
        public NameValueMappingInfo GetNameValueMappingInfo(string nvName)
        {
            return Cache.TryGetCache<NameValueMappingInfo>(GetCacheKey<NameValueMappingInfo>(nvName), new Func<NameValueMappingInfo>(delegate()
            {
                return GetInfo<NameValueMappingInfo>(nvName);
            }));
        }


        /// <summary>
        /// 根据<see cref="P:Feng.ActionInfo.Name"/>得到<see cref="ActionInfo"/>数据
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns>得到<see cref="T:Feng.ActionInfo"/>信息</returns>
        public ActionInfo GetActionInfo(string actionName)
        {
            return Cache.TryGetCache<ActionInfo>(GetCacheKey<ActionInfo>(actionName), new Func<ActionInfo>(delegate()
            {
                ActionInfo r = null;
                ICache c = ServiceProvider.GetService<ICache>();

                IEnumerable<ActionInfo> list = GetInfos<ActionInfo>();
                foreach (ActionInfo info in list)
                {
                    if (c != null)
                    {
                        c.Put(GetCacheKey<ActionInfo>(info.ID), info);
                    }

                    if (info.ID == actionName)
                    {
                        r = info;
                    }
                }
                return r;
            }));
        }

        ///// <summary>
        ///// 得到所有<see cref="AlertRuleInfo"/>数据
        ///// </summary>
        ///// <returns>警告配置信息</returns>
        //public IList<AlertRuleInfo> GetAlertRuleInfo()
        //{
        //    try
        //    {
        //        return GetAlertRuleInfo();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionProcess.ProcessWithResume(ex);
        //        return null;
        //    }
        //}

        /// <summary>
        /// 根据<see cref="P:ReportInfo.Name"/>得到<see cref="ReportInfo"/>信息
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public ReportInfo GetReportInfo(string reportName)
        {
            try
            {
                return Cache.TryGetCache<ReportInfo>(GetCacheKey<ReportInfo>(reportName), new Func<ReportInfo>(delegate()
                {
                    return GetInfo<ReportInfo>(reportName);
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据<see cref="P:ReportDataInfo.ReportName"/>得到<see cref="ReportDataInfo"/>信息
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public IList<ReportDataInfo> GetReportDataInfo(string reportName)
        {
            try
            {
                return Cache.TryGetCache<IList<ReportDataInfo>>(GetCacheKey<ReportDataInfo>(reportName), new Func<IList<ReportDataInfo>>(delegate()
                {
                    return GetInfos<ReportDataInfo>(string.Format("Report.ID = '{0}'", reportName));
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IList<ServerTaskScheduleInfo> GetTaskScheduleInfo()
        //{
        //    try
        //    {
        //        return m_dal.GetTaskScheduleInfo();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionProcess.ProcessWithResume(ex);
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IList<WebServiceInfo> GetWebServiceInfos()
        //{
        //    try
        //    {
        //        return m_dal.GetWebServiceInfos();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionProcess.ProcessWithResume(ex);
        //        return null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public IList<ParamCreatorInfo> GetParamCreatorInfos(string paramName)
        {
            try
            {
                return Cache.TryGetCache<IList<ParamCreatorInfo>>(GetCacheKey<ParamCreatorInfo>(paramName), new Func<IList<ParamCreatorInfo>>(delegate()
                {
                    return GetInfos<ParamCreatorInfo>(string.Format("ParamName = '{0}'", paramName));
                }));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }
#endregion

        #region "NoBufferUserData"
        /// <summary>
        /// 根据用户名得到用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>用户设置信息</returns>
        public UserConfigurationInfo GetUserConfigurationInfo(string userName)
        {
            try
            {
                var list = m_dal.GetInfos<UserConfigurationInfo>(string.Format("UserName = '{0}'", userName));
                if (list != null && list.Count > 0)
                    return list[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        public void GetUserConfigurationData(UserConfigurationInfo userInfo)
        {
            try
            {
                m_dal.GetUserConfigurationData(userInfo);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return;
            }
        }
        /// <summary>
        /// 保存或更新用户数据
        /// </summary>
        /// <param name="userInfo"></param>
        public void SaveOrUpdateUserConfigurationInfo(UserConfigurationInfo userInfo)
        {
            m_dal.SaveOrUpdateUserConfigurationInfo(userInfo);
        }

        /// <summary>
        /// 得到所有<see cref="AlertInfo"/>数据
        /// </summary>
        /// <returns>警告信息</returns>
        public IList<AlertInfo> GetAlertInfo()
        {
            try
            {
                return GetInfos<AlertInfo>(string.Format("IsFixed = false and (RecipientUser = '{0}' or RecipientRole in '{1}",
                    SystemConfiguration.UserName, Feng.Utils.ConvertHelper.StringArrayToString(SystemConfiguration.Roles)));
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }
        #endregion
    }
}