using System;
using Microsoft.Win32;

namespace NeokernelService
{
    /// <summary>
    /// Reads and writes registry keys 
    /// </summary>
    public class RegistryRW
    {
        public RegistryRW()
        {
            //
            // TODO: 
            //
        }


        public enum RegistryRootKeys
        {
            HKEY_CLASSES_ROOT,
            HKEY_CURRENT_CONFIG,
            HKEY_CURRENT_USER,
            HKEY_DYN_DATA,
            HKEY_LOCAL_MACHINE,
            HKEY_PERFORMANCE_DATA,
            HKEY_USERS
        }

        private static RegistryKey GetRegistryRootKey(RegistryRootKeys rootKey)
        {
            RegistryKey regKey = null;
            switch (rootKey)
            {
                case RegistryRootKeys.HKEY_CLASSES_ROOT:
                    regKey = Registry.ClassesRoot;
                    break;
                case RegistryRootKeys.HKEY_CURRENT_CONFIG:
                    regKey = Registry.CurrentConfig;
                    break;
                case RegistryRootKeys.HKEY_CURRENT_USER:
                    regKey = Registry.CurrentUser;
                    break;
                case RegistryRootKeys.HKEY_LOCAL_MACHINE:
                    regKey = Registry.LocalMachine;
                    break;
                case RegistryRootKeys.HKEY_PERFORMANCE_DATA:
                    regKey = Registry.PerformanceData;
                    break;
                case RegistryRootKeys.HKEY_USERS:
                    regKey = Registry.Users;
                    break;
            }
            return regKey;
        }

        public static object ReadValue(RegistryRootKeys rootKey, string keyPath,
            string valueName, object defaultValue)
        {
            RegistryKey regKey = GetRegistryRootKey(rootKey).OpenSubKey(@keyPath);
            object regValue = defaultValue;
            if (regKey != null)
            {
                regValue = regKey.GetValue(valueName);
            }
            else
            {
                WriteValue(rootKey, keyPath, valueName, defaultValue, true);
            }
            return regValue;
        }

        public static void WriteValue(RegistryRootKeys rootKey, string keyPath,
            string valueName, object value, bool createIfNotExist)
        {

            RegistryKey regKey = GetRegistryRootKey(rootKey);
            string[] pathToken = keyPath.Split('\\');
            RegistryKey subKey = null;
            for (int i = 0; i < pathToken.Length; i++)
            {
                if (regKey != null)
                {
                    subKey = regKey.OpenSubKey(pathToken[i], true);
                    if (subKey == null && createIfNotExist)
                    {
                        subKey = regKey.CreateSubKey(pathToken[i]);
                    }
                    regKey = subKey;
                }
            }
            if (regKey != null)
            {
                regKey.SetValue(valueName, value);
            }
            else
            {
                throw new ApplicationException("Cannot create " + rootKey.ToString() + "\\" + keyPath);
            }
        }
    }
}
