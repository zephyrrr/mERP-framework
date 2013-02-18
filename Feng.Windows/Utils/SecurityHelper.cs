using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Feng.Windows.Utils
{
    public static class SecurityHelper
    {
        public const string DataConnectionStringName = "DataConnectionString";

        private const string s_defaultServer = "nbzsServer";
        private static List<string> s_changetoAutomaticNames = new List<string>();

        private static string GetAutomaticServer()
        {
            string[] defaultChooseServers = new string[] { "192.168.0.10", "17haha8.gicp.net", "17haha8.oicp.net", "17haha8.vicp.net" };
            int port = 8033;
            string selectedServer = null;
            for (int i = 0; i < defaultChooseServers.Length; ++i)
            {
                if (NetHelper.CheckPortOpen(defaultChooseServers[i], port))
                {
                    selectedServer = defaultChooseServers[i];
                    break;
                }
            }
            if (string.IsNullOrEmpty(selectedServer))
                return null;

            return selectedServer;
        }

        /// <summary>
        /// 把nbzsServer设置到自动选择的服务器地址上
        /// </summary>
        public static void SelectAutomaticServer()
        {
            s_changetoAutomaticNames.Clear();

            try
            {
                string selectedServer = null;
                foreach (ConnectionStringSettings i in ConfigurationManager.ConnectionStrings)
                {
                    try
                    {
                        System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                        builder.ConnectionString = i.ConnectionString;

                        if (builder.DataSource == s_defaultServer)
                        {
                            if (string.IsNullOrEmpty(selectedServer))
                            {
                                selectedServer = GetAutomaticServer();
                                if (string.IsNullOrEmpty(selectedServer))
                                    return;
                            }
                            builder.DataSource = selectedServer + ", 8033";

                            ChangeConnectionString(i.Name, builder.ConnectionString, i.ProviderName);
                            s_changetoAutomaticNames.Add(i.Name);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        /// <summary>
        /// 恢复自动选择的服务器地址
        /// </summary>
        public static void DeselectAutomaticServer()
        {
            if (s_changetoAutomaticNames.Count == 0)
                return;

            try
            {
                foreach (ConnectionStringSettings i in ConfigurationManager.ConnectionStrings)
                {
                    try
                    {
                        if (s_changetoAutomaticNames.Contains(i.Name))
                        {
                            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                            builder.ConnectionString = i.ConnectionString;

                            builder.DataSource = s_defaultServer;

                            ChangeConnectionString(i.Name, builder.ConnectionString, i.ProviderName);
                            s_changetoAutomaticNames.Remove(i.Name);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
        }

        /// <summary>
        /// 加密保存连接字符串
        /// </summary>
        public static void SaveConnectionStrings(string connectionStringFileName, string encryptedFileName)
        {
            AMS.Profile.IProfile profile = new AMS.Profile.Xml(encryptedFileName);

            string sKey = Cryptographer.GenerateKey();
            profile.SetValue("ConnectionStringSetting", "Key", sKey);

            string originalTxt = System.IO.File.ReadAllText(connectionStringFileName, Encoding.UTF8);
            string encryptedText = Cryptographer.EncryptSymmetric(originalTxt, sKey);

            profile.SetValue("ConnectionStringSetting", "ConnectionString", encryptedText);
        }

        private static void RemoveEncryptedData()
        {
            string fileName = System.Reflection.Assembly.GetEntryAssembly().Location + ".config";
            XmlDocument docXML = new XmlDocument();
            docXML.Load(fileName);
            RemoveEncryptedDataInNode(docXML);
            docXML.Save(fileName);
        }

        private static void RemoveEncryptedDataInNode(XmlNode parentNode)
        {
            IList<XmlNode> childNodes = new List<XmlNode>();
            foreach (XmlNode i in parentNode.ChildNodes)
            {
                childNodes.Add(i);
            }
            foreach (XmlNode i in childNodes)
            {
                if (i.Attributes != null && i.Attributes["configProtectionProvider"] != null)
                {
                    parentNode.RemoveChild(i);
                }
                else
                {
                    RemoveEncryptedDataInNode(i);
                }
            }
        }
        
        /// <summary>
        /// 从加密文件中读取连接字符串
        /// </summary>
        public static bool LoadConnectionStrings()
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings[DataConnectionStringName] != null)
                    return false;

                // <connectionStrings configSource="connections.config"></connectionStrings>

                AMS.Profile.IProfile profile = new AMS.Profile.Xml(SystemDirectory.DataDirectory + "\\Dbs.dat");
                string skey = profile.GetValue("ConnectionStringSetting", "Key", string.Empty);

                if (!string.IsNullOrEmpty(skey))
                {
                    string cryptedConnectionStrings = profile.GetValue("ConnectionStringSetting", "ConnectionString", string.Empty);
                    if (!string.IsNullOrEmpty(cryptedConnectionStrings))
                    {
                        string connectionStrings = Cryptographer.DecryptSymmetric(cryptedConnectionStrings, skey);
                        if (!string.IsNullOrEmpty(connectionStrings))
                        {
                            string tempConfigFileName = SystemDirectory.DataDirectory + "\\temp.config";
                            System.IO.File.WriteAllText(tempConfigFileName, connectionStrings, Encoding.UTF8);

                            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = tempConfigFileName };
                            Configuration externalConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                            Configuration exeConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            foreach (ConnectionStringSettings i in externalConfig.ConnectionStrings.ConnectionStrings)
                            {
                                if (exeConfig.ConnectionStrings.ConnectionStrings[i.Name] == null)
                                {
                                    exeConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(i.Name, i.ConnectionString, i.ProviderName));
                                }
                                else
                                {
                                    //the full name of the connection string can be found in the app.config file
                                    // in the "name" attribute of the connection string
                                    exeConfig.ConnectionStrings.ConnectionStrings[i.Name].ConnectionString = i.ConnectionString;
                                    exeConfig.ConnectionStrings.ConnectionStrings[i.Name].ProviderName = i.ProviderName;
                                }
                            }
                            if (!exeConfig.ConnectionStrings.SectionInformation.IsProtected)
                            {
                                //exeConfig.ConnectionStrings.SectionInformation.UnprotectSection();
                                // Encrypt the section.
                                exeConfig.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                            }

                            exeConfig.Save(ConfigurationSaveMode.Full);
                            ConfigurationManager.RefreshSection(exeConfig.ConnectionStrings.SectionInformation.Name);

                            // re-init ConfigurationManager
                            var constructorInfo = typeof(System.Configuration.ConfigurationManager).GetConstructor(
                                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new Type[0], null);
                            constructorInfo.Invoke(null, null);

                            System.IO.File.Delete(tempConfigFileName);
                        }
                    }
                }
                return true;
            }
            catch (System.Configuration.ConfigurationErrorsException)
            {
                RemoveEncryptedData();
                throw;
            }
        }

        /// <summary>
        /// 改变app.config中的ConnectionString.如果不存在则新增
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        public static void ChangeConnectionString(string name, string connectionString, string providerName)
        {
            Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (_config.ConnectionStrings.ConnectionStrings[name] == null)
            {
                _config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(name, connectionString, providerName));
            }
            else
            {
                //the full name of the connection string can be found in the app.config file
                // in the "name" attribute of the connection string
                _config.ConnectionStrings.ConnectionStrings[name].ConnectionString = connectionString;
                _config.ConnectionStrings.ConnectionStrings[name].ProviderName = providerName;
            }

            //Save to file
            _config.Save(ConfigurationSaveMode.Full);

            //force changes to take effect so that we can start using
            //this new connection string immediately
            ConfigurationManager.RefreshSection(_config.ConnectionStrings.SectionInformation.Name);
        }

        /// <summary>
        /// RemoveConnectionStrings
        /// </summary>
        public static void RemoveConnectionStrings()
        {
            Configuration exeConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (exeConfig.ConnectionStrings.SectionInformation.IsProtected)
            {
                exeConfig.ConnectionStrings.SectionInformation.UnprotectSection();
            }
            exeConfig.ConnectionStrings.ConnectionStrings.Clear();
            exeConfig.Save(ConfigurationSaveMode.Full);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fileName"></param>
        //public static void RemoveConfigConnectionstring(string fileName)
        //{
        //    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(fileName);

        //    config.ConnectionStrings.LockItem = false;
        //    foreach (ConnectionStringSettings cs in config.ConnectionStrings.ConnectionStrings)
        //    {
        //        cs.ConnectionString = string.Empty;
        //        cs.LockItem = false;
        //    }
        //    config.Save();
        //}
    }
}
