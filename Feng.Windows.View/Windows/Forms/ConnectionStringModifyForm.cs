using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Data.ConnectionUI;

namespace Feng.Windows.Forms
{
    public partial class ConnectionStringModifyForm : Form
    {
        private string m_connectionStringFileName;
        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionStringModifyForm()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionStringFileName"></param>
        public ConnectionStringModifyForm(string connectionStringFileName)
        {
            if (string.IsNullOrEmpty(connectionStringFileName))
            {
                m_connectionStringFileName = SystemDirectory.WorkDirectory + "\\connectionStrings.config";
            }
            else
            {
                m_connectionStringFileName = connectionStringFileName;
            }
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!System.IO.File.Exists(m_connectionStringFileName))
            {
                MessageForm.ShowError("不存在连接字符串文件！");
                this.Close();
                return;
            }
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = m_connectionStringFileName };
            Configuration externalConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            int defaultSelectedIndex = -1;
            foreach (ConnectionStringSettings css in externalConfig.ConnectionStrings.ConnectionStrings)
            {
                if (css.Name == "LocalSqlServer")
                    continue;

                m_csss[css.Name] = css;
                cobConnectionStrings.Items.Add(css.Name);

                if (css.Name == Feng.Windows.Utils.SecurityHelper.DataConnectionStringName)
                {
                    defaultSelectedIndex = cobConnectionStrings.Items.Count - 1;
                }
            }

            cobConnectionStrings.SelectedIndex = defaultSelectedIndex;
        }

        private Dictionary<string, ConnectionStringSettings> m_csss = new Dictionary<string, ConnectionStringSettings>();
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (cobConnectionStrings.SelectedItem == null)
                return;

            ConnectionStringSettings css = m_csss[cobConnectionStrings.SelectedItem.ToString()];
            DataConnectionDialog dlg = new DataConnectionDialog();
            Microsoft.Data.ConnectionUI.DataSource.AddStandardDataSources(dlg);

            DataProvider dp = GetDataProviderFromString(css.ProviderName);
            dlg.SelectedDataSource = GetDataSourceFromDataProvider(dp);
            dlg.SelectedDataProvider = dp;
            dlg.ConnectionString = css.ConnectionString;
            if (Microsoft.Data.ConnectionUI.DataConnectionDialog.Show(dlg) == DialogResult.OK)
            {
                css.ProviderName = dlg.SelectedDataProvider.Name;
                css.ConnectionString = dlg.ConnectionString;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap { ExeConfigFilename = m_connectionStringFileName };
            Configuration externalConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            foreach(KeyValuePair<string, ConnectionStringSettings> kvp in m_csss)
            {
                if (externalConfig.ConnectionStrings.ConnectionStrings[kvp.Key] == null)
                {
                    externalConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(kvp.Key, kvp.Value.ConnectionString, kvp.Value.ProviderName));
                }
                else
                {
                    //the full name of the connection string can be found in the app.config file
                    // in the "name" attribute of the connection string
                    externalConfig.ConnectionStrings.ConnectionStrings[kvp.Key].ConnectionString = kvp.Value.ConnectionString;
                    externalConfig.ConnectionStrings.ConnectionStrings[kvp.Key].ProviderName = kvp.Value.ProviderName;
                }
            }
            externalConfig.Save(ConfigurationSaveMode.Modified);

            Feng.Windows.Utils.SecurityHelper.SaveConnectionStrings(m_connectionStringFileName, SystemDirectory.DataDirectory + "\\Dbs.dat");
        }

        private DataProvider GetDataProviderFromString(string dataProvider)
        {
            switch (dataProvider)
            {
                case "System.Data.SqlClient":
                    return DataProvider.SqlDataProvider;
                case "System.Data.Odbc":
                    return DataProvider.OdbcDataProvider;
                case "System.Data.OleDb":
                    return DataProvider.OleDBDataProvider;
                case "System.Data.OracleClient":
                    return DataProvider.OracleDataProvider;
                default:
                    return DataProvider.SqlDataProvider;
            }
        }
        private DataSource GetDataSourceFromDataProvider(DataProvider p)
        {
            if (p == Microsoft.Data.ConnectionUI.DataProvider.SqlDataProvider)
                return Microsoft.Data.ConnectionUI.DataSource.SqlDataSource;
            else if (p == Microsoft.Data.ConnectionUI.DataProvider.OdbcDataProvider)
                return Microsoft.Data.ConnectionUI.DataSource.OdbcDataSource;
            else if (p == Microsoft.Data.ConnectionUI.DataProvider.OleDBDataProvider)
                return Microsoft.Data.ConnectionUI.DataSource.OdbcDataSource;
            else if (p == Microsoft.Data.ConnectionUI.DataProvider.OracleDataProvider)
                return Microsoft.Data.ConnectionUI.DataSource.OracleDataSource;
            else
                return null;
        }

        
    }
}
