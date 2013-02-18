using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Text;
using System.Collections.Generic;

namespace Feng.Windows.Utils.Excel
{
    /// <summary>
    /// 
    /// </summary>
    internal class OleHelper
    {
        #region 共有方法
        /// <summary>
        /// 获取Connection
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="FileType"></param>
        /// <param name="firstRowasColumnName"></param>
        private static OleDbConnection GetConnection(string FileName, ExcelFileType FileType, bool firstRowasColumnName)
        {
            string conStr = "";
            switch (FileType)
            {
                case ExcelFileType.Xls: //Excel 97-03
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    break;
                case ExcelFileType.Xlsx: //Excel 07
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1}'";
                    break;
            }
            conStr = String.Format(conStr, FileName, firstRowasColumnName ? "Yes" : "No");
            return new OleDbConnection(conStr);
        }

        ///// <summary>
        ///// 实例化一个执行T-Sql语句的SqlCommand对象实例
        ///// </summary>
        ///// <param name="que">要执行的SQL语句</param>
        ///// <param name="FileName"></param>
        ///// <param name="FileType"></param>
        ///// <returns></returns>
        //private static OleDbCommand BuildCommand(string que, string FileName, ExcelFileType FileType)
        //{
        //    OleDbConnection conn = GetConnection(FileName, FileType);
        //    try
        //    {
        //        if (conn.State == ConnectionState.Closed)
        //            conn.Open();

        //        OleDbCommand cmd = new OleDbCommand(que, conn);
        //        return cmd;
        //    }
        //    catch (OleDbException EX)
        //    {
        //        throw EX;
        //    }

        //}


