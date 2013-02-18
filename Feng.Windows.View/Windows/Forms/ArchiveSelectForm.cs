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
    /// <summary>
    /// ArchiveSelectForm
    /// </summary>
    public partial class ArchiveSelectForm : PositionPersistForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selectFormName"></param>
        public ArchiveSelectForm(string selectFormName)
        {
            InitializeComponent();

            IList<WindowSelectInfo> list = ADInfoBll.Instance.GetWindowSelectInfo(selectFormName);
            if (list == null)
            {
                return;
            }

            foreach (WindowSelectInfo info in list)
            {
                if (!Authority.AuthorizeByRule(info.Visible))
                    continue;

                MyRadioButton rbg = new MyRadioButton();
                rbg.Text = info.Text;
                rbg.Tag = info;

                this.flowLayoutPanel1.Controls.Add(rbg);
            }
            if (this.flowLayoutPanel1.Controls.Count > 0)
            {
                (this.flowLayoutPanel1.Controls[0] as MyRadioButton).Checked = true;
            }
        }

        /// <summary>
        /// 选定的FormType
        /// </summary>
        public MyChildForm SelectedForm
        {
            get
            {
                foreach (MyRadioButton rbg in this.flowLayoutPanel1.Controls)
                {
                    if (rbg.Checked)
                    {
                        WindowSelectInfo info = rbg.Tag as WindowSelectInfo;
                        if (info.Form != null)
                        {
                            FormInfo formInfo = ADInfoBll.Instance.GetFormInfo(info.Form.Name);
                            return ArchiveFormFactory.CreateForm(formInfo) as MyChildForm;
                        }
                        else
                        {
                            WindowInfo windowInfo = ADInfoBll.Instance.GetWindowInfo(info.Window.Name);
                            return ServiceProvider.GetService<IWindowFactory>().CreateWindow(windowInfo) as MyChildForm;
                        }
                    }
                }
                return null;
            }
        }
    }
}
