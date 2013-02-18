using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CrystalDecisions.Shared;
using CrystalLibrary;
using Feng.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class MyCrystalReportViewer : CrystalDecisions.Windows.Forms.CrystalReportViewer, IBindingControl
    {
        protected override void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                _crystalHelper.Close();
                _crystalHelper.Dispose();
                _crystalHelper = null;
                m_ds.Dispose();
                m_ds = null;
            }
            base.Dispose(disposeManaged);
        }

        /// <summary>
        /// 
        /// </summary>
        public MyCrystalReportViewer()
            : base()
        {
            //base.ShowGroupTreeButton = false;
            //base.ShowLogo = false;
            //base.ShowParameterPanelButton = false;

            //foreach (System.Windows.Forms.Control ctrl in this.Controls)
            //{
            //    if (ctrl is System.Windows.Forms.ToolStrip)
            //    {
            //        System.Windows.Forms.ToolStripButton btnExport = new System.Windows.Forms.ToolStripButton();
            //        btnExport.Text = "导出";
            //        (ctrl as System.Windows.Forms.ToolStrip).Items.Add(btnExport);
            //        btnExport.Click += new EventHandler(btnExport_Click);
            //    }
            //}
        }

        //void btnExport_Click(object sender, EventArgs e)
        //{
        //    if (_crystalHelper.ReportSource != null)
        //    {
        //        // Declare variables and get the export options.
        //        ExportOptions exportOpts;
        //        ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
        //        DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
        //        exportOpts = _crystalHelper.ReportSource.ExportOptions;

        //        // Set the excel format options.
        //        excelFormatOpts.ExcelUseConstantColumnWidth = true;
        //        excelFormatOpts.ExcelAreaType = AreaSectionKind.WholeReport;
        //        //excelFormatOpts.ShowGridLines = false;

        //        exportOpts.ExportFormatType = ExportFormatType.Excel;
        //        exportOpts.FormatOptions = excelFormatOpts;

        //        // Set the disk file options and export.
        //        exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
        //        diskOpts.DiskFileName = "c:\\a.xls";
        //        exportOpts.DestinationOptions = diskOpts;

        //        _crystalHelper.ReportSource.Export();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportFile"></param>
        public MyCrystalReportViewer(string reportFile)
            : this()
        {
            _crystalHelper.ReportFile = reportFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportSource"></param>
        public MyCrystalReportViewer(CrystalDecisions.CrystalReports.Engine.ReportDocument reportSource)
            : this()
        {
            _crystalHelper.ReportSource = reportSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportFile"></param>
        /// <param name="ds">当数据源是用Dataset时，设置DataSet模版</param>
        public MyCrystalReportViewer(string reportFile, DataSet ds)
            : this(reportFile)
        {
            TemplateDataSet = ds;
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="reportSource"></param>
        /// <param name="ds">当数据源是用Dataset时，设置DataSet模版</param>
        public MyCrystalReportViewer(CrystalDecisions.CrystalReports.Engine.ReportDocument reportSource, DataSet ds)
            : this(reportSource)
        {
            TemplateDataSet = ds;
        }

        private DataSet m_ds;

        /// <summary>
        /// TemplateDataSet
        /// </summary>
        public DataSet TemplateDataSet
        {
            get { return m_ds; }
            set { m_ds = value; _crystalHelper.DataSource = m_ds; }
        }

        private CrystalLibrary.CrystalHelper _crystalHelper = new CrystalHelper();

        /// <summary>
        /// CrystalHelper
        /// </summary>
        public CrystalLibrary.CrystalHelper CrystalHelper
        {
            get { return _crystalHelper; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        public void SetDataBinding(object dataSource, string dataMember)
        {
            foreach (DataTable i in m_ds.Tables)
            {
                i.Rows.Clear();
            }

            DataTable dt = dataSource as DataTable;
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow newRow = m_ds.Tables[0].NewRow();

                    foreach (DataColumn col in m_ds.Tables[0].Columns)
                    {
                        newRow[col.ColumnName] = row[col.ColumnName];
                    }

                    m_ds.Tables[0].Rows.Add(newRow);
                }
            }
            else
            {
                IEnumerable list = dataSource as IEnumerable;
                if (list != null)
                {
                    foreach (object row in list)
                    {
                        DataRow newRow = m_ds.Tables[0].NewRow();

                        foreach (DataColumn col in m_ds.Tables[0].Columns)
                        {
                            newRow[col.ColumnName] = EntityScript.GetPropertyValue(row, col.ColumnName);
                        }

                        m_ds.Tables[0].Rows.Add(newRow);
                    }
                }
                else
                {
                    throw new NotSupportedException("MyCrystalReportViewer only support DataTable and IList now!");
                }
            }

            OpenReport();
        }


        /// <summary>
        /// 
        /// </summary>
        public void OpenReport()
        {
            // if closed, can't be reopened;
            //if (_crystalHelper.IsOpen)
            //{
            //    _crystalHelper.Close();
            //}

            _crystalHelper.Open();

            this.ReportSource = _crystalHelper.ReportSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void SetState(StateType type)
        {
        }
    }
}
