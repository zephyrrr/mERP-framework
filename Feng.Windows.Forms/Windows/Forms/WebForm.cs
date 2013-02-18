using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    public partial class WebForm : Form
    {
        public WebForm()
        {
            InitializeComponent();
        }

        public WebForm(string formText, string address)
            :this()
        {
            this.Text = formText;
            this.webBrowser1.Navigate(address);
        }
    }
}
