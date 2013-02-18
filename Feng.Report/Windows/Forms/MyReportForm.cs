using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Feng.Utils;

namespace Feng.Windows.Forms
{
    public partial class MyReportForm : Form
    {
        protected MyReportForm()
        {
            InitializeComponent();
        }

        public MyReportForm(string reportInfoName, Dictionary<string, IEnumerable> data)
            : this()
        {
            m_reportInfoName = reportInfoName;
            m_data = data;

            ReportInfo reportInfo = ADInfoBll.Instance.GetReportInfo(reportInfoName);
            ReportDocument reportDocument = ReflectionHelper.CreateInstanceFromName(reportInfo.ReportDocument) as ReportDocument;
            this.crystalReportViewer1.CrystalHelper.ReportSource = reportDocument;
            this.crystalReportViewer1.TemplateDataSet = ReflectionHelper.CreateInstanceFromName(reportInfo.DatasetName) as System.Data.DataSet;

            FillDataSet(data);

            if (reportInfo.AfterProcessId.HasValue)
            {
                ADUtils.ExecuteProcess(ADInfoBll.Instance.GetProcessInfo(reportInfo.AfterProcessId.Value),
                   new object[] { this.crystalReportViewer1.TemplateDataSet, m_data });
            }
        }

        private Dictionary<string, IEnumerable> m_data;
        private string m_reportInfoName;
        private void FillDataSet(Dictionary<string, IEnumerable> data)
        {
            foreach (System.Data.DataTable dt in this.TemplateDataSet.Tables)
            {
                GenerateReportData.Generate(dt, data[dt.TableName], dt.TableName);
            }
        }

        /// <summary>
        /// Data
        /// </summary>
        public Dictionary<string, IEnumerable> Data
        {
            get { return m_data; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataSet TemplateDataSet
        {
            get { return this.crystalReportViewer1.TemplateDataSet; }
        }

        private void MyReportForm_Load(object sender, EventArgs e)
        {
            lblHint.Dock = DockStyle.Fill;
            lblHint.Visible = true;

            this.crystalReportViewer1.OpenReport();

            lblHint.Visible = false;
        }
    }
}
