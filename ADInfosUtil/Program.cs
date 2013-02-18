using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;

namespace Feng.Windows.Utils
{
    static class AdInfoUtilProgram
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //System.Windows.Forms.MessageBox.Show("Debug");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Helper.ReplaceFiles("E:\\MySource\\nbzs\\wlxt\\trunk\\Cd2\\PythonScript", "*.py", "\"Feng.Windows.View\"", "\"Feng.Windows.Application\"");
            //Helper.ReplaceFiles("E:\\MySource\\nbzs\\wlxt\\trunk\\Cd2\\PythonScript", "*.py", "\"Feng.Application\"", "\"Feng.Windows.Model\"");
            ////var i = Feng.Utils.ReflectionHelper.GetTypeFromName("Feng.NH.SearchManager`1, Feng.Windows.Controller");

            SystemConfiguration.Roles = new string[] { "系统管理员" };
            ProgramHelper.InitProgram();

            PdnResourcesCollection.Instance.AddPdnResource("Feng", "Feng.Resource", "Feng.Resource", true);

            //Feng.Utils.HelpGenerator.GenerateXml("c:\\a.xml");

            //using (Feng.NH.INHibernateRepository rep = new Feng.NH.Repository("default"))
            //{
            //    var i = rep.Session.Get<Feng.WindowInfo>("报表_财务费用开支明细");
            //    rep.Initialize(i.WindowTabs, i);
            //    rep.Initialize(i.WindowTabs[0].GridInfos, i.WindowTabs[0]);
            //}

            string language = System.Configuration.ConfigurationManager.AppSettings["Language"];
            if (string.IsNullOrEmpty(language))
                language = "PythonScript";

            var sm = ServiceProvider.GetService<Feng.NH.ISessionFactoryManager>();
            if (sm != null)
            {
                sm.DeleteSessionFactoryCache();
                Feng.NH.ICacheConfigurationManager cm = sm as Feng.NH.ICacheConfigurationManager;
                if (cm != null)
                {
                    cm.DeleteConfigurationCaches();
                }
            }

            Form form = null;

            if (args.Length > 0)
            {
                //System.Console.ReadLine();
                if (args[0] == "-hbm" && args.Length == 2)
                {
                    Feng.NH.NHMAHelper.ExportMappingAttribute(args[1]);
                    return;
                }
                else if (args[0] == "-p" && args.Length >= 2)
                {
                    DefaultServiceProvider.Instance.SetDefaultService<IExceptionProcess>(new ConsoleExceptionProcess());

                    Dictionary<string, object> processParams = new Dictionary<string, object>();
                    for (int i = 2; i < args.Length; i += 2)
                    {
                        processParams[args[i]] = args[i + 1];
                    }
                    ProcessInfoHelper.ExecuteProcess(args[1], processParams);
                    return;
                }
                else if (args[0] == "-s")
                {
                    if (args.Length >= 2)
                    {
                        language = args[1];
                    }
                    switch (language)
                    {
                        case "PythonScript":
                            form = new Feng.Windows.Forms.PythonScriptForm();
                            break;
                        case "PythonCode":
                            form = new Feng.Windows.Forms.PythonCodeForm();
                            break;
                        case "RubyScript":
                            form = new Feng.Windows.Forms.RubyScriptForm();
                            break;
                        case "RubyCode":
                            form = new Feng.Windows.Forms.RubyCodeForm();
                            break;
                        case "CSharpCode":
                            form = new Feng.Windows.Forms.CSharpCodeForm();
                            break;
                    }
                }
            }
            else
            {
                form = new Form1();
            }

            Application.Run(form);
        }
    }
}
