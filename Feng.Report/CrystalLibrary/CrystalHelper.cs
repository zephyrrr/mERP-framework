// -------------------------------------------------------------------------------------------
// Author	: Jan Schreuder
//
// This code is provided as freeware. The code has been tested. You are free to use this code
// in whenever and wherever you want, provided the headers in the code remain in place. 
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// -------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

// Crystalreports required assemblies
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;

namespace CrystalLibrary
{
    /// <summary>
    /// Supported export formats for Crystal reports.
    /// </summary>
    public enum CrystalExportFormat
    {
        /// <summary>
        /// Export to Microsoft Word (*.doc)
        /// </summary>
        Word,

        /// <summary>
        /// Export to Microsoft Excel (*.xls)
        /// </summary>
        Excel,

        /// <summary>
        /// Export to Rich Text format (*.rtf)
        /// </summary>
        RichText,

        /// <summary>
        /// Export to Adobe Portable Doc format (*.pdf)
        /// </summary>
        PortableDocFormat
    }

    /// <summary>
    /// <p>The CrystalHelper class handles a number of actions that are usually
    /// required to use Crystal Reports for Visual Studio .Net. 
    /// </p>
    /// <p>This class can be used for all reports based on typed datasets and reports that
    /// have embedded queries.</p>
    /// <p>Please note that you need to register Crystal Reports when you want to use 
    /// it in Visual Studio. Registration of Crystal Reports for VS.Net is free!</p>
    /// </summary>
    /// <example>
    /// The following example shows how CrystalHelper can be used to print a report
    /// where the information is supplied by a DataSet:
    /// <code>
    /// void PrintReport(SqlConnection conn)
    /// {
    /// 	using (DataSet ds = new TestData())
    /// 	{
    /// 		SqlHelper.FillDataset(conn, 
    /// 			CommandType.Text, "SELECT * FROM Customers", 
    /// 			ds, new string [] {"Customers"});
    /// 
    /// 			using (CrystalHelper helper = new CrystalHelper(new CrystalReport1()))
    /// 		{
    /// 			helper.DataSource = ds;
    /// 			helper.Open();
    /// 			helper.Print();
    /// 			helper.Close();
    /// 		}
    /// 	}
    /// }
    /// </code>
    /// 
    /// </example>
    [CLSCompliant(false)]
    public class CrystalHelper : IDisposable
    {
        #region Local Variables

        private string _datebaseName;
        private string _serverName;
        private string _userId;
        private string _password;
        private bool _integratedSecurity;

        private ReportDocument _reportDocument;
        private DataSet _reportData;
        private string _reportFile;
        private bool _reportIsOpen;
        private SortedList<string, object> _parameters;

        #endregion

        #region Private methods

        /// <summary>
        /// Get the Crystal Export format using the CrystalExportFormat export format definitions
        /// </summary>
        /// <param name="exportFormat"><see cref="CrystalExportFormat"/> export type</param>
        /// <returns>Crystal Reports <see cref="ExportFormatType"/> type</returns>
        private static ExportFormatType GetExportType(CrystalExportFormat exportFormat)
        {
            ExportFormatType result = ExportFormatType.RichText;

            switch (exportFormat)
            {
                case CrystalExportFormat.Word:
                    result = ExportFormatType.WordForWindows;
                    break;
                case CrystalExportFormat.Excel:
                    result = ExportFormatType.Excel;
                    break;
                case CrystalExportFormat.RichText:
                    result = ExportFormatType.RichText;
                    break;
                case CrystalExportFormat.PortableDocFormat:
                    result = ExportFormatType.PortableDocFormat;
                    break;
                default:
                    throw new CrystalHelperException(string.Format(CultureInfo.CurrentUICulture, "Unsupported export format '{0}'.", exportFormat.ToString()));
            }
            return result;
        }

