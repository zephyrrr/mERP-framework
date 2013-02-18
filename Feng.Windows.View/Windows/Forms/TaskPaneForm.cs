using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Forms;

namespace Feng.Windows.Forms
{
    public partial class TaskPaneForm : MyChildForm
    {
        public TaskPaneForm()
        {
            InitializeComponent();

            m_taskPane = new TaskPane("全部");
            m_taskPane.Dock = DockStyle.Fill;
            this.Controls.Add(m_taskPane);
        }
        private TaskPane m_taskPane;

        protected override void Form_Load(object sender, EventArgs e)
        {
            base.Form_Load(sender, e);

            m_taskPane.RefreshItem();
        }
    }
}
