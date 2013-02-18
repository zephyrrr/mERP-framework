using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeokernelService
{
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                // C:\Windows\Microsoft.NET\Framework\v2.0.50727\installutil.exe NeokernelService.exe
                this.textBoxSVNBinaryPath.Text = RegistryRW.ReadValue(RegistryRW.RegistryRootKeys.HKEY_LOCAL_MACHINE, "SOFTWARE\\NeokernelService", "Path", "E:\\Hd\trunk\\Hd2\\Reference").ToString();
                this.textBoxRepositoryPath.Text = RegistryRW.ReadValue(RegistryRW.RegistryRootKeys.HKEY_LOCAL_MACHINE, "SOFTWARE\\NeokernelService", "Parameters", "-startup_dir E:\\Hd\\trunk\\Hd2\\Reference -startup_agent Feng.Server.Agents.StartupAgent -reporter com.neokernel.io.ServiceReporter").ToString();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryRW.WriteValue(RegistryRW.RegistryRootKeys.HKEY_LOCAL_MACHINE, "SOFTWARE\\NeokernelService", "Path", this.textBoxSVNBinaryPath.Text.ToString(), true);
                RegistryRW.WriteValue(RegistryRW.RegistryRootKeys.HKEY_LOCAL_MACHINE, "SOFTWARE\\NeokernelService", "Parameters", this.textBoxRepositoryPath.Text.ToString(), true);
                MessageBox.Show("Your configuration has been updated");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please login with an administrative account");
                System.Console.WriteLine(ex.ToString());
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSVNBinaryPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            this.textBoxSVNBinaryPath.Text = path;
        }
    }
}
