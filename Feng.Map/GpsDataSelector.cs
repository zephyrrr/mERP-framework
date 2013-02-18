using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Feng.Map
{
    public partial class GpsDataSelector : Form
    {
        public GpsDataSelector()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtCar.Text))
            //{
            //    m_mapForm.LoadGpsData(txtCar.Text, dtpStart.Value, dtpEnd.Value);
            //}
        }

        public string CarName
        {
            get { return txtCar.Text; }
        }

        public DateTime StartTime
        {
            get { return dtpStart.Value; }
        }

        public DateTime EndTime
        {
            get { return dtpEnd.Value; }
        }
    }
}
