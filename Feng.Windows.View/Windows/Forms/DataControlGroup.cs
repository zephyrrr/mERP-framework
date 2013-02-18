using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    public partial class DataControlGroup : UserControl
    {
        public DataControlGroup()
        {
            InitializeComponent();
        }

        public void SetInnerControls(Control controlLeft, Control controlRight)
        {
            controlLeft.Dock = DockStyle.Fill;
            controlRight.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(controlLeft);
            this.panel2.Controls.Add(controlRight);
        }
    }
}
