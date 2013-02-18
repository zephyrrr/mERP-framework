using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xceed.Ftp;
using Xceed.FileSystem;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// FtpHelper
    /// </summary>
    public class FtpHelper : Singleton<FtpHelper>
    {
        private class FtpSiteData
        {
            public string Host
            {
                get;
                set;
            }

            public int Port
            {
                get;
                set;
            }

            public string UserName
            {
                get;
                set;
            }

            public string Password
            {
                get;
                set;
            }
        }

        static FtpHelper()
        {
            XceedLicense.SetXceedLicense("Xceed.Ftp", "Xceed.Ftp.v5.1", "FTN51-BU0HR-KY598-0RKA");
            XceedLicense.SetXceedLicense("Xceed.FileSystem", "Xceed.FileSystem.v5.1", "ZIN51-TR163-A813S-NWTA");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FtpHelper()
        {
            PassiveTransfer = true;
        }

        /// <summary>
        /// PassiveTransfer
        /// </summary>
        public bool PassiveTransfer
        {
            get;
            set;
        }

        /// <summary>
        /// TraceWriter
        /// Can set to  Console.Out;
        /// </summary>
        public System.IO.TextWriter TraceWriter
        {
            get;
            set;
        }

        private void SetConnection(FtpConnection connection)
        {
            connection.PassiveTransfer = PassiveTransfer;
            connection.TraceWriter = TraceWriter;
        }

        private FtpSiteData ParseFtpAddress(string ftpAddress)
        {
            Regex regex = new Regex("ftp://((?<UserName>\\w+)(:(?<Password>\\w+))?@)?(?<HostName>(\\w|\\.)+)(:(?<Port>\\w+))?");
            Match match = regex.Match(ftpAddress);
            if (!match.Success)
                return null;
            else
                return new FtpSiteData
                {
                    Host = match.Result("${HostName}"),
                    Port = string.IsNullOrEmpty(match.Result("${Port}")) ? 21 : Convert.ToInt32(match.Result("${Port}")),
                    UserName = string.IsNullOrEmpty(match.Result("${UserName}")) ? "anonymous" : match.Result("${UserName}"),
                    Password = match.Result("${Password}")
                };
        }

        /// <summary>
        /// 断点续传下载
        /// </summary>
        /// <param name="ftpAddress"></param>
        /// <param name="remoteFilename"></param>
        /// <param name="localFilename"></param>
        /// <returns></returns>
        public void DownloadFile(string ftpAddress, string remoteFilename, string localFilename)
        {
            while (true)
            {
                try
                {
                    Write(string.Format("Begin Download {1} to {0} in {2}", localFilename, remoteFilename, ftpAddress));

                    bool ret = DownloadFile(ftpAddress, remoteFilename, localFilename, true);
                    if (ret)
                        break;
                }
                //catch (Xceed.Ftp.FtpException)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //}
                //catch (Xceed.FileSystem.FileSystemException)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //}
                catch (Exception ex)
                {
                    if (TraceWriter != null)
                    {
                        TraceWriter.WriteLine("Sleep 1s for Exception " + ex.Message);
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }
            Write(string.Format("End Download"));
        }

        private void Write(string s)
        {
            if (TraceWriter != null)
            {
                TraceWriter.WriteLine(s);
            }
        }
        /// <summary>
        /// 断点续传上传
        /// </summary>
        /// <param name="ftpAddress"></param>
        /// <param name="localFilename"></param>
        /// <param name="remoteFilename"></param>
        public void UploadFile(string ftpAddress, string localFilename, string remoteFilename)
        {
            while (true)
            {
                try
                {
                    Write(string.Format("Begin Upload {0} to {1} in {2}", localFilename, remoteFilename, ftpAddress));

                    bool ret = UploadFile(ftpAddress, localFilename, remoteFilename, true);
                    if (ret)
                        break;
                }
                //catch (Xceed.Ftp.FtpException)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //}
                //catch (Xceed.FileSystem.FileSystemException)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //}
                catch (Exception ex)
                {
                    Write("Sleep 1s for Exception " + ex.Message);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            Write(string.Format("End Upload"));
        }

        /// <summary>
        /// 下载文件（可断点续传）
        /// </summary>
        /// <param name="ftpAddress">服务器地址，格式为 userName:Password@hostName:port(ftp://softer:111111@ftp.softer.com:21)</param>
        /// <param name="remoteFilename"></param>
        /// <param name="localFilename"></param>
        /// <param name="resumeOperation"></param>
        public bool DownloadFile(string ftpAddress, string remoteFilename, string localFilename, bool resumeOperation)
        {
            FtpSiteData siteData = ParseFtpAddress(ftpAddress);
            if (siteData == null)
            {
                throw new ArgumentException("Invalid ftp address format!");
            }

            using (FtpConnection connection = new FtpConnection(siteData.Host, siteData.Port, siteData.UserName, siteData.Password))
            {
                SetConnection(connection);

                AbstractFolder remoteFolder = new FtpFolder(connection);
                AbstractFile remoteFile = remoteFolder.GetFile(remoteFilename);
                // 不行，必须要从Folder来
                //AbstractFile remoteFile = new FtpFile(connection, remoteFilename);
                AbstractFile localFile = new DiskFile(localFilename);

                if (!resumeOperation || !localFile.Exists || remoteFile.Size < localFile.Size)
                {
                    remoteFile.CopyTo(localFile, true);
                }
                else if (remoteFile.Size == localFile.Size)
                {
                    return true;
                }
                else if (remoteFile.Size > localFile.Size)
                {
                    byte[] buf = new byte[1024];
                    int cnt = -1;

                    using (System.IO.Stream localStream = localFile.OpenWrite(false))
                    {
                        using (System.IO.Stream remoteStream = remoteFile.OpenRead())
                        {
                            remoteStream.Seek(localFile.Size, System.IO.SeekOrigin.Begin);
                            localStream.Seek(0, System.IO.SeekOrigin.End);

                            do
                            {
                                cnt = remoteStream.Read(buf, 0, buf.Length);
                                localStream.Write(buf, 0, cnt);
                            } while (cnt == buf.Length);
                        }
                    }
                }

                return true;
            }

            

            //FtpClient client = LoginFtp(ftpAddress);
            //if (System.IO.File.Exists(localFilename))
            //{
            //    Xceed.FileSystem.DiskFile file = new Xceed.FileSystem.DiskFile(localFilename);

            //    using (System.IO.Stream localStream = file.OpenWrite(false))
            //    {
            //        client.ReceiveFile(remoteFilename, file.Size, localStream);
            //    }
            //}
            //else
            //{
            //    client.ReceiveFile(remoteFilename, localFilename);
            //}
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="ftpAddress"></param>
        /// <param name="remoteFilename"></param>
        /// <param name="localFilename"></param>
        /// <param name="resumeOperation"></param>
        public bool UploadFile(string ftpAddress, string localFilename, string remoteFilename, bool resumeOperation)
        {
            FtpSiteData siteData = ParseFtpAddress(ftpAddress);
            if (siteData == null)
            {
                throw new ArgumentException("Invalid ftp address format!");
            }

            using (FtpConnection connection = new FtpConnection(siteData.Host, siteData.Port, siteData.UserName, siteData.Password))
            {
                SetConnection(connection);

                AbstractFolder remoteFolder = new FtpFolder(connection);
                AbstractFile remoteFile = remoteFolder.GetFile(remoteFilename);

                AbstractFile localFile = new DiskFile(localFilename);

                if (!resumeOperation || !remoteFile.Exists || remoteFile.Size > localFile.Size)
                {
                    localFile.CopyTo(remoteFile, true);
                }
                else if (remoteFile.Size == localFile.Size)
                {
                    return true;
                }
                else if (remoteFile.Size < localFile.Size)
                {
                    byte[] buf = new byte[1024];
                    int cnt = -1;

                    using (System.IO.Stream remoteStream = remoteFile.OpenWrite(false))
                    {
                        using (System.IO.Stream localStream = localFile.OpenRead())
                        {
                            localStream.Seek(remoteFile.Size, System.IO.SeekOrigin.Begin);
                            // can't seek. OpenWrite如果不overwrite自动append
                            //remoteStream.Seek(0, System.IO.SeekOrigin.End);

                            do
                            {
                                cnt = localStream.Read(buf, 0, buf.Length);
                                remoteStream.Write(buf, 0, cnt);
                            } while (cnt == buf.Length);
                        }
                    }
                }

                return true;
            }

            //FtpClient client = LoginFtp(ftpAddress);
            //client.SendFile(localFilename, remoteFilename);
        }

        //private FtpClient LoginFtp(string ftpAddress)
        //{
        //    FtpSiteData siteData = ParseFtpAddress(ftpAddress);
        //    if (siteData == null)
        //    {
        //        throw new ArgumentException("Invalid ftp address format!");
        //    }
        //    FtpClient client = new FtpClient();

        //    client.Connect(siteData.Host, siteData.Port);
        //    client.Login(siteData.UserName, siteData.Password);

        //    return client;
        //}
    }
}
