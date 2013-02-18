using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Forms;

namespace Feng.Windows.Utils
{
    public class UserActions : Singleton<UserActions>
    {
        public IList<MenuInfo> TopMenuInfos
        {
            get { return m_topMenuInfos; }
        }
        private IList<MenuInfo> m_topMenuInfos;

        public void LoadInitializeData()
        {
            m_topMenuInfos = ADInfoBll.Instance.GetTopMenuInfos();
        }
        public void LoginUser()
        {
            System.Drawing.Image imageSplash = ImageResource.TryGet("Images.Splash.png");
            SplashScreen.Instance.SetBackgroundImage(imageSplash);
            //Version assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;

            SplashScreen.Instance.SetTitleString(ProductInfo.Instance.Name
                + "  v" + ProductInfo.Instance.CurrentVersion.ToString()
                + "   (C)" + System.DateTime.Today.Year + " "
                + ProductInfo.Instance.CompanyName);

            SplashScreen.Instance.BeginDisplay();
            SplashScreen.Instance.SetCommentaryString("正在初始化程序，请稍候......");

            // Todo: Load at first
            //Feng.NH.Repository.InitializeDefaultSessionFactory();

            SplashScreen.Instance.SetCommentaryString("正在下载用户配置......");
            OnUserLogin();

            SplashScreen.Instance.SetCommentaryString("正在载入初始数据......");
            LoadInitializeData();

            SplashScreen.Instance.SetCommentaryString("正在载入插件......");
            UnloadPlugins();
            LoadPlugins();

            SplashScreen.Instance.EndDisplay();
        }

        public void LogoutUser()
        {
            OnUserLogout();

            UnloadPlugins();
        }
        #region "Plugin"
        ///// <summary>
        ///// 
        ///// </summary>
        //public event EventHandler PluginUnloaded;

        private void UnloadPlugins()
        {
            //if (PluginUnloaded != null)
            //{
            //    PluginUnloaded(this, System.EventArgs.Empty);
            //}
            foreach (IPlugin p in m_plugins)
            {
                p.OnUnload();
            }
            m_plugins.Clear();
        }

        private List<IPlugin> m_plugins = new List<IPlugin>();
        private void LoadPlugins()
        {
            IList<PluginInfo> list = ADInfoBll.Instance.GetInfos<PluginInfo>();
            foreach (PluginInfo i in list)
            {
                if (Authority.AuthorizeByRule(i.Permission))
                {
                    try
                    {
                        object ret = ProcessInfoHelper.ExecuteProcess(i.Process.Name, new Dictionary<string, object> { { "mdiForm", this } });
                        IPlugin p = ret as IPlugin;
                        if (p == null)
                        {
                            throw new ArgumentException(string.Format("插件\"{0}\"创建错误", i.Name), new ArgumentException("Plugin should return an instance of IPlugin!"));
                        }
                        p.OnLoad();

                        m_plugins.Add(p);
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithNotify(ex);
                    }
                }
            }
        }
        #endregion

        #region "User"
        private void OnUserLogin()
        {
            DateTime? d = UserConfigurationHelper.DownloadConfiguration(SystemConfiguration.GlobalUserName, SystemDirectory.GlobalDataDirectory);
            //Feng.Utils.UserConfigurationHelper.DownloadConfiguration(SystemConfiguration.UserName, SystemDirectory.UserDataDirectory);

            var ass = System.Reflection.Assembly.GetCallingAssembly();
            if (ass.Location == null)
                return;

            var assInfo = new System.IO.FileInfo(ass.Location);

            IPersistentCache c = ServiceProvider.GetService<IPersistentCache>();
            if (c != null && c.CacheTime.HasValue)
            {
                //c.Unpersistent();

                if ((!d.HasValue || d.Value < c.CacheTime.Value) && assInfo.LastWriteTime < c.CacheTime.Value)
                {
                    c.Unpersistent();
                }
                else
                {
                    c.Destroy();
                }
            }

            // Spring.net 默认是Singleton的
            IDataBuffers db = ServiceProvider.GetService<IDataBuffers>();
            if (db != null)
            {
                db.LoadData();
            }

            if (!m_haveBindCommands)
            {
                CommnadBindingHelper.BindingCommands();
                m_haveBindCommands = true;
            }
        }

        private bool m_haveBindCommands;

        private void OnUserLogout()
        {
            IPersistentCache c = ServiceProvider.GetService<IPersistentCache>();
            if (c != null)
            {
                c.Persistent();
            }
            IDataBuffers db = ServiceProvider.GetService<IDataBuffers>();
            if (db != null)
            {
                db.Clear();
            }

            //UserConfigurationHelper.UploadConfiguration(SystemConfiguration.UserName, SystemDirectory.UserDataDirectory);
        }
        #endregion
    }
}
