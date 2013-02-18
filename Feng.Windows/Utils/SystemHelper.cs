using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Management;
namespace Feng.Windows.Utils
{
    /// <summary>
    /// 系统Helper
    /// </summary>
    public static class SystemHelper
    {
	   /* 32bit OS 	64bit OS
        32bit CPU 	AddressWidth = 32 	N/A
        64bit CPU 	AddressWidth = 32 	AddressWidth = 64*/
        public static bool IsWindows64()
        {
            string addressWidth = String.Empty;
            ConnectionOptions mConnOption = new ConnectionOptions();
            ManagementScope mMs = new ManagementScope("\\\\localhost", mConnOption);
            ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
            ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
            ManagementObjectCollection mObjectCollection = mSearcher.Get();
            foreach (ManagementObject mObject in mObjectCollection)
            {
                addressWidth = mObject["AddressWidth"].ToString();
            }
            return addressWidth == "64" || string.IsNullOrEmpty(addressWidth);
        }
        ///// <summary>
        ///// 获得ConnectionStringSettingsCollection
        ///// </summary>
        ///// <returns></returns>
        //public static ConnectionStringSettingsCollection ConnectionStringSettings
        //{
        //    get
        //    {
        //        Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //        return _config.ConnectionStrings.ConnectionStrings;
        //    }
        //}
    }
}
