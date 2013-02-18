using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace NeokernelService
{
    /// <summary>
    /// Zusammenfassung f黵 ProjectInstaller.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;

        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
