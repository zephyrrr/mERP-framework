using System;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelXmlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        public static void WriteExcelXmlHead(System.IO.StreamWriter sw)
        {
            //const string styleExcelXml = " <Styles>\r\n" +
            //       "  <Style ss:ID=\"Default\" ss:Name=\"Normal\"> " +
            //           " <Alignment ss:Vertical=\"Bottom\"/> " +
            //           " <Borders/> <Font/> <Interior/> <NumberFormat/> <Protection/> </Style>\r\n" +
            //       "  <Style ss:ID=\"BoldColumn\"> <Font x:Family=\"Swiss\" ss:Bold=\"1\"/> </Style>\r\n" +
            //       "  <Style ss:ID=\"xsString\">  <NumberFormat ss:Format=\"@\"/> </Style>\r\n" +
            //       "  <Style ss:ID=\"xsNumber\"> <NumberFormat ss:Format=\"0.0000\"/> </Style>\r\n" +
            //       "  <Style ss:ID=\"xsBoolean\"> <NumberFormat ss:Format=\"0\"/> </Style>\r\n" +
            //       "  <Style ss:ID=\"DateLiteral\"> <NumberFormat ss:Format=\"mm/dd/yyyy;@\"/> </Style>\r\n" +
            //     " </Styles>\r\n";

            sw.Write(m_startExcelXml);
            sw.Write(" <Styles>\r\n");
            sw.Write(m_styleHeader);
            sw.Write(m_styleBoolean);
            sw.Write(m_styleString);
            sw.Write(m_styleNumber);
            sw.Write(" </Styles>\r\n");
        }

        private const string m_startExcelXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<?mso-application progid=\"Excel.Sheet\"?>\r\n" +
                    "<Workbook xmlns:o=\"urn:schemas-microsoft-com:office:office\"" +
                    " xmlns:x=\"urn:schemas-microsoft-com:office:excel\"" +
                    " xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"" +
                    " xmlns:html=\"http://www.w3.org/TR/REC-html40\" xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n";

        private const string m_styleBoolean = @"<Style ss:ID=""xsBoolean"">
      <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" />
      <Borders>
        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
      </Borders>
      <Font ss:FontName=""宋体"" ss:Size=""9"" />
      <Interior ss:Color=""#FFFFFF"" ss:Pattern=""Solid"" />
    </Style>";

        private const string m_styleString = @"<Style ss:ID=""xsString"">
      <Alignment ss:Horizontal=""Left"" ss:Vertical=""Top"" />
      <Borders>
        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
      </Borders>
      <Font ss:FontName=""宋体"" ss:Size=""9"" />
      <Interior ss:Color=""#FFFFFF"" ss:Pattern=""Solid"" />
    </Style>";

        private const string m_styleNumber = @"<Style ss:ID=""xsNumber"">
      <Alignment ss:Horizontal=""Right"" ss:Vertical=""Top"" />
      <Borders>
        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
      </Borders>
      <Font ss:FontName=""宋体"" ss:Size=""9"" />
      <Interior ss:Color=""#FFFFFF"" ss:Pattern=""Solid"" />
    </Style>";

        private const string m_styleHeader = @"<Style ss:ID=""xsHeader"">
      <Borders>
        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
        <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""DarkGray"" />
      </Borders>
      <Font ss:FontName=""宋体"" ss:Size=""9"" ss:Color=""Black"" ss:Bold=""1"" />
      <Interior ss:Color=""White"" ss:Pattern=""Solid"" />
    </Style>";

        /// <summary>
        /// 写入Excel尾
        /// </summary>
        /// <param name="sw"></param>
        public static void WriteExcelXmlTail(System.IO.StreamWriter sw)
        {
            const string endExcelXML = "</Workbook>";
            sw.Write(endExcelXML);
        }

        /// <summary>
        /// 写入Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void WriteExcelXml(DataTable dt, string fileName)
        {
            WriteExcelXml(dt, fileName, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        public static void WriteExcelXml(DataTable dt, string fileName, string sheetName)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName);

            WriteExcelXmlHead(sw);

            WriteExcelXmlTableHead(sw, sheetName);

            WriteExcelXmlRows(dt, sw, true);

            WriteExcelXmlTableTail(sw);

            WriteExcelXmlTail(sw);

            sw.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="sheetName"></param>
        public static void WriteExcelXmlTableHead(System.IO.StreamWriter sw, string sheetName)
        {
            sw.Write(" <Worksheet ss:Name=\"" + (string.IsNullOrEmpty(sheetName) ? "Sheet1" : sheetName) + "\">\r\n");
            sw.Write("  <Table>\r\n");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sw"></param>
        public static void WriteExcelXmlTableTail(System.IO.StreamWriter sw)
        {
            sw.Write("  </Table>\r\n");
            sw.Write(" </Worksheet>\r\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sw"></param>
        /// <param name="sheetName"></param>
        public static void WriteExcelXml(DataTable dt, System.IO.StreamWriter sw, string sheetName)
        {
            WriteExcelXmlTableHead(sw, sheetName);

            WriteExcelXmlRows(dt, sw, true);

            WriteExcelXmlTableTail(sw);
        }


        private static string GetDataType(Type dataType)
        {
            if (dataType.IsGenericType && (dataType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                dataType = Nullable.GetUnderlyingType(dataType);
            }
            if ((((dataType == typeof(int)) || (dataType == typeof(double))) || ((dataType == typeof(decimal)) || (dataType == typeof(float)))) || ((((dataType == typeof(short)) || (dataType == typeof(float))) || ((dataType == typeof(ushort)) || (dataType == typeof(uint)))) || (((dataType == typeof(ulong)) || (dataType == typeof(short))) || (dataType == typeof(long)))))
            {
                return "Number";
            }
            if (dataType == typeof(bool))
            {
                return "Boolean";
            }
            return "String";
        }

        /// <summary>
        /// 写入Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sw"></param>
        /// <param name="writeHeader"></param>
        public static void WriteExcelXmlRows(DataTable dt, System.IO.StreamWriter sw, bool writeHeader)
        {
            ////if the number of rows is > 64000 create a new page to continue output
            //if (dt.Rows.Count >= 64000)
            //{
            //    throw new NotSupportedException("Row Count >= 64000");

            //    //rowCount = 0;
            //    //sheetCount++;
            //    //sw.Write("</Table>");
            //    //sw.Write(" </Worksheet>");
            //    //sw.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
            //    //sw.Write("<Table>");
            //}

            int rowCount = 0;
            // int sheetCount = 1;

            if (writeHeader)
            {
                sw.Write("   <ss:Row>\r\n");
                for (int x = 0; x < dt.Columns.Count; x++)
                {
                    sw.Write("    <ss:Cell ss:StyleID=\"xsHeader\"><ss:Data ss:Type=\"String\">");
                    sw.Write(dt.Columns[x].ColumnName);
                    sw.Write("</ss:Data></ss:Cell>\r\n");
                }
                sw.Write("   </ss:Row>\r\n");
            }
            foreach (DataRow x in dt.Rows)
            {
                rowCount++;

                sw.Write("   <ss:Row>\r\n"); //ID=" + rowCount + "
                for (int y = 0; y < dt.Columns.Count; y++)
                {
                    sw.Write("    ");
                    string dataType = GetDataType(x[y].GetType());
                    switch (dataType)
                    {
                        case "String":
                            string XMLstring = x[y].ToString();
                            XMLstring = XMLstring.Trim();
                            XMLstring = ReplaceKeyWords(XMLstring);
                            sw.Write("<ss:Cell ss:StyleID=\"xsString\">" +
                                           "<ss:Data ss:Type=\"String\">");
                            sw.Write(XMLstring);
                            sw.Write("</ss:Data></ss:Cell>\r\n");
                            break;
                        //case "System.DateTime":
                        //    //Excel has a specific Date Format of YYYY-MM-DD followed by  
                        //    //the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000
                        //    //The Following Code puts the date stored in XMLDate 
                        //    //to the format above
                        //    DateTime XMLDate = (DateTime)x[y];
                        //    string XMLDatetoString = ""; //Excel Converted Date
                        //    XMLDatetoString = XMLDate.Year.ToString() +
                        //         "-" +
                        //         (XMLDate.Month < 10 ? "0" +
                        //         XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
                        //         "-" +
                        //         (XMLDate.Day < 10 ? "0" +
                        //         XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
                        //         "T" +
                        //         (XMLDate.Hour < 10 ? "0" +
                        //         XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
                        //         ":" +
                        //         (XMLDate.Minute < 10 ? "0" +
                        //         XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
                        //         ":" +
                        //         (XMLDate.Second < 10 ? "0" +
                        //         XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
                        //         ".000";
                        //    sw.Write("<Cell ss:StyleID=\"DateLiteral\">" +
                        //                 "<Data ss:Type=\"DateTime\">");
                        //    sw.Write(XMLDatetoString);
                        //    sw.Write("</Data></Cell>\r\n");
                        //    break;
                        case "Boolean":
                            sw.Write("<ss:Cell ss:StyleID=\"xsBoolean\">" +
                                        "<ss:Data ss:Type=\"Boolean\">");
                            sw.Write(x[y].ToString().ToLower() == "true" ? "1" : "0");
                            sw.Write("</ss:Data></ss:Cell>\r\n");
                            break;
                        case "Number":
                            sw.Write("<ss:Cell ss:StyleID=\"xsNumber\">" +
                                    "<ss:Data ss:Type=\"Number\">");
                            sw.Write(x[y].ToString());
                            sw.Write("</ss:Data></ss:Cell>\r\n");
                            break;
                        default:
                            throw (new NotSupportedException(dataType.ToString() + " not handled."));
                    }
                }
                sw.Write("   </ss:Row>\r\n");
            }

        }

        private static string ReplaceKeyWords(string s)
        {
            s = s.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;");
            return s;
        }

        private static string AntiReplaceKeyWords(string s)
        {
            s = s.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&apos;", "'");
            return s;
        }

        /// <summary>
        /// 读入Excel
        /// </summary>
        /// <param name="fileName">Imported file</param>
        /// <param name="firstRowasColumnName"></param>
        /// <returns>dataTable result</returns>
        public static IList<DataTable> ReadExcelXml(string fileName, bool firstRowasColumnName = true)
        {
            IList<DataTable > ret = new List<DataTable>();

            XmlDocument xc = new XmlDocument();
            xc.Load(fileName);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xc.NameTable);
            nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            //XmlElement xe = (XmlElement)xc.DocumentElement.SelectSingleNode("//ss:Worksheet/ss:Table", nsmgr);
            //if (xe == null)
            //    return null;

            XmlNodeList xlTables = xc.DocumentElement.SelectNodes("//ss:Worksheet/ss:Table", nsmgr);
            foreach (XmlElement xiTable in xlTables)
            {
                DataTable dt = new DataTable();
                dt.TableName = xiTable.ParentNode.Attributes["ss:Name"].Value;
                ret.Add(dt);

                XmlNodeList xl = xiTable.SelectNodes("ss:Row", nsmgr);

                if (firstRowasColumnName)
                {
                    int Row = -1, Col = 0;
                    Dictionary<int, string> cols = new Dictionary<int, string>();
                    foreach (XmlElement xi in xl)
                    {
                        XmlNodeList xcells = xi.SelectNodes("ss:Cell", nsmgr);
                        Col = 0;
                        foreach (XmlElement xcell in xcells)
                        {
                            if (Row == -1)
                            {
                                // 不通过第一行添加Column，而是通过数据行添加
                                //dt.Columns.Add(xcell.InnerText);
                                cols[Col++] = xcell.InnerText;
                            }
                            else
                            {
                                if (xcell.Attributes["ss:Index"] != null)
                                {
                                    int idx = int.Parse(xcell.Attributes["ss:Index"].InnerText);
                                    Col = idx - 1;
                                }
                                string typeString = null;
                                XmlNode dataNode = xcell.SelectSingleNode("ss:Data", nsmgr);
                                if (dataNode != null && dataNode.Attributes["ss:Type"] != null)
                                {
                                    typeString = dataNode.Attributes["ss:Type"].InnerText;
                                }
                                SetCol(dt, Row, (string)cols[Col++], xcell.InnerText, GetTypeFromTypeString(typeString));
                            }
                        }
                        Row++;
                    }
                }
                else
                {
                    int Row = 0, Col = 0;
                    foreach (XmlElement xi in xl)
                    {
                        XmlNodeList xcells = xi.SelectNodes("ss:Cell", nsmgr);
                        Col = 0;
                        foreach (XmlElement xcell in xcells)
                        {
                            if (xcell.Attributes["ss:Index"] != null)
                            {
                                int idx = int.Parse(xcell.Attributes["ss:Index"].InnerText);
                                Col = idx - 1;
                            }
                            string typeString = null;
                            XmlNode dataNode = xcell.SelectSingleNode("ss:Data", nsmgr);
                            if (dataNode != null && dataNode.Attributes["ss:Type"] != null)
                            {
                                typeString = dataNode.Attributes["ss:Type"].InnerText;
                            }
                            string columnName = "F" + Col.ToString();
                            SetCol(dt, Row, columnName, xcell.InnerText, typeof(string));
                            Col++;
                        }
                        Row++;
                    }
                }
            }

            return ret;
        }

        private static Type GetTypeFromTypeString(string typeString)
        {
            if (string.IsNullOrEmpty(typeString))
                return typeof(string);
            switch (typeString.ToUpper())
            {
                case "STRING":
                    return typeof(string);
                case "DATETIME":
                    return typeof(DateTime);
                case "BOOLEAN":
                    return typeof(bool);
                case "NUMBER":
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Adds row to datatable, manages System.DBNull and so
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="AcceptChanges"></param>
        /// <returns></returns>
        private static int AddRow(DataTable dt, bool AcceptChanges)
        {
            object[] Values = new object[dt.Columns.Count];
            for (int Column = 0; Column < dt.Columns.Count; Column++)
            {
                if (!dt.Columns[Column].AllowDBNull)
                {
                    if (dt.Columns[Column].DefaultValue != null &&
                        dt.Columns[Column].DefaultValue != System.DBNull.Value)
                    {
                        Values[Column] = dt.Columns[Column].DefaultValue;
                    }
                }
            }
            dt.Rows.Add(Values);
            if (AcceptChanges)
            {
                dt.AcceptChanges();
            }
            return dt.Rows.Count - 1;
        }

        /// <summary>
        /// Sets data into datatable in safe manner of row index
        /// </summary>
        /// <param name="dt">DataTable to set</param>
        /// <param name="Row">Ordinal row index</param>
        /// <param name="ColumnName">name of column to set</param>
        /// <param name="Value">non/typed value to set</param>
        /// <param name="TypeOfValue">Becase Value can be null we must know datatype to manage default values</param>
        /// <returns></returns>
        private static DataColumn SetCol(DataTable dt, int Row, string ColumnName,
                                        object Value, System.Type TypeOfValue)
        {
            if (dt == null || string.IsNullOrEmpty(ColumnName))
                return null;

            if (Value == null)
                Value = System.DBNull.Value;
            else if (string.IsNullOrEmpty(Value.ToString()))
                Value = System.DBNull.Value;

            int nIndex = -1;
            DataColumn dcol = null;
            if (dt.Columns.Contains(ColumnName))
            {
                dcol = dt.Columns[ColumnName];
            }
            else
            {
                dcol = dt.Columns.Add(ColumnName, TypeOfValue);
            }
            if (dcol.ReadOnly)
                dcol.ReadOnly = false;

            nIndex = dcol.Ordinal;
            //new empty row appended
            if (dt.Rows.Count == Row && Row >= 0)
            {
                AddRow(dt, false);
            }
            //one row
            if (Row >= 0)
            {
                object newValue = Feng.Utils.ConvertHelper.ChangeType(Value, TypeOfValue);
                if (newValue != null && TypeOfValue == typeof(string))
                {
                    newValue = AntiReplaceKeyWords(newValue.ToString());
                }
                dt.Rows[Row][nIndex] = newValue == null ? System.DBNull.Value : newValue;
            }
            else if (Row == -1)
            { //all rows
                try
                {
                    for (Row = 0; Row < dt.Rows.Count; Row++)
                    {
                        if (dt.Rows[Row].RowState == DataRowState.Deleted)
                        {
                            continue;
                        }
                        object newValue = Feng.Utils.ConvertHelper.ChangeType(Value, TypeOfValue);
                        if (newValue != null && TypeOfValue == typeof(string))
                        {
                            newValue = AntiReplaceKeyWords(newValue.ToString());
                        }
                        dt.Rows[Row][nIndex] = newValue == null ? System.DBNull.Value : newValue;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dcol;
        }
    }
}
