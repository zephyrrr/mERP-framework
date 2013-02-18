using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using Spring.Context;
using Spring.Context.Support;
using Feng.Windows.Forms;
using Feng.Windows;
using Feng.Windows.Utils;

namespace Feng.Run
{
    /// <summary>
    /// 
    /// </summary>
    public class FengProgram
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceAssembly"></param>
        public FengProgram(string resourceAssembly)
            : base()
        {
            if (!string.IsNullOrEmpty(resourceAssembly))
            {
                string[] ss = resourceAssembly.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (ss.Length != 2)
                {
                    throw new ArgumentException("Invalid Program run args!", "resourceAssembly");
                }
                //ResourceInfoHelper.ResolveResource(ss[2], ResourceType.File);
                PdnResourcesCollection.Instance.AddPdnResource(ss[0], ss[0], ss[1], true);
            }

            PdnResourcesCollection.Instance.AddPdnResource("Feng", "Feng.Resource", "Feng.Resource", string.IsNullOrEmpty(resourceAssembly));
        }

        private static AppConfigSection s_appConfig;
        /// <summary>
        /// 
        /// </summary>
        public static AppConfigSection AppConfig
        {
            get
            {
                if (s_appConfig == null)
                {
                    s_appConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None).GetSection("AppConfig") as AppConfigSection;
                }
                return s_appConfig;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Main(string[] args)
        {
            bool loadCs = false;
            try
            {
                //if (args.Length < 2)
                //{
                //    MessageForm.ShowError("运行应用程序错误！");
                //    return;
                //}

                ProgramHelper.InitProgram();

                loadCs = SecurityHelper.LoadConnectionStrings();
                SecurityHelper.SelectAutomaticServer();
                //SecutiryHelper.SaveConnectionStrings("connectionStrings.config", "Data\\Dbs.dat");

                bool outerLogin = true;
                Dictionary<string, string> dictArgs = Feng.Utils.StringHelper.ParseCommandLineArgs(args);
                if (dictArgs.ContainsKey("username") && dictArgs["username"] == "anonymous"
                    && dictArgs.ContainsKey("password") && dictArgs["password"] == "nowandfuture")
                {
                    outerLogin = false;
                }
                // no more parameters
                if (dictArgs.Count == 0)
                {
                    outerLogin = false;
                }

                if (outerLogin)
                {
                    if (!dictArgs.ContainsKey("username") || !dictArgs.ContainsKey("password"))
                        return;

                    var p = LoginHelper.Login(dictArgs["username"], dictArgs["password"]);
                    if (p == null)
                        return;

                    System.Threading.Thread.CurrentPrincipal = p;
                }
                else
                {
                    using (LoginForm frm = new LoginForm())
                    {
                        if (frm.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }
                }

                UserActions.Instance.LoginUser();

                RunProgram();

                //if (outerLogin)
                //{
                //    string fileName = "Feng.Run.exe.config";
                //    if (System.IO.File.Exists(fileName))
                //    {
                //        System.IO.File.Delete(fileName);
                //    }
                //}
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "出现严重错误，程序将关闭！");
            }
            finally
            {
                SecurityHelper.DeselectAutomaticServer();

                if (loadCs)
                {
                    SecurityHelper.RemoveConnectionStrings();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunProgram()
        {
            Form form = ServiceProvider.GetService<IApplication>() as Form;
            if (form != null)
            {
                form.Text = ProductInfo.Instance.Name;
                Application.Run(form);
            }
            else
            {
                MessageForm.ShowError("IApplication must be a Form in WinForm Application!");
            }
        }
    }
}
