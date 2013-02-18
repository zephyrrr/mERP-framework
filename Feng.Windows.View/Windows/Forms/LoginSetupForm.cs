using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public partial class LoginSetupForm : Form
    {
        public LoginSetupForm()
        {
            InitializeComponent();
        }

        private int m_oldCobIdx = -1;
        private Dictionary<string, ConnectionStringSettings> m_csss = new Dictionary<string, ConnectionStringSettings>();
        private void LoginSetupForm_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.ConnectionStrings.Count == 0)
                return;

            ConnectionStringSettingsCollection cssc = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings css in cssc)
            {
                m_csss[css.Name] = css;
                cobConnectionString.Items.Add(css.Name);
            }
            if (cobConnectionString.Items.Count > 0)
            {
                cobConnectionString.SelectedIndex = 0;
                cobConnectionString.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
                txtConnectionString.Text = m_csss[cobConnectionString.Items[0].ToString()].ConnectionString;
                txtProviderName.Text = m_csss[cobConnectionString.Items[0].ToString()].ProviderName;
                m_oldCobIdx = 0;
            }
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_oldCobIdx != -1)
            {
                m_csss[cobConnectionString.Items[m_oldCobIdx].ToString()].ConnectionString = txtConnectionString.Text;
                m_csss[cobConnectionString.Items[m_oldCobIdx].ToString()].ProviderName = txtProviderName.Text;
            }

            if (cobConnectionString.SelectedIndex != -1)
            {
                m_oldCobIdx = cobConnectionString.SelectedIndex;
                txtConnectionString.Text = m_csss[cobConnectionString.SelectedItem.ToString()].ConnectionString;
                txtProviderName.Text = m_csss[cobConnectionString.Items[0].ToString()].ProviderName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_oldCobIdx != -1)
            {
                m_csss[cobConnectionString.Items[m_oldCobIdx].ToString()].ConnectionString = txtConnectionString.Text;
                m_csss[cobConnectionString.Items[m_oldCobIdx].ToString()].ProviderName = txtProviderName.Text;
            }
            foreach (KeyValuePair<string, ConnectionStringSettings> kvp in m_csss)
            {
                SecurityHelper.ChangeConnectionString(kvp.Key, kvp.Value.ConnectionString, kvp.Value.ProviderName);
            }
        }
    }
}
