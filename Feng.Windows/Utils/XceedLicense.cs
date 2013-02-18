using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// Utility for Xceed
    /// </summary>
    public static class XceedLicense
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="assemblyName"></param>
        /// <param name="license"></param>
        public static void SetXceedLicense(string typeName, string assemblyName, string license)
        {
            if (System.IO.File.Exists(SystemDirectory.WorkDirectory + "\\" + assemblyName + ".dll"))
            {
                string fullTypeName = typeName + ".Licenser, " + assemblyName;
                Type type = Feng.Utils.ReflectionHelper.GetTypeFromName(fullTypeName);
                if (type != null)
                {
                    type.InvokeMember("LicenseKey", BindingFlags.SetProperty | BindingFlags.Static | BindingFlags.Public,
                             null, null, new object[] { license }, null, null, null);
                }
            }
        }
    }
}