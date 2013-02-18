using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Feng.Utils;

namespace Feng
{
    ///// <summary>
    ///// 程序类型
    ///// </summary>
    //public enum ApplicationType
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    None = 0,
    //    /// <summary>
    //    /// WinForm
    //    /// </summary>
    //    WinForm = 1,
    //    /// <summary>
    //    /// WPF
    //    /// </summary>
    //    WPF = 2,
    //    /// <summary>
    //    /// Web
    //    /// </summary>
    //    Web = 3
    //}

    /// <summary>
    /// 系统全局变量
    /// 关于目录配置：
    /// Layout: 
    /// 默认配置：
    /// SearchControlContainer: 
    /// "SearchControlContainer." + "." + m_sm.ManagerName + ".Layout";
    /// in SystemConfiguration.DefaultUserProfile
    /// Grid: 
    /// "MyGrid." + (form != null ? form.Name : "") + "." + grid.GridName + ".Layout" 
    /// "MyGrid." + (form != null ? form.Name : "") + "." + grid.GridName + "." + level.ToString() + "." + detailGrid.GridName + ".Layout"
    /// in SystemConfiguration.DefaultUserProfile
    /// 
    /// MaxResult: "SearchManager." + m_sm.Name
    /// 
    /// 自定义配置：
    /// SearchControlContainer:  (system.xmls.default)
    /// SystemConfiguration.UserDataDirectory + m_sm.DisplayManager.Name + "\\" + m_sm.Name + "\\" + "*.xmls"
    /// SystemConfiguration.GlobalDataDirectory + ...
    /// Grid:  (system.xmlg.default)
    /// SystemConfiguration.UserDataDirectory + (this.FindForm().Name + "\\") + (this.GridName + "\\") + "*.xmlg"
    /// SystemConfiguration.GlobalDataDirectory + ...
    /// GridReport：
    /// SystemConfiguration.UserDataDirectory + (this.FindForm().Name + "\\") + (this.GridName + "\\") + "*.xml"
    /// SystemConfiguration.GlobalDataDirectory + (this.FindForm().Name + "\\") + (this.GridName + "\\") + "*.xml"
    /// SystemConfiguration.GlobalDataDirectory
    /// ArchiveDetailForm: (system.xmlc.default)
    /// 
    /// PositionPersistForm:
    /// DefaultUserProfile(Todo: SystemConfiguration.UserDataDirectory + (this.Name + "\\") + "PositionPersist.xml")
    /// 
    /// ILayoutControl: 读取优先级：先User后Global
    /// </summary>
    public static class SystemConfiguration
    {
        /// <summary>
        /// 调试模式
        /// </summary>
        public static bool IsInDebug = false;

        /// <summary>
        /// 采用多线程
        /// </summary>
        public static bool UseMultiThread = true;

        /// <summary>
        /// GlobalUserName
        /// </summary>
        public const string GlobalUserName = "Global";

        ///// <summary>
        ///// 程序类型
        ///// </summary>
        //public static ApplicationType ApplicationType
        //{
        //    get;
        //    set;
        //}

        private static string s_applicationName;

        /// <summary>
        /// 程序名，用于验证等
        /// </summary>
        public static string ApplicationName
        {
            get { return GetAppName(); }
            set { s_applicationName = value; }
        }

        /// <summary>
        /// GetAppName
        /// </summary>
        /// <returns></returns>
        private static string GetAppName()
        {
            if (!string.IsNullOrEmpty(s_applicationName))
            {
                return s_applicationName;
            }
            Assembly clientAssembly = Assembly.GetExecutingAssembly();
            if (clientAssembly == null)
            {
                return null;
            }
            AssemblyName assemblyName = clientAssembly.GetName();
            s_applicationName = assemblyName.Name;
            return s_applicationName;
        }

        private static string _name;
        /// <summary>
        /// CurrentUserName
        /// </summary>
        public static string UserName
        {
            get
            {
                // 当多线程的时候，不能这么用
                //string name = System.Threading.Thread.CurrentPrincipal == null
                //                  ? "anonymous"
                //                  : System.Threading.Thread.CurrentPrincipal.Identity.Name;
                //return name;
                return string.IsNullOrEmpty(_name) ? "anonymous" : _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Role
        /// </summary>
        public static string[] Roles { get; set; }

        //private static string[] s_emptyRoles = new string[0];
        ///// <summary>
        ///// Role
        ///// </summary>
        //public static string[] Roles
        //{
        //    get
        //    {
        //        System.Security.Principal.GenericPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.GenericPrincipal;
        //        string[] roles = p == null ? s_emptyRoles : 
        //        return name;
        //    }
        //}

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password;

        /// <summary>
        /// Current User's Client Id
        /// </summary>
        public static int ClientId;

        /// <summary>
        /// Current User's Organization Id
        /// </summary>
        public static int OrgId;

        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string Server;

        ///// <summary>
        ///// 服务器端口
        ///// </summary>
        //public static int ServerPort { get; set; }

        ///// <summary>
        ///// 产品名
        ///// </summary>
        //public static string ProductName
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// 公司名
        ///// </summary>
        //public static string CompanyName
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        /////  版本
        ///// </summary>
        //public static string Version
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 
        /// </summary>
        public const string AdministratorsRoleName = "系统管理员";
        
        /// <summary>
        /// 
        /// </summary>
        public const string DeveloperRoleName = "系统开发人员";

        /// <summary>
        /// 精简模式（可配置性弱，少内存，速度快）
        /// </summary>
        public static bool LiteMode = true;

        ///// <summary>
        ///// 系统模式(默认为Hd（Cd），可以为Zkzx)
        ///// </summary>
        //public static string ApplicationMode
        //{
        //    get;
        //    set;
        //}
    }
}