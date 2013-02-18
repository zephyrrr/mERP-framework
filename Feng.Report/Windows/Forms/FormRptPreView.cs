//using System.Data;
//using System.Collections;
//using System;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using System.IO;
//using System.Windows.Forms;
//using Feng.Utils;

//namespace Feng.Windows.Forms
//{
//    /// <summary>
//    /// Report print preview
//    /// </summary>
//    public class FormRptPreView : System.Windows.Forms.Form
//    {
//        private static DataSet m_dataSet;
//        ReportClass m_reportDocument;
//        int ReportCopyNum = 1;
//        bool isDirectPrint = false;

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="orptDs"></param>
//        /// <param name="assemblyName"></param>
//        /// <param name="reportName"></param>
//        public FormRptPreView(DataSet orptDs, string assemblyName, string reportName)
//        {
//            Initialize(orptDs, ReflectionHelper.CreateInstanceFromType(ReflectionHelper.GetTypeFromName(assemblyName, reportName)) as ReportClass, reportName);
//        }

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="orptDs"></param>
//        /// <param name="reportDocument"></param>
//        public FormRptPreView(DataSet orptDs, ReportClass reportDocument)
//        {
//            InitializeComponent();

//            string s = reportDocument.ResourceName;
//            int idx = s.IndexOf('.');

//            Initialize(orptDs, reportDocument, s.Substring(0, idx));
//        }

//        private void Initialize(DataSet orptDs, ReportClass reportDocument, string reportName)
//        {
//            m_dataSet = orptDs;
//            m_reportDocument = reportDocument;

//            this.Text = reportName;
//        }
//        #region " Windows 窗体设计器生成的代码 "

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (!(components == null))
//                {
//                    components.Dispose();
//                }
//            }
//            base.Dispose(disposing);
//        }

//        private System.ComponentModel.Container components = null;
//        internal CrystalDecisions.Windows.Forms.CrystalReportViewer OrptView;
//        internal System.Windows.Forms.Label lblHint;
//        private void InitializeComponent()
//        {
//            this.OrptView = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
//            this.lblHint = new System.Windows.Forms.Label();
//            this.SuspendLayout();
//            // 
//            // OrptView
//            // 
//            this.OrptView.ActiveViewIndex = -1;
//            this.OrptView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.OrptView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.OrptView.Location = new System.Drawing.Point(0, 0);
//            this.OrptView.Name = "OrptView";
//            this.OrptView.SelectionFormula = "";
//            this.OrptView.Size = new System.Drawing.Size(903, 482);
//            this.OrptView.TabIndex = 5;
//            this.OrptView.ViewTimeSelectionFormula = "";
//            // 
//            // lblHint
//            // 
//            this.lblHint.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
//            this.lblHint.ForeColor = System.Drawing.Color.Blue;
//            this.lblHint.Location = new System.Drawing.Point(360, 168);
//            this.lblHint.Name = "lblHint";
//            this.lblHint.Size = new System.Drawing.Size(301, 80);
//            this.lblHint.TabIndex = 12;
//            this.lblHint.Text = "正在加载报表模板，请稍候.....";
//            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            this.lblHint.Visible = false;
//            // 
//            // FormRptPreView
//            // 
//            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
//            this.ClientSize = new System.Drawing.Size(903, 482);
//            this.Controls.Add(this.lblHint);
//            this.Controls.Add(this.OrptView);
//            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
//            this.Name = "FormRptPreView";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
//            this.Text = "报表打印预览";
//            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
//            this.Load += new System.EventHandler(this.frm_RptPreView_Load);
//            this.ResumeLayout(false);

//        }

//        #endregion


//        private void frm_RptPreView_Load(System.Object sender, System.EventArgs e)
//        {
//            try
//            {
//                if (ReportCopyNum == 0 || !isDirectPrint)
//                {
//                    ReportCopyNum = 1;
//                }

//                lblHint.Dock = DockStyle.Fill;
//                lblHint.Visible = true;

//                //ReportDocument Orpt = new ReportDocument();

//                //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
//                //ReportDocument Orpt = (ReportDocument)Utils.CreateInstanceFromName(assembly, assembly.GetName().Name + ".Report." + ReportName);

//                //Orpt.Load(m_baseDirectory + ReportName + ".rpt");

//                OrptView.ReportSource = null;

//                //TableLogOnInfo logOnInfo = new TableLogOnInfo();
//                //logOnInfo.ConnectionInfo.ServerName = ConfigFileData.Instance.ServerName;
//                //logOnInfo.ConnectionInfo.DatabaseName = ConfigFileData.Instance.dbName;
//                //logOnInfo.ConnectionInfo.UserID = ConfigFileData.Instance.dbUid;
//                //logOnInfo.ConnectionInfo.Password = ConfigFileData.Instance.dbPass;
//                //Orpt.Database.Tables[0].ApplyLogOnInfo(logOnInfo);
//                //Orpt.Database.Tables[1].ApplyLogOnInfo(logOnInfo);
//                for (int i = 0; i < m_reportDocument.Database.Tables.Count; ++i)
//                {
//                    Table t = null;
//                    try
//                    {
//                        t = m_reportDocument.Database.Tables[m_dataSet.Tables[i].TableName];
//                    }
//                    catch (Exception)
//                    {
//                    }
//                    if (t != null)
//                    {
//                        t.SetDataSource(m_dataSet.Tables[i]);
//                    }
//                    else
//                    {
//                        m_reportDocument.Database.Tables[i].SetDataSource(m_dataSet.Tables[i]);
//                    }
//                }

//                lblHint.Visible = false;

//                if (isDirectPrint)
//                {
//                    try
//                    {
//                        m_reportDocument.PrintToPrinter(ReportCopyNum, true, 0, 0);
//                    }
//                    catch (Exception)
//                    {
//                        MessageBox.Show("用户取消打印或打印机出错！");
//                    }
//                    finally
//                    {
//                        this.Close();
//                    }
//                }
//                else
//                {
//                    OrptView.ReportSource = m_reportDocument;
//                    //OrptView.DisplayGroupTree = false;
//                }
//            }
//            catch (Exception ex)
//            {
//                ExceptionProcess.ProcessUnhandledException(ex);
//            }
//        }
		
//    }
//}
