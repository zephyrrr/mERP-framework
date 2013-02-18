using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractApplicationDirectory : IApplicationDirectory
    {
        /// <summary>
        /// 程序主目录
        /// </summary>
        /// <returns></returns>
        public abstract string GetMainDirectory();

        private const string LocalResourceDir = "LocalResource\\";
        /// <summary>
        /// 本地资源路径
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public string GetLocalResourcePath(string resourceName)
        {
            return GetMainDirectory() + "\\" + LocalResourceDir + resourceName;
        }

        private const string ServerResourceDir = "ServerResource\\";
        /// <summary>
        /// 服务器下载资源路径
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public string GetServerResourcePath(string resourceName)
        {
            return GetMainDirectory() + "\\" + ServerResourceDir + resourceName;
        }

        /// <summary>
        /// 如果文件名不包含路径，则附加上程序运行路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFullPath(string fileName)
        {
            if (fileName.Contains("\\"))
            {
                return fileName;
            }
            else
            {
                return GetMainDirectory() + "\\" + fileName;
            }
        }
    }
}
