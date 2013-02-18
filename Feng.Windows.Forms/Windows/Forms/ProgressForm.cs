using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }
        public bool IsActive
        {
            get { return loadingCircle1.Active; }
        }

        //public static Dictionary<string, ProgressForm> m_progressForms = new Dictionary<string, ProgressForm>();

        //public static ProgressForm Start(string name, bool visibleStop)
        //{
        //    ProgressForm form = Start(name);
        //    form.btnStop.Visible = visibleStop;
        //    return form;
        //}

        //public static ProgressForm Start(string name)
        //{
        //    if (!m_progressForms.ContainsKey(name))
        //    {
        //        m_progressForms[name] = new ProgressForm();
        //    }
        //    m_progressForms[name].Show(parentForm);
        //    parentForm.Enabled = false;

        //    return m_progressForms[name];
        //}
        //public static void Stop(Control control)
        //{
        //    Form parentForm = FindParentForm(control);
        //    if (m_progressForms.ContainsKey(parentForm))
        //    {
        //        m_progressForms[parentForm].Close();
        //        m_progressForms.Remove(parentForm);
        //    }
        //    parentForm.Enabled = true;
        //}

        private Form m_owner;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public void Start(Form owner)
        {
            Start(owner, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="caption"></param>
        public void Start(Form owner, string caption)
        {
            if (!string.IsNullOrEmpty(caption))
            {
                this.Text = caption;
                this.myLabel1.Text = this.myLabel1.Text.Replace("执行", caption);
            }

            this.loadingCircle1.Active = true;

            m_owner = owner;
            if (m_owner != null)    //  && m_owner.IsMdiChild 不一定要是Mdi，可能是弹出窗体
            {
                //m_owner.Enabled = false;  //  这样会使查找的时候，AutoHide，自动跳到第一个ChildForm
                foreach (Control c in m_owner.Controls)
                {
                    c.Enabled = false;
                }
                this.Show();
                // this.Show(m_owner) 带导致窗体不正常切换
            }
            else
            {
                this.ShowDialog();
            }
        }

        public void Stop()
        {
            if (m_owner != null)
            {
                foreach (Control c in m_owner.Controls)
                {
                    c.Enabled = true;
                }
            }

            this.loadingCircle1.Active = false;
            this.Hide();
        }

        //private static Form FindParentForm(Control control)
        //{
        //    Form form = control.FindForm();
        //    while (true)
        //    {
        //        if (form.IsMdiChild)
        //        {
        //            return form;
        //        }
        //        form = form.ParentForm;
        //        if (form == null)
        //        {
        //            return null;
        //        }
        //    }
        //}

        public event EventHandler ProgressStopped;
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (ProgressStopped != null)
            {
                ProgressStopped(this, System.EventArgs.Empty);
            }

            Stop();
        }
    }
}
