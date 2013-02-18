using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public enum DataDirectoryType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 全局
        /// </summary>
        Global = 1,
        /// <summary>
        /// 用户
        /// </summary>
        User = 2,
    }

    /// <summary>
    /// 系统目录
    /// </summary>
    public static class SystemDirectory
    {
        /// <summary>
        /// 清空
        /// </summary>
        public static void Clear()
        {
            s_globalDataDirectory = null;
            s_userDataDirectory = null;
        }

        /// <summary>
        /// 获得目录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appendix"></param>
        /// <returns></returns>
        public static string GetDirectory(DataDirectoryType type, string appendix)
        {
            switch (type)
            {
                case DataDirectoryType.Global:
                    return GlobalDataDirectory + "\\" + (appendix == null ? string.Empty : appendix);
                case DataDirectoryType.User:
                    return UserDataDirectory + "\\" + (appendix == null ? string.Empty : appendix);
                default:
                    throw new ArgumentException("Invalid DataDirectory Type1");
            }
        }

        private static string s_workDirectory;
        /// <summary>
        /// 程序主目录
        /// </summary>
        public static string WorkDirectory
        {
            get 
            {
                if (string.IsNullOrEmpty(s_workDirectory))
                {
                    var s = ServiceProvider.GetService<IApplicationDirectory>();
                    if (s != null)
                    {
                        s_workDirectory = s.GetMainDirectory();
                    }
                }

                return s_workDirectory; 
            }
        }

        private static string s_globalDataDirectory;
        /// <summary>
        /// 全局数据目录
        /// </summary>
        public static string GlobalDataDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(s_globalDataDirectory))
                {
                    s_globalDataDirectory = DataDirectory + "Global\\";
                    Feng.Utils.IOHelper.TryCreateDirectory(s_globalDataDirectory);
                }
                return s_globalDataDirectory;
            }
        }

        private static string s_dataDirectory;
        /// <summary>
        /// 数据目录
        /// </summary>
        public static string DataDirectory
        {
            get 
            {
                if (string.IsNullOrEmpty(s_dataDirectory))
                {
                    s_dataDirectory = WorkDirectory + "\\Data\\";
                    Feng.Utils.IOHelper.TryCreateDirectory(s_dataDirectory);
                }
                return s_dataDirectory; 
            }
        }

        private static string s_userDataDirectory;
        /// <summary>
        /// 用户数据目录
        /// </summary>
        public static string UserDataDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(s_userDataDirectory))
                {
                    s_userDataDirectory = DataDirectory + SystemConfiguration.UserName + "\\";
                    Feng.Utils.IOHelper.TryCreateDirectory(s_userDataDirectory);
                }
                return s_userDataDirectory;
            }
        }

       

        /// <summary>
        /// 配置文件名（每用户一个）
        /// </summary>
        public static string UserConfigFileName
        {
            get
            {
                return UserDataDirectory + SystemConfiguration.ApplicationName + "." + SystemConfiguration.UserName + ".config";
            }
        }

        /// <summary>
        /// 配置文件名（全局）
        /// </summary>
        public static string GlobalConfigFileName
        {
            get
            {
                return DataDirectory + SystemConfiguration.ApplicationName + ".config";
            }
        }
    }
}
