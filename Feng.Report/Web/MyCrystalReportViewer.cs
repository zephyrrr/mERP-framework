using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CrystalLibrary;
using Feng.Utils;

namespace Feng.Web
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class MyCrystalReportViewer : CrystalDecisions.Web.CrystalReportViewer
    {
        /// <summary>
        /// 
        /// </summary>
        public MyCrystalReportViewer()
        {
        }

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
            m_ds = ds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportSource"></param>
        /// <param name="ds">当数据源是用Dataset时，设置DataSet模版</param>
        public MyCrystalReportViewer(CrystalDecisions.CrystalReports.Engine.ReportDocument reportSource, DataSet ds)
            : this(reportSource)
        {
            m_ds = ds;
        }

        private DataSet m_ds;

        private CrystalLibrary.CrystalHelper _crystalHelper = new CrystalHelper();

        /// <summary>
        /// CrystalHelper
        /// </summary>
        public CrystalLibrary.CrystalHelper CrystalHelper
        {
            get { return _crystalHelper; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        //public void SetDataBinding(object dataSource, string dataMember)
        //{
        //    foreach (DataTable i in m_ds.Tables)
        //    {
        //        i.Rows.Clear();
        //    }

        //    DataTable dt = dataSource as DataTable;
        //    if (dt != null)
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            DataRow newRow = m_ds.Tables[0].NewRow();

        //            foreach (DataColumn col in m_ds.Tables[0].Columns)
        //            {
        //                newRow[col.ColumnName] = row[col.ColumnName];
        //            }

        //            m_ds.Tables[0].Rows.Add(newRow);
        //        }
        //    }
        //    else
        //    {
        //        IEnumerable list = dataSource as IEnumerable;
        //        if (list != null)
        //        {
        //            foreach (object row in list)
        //            {
        //                DataRow newRow = m_ds.Tables[0].NewRow();

        //                foreach (DataColumn col in m_ds.Tables[0].Columns)
        //                {
        //                    newRow[col.ColumnName] = EntityHelper.GetPropertyValue(row, col.ColumnName);
        //                }

        //                m_ds.Tables[0].Rows.Add(newRow);
        //            }
        //        }
        //        else
        //        {
        //            throw new NotSupportedException("MyCrystalReportViewer only support DataTable and IList now!");
        //        }
        //    }

        //    _crystalHelper.DataSource = m_ds;

        //    OpenReport();
        //}

        /// <summary>
        /// 
        /// </summary>
        public void OpenReport()
        {
            if (_crystalHelper.IsOpen)
            {
                _crystalHelper.Close();
            }

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
