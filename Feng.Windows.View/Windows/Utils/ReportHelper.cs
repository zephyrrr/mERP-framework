using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    public static class ReportHelper
    {
        /// <summary>
        /// 创建rdlc的Report
        /// </summary>
        /// <param name="documentName"></param>
        /// <returns></returns>
        public static string CreateMsReportFile(string documentName)
        {
            if (System.IO.File.Exists(documentName))
            {
                return documentName;
            }
            else
            {
                ResourceContent data = ResourceInfoHelper.ResolveResource(documentName, ResourceType.MsReport);
                if (data != null)
                {
                    switch (data.Type)
                    {
                        case ResourceContentType.File:
                            return data.Content.ToString();
                        case ResourceContentType.Binary:
                            string fileName = System.IO.Path.GetTempFileName();
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, false))
                            {
                                sw.Write(data);
                            }
                            return fileName;
                        default:
                            throw new ArgumentException("Invalid Resource Content Type!");
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("Can't find resouce of {0}!", documentName));
                }
            }
        }

        public static CrystalDecisions.CrystalReports.Engine.ReportDocument CreateReportDocument(string documentName)
        {
            try
            {
                CrystalDecisions.CrystalReports.Engine.ReportDocument ret =
                   Feng.Utils.ReflectionHelper.CreateInstanceFromName(documentName) as CrystalDecisions.CrystalReports.Engine.ReportDocument;
                if (ret != null)
                    return ret;
            }
            catch (Exception)
            {
            }

            ResourceContent res = ResourceInfoHelper.ResolveResource(documentName, ResourceType.Report);
            if (res != null)
            {
                switch (res.Type)
                {
                    case ResourceContentType.File:
                        {
                            CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                            reportDocument.Load(res.Content.ToString());
                            return reportDocument;
                        }
                    case ResourceContentType.Binary:
                        {
                            byte[] data = (byte[])res.Content;
                            string fileName = System.IO.Path.GetTempFileName();
                            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                            {
                                fs.Write(data, 0, data.Length);
                            }
                            CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                            reportDocument.Load(fileName);
                            return reportDocument;
                        }
                    default:
                        throw new ArgumentException("Invalid Resource Content Type!");
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Can't find resouce of {0}!", documentName));
            }
        }

        public static System.Data.DataSet CreateDataset(string datasetName)
        {
            try
            {
                 System.Data.DataSet ds = Feng.Utils.ReflectionHelper.CreateInstanceFromName(datasetName) as System.Data.DataSet;
                 if (ds != null)
                     return ds;
            }
            catch (Exception)
            {
            }

            ResourceContent dataSetXmlSchema = ResourceInfoHelper.ResolveResource(datasetName, ResourceType.Dataset);
            if (dataSetXmlSchema != null)
            {
                switch (dataSetXmlSchema.Type)
                {
                    case ResourceContentType.File:
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            using (System.IO.StreamReader sr = new System.IO.StreamReader(dataSetXmlSchema.Content.ToString()))
                            {
                                ds.ReadXmlSchema(sr);
                            }
                            return ds;
                        }
                    case ResourceContentType.String:
                        {
                            System.Data.DataSet ds = new System.Data.DataSet();
                            using (System.IO.StringReader sr = new System.IO.StringReader(dataSetXmlSchema.Content.ToString()))
                            {
                                ds.ReadXmlSchema(sr);
                            }
                            return ds;
                        }
                    default:
                        throw new ArgumentException("Invalid Resource Content Type!");
                }
                
            }
            else
            {
                throw new ArgumentException(string.Format("Can't find resouce of {0}!", datasetName));
            }
        }
    }
}
