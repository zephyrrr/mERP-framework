using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Spring.Context;
using Spring.Context.Support;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Feng.Updater;
using Feng.Windows.Utils;

namespace Feng.Run
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //System.Windows.Forms.MessageBox.Show("Start Debug Version!");

            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CN");

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                InitConfigurations();

                if (args.Length == 0)
                {

                    // 程序运行的时候，不更新. 自身也是
                    if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location)).Length > 1)
                    {
                        Run(args);
                    }
                    else
                    {
                        TryCopyNotUpdated();

                        if (!TryUpdate())
                        {
                            // 如果直接放在这里，会导致函数开始时就load spring Assembly
                            Run(args);
                        }
                    }
                }
                else if (args.Length == 1)
                {
                    //if (args[0] == "-scriptForm")
                    //{
                    //    (new Feng.Utils.ScriptForm()).ShowDialog();
                    //}
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "错误");
            }
        }

        private static void InitConfigurations()
        {
            SystemConfiguration.ApplicationName = FengProgram.AppConfig.Product.Name;
            SystemConfiguration.Server = FengProgram.AppConfig.Product.ServerPath;
            if (!SystemConfiguration.Server.Contains("//"))
                SystemConfiguration.Server = "http://" + SystemConfiguration.Server;

            SystemConfiguration.LiteMode = FengProgram.AppConfig.Product.LiteMode;
            //SystemConfiguration.ApplicationMode = FengProgram.AppConfig.Product.ApplicationMode;
            SystemConfiguration.IsInDebug = FengProgram.AppConfig.Product.DebugMode;

            ProductInfo.Instance.Name = FengProgram.AppConfig.Product.ProductName;
            ProductInfo.Instance.CompanyName = FengProgram.AppConfig.Product.CompanyName;
            ProductInfo.Instance.CurrentVersion = FengProgram.AppConfig.Product.CurrentAppVersion;
        }

        private static void Run(string[] args)
        {
            FengProgram program = null;
            if (!SystemConfiguration.LiteMode)
            {
                program = CreateSprintProgram();
            }
            if (program == null)
            {
                program = new FengProgram(FengProgram.AppConfig.Product.ResoucrceAssembly);
            }

            string mutexName = Application.ExecutablePath.Replace("\\", "");

            //bool appNewInstance;
            //Mutex m = new Mutex(false, mutexName, out appNewInstance);
            //MessageBox.Show(mutexName + "," + appNewInstance.ToString());

            //if (!appNewInstance)
            //{
            //    MessageForm.ShowWarning("程序已运行!");
            //    GC.KeepAlive(m);
            //    GC.SuppressFinalize(m);
            //    return;
            //}

            using (System.Threading.Mutex mutex = new System.Threading.Mutex(false, mutexName))
            {
                if (!mutex.WaitOne(0, false))
                {
                    System.Windows.Forms.MessageBox.Show("程序已运行！", "错误");
                    return;
                }

                //AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
                //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

                program.Main(args);
            }


            //var d = Feng.NameValueMappingCollection.Instance.GetDataSource("人员单位_全部");

            //Feng.Utils.QueryPerformance.StartQuery();
            //Feng.Grid.MyGrid g = new Feng.Grid.MyGrid();
            ////g.BeginInit();
            //g.SetDataBinding(d, null);
            ////g.EndInit();
            //double a = Feng.Utils.QueryPerformance.StopQuery();

            //Feng.Utils.QueryPerformance.StartQuery();
            //Feng.Windows.Forms.MyComboBox c1 = new Feng.Windows.Forms.MyComboBox();
            ////c1.BeginInit();
            //c1.SetDataBinding(d, null);
            ////c1.EndInit();
            //double b = Feng.Utils.QueryPerformance.StopQuery();

            //Feng.Utils.QueryPerformance.StartQuery();
            //Feng.Windows.Forms.MyOptionPicker c2 = new Feng.Windows.Forms.MyOptionPicker();
            //c2.DropDownControl.BeginInit();
            //c2.SetDataBinding(d, null);
            //c2.DropDownControl.EndInit();
            //double c = Feng.Utils.QueryPerformance.StopQuery();

            //System.Windows.Forms.MessageBox.Show(a + "," + b + "," + c);
        }

        private static FengProgram CreateSprintProgram()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            try
            {
                return ctx.GetObject("Program") as FengProgram;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void TryCopyNotUpdated()
        {
            Feng.Updater.UpdateHelper.UpdateFiles();
        }

        private static bool TryUpdate()
        {
            //if (System.Windows.Forms.MessageBox.Show("检测更新！", "Update", System.Windows.Forms.MessageBoxButtons.YesNo)
            //    == System.Windows.Forms.DialogResult.No)
            //    return false;
            if (FengProgram.AppConfig == null)
                return false;

            //log4net.ILog m_log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            AutoUpdater updater = new AutoUpdater();
            updater.ServerPath = SystemConfiguration.Server + "/" + SystemConfiguration.ApplicationName;
            if (!string.IsNullOrEmpty(updater.ServerPath))
            {
                //m_log.Debug("开始更新...");
                bool hasUpdate = updater.Update();
                if (hasUpdate && updater.NewVersionAvailable)
                {
                    //m_log.Debug(string.Format("更新到{0}版本", updater.AutoUpdateConfig.AvailableVersion));

                    try
                    {
                        //System.Threading.Mutex mutex = new System.Threading.Mutex(true, SystemConfiguration.ApplicationName + ".update");

                        ProcessStartInfo p = new ProcessStartInfo("Feng.Updater.exe");
                        p.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
                        p.Arguments = "-DoUpdate";
                        Process updaterProcess = Process.Start(p);

                        Application.Exit();

                        return true;
                    }
                    catch (Exception)
                    {
                        System.Windows.Forms.MessageBox.Show("更新错误，请重新安装程序！");
                        //m_log.Error("更新错误", ex);
                    }
                }
            }

            return false;
        }

        //private static readonly log4net.ILog m_log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            //MessageBox.Show(args.LoadedAssembly.ToString() + "," +Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            //System.Windows.Forms.MessageBox.Show("loaded " + args.LoadedAssembly.FullName);
            //m_log.Info("loaded " + args.LoadedAssembly.FullName);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                ExceptionProcess.ProcessUnhandledException(e.Exception);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(e.Exception.Message, "错误");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            try
            {
                ExceptionProcess.ProcessUnhandledException(ex);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "错误");
            }
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //System.Windows.Forms.MessageBox.Show("Try load " + args.Name);
            //m_log.Info("Try load " + args.Name);
            //return null;

            try
            {
                string strFileName = args.Name.Split(',')[0];

                // NHibernate
                if (strFileName.EndsWith(".XmlSerializers") || strFileName.EndsWith(".resources"))
                    return null;

                ResourceContent rc = ResourceInfoHelper.ResolveResource(strFileName + ".dll", ResourceType.File);
                if (rc == null)
                    return null;

                switch (rc.Type)
                {
                    case ResourceContentType.File:
                        return System.Reflection.Assembly.LoadFrom(rc.Content.ToString());
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
