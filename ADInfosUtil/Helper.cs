using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using NHibernate.Mapping.Attributes;
using Feng.Data;

namespace Feng.Windows.Utils
{
    public sealed class Helper
    {
        #region "Create AD Settings"

        private static Feng.NH.INHibernateRepository GenerateRepository()
        {
            return new Feng.NH.Repository("ADBuffer");
        }

        public static void CreateSettings4WebserviceTest()
        {
            Dictionary<string, int> already = new Dictionary<string, int>();
            var tabInfos = ADInfoBll.Instance.GetInfos<WindowTabInfo>();
            foreach(var i in tabInfos)
            {
                //if (i.Name.StartsWith("AD"))
                //    continue;
                CreateSettings4WSWintab(i.Name, !already.ContainsKey(i.GridName), true);
                already[i.GridName] = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateDefaultSettings()
        {
            ClearDefaultSettings();

            CreateTopMenu();

            CreateSettings<MenuInfo>("AD_Menu");
            CreateSettings<ActionInfo>("AD_Action");
            CreateSettings<ProcessInfo>("AD_Process");
            CreateSettings<FormInfo>("AD_Form");

            CreateSettings<WindowInfo>("AD_Window");
            CreateSettings<WindowTabInfo>("AD_Window_Tab");
            CreateSettings<WindowTabEventInfo>("AD_Window_Tab_Event");
            CreateSettings<WindowMenuInfo>("AD_Window_Menu");
            CreateSettings<EventProcessInfo>("AD_EventProcess");
            CreateSettings<WindowSelectInfo>("AD_Window_Select");

            CreateSettings<GridInfo>("AD_Grid");
            CreateSettings<GridColumnInfo>("AD_Grid_Column");
            CreateSettings<GridRowInfo>("AD_Grid_Row");
            CreateSettings<GridCellInfo>("AD_Grid_Cell");
            CreateSettings<GridRelatedInfo>("AD_Grid_Related");
            CreateSettings<GridRelatedAddressInfo>("AD_Grid_Related_Address");
            CreateSettings<GridGroupInfo>("AD_Grid_Group");
            CreateSettings<GridFilterInfo>("AD_Grid_Filter");
            CreateSettings<GridColumnWarningInfo>("AD_Grid_Column_Warning");

            CreateSettings<TaskInfo>("AD_Task");
            CreateSettings<AlertRuleInfo>("AD_AlertRule");
            CreateSettings<CommandBindingInfo>("AD_CommandBinding");

            CreateSettings<CustomSearchInfo>("AD_Search_Custom");

            CreateSettings<NameValueMappingInfo>("AD_NameValueMapping");
            CreateSettings<EntityBufferInfo>("AD_EntityBuffer");
            CreateSettings<SimpleParamInfo>("AD_SimpleParam");
            CreateSettings<ParamCreatorInfo>("AD_Param_Creator");
            CreateSettings<ResourceInfo>("AD_Resource");

            CreateSettings<ClientInfo>("AD_Client");
            CreateSettings<OrganizationInfo>("AD_Org");
            CreateSettings<RoleInfo>("AD_Role");
            CreateSettings<PluginInfo>("AD_Plugin");
            CreateSettings<WebServiceInfo>("AD_Web_Service");

            CreateSettings<UserConfigurationInfo>("SD_User_Configuration");
            CreateSettings<AlertInfo>("SD_Alert");
            CreateSettings<AuditLogRecord>("SD_AuditLog");

            CreateSettings<ReportInfo>("AD_Report");
            CreateSettings<ReportDataInfo>("AD_Report_Data");

            // maybe no gps domain
            //CreateFormSetting("SD_Track", "Feng.Windows.Forms.TrackForm, Feng.Windows.Application");
            using (var rep = GenerateRepository())
            {
                if (Feng.NH.TypedEntityMetadata.GenerateEntityInfo(rep.Session.SessionFactory, typeof(Track)).IdName != null)
                {
                    CreateSettings<Track>("SD_Track");
                    try
                    {
                        rep.BeginTransaction();

                        string formName = "SD_Track";
                        string className = "Feng.Windows.Forms.TrackForm, Feng.Windows.Application";

                        FormInfo formInfo = rep.Get<FormInfo>(formName);
                        if (formInfo == null)
                        {
                            formInfo = new FormInfo { ID = formName, Access = "I:*", Text = formName, ClassName = className, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true };
                            rep.Save(formInfo);
                        }

                        ActionInfo actionInfo = rep.Get<ActionInfo>(formName);
                        actionInfo.ActionType = ActionType.Form;
                        actionInfo.Form = formInfo;
                        rep.Update(actionInfo);

                        var columnInfos = rep.List<GridColumnInfo>("from GridColumnInfo where GridName = 'SD_Track'", null);
                        foreach (var i in columnInfos)
                        {
                            if (i.CellViewerManager == "DateTime")
                            {
                                i.CellViewerManagerParam = "yyyy-MM-dd HH:mm";
                                rep.Update(i);
                            }
                        }
                        rep.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        rep.RollbackTransaction();
                        ExceptionProcess.ProcessWithNotify(ex);
                    }
                }
            }

            // WinTab to Grids
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();

                    GridRelatedInfo info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Grid"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window_Tab",
                        ID = "WinTab_Grid",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "WinTab_Grid",
                        SeqNo = 0,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "GridName = %GridName%"
                    };
                    rep.Save(info);

                    info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Grid_Column"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window_Tab",
                        ID = "WinTab_Grid_Column",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "WinTab_Grid_Column",
                        SeqNo = 1,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "GridName = %GridName%"
                    };
                    rep.Save(info);

                    info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Grid_Row"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window_Tab",
                        ID = "WinTab_Grid_Row",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "WinTab_Grid_Row",
                        SeqNo = 2,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "GridName = %GridName%"
                    };
                    rep.Save(info);

