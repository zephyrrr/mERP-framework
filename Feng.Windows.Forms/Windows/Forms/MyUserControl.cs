using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MyUserControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public MyUserControl()
        {
            InitializeComponent();

            this.ImeMode = ImeMode.OnHalf;
            this.ControlAdded += new ControlEventHandler(MyUserControl_ControlAdded);
        }

        private void MyUserControl_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.ImeMode = ImeMode.Inherit;
        }
    }
}