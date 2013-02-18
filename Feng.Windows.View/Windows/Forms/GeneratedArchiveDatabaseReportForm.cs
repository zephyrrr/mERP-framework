using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Utils;
using Feng.Search;


namespace Feng.Windows.Forms
{
    /// <summary>
    /// 根据数据库生成的报表，可传入参数
    /// </summary>
    [CLSCompliant(false)]
    public class GeneratedArchiveDatabaseReportForm : ArchiveDatabaseReportForm
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

        public GeneratedArchiveDatabaseReportForm(WindowInfo windowInfo)
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

            IDisplayManager dmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(tabInfos[0], smMaster);

            smMaster.DataLoading += new EventHandler<DataLoadingEventArgs>(smMaster_DataLoading);
            ReportInfo reportInfo = ADInfoBll.Instance.GetReportInfo(windowInfo.Name);
            CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = ReportHelper.CreateReportDocument(reportInfo.ReportDocument);

            this.ReportViewer.CrystalHelper.ReportSource = reportDocument;

            // Now only support Sql
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[SecurityHelper.DataConnectionStringName].ConnectionString;

            this.ReportViewer.CrystalHelper.ServerName = builder.DataSource;
            this.ReportViewer.CrystalHelper.DatabaseName = builder.InitialCatalog;
            this.ReportViewer.CrystalHelper.UserId = builder.UserID;
            this.ReportViewer.CrystalHelper.Password = builder.Password;
            this.ReportViewer.CrystalHelper.IntegratedSecurity = builder.IntegratedSecurity;

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

        void smMaster_DataLoading(object sender, DataLoadingEventArgs e)
        {
            e.Cancel = true;

            ISearchExpression searchExpression = e.SearchExpression;
            IList<ISearchOrder> searchOrders = e.SearchOrders;

            ReportGenerator.SetParameter(this.ReportViewer.CrystalHelper, searchExpression);

            this.ReportViewer.OpenReport();
        }

        void GeneratedArchiveDatabaseReportForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.ReportViewer.CrystalHelper.Close();
        }
        
    }
}
