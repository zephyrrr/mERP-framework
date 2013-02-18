using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace NeokernelService
{
    /// <summary>
    /// Neokernel服务
    /// sc create nkservice binPath= "E:\Hd\trunk\Hd2\Reference\nkservice.exe -startup_agent E:\Hd\trunk\Hd2\Reference\Feng.Server.Agents.StartupAgent" displayname= nkservice
    /// 这种方式不能传入参数。
    /// </summary>
    partial class nkService : ServiceBase
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        private System.Diagnostics.EventLog eventLog;
        private string nkExe;
        private string nkBinaryPath;
        private string nkParameters;
        private string nkPath;

        public nkService()
        {
            InitializeComponent();
        }

        static void Main()
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;

            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new nkService() };

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }

        protected override void OnStart(string[] args)
        {
            this.nkExe = "neokernel.exe";
            this.AutoLog = false;
            try
            {
                this.nkBinaryPath = RegistryRW.ReadValue(RegistryRW.RegistryRootKeys.HKEY_LOCAL_MACHINE, "SOFTWARE\\NeokernelService", "Path", " ").ToString();
                this.nkParameters = RegistryRW.ReadValue(RegistryRW.RegistryRootKeys.HKEY_LOCAL_MACHINE, "SOFTWARE\\NeokernelService", "Parameters", " ").ToString();
                this.nkPath = System.IO.Path.Combine(nkBinaryPath, nkExe);
            }
            catch (Exception)
            {
                this.eventLog.WriteEntry("Please run nkServiceAdmin to create a valid configuration");
                // raise exception otherwise the service will start even if svnserve.exe could not be started
                throw;
            }

            try
            {
                process.EnableRaisingEvents = false;
                process.StartInfo.FileName = nkPath;
                //use --daemon because svnserve has to work as daemon in our case anyway
                process.StartInfo.Arguments += nkParameters;

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.ErrorDialog = true;

                process.Start();
                this.eventLog.WriteEntry("SVN Service successfully started");
            }
            catch (Exception)
            {
                eventLog.WriteEntry("Failed to start nkService: " + nkPath.ToString() + " " + process.StartInfo.Arguments.ToString() + " Check your configuration", System.Diagnostics.EventLogEntryType.Error);
                // raise exception otherwise the service will be running even if svnserve.exe could not be started
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                process.Kill();
                process.WaitForExit();
                this.eventLog.WriteEntry("SVN Service shutdown successfull");
            }
            catch (Exception)
            {
                eventLog.WriteEntry("Failed to stop svnserve.exe", System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}