        ///// <summary>
        ///// 关闭连接和CMD命令并清理资源
        ///// </summary>
        //private static void CloseConn()
        //{
        //    if (conn != null)
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //            conn.Dispose();
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// 执行一个T-SQL查询，返回DataTable对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="firstRowasColumnName"></param>
        /// <returns></returns>
        public static IList<DataTable> ReadExcel(string fileName, ExcelFileType fileType, bool firstRowasColumnName)
        {
            OleDbConnection conn = GetConnection(fileName, fileType, firstRowasColumnName);

            OleDbCommand cmdExcel = new OleDbCommand();

            OleDbDataAdapter oda = new OleDbDataAdapter();

            // dt = new DataTable();

            cmdExcel.Connection = conn;

            //Get the name of  Sheet 
            conn.Open();

            DataTable dtExcelSchema;

            dtExcelSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });//(OleDbSchemaGuid.Tables, null);

            IList<string> list = new List<string>();
            for (int i = 0; i <= dtExcelSchema.Rows.Count - 1; i++)
            {
                list.Add(dtExcelSchema.Rows[i]["TABLE_NAME"].ToString());
            }
            //  string SheetName = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();

            conn.Close();

            IList<DataTable> listable = new List<DataTable>();
            //Read Data from  Sheet 
            for(int i=0; i<list.Count; ++i)
            {
                string sheetName = list[i];
                // 当中文时，可能会有''，例如'十二月$'
                sheetName = sheetName.Replace("'", "").Replace("\"", "");
                if (sheetName.EndsWith("$"))
                {
                    conn.Open();

                    DataTable dtb = new DataTable(sheetName.Replace("$","").ToString());

                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";

                    oda.SelectCommand = cmdExcel;

                    oda.Fill(dtb);

                    listable.Add(dtb);

                    conn.Close();
                }
            }

            return listable;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sourceTable"></param>
        private static void Table2Excel(OleDbConnection conn, DataTable sourceTable)
        {
            string sheetName = sourceTable.TableName;
            using (OleDbDataAdapter da = new OleDbDataAdapter(string.Format("SELECT * FROM {0}", sheetName), conn))
            {
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
                DataTable dataTable = new DataTable(sheetName);
                //读取表单（空表）
                da.Fill(dataTable);

                //为空表写数据
                foreach (DataRow sRow in sourceTable.Rows)
                {
                    DataRow nRow = dataTable.NewRow();
                    for (int i = 0; i < sourceTable.Columns.Count; i++)
                    {

                        if (sRow[i] is System.Byte[])
                            nRow[i] = "二进制数据";
                        else
                            nRow[i] = sRow[i];
                    }
                    dataTable.Rows.Add(nRow);
                }
                //更新表单
                da.Update(dataTable);
            }
        }

        

        /// <summary>
        /// 将内存中的DataTable转成Excel
        /// </summary>
        /// <param name="fileName">Excel保存路径</param>
        /// <param name="sourceTable">内存中的DataTable</param>
        /// <param name="excelType"></param>
        /// <param name="firstRowasColumnName">firstRowasColumnName</param>
        public static void WriteExcel(string fileName, DataTable sourceTable, ExcelFileType excelType, bool firstRowasColumnName)
        {
            string sheetName = sourceTable.TableName;
            //  // 必须要firstRowasColumnName=true，否则不能Insert
            using (OleDbConnection conn = GetConnection(fileName, excelType, true))
            {
                conn.Open();
                //检查表单是否已存在
                string[] sheets = GetSheets(conn);
                for (int i = 0; i < sheets.Length; i++)
                {
                    if (sheets[i].Equals(sheetName, StringComparison.OrdinalIgnoreCase))
                    {
                        //如果存在，删除
                        using (OleDbCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = string.Format("DROP TABLE {0} ", sheetName);
                            cmd.ExecuteNonQuery();
                        }
                        break;
                    }
                }
                //创建表单
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("CREATE TABLE {0} ({1});", sheetName, BuildColumnsString(sourceTable));
                    cmd.ExecuteNonQuery();
                }
                using (OleDbCommand cmd = conn.CreateCommand())
                {
                    foreach (DataRow sRow in sourceTable.Rows)
                    {
                        string colName = string.Empty;
                        string colValue = string.Empty;
                        string colInsert = string.Empty;

                        for (int i = 0; i < sourceTable.Columns.Count; i++)
                        {
                            if (sRow[i].ToString().IndexOf('\'') > 0)
                            {
                                Table2Excel(conn, sourceTable);
                                return;
                            }

                            if (i > 0)
                            {
                                colName += ",";
                                colValue += ",";
                            }
                            colName += "[" + sourceTable.Columns[i].ColumnName.ToString() + "]";
                            colValue += sRow[i] == System.DBNull.Value ? "NULL" : "'" + sRow[i].ToString() + "'";
                        }

                        cmd.CommandText = string.Format("INSERT INTO {0}({1}) VALUES({2});", sheetName, colName, colValue);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        /// <summary>
        /// 从<paramref name="sourceTable"/>构建字段字符串
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <returns></returns>
        private static string BuildColumnsString(DataTable sourceTable)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sourceTable.Columns.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                string colName = "[" + sourceTable.Columns[i].ColumnName + "]";
                sb.Append(colName);
                sb.Append(" ");//为了避免系统关键字，将所有字段名后添加下划线
                sb.Append(SwitchToSqlType(sourceTable.Columns[i]));
                sb.Append(" NULL"); // 所有都可空
            }
            return sb.ToString();
        }



        /// <summary>
        /// 将<paramref name="column"/>的DataType转成数据库关键字
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string SwitchToSqlType(DataColumn column)
        {
            string TypeFullName = column.DataType.FullName;
            switch (TypeFullName)
            {
                case "System.String":
                case "System.Boolean":
                case "System.Char":
                case "System.NVarchar":
                case "System.Varchar":
                case "System.DBNull":
                    return "String";
                case "System.DateTime":
                    return "DateTime";
                case "System.Decimal":
                case "System.Double":
                    return "Decimal";
                case "System.Guid":
                    return "uniqueidentifier";
                default:
                    return "string";
            }
        }
        /// <summary>
        /// 获取EXCEL的所有表单
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static string[] GetSheets(OleDbConnection conn)
        {
            //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等
            DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

            //包含excel中表名的字符串数组
            string[] strTableNames = new string[dtSheetName.Rows.Count];
            for (int k = 0; k < dtSheetName.Rows.Count; k++)
            {
                DataRow row = dtSheetName.Rows[k];
                strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
            }
            return strTableNames;
        }
    }
}
