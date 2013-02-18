using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Utils;
using Feng.Search;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 根据Dataset生成的报表
    /// </summary>
    [CLSCompliant(false)]
    public class GeneratedArchiveDataSetReportForm : ArchiveDataSetReportForm
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.SearchManager != null)
                {
                    this.SearchManager.DataLoading -= new EventHandler<DataLoadingEventArgs>(smMaster_DataLoading);
                }
            }
            base.Dispose(disposing);
        }

        public GeneratedArchiveDataSetReportForm(WindowInfo windowInfo)
            : base()
        {
            this.Name = windowInfo.Name;
            this.Text = windowInfo.Text;

            IList<WindowTabInfo> tabInfos = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowInfo.Name);
            if (tabInfos == null)
            {
                throw new ArgumentException("there is no windowTab with windowId of " + windowInfo.Name);
            }
            if (tabInfos.Count == 0)
            {
                throw new ArgumentException("There should be at least one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }
            if (tabInfos.Count > 1)
            {
                throw new ArgumentException("There should be at most one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }

            ISearchManager smMaster = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfos[0], null);
            this.SearchManager = smMaster;

            //IDisplayManager dmMaster = ArchiveFormFactory.GenerateDisplayManager(windowInfo, tabInfos[0], smMaster);

            this.SearchManager.DataLoading += new EventHandler<DataLoadingEventArgs>(smMaster_DataLoading);
            
            m_reportInfo = ADInfoBll.Instance.GetReportInfo(windowInfo.Name);

            CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = ReportHelper.CreateReportDocument(m_reportInfo.ReportDocument);
            
            this.ReportViewer.CrystalHelper.ReportSource = reportDocument;
            this.ReportViewer.TemplateDataSet = ReportHelper.CreateDataset(m_reportInfo.DatasetName);

            m_reportDataInfos = ADInfoBll.Instance.GetReportDataInfo(m_reportInfo.Name);
            foreach (ReportDataInfo reportDataInfo in m_reportDataInfos)
            {
                if (string.IsNullOrEmpty(reportDataInfo.SearchManagerClassName))
                {
                    throw new ArgumentException("ReportDataInfo of " + reportDataInfo.Name + " 's SearchManagerClassName must not be null!");
                }

                ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(reportDataInfo.SearchManagerClassName, reportDataInfo.SearchManagerClassParams);

                m_sms.Add(sm);
            }

            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(GeneratedArchiveDataSetReportForm_FormClosed);

            GeneratedArchiveSeeForm.InitializeWindowProcess(windowInfo, this);

            ArchiveSearchForm searchForm = null;
            this.SetSearchPanel(() =>
            {
                if (searchForm == null)
                {
                    searchForm = new ArchiveSearchForm(this, smMaster, tabInfos[0]);
                }
                return searchForm;
            });
        }

        protected override void Form_Closing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this.SearchManager.DataLoading -= new EventHandler<DataLoadingEventArgs>(smMaster_DataLoading);
        }

        void smMaster_DataLoading(object sender, DataLoadingEventArgs e)
        {
            e.Cancel = true;

            ISearchExpression searchExpression = e.SearchExpression;
            IList<ISearchOrder> searchOrders = e.SearchOrders;

            if (this.SearchManager != null)
            {
                for (int i = 0; i < m_reportDataInfos.Count; ++i)
                {
                    m_sms[i].EnablePage = this.SearchManager.EnablePage;
                    m_sms[i].FirstResult = this.SearchManager.FirstResult;
                    m_sms[i].MaxResult = this.SearchManager.MaxResult;
                    m_sms[i].IsResultDistinct = this.SearchManager.IsResultDistinct;
                }
            }

            bool haveSetMasterSearchManager = false;
            for (int i = 0; i < m_reportDataInfos.Count; ++i)
            {
                object data = m_sms[i].GetData(searchExpression, searchOrders);
                System.Collections.IEnumerable dataList = data as System.Collections.IEnumerable;
                if (dataList == null)
                {
                    dataList = (data as System.Data.DataTable).DefaultView;
                }

                System.Data.DataTable dt = this.ReportViewer.TemplateDataSet.Tables[m_reportDataInfos[i].DatasetTableName];
                dt.Rows.Clear();
                GenerateReportData.Generate(dt, dataList, m_reportDataInfos[i].GridName);

                if (!haveSetMasterSearchManager)
                {
                    this.SearchManager.Count = m_sms[i].GetCount(searchExpression);
                    haveSetMasterSearchManager = true;
                    this.SearchManager.OnDataLoaded(new DataLoadedEventArgs(m_sms[i].Result, m_sms[i].Count));
                }
            }

            // Set Parameter
            ReportGenerator.SetParameter(this.ReportViewer.CrystalHelper, searchExpression);

            if (m_reportInfo.AfterProcess != null)
            {
                ProcessInfoHelper.ExecuteProcess(ADInfoBll.Instance.GetProcessInfo(m_reportInfo.AfterProcess.Name),
                   new Dictionary<string, object> { { "masterForm", this } });
            }

            this.ReportViewer.OpenReport();
        }

        void GeneratedArchiveDataSetReportForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.ReportViewer.CrystalHelper.Close();
        }

        private IList<ISearchManager> m_sms = new List<ISearchManager>();
        private IList<ReportDataInfo> m_reportDataInfos;
        private ReportInfo m_reportInfo;
    }

    
}
