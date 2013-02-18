using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Permissions;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// Helper for 进程
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="argument">参数</param>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static System.Diagnostics.Process ExecuteApplication(string applicationName, string argument)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                return null;
            }

            int index = applicationName.LastIndexOf('\\');

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = index == -1 ? SystemDirectory.WorkDirectory + "\\" + applicationName : applicationName;
            psi.UseShellExecute = true;
            psi.WorkingDirectory = index == -1 ? SystemDirectory.WorkDirectory : applicationName.Substring(0, index);
            psi.Arguments = argument;

            return System.Diagnostics.Process.Start(psi);
        }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="applicationName"></param>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static System.Diagnostics.Process ExecuteApplication(string applicationName)
        {
            return ExecuteApplication(applicationName, string.Empty);
        }

        /// <summary>
        /// 打开网页
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static System.Diagnostics.Process OpenUrl(string address)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = address;
            psi.UseShellExecute = true;

            return System.Diagnostics.Process.Start(psi);
        }

        /// <summary>
        /// 打开网页内容
        /// </summary>
        /// <param name="htmlInfo"></param>
        /// <returns></returns>
        public static System.Diagnostics.Process OpenHtml(string htmlInfo)
        {
            string fileName = Feng.Utils.IOHelper.GetTempPathFileName("网页快照.html");
            using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("gb2312")))
            {
                sw.Write(htmlInfo);
            }
            return OpenUrl("file://" + fileName);
        }

        ///// <summary>
        ///// 禁用Close按钮
        ///// </summary>
        ///// <param name="form"></param>
        //public static void DisableCloseButton(Form form)
        //{
        //    // remove close button
        //    IntPtr hMenu = GetSystemMenu(form.Handle, false);
        //    int menuItemCount = GetMenuItemCount(hMenu);
        //    RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        //}
    }
}
