using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Feng.Windows.Utils;

namespace Feng.Windows
{
    public class WindowsDirectory : AbstractApplicationDirectory
    {
        /// <summary>
        /// GetDataDirectory
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public override string GetMainDirectory()
        {
            if (SystemConfiguration.LiteMode)
            {
                return GetExeAppDataPath();
            }
            else
            {
                return GetNetworkDeployedUserAppDataPath();
            }
        }

        private string GetNetworkDeployedUserAppDataPath()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)  // will throw InvalidDeploymentException
            {
                return System.Windows.Forms.Application.UserAppDataPath;
            }
            else
            {
                return GetExeAppDataPath();
            }
        }

        private string GetExeAppDataPath()
        {
            //Web编程
            //# HttpContext.Current.Server.MapPath("FileName")
            //# System.Web.HttpContext.Current.Request.Path
            //#
            //# //Windows编程
            //# System.Environment.CurrentDirectory
            //#
            //# //Mobile编程
            //# Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase); 
            //                 还有一个是用来处理在asp.net中调用dll文件,而DLL文件如果想知道当前的web站点的工作目录可以用
            //System.AppDomain.CurrentDomain.BaseDirectory 

            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);   // remove file://
            //_ExeDir = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
            //_PathStr = _ExeDir;
            //if (!Directory.Exists(_PathStr))
            //{
            //    _PathStr = _ExeDir + "..\\..\\..\\";
            //    if (!Directory.Exists(_PathStr))
            //    {
            //        throw new DirectoryNotFoundException("Cannot find directory " + _PathStr + ". Fatal error.");
            //    }
            //}
            return path;
        }
    }

    public static class SystemProfileFile
    {
        public static void Clear()
        {
            s_globalProfile = null;
            s_userProfile = null;
        }

        private static AMS.Profile.Xml s_userProfile;
        /// <summary>
        /// DefaultUserProfile
        /// </summary>
        public static AMS.Profile.IProfile DefaultUserProfile
        {
            get 
            {
                if (s_userProfile == null)
                {
                    string fileName = SystemDirectory.UserConfigFileName;
                    s_userProfile = new AMS.Profile.Xml(SystemDirectory.UserConfigFileName);
                }
                return s_userProfile; 
            }
        }

        private static AMS.Profile.Xml s_globalProfile;
        /// <summary>
        /// DefaultGlobalProfile
        /// </summary>
        public static AMS.Profile.IProfile DefaultGlobalProfile
        {
            get 
            {
                if (s_globalProfile == null)
                {
                    s_globalProfile = new AMS.Profile.Xml(SystemDirectory.GlobalConfigFileName);
                }
                return s_globalProfile; 
            }
        }
    }
}
