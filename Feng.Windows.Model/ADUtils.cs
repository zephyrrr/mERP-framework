using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Feng.Windows.Utils;
using Feng.Data;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ADUtils
    {
        #region "Export Settings"
        private static string[] s_adTables = new string[] {"AD_Form", "AD_Process", "AD_Window", "AD_Window_Tab", "AD_Window_Menu", 
            "AD_Report", "AD_Report_Data", "AD_Action", "AD_Menu", "AD_Grid", "AD_Grid_Column", "AD_Grid_Row", "AD_Grid_Cell",
            "AD_Grid_Related", "AD_Grid_Group", "AD_Grid_Filter", "AD_Grid_Column_Warning", "Ad_Window_Select", 
            "AD_EventProcess", "AD_AlertRule", "AD_Task", "AD_EntityBuffer", "AD_NameValueMapping", "AD_SimpleParam", "AD_Param_Creator",
            "AD_Search_Custom",
            "AD_Server_TaskSchedule", "AD_Web_Service"};

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <param name="filePath"></param>
        public static void ExportTemplate(string filePath)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false);
            ExcelXmlHelper.WriteExcelXmlHead(sw);

            foreach (string s in s_adTables)
            {
                ExcelXmlHelper.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT TOP 1 * FROM " + s), sw, s);
            }

            ExcelXmlHelper.WriteExcelXmlTail(sw);
            sw.Close();
        }

        /// <summary>
        /// 把现有数据库中的所有配置保存到数据文件中(单个文件)
        /// </summary>
        /// <param name="filePath"></param>
        public static void ExportAllToXmlFile(string filePath)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false);
            ExcelXmlHelper.WriteExcelXmlHead(sw);

            foreach (string s in s_adTables)
            {
                ExcelXmlHelper.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + s), sw, s);
            }

            ExcelXmlHelper.WriteExcelXmlTail(sw);
            sw.Close();
        }

        public static IList<Tuple<string, string, string>> FindInAdInfos(string keyWord)
        {
            if (string.IsNullOrEmpty(keyWord))
                return null;

            string filePath = SystemDirectory.WorkDirectory + "\\AllAdInfos.xml";
            //if (System.IO.File.Exists(filePath))
            //{
            //    if (MessageForm.ShowYesNo("已存在信息配置文件，是否重新生成？"))
            //    {
            //        System.IO.File.Delete(filePath);
            //    }
            //}
            if (!System.IO.File.Exists(filePath))
            {
                ExportAllToXmlFile(filePath);
            }
            var dts = ExcelXmlHelper.ReadExcelXml(filePath, true);
            IList<Tuple<string, string, string>> ret = new List<Tuple<string, string, string>>();
            foreach (var dt in dts)
            {
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    for(int i=0; i<dt.Columns.Count; ++i)
                    {
                        var v = row[i];
                        if (v != null)
                        {
                            var s = v.ToString();
                            if (s.Contains(keyWord))
                            {
                                ret.Add(new Tuple<string,string,string>(dt.TableName, dt.Columns[i].ColumnName, s));
                            }
                        }
                    }
                }
            }
            return ret;
        }
        private const string s_exportPathOthers = "\\Others\\";
        private const string s_exportPathMenus = "\\Menus\\";
        private const string s_exportPathWindows = "\\Windows\\";
        private const string s_exportPathReports = "\\Reports\\";
        private const string s_exportPathSelectWindows = "\\SelectWindows\\";

        private static NH.INHibernateRepository GenerateRepository()
        {
            return new NH.Repository("ADBuffer");
        }

        private static void ExportSingleTable(string dirPath, string tableName)
        {
            using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + "\\" + tableName + ".xml"))
            {
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM " + tableName), tableName);
            }
        }
        /// <summary>
        /// 把现有数据库中的所有配置保存到数据文件中(多个文件)
        /// </summary>
        /// <param name="dirPath"></param>
        public static void ExportAllToPath(string dirPath)
        {
            Feng.Utils.IOHelper.TryCreateDirectory(dirPath + s_exportPathOthers);
            Feng.Utils.IOHelper.TryCreateDirectory(dirPath + s_exportPathMenus);
            Feng.Utils.IOHelper.TryCreateDirectory(dirPath + s_exportPathWindows);
            Feng.Utils.IOHelper.TryCreateDirectory(dirPath + s_exportPathReports);
            Feng.Utils.IOHelper.TryCreateDirectory(dirPath + s_exportPathSelectWindows);

            // Others
            using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + s_exportPathOthers + "\\AD_Alerts.xml"))
            {
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_AlertRule"), "AD_AlertRule");
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Task"), "AD_Task");
            }

            using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + s_exportPathOthers + "\\AD_MultiOrgs.xml"))
            {
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Client"), "AD_Client");
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM Ad_Org"), "Ad_Org");
            }

            using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + s_exportPathOthers + "\\AD_Buffers.xml"))
            {
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_EntityBuffer"), "AD_EntityBuffer");
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_NameValueMapping"), "AD_NameValueMapping");
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_SimpleParam"), "AD_SimpleParam");
                ew.WriteExcelXml(Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Param_Creator"), "AD_Param_Creator");
                
            }

            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Search_Custom");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_EventProcess");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Server_TaskSchedule");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Process");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Web_Service");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Command_Binding");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Plugin");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Resource");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Resource");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Resource");
            ExportSingleTable(dirPath + s_exportPathOthers, "AD_Resource");

            // Select Window
            System.Data.DataTable dt = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM Ad_Window_Select");
            foreach (System.Data.DataRow row in dt.Rows)
            {
                using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + s_exportPathSelectWindows + "\\AD_WindowSelect_" + row["Name"].ToString() + ".xml"))
                {
                    System.Data.DataTable dt2 = DbHelper.Instance.ExecuteDataTable("SELECT * FROM Ad_Window_Select WHERE NAME = '" + row["Name"].ToString() + "'");
                    ew.WriteExcelXml(dt2, "Ad_Window_Select");

                    WriteWindowForm(ew, row["Window"].ToString(), row["Form"].ToString());
                }
            }


            // Menus(main)
            IList<MenuInfo> menus;
            using (var rep = GenerateRepository())
            {
                menus = rep.List<MenuInfo>(NHibernate.Criterion.DetachedCriteria.For<MenuInfo>()
                .Add(NHibernate.Criterion.Expression.IsNotNull("ParentMenu")));
            }

            foreach (MenuInfo menuInfo in menus)
            {
                using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + s_exportPathMenus + "\\AD_Menu_" + menuInfo.Name + ".xml"))
                {
                    string sql = "SELECT * FROM AD_Menu WHERE Name = '" + menuInfo.Name + "'";
                    dt = DbHelper.Instance.ExecuteDataTable(sql);
                    string parentName = (string)dt.Rows[0]["ParentMenu"];
                    ew.WriteExcelXml(dt, "AD_Menu");
                    while (true)
                    {
                        string sql2 = "SELECT * FROM AD_Menu WHERE Name = '" + parentName + "'";
                        System.Data.DataTable dtt = DbHelper.Instance.ExecuteDataTable(sql2);

                        if (dtt.Rows[0]["ParentMenu"] == System.DBNull.Value)
                            break;
                        parentName = (string)dtt.Rows[0]["ParentMenu"];
                        ew.WriteExcelXml(dtt, "AD_Menu");
                    }

                    if (menuInfo.Action != null)
                    {
                        dt = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Action WHERE Name = '" + menuInfo.Action.Name + "'");
                        ew.WriteExcelXml(dt, "AD_Action");

                        if (dt.Rows[0]["ActionType"].ToString() == "2")
                        {
                            System.Data.DataTable dt3 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Process WHERE Name = '" + dt.Rows[0]["Process"].ToString() + "'");
                            ew.WriteExcelXml(dt3, "AD_Process");
                        }
                        else
                        {
                            WriteWindowForm(ew, dt.Rows[0]["Window"].ToString(), dt.Rows[0]["Form"].ToString());
                        }
                    }
                }
            }

            // Windows
            IList<WindowInfo> windows;
            using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository(typeof(WindowInfo)))
            {
                windows = rep.List<WindowInfo>();
            }
            foreach (WindowInfo windowInfo in windows)
            {
                using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + s_exportPathWindows + "\\AD_Window_" + windowInfo.Name + ".xml"))
                {
                    WriteWindowForm(ew, windowInfo.Name, null);
                }
            }

            // Report
            IList<ReportInfo> reports;
            using (IRepository rep = GenerateRepository())
            {
                reports = rep.List<ReportInfo>();
            }
            foreach (ReportInfo reportInfo in reports)
            {
                using (ExcelXmlWriter ew = new ExcelXmlWriter(dirPath + s_exportPathReports + "\\AD_Report_" + reportInfo.Name + ".xml"))
                {
                    WriteReport(ew, reportInfo.Name);
                }
            }
        }

        private static void WriteReport(ExcelXmlWriter ew, string reportName)
        {
            // Report Form
            System.Data.DataTable dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Report WHERE Name = '" + reportName + "'");
            ew.WriteExcelXml(dt4, "AD_Report");

            if (dt4.Rows.Count > 0)
            {
                System.Data.DataRow row = dt4.Rows[0];
                System.Data.DataTable dt6 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Report_Data WHERE Report = '" + row["Name"].ToString() + "'");
                ew.WriteExcelXml(dt6, "AD_Report_Data");

                WriteGrid(ew, dt6);
            }
        }

        private static void WriteWindowForm(ExcelXmlWriter ew, string windowName, string formName)
        {
            System.Data.DataTable dt2 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Window WHERE Name = '" + windowName + "'");
            ew.WriteExcelXml(dt2, "AD_Window");
            System.Data.DataTable dt3 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Form WHERE Name = '" + formName + "'");
            ew.WriteExcelXml(dt3, "AD_Form");

            if (dt2.Rows.Count > 0)
            {
                dt3 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Window_Tab WHERE Window = '" + dt2.Rows[0]["Name"].ToString() + "'");
                ew.WriteExcelXml(dt3, "AD_Window_Tab");

                System.Data.DataTable dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Window_Menu WHERE Window = '" + dt2.Rows[0]["Name"].ToString() + "'");
                ew.WriteExcelXml(dt4, "AD_Window_Menu");

                foreach (System.Data.DataRow row in dt4.Rows)
                {
                    System.Data.DataTable dt6 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Process WHERE Name = '" + row["ExecuteParam"].ToString() + "'");
                    ew.WriteExcelXml(dt6, "AD_Process");
                }

                if (dt2.Rows[0]["WindowType"].ToString() == "5")
                {
                    WriteReport(ew, dt2.Rows[0]["Name"].ToString());
                }
                else
                {
                    WriteGrid(ew, dt3);
                }

                WriteWindowForm(ew, dt2.Rows[0]["DetailWindow"].ToString(), dt2.Rows[0]["DetailForm"].ToString());
            }
        }

        private static void WriteGrid(ExcelXmlWriter ew, System.Data.DataTable dt3)
        {
            System.Data.DataTable dt4 = null;
            foreach (System.Data.DataRow row in dt3.Rows)
            {
                dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid WHERE GridName = '" + row["GridName"].ToString() + "'");
                ew.WriteExcelXml(dt4, "AD_Grid");
            }

            System.Data.DataTable dt5;

            foreach (System.Data.DataRow row in dt3.Rows)
            {
                dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Column WHERE GridName = '" + row["GridName"].ToString() + "'");
                ew.WriteExcelXml(dt4, "AD_Grid_Column");

                foreach (System.Data.DataRow row1 in dt4.Rows)
                {
                    string gridColumnName = null;
                    if (row1["GridColumnName"] != System.DBNull.Value)
                        gridColumnName = row1["GridColumnName"].ToString();
                    else if (row1["Navigator"] != System.DBNull.Value)
                        gridColumnName = row1["Navigator"].ToString() + "." + row1["PropertyName"].ToString();
                    else
                        gridColumnName = row1["PropertyName"].ToString();
                    dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Column_Warning WHERE GroupName = '" + gridColumnName + "'");
                    ew.WriteExcelXml(dt5, "AD_Grid_Column_Warning");
                }
            }

            foreach (System.Data.DataRow row in dt3.Rows)
            {
                dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Row WHERE GridName = '" + row["GridName"].ToString() + "'");
                ew.WriteExcelXml(dt5, "AD_Grid_Row");
            }

            foreach (System.Data.DataRow row in dt3.Rows)
            {
                dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Cell WHERE GridName = '" + row["GridName"].ToString() + "'");
                ew.WriteExcelXml(dt5, "AD_Grid_Cell");
            }

            foreach (System.Data.DataRow row in dt3.Rows)
            {
                dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Related WHERE GridName = '" + row["GridName"].ToString() + "'");
                ew.WriteExcelXml(dt5, "AD_Grid_Related");
            }

            foreach (System.Data.DataRow row in dt3.Rows)
            {
                dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Group WHERE GridName = '" + row["GridName"].ToString() + "'");
                ew.WriteExcelXml(dt5, "AD_Grid_Group");
            }

            foreach (System.Data.DataRow row in dt3.Rows)
            {
                dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Filter WHERE GridName = '" + row["GridName"].ToString() + "'");
                ew.WriteExcelXml(dt5, "AD_Grid_Filter");
            }
        }

        //private static void WriteWindowForm(System.IO.StreamWriter sw, string windowName, string formName)
        //{
        //    System.Data.DataTable dt2 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Window WHERE Name = '" + windowName + "'");
        //    ExcelXmlHelper.WriteExcelXml(dt2, sw, "AD_Window");
        //    System.Data.DataTable dt3 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Form WHERE Name = '" + formName + "'");
        //    ExcelXmlHelper.WriteExcelXml(dt3, sw, "AD_Form");

        //    if (dt2.Rows.Count > 0)
        //    {
        //        dt3 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Window_Tab WHERE Window = '" + dt2.Rows[0]["Name"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXml(dt3, sw, "AD_Window_Tab");

        //        System.Data.DataTable dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Window_Menu WHERE Window = '" + dt2.Rows[0]["Name"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXml(dt4, sw, "AD_Window_Menu");

        //        ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Process");
        //        foreach (System.Data.DataRow row in dt4.Rows)
        //        {
        //            System.Data.DataTable dt6 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Process WHERE Name = '" + row["ExecuteParam"].ToString() + "'");
        //            ExcelXmlHelper.WriteExcelXmlRows(dt6, sw, row == dt4.Rows[0]);
        //        }
        //        ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //        if (dt2.Rows[0]["WindowType"].ToString() == "5")
        //        {
        //            // Report Form
        //            dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Report WHERE Name = '" + dt2.Rows[0]["Name"].ToString() + "'");
        //            ExcelXmlHelper.WriteExcelXml(dt4, sw, "AD_Report");

        //            ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Report_Data");
        //            // foreach (System.Data.DataRow row in dt4.Rows)    // only on AD_Report
        //            //{
        //            System.Data.DataRow row = dt4.Rows[0];
        //            System.Data.DataTable dt6 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Report_Data WHERE Report = '" + row["Name"].ToString() + "'");
        //            ExcelXmlHelper.WriteExcelXmlRows(dt6, sw, row == dt4.Rows[0]);
        //            //}
        //            ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //            WriteGrid(sw, dt6); 
        //        }
        //        else
        //        {
        //            WriteGrid(sw, dt3);
        //        }
        //    }
        //}

        //private static void WriteGrid(System.IO.StreamWriter sw, System.Data.DataTable dt3)
        //{
        //    System.Data.DataTable dt4 = null;
        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid");
        //    foreach (System.Data.DataRow row in dt3.Rows)
        //    {
        //        dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid WHERE GridName = '" + row["GridName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt4, sw, row == dt3.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid_Column");
        //    foreach (System.Data.DataRow row in dt3.Rows)
        //    {
        //        dt4 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Column WHERE GridName = '" + row["GridName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt4, sw, row == dt3.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    System.Data.DataTable dt5;
        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid_Column_Warning");
        //    foreach (System.Data.DataRow row1 in dt4.Rows)
        //    {
        //        dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Column_Warning WHERE GroupName = '" + row1["PropertyName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt5, sw, row1 == dt4.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid_Row");
        //    foreach (System.Data.DataRow row in dt3.Rows)
        //    {
        //        dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Row WHERE GridName = '" + row["GridName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt5, sw, row == dt3.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid_Cell");
        //    foreach (System.Data.DataRow row in dt3.Rows)
        //    {
        //        dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Cell WHERE GridName = '" + row["GridName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt5, sw, row == dt3.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid_Related");
        //    foreach (System.Data.DataRow row in dt3.Rows)
        //    {
        //        dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Related WHERE GridName = '" + row["GridName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt5, sw, row == dt3.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid_Group");
        //    foreach (System.Data.DataRow row in dt3.Rows)
        //    {
        //        dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Group WHERE GridName = '" + row["GridName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt5, sw, row == dt3.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    ExcelXmlHelper.WriteExcelXmlTableHead(sw, "AD_Grid_Filter");
        //    foreach (System.Data.DataRow row in dt3.Rows)
        //    {
        //        dt5 = Feng.Data.DbHelper.Instance.ExecuteDataTable("SELECT * FROM AD_Grid_Filter WHERE GridName = '" + row["GridName"].ToString() + "'");
        //        ExcelXmlHelper.WriteExcelXmlRows(dt5, sw, row == dt3.Rows[0]);
        //    }
        //    ExcelXmlHelper.WriteExcelXmlTableTail(sw);

        //    // AD_Grid_Column_Warning
        //}
        #endregion

        #region "Import Setttings"
        private void ClearAll()
        {
            /* delete from AD_Grid
            delete from AD_Grid_Cell
            delete from AD_Grid_Column
            delete from AD_Grid_Column_Warning
            delete from AD_Grid_Filter
            delete from AD_Grid_Group
            delete from AD_Grid_Related
            delete from AD_Grid_Row

            delete from AD_NameValueMapping
            delete from AD_EntityBuffer
            delete from AD_SimpleParam
            delete from AD_Param_Creator
             * 
            delete from AD_Task
            delete from AD_AlertRule

            delete from AD_Report_Data
            delete from AD_Report

            delete from AD_Process

            delete from AD_Window_Tab
            delete from AD_Window_Menu
            delete from AD_Window_Select
            delete from AD_Window

            delete from AD_Search_Custom

            delete from AD_Form

            delete from AD_Action

            delete from AD_Menu_Property

            delete from AD_Menu

            delete from AD_Role*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void ImportFromXmlFile(string fileName)
        {
            ImportFromXmlFile(fileName, false);
        }

        private const string s_adPreTableName = "AD_";
        /// <summary>
        /// 把给定文件的数据按照不同数据名文件名导入到数据库中
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tryImport"></param>
        public static void ImportFromXmlFile(string fileName, bool tryImport)
        {
            DisableFKConstraint(s_adPreTableName);

            IList<MyDbCommand> cmds = new List<MyDbCommand>();

            IList<System.Data.DataTable> list = ExcelXmlHelper.ReadExcelXml(fileName, true);
            foreach (System.Data.DataTable i in list)
            {
                foreach (System.Data.DataRow row in i.Rows)
                {
                    System.Data.Common.DbCommand sqlSelect = GenerateSelectCountCommand(i.TableName, row);
                    System.Data.Common.DbCommand sqlInsert = GenerateInsertCommand(i.TableName, row);
                    System.Data.Common.DbCommand sqlUpdate = GenerateUpdateCommand(i.TableName, row);

                    StringBuilder sb = new StringBuilder();
                    sb.Append("IF ((");
                    sb.Append(sqlSelect.CommandText);
                    sb.Append(") = 1) BEGIN ");
                    sb.Append(sqlUpdate.CommandText);
                    sb.Append(" END ");
                    sb.Append(" ELSE BEGIN ");
                    sb.Append(sqlInsert.CommandText);
                    sb.Append(" END ");

                    System.Data.Common.DbCommand sql = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());
                    foreach (System.Data.Common.DbParameter para in sqlSelect.Parameters)
                    {
                        System.Data.Common.DbParameter parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                        parameter.ParameterName = para.ParameterName;
                        parameter.Value = para.Value;
                        sql.Parameters.Add(parameter);
                    }
                    foreach (System.Data.Common.DbParameter para in sqlInsert.Parameters)
                    {
                        System.Data.Common.DbParameter parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                        parameter.ParameterName = para.ParameterName;
                        parameter.Value = para.Value;
                        sql.Parameters.Add(parameter);
                    }
                    foreach (System.Data.Common.DbParameter para in sqlUpdate.Parameters)
                    {
                        System.Data.Common.DbParameter parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                        parameter.ParameterName = para.ParameterName;
                        parameter.Value = para.Value;
                        sql.Parameters.Add(parameter);
                    }

                    cmds.Add(new MyDbCommand(sql, ExpectedResultTypes.Special, "1"));
                    //System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(sql);
                    //if (dt.Rows[0][0].ToString() == "0")
                    //{
                    //    sql = GenerateInsertCommand(i.TableName, row);
                    //    cmds.Add(new MyDbCommand(sql, ExpectedResultTypes.Special, "1"));
                    //}
                    //else
                    //{
                    //    sql = GenerateUpdateCommand(i.TableName, row);
                    //    cmds.Add(new MyDbCommand(sql, ExpectedResultTypes.Special, "1"));
                    //}

                    if (tryImport)
                        break;
                }
            }
            DbHelper.Instance.UpdateTransaction(cmds);

            EnableFKConstraint(s_adPreTableName);
        }

        /// <summary>
        /// 按照文件内容，通过主键删除数据
        /// </summary>
        /// <param name="fileName"></param>
        public static void DeleteFromXmlFile(string fileName)
        {
            DisableFKConstraint(s_adPreTableName);

            IList<MyDbCommand> cmds = new List<MyDbCommand>();

            IList<System.Data.DataTable> list = ExcelXmlHelper.ReadExcelXml(fileName, true);
            foreach (System.Data.DataTable i in list)
            {
                foreach (System.Data.DataRow row in i.Rows)
                {
                    System.Data.Common.DbCommand sql = GenerateDeleteCommand(i.TableName, row);
                    System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(sql);
                }
            }
            DbHelper.Instance.UpdateTransaction(cmds);

            EnableFKConstraint(s_adPreTableName);
        }

        /// <summary>
        /// 把给定目录下的数据按照不同数据名文件名导入到数据库中
        /// </summary>
        /// <param name="dirPath"></param>
        public static void ImportFromPath(string dirPath)
        {
            string[] files = System.IO.Directory.GetFiles(dirPath, "*.xml");
            foreach (string file in files)
            {
                ImportFromXmlFile(file);
            }
        }

        private const string s_idName = "Name";
        private static string GetDefaultIdName(System.Data.DataTable dt)
        {
            string idName = s_idName;
            if (!dt.Columns.Contains(idName))
            {
                idName = dt.Columns[0].ColumnName;
            }
            return idName;
        }

        private static System.Data.Common.DbCommand GenerateDeleteCommand(string tableName, System.Data.DataRow row)
        {
            string idName = GetDefaultIdName(row.Table);

            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ");
            sb.Append(tableName);
            sb.Append(" WHERE " + idName + " = @DELETE" + idName);

            System.Data.Common.DbCommand cmd = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());

            System.Data.Common.DbParameter parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
            parameter.ParameterName = "@DELETE" + idName;
            parameter.Value = row[idName];
            cmd.Parameters.Add(parameter);

            return cmd;
        }

        private static System.Data.Common.DbCommand GenerateSelectCountCommand(string tableName, System.Data.DataRow row)
        {
            string idName = GetDefaultIdName(row.Table);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) FROM ");
            sb.Append(tableName);
            sb.Append(" WHERE " + idName + " = @SELECT" + idName);

            System.Data.Common.DbCommand cmd = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());

            System.Data.Common.DbParameter parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
            parameter.ParameterName = "@SELECT" + idName;
            parameter.Value = row[idName];
            cmd.Parameters.Add(parameter);

            return cmd;
        }

        private static System.Data.Common.DbCommand GenerateUpdateCommand(string tableName, System.Data.DataRow row)
        {
            string idName = GetDefaultIdName(row.Table);

            StringBuilder prefix = new StringBuilder();
            prefix.Append("UPDATE ");
            prefix.Append(tableName);
            prefix.Append(" SET ");

            StringBuilder sb = new StringBuilder();
            sb.Remove(0, sb.Length);
            sb.Append(prefix);

            System.Data.Common.DbParameter parameter;
            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (column.ColumnName == idName)
                    continue;
                string s1 = column.ColumnName;
                string s2 = "@UPDATE" + i.ToString();
                sb.Append("[" + s1 + "]" + "= " +  s2);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);

            sb.Append(" WHERE " + idName + " = @UPDATE" + idName);

            System.Data.Common.DbCommand cmd = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());
            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                if (column.ColumnName == idName)
                    continue;
                string s1 = column.ColumnName;
                string s2 = "@UPDATE" + i.ToString();

                parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                parameter.ParameterName = s2;
                parameter.Value = row[column.ColumnName];
                cmd.Parameters.Add(parameter);
            }
            parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
            parameter.ParameterName = "@UPDATE" + idName;
            parameter.Value = row[idName];
            cmd.Parameters.Add(parameter);

            return cmd;
        }

        private static System.Data.Common.DbCommand GenerateInsertCommand(string tableName, System.Data.DataRow row)
        {
            StringBuilder prefix = new StringBuilder();
            prefix.Append("INSERT INTO ");
            prefix.Append(tableName + " (");
            
            StringBuilder sb = new StringBuilder();
            sb.Remove(0, sb.Length);
            sb.Append(prefix);

            System.Data.Common.DbParameter parameter;
            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];
                sb.Append("[" + column.ColumnName + "],");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") VALUES (");
            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];

                sb.Append("@INSERT" + i.ToString());
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");

            System.Data.Common.DbCommand cmd = DbHelper.Instance.Database.GetSqlStringCommand(sb.ToString());

            for (int i = 0; i < row.Table.Columns.Count; ++i)
            {
                System.Data.DataColumn column = row.Table.Columns[i];

                parameter = DbHelper.Instance.Database.DbProviderFactory.CreateParameter();
                parameter.ParameterName = "@INSERT" + i.ToString();
                parameter.Value = row[column.ColumnName];
                cmd.Parameters.Add(parameter);
            }

            return cmd;
        }

        /// <summary>
        /// Disable 所有外键
        /// </summary>
        public static void DisableFKConstraint()
        {
            DisableFKConstraint(null);
        }

        /// <summary>
        /// Enable 所有外键
        /// </summary>
        public static void EnableFKConstraint()
        {
            EnableFKConstraint(null);
        }

        /// <summary>
        /// Disable 以preTableName开头的表的 外键
        /// </summary>
        /// <param name="preTableName"></param>
        public static void DisableFKConstraint(string preTableName)
        {
            // in SQLite, there is no table sysobjects
            try
            {
                string s = "select  'ALTER TABLE ['  + b.name +  '] NOCHECK CONSTRAINT ' +  a.name +';' as  禁用约束 " +
                            " from  sysobjects  a ,sysobjects  b " +
                            " where  a.xtype ='f' and  a.parent_obj = b.id";
                if (!string.IsNullOrEmpty(preTableName))
                {
                    s += " and substring(b.name, 1, 3) = '" + preTableName + "'";
                }
                System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(s);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    DbHelper.Instance.ExecuteNonQuery(row[0].ToString());
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Enable 外键
        /// </summary>
        public static void EnableFKConstraint(string preTableName)
        {
            try
            {
                string s = "select  'ALTER TABLE ['  + b.name +  '] CHECK  CONSTRAINT ' +  a.name +';' as  禁用约束 " +
                            " from  sysobjects  a ,sysobjects  b " +
                            " where  a.xtype ='f' and  a.parent_obj = b.id";
                if (!string.IsNullOrEmpty(preTableName))
                {
                    s += " and substring(b.name, 1, 3) = '" + preTableName + "'";
                }
                System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(s);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    DbHelper.Instance.ExecuteNonQuery(row[0].ToString());
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region "TaskSchedule"
        ///// <summary>
        ///// 按照系统配置信息重新创建任务
        ///// </summary>
        //public void RecreateScheduleTasks()
        //{
        //    string taskNamePre = SystemConfiguration.ApplicationName + " - ";

        //    ScheduledTasks st = new ScheduledTasks();
        //    foreach (string taskName in st.GetTaskNames())
        //    {
        //        if (taskName.StartsWith(taskNamePre))
        //        {
        //            st.DeleteTask(taskName);
        //        }
        //    }

        //    IList<ServerTaskScheduleInfo> list = ADInfoBll.Instance.GetTaskScheduleInfo();
        //    foreach (ServerTaskScheduleInfo i in list)
        //    {
        //        Task t = st.CreateTask(taskNamePre + i.Name);
        //        t.ApplicationName = i.ApplicationName;
        //        t.Parameters = i.Parameters;
        //        t.WorkingDirectory = i.WorkingDirectory;
        //        t.Creator = taskNamePre + "admin";
        //        t.SetAccountInformation(string.Empty, (string)null);    // local system account
        //        t.IdleWaitDeadlineMinutes = 20;
        //        t.IdleWaitMinutes = 10;
        //        t.MaxRunTime = new TimeSpan(1, 0, 0);
        //        t.Priority = System.Diagnostics.ProcessPriorityClass.High;

        //        switch (i.TaskScheduleType)
        //        {
        //            case TaskScheduleType.Daily:
        //                t.Triggers.Add(new DailyTrigger((short)i.RunTime.Hour, (short)i.RunTime.Minute, (short)i.RunTime.Second));
        //                break;
        //            default:
        //                throw new NotSupportedException("Invalid TaskScheduleType!");
        //        }
        //    }
        //}
        #endregion
    }
}
