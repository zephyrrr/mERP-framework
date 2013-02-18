using System;
using System.Collections.Generic;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using Feng.Search;
using CrystalLibrary;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 报表生成
    /// </summary>
    public class ReportGenerator
    {
        /// <summary>
        /// 打开报表(全部)
        /// </summary>
        /// <param name="monthReportInfo"></param>
        public static void OpenReports(MonthReportInfo monthReportInfo)
        {
            if (monthReportInfo == null)
            {
                throw new ArgumentNullException("monthReportInfo");
            }
            string fileName = System.IO.Path.GetTempFileName();
            fileName = System.IO.Path.ChangeExtension(fileName, ".pdf");

            PdfMerge pdfMerge = new PdfMerge();
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<MonthReportInfo>())
            {
                rep.Attach(monthReportInfo);
                foreach (MonthReportDataInfo i in monthReportInfo.Reports)
                {
                    pdfMerge.AddDocument(i.Name, i.Data);
                }
            }
            
            pdfMerge.Merge(fileName);
            ProcessHelper.ExecuteApplication(fileName);
        }

        /// <summary>
        /// 打开报表
        /// </summary>
        /// <param name="monthReportDataInfo"></param>
        public static void OpenReport(MonthReportDataInfo monthReportDataInfo)
        {
            if (monthReportDataInfo == null)
            {
                throw new ArgumentNullException("monthReportDataInfo");
            }
            string fileName = System.IO.Path.GetTempFileName();
            fileName = System.IO.Path.ChangeExtension(fileName, ".pdf");
            byte[] data = monthReportDataInfo.Data;
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.CreateNew);
            using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(fs))
            {
                w.Write(data);
            }
            fs.Close();

            ProcessHelper.ExecuteApplication(fileName);
        }

        /// <summary>
        /// 生成多个报表，并保存到月报表数据中
        /// </summary>
        /// <param name="reportInfoNames"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public static void GenerateMonthReport(MonthReportInfo monthReportInfo, string[] reportInfoNames)
        {
            LogEntityDao<MonthReportInfo> dao = new LogEntityDao<MonthReportInfo>();
            LogEntityDao<MonthReportDataInfo> subDao = new LogEntityDao<MonthReportDataInfo>();

            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<MonthReportInfo>())
            {
                try
                {
                    rep.BeginTransaction();
                    dao.Update(rep, monthReportInfo);

                    if (monthReportInfo.Reports != null)
                    {
                        foreach (MonthReportDataInfo i in monthReportInfo.Reports)
                        {
                            rep.Delete(i);
                        }
                        monthReportInfo.Reports.Clear();
                    }
                    foreach (string s in reportInfoNames)
                    {
                        MonthReportDataInfo dataInfo = new MonthReportDataInfo();
                        dataInfo.Name = s;
                        dataInfo.MonthReport = monthReportInfo;
                        dataInfo.Data = GenerateReport(s, monthReportInfo.报表日期起, monthReportInfo.报表日期止);
                        dataInfo.ClientId = 0;
                        dataInfo.OrgId = 0;

                        if (monthReportInfo.Reports == null)
                        {
                            monthReportInfo.Reports = new List<MonthReportDataInfo>();
                        }
                        monthReportInfo.Reports.Add(dataInfo);

                        subDao.Save(rep, dataInfo);
                    }

                    rep.CommitTransaction();
                }
                catch (Exception ex)
                {
                    rep.RollbackTransaction();
                    ExceptionProcess.ProcessWithNotify(ex);
                }

            }
        }
               
        /// <summary>
        /// 生成报表
        /// </summary>
        /// <param name="reportInfoName"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public static byte[] GenerateReport(string reportInfoName, DateTime dateStart, DateTime dateEnd)
        {
            CrystalHelper crystalHelper = new CrystalHelper();

            ReportInfo reportInfo = ADInfoBll.Instance.GetReportInfo(reportInfoName);
            if (reportInfo == null)
            {
                throw new ArgumentException("不存在名为" + reportInfoName + "的ReportInfo!");
            }
            ReportDocument reportDocument = ReportHelper.CreateReportDocument(reportInfo.ReportDocument);
            crystalHelper.ReportSource = reportDocument;
            System.Data.DataSet templateDataSet = ReportHelper.CreateDataset(reportInfo.DatasetName);

            IList<ISearchManager> sms = new List<ISearchManager>();
            IList<ReportDataInfo> reportDataInfos = ADInfoBll.Instance.GetReportDataInfo(reportInfo.Name);
            foreach (ReportDataInfo reportDataInfo in reportDataInfos)
            {
                if (string.IsNullOrEmpty(reportDataInfo.SearchManagerClassName))
                {
                    throw new ArgumentException("ReportDataInfo of " + reportDataInfo.Name + " 's SearchManagerClassName must not be null!");
                }

                ISearchManager sm = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(reportDataInfo.SearchManagerClassName, reportDataInfo.SearchManagerClassParams);

                sm.EnablePage = false;

                sms.Add(sm);
            }

            ISearchExpression se = SearchExpression.And(SearchExpression.Ge("日期", dateStart),
                SearchExpression.Le("日期", dateEnd));
            for (int i = 0; i < reportDataInfos.Count; ++i)
            {
                System.Collections.IEnumerable dataList = sms[i].GetData(se, null);

                string s = reportDataInfos[i].DatasetTableName;
                if (!templateDataSet.Tables.Contains(s))
                {
                    throw new ArgumentException("报表DataSet中未包含名为" + s + "的DataTable！");
                }
                System.Data.DataTable dt = templateDataSet.Tables[s];
                dt.Rows.Clear();
                GenerateReportData.Generate(dt, dataList, reportDataInfos[i].GridName);
            }

            // Set Parameter
            SetParameter(crystalHelper, se);

            crystalHelper.DataSource = templateDataSet;

            string fileName = System.IO.Path.GetTempFileName();
            crystalHelper.Export(fileName, CrystalExportFormat.PortableDocFormat);

            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] fileData = new byte[fs.Length];

            using (System.IO.BinaryReader sr = new System.IO.BinaryReader(fs))
            {
                sr.Read(fileData, 0, fileData.Length);
            }
            fs.Close();
            System.IO.File.Delete(fileName);

            return fileData;
        }

        internal static void SetParameter(CrystalHelper crystalHelper, ISearchExpression se)
        {
            if (se == null)
                return;

            LogicalExpression le = se as LogicalExpression;
            if (le != null)
            {
                SetParameter(crystalHelper, le.LeftHandSide);
                SetParameter(crystalHelper, le.RightHandSide);
            }
            else
            {
                SimpleExpression cse = se as SimpleExpression;

                string simpleParamName = "@" + cse.FullPropertyName;
                string complexParamName = "@" + cse.FullPropertyName + cse.Operator.ToString();
                switch (cse.Operator)
                {
                    case SimpleOperator.Any:
                    case SimpleOperator.EqProperty:
                    case SimpleOperator.IsNotNull:
                    case SimpleOperator.IsNull:
                    case SimpleOperator.NotEq:
                    case SimpleOperator.NotEqProperty:
                    case SimpleOperator.Sql:
                        throw new ArgumentException(cse.Operator + " is not supported in procedure!");
                    case SimpleOperator.Ge:
                    case SimpleOperator.Gt:
                    case SimpleOperator.Le:
                    case SimpleOperator.Lt:
                        crystalHelper.SetParameter(complexParamName, cse.Values);
                        break;
                    case SimpleOperator.Eq:
                    case SimpleOperator.GInG:
                    case SimpleOperator.InG:
                    case SimpleOperator.Like:
                        crystalHelper.SetParameter(simpleParamName, cse.Values);
                        break;
                }
            }
        }
    }
}