        /// <summary>
        /// Get the CrystalExportFormat export format definitions using the file name extension
        /// </summary>
        /// <param name="fileName">Name of the export file</param>
        /// <returns><see cref="CrystalExportFormat"/> export type</returns>
        [SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", MessageId = "stack0")]
        private static CrystalExportFormat GetExportFormat(string fileName)
        {
            CrystalExportFormat result = CrystalExportFormat.RichText;

            string extension = Helpers.FileExtension(fileName).ToUpper(CultureInfo.CurrentCulture);

            switch (extension)
            {
                case "DOC":
                    result = CrystalExportFormat.Word;
                    break;
                case "XLS":
                    result = CrystalExportFormat.Excel;
                    break;
                case "RTF":
                    result = CrystalExportFormat.RichText;
                    break;
                case "PDF":
                    result = CrystalExportFormat.PortableDocFormat;
                    break;
                default:
                    throw new CrystalHelperException(string.Format(CultureInfo.CurrentUICulture, "Unsupported export format for file '{0}'.", fileName));
            }
            return result;
        }

        /// <summary>
        /// Assign a <see cref="ConnectionInfo"/> to a <see cref="CrystalDecisions.CrystalReports.Engine.Table"/> object
        /// </summary>
        /// <param name="table">Table to which the connection is to be assigned</param>
        /// <param name="connection">Connection to the database.</param>
        private static void AssignTableConnection(CrystalDecisions.CrystalReports.Engine.Table table, ConnectionInfo connection)
        {
            // Cache the logon info block
            TableLogOnInfo logOnInfo = table.LogOnInfo;

            // Set the connection
            logOnInfo.ConnectionInfo = connection;

            // Apply the connection to the table!
            table.ApplyLogOnInfo(logOnInfo);
        }

        /// <summary>
        /// Assign the database connection to the table in all the report sections.
        /// </summary>
        private void AssignConnection()
        {
            ConnectionInfo connection = new ConnectionInfo();

            connection.DatabaseName = _datebaseName;
            connection.ServerName = _serverName;
            if (_integratedSecurity)
            {
                connection.IntegratedSecurity = _integratedSecurity;
            }
            else
            {
                connection.UserID = _userId;
                connection.Password = _password;
            }

            // First we assign the connection to all tables in the main report
            //
            foreach (CrystalDecisions.CrystalReports.Engine.Table table in _reportDocument.Database.Tables)
            {
                AssignTableConnection(table, connection);
            }

            // Now loop through all the sections and its objects to do the same for the subreports
            //
            foreach (CrystalDecisions.CrystalReports.Engine.Section section in _reportDocument.ReportDefinition.Sections)
            {
                // In each section we need to loop through all the reporting objects
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subReport = (SubreportObject)reportObject;
                        ReportDocument subDocument = subReport.OpenSubreport(subReport.SubreportName);

                        foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                        {
                            AssignTableConnection(table, connection);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Assign the DataSet to the report.
        /// </summary>
        private void AssignDataSet()
        {
            DataSet reportData = _reportData.Copy();

            // Remove primary key info. CR9 does not appreciate this information!!!
            foreach (DataTable dataTable in reportData.Tables)
            {
                foreach (DataColumn dataColumn in dataTable.PrimaryKey)
                {
                    dataColumn.AutoIncrement = false;
                }
                dataTable.PrimaryKey = null;
            }

            // Now assign the dataset to all tables in the main report
            //
            _reportDocument.SetDataSource(reportData);

            //for (int i = 0; i < _reportDocument.Database.Tables.Count; ++i)
            //{
            //    Table t = null;
            //    try
            //    {
            //        t = _reportDocument.Database.Tables[reportData.Tables[i].TableName];
            //    }
            //    catch (Exception)
            //    {
            //    }
            //    if (t != null)
            //    {
            //        t.SetDataSource(reportData.Tables[i]);
            //    }
            //    else
            //    {
            //        _reportDocument.Database.Tables[i].SetDataSource(reportData.Tables[i]);
            //    }
            //}


            // Now loop through all the sections and its objects to do the same for the subreports
            //
            foreach (CrystalDecisions.CrystalReports.Engine.Section section in _reportDocument.ReportDefinition.Sections)
            {
                // In each section we need to loop through all the reporting objects
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subReport = (SubreportObject)reportObject;
                        ReportDocument subDocument = subReport.OpenSubreport(subReport.SubreportName);

                        subDocument.SetDataSource(reportData);
                    }
                }
            }
        }

        /// <summary>
        /// Create a ReportDocument object using a report file and store
        /// the name of the file.
        /// </summary>
        /// <param name="reportFile">Name of the CrystalReports file (*.rpt).</param>
        /// <returns>A valid ReportDocument object.</returns>
        private ReportDocument CreateReportDocument(string reportFile)
        {
            ReportDocument newDocument = new ReportDocument();

            _reportFile = reportFile;
            newDocument.Load(reportFile);

            return newDocument;
        }

        /// <summary>
        /// Sets the parameters that have been added using the SetParameter method
        /// </summary>
        private void SetParameters()
        {
            foreach (ParameterFieldDefinition parameter in _reportDocument.DataDefinition.ParameterFields)
            {
                try
                {
                    // Now get the current value for the parameter
                    CrystalDecisions.Shared.ParameterValues currentValues = parameter.CurrentValues;
                    currentValues.Clear();

                    // Create a value object for Crystal reports and assign the specified value.
                    CrystalDecisions.Shared.ParameterDiscreteValue newValue = new CrystalDecisions.Shared.ParameterDiscreteValue();

                    if (_parameters.ContainsKey(parameter.Name))
                    {
                        newValue.Value = _parameters[parameter.Name];
                        // Now add the new value to the values collection and apply the 
                        // collection to the report.
                        currentValues.Add(newValue);
                        parameter.ApplyCurrentValues(currentValues);
                    }
                    else
                    {
                        parameter.ApplyCurrentValues(parameter.DefaultValues);
                    }

                }
                catch
                {
                    // Ignore any errors
                }
            }
        }

        #endregion

        /// <summary>
        /// IsOpen
        /// </summary>
        public bool IsOpen
        {
            get { return _reportIsOpen; }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrystalHelper"/> class.
        /// </summary>
        public CrystalHelper()
        {
            _parameters = new SortedList<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrystalHelper"/> class.
        /// </summary>
        /// <param name="report">The <see cref="T:ReportDocument"/> object for an embedded report.</param>
        public CrystalHelper(ReportDocument report)
            : this()
        {
            _reportDocument = report;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrystalHelper"/> class.
        /// </summary>
        /// <param name="reportFile">Name and path for a CrystalReports (*.rpt) file.</param>
        public CrystalHelper(string reportFile)
            : this()
        {
            _reportDocument = CreateReportDocument(reportFile);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrystalHelper"/> class.
        /// </summary>
        /// <param name="report">The <see cref="T:ReportDocument"/> object for an embedded report.</param>
        /// <param name="serverName">Name of the database server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userId">The user id required for logon.</param>
        /// <param name="userPassword">The password for the specified user.</param>
        public CrystalHelper(ReportDocument report, string serverName, string databaseName, string userId, string userPassword)
            : this()
        {
            _reportDocument = report;

            // Setup the connection information 
            _serverName = serverName;
            _datebaseName = databaseName;
            _userId = userId;
            _password = userPassword;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrystalHelper"/> class.
        /// </summary>
        /// <param name="report">The <see cref="T:ReportDocument"/> object for an embedded report.</param>
        /// <param name="serverName">Name of the database server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="integratedSecurity">if set to <c>true</c> integrated security is used to connect to the database; false if otherwise.</param>
        public CrystalHelper(ReportDocument report, string serverName, string databaseName, bool integratedSecurity)
            : this()
        {
            _reportDocument = report;

            // Setup the connection information 
            _serverName = serverName;
            _datebaseName = databaseName;
            _integratedSecurity = integratedSecurity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrystalHelper"/> class.
        /// </summary>
        /// <param name="reportFile">Name and path for a CrystalReports (*.rpt) file.</param>
        /// <param name="serverName">Name of the database server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userId">The user id required for logon.</param>
        /// <param name="userPassword">The password for the specified user.</param>
        public CrystalHelper(string reportFile, string serverName, string databaseName, string userId, string userPassword)
            : this()
        {
            _reportDocument = CreateReportDocument(reportFile);

            // Setup the connection information 
            _serverName = serverName;
            _datebaseName = databaseName;
            _userId = userId;
            _password = userPassword;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CrystalHelper"/> class.
        /// </summary>
        /// <param name="reportFile">Name and path for a CrystalReports (*.rpt) file.</param>
        /// <param name="serverName">Name of the database server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="integratedSecurity">if set to <c>true</c> integrated security is used to connect to the database; false if otherwise.</param>
        public CrystalHelper(string reportFile, string serverName, string databaseName, bool integratedSecurity)
            : this()
        {
            _reportDocument = CreateReportDocument(reportFile);

            // Setup the connection information 
            _serverName = serverName;
            _datebaseName = databaseName;
            _integratedSecurity = integratedSecurity;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the report source.
        /// </summary>
        /// <value>The report source.</value>
        /// <remarks>
        /// Use this property when you want to show the report in the 
        /// CrystalReportsViewer control.</remarks>
        public ReportDocument ReportSource
        {
            get
            {
                return _reportDocument;
            }
            set
            {
                if (_reportDocument != null)
                {
                    _reportDocument.Dispose();
                }
                _reportDocument = value;
                if (value == null)
                {
                    _reportFile = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the report file.
        /// </summary>
        /// <value>The report file.</value>
        public string ReportFile
        {
            get
            {
                return _reportFile;
            }
            set
            {
                if (_reportDocument != null)
                {
                    _reportDocument.Dispose();
                }
                if (value != null && value.Trim().Length > 0)
                {
                    _reportDocument = CreateReportDocument(value);
                }
                else
                {
                    _reportFile = value;
                    _reportDocument = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public DataSet DataSource
        {
            get
            {
                return _reportData;
            }
            set
            {
                if (_reportData != null)
                {
                    _reportData.Dispose();
                }
                _reportData = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName
        {
            get
            {
                return _serverName;
            }
            set
            {
                _serverName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get
            {
                return _datebaseName;
            }
            set
            {
                _datebaseName = value;
            }
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public string UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [integrated security].
        /// </summary>
        /// <value><c>true</c> if integrated security is used to connect to the database; false if otherwise.</value>
        public bool IntegratedSecurity
        {
            get
            {
                return _integratedSecurity;
            }
            set
            {
                _integratedSecurity = value;
            }
        }

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets the export filter for a <see cref="T:SaveFileDialog"/>
        /// </summary>
        /// <value>The export filter.</value>
        public static string ExportFilter
        {
            get
            {
                StringBuilder filter = new StringBuilder();

                // TODO: Translations???
                filter.Append("Rich text (*.rtf)|*.rtf|");
                filter.Append("Excel (*.xls)|*.xls|");
                filter.Append("Word (*.doc)|*.doc|");
                filter.Append("Portable Document Format (*.pdf)|*.pdf");

                return filter.ToString();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set value for report parameters
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="value">Value to be set for the specified parameter.</param>
        public void SetParameter(string name, object value)
        {
            if (_parameters.ContainsKey(name))
            {
                _parameters[name] = value;
            }
            else
            {
                _parameters.Add(name, value);
            }
        }

        /// <summary>
        /// Close the report.
        /// </summary>
        /// <remarks>
        /// This method will throw an exception when the Open() method is not yet called on this object.
        /// </remarks>
        public void Close()
        {
            //if (!_reportIsOpen) throw new InvalidOperationException("The report is already closed.");

            _reportDocument.Close();
            _reportIsOpen = false;
        }

        /// <summary>
        /// Open the report.
        /// </summary>
        /// <remarks>
        /// <p>This method will first attempt to assign the DataSource if that was specified. When no DataSource has been
        /// assigned, a check will be made to see if database connection information has been specified. When this is the case
        /// this information will be assigned to the report.
        /// </p>
        /// <P>This method will throw an exception when:
        /// <list type="bullet">
        /// <item><description>The report (rpt) has not been assigned yet.</description></item>
        /// <item><description>The Open() method has already been called on this object.</description></item>
        /// <item><description>A table being used in the report does not exist in the dataset which has been assigned to this report. (only when a dataset has been assigned to the DataSource property)</description></item>
        /// <item><description>The ServerName, DatabaseName or UserId property has not been set. (only when the DataSource property has not been set)</description></item>
        /// <item><description>When no database connection or datset could be assignd.</description></item>
        /// </list>
        /// </P></remarks>
        public void Open()
        {
            if (_reportDocument == null) throw new CrystalHelperException("First assign a report document.");
            //if (_reportIsOpen) throw new InvalidOperationException("The report is already open.");

            // Check if the connection object exists. If so assign that
            // to the report.
            if (_reportData != null)
            {
                // Assign the dataset to the report.
                AssignDataSet();

                // 在AssignDataSet前设置，会清空参数
                SetParameters();
            }
            else
            {
                // Todo: 还未测试（原始是在前面）
                SetParameters();

                if (_serverName.Length == 0 || (!_integratedSecurity && _userId.Length == 0) || _datebaseName.Length == 0)
                {
                    throw new CrystalHelperException("Connection information is incomplete. Report could not be opened.");
                }
                AssignConnection();
            }
            _reportIsOpen = true;
        }

        /// <summary>
        /// Force a refresh of the data in the report. 
        /// </summary>
        /// <remarks>
        /// When the report is based on a DataSource, the report will be refreshed using 
        /// data in that DataSource. In case the report has it's own database connection 
        /// and uses SQL queries, the report will be refreshed using that information. 
        /// <br/>
        /// <p>
        /// This method will throw an exception when the Open() method is not yet called on this object.
        /// </p>
        /// </remarks>
        public void Refresh()
        {
            if (!_reportIsOpen) throw new InvalidOperationException("The report is not open.");

            _reportDocument.Refresh();
        }

        /// <summary>
        /// Print the report to the specified printer. 
        /// </summary>
        /// <param name="printerName">Name of the printer to print the information to.</param>
        /// <param name="nrCopies">The number of copies required.</param>
        /// <param name="collatePages">Indicates whether to collate the pages.</param>
        /// <param name="firstPage">First page to be printed. When less than 1, printing starts at the first page.</param>
        /// <param name="lastPage">Last page to be printed. When less than 1, or more than 9999, the last page will be printed as last page.</param>
        /// <remarks>
        /// This method will throw an exception when the Open() method is not yet called on this object.
        /// </remarks>
        public void Print(string printerName, int nrCopies, bool collatePages, int firstPage, int lastPage)
        {
            bool openedHere = false;

            if (!_reportIsOpen)
            {
                this.Open();
                openedHere = true;
            }

            if (!string.IsNullOrEmpty(printerName))
            {
                _reportDocument.PrintOptions.PrinterName = printerName;
            }

            if (firstPage < 1) firstPage = 0;
            if ((lastPage < 1) || (lastPage > 9999)) lastPage = 0;

            _reportDocument.PrintToPrinter(nrCopies, collatePages, firstPage, lastPage);
            if (openedHere)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Prints one copy of the entire report to the default printer.
        /// </summary>
        /// <remarks>
        /// This method will throw an exception when the Open() method is not yet called on this object.
        /// </remarks>
        public void Print()
        {
            this.Print(null, 1, false, 0, 0);
        }

        /// <summary>
        /// Export the report to a file. 
        /// </summary>
        /// <param name="fileName">Name of the export file.</param>
        /// <remarks>
        /// The type of document is specified by the extension for the file 
        /// name When no extension is specified, the export format will default to Rich text.
        /// The following document types are supported:
        /// <list type="bullet">
        /// <item><description>Word (*.doc).</description></item>
        /// <item><description>Excel (*.xls).</description></item>
        /// <item><description>Rich text (*.rtf).</description></item>
        /// <item><description>Portable Doc Format (*.pdf)</description></item>
        /// </list><br/>
        /// <p>
        /// This method will throw an exception when the Open() method is not yet called on this object.
        /// </p>
        /// </remarks>
        public void Export(string fileName)
        {
            this.Export(fileName, GetExportFormat(fileName));
        }

        /// <summary>
        /// Export the report to a file using the specified format. 
        /// </summary>
        /// <param name="fileName">Name of the export file.</param>
        /// <param name="exportFormat">CrystalExportFormat.</param>
        /// <remarks>
        /// The following document types are supported:
        /// <list type="bullet">
        /// <item><description>Word (*.doc).</description></item>
        /// <item><description>Excel (*.xls).</description></item>
        /// <item><description>Rich text (*.rtf).</description></item>
        /// <item><description>Portable Doc Format (*.pdf)</description></item>
        /// </list><br/>
        /// <p>
        /// This method will throw an exception when the Open() method is not yet called on this object.
        /// </p>
        /// </remarks>
        public void Export(string fileName, CrystalExportFormat exportFormat)
        {
            bool openedHere = false;

            if (!_reportIsOpen)
            {
                this.Open();
                openedHere = true;
            }

            _reportDocument.ExportToDisk(GetExportType(exportFormat), fileName);

            if (openedHere)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Exports the report to the specified HttpResponse.
        /// </summary>
        /// <param name="response">The response object to export the report to.</param>
        /// <param name="exportFormat">The export format.</param>
        public void Export(HttpResponse response, CrystalExportFormat exportFormat)
        {
            this.Export(response, exportFormat, false, "");
        }

        /// <summary>
        /// Exports the report to the specified HttpResponse and prompts the user for a file name. The default 
        /// name is the specified file name, 
        /// </summary>
        /// <param name="response">The response object to export the report to.</param>
        /// <param name="exportFormat">The export format.</param>
        /// <param name="asAttachment">If the asAttachment Boolean variable is set to True, a File Download dialog box appears. If the asAttachment Boolean variable is set to False, the exported report opens in the browser window.</param>
        /// <param name="attachmentName">When you choose to save the file, the file name is set to the attachmentName string variable. If you do not specify the attachmentName variable, then the default file name is "Untitled," with the specified file extension. The file name can be changed in the Save As dialog box.</param>
        public void Export(HttpResponse response, CrystalExportFormat exportFormat, bool asAttachment, string attachmentName)
        {
            if (response == null) throw new ArgumentNullException("response");
            if (attachmentName == null) throw new ArgumentNullException("asAttachment");

            bool openedHere = false;

            try
            {
                if (!_reportIsOpen)
                {
                    this.Open();
                    openedHere = true;
                }

                _reportDocument.ExportToHttpResponse(GetExportType(exportFormat), response, asAttachment, attachmentName);
            }
            finally
            {
                if (openedHere)
                {
                    this.Close();
                }
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Show or hide the status bar on the crystalreports viewer.
        /// </summary>
        /// <param name="viewer">Current viewer.</param>
        /// <param name="visible">Set if the status bar is visible or not.</param>
        /// <remarks>
        /// This method will throw an exception when the viewer object is not specified.
        /// </remarks>
        /// <example>
        /// This example shows how the status bar can be removed:
        /// <code>
        /// CrystalHelper.ViewerStatusBar(myViewer, false);
        /// </code>
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void ViewerStatusBar(CrystalReportViewer viewer, bool visible)
        {
            if (viewer == null) throw new ArgumentNullException("viewer");

            foreach (Control control in viewer.Controls)
            {
                if (string.Compare(control.GetType().Name, "StatusBar", true, CultureInfo.InvariantCulture) == 0)
                {
                    control.Visible = visible;
                }
            }
        }

        /// <summary>
        /// Show or hide the tabs on the crystalreports viewer.
        /// </summary>
        /// <param name="viewer">Current viewer</param>
        /// <param name="visible">Set if the status bar is visible or not.</param>
        /// <remarks>
        /// This method will throw an exception when the viewer object is not specified.
        /// </remarks>
        /// <example>
        /// This example shows how the tabs can be removed:
        /// <code>
        /// CrystalHelper.ViewerTabs(myViewer, false);
        /// </code>
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void ViewerTabs(CrystalReportViewer viewer, bool visible)
        {
            if (viewer == null) throw new ArgumentNullException("viewer");

            foreach (Control control in viewer.Controls)
            {
                if (string.Compare(control.GetType().Name, "PageView", true, CultureInfo.InvariantCulture) == 0)
                {
                    TabControl tab = (TabControl)((PageView)control).Controls[0];

                    if (!visible)
                    {
                        tab.ItemSize = new Size(0, 1);
                        tab.SizeMode = TabSizeMode.Fixed;
                        tab.Appearance = TabAppearance.Buttons;
                    }
                    else
                    {
                        tab.ItemSize = new Size(67, 18);
                        tab.SizeMode = TabSizeMode.Normal;
                        tab.Appearance = TabAppearance.Normal;
                    }
                }
            }
        }

        /// <summary>
        /// Replace the current name of a tab with a new name.
        /// </summary>
        /// <param name="viewer">Current viewer.</param>
        /// <param name="oldName">The name to be replaced.</param>
        /// <param name="newName">The new name.</param>
        /// <remarks>
        /// This method will throw an exception when:
        /// <list type="bullet">
        /// <item><description>The viewer object is not specified.</description></item>
        /// <item><description>The Open() method is not yet called on this object.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// This example shows how the default tab description MainReport can be replaced.
        /// <code>
        /// CrystalHelper.ViewerStatusBar(myViewer, "MainReport", "And now for something completely different");
        /// </code>
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void ReplaceReportName(CrystalReportViewer viewer, string oldName, string newName)
        {
            if (viewer == null) throw new ArgumentNullException("viewer");
            if (oldName == null || oldName.Length == 0) throw new ArgumentException("May not be empty.", "oldName");

            foreach (Control control in viewer.Controls)
            {
                if (string.Compare(control.GetType().Name, "PageView", true, CultureInfo.InvariantCulture) == 0)
                {
                    foreach (Control controlInPage in control.Controls)
                    {
                        if (string.Compare(controlInPage.GetType().Name, "TabControl", true, CultureInfo.InvariantCulture) == 0)
                        {
                            TabControl tabs = (TabControl)controlInPage;

                            foreach (TabPage tabPage in tabs.TabPages)
                            {
                                if (string.Compare(tabPage.Text, oldName, false, CultureInfo.InvariantCulture) == 0)
                                {
                                    tabPage.Text = newName;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose of this object's resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true); // as a service to those who might inherit from us
        }

        /// <summary>
        ///	Free the instance variables of this object.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reportIsOpen)
                {
                    _reportDocument.Close();
                }
                if (_reportDocument != null)
                {
                    _reportDocument.Dispose();
                    _reportDocument = null;
                }

                if (_reportData != null)
                {
                    _reportData.Dispose();
                    _reportData = null;
                }
            }
        }

        #endregion
    }
}
