using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 程序目录
    /// </summary>
    public interface IApplicationDirectory
    {
        /// <summary>
        /// 程序主目录
        /// </summary>
        /// <returns></returns>
        string GetMainDirectory();

        /// <summary>
        /// 本地资源路径
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        string GetLocalResourcePath(string resourceName);

        /// <summary>
        /// 服务器下载资源路径
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        string GetServerResourcePath(string resourceName);

        /// <summary>
        /// 得到文件完整路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetFullPath(string fileName);
    }
}
