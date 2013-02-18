using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Utils;

namespace Feng.Windows.Utils
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnExportAllToOne_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "Excel Xml(*.xml)|*.xml";
            //saveFileDialog1.Title = "保存";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ADUtils.ExportAllToXmlFile(dlg.FileName);
            }
        }
        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "Excel Xml(*.xml)|*.xml";
            //saveFileDialog1.Title = "保存";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ADUtils.ExportTemplate(dlg.FileName);
            }
        }

        private void btnExportPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ADUtils.ExportAllToPath(dlg.SelectedPath);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "Excel Xml(*.xml)|*.xml";
            dlg.Multiselect = true;
            //saveFileDialog1.Title = "保存";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (MessageForm.ShowYesNo("您正在导入数据到数据库，如果有相同的主键，将会替换数据库数据，是否确认？"))
                {
                    foreach (string fileName in dlg.FileNames)
                    {
                        ADUtils.ImportFromXmlFile(fileName, true);
                        ADUtils.ImportFromXmlFile(fileName);
                    }
                }
            }
        }

        private void btnImportFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (MessageForm.ShowYesNo("您正在导入数据到数据库，如果有相同的主键，将会替换数据库数据，是否确认？"))
                {
                    foreach (string fileName in System.IO.Directory.GetFiles(dlg.SelectedPath))
                    {
                        ADUtils.ImportFromXmlFile(fileName);
                    }
                }
            }
        }

        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "Excel Xml(*.xml)|*.xml";
            //saveFileDialog1.Title = "保存";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (MessageForm.ShowYesNo("您正在删除数据库数据，按照主键删除，是否确认？"))
                {
                    ADUtils.DeleteFromXmlFile(dlg.FileName);
                }
            }
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDbTableName.Text))
            {
                MessageForm.ShowWarning("Table Name不能为空");
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "Excel Xml(*.xml)|*.xml";
            //saveFileDialog1.Title = "保存";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(dlg.FileName, false, Encoding.UTF8);
                ExcelXmlHelper.WriteExcelXmlHead(sw);

                string[] ss = txtDbTableName.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in ss)
                {
                    ExcelXmlHelper.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + s), sw, s);
                }
                ExcelXmlHelper.WriteExcelXmlTail(sw);
                sw.Close();
            }
        }

        private void btnExcuteScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "Sql script(*.sql)|*.sql";
            //saveFileDialog1.Title = "保存";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (MessageForm.ShowYesNo("您正在执行数据库脚本，是否确认？"))
                {
                    Feng.Data.DbHelper.ExecuteSqlScript(dlg.FileName);
                }
            }
        }

        private void btnExportViews_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.Filter = "Sql script(*.sql)|*.sql";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(dlg.FileName, false, Encoding.UTF8);
                System.Data.DataTable dt = Feng.Data.DbHelper.GetViewFuncProcTrigs();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    sw.WriteLine(row["definition"].ToString());
                }
                sw.Close();
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(dlg.SelectedPath + "\\ViewFuncs.sql", false, Encoding.UTF8);
                System.Data.DataTable dt = Feng.Data.DbHelper.GetViewFuncProcTrigs();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    sw.WriteLine(row["definition"].ToString());
                }
                sw.Close();

                ADUtils.ExportAllToXmlFile(dlg.SelectedPath + "\\ADInfos.xml");
            }
        }

        private void btnTestView_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = Feng.Data.DbHelper.GetViewFuncProcTrigs();
            foreach (System.Data.DataRow row in dt.Rows)
            {
                if (row["Type"].ToString() == "View")
                {
                    string cmd = "SELECT TOP 1 * FROM " + row["name"].ToString();
                    try
                    {
                        Feng.Data.DbHelper.Instance.ExecuteDataTable(cmd);
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcess.ProcessWithNotify(new ArgumentException("视图" + row["name"].ToString() + "有误！", ex));
                    }
                }
            }
        }

        private void btnDisableFK_Click(object sender, EventArgs e)
        {
            if (ckbFKSelection.Checked)
            {
                ADUtils.DisableFKConstraint("AD_");
            }
            else
            {
                ADUtils.DisableFKConstraint(null);
            }
        }

        private void btnEnableFK_Click(object sender, EventArgs e)
        {
            if (ckbFKSelection.Checked)
            {
                ADUtils.EnableFKConstraint("AD_");
            }
            else
            {
                ADUtils.EnableFKConstraint(null);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.lblServerName.Text = ConfigurationHelper.GetServerDatabaseName();
            this.txtConnectionString.Text = System.Configuration.ConfigurationManager.ConnectionStrings[SecurityHelper.DataConnectionStringName].ConnectionString;
        }

        private void btnPackageCurrentDb_Click(object sender, EventArgs e)
        {
            ModuleHelper.PackageCurrentDatabase(txtModuleName.Text);
        }

        private void btnModuleGenerateInfo_Click(object sender, EventArgs e)
        {
            ModuleHelper.GenerateModuleInfos(txtModuleName.Text);
        }

        private void btnPackgeModuleAccoringInfo_Click(object sender, EventArgs e)
        {
            ModuleHelper.PackageModule(txtModuleName.Text);
        }

        private void btnInstallModule_Click(object sender, EventArgs e)
        {
            ModuleHelper.InstallModule(txtModuleName.Text);
        }

        private void btnUnInstallModule_Click(object sender, EventArgs e)
        {
            ModuleHelper.UninstallModule(txtModuleName.Text);
        }

        private void btnUpdateConnectionString_Click(object sender, EventArgs e)
        {
            SecurityHelper.ChangeConnectionString(SecurityHelper.DataConnectionStringName, txtConnectionString.Text, "System.Data.SqlClient");
            //System.Configuration.ConfigurationManager.ConnectionStrings["DataConnectionString"].ConnectionString = txtConnectionString.Text;
        }

        private void btnSchemaExport_Click(object sender, EventArgs e)
        {
            string fileName = System.IO.Path.GetTempFileName();
            fileName = System.IO.Path.ChangeExtension(fileName, "txt");
            Feng.NH.NHibernateHelper.ExportSchema(fileName);

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(fileName);
            if (process != null)
            {
                process.WaitForExit();
            }
            System.IO.File.Delete(fileName);
        }

        private void btnSchemaUpdate_Click(object sender, EventArgs e)
        {
            string fileName = System.IO.Path.GetTempFileName();
            fileName = System.IO.Path.ChangeExtension(fileName, "txt");
            Feng.NH.NHibernateHelper.UpdateSchema(fileName);

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(fileName);
            if (process != null)
            {
                process.WaitForExit();
            }
            System.IO.File.Delete(fileName);
        }

        private void btnGenerateHbm_Click(object sender, EventArgs e)
        {
            string fileName = System.IO.Path.GetTempFileName();
            Feng.NH.NHMAHelper.ExportMappingAttribute(fileName);

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(fileName);
            if (process != null)
            {
                process.WaitForExit();
            }
            System.IO.File.Delete(fileName);
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath + ".config");
        }

        private void btnGenerateHelpXml_Click(object sender, EventArgs e)
        {
            IOHelper.TryCreateDirectory(".\\Help\\Data\\Window\\");
            IOHelper.TryCreateDirectory(".\\Help\\Data\\Grid\\");

            HelpGenerator.GenerateXml(".\\Help\\Data");
        }

        private void btnGenerateHelpHtml_Click(object sender, EventArgs e)
        {
        	IOHelper.TryCreateDirectory(".\\Help\\html");

        	foreach(string fileName in System.IO.Directory.GetFiles(".\\Help\\Data\\Window"))
        	{
                //if (!fileName.Contains("资金票据_凭证.xml"))
                //    continue;
        		HelpGenerator.GenerateHtml(fileName, ".\\Help\\window_template.xslt",
                                                      ".\\Help\\html\\window_" + System.IO.Path.GetFileNameWithoutExtension(fileName) + ".html");
        	}
        	foreach(string fileName in System.IO.Directory.GetFiles(".\\Help\\Data\\Grid"))
        	{
                //if (!fileName.Contains("凭证.xml"))
                //    continue;
        		HelpGenerator.GenerateHtml(fileName, ".\\Help\\grid_template.xslt",
                                                      ".\\Help\\html\\grid_" + System.IO.Path.GetFileNameWithoutExtension(fileName) + ".html");
        	}
        }

        private void btnCreateDefaultSettings_Click(object sender, EventArgs e)
        {
            if (MessageForm.ShowYesNo("是否确认重新创建？"))
            {
                Helper.CreateDefaultSettings();
                Helper.CreateSettings4WebserviceTest();
            }
        }

        private void btnClearDefaultSettings_Click(object sender, EventArgs e)
        {
            if (MessageForm.ShowYesNo("是否确认删除？"))
            {
                Helper.ClearDefaultSettings();
            }
        }

        private void btnGenerateAdInfos_Click(object sender, EventArgs e)
        {
            if (!MessageForm.ShowYesNo("是否确认创建？"))
                return;

            Type type = null;
            try
            {
                type = ReflectionHelper.GetTypeFromName(txtAdInfoCreatorType.Text);
            }
            catch(Exception)
            {
            }
            if (type != null)
            {
                Helper.CreateSettings(type, txtAdInfoCreatorGridName.Text);
            }
            else
            {
                System.Data.DataTable dt = null;
                try
                {
                    dt = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + txtAdInfoCreatorType.Text);
                }
                catch (Exception)
                {
                }
                if (dt != null)
                {
                    Helper.CreateSettings(dt, txtAdInfoCreatorGridName.Text);
                }
            }
        }

        private void btnGenerateAdInfos2_Click(object sender, EventArgs e)
        {
            if (!MessageForm.ShowYesNo("是否确认创建？"))
                return;

            Helper.CreateSettings4WSWintab(txtAdInfoCreatorGridName.Text, true, false);
        }

        private void btnPythonScript_Click(object sender, EventArgs e)
        {
            var form = new Feng.Windows.Forms.PythonScriptForm();
            form.Show();
        }

        
    }
}
