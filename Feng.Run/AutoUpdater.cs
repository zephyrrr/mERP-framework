/*
 * AutoUpdater.cs
 * This class is the main component of the AutoUpdater
 *  
 * Copyright 2004 Conversive, Inc.
 * 
 */

/*
 * Conversive's C# AutoUpdater Component
 * Copyright 2004 Conversive, Inc.
 * 
 * This is a component which allows automatic software updates.
 * It is written in C# and was developed by Conversive, Inc. on April 14th 2004.
 * 
 * The C# AutoUpdater Component is licensed under the LGPL:
 * ------------------------------------------------------------------------
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 * ------------------------------------------------------------------------
 */

using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Net;

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Feng.Updater
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class AutoUpdater
    {
        public AutoUpdater()
        {
            this.AutoDownload = true;
        }

        #region"Server Property"
        [Category("AutoUpdater Configuration")]
        public string ProxyURL
        {
            get;
            set;
        }

        [DefaultValue(@"")]
        [Description("The UserName to authenticate with."),
        Category("AutoUpdater Configuration")]
        public string LoginUserName
        { 
            get;
            set;
        }

        [DefaultValue(@"")]
        [Description("The Password to authenticate with."),
        Category("AutoUpdater Configuration")]
        public string LoginUserPass
        {
            get;
            set;
        }

        [DefaultValue(@"http://localhost/")]
        [Description("The URL Path to the configuration file."),
        Category("AutoUpdater Configuration")]
        public string ServerPath
        {
            get;
            set;
        }
        #endregion

        #region "Update Interactive Property"
        //If true, the app will automatically download the latest version, if false the app will use the DownloadForm to prompt the user, if AutoDownload is false and DownloadForm is null, it doesn't download
        [DefaultValue(true)]
        [Description("Set to True if you want the app to restart automatically, set to False if you want to use the DownloadForm to prompt the user, if AutoDownload is false and DownloadForm is null, the app will not download the latest version."),
        Category("AutoUpdater Configuration")]
        public bool AutoDownload
        {
            get;
            set;
        }

        public Form DownloadForm
        {
            get;
            set;
        }

        //[DefaultValue(false)]
        //[Description("Set to True if you want the app to restart automatically, set to False if you want to use the RestartForm to prompt the user, if AutoRestart is false and RestartForm is null, the app will not restart."),
        //Category("AutoUpdater Configuration")]
        //public bool AutoRestart
        //{
        //    get;
        //    set;
        //}

        public Form RestartForm
        {
            get;
            set;
        }
        #endregion

        //[BrowsableAttribute(false)]
        //public string LatestConfigChanges
        //{
        //    get
        //    {
        //        string stRet = null;
        //        //Protect against NPE's
        //        if (this._AutoUpdateConfig != null)
        //            stRet = _AutoUpdateConfig.LatestChanges;
        //        return stRet;
        //    }
        //}

        //[BrowsableAttribute(false)]
        //public Version LatestConfigVersion
        //{
        //    get
        //    {
        //        Version versionRet = null;
        //        //Protect against NPE's
        //        if (this._AutoUpdateConfig != null)
        //            versionRet = new Version(this._AutoUpdateConfig.AvailableVersion);
        //        return versionRet;
        //    }
        //}

        //[BrowsableAttribute(false)]
        //public Version CurrentAppVersion
        //{
        //    get
        //    {
        //        return System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
        //    }
        //}

        //[BrowsableAttribute(false)]
        public bool NewVersionAvailable
        { 
            get 
            {
                if (_AutoUpdateConfig != null && !string.IsNullOrEmpty(_AutoUpdateConfig.AvailableVersion))
                {
                    return new Version(_AutoUpdateConfig.AvailableVersion) > Feng.Run.FengProgram.AppConfig.Product.CurrentAppVersion;
                }
                return false;
            }
        }


        #region "Update Event"
        public delegate void ConfigFileDownloaded(bool bNewVersionAvailable);
        public event ConfigFileDownloaded OnConfigFileDownloaded;

        public delegate void AutoUpdateComplete();
        public event AutoUpdateComplete OnAutoUpdateComplete;

        public delegate void AutoUpdateError(string stMessage, Exception e);
        public event AutoUpdateError OnAutoUpdateError;
        #endregion

        private AutoUpdateConfig _AutoUpdateConfig;
        public AutoUpdateConfig AutoUpdateConfig
        { 
            get
            { 
                return _AutoUpdateConfig; 
            }
        }

        /// <summary>
        /// TryUpdate: Invoke this method if you just want to load the config without autoupdating
        /// </summary>
        public void LoadConfig()
        {
            Thread backgroundLoadConfigThread = new Thread(new ThreadStart(this.loadConfigThread));
            backgroundLoadConfigThread.IsBackground = true;
            backgroundLoadConfigThread.Start();
        }//TryUpdate()

        //private static bool ValidateRemoteCertificate(object sender, System.Security.Cryptography.X509Certificate certificate,
        //    System.Security.Cryptography.X509Chain chain, System.Net.ServicePointManager.SslPolicyErrors policyErrors)
        //{
        //    return true;
        //}

        /// <summary>
        /// loadConfig: This method just loads the config file so the app can check the versions manually
        /// </summary>
        private void loadConfigThread()
        {
            AutoUpdateConfig config = new AutoUpdateConfig();
            config.OnLoadConfigError += new AutoUpdateConfig.LoadConfigError(config_OnLoadConfigError);

            ////For using untrusted SSL Certificates
            //System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            //    new System.Net.ServicePointManager.RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            //Do the load of the config file
            if (config.LoadConfig(this.ServerPath + "UpdateVersion.xml", this.LoginUserName, this.LoginUserPass, this.ProxyURL))
            {
                this._AutoUpdateConfig = config;
                if (this.OnConfigFileDownloaded != null)
                {
                    this.OnConfigFileDownloaded(this.NewVersionAvailable);
                }
            }
            //else
            //	MessageBox.Show("Problem loading config file, from: " + this.ConfigURL);
        }

        /// <summary>
        /// UpdateAsync: Invoke this method when you are ready to run the update checking thread
        /// </summary>
        public void UpdateAsync()
        {
            Thread backgroundThread = new Thread(new ThreadStart(this.updateThread));
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        public bool Update()
        {
            string stUpdateName = "update";
            if (this._AutoUpdateConfig == null)//if we haven't already downloaded the config file, do so now
                this.loadConfigThread();
            if (this._AutoUpdateConfig != null)//make sure we were able to download it
            {
                //Check the file for an update
                if (this.NewVersionAvailable)
                {
                    //Download file if the user requests or AutoDownload is True
                    if (this.AutoDownload || (this.DownloadForm != null && this.DownloadForm.ShowDialog() == DialogResult.Yes))
                    {
                        //MessageBox.Show("New Version Available, New Version: " + vConfig.ToString() + "\r\nDownloading File from: " + config.AppFileURL);
                        DirectoryInfo diDest = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
                        string stPath = diDest.FullName + System.IO.Path.DirectorySeparatorChar + stUpdateName + ".zip";
                        //There is a new version available
                        if (this.downloadFile(this.ServerPath + this._AutoUpdateConfig.AppFileName, stPath))
                        {
                            //MessageBox.Show("Downloaded New File");
                            string stDest = diDest.FullName + System.IO.Path.DirectorySeparatorChar + stUpdateName + System.IO.Path.DirectorySeparatorChar;
                            //Extract Zip File
                            this.unzip(stPath, stDest);
                            //Delete Zip File
                            File.Delete(stPath);
                            if (this.OnAutoUpdateComplete != null)
                            {
                                this.OnAutoUpdateComplete();
                            }
                            ////Restart App if Necessary
                            ////If true, the app will restart automatically, if false the app will use the RestartForm to prompt the user, if RestartForm is null, it doesn't restart
                            //if (this.AutoRestart || (this.RestartForm != null && this.RestartForm.ShowDialog() == DialogResult.Yes))
                            //    this.restart();
                            ////else don't restart

                            return true;
                        }
                        //else
                        //	MessageBox.Show("Didn't Download File");
                    }

                }
                //else
                //	MessageBox.Show("No New Version Available, Web Version: " + vConfig.ToString() + ", Current Version: " +  vCurrent.ToString());
            }
            return false;
        }

        /// <summary>
        /// updateThread: This is the Thread that runs for checking updates against the config file
        /// </summary>
        private void updateThread()
        {
            Update();
        }

        /// <summary>
        /// downloadFile: Download a file from the specified url and copy it to the specified path
        /// </summary>
        private bool downloadFile(string url, string path)
        {
            try
            {
                WebResponse Response;
                WebRequest Request;

                Request = HttpWebRequest.Create(url);
                //Request.Headers.Add("Translate: f"); //Commented out 11/16/2004 Matt Palmerlee, this Header is more for DAV and causes a known security issue
                if (this.LoginUserName != null && this.LoginUserName != "")
                    Request.Credentials = new NetworkCredential(this.LoginUserName, this.LoginUserPass);
                else
                    Request.Credentials = CredentialCache.DefaultCredentials;

                //Added 11/16/2004 For Proxy Clients, Thanks George for submitting these changes
                if (!string.IsNullOrEmpty(this.ProxyURL))
                {
                    Request.Proxy = new WebProxy(this.ProxyURL);
                }

                Response = Request.GetResponse();

                Stream respStream = null;
                respStream = Response.GetResponseStream();

                //Do the Download
                byte[] buffer = new byte[4096];
                int length;

                FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write);

                length = respStream.Read(buffer, 0, 4096);
                while (length > 0)
                {
                    fs.Write(buffer, 0, length);
                    length = respStream.Read(buffer, 0, 4096);
                }
                fs.Close();
            }
            catch (Exception e)
            {
                string stMessage = "Problem downloading and copying file from: " + url + " to: " + path;
                //MessageBox.Show(stMessage);
                if (File.Exists(path))
                    File.Delete(path);
                this.sendAutoUpdateError(stMessage, e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// unzip: Open the zip file specified by stZipPath, into the stDestPath Directory
        /// </summary>
        private void unzip(string stZipPath, string stDestPath)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(stZipPath));

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                string fileName = stDestPath + Path.GetDirectoryName(theEntry.Name) + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(theEntry.Name);

                //create directory for file (if necessary)
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                if (!theEntry.IsDirectory)
                {
                    FileStream streamWriter = File.Create(fileName);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    try
                    {
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch { }

                    streamWriter.Close();
                }
            }
            s.Close();
        }//unzip(string stZipPath, string stDestPath)

        /// <summary>
        /// restart: Restart the app, the AppStarter will be responsible for actually restarting the main application.
        /// </summary>
        private void restart()
        {
            Environment.ExitCode = 2; //the surrounding AppStarter must look for this to restart the app.
            Application.Exit();
        }

        private void config_OnLoadConfigError(string stMessage, Exception e)
        {
            this.sendAutoUpdateError(stMessage, e);
        }

        private void sendAutoUpdateError(string stMessage, Exception e)
        {
            if (this.OnAutoUpdateError != null)
                this.OnAutoUpdateError(stMessage, e);
        }
    }
}
