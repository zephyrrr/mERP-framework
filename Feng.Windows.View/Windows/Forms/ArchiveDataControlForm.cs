using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public partial class ArchiveDataControlForm : PositionPersistForm
    {
        public ArchiveDataControlForm()
        {
            InitializeComponent();
        }

        //private DataControlCollection m_dcc = new DataControlCollection();

        /// <summary>
        /// 
        /// </summary>
        public IDataControlCollection DataControls
        {
            get { return m_cm.DisplayManager.DataControls; }
        }

        private IControlManager m_cm;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="name"></param>
        public ArchiveDataControlForm(IControlManager cm, string controlGroupName)
            : this()
        {
            Initialize(cm, ADInfoBll.Instance.GetGridColumnInfos(controlGroupName));
        }

        public ArchiveDataControlForm(IControlManager cm, IList<GridColumnInfo> infos)
            : this()
        {
            Initialize(cm, infos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="infos"></param>
        protected void Initialize(IControlManager cm, IList<GridColumnInfo> infos)
        {
            m_cm = cm;
            if (infos != null)
            {
                foreach (GridColumnInfo info in infos)
                {
                    if (string.IsNullOrEmpty(info.DataControlType))
                    {
                        continue;
                    }

                    IDataControl dc = ControlFactory.GetDataControl(info, cm.DisplayManager.Name);
                    if (dc != null)
                    {
                        //m_dcc.Add(dc);
                        m_cm.DisplayManager.DataControls.Add(dc);

                        Control c = dc as Control;

                        this.flowLayoutPanel1.Controls.Add(c);
                    }
                }

                ArchiveDetailForm.SetDataControlDefaultValues(m_cm);
            }

            m_closeOk = false;
        }

        private bool m_closeOk = false;

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_cm.SaveCurrent())
            {
                m_closeOk = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_closeOk = true;
        }

        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (!m_closeOk)
            {
                e.Cancel = true;
            }
        }
    }
}
