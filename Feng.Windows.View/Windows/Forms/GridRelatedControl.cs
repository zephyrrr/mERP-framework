using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Xceed.SmartUI;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public partial class GridRelatedControl : UserControl
    {
        private Xceed.SmartUI.Controls.OfficeTaskPane.SmartOfficeTaskPane m_taskPane1, m_taskPane2;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="dm"></param>
        /// <param name="parentForm"></param>
        public GridRelatedControl(string gridName, IDisplayManager dm, IArchiveMasterForm parentForm)
        {
            InitializeComponent();

            m_taskPane1 = new GridGotoFormTaskPane(gridName, dm, parentForm);
            m_taskPane1.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(m_taskPane1);

            m_taskPane2 = new GridRelatedAddressTaskPane(gridName, dm, parentForm, linkLabelAddress);
            m_taskPane2.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(m_taskPane2);
        }

        private void linkLabelAddress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(linkLabelAddress.Text))
            {
                ApplicationExtention.NavigateTo(ServiceProvider.GetService<IApplication>(), linkLabelAddress.Text);
            }
        }
    }
}
