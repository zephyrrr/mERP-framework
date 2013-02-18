using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Feng.Utils
{
    public static class SecutiryHelper
    {
        /// <summary>
        /// 加密保存连接字符串
        /// </summary>
        public static void SaveConnectionStrings(string connectionStringFileName, string encryptedFileName)
        {
            AMS.Profile.IProfile profile = new AMS.Profile.Xml(encryptedFileName);

            string sKey = Cryptographer.GeneratePassword();
            profile.SetValue("ConnectionStringSetting", "Key", sKey);

            string originalTxt = System.IO.File.ReadAllText(connectionStringFileName, Encoding.UTF8);
            string encryptedText = Cryptographer.EncryptSymmetric(originalTxt, sKey);

            profile.SetValue("ConnectionStringSetting", "ConnectionString", encryptedText);
        }

        /// <summary>
        /// 从加密文件中读取连接字符串
        /// </summary>
        public static void LoadConnectionStrings()
        {
            // <connectionStrings configSource="connections.config"></connectionStrings>

            AMS.Profile.IProfile profile = new AMS.Profile.Xml(IOHelper.GetDataDirectory() + "\\Dbs.dat");
            string skey = profile.GetValue("ConnectionStringSetting", "Key", string.Empty);
            
            if (!string.IsNullOrEmpty(skey))
            {
                string cryptedConnectionStrings = profile.GetValue("ConnectionStringSetting", "ConnectionString", string.Empty);
                if (!string.IsNullOrEmpty(cryptedConnectionStrings))
                {
                    string connectionStrings = Cryptographer.DecryptSymmetric(cryptedConnectionStrings, skey);
                    if (!string.IsNullOrEmpty(connectionStrings))
                    {
                        string tempConfigFileName = IOHelper.GetDataDirectory() + "\\temp.config";
                        System.IO.File.WriteAllText(tempConfigFileName, connectionStrings, Encoding.UTF8);

                        ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = tempConfigFileName };
                        Configuration externalConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                        foreach (ConnectionStringSettings i in externalConfig.ConnectionStrings.ConnectionStrings)
                        {
                            SystemHelper.ChangeConnectionString(i.Name, i.ConnectionString, i.ProviderName);
                        }
                        System.IO.File.Delete(tempConfigFileName);
                    }
                }
            }
        }
    }
}