                    info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Grid_Cell"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window_Tab",
                        ID = "WinTab_Grid_Cell",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "WinTab_Grid_Cell",
                        SeqNo = 3,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "GridName = %GridName%"
                    };
                    rep.Save(info);

                    info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Grid_Related"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window_Tab",
                        ID = "WinTab_Gird_Related",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "WinTab_Grid_Related",
                        SeqNo = 4,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "GridName = %GridName%"
                    };
                    rep.Save(info);

                    info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Grid_Group"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window_Tab",
                        ID = "WinTab_Gird_Group",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "WinTab_Grid_Group",
                        SeqNo = 5,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "GridName = %GridName%"
                    };
                    rep.Save(info);

                    info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Window"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window_Tab",
                        ID = "WinTab_Window",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "WinTab_Window",
                        SeqNo = 5,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "Name = %Window.Name%"
                    };
                    rep.Save(info);

                    info = new GridRelatedInfo
                    {
                        Action = rep.Get<ActionInfo>("AD_Window_Menu"),
                        ClientId = 0,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridLevel = null,
                        GridName = "AD_Window",
                        ID = "Window_WindowMenu",
                        OrgId = 0,
                        RelatedType = GridRelatedType.ByRows,
                        Text = "Window_WindowMenu",
                        SeqNo = 9,
                        Version = 0,
                        Visible = "I:*",
                        SearchExpression = "Window.Name = %Name%"
                    };
                    rep.Save(info);

                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        private static void CreateTopMenu()
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();

                    MenuInfo menuInfo = new MenuInfo { Action = null, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = "System", SeqNo = 999, Text = "System", ParentMenu = null, Visible = "R:" + SystemConfiguration.AdministratorsRoleName };
                    rep.Save(menuInfo);

                    menuInfo = new MenuInfo { Action = null, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = "SystemTest", SeqNo = 999, Text = "SystemTest", ParentMenu = null, Visible = "R:" + SystemConfiguration.AdministratorsRoleName };
                    rep.Save(menuInfo);

                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        private const string systemName = "system";
        private const string systemNameQuoto = "'system'";
        /// <summary>
        /// 清除默认配置
        /// </summary>
        public static void ClearDefaultSettings()
        {
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Web_Service WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Grid_Related WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Window_Menu WHERE CreatedBy = " + systemNameQuoto);

            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Grid WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Grid_Row WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Grid_Cell WHERE CreatedBy = " + systemNameQuoto);

            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Grid_Column WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Window_Tab WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Menu WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Action WHERE CreatedBy = " + systemNameQuoto);
            DbHelper.Instance.ExecuteNonQuery("DELETE FROM AD_Window WHERE CreatedBy = " + systemNameQuoto);
        }

        public static void CreateFormSetting(string formName, string className)
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();

                    FormInfo formInfo = new FormInfo { ID = formName, Access = "I:*", Text = formName, ClassName = className, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true };
                    rep.Save(formInfo);

                    // Action
                    ActionInfo actionInfo = new ActionInfo { ID = formName, Access = "I:*", ActionType = ActionType.Form, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, Form = formInfo };
                    rep.Save(actionInfo);

                    // Menu
                    MenuInfo topMenu = rep.Get<MenuInfo>("System");
                    MenuInfo menuInfo = new MenuInfo { Action = actionInfo, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = formName, SeqNo = 0, Text = formName, ParentMenu = topMenu, Visible = "I:*" };
                    rep.Save(menuInfo);

                    rep.CommitTransaction();
                }
                catch (Exception)
                {
                    rep.RollbackTransaction();
                }
            }

        }

        private const string s_idName = "ID";

        public static void CreateSettings4WSType(Type type, string gridName)
        {
            var srcWintabName = gridName;
            var srcWintabInfo = ADInfoBll.Instance.GetWindowTabInfo(srcWintabName);
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();

                    string typeName = GetTypeFullName(type);
                    // Tab
                    WindowTabInfo tabInfo = new WindowTabInfo
                    {
                        BusinessLayerClassName = srcWintabInfo.BusinessLayerClassName, //"Feng.BaseDao`1[[" + typeName + "]], Feng.Dao",
                        ControlManagerClassName = srcWintabInfo.ControlManagerClassName, //"Feng.Windows.Forms.ControlManager`1[[" + typeName + "]], Feng.Windows.Controller",
                        DisplayManagerClassName = srcWintabInfo.DisplayManagerClassName,
                        SearchManagerClassName = "Feng.Net.SearchManager`1[[" + typeName + "]], Feng.Windows.Controller",
                        SearchManagerClassParams = "REST, DSS_" + type.Name,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridName = srcWintabInfo.GridName,
                        ID = "WS_" + srcWintabName,
                        Text = srcWintabInfo.Text,
                        Window = ADInfoBll.Instance.GetWindowInfo(gridName),
                        IsInDetailForm = srcWintabInfo.IsInDetailForm,
                        IsInGrid = srcWintabInfo.IsInGrid,
                        OrderByClause = srcWintabInfo.OrderByClause,
                        WhereClause = srcWintabInfo.WhereClause,
                        Parent = srcWintabInfo.Parent == null ? null : ADInfoBll.Instance.GetWindowTabInfo("WS_" + srcWintabInfo.Parent.ID),
                        PositionChanged = srcWintabInfo.PositionChanged,
                        RepositoryConfigName = srcWintabInfo.RepositoryConfigName,
                        SelectedDataValueChanged = srcWintabInfo.SelectedDataValueChanged,
                        SeqNo = srcWintabInfo.SeqNo,
                    };
                    rep.Save(tabInfo);

                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        private static string GetTypeFullName(Type type)
        {
            string typeName = type.AssemblyQualifiedName;
            int idxType = typeName.IndexOf(',');
            idxType = typeName.IndexOf(',', idxType + 1);
            typeName = typeName.Substring(0, idxType);
            return typeName;
        }

        public static void CreateSettings4WSWintab(string srcWintabName, bool createGrid, bool createMenus)
        {
            string wintabName = "WSV_" + srcWintabName;
            var srcWintabInfo = ADInfoBll.Instance.GetWindowTabInfo(srcWintabName);
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();

                    // WebService
                    WebServiceInfo webServiceInfo = new WebServiceInfo { ID = wintabName, ExecuteParam = srcWintabName, Type = WebServiceType.DataSearchView, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true };
                    rep.Save(webServiceInfo);

                    WindowInfo windowInfo = null;
                    if (createMenus)
                    {
                        // Window
                        windowInfo = new WindowInfo { Access = "I:*", Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = wintabName, Text = wintabName, WindowType = WindowType.Query, GenerateDetailForm = true };
                        rep.Save(windowInfo);

                        // Action
                        ActionInfo actionInfo = new ActionInfo { ID = wintabName, Access = "I:*", ActionType = ActionType.Window, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, Window = windowInfo };
                        rep.Save(actionInfo);

                        // Menu
                        MenuInfo topMenu = rep.Get<MenuInfo>("SystemTest");
                        MenuInfo menuInfo = new MenuInfo { Action = actionInfo, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = wintabName, SeqNo = 0, Text = wintabName, ParentMenu = topMenu, Visible = "I:*" };
                        rep.Save(menuInfo);
                    }

                    // Tab
                    WindowTabInfo tabInfo = new WindowTabInfo
                    {
                        BusinessLayerClassName = "Feng.EmptyDao, Feng.Dao",
                        ControlManagerClassName = "Feng.Windows.Forms.ControlManager, Feng.Windows.Controller",
                        DisplayManagerClassName = "Feng.Windows.Forms.DisplayManager, Feng.Windows.Controller",
                        SearchManagerClassName = "Feng.Net.SearchManager, Feng.Windows.Controller",
                        SearchManagerClassParams = "REST, DSRS_" + srcWintabName,
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridName = "WSV_" + srcWintabInfo.GridName,
                        ID = wintabName,
                        Text = wintabName,
                        Window = windowInfo,
                        IsInDetailForm = srcWintabInfo.IsInDetailForm,
                        IsInGrid = srcWintabInfo.IsInGrid,
                        OrderByClause = srcWintabInfo.OrderByClause,
                        WhereClause = srcWintabInfo.WhereClause,
                        Parent = srcWintabInfo.Parent == null ? null : ADInfoBll.Instance.GetWindowTabInfo("WSV_" + srcWintabInfo.Parent.ID),
                        PositionChanged = srcWintabInfo.PositionChanged,
                        RepositoryConfigName = srcWintabInfo.RepositoryConfigName,
                        SelectedDataValueChanged = srcWintabInfo.SelectedDataValueChanged,
                        SeqNo = srcWintabInfo.SeqNo,
                    };
                    rep.Save(tabInfo);

                    if (createGrid)
                    {
                        string gridName = "WSV_" + srcWintabInfo.GridName;

                        // grid
                        GridInfo gridInfo = null;
                        var srcGridInfo = ADInfoBll.Instance.GetGridInfo(srcWintabInfo.GridName);
                        if (srcGridInfo == null)
                        {
                            gridInfo = new GridInfo
                            {
                                AllowDelete = "I:Nobody",
                                AllowEdit = "I:Nobody",
                                AllowInsert = "I:Nobody",
                                AllowInnerDelete = "I:Nobody",
                                AllowInnerEdit = "I:Nobody",
                                AllowInnerInsert = "I:Nobody",
                                AllowOperationDelete = "I:Nobody",
                                AllowOperationEdit = "I:Nobody",
                                AllowOperationInsert = "I:Nobody",
                                AllowExcelOperation = "I:Nobody",
                                ReadOnly = "I:*",
                                AllowInnerSearch = null,
                                AllowInnerFilter = null,
                                AllowInnerTextFilter = null,
                                AllowInnerMenu = "I:*",
                                ID = gridName,
                                GridName = gridName,
                                Visible = "I:*",
                                Created = System.DateTime.Now,
                                CreatedBy = systemName,
                                IsActive = true,
                                ClientId = 0,
                                OrgId = 0
                            };
                        }
                        else
                        {
                            gridInfo = new GridInfo
                            {
                                AllowDelete = srcGridInfo.AllowDelete,
                                AllowEdit = srcGridInfo.AllowEdit,
                                AllowInsert = srcGridInfo.AllowInsert,
                                AllowInnerDelete = srcGridInfo.AllowInnerDelete,
                                AllowInnerEdit = srcGridInfo.AllowInnerEdit,
                                AllowInnerInsert = srcGridInfo.AllowInnerInsert,
                                AllowOperationDelete = srcGridInfo.AllowOperationDelete,
                                AllowOperationEdit = srcGridInfo.AllowOperationEdit,
                                AllowOperationInsert = srcGridInfo.AllowOperationInsert,
                                AllowExcelOperation = srcGridInfo.AllowExcelOperation,
                                ReadOnly = srcGridInfo.ReadOnly,
                                AllowInnerSearch = srcGridInfo.AllowInnerSearch,
                                AllowInnerFilter = srcGridInfo.AllowInnerFilter,
                                AllowInnerTextFilter = srcGridInfo.AllowInnerTextFilter,
                                AllowInnerMenu = srcGridInfo.AllowInnerMenu,
                                ID = gridName,
                                GridName = gridName,
                                Visible = srcGridInfo.Visible,
                                Created = System.DateTime.Now,
                                CreatedBy = systemName,
                                IsActive = srcGridInfo.IsActive,
                                ClientId = srcGridInfo.ClientId,
                                OrgId = srcGridInfo.OrgId
                            };
                        }
                        rep.Save(gridInfo);

                        var srcGridColumnInfos = ADInfoBll.Instance.GetGridColumnInfos(srcWintabInfo.GridName);
                        int idx = 1;
                        foreach (GridColumnInfo info in srcGridColumnInfos)
                        {
                            string propertyName = info.GridColumnName;
                            int newIdx = idx;
                            GridColumnInfo gridColumnInfo;

                            if (info.GridColumnType != GridColumnType.Normal
                                && info.GridColumnType != GridColumnType.ExpressionColumn)
                            {
                                gridColumnInfo = new GridColumnInfo
                                {
                                    ID = gridName + "_" + propertyName,
                                    AllowSetList = info.AllowSetList,
                                    Caption = propertyName,
                                    ColumnVisible = info.ColumnVisible,
                                    Created = System.DateTime.Now,
                                    CreatedBy = systemName,
                                    IsActive = info.IsActive,
                                    DataControlVisible = info.DataControlVisible,
                                    SearchControlVisible = info.SearchControlVisible,
                                    GridName = gridName,
                                    GridColumnType = info.GridColumnType,
                                    PropertyName = propertyName,
                                    ReadOnly = info.ReadOnly,
                                    NotNull = info.NotNull,
                                    SeqNo = newIdx,
                                    TypeName = info.TypeName,
                                    DataControlType = info.DataControlType,
                                    SearchControlType = info.SearchControlType,
                                    CellEditorManager = info.CellEditorManager,
                                    CellViewerManager = info.CellViewerManager,
                                    SearchNullUseFull = info.SearchNullUseFull,
                                    ClientId = info.ClientId,
                                    OrgId = info.OrgId
                                };

                            }
                            else
                            {
                                Type propertyType = typeof(string);

                                string propertyTypeName = GetTypeFullName(propertyType);

                                gridColumnInfo = new GridColumnInfo
                                {
                                    ID = gridName + "_" + propertyName,
                                    AllowSetList = info.AllowSetList,
                                    Caption = propertyName,
                                    ColumnVisible = info.ColumnVisible,
                                    Created = System.DateTime.Now,
                                    CreatedBy = systemName,
                                    IsActive = info.IsActive,
                                    DataControlVisible = info.DataControlVisible,
                                    SearchControlVisible = info.SearchControlVisible,
                                    GridName = gridName,
                                    GridColumnType = GridColumnType.Normal,
                                    PropertyName = propertyName,
                                    ReadOnly = info.ReadOnly,
                                    NotNull = info.NotNull,
                                    SeqNo = newIdx,
                                    TypeName = propertyTypeName,
                                    DataControlType = GetDefaultDataControlType(propertyType),
                                    SearchControlType = GetDefaultSearchControlType(propertyType),
                                    CellEditorManager = GetDefaultGridEditor(propertyType),
                                    CellViewerManager = GetDefaultGridViewer(propertyType),
                                    SearchNullUseFull = info.SearchNullUseFull,
                                    ClientId = info.ClientId,
                                    OrgId = info.OrgId
                                };
                            }

                            gridColumnInfo.ID = gridColumnInfo.Name.Substring(0, Math.Min(50, gridColumnInfo.ID.Length));
                            rep.Save(gridColumnInfo);
                            idx++;
                        }
                    }
                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        /// <summary>
        /// 根据类型创建默认配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CreateSettings<T>(string gridName)
            where T : IEntity
        {
            CreateSettings(typeof(T), gridName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="gridName"></param>
        public static void CreateSettings(Type type, string gridName)
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();

                    // WebServvice
                    {
                        string typeName = GetTypeFullName(type);
                        WebServiceInfo webServiceInfo = new WebServiceInfo { ID = gridName, ExecuteParam = typeName, Type = WebServiceType.DataSearch, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true };
                        rep.Save(webServiceInfo);
                    }

                    // Window
                    WindowInfo windowInfo = new WindowInfo { Access = "I:*", Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = gridName, Text = gridName, WindowType = WindowType.Maintain, GenerateDetailForm = true };
                    rep.Save(windowInfo);

                    // Action
                    ActionInfo actionInfo = new ActionInfo { ID = gridName, Access = "I:*", ActionType = ActionType.Window, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, Window = windowInfo };
                    rep.Save(actionInfo);

                    // Menu
                    MenuInfo topMenu = rep.Get<MenuInfo>("System");
                    MenuInfo menuInfo = new MenuInfo { Action = actionInfo, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = gridName, SeqNo = 0, Text = gridName, ParentMenu = topMenu, Visible = "I:*" };
                    rep.Save(menuInfo);

                    // Tab
                    {
                        string typeName = GetTypeFullName(type);
                        WindowTabInfo tabInfo = new WindowTabInfo
                        {
                            BusinessLayerClassName = "Feng.BaseDao`1[[" + typeName + "]], Feng.Dao",
                            ControlManagerClassName = "Feng.Windows.Forms.ControlManager`1[[" + typeName + "]], Feng.Windows.Controller",
                            SearchManagerClassName = "Feng.NH.SearchManager`1[[" + typeName + "]], Feng.Windows.Controller",
                            Created = System.DateTime.Now,
                            CreatedBy = systemName,
                            IsActive = true,
                            GridName = gridName,
                            IsInDetailForm = true,
                            IsInGrid = true,
                            ID = gridName,
                            SeqNo = 0,
                            Text = gridName,
                            Window = windowInfo
                        };
                        if (typeof(IMultiOrgEntity).IsAssignableFrom(type))
                        {
                            tabInfo.BusinessLayerClassName = "Feng.MultiOrgEntityDao`1[[" + typeName + "]], Feng.Dao";
                        }
                        else if (typeof(ILogEntity).IsAssignableFrom(type))
                        {
                            tabInfo.BusinessLayerClassName = "Feng.LogEntityDao`1[[" + typeName + "]], Feng.Dao";
                        }
                        rep.Save(tabInfo);
                    }

                    // grid
                    GridInfo gridInfo = new GridInfo
                    {
                        AllowDelete = "I:*",
                        AllowEdit = "I:*",
                        AllowInsert = "I:*",
                        AllowInnerDelete = "I:*",
                        AllowInnerEdit = "I:*",
                        AllowInnerInsert = "I:*",
                        AllowOperationDelete = "I:*",
                        AllowOperationEdit = "I:*",
                        AllowOperationInsert = "I:*",
                        AllowExcelOperation = "I:*",
                        AllowInnerSearch = null,
                        AllowInnerFilter = null,
                        AllowInnerTextFilter = null,
                        AllowInnerMenu = "I:*",
                        ID = gridName,
                        GridName = gridName,
                        Visible = "I:*",
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        ClientId = 0,
                        OrgId = 0
                    };
                    rep.Save(gridInfo);

                    PropertyInfo[] pInfos = type.GetProperties();
                    int idx = 1;
                    foreach (PropertyInfo pInfo in pInfos)
                    {
                        if (pInfo.Name == "Version")
                            continue;
                        if (typeof(ILogEntity).IsAssignableFrom(type))
                        {
                            if (pInfo.Name == "Created" || pInfo.Name == "CreatedBy"
                                || pInfo.Name == "Updated" || pInfo.Name == "UpdatedBy")
                                continue;
                        }
                        if (typeof(IMultiOrgEntity).IsAssignableFrom(type))
                        {
                            if (pInfo.Name == "ClientId" || pInfo.Name == "OrgId")
                                continue;
                        }

                        if (!pInfo.CanWrite)
                            continue;

                        bool useObjectEditor = false;
                        bool createGridColumn = false;
                        if (pInfo.PropertyType.IsValueType || pInfo.PropertyType.IsEnum || pInfo.PropertyType == typeof(string))
                        {
                            if (Attribute.IsDefined(pInfo, typeof(NHibernate.Mapping.Attributes.PropertyAttribute))
                                || Attribute.IsDefined(pInfo, typeof(NHibernate.Mapping.Attributes.IdAttribute)))
                            {
                                createGridColumn = true;
                            }
                        }
                        else
                        {
                            NHibernate.Mapping.Attributes.ManyToOneAttribute p =
                                Attribute.GetCustomAttribute(pInfo, typeof(NHibernate.Mapping.Attributes.ManyToOneAttribute)) as NHibernate.Mapping.Attributes.ManyToOneAttribute;
                            if (p != null && p.Insert && p.Update)
                            {
                                createGridColumn = true;
                                useObjectEditor = true;
                            }
                        }

                        if (createGridColumn)
                        {
                            int newIdx = idx;
                            string newPropertyName = pInfo.Name;
                            if (pInfo.Name == s_idName)
                            {
                                newIdx = 0;
                                newPropertyName = "ID";
                            }
                            Type propertyType = pInfo.PropertyType;
                            if (pInfo.PropertyType.IsGenericType && pInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                propertyType = pInfo.PropertyType.GetGenericArguments()[0];
                            }
                            string propertyTypeName = GetTypeFullName(propertyType);

                            GridColumnInfo gridColumnInfo = new GridColumnInfo
                            {
                                ID = gridName + "_" + pInfo.Name,
                                AllowSetList = false,
                                Caption = pInfo.Name,
                                ColumnVisible = "I:*",
                                Created = System.DateTime.Now,
                                CreatedBy = systemName,
                                IsActive = true,
                                DataControlVisible = "I:*",
                                SearchControlVisible = "I:*",
                                GridName = gridName,
                                GridColumnType = GridColumnType.Normal,
                                PropertyName = newPropertyName,
                                ReadOnly = "False",
                                NotNull = "False",
                                SeqNo = newIdx,
                                TypeName = propertyTypeName,
                                DataControlType = GetDefaultDataControlType(propertyType),
                                SearchControlType = GetDefaultSearchControlType(propertyType),
                                CellEditorManager = GetDefaultGridEditor(propertyType),
                                CellViewerManager = GetDefaultGridViewer(propertyType),
                                SearchNullUseFull = true,
                                ClientId = 0,
                                OrgId = 0
                            };

                            gridColumnInfo.ID = gridColumnInfo.Name.Substring(0, Math.Min(50, gridColumnInfo.Name.Length));

                            if (propertyType.IsEnum)
                            {
                                gridColumnInfo.CellEditorManagerParam = "Enum";
                                gridColumnInfo.SearchControlInitParam = "Enum";
                            }

                            if (useObjectEditor)
                            {
                                Feng.NH.TypedEntityMetadata entityInfo = Feng.NH.TypedEntityMetadata.GenerateEntityInfo(rep.Session.SessionFactory, propertyType);
                                gridColumnInfo.DataControlType = "MyObjectTextBox";
                                gridColumnInfo.SearchControlType = "MyTextBox";
                                gridColumnInfo.SearchControlFullPropertyName = pInfo.Name + ":" + entityInfo.IdName;
                                gridColumnInfo.CellEditorManager = "ObjectTextBox";
                                gridColumnInfo.CellViewerManager = "Object";
                                gridColumnInfo.CellViewerManagerParam = "%" + entityInfo.IdName + "%";
                                gridColumnInfo.CellEditorManagerParam = null;
                            }
                            rep.Save(gridColumnInfo);

                            idx++;
                        }
                    }
                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }

            CreateSettings4WSType(type, gridName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="gridName"></param>
        public static void CreateSettings(System.Data.DataTable table, string gridName)
        {
            using (var rep = GenerateRepository())
            {
                try
                {
                    rep.BeginTransaction();

                    // Window
                    WindowInfo windowInfo = new WindowInfo { Access = "I:*", Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = gridName, Text = gridName, WindowType = WindowType.Maintain, GenerateDetailForm = true };
                    rep.Save(windowInfo);

                    // Action
                    ActionInfo actionInfo = new ActionInfo { ID = gridName, Access = "I:*", ActionType = ActionType.Window, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, Window = windowInfo };
                    rep.Save(actionInfo);

                    // Menu
                    MenuInfo topMenu = rep.Get<MenuInfo>("System");
                    MenuInfo menuInfo = new MenuInfo { Action = actionInfo, Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, ID = gridName, SeqNo = 0, Text = gridName, ParentMenu = topMenu, Visible = "I:*" };
                    rep.Save(menuInfo);

                    // Tab
                    WindowTabInfo tabInfo = new WindowTabInfo
                    {
                        BusinessLayerClassName = "Feng.Data.DataTableDao, Feng.Dao",
                        ControlManagerClassName = "Feng.Windows.Forms.ControlManager, Feng.Windows.Controller",
                        SearchManagerClassName = "Feng.Data.SearchManager, Feng.Windows.Controller",
                        Created = System.DateTime.Now,
                        CreatedBy = systemName,
                        IsActive = true,
                        GridName = gridName,
                        IsInDetailForm = true,
                        IsInGrid = true,
                        ID = gridName,
                        SeqNo = 0,
                        Text = gridName,
                        Window = windowInfo
                    };

                    rep.Save(tabInfo);

                    // grid
                    //GridInfo gridInfo = new GridInfo { AllowDelete = "I:*", AllowEdit = "I:*", AllowInnerDelete = "I:*", AllowInnerEdit = "I:*", AllowInnerInsert = "I:*", AllowInsert  = "I:*", AllowOperationDelete = "I:*", AllowOperationEdit  = "I:*", AllowOperationInsert = "I:*", , Created = System.DateTime.Now, CreatedBy = systemName, IsActive = true, Name = gridName, Visible = "I:*"};

                    int idx = 1;
                    foreach (System.Data.DataColumn pInfo in table.Columns)
                    {
                        bool createGridColumn = true;

                        if (createGridColumn)
                        {
                            int newIdx = idx;
                            string newPropertyName = pInfo.ColumnName;
                            if (pInfo.ColumnName == s_idName)
                            {
                                newIdx = 0;
                                newPropertyName = "ID";
                            }
                            Type propertyType = pInfo.DataType;
                            if (pInfo.DataType.IsGenericType && pInfo.DataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                propertyType = pInfo.DataType.GetGenericArguments()[0];
                            }
                            string propertyTypeName = pInfo.ColumnName;

                            GridColumnInfo gridColumnInfo = new GridColumnInfo
                            {
                                ID = gridName + "_" + pInfo.ColumnName,
                                AllowSetList = false,
                                Caption = pInfo.ColumnName,
                                ColumnVisible = "I:*",
                                Created = System.DateTime.Now,
                                CreatedBy = systemName,
                                IsActive = true,
                                DataControlVisible = "I:*",
                                SearchControlVisible = "I:*",
                                GridName = gridName,
                                GridColumnType = GridColumnType.Normal,
                                PropertyName = newPropertyName,
                                ReadOnly = "False",
                                NotNull = "False",
                                SeqNo = newIdx,
                                TypeName = propertyTypeName,
                                DataControlType = GetDefaultDataControlType(propertyType),
                                SearchControlType = GetDefaultSearchControlType(propertyType),
                                CellEditorManager = GetDefaultGridEditor(propertyType),
                                CellViewerManager = GetDefaultGridViewer(propertyType)
                            };

                            gridColumnInfo.ID = gridColumnInfo.Name.Substring(0, Math.Min(50, gridColumnInfo.ID.Length));

                            rep.Save(gridColumnInfo);

                            idx++;
                        }
                    }
                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
        }

        private static string GetDefaultSearchControlType(Type type)
        {
            if (type == typeof(int))
                return "MyIntegerTextBox";
            else if (type == typeof(long))
                return "MyLongTextBox";
            else if (type == typeof(double))
                return "MyNumericTextBox";
            else if (type == typeof(decimal))
                return "MyCurrencyTextBox";
            else if (type == typeof(DateTime))
                return "MyDatePicker";
            else if (type == typeof(bool))
                return "MyThreeStateCheckbox";
            else if (type.IsEnum)
                return "MyOptionPicker";
            else
                return "MyMultilineTextBox";
        }

        private static string GetDefaultDataControlType(Type type)
        {
            if (type == typeof(int))
                return "MyIntegerTextBox";
            else if (type == typeof(long))
                return "MyLongTextBox";
            else if (type == typeof(double))
                return "MyNumericTextBox";
            else if (type == typeof(decimal))
                return "MyCurrencyTextBox";
            else if (type == typeof(DateTime))
                return "MyDatePicker";
            else if (type == typeof(bool))
                return "MyCheckBox";
            else if (type.IsEnum)
                return "MyComboBox";
            else
                return "MyMultilineTextBox";
        }

        private static string GetDefaultGridViewer(Type type)
        {
            if (type == typeof(decimal) || type == typeof(double))
                return "Numeric";
            else if (type == typeof(DateTime))
                return "DateTime";
            else if (type == typeof(string))
                return "MultiLine";
            else
                return null;
        }

        private static string GetDefaultGridEditor(Type type)
        {
            if (type == typeof(decimal) || type == typeof(double))
                return "Numeric";
            else if (type == typeof(DateTime))
                return "Date";
            else if (type == typeof(string))
                return "MultiLine";
            else if (type.IsEnum)
                return "Combo";
            else
                return null;
        }
        #endregion

        public static void ReplaceFiles(string directoryPath, string pattern, string toFind, string toReplace)
        {
            foreach (string file in System.IO.Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories))
            {
                string s;
                using (StreamReader sr = new StreamReader(file))
                {
                    s = sr.ReadToEnd();
                }
                s = s.Replace(toFind, toReplace);
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine(s);
                }
            }
        }
    }
}
