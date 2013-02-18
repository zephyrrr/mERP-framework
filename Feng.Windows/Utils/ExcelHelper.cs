using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public enum ExcelFileType
    {
        /// <summary>
        /// 
        /// </summary>
        Xlsx = 1,
        /// <summary>
        /// 
        /// </summary>
        Xls = 2,
        /// <summary>
        /// 
        /// </summary>
        Xml = 3
    }

    /// <summary>
    /// Excel读写帮助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 自动根据文件扩展名和文件格式判断excel文件类型
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="fileName"></param>
        /// <param name="firstRowasColumnName"></param>
        public static void WriteExcel(IList<DataTable> dts, string fileName, bool firstRowasColumnName)
        {
            int k = fileName.LastIndexOf('.');
            string excelType = fileName.Substring(k);

            if (excelType != null)
            {
                if (excelType == ".xml")
                {
                    WriteExcel(dts, fileName, ExcelFileType.Xml, firstRowasColumnName);
                }
                else if (excelType == ".xlsx")
                {
                    WriteExcel(dts, fileName, ExcelFileType.Xlsx, firstRowasColumnName);
                }
                else if (excelType == ".xls")
                {
                    WriteExcel(dts, fileName, ExcelFileType.Xls, firstRowasColumnName);
                }
                else
                {
                    throw new NotSupportedException("Not supported file format!");
                }
            }
            //throw new NotSupportedException("Not supported file format!");
        }

        /// <summary>
        /// 把DataTables写入Excel（xml格式和xls格式）
        /// 每个DataTable一个Sheet，Sheet.Name = DataTable.TableName。
        /// 类型要一致（例如DataTable是DateTime类型，Excel中列也要是时间类型）
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="fileName"></param>
        /// <param name="excelType"></param>
        /// <param name="firstRowasColumnName">第一列是否Column名字。</param>
        public static void WriteExcel(IList<DataTable> dts, string fileName, ExcelFileType excelType, bool firstRowasColumnName)
        {
            switch (excelType)
            {
                case ExcelFileType.Xml:

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
                    {

                        ExcelXmlHelper.WriteExcelXmlHead(sw);

                        foreach (DataTable dt in dts)
                        {
                            ExcelXmlHelper.WriteExcelXmlTableHead(sw, dt.TableName);

                            ExcelXmlHelper.WriteExcelXmlRows(dt, sw, firstRowasColumnName);

                            ExcelXmlHelper.WriteExcelXmlTableTail(sw);
                        }

                        ExcelXmlHelper.WriteExcelXmlTail(sw);

                    }
                    break;
                case ExcelFileType.Xls:
                case ExcelFileType.Xlsx:
                    foreach (DataTable dt in dts)
                    {
                        Excel.OleHelper.WriteExcel(fileName, dt, excelType, firstRowasColumnName);
                    }
                    break;
            }
        }

        /// <summary>
        /// 读入Excel文件。一个Sheet一个DataTable。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IList<DataTable> ReadExcel(string fileName)
        {
            return ReadExcel(fileName, true);
        }

        /// <summary>
        /// 读入Excel文件。一个Sheet一个DataTable。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="firstRowasColumnName">第一行是不是座位DataTable的ColumnName</param>
        /// <returns></returns>
        public static IList<DataTable> ReadExcel(string fileName, bool firstRowasColumnName)
        {
            int k = fileName.LastIndexOf('.');
            string excelType = fileName.Substring(k);

            if (excelType != null)
            {
                if (excelType == ".xml")
                {
                    return ExcelXmlHelper.ReadExcelXml(fileName, firstRowasColumnName);
                }
                else if (excelType == ".xlsx")
                {
                    return ReadExcel(fileName, ExcelFileType.Xlsx, firstRowasColumnName);
                }
                else if (excelType == ".xls")
                {
                    return ReadExcel(fileName, ExcelFileType.Xls, firstRowasColumnName);
                }
                else
                {
                    throw new NotSupportedException("Not supported file format!");
                }
            }
            throw new NotSupportedException("Not supported file format!");
        }

        /// <summary>
        /// 读入Excel文件。一个Sheet一个DataTable。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="excelType"></param>
        /// <param name="firstRowasColumnName"></param>
        /// <returns></returns>
        public static IList<DataTable> ReadExcel(string fileName, ExcelFileType excelType, bool firstRowasColumnName)
        {
            return Excel.OleHelper.ReadExcel(fileName, excelType, firstRowasColumnName);
        }

        /// <summary>
        /// 转换格式
        /// </summary>
        /// <param name="srcFileName"></param>
        /// <param name="destFileName"></param>
        /// <param name="firstRowasColumnName">当数据很复杂时，不能方便的转换为DataTable时，firstRowasColumnName设置为false。此时转化后的Excel有头F1，F2...</param>
        public static void ConvertExcel(string srcFileName, string destFileName, bool firstRowasColumnName)
        {
            IList<DataTable> ldt = ReadExcel(srcFileName, firstRowasColumnName);
            WriteExcel(ldt, destFileName, firstRowasColumnName);
        }

        ///// <summary>
        ///// 转换格式
        ///// </summary>
        ///// <param name="srcFileName"></param>
        ///// <param name="destFileName"></param>
        ///// <param name="srcType"></param>
        ///// <param name="destType"></param>
        //public static void ConvertExcel(string srcFileName, string destFileName, ExcelFileType srcType, ExcelFileType destType)
        //{
        //    IList<DataTable> ldt = ReadExcel(srcFileName, srcType, false);
        //    WriteExcel(ldt, destFileName, destType, false);
        //}

        
    }
}
