using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// ConfigurationHelper
    /// </summary>
    public static class UserConfigurationHelper
    {
        public static bool UpdateStartFormofConfiguration(string userName, string startForm)
        {
            var cmd = Feng.Data.DbHelper.Instance.Database.DbProviderFactory.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "UPDATE SD_User_Configuration SET StartForm = @StartForm WHERE UserName = @UserName";
            cmd.Parameters.Add(Feng.Data.DbHelper.Instance.CreateParameter("@StartForm", string.IsNullOrEmpty(startForm) ? DBNull.Value : (object)startForm));
            cmd.Parameters.Add(Feng.Data.DbHelper.Instance.CreateParameter("@UserName", userName));
            try
            {
                Feng.Data.DbHelper.Instance.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithNotify(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// UploadConfiguration
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static DateTime? UploadConfiguration(string userName, byte[] userData)
        {
            UserConfigurationInfo userInfo = ADInfoBll.Instance.GetUserConfigurationInfo(userName);

            // create new Entity because there is a lazy property in entity, which will cause "no persister for proxy" exception
            UserConfigurationInfo newUserInfo = new UserConfigurationInfo();
            if (userInfo == null)
            {
                newUserInfo.UserName = userName;
            }
            else
            {
                newUserInfo.ID = userInfo.ID;
                newUserInfo.StartForm = userInfo.StartForm;
                newUserInfo.Version = userInfo.Version;
                newUserInfo.UserName = userInfo.UserName;
                newUserInfo.Created = userInfo.Created;
                newUserInfo.CreatedBy = userInfo.CreatedBy;
            }

            if (userInfo == null || userInfo.UserDataLength != userData.Length)
            {
                try
                {
                    newUserInfo.UserData = userData;
                    newUserInfo.UserDataLength = userData.Length;
                    ADInfoBll.Instance.SaveOrUpdateUserConfigurationInfo(newUserInfo);
                    return System.DateTime.Now;
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userDirectory"></param>
        public static DateTime? UploadConfiguration(string userName, string userDirectory)
        {
            byte[] s = CompressionHelper.CompressFromFolder(userDirectory);
            UploadConfiguration(userName, s);
            return null;
        }

        private const string m_configurationVersionFile = "version.db";
        /// <summary>
        /// 下载，和本地合并
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userDirectory"></param>
        public static DateTime? DownloadConfiguration(string userName, string userDirectory)
        {
            UserConfigurationInfo userInfo = ADInfoBll.Instance.GetUserConfigurationInfo(userName);
            if (userInfo == null)
                return null;

            bool downloadNew = true;
            string versionFileName = Path.Combine(userDirectory, m_configurationVersionFile);
            if (Directory.Exists(userDirectory))
            {
                //byte[] userData = CompressionHelper.CompressFromFolder(userDirectory);
                //if (userInfo.UserDataLength == userData.Length)
                //{
                //    downloadNew = false;
                //}
                if (File.Exists(versionFileName))
                {
                    using (StreamReader sr = new StreamReader(versionFileName))
                    {
                        string s = sr.ReadLine();
                        int? version = Feng.Utils.ConvertHelper.ToInt(s);
                        if (version.HasValue && version.Value == userInfo.Version)
                        {
                            downloadNew = false;
                        }
                    }
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(userDirectory);
            }

            if (downloadNew)
            {
                try
                {
                    try
                    {
                        Feng.Utils.IOHelper.HardDirectoryDelete(userDirectory);
                    }
                    catch (Exception)
                    {
                    }

                    ADInfoBll.Instance.GetUserConfigurationData(userInfo);
                    CompressionHelper.DecompressToFolder(userInfo.UserData, SystemDirectory.DataDirectory);

                    using (StreamWriter sw = new StreamWriter(versionFileName))
                    {
                        sw.WriteLine(userInfo.Version.ToString());
                    }
                }
                catch (Exception)
                {
                }
            }
            return userInfo.Updated.HasValue ? userInfo.Updated : userInfo.Created;
        }
    }
}
