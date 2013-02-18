/*
 * Xceed Grid for .NET - "Generate Report" form - Source code for customization
 * Copyright (c) 2005 - Xceed Software Inc.
 * 
 * [GenerateReportForm.cs]
 * 
 * This file is part of Xceed Grid for .NET. The source code provided in this file 
 * is intended to allow developers invoking Xceed Grid for .NET's "Generate Report"
 * form to make custom changes to it, recompile it, and be able to use their 
 * custom version instead. Instructions on how to invoke your custom version
 * are provided in the documentation. No other uses of this source code are 
 * permitted without express written permission by Xceed. This source code 
 * is provided "as is", without warranty of any kind, either expressed or implied.
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Xceed.Editors;
using Xceed.Grid;
using Xceed.Grid.Editors;
using Xceed.Grid.Reporting;
using Xceed.Grid.Collections;

namespace Feng.Grid.Print
{
    /// <summary>
    /// Represents a form which can be used to generate a report.
    /// </summary>
    public class GenerateReportForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button butPrint;
        private System.Windows.Forms.Button butPreview;
        private System.Windows.Forms.Button butExport;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.GroupBox groupReportStyle;
        private System.Windows.Forms.Panel panelReportStyleList;
        private System.Windows.Forms.ComboBox comboPrinter;
        private System.Windows.Forms.Button butRemoveStyle;
        private System.Windows.Forms.Button butEditStyle;
        private System.Windows.Forms.Button butNewStyle;
        private System.Windows.Forms.GroupBox groupSamplePage;
        private System.Windows.Forms.Panel panelSamplePage;
        private System.Windows.Forms.GroupBox groupOutputSettings;
        private System.Windows.Forms.Button butPageSetup;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label labelPrinter;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel panelStyleButtons;
        private System.Windows.Forms.GroupBox groupColumns;
        private System.Windows.Forms.Panel panelColumnList;
        private System.Windows.Forms.Label labelControlPlaceholder1;
        private System.Windows.Forms.Label labelControlPlaceholder3;
        private System.Windows.Forms.Label labelControlPlaceholder2;
        private System.Windows.Forms.Panel panelColumnsButtons;
        private System.Windows.Forms.Label labelLayout;
        private System.Windows.Forms.GroupBox groupReport;
        private System.Windows.Forms.Label labelTitle;
        private Feng.Windows.Forms.MyMultilineTextBox textTitle;
        private System.Windows.Forms.ComboBox comboLayout;
        private System.Windows.Forms.Button butResetLayout;
        private System.Windows.Forms.Button butResetPrinter;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Initializes a new instance of the GenerateReportForm specifying the <see cref="Xceed.Grid.GridControl"/>
        /// from which to create the report.
        /// </summary>
        /// <param name="grid">A reference to the <see cref="Xceed.Grid.GridControl"/> from which to create
        /// the report.</param>
        /// <remarks><para>
        /// When using this overload of the GenerateReportForm constructor, the "New", "Edit", and "Remove" buttons
        /// which are used to create/modify custom report styles are not available.
        /// </para></remarks>
        public GenerateReportForm(GridControl grid)
            : this(grid, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GenerateReportForm specifying the <see cref="Xceed.Grid.GridControl"/>
        /// from which to create the report as well as the folder where custom report styles are located.
        /// </summary>
        /// <param name="grid">A reference to the <see cref="Xceed.Grid.GridControl"/> from which to create
        /// the report.</param>
        /// <param name="styleFolder">The folder where custom report styles are located.</param>
        /// <remarks><para>
        /// When this overload of the GenerateReportForm constructor is used, the "New", "Edit", and "Remove" buttons which
        /// are used to create/modify custom report styles will be displayed.
        /// </para></remarks>
        public GenerateReportForm(GridControl grid, string styleFolder, string styleFolderGlobal)
        {
            if (grid == null)
                throw new ArgumentNullException("grid");

            if (styleFolder == null)
                throw new ArgumentNullException("styleFolder");

            m_gridLiveData = grid;

            if (styleFolder.Length > 0)
            {
                // Store the full, absolute directory name
                m_styleFolder = Path.GetFullPath(styleFolder);
            }
            if (styleFolderGlobal.Length > 0)
            {
                // Store the full, absolute directory name
                m_styleFolderGlobal = Path.GetFullPath(styleFolderGlobal);
            }

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            grid.ReportSettings.Load();
            this.LoadFormBoundsFromRegistry();
        }

        /// <summary>
        /// Initializes a new instance of the GenerateReportForm specifying the Report instance that 
        /// will be used by the form.</summary>
        /// <param name="report">A reference to the <see cref="Report"/> instance that 
        /// will be used by the form.</param>
        /// <remarks><para>
        /// <see cref="Report.ReportStyleSheet"/> will be set to the last selection.
        /// </para></remarks>
        public GenerateReportForm(Report report)
            : this(report, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GenerateReportForm specifying the Report instance that will be used by the form
        /// as well as the location of the custom styles.
        /// </summary>
        /// <param name="report">A reference to the <see cref="Report"/> instance that 
        /// will be used by the form.</param>
        /// <param name="styleFolder">The folder where custom report styles are located.</param>
        /// <remarks><para>
        /// <see cref="Report.ReportStyleSheet"/> will be set to the last selection.
        /// </para></remarks>
        public GenerateReportForm(Report report, string styleFolder)
            : this(report.GridControl, styleFolder, string.Empty)
        {
            m_report = report;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the form is disposing; <see langword="false"/> otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateReportForm));
            this.groupReportStyle = new System.Windows.Forms.GroupBox();
            this.panelReportStyleList = new System.Windows.Forms.Panel();
            this.labelControlPlaceholder1 = new System.Windows.Forms.Label();
            this.panelStyleButtons = new System.Windows.Forms.Panel();
            this.butRemoveStyle = new System.Windows.Forms.Button();
            this.butEditStyle = new System.Windows.Forms.Button();
            this.butNewStyle = new System.Windows.Forms.Button();
            this.groupSamplePage = new System.Windows.Forms.GroupBox();
            this.panelSamplePage = new System.Windows.Forms.Panel();
            this.labelControlPlaceholder3 = new System.Windows.Forms.Label();
            this.butPrint = new System.Windows.Forms.Button();
            this.butPreview = new System.Windows.Forms.Button();
            this.butExport = new System.Windows.Forms.Button();
            this.butClose = new System.Windows.Forms.Button();
            this.groupOutputSettings = new System.Windows.Forms.GroupBox();
            this.butResetPrinter = new System.Windows.Forms.Button();
            this.labelPrinter = new System.Windows.Forms.Label();
            this.comboPrinter = new System.Windows.Forms.ComboBox();
            this.butPageSetup = new System.Windows.Forms.Button();
            this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
            this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.groupColumns = new System.Windows.Forms.GroupBox();
            this.panelColumnList = new System.Windows.Forms.Panel();
            this.labelControlPlaceholder2 = new System.Windows.Forms.Label();
            this.panelColumnsButtons = new System.Windows.Forms.Panel();
            this.comboLayout = new System.Windows.Forms.ComboBox();
            this.labelLayout = new System.Windows.Forms.Label();
            this.butResetLayout = new System.Windows.Forms.Button();
            this.groupReport = new System.Windows.Forms.GroupBox();
            this.textTitle = new Feng.Windows.Forms.MyMultilineTextBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupReportStyle.SuspendLayout();
            this.panelReportStyleList.SuspendLayout();
            this.panelStyleButtons.SuspendLayout();
            this.groupSamplePage.SuspendLayout();
            this.panelSamplePage.SuspendLayout();
            this.groupOutputSettings.SuspendLayout();
            this.groupColumns.SuspendLayout();
            this.panelColumnList.SuspendLayout();
            this.panelColumnsButtons.SuspendLayout();
            this.groupReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // groupReportStyle
            // 
            this.groupReportStyle.Controls.Add(this.panelReportStyleList);
            this.groupReportStyle.Controls.Add(this.panelStyleButtons);
            this.groupReportStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupReportStyle.Location = new System.Drawing.Point(10, 78);
            this.groupReportStyle.Name = "groupReportStyle";
            this.groupReportStyle.Size = new System.Drawing.Size(384, 198);
            this.groupReportStyle.TabIndex = 1;
            this.groupReportStyle.TabStop = false;
            this.groupReportStyle.Text = "报表样式(&Y)";
            // 
            // panelReportStyleList
            // 
            this.panelReportStyleList.Controls.Add(this.labelControlPlaceholder1);
            this.panelReportStyleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReportStyleList.Location = new System.Drawing.Point(3, 17);
            this.panelReportStyleList.Name = "panelReportStyleList";
            this.panelReportStyleList.Padding = new System.Windows.Forms.Padding(8);
            this.panelReportStyleList.Size = new System.Drawing.Size(378, 143);
            this.panelReportStyleList.TabIndex = 0;
            // 
            // labelControlPlaceholder1
            // 
            this.labelControlPlaceholder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControlPlaceholder1.Enabled = false;
            this.labelControlPlaceholder1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlPlaceholder1.Location = new System.Drawing.Point(8, 8);
            this.labelControlPlaceholder1.Name = "labelControlPlaceholder1";
            this.labelControlPlaceholder1.Size = new System.Drawing.Size(362, 127);
            this.labelControlPlaceholder1.TabIndex = 0;
            this.labelControlPlaceholder1.Text = "A GridControl named gridReportStyleList, that contains a list of available report" +
                " styles, will be added here at run-time. The code is in the \"ReportStyle List\" c" +
                "ode region.";
            this.labelControlPlaceholder1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelControlPlaceholder1.Visible = false;
            // 
            // panelStyleButtons
            // 
            this.panelStyleButtons.Controls.Add(this.butRemoveStyle);
            this.panelStyleButtons.Controls.Add(this.butEditStyle);
            this.panelStyleButtons.Controls.Add(this.butNewStyle);
            this.panelStyleButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStyleButtons.Location = new System.Drawing.Point(3, 160);
            this.panelStyleButtons.Name = "panelStyleButtons";
            this.panelStyleButtons.Size = new System.Drawing.Size(378, 35);
            this.panelStyleButtons.TabIndex = 1;
            // 
            // butRemoveStyle
            // 
            this.butRemoveStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butRemoveStyle.Location = new System.Drawing.Point(163, 0);
            this.butRemoveStyle.Name = "butRemoveStyle";
            this.butRemoveStyle.Size = new System.Drawing.Size(72, 25);
            this.butRemoveStyle.TabIndex = 5;
            this.butRemoveStyle.Text = "删除(&R)";
            this.butRemoveStyle.Click += new System.EventHandler(this.butRemoveStyle_Click);
            // 
            // butEditStyle
            // 
            this.butEditStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butEditStyle.Location = new System.Drawing.Point(86, 0);
            this.butEditStyle.Name = "butEditStyle";
            this.butEditStyle.Size = new System.Drawing.Size(72, 25);
            this.butEditStyle.TabIndex = 4;
            this.butEditStyle.Text = "编辑(&E)...";
            this.butEditStyle.Click += new System.EventHandler(this.butEditStyle_Click);
            // 
            // butNewStyle
            // 
            this.butNewStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butNewStyle.Location = new System.Drawing.Point(10, 0);
            this.butNewStyle.Name = "butNewStyle";
            this.butNewStyle.Size = new System.Drawing.Size(72, 25);
            this.butNewStyle.TabIndex = 3;
            this.butNewStyle.Text = "新建(&N)...";
            this.butNewStyle.Click += new System.EventHandler(this.butNewStyle_Click);
            // 
            // groupSamplePage
            // 
            this.groupSamplePage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSamplePage.Controls.Add(this.panelSamplePage);
            this.groupSamplePage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupSamplePage.Location = new System.Drawing.Point(403, 9);
            this.groupSamplePage.Name = "groupSamplePage";
            this.groupSamplePage.Size = new System.Drawing.Size(480, 555);
            this.groupSamplePage.TabIndex = 0;
            this.groupSamplePage.TabStop = false;
            this.groupSamplePage.Text = "样例";
            // 
            // panelSamplePage
            // 
            this.panelSamplePage.Controls.Add(this.labelControlPlaceholder3);
            this.panelSamplePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSamplePage.Location = new System.Drawing.Point(3, 17);
            this.panelSamplePage.Name = "panelSamplePage";
            this.panelSamplePage.Padding = new System.Windows.Forms.Padding(8);
            this.panelSamplePage.Size = new System.Drawing.Size(474, 535);
            this.panelSamplePage.TabIndex = 0;
            // 
            // labelControlPlaceholder3
            // 
            this.labelControlPlaceholder3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControlPlaceholder3.Enabled = false;
            this.labelControlPlaceholder3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlPlaceholder3.Location = new System.Drawing.Point(8, 8);
            this.labelControlPlaceholder3.Name = "labelControlPlaceholder3";
            this.labelControlPlaceholder3.Size = new System.Drawing.Size(458, 519);
            this.labelControlPlaceholder3.TabIndex = 0;
            this.labelControlPlaceholder3.Text = "A ReportPreviewControl named reportPreview, that displays a preview of the first " +
                "page of the report, will be added here at run-time. The code is in the \"Sample P" +
                "age\" code region.";
            this.labelControlPlaceholder3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelControlPlaceholder3.Visible = false;
            // 
            // butPrint
            // 
            this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butPrint.Location = new System.Drawing.Point(499, 572);
            this.butPrint.Name = "butPrint";
            this.butPrint.Size = new System.Drawing.Size(90, 25);
            this.butPrint.TabIndex = 1;
            this.butPrint.Text = "打印(&P)";
            this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
            // 
            // butPreview
            // 
            this.butPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butPreview.Location = new System.Drawing.Point(595, 572);
            this.butPreview.Name = "butPreview";
            this.butPreview.Size = new System.Drawing.Size(90, 25);
            this.butPreview.TabIndex = 2;
            this.butPreview.Text = "打印预览(&V)";
            this.butPreview.Click += new System.EventHandler(this.butPreview_Click);
            // 
            // butExport
            // 
            this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butExport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butExport.Location = new System.Drawing.Point(691, 572);
            this.butExport.Name = "butExport";
            this.butExport.Size = new System.Drawing.Size(90, 25);
            this.butExport.TabIndex = 3;
            this.butExport.Text = "导出(&X)...";
            this.butExport.Click += new System.EventHandler(this.butExport_Click);
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butClose.Location = new System.Drawing.Point(787, 572);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(90, 25);
            this.butClose.TabIndex = 4;
            this.butClose.Text = "关闭(&O)";
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // groupOutputSettings
            // 
            this.groupOutputSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupOutputSettings.Controls.Add(this.butResetPrinter);
            this.groupOutputSettings.Controls.Add(this.labelPrinter);
            this.groupOutputSettings.Controls.Add(this.comboPrinter);
            this.groupOutputSettings.Controls.Add(this.butPageSetup);
            this.groupOutputSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupOutputSettings.Location = new System.Drawing.Point(10, 469);
            this.groupOutputSettings.Name = "groupOutputSettings";
            this.groupOutputSettings.Size = new System.Drawing.Size(384, 95);
            this.groupOutputSettings.TabIndex = 8;
            this.groupOutputSettings.TabStop = false;
            this.groupOutputSettings.Text = "输出设置";
            // 
            // butResetPrinter
            // 
            this.butResetPrinter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butResetPrinter.Location = new System.Drawing.Point(278, 60);
            this.butResetPrinter.Name = "butResetPrinter";
            this.butResetPrinter.Size = new System.Drawing.Size(90, 25);
            this.butResetPrinter.TabIndex = 27;
            this.butResetPrinter.Text = "重置";
            this.butResetPrinter.Click += new System.EventHandler(this.butResetPrinter_Click);
            // 
            // labelPrinter
            // 
            this.labelPrinter.AutoSize = true;
            this.labelPrinter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelPrinter.Location = new System.Drawing.Point(10, 28);
            this.labelPrinter.Name = "labelPrinter";
            this.labelPrinter.Size = new System.Drawing.Size(65, 12);
            this.labelPrinter.TabIndex = 27;
            this.labelPrinter.Text = "打印机(&i):";
            this.labelPrinter.Click += new System.EventHandler(this.labelPrinter_Click);
            // 
            // comboPrinter
            // 
            this.comboPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPrinter.Location = new System.Drawing.Point(81, 26);
            this.comboPrinter.Name = "comboPrinter";
            this.comboPrinter.Size = new System.Drawing.Size(293, 20);
            this.comboPrinter.TabIndex = 25;
            this.comboPrinter.SelectionChangeCommitted += new System.EventHandler(this.comboPrinter_SelectionChangeCommitted);
            // 
            // butPageSetup
            // 
            this.butPageSetup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butPageSetup.Location = new System.Drawing.Point(67, 60);
            this.butPageSetup.Name = "butPageSetup";
            this.butPageSetup.Size = new System.Drawing.Size(115, 25);
            this.butPageSetup.TabIndex = 26;
            this.butPageSetup.Text = "页面设置(&S)...";
            this.butPageSetup.Click += new System.EventHandler(this.butPageSetup_Click);
            // 
            // printPreviewDialog
            // 
            this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(1280, 740);
            this.printPreviewDialog.Enabled = true;
            this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
            this.printPreviewDialog.Name = "printPreviewDialog";
            this.printPreviewDialog.UseAntiAlias = true;
            this.printPreviewDialog.Visible = false;
            this.printPreviewDialog.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "pdf";
            this.saveFileDialog.Filter = "Adobe PDF Document (*.pdf)|*.pdf|HTML Document (*.html;*.htm)|*.html;*.htm|JPEG I" +
                "mage (*.jpeg;*.jpg)|*.jpeg;*.jpg|TIFF Image (*.tiff;*.tif)|*.tiff;*.tif";
            this.saveFileDialog.Title = "Select File for Export";
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Empty;
            this.imageList.Images.SetKeyName(0, "");
            // 
            // groupColumns
            // 
            this.groupColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupColumns.Controls.Add(this.panelColumnList);
            this.groupColumns.Controls.Add(this.panelColumnsButtons);
            this.groupColumns.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupColumns.Location = new System.Drawing.Point(10, 284);
            this.groupColumns.Name = "groupColumns";
            this.groupColumns.Size = new System.Drawing.Size(384, 176);
            this.groupColumns.TabIndex = 9;
            this.groupColumns.TabStop = false;
            this.groupColumns.Text = "表格列设置(&C)";
            // 
            // panelColumnList
            // 
            this.panelColumnList.Controls.Add(this.labelControlPlaceholder2);
            this.panelColumnList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColumnList.Location = new System.Drawing.Point(3, 17);
            this.panelColumnList.Name = "panelColumnList";
            this.panelColumnList.Padding = new System.Windows.Forms.Padding(8);
            this.panelColumnList.Size = new System.Drawing.Size(378, 121);
            this.panelColumnList.TabIndex = 0;
            // 
            // labelControlPlaceholder2
            // 
            this.labelControlPlaceholder2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControlPlaceholder2.Enabled = false;
            this.labelControlPlaceholder2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlPlaceholder2.Location = new System.Drawing.Point(8, 8);
            this.labelControlPlaceholder2.Name = "labelControlPlaceholder2";
            this.labelControlPlaceholder2.Size = new System.Drawing.Size(362, 105);
            this.labelControlPlaceholder2.TabIndex = 0;
            this.labelControlPlaceholder2.Text = resources.GetString("labelControlPlaceholder2.Text");
            this.labelControlPlaceholder2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelControlPlaceholder2.Visible = false;
            // 
            // panelColumnsButtons
            // 
            this.panelColumnsButtons.Controls.Add(this.comboLayout);
            this.panelColumnsButtons.Controls.Add(this.labelLayout);
            this.panelColumnsButtons.Controls.Add(this.butResetLayout);
            this.panelColumnsButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelColumnsButtons.Location = new System.Drawing.Point(3, 138);
            this.panelColumnsButtons.Name = "panelColumnsButtons";
            this.panelColumnsButtons.Size = new System.Drawing.Size(378, 35);
            this.panelColumnsButtons.TabIndex = 1;
            // 
            // comboLayout
            // 
            this.comboLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLayout.Location = new System.Drawing.Point(67, 2);
            this.comboLayout.Name = "comboLayout";
            this.comboLayout.Size = new System.Drawing.Size(202, 20);
            this.comboLayout.TabIndex = 4;
            this.comboLayout.SelectionChangeCommitted += new System.EventHandler(this.comboLayout_SelectionChangeCommitted);
            // 
            // labelLayout
            // 
            this.labelLayout.Location = new System.Drawing.Point(10, 4);
            this.labelLayout.Name = "labelLayout";
            this.labelLayout.Size = new System.Drawing.Size(57, 25);
            this.labelLayout.TabIndex = 3;
            this.labelLayout.Text = "布局(&L):";
            // 
            // butResetLayout
            // 
            this.butResetLayout.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butResetLayout.Location = new System.Drawing.Point(278, 0);
            this.butResetLayout.Name = "butResetLayout";
            this.butResetLayout.Size = new System.Drawing.Size(90, 25);
            this.butResetLayout.TabIndex = 5;
            this.butResetLayout.Text = "重置";
            this.butResetLayout.Click += new System.EventHandler(this.butResetLayout_Click);
            // 
            // groupReport
            // 
            this.groupReport.Controls.Add(this.textTitle);
            this.groupReport.Controls.Add(this.labelTitle);
            this.groupReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupReport.Location = new System.Drawing.Point(10, 9);
            this.groupReport.Name = "groupReport";
            this.groupReport.Size = new System.Drawing.Size(384, 60);
            this.groupReport.TabIndex = 0;
            this.groupReport.TabStop = false;
            this.groupReport.Text = "报表";
            // 
            // textTitle
            // 
            this.textTitle.DropDownAllowFocus = true;
            this.textTitle.Location = new System.Drawing.Point(69, 26);
            this.textTitle.Name = "textTitle";
            this.textTitle.Size = new System.Drawing.Size(305, 21);
            this.textTitle.TabIndex = 5;
            // 
            // 
            // 
            this.textTitle.TextBoxArea.Name = "";
            this.textTitle.TextBoxArea.SelectOnFocus = true;
            this.textTitle.TextBoxArea.TabIndex = 0;
            this.textTitle.Leave += new System.EventHandler(this.textTitle_Leave);
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(10, 28);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(53, 15);
            this.labelTitle.TabIndex = 4;
            this.labelTitle.Text = "标题(&T):";
            // 
            // GenerateReportForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.butClose;
            this.ClientSize = new System.Drawing.Size(892, 612);
            this.Controls.Add(this.groupReport);
            this.Controls.Add(this.groupColumns);
            this.Controls.Add(this.groupOutputSettings);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butExport);
            this.Controls.Add(this.butPreview);
            this.Controls.Add(this.butPrint);
            this.Controls.Add(this.groupReportStyle);
            this.Controls.Add(this.groupSamplePage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 646);
            this.Name = "GenerateReportForm";
            this.ShowInTaskbar = false;
            this.Text = "生成报表";
            this.groupReportStyle.ResumeLayout(false);
            this.panelReportStyleList.ResumeLayout(false);
            this.panelStyleButtons.ResumeLayout(false);
            this.groupSamplePage.ResumeLayout(false);
            this.panelSamplePage.ResumeLayout(false);
            this.groupOutputSettings.ResumeLayout(false);
            this.groupOutputSettings.PerformLayout();
            this.groupColumns.ResumeLayout(false);
            this.panelColumnList.ResumeLayout(false);
            this.panelColumnsButtons.ResumeLayout(false);
            this.groupReport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textTitle)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Form Initialization

        /// <summary>
        /// Raises the <see cref="Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            this.textTitle.Text = m_gridLiveData.ReportSettings.Title;
            this.InitializeReportStyleList();
            this.InitializeSamplePage();
            this.InitializeLayoutCombo();
            this.InitializePrinterCombo();
            this.InitializeVisibleFieldsList();
            this.SetupTabOrder();

            m_initialized = true;

            this.InvalidatePreview();

            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="Form.Closing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel)
            {
                m_gridLiveData.ReportSettings.Save();
                this.SaveFormBoundsToRegistry();
            }
        }

        private void SetupTabOrder()
        {
            int tabIndex = 0;

            this.groupReport.TabIndex = tabIndex++;
            this.labelTitle.TabIndex = tabIndex++;
            this.textTitle.TabIndex = tabIndex++;
            this.groupReportStyle.TabIndex = tabIndex++;
            this.gridReportStyleList.TabIndex = tabIndex++;
            this.butNewStyle.TabIndex = tabIndex++;
            this.butEditStyle.TabIndex = tabIndex++;
            this.butRemoveStyle.TabIndex = tabIndex++;
            this.groupColumns.TabIndex = tabIndex++;
            this.panelColumnList.TabIndex = tabIndex++;
            this.panelColumnsButtons.TabIndex = tabIndex++;
            this.labelLayout.TabIndex = tabIndex++;
            this.comboLayout.TabIndex = tabIndex++;
            this.butResetLayout.TabIndex = tabIndex++;
            this.labelPrinter.TabIndex = tabIndex++;
            this.comboPrinter.TabIndex = tabIndex++;
            this.butPageSetup.TabIndex = tabIndex++;
            this.butResetPrinter.TabIndex = tabIndex++;
            this.butPrint.TabIndex = tabIndex++;
            this.butPreview.TabIndex = tabIndex++;
            this.butExport.TabIndex = tabIndex++;
            this.butClose.TabIndex = tabIndex++;

            this.gridReportStyleList.Select();
        }

        #endregion Form Initialization

        #region ReportTitle

        private void textTitle_Leave(object sender, System.EventArgs e)
        {
            if (m_gridLiveData.ReportSettings.Title != textTitle.Text)
            {
                m_gridLiveData.ReportSettings.Title = textTitle.Text;
                // Setting m_currentPrintDocument to null will have the effect of creating a new 
                // Report instance (with the new title) in the following InvalidatePreview.
                m_currentPrintDocument = null;
                this.InvalidatePreview();
            }
        }

        #endregion ReportTitle

        #region ReportStyle List

        private GridControl gridReportStyleList = new GridControl();

        private void InitializeReportStyleList()
        {
            this.gridReportStyleList.Dock = DockStyle.Fill;
            this.gridReportStyleList.BorderStyle = BorderStyle.FixedSingle;
            this.gridReportStyleList.GridLineColor = Color.Transparent;
            this.gridReportStyleList.SelectionMode = SelectionMode.One;
            this.gridReportStyleList.AllowCellNavigation = false;
            this.gridReportStyleList.RowSelectorPane.Visible = false;
            this.gridReportStyleList.ScrollBars = GridScrollBars.Vertical;
            this.gridReportStyleList.FixedColumnSplitter.Visible = false;
            this.gridReportStyleList.SelectedRowsChanged += new EventHandler(gridReportStyleList_SelectedRowsChanged);

            this.gridReportStyleList.SizeChanged += new EventHandler(gridReportStyleList_SizeChanged);

            this.gridReportStyleList.Columns.Add(new Column("Name", typeof(string)));
            this.gridReportStyleList.Columns.Add(new Column("ReportStyleSheet", typeof(ReportStyleSheet)));
            this.gridReportStyleList.Columns.Add(new Column("FileName", typeof(string)));

            // Change to Chinese
            this.gridReportStyleList.Columns["Name"].Title = "名字";
            this.gridReportStyleList.Columns["ReportStyleSheet"].Title = "报表样式";
            this.gridReportStyleList.Columns["FileName"].Title = "文件名";

            this.gridReportStyleList.Columns["ReportStyleSheet"].Visible = false;
            this.gridReportStyleList.Columns["FileName"].Visible = false;

            this.panelReportStyleList.Controls.Add(this.gridReportStyleList);

            if (string.IsNullOrEmpty(m_styleFolderGlobal))
                this.panelStyleButtons.Visible = false;

            this.PopulateReportStyleList(string.Empty);
        }

        private void gridReportStyleList_SelectedRowsChanged(object sender, EventArgs e)
        {
            bool enableEditing = false;

            if (this.gridReportStyleList.SelectedRows.Count > 0)
            {
                DataRow selectedRow = this.gridReportStyleList.SelectedRows[0] as DataRow;
                enableEditing = !selectedRow.ReadOnly;
            }

            this.butEditStyle.Enabled = enableEditing;
            this.butRemoveStyle.Enabled = enableEditing;

            m_currentPrintDocument = null;
            this.InvalidatePreview();
        }

        private void gridReportStyleList_SizeChanged(object sender, EventArgs e)
        {
            this.gridReportStyleList.Columns["Name"].Width = this.gridReportStyleList.DisplayRectangle.Width;
        }

        private void PopulateReportStyleList(string styleNameToSelect)
        {
            this.gridReportStyleList.DataRows.Clear();

            DataRow newRow;

            //newRow = this.gridReportStyleList.DataRows.AddNew();
            //newRow.Cells["Name"].Value = "Default";
            //newRow.Cells["ReportStyleSheet"].Value = ReportStyleSheet.Default;
            //newRow.ReadOnly = true;
            //newRow.EndEdit();

            if (!string.IsNullOrEmpty(m_styleFolder) || !string.IsNullOrEmpty(m_styleFolderGlobal))
            {
                string[] ss = new string[] { m_styleFolder, m_styleFolderGlobal, m_styleFolderGlobal + "..\\..\\" };
                foreach (string folder in ss)
                {
                    if (string.IsNullOrEmpty(folder))
                        continue;
                    if (!Directory.Exists(folder))
                        continue;

                    foreach (string fileName in Directory.GetFiles(folder, "*.xml"))
                    {
                        ReportStyleSheet styleSheet;

                        try
                        {
                            styleSheet = ReportStyleSheet.Load(fileName);
                        }
                        catch
                        {
                            continue;
                        }

                        newRow = this.gridReportStyleList.DataRows.AddNew();
                        newRow.Cells["Name"].Value = Path.GetFileNameWithoutExtension(fileName);
                        newRow.Cells["ReportStyleSheet"].Value = styleSheet;
                        newRow.Cells["FileName"].Value = fileName;
                        newRow.EndEdit();
                    }
                }
            }
            else
            {
                foreach (ReportStyleSheet styleSheet in ReportStyleSheet.StockStyleSheets)
                {
                    newRow = this.gridReportStyleList.DataRows.AddNew();
                    newRow.Cells["Name"].Value = styleSheet.Name;
                    newRow.Cells["ReportStyleSheet"].Value = styleSheet;
                    newRow.EndEdit();
                }
            }

            if (styleNameToSelect.Length > 0)
            {
                foreach (DataRow dataRow in this.gridReportStyleList.DataRows)
                {
                    if ((string)dataRow.Cells["Name"].Value == styleNameToSelect)
                    {
                        this.gridReportStyleList.SelectedRows.Clear();
                        dataRow.IsSelected = true;
                        dataRow.BringIntoView();
                        break;
                    }
                }
            }
        }

        #endregion ReportStyle List

        #region VisibleFields List

        private VisibleFieldsGridControl gridVisibleFields = new VisibleFieldsGridControl();

        private class VisibleFieldsGridControl : GridControl
        {
            public VisibleFieldsGridControl()
                : base()
            {
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                m_selectedRow = (this.SelectedRows.Count == 0) ? null : this.SelectedRows[0];

                base.OnMouseDown(e);
            }

            private Row m_selectedRow;
            public Row SelectedRow
            {
                get { return m_selectedRow; }
            }
        }

        #region WidthColumnUnit Enum

        private enum WidthColumnUnit
        {
            Centimeters,
            Inches,
            Weight
        }

        #endregion WidthColumnUnit Enum

        #region ColumnProxy

        private class ColumnProxy
        {
            public static ColumnProxy[] FromGrid(GridControl grid, out bool hasDetailGridColumns)
            {
                if (grid == null)
                    throw new ArgumentNullException("grid");

                ArrayList proxies = new ArrayList(grid.Columns.Count);

                foreach (Column column in grid.Columns)
                {
                    if (ColumnProxy.ShouldAddColumnToProxy(column))
                    {
                        proxies.Add(new ColumnProxy(column));
                    }
                }

                // Assume for now that there are no detail grid columns
                hasDetailGridColumns = false;

                /*
                 * --UNSUPPORTED EXPERIMENTAL--
                 * 
                 * By default, this Form does not show the columns from detail grids
                 * in its "Column Settings" list.
                 * 
                 * This means that you cannot set the visiblity or change the width of
                 * these detail grid columns from within this Form.
                 * 
                 * Since each data row can have different detail grids with different
                 * columns, supporting a general
                 * case that handles every possible combination and displays column names
                 * in a functionnal and pleasent manner is beyond the scope of a helper
                 * form.
                 * 
                 * We do however offer you a basic algorithm that will display detail grid
                 * columns for most basic master/detail grids. The algorithm is located in the
                 * ColumnProxy.AddDetailGridColumnsFromDataRows() method and its invocation is
                 * commented out by default. To enable it, just uncomment the call below.
                 * 
                 * As the name implies, the algorithm looks at the supplied grid's live data rows.
                 * As it is written now, it will, however, only work correctly if <see cref="Xceed.Grid.GridControl.SynchronizeDetailGrids"/> is true.
                 * Please look at ColumnProxy.AddDetailGridColumnsFromDataRows()'s summary for details.
                 * 
                 * Also not addressed is the possible need to save the detail grid column's
                 * visibility and width as is done with the regular columns. You will have to
                 * implement your own saving system for these columns.
                 * 
                 * Understand that Xceed does not guarantee that detail columns will show up
                 * correctly in the "Column Settings" list of this Form nor will we provide
                 * support for the code in AddDetailGridColumnsFromDataRows(). It is provided
                 * as a sample, an example you can expand to suit to the complexity of your grids
                 * and to the desired visual presentation of the column titles. */

                /* TODO: Uncomment the code here to enable adding of the detail grid columns */
                // Add columns from the grid's data rows
                //hasDetailGridColumns = ColumnProxy.AddDetailGridColumnsFromDataRows( grid, proxies, grid.DataRows, 0 );

                return (ColumnProxy[])proxies.ToArray(typeof(ColumnProxy));
            }

            private static bool ShouldAddColumnToProxy(Column column)
            {
                bool shouldAdd = false;

                if (!typeof(IList).IsAssignableFrom(column.DataType) ||
                    typeof(byte[]).IsAssignableFrom(column.DataType))
                {
                    shouldAdd = true;
                }

                return shouldAdd;
            }

            /// <summary>
            /// Looks for and recursively adds ColumnProxy objects, based on detail
            /// grids from the supplied data row collection, to the
            /// supplied proxy collection.
            /// </summary>
            /// <remarks><para>
            /// This method is provided as an example, a starting point for you to modify
            /// to your specific needs.
            /// 
            /// As the name implies, the algorithm looks at data rows to find detail grids
            /// and their cplumns. It does not scan detail grid templates. There are, however, limitations
            /// to this algorithm, especially when GridControl.SynchronizeDetailGrids is false.
            /// Please look at the block comments in the code below for details.
            /// 
            /// The algorithm does not compute a 'fancy' column title for the detail grid
            /// columns, it uses the column object's title.
            /// 
            /// Understand that Xceed does not guarantee that detail columns will show up
            /// correctly in the "Column Settings" list of this Form nor will we provide
            /// support for the code in this particular method. It is provided
            /// as a sample, an example you can expand to suit to the complexity of your grids
            /// and to the desired visual presentation of the column titles.
            /// 
            /// </para></remarks>
            /// <param name="grid">Grid control on which we are operating</param>
            /// <param name="proxies">Collection to which the ColumnProxy objects will be added</param>
            /// <param name="dataRows">List of data rows that will be scanned for detail grids. Can be null</param>
            /// <param name="nestingLevel">Current number of recurse levels. Could be used to help build column titles</param>
            /// <returns>true is columns have been added by the method, false otherwise</returns>
            private static bool AddDetailGridColumnsFromDataRows(GridControl grid, ArrayList proxies, DataRowList dataRows, int nestingLevel)
            {
                bool columnsAdded;
                ReadOnlyDetailGridList detailGrids;
                ColumnProxy columnProxy;
                string title;

                // Assume no columns will be added
                columnsAdded = false;

                // If we have data rows
                if (dataRows != null && dataRows.Count > 0)
                {
                    /* If GridControl.SynchronizeDetailGrids is true, we can safely assume that
                     * all the detail grids in the supplied DataRows collection are the same so we only
                     * need to look at the first one.
                     * 
                     * If GridControl.SynchronizeDetailGrids is false, the detail grids
                     * in the supplied collection can all be different. Reliably detecting
                     * the changes between different detail grids, in every situation, is a
                     * huge undertaking we can't support right now, so we'll stick with looking
                     * at the first data row in this case also. */

                    /*
                    // If we cannot assume that all detail grids in the supplied collection are the same
                    if( !grid.SynchronizeDetailGrids )
                    {
                      // TODO: If you really need to have better detail grid support when 
                      // GridControl.SynchronizeDetailGrids is false you can implement that support
                      // here.
                    }
                    */

                    // Get the first data row's detail grid collection
                    detailGrids = dataRows[0].DetailGrids;

                    // Go through each detail grid in the current detail grid collection
                    foreach (DetailGrid detailGrid in detailGrids)
                    {
                        // Recurse into the current detail grid's data rows while keeping note
                        // if columns were added
                        columnsAdded = ColumnProxy.AddDetailGridColumnsFromDataRows(grid, proxies, detailGrid.DataRows, nestingLevel + 1) || columnsAdded;

                        // Go through each column in the current detail grid
                        foreach (Column column in detailGrid.Columns)
                        {
                            // If we should add the column to the proxy collection
                            if (ColumnProxy.ShouldAddColumnToProxy(column))
                            {
                                /* TODO: Compute a more fancy title. Perhaps using the current detail grid
                                 * title, the nesting level or some other information */

                                title = column.Title;

                                // Create a column proxy based on the current column
                                columnProxy = new ColumnProxy(column, title);

                                // Add the column to the supplied proxy collection
                                proxies.Add(columnProxy);

                                // Columns have beed added
                                columnsAdded = true;
                            }
                        }
                    }
                }

                return columnsAdded;
            }

            public static WidthColumnUnit DisplayUnit = WidthColumnUnit.Inches;

            public ColumnProxy(Column column)
                : this(column, null)
            {

            }

            public ColumnProxy(Column column, string title)
            {
                if (column == null)
                    throw new ArgumentNullException("column");

                m_column = column;
                m_title = title;
            }

            public bool Visible
            {
                get { return m_column.ReportStyle.Visible; }
                set { m_column.ReportStyle.Visible = value; }
            }

            public string Title
            {
                get
                {
                    if (m_title != null)
                    {
                        return m_title;
                    }
                    else
                    {
                        return m_column.Title;
                    }
                }

                set
                {
                    /* null is an acceptable value here */

                    m_title = value;
                }
            }

            public double Width
            {
                get
                {
                    if (DisplayUnit == WidthColumnUnit.Weight)
                    {
                        return m_column.ReportStyle.Width;
                    }
                    else
                    {
                        bool inCentimeters = (DisplayUnit == WidthColumnUnit.Centimeters);

                        return Math.Round(
                          UnitConverter.FromHundredthsOfAnInch(m_column.ReportStyle.Width,
                          inCentimeters ? Unit.Centimeters : Unit.Inches),
                          inCentimeters ? 1 : 2);
                    }
                }

                set
                {
                    switch (DisplayUnit)
                    {
                        case WidthColumnUnit.Centimeters:
                            m_column.ReportStyle.Width = UnitConverter.ToHundredthsOfAnInch(value, Unit.Centimeters);
                            break;
                        case WidthColumnUnit.Inches:
                            m_column.ReportStyle.Width = UnitConverter.ToHundredthsOfAnInch(value, Unit.Inches);
                            break;
                        case WidthColumnUnit.Weight:
                            m_column.ReportStyle.Width = (int)value;
                            break;
                    }
                }
            }

            private Column m_column;
            private string m_title; // = null;
        }

        #endregion ColumnProxy

        private void InitializeVisibleFieldsList()
        {
            this.gridVisibleFields.Dock = DockStyle.Fill;
            this.gridVisibleFields.BorderStyle = BorderStyle.FixedSingle;
            this.gridVisibleFields.GridLineColor = Color.Transparent;
            this.gridVisibleFields.SelectionMode = SelectionMode.One;
            this.gridVisibleFields.RowSelectorPane.Visible = false;
            this.gridVisibleFields.AllowCellNavigation = false;
            this.gridVisibleFields.ScrollBars = GridScrollBars.Vertical;
            this.gridVisibleFields.FixedColumnSplitter.Visible = false;

            //
            // Configure column manager row
            //

            ColumnManagerRow columnManagerRow = new ColumnManagerRow();
            this.gridVisibleFields.FixedHeaderRows.Add(columnManagerRow);
            columnManagerRow.AllowColumnReorder = false;
            columnManagerRow.AllowColumnResize = false;
            columnManagerRow.AllowSort = false;

            //
            // Configure columns
            //

            // "Visible" column

            DataBoundColumn visibleColumn = new DataBoundColumn("Visible");
            this.gridVisibleFields.Columns.Add(visibleColumn);

            visibleColumn.Title = "";
            visibleColumn.Width = 24;

            this.gridVisibleFields.DataRowTemplate.Cells[visibleColumn.Index].Click += new EventHandler(this.gridVisibleFields_Visible_Click);
            this.gridVisibleFields.DataRowTemplate.Cells[visibleColumn.Index].CanBeCurrent = false;

            // "Title" column

            DataBoundColumn titleColumn = new DataBoundColumn("Title");
            this.gridVisibleFields.Columns.Add(titleColumn);

            titleColumn.Title = "Name";

            this.gridVisibleFields.DataRowTemplate.Cells[titleColumn.Index].CanBeCurrent = false;

            // "Width" column

            DataBoundColumn widthColumn = new DataBoundColumn("Width");
            this.gridVisibleFields.Columns.Add(widthColumn);

            NumericEditor numEditor = new NumericEditor();
            numEditor.TemplateControl.Controls.Remove(numEditor.TemplateControl.DropDownButton);
            widthColumn.CellEditorManager = numEditor;

            this.InitializeWidthColumnUnit();

            Cell widthCellTemplate = this.gridVisibleFields.DataRowTemplate.Cells["Width"];

            widthCellTemplate.Click += new EventHandler(this.gridVisibleFields_Width_Click);

            // "EditWidth" column

            Column editWidthColumn = new Column("EditWidth", typeof(Image));
            this.gridVisibleFields.Columns.Add(editWidthColumn);

            // Change to Chinese
            this.gridVisibleFields.Columns["Title"].Title = "名字";
            this.gridVisibleFields.Columns["Width"].Title = "宽度";
            this.gridVisibleFields.Columns["EditWidth"].Title = "编辑宽度";

            editWidthColumn.Title = "";
            editWidthColumn.Width = 24;

            Cell editWidthCellTemplate = this.gridVisibleFields.DataRowTemplate.Cells["EditWidth"];

            editWidthCellTemplate.Click += new EventHandler(this.gridVisibleFields_EditWidth_Click);
            editWidthCellTemplate.MouseEnter += new EventHandler(this.HandCursor_MouseEnter);
            editWidthCellTemplate.MouseLeave += new EventHandler(this.HandCursor_MouseLeave);
            editWidthCellTemplate.CanBeCurrent = false;

            //
            // Configure Data
            //
            this.gridVisibleFields.DataRowTemplate.Cells[visibleColumn.Index].ValueChanged += new EventHandler(gridVisibleFields_ValueChanged);
            this.gridVisibleFields.DataRowTemplate.Cells[widthColumn.Index].ValueChanged += new EventHandler(gridVisibleFields_ValueChanged);
            this.gridVisibleFields.DataRowTemplate.KeyDown += new KeyEventHandler(this.gridVisibleFields_DataRow_KeyDown);
            this.gridVisibleFields.DataRowTemplate.KeyPress += new KeyPressEventHandler(gridVisibleFields_DataRow_KeyPress);
            this.gridVisibleFields.DataRowTemplate.Cells[widthColumn.Index].EditLeft += new EditLeftEventHandler(this.gridVisibleFields_Width_EditLeft);
            this.gridVisibleFields.DataRowTemplate.EditEnded += new EventHandler(this.gridVisibleFields_DataRow_EditEnded);
            this.gridVisibleFields.DataRowTemplate.EditCanceled += new EventHandler(this.gridVisibleFields_DataRow_EditCanceled);
            this.gridVisibleFields.AddingDataRow += new AddingDataRowEventHandler(this.gridVisibleFields_AddingDataRow);

            this.gridVisibleFields.DataSource = ColumnProxy.FromGrid(m_gridLiveData, out m_hasDetailGridColumns);

            this.gridVisibleFields.SizeChanged += new EventHandler(gridVisibleFields_SizeChanged);
            this.panelColumnList.Controls.Add(this.gridVisibleFields);
        }

        private void InitializeWidthColumnUnit()
        {
            Column widthColumn = this.gridVisibleFields.Columns["Width"];

            if (m_gridLiveData.ReportSettings.ColumnLayout == ColumnLayout.FitToPage)
            {
                widthColumn.Title = "Weight";
                widthColumn.FormatSpecifier = "0";
                ColumnProxy.DisplayUnit = WidthColumnUnit.Weight;
            }
            else
            {
                widthColumn.Title = "Width";

                if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
                {
                    widthColumn.FormatSpecifier = "0.0 cm";
                    ColumnProxy.DisplayUnit = WidthColumnUnit.Centimeters;
                }
                else
                {
                    widthColumn.FormatSpecifier = "0.00 inches";
                    ColumnProxy.DisplayUnit = WidthColumnUnit.Inches;
                }
            }
        }

        private void gridVisibleFields_Width_EditLeft(object sender, EditLeftEventArgs e)
        {
            Cell cell = (Cell)sender;

            if (e.Commited)
            {
                cell.ParentRow.EndEdit();
            }
            else
            {
                cell.ParentRow.CancelEdit();
            }
        }

        private void gridVisibleFields_DataRow_EditEnded(object sender, EventArgs e)
        {
            DataRow dataRow = (DataRow)sender;

            this.gridVisibleFields.CurrentColumn = null;
            dataRow.AllowCellNavigation = false;
        }

        private void gridVisibleFields_DataRow_EditCanceled(object sender, EventArgs e)
        {
            DataRow dataRow = (DataRow)sender;

            this.gridVisibleFields.CurrentColumn = null;
            dataRow.AllowCellNavigation = false;
        }

        private void gridVisibleFields_Visible_Click(object sender, EventArgs e)
        {
            Cell cell = sender as Cell;

            if (cell == null)
                return;

            if (cell.ParentRow.IsBeingEdited)
                return;

            cell.Value = !((bool)cell.Value);
        }

        private void gridVisibleFields_Width_Click(object sender, EventArgs e)
        {
            Cell cell = sender as Cell;

            if (cell == null)
                return;

            CellRow parentRow = cell.ParentRow;

            if ((this.gridVisibleFields.SelectedRow != parentRow) || (parentRow.IsBeingEdited))
                return;

            parentRow.AllowCellNavigation = true;
            parentRow.Cells["Width"].EnterEdit();
        }

        private void gridVisibleFields_EditWidth_Click(object sender, EventArgs e)
        {
            Cell cell = sender as Cell;

            if (cell == null)
                return;

            CellRow parentRow = cell.ParentRow;

            if (parentRow.IsBeingEdited)
                return;

            parentRow.AllowCellNavigation = true;
            parentRow.Cells["Width"].EnterEdit();
        }

        private void HandCursor_MouseEnter(object sender, EventArgs e)
        {
            this.gridVisibleFields.Cursor = Cursors.Hand;
        }

        private void HandCursor_MouseLeave(object sender, EventArgs e)
        {
            this.gridVisibleFields.Cursor = Cursors.Default;
        }

        private void UpdateVisibleFieldNameColumnWidth()
        {
            this.gridVisibleFields.Columns["Title"].Width =
              this.gridVisibleFields.DisplayRectangle.Width -
                this.gridVisibleFields.Columns["Visible"].Width -
                this.gridVisibleFields.Columns["Width"].Width -
                this.gridVisibleFields.Columns["EditWidth"].Width;
        }

        private void gridVisibleFields_SizeChanged(object sender, EventArgs e)
        {
            this.UpdateVisibleFieldNameColumnWidth();
        }

        private void gridVisibleFields_ValueChanged(object sender, EventArgs e)
        {
            this.InvalidatePreview();
        }

        private void gridVisibleFields_DataRow_KeyPress(object sender, KeyPressEventArgs e)
        {
            Xceed.Grid.DataRow row = sender as Xceed.Grid.DataRow;

            if (row == null)
                return;

            Cell cellWidth = row.Cells["Width"];

            WinNumericTextBox numEditorTemplate = (WinNumericTextBox)cellWidth.CellEditorManager.TemplateControl;

            if ((Array.IndexOf(numEditorTemplate.DigitInputChars, e.KeyChar) == -1) &&
              (Array.IndexOf(numEditorTemplate.DecimalSeparatorInputChars, e.KeyChar) == -1))
            {
                return;
            }

            row.AllowCellNavigation = true;

            cellWidth.EnterEdit();

            WinNumericTextBox numEditor = (WinNumericTextBox)cellWidth.CellEditorControl;

            numEditor.TextBoxArea.Text = e.KeyChar.ToString();
            numEditor.TextBoxArea.SelectionStart = 1;
            numEditor.TextBoxArea.SelectionLength = 0;

            e.Handled = true;
        }

        private void gridVisibleFields_DataRow_KeyDown(object sender, KeyEventArgs e)
        {
            Xceed.Grid.DataRow row = sender as Xceed.Grid.DataRow;

            if (row == null)
                return;


            switch (e.KeyCode)
            {
                case Keys.Space:
                    {
                        Cell cellVisible = row.Cells["Visible"];
                        cellVisible.Value = !((bool)cellVisible.Value);
                        e.Handled = true;
                        break;
                    }
                case Keys.F2:
                    {
                        if (row.IsBeingEdited)
                            return;

                        row.AllowCellNavigation = true;
                        row.Cells["Width"].EnterEdit();
                        e.Handled = true;
                        break;
                    }
            }
        }

        private void gridVisibleFields_AddingDataRow(object sender, AddingDataRowEventArgs e)
        {
            e.DataRow.Cells["EditWidth"].Value = this.imageList.Images[0];
        }

        #endregion VisibleFields List

        #region Layout Combo

        #region LayoutComboItem Class

        private class LayoutComboItem
        {
            public LayoutComboItem(ColumnLayout columnLayout, string description)
            {
                ColumnLayout = columnLayout;
                Description = description;
            }

            public ColumnLayout ColumnLayout;

            public string Description;

            public override string ToString()
            {
                return Description;
            }
        }

        #endregion LayoutComboItem Class

        private void InitializeLayoutCombo()
        {
            this.comboLayout.Items.Clear();
            m_defaultLayoutIndex = this.comboLayout.Items.Add(new LayoutComboItem(ColumnLayout.SpanAcrossPages, "横跨页面" /*"Span across pages"*/));
            this.comboLayout.Items.Add(new LayoutComboItem(ColumnLayout.FitToPage, "适合页面"/*"Fit to page"*/));

            foreach (LayoutComboItem item in this.comboLayout.Items)
            {
                if (item.ColumnLayout == m_gridLiveData.ReportSettings.ColumnLayout)
                {
                    this.comboLayout.SelectedItem = item;
                    break;
                }
            }
        }

        private void ApplyNewLayoutValue()
        {
            bool changeWidthWeight = (m_gridLiveData.ReportSettings.ColumnLayout == ColumnLayout.FitToPage) ||
              (((LayoutComboItem)this.comboLayout.SelectedItem).ColumnLayout == ColumnLayout.FitToPage);

            m_gridLiveData.ReportSettings.ColumnLayout = ((LayoutComboItem)this.comboLayout.SelectedItem).ColumnLayout;

            if (changeWidthWeight)
                this.InitializeWidthColumnUnit();

            // Setting m_currentPrintDocument to null will have the effect of creating a new 
            // Report instance (with the new layout value) in the following InvalidatePreview.
            m_currentPrintDocument = null;
            this.InvalidatePreview();
        }

        private void ResetLayoutCombo()
        {
            this.comboLayout.SelectedIndex = m_defaultLayoutIndex;
            this.ApplyNewLayoutValue();
        }

        private void comboLayout_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            this.ApplyNewLayoutValue();
        }

        #endregion Layout Combo

        #region Printer Combo

        private void InitializePrinterCombo()
        {
            string preferredPrinter = m_gridLiveData.ReportSettings.PrinterName;
            string defaultPrinter = new System.Drawing.Printing.PrintDocument().PrinterSettings.PrinterName;

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                int index = this.comboPrinter.Items.Add(printer);

                if (printer == preferredPrinter)
                {
                    this.comboPrinter.SelectedIndex = index;
                    this.CurrentPrinterSettings.PrinterName = printer;
                }

                if ((this.comboPrinter.SelectedIndex < 0) && (printer == defaultPrinter))
                    this.comboPrinter.SelectedIndex = index;
            }
        }

        private void SetSelectedPrinter()
        {
            this.comboPrinter.SelectedItem = this.CurrentPrinterSettings.PrinterName;
        }

        private void comboPrinter_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            this.CurrentPrinterSettings.PrinterName = (string)this.comboPrinter.SelectedItem;
            m_gridLiveData.ReportSettings.PrinterName = this.CurrentPrinterSettings.PrinterName;
        }

        #endregion Printer Combo

        #region Sample Page

        private ReportPreviewControl reportPreview = new ReportPreviewControl();

        private void InitializeSamplePage()
        {
            this.reportPreview.Dock = DockStyle.Fill;
            this.panelSamplePage.Controls.Add(this.reportPreview);
        }

        private void InvalidatePreview()
        {
            if (!m_initialized)
                return;

            if (m_report == null)
            {
                m_report = new Report(m_gridLiveData, this.CurrentStyleSheet);
            }
            else
            {
                m_report.ReportStyleSheet = this.CurrentStyleSheet;
            }

            // In preview mode, we don't want to calculate the total number of pages needlessly
            // (we only display the first page). This results in a faster refresh.
            bool oldValue = m_report.CalculateTotalPages;
            m_report.CalculateTotalPages = false;
            this.reportPreview.PrintDocument = this.CurrentPrintDocument;
            m_report.CalculateTotalPages = oldValue;
        }

        #endregion SamplePage

        #region Button Click Handlers

        private void butPageSetup_Click(object sender, EventArgs e)
        {
            bool currentColor = this.CurrentPageSettings.Color;
            bool currentLandscape = this.CurrentPageSettings.Landscape;
            System.Drawing.Printing.Margins currentMargins = this.CurrentPageSettings.Margins;
            PaperSize currentPaperSize = this.CurrentPageSettings.PaperSize;
            string currentPaperSourceName = this.CurrentPageSettings.PaperSource.SourceName;

            this.pageSetupDialog.Document = this.CurrentPrintDocument;

            if (this.pageSetupDialog.ShowDialog(this) == DialogResult.OK)
            {
                PageSettings newSettings = this.pageSetupDialog.PageSettings;

                if (newSettings.Color != currentColor)
                    m_gridLiveData.ReportSettings.Color = newSettings.Color;

                if (newSettings.Landscape != currentLandscape)
                    m_gridLiveData.ReportSettings.Landscape = newSettings.Landscape;

                if ((newSettings.Margins.Left != currentMargins.Left)
                  || (newSettings.Margins.Top != currentMargins.Top)
                  || (newSettings.Margins.Right != currentMargins.Right)
                  || (newSettings.Margins.Bottom != currentMargins.Bottom))
                    m_gridLiveData.ReportSettings.Margins = newSettings.Margins;

                if ((newSettings.PaperSize.Height != currentPaperSize.Height)
                  || (newSettings.PaperSize.PaperName != currentPaperSize.PaperName)
                  || (newSettings.PaperSize.Width != currentPaperSize.Width))
                    m_gridLiveData.ReportSettings.PaperSize = newSettings.PaperSize;

                if (newSettings.PaperSource.SourceName != currentPaperSourceName)
                    m_gridLiveData.ReportSettings.PaperSourceName = newSettings.PaperSource.SourceName;

                this.SetSelectedPrinter();
                this.InvalidatePreview();
            }
        }

        private void butPrint_Click(object sender, EventArgs e)
        {
            try
            {
                // The same Report/PrintDocument is used to display the preview of the report on 
                // this form and to print the report. In preview mode, the Total Pages calculation 
                // is deactivated. We want to reactivate it before starting the print process.
                bool oldValue = m_report.CalculateTotalPages;
                m_report.CalculateTotalPages = true;
                this.CurrentPrintDocument.Print();
                m_report.CalculateTotalPages = oldValue;
            }
            catch (Win32Exception)
            {
                // We get a Win32Exception when the user cancels the printing process. Don't let it get out.
            }
        }

        private void butPreview_Click(object sender, EventArgs e)
        {
            // The same Report/PrintDocument is used to display the preview of the report on 
            // this form (first page only) and to PrintPreview the whole report. In the single-page 
            // preview mode, the Total Pages calculation is deactivated. We want to reactivate it 
            // before starting the complete PrintPreview process.
            bool oldValue = m_report.CalculateTotalPages;
            m_report.CalculateTotalPages = true;
            this.printPreviewDialog.Document = this.CurrentPrintDocument;
            this.printPreviewDialog.ShowDialog(this);
            //MyGridPrintDocument gridPrintDocument = new MyGridPrintDocument(m_gridLiveData);
            //PrintPreviewForm printPreviewForm = new PrintPreviewForm(gridPrintDocument);
            //printPreviewForm.ShowDialog(this);
            m_report.CalculateTotalPages = oldValue;
        }

        private void butExport_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.FileName = string.Empty;

            if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                ExportFormat exportFormat = this.ConvertExportFileDialogFilterToExportFormat(this.saveFileDialog.FilterIndex);

                ExportSettings settings = new ExportSettings();
                settings.Landscape = this.CurrentPageSettings.Landscape;
                settings.Margins = this.CurrentPageSettings.Margins;
                settings.PaperSize = this.CurrentPageSettings.PaperSize;

                // The same Report/PrintDocument is used to display the preview of the report on 
                // this form and to export the report. In preview mode, the Total Pages calculation 
                // is deactivated. We want to reactivate it before starting the export process.
                bool oldValue = m_report.CalculateTotalPages;
                m_report.CalculateTotalPages = true;

                try
                {
                    m_report.Export(this.saveFileDialog.FileName, exportFormat, true, true, settings);
                }
                catch (Exception exception)
                {
                    /* Something happened during export.
                     * 
                     * Although the export code itself should not throw exceptions, we must
                     * expect this to happen because of common situations like:
                     * 
                     * - The file chosen by the user is opened by another process.
                     * - The file chosen by the user is otherwise not accessible or writable.
                     */

                    // Warn the user
                    System.Windows.Forms.MessageBox.Show(
                      String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}\n\n({1})", exception.Message, exception.GetType().FullName),
                      "Export - Problem",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Exclamation);
                }

                m_report.CalculateTotalPages = oldValue;
            }
        }

        private void butNewStyle_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;

            for (int i = 1; i < 1000; i++)
            {
                Feng.Utils.IOHelper.TryCreateDirectory(m_styleFolder);

                string f = Path.Combine(m_styleFolder, "ReportStyle" + i.ToString() + ".xml");

                if (!File.Exists(f))
                {
                    fileName = f;
                    break;
                }
            }

            if (fileName.Length == 0)
                return;

            CustomizeReportStyleForm customizeForm = new CustomizeReportStyleForm(fileName);

            if (customizeForm.ShowDialog(this) == DialogResult.OK)
                this.PopulateReportStyleList(customizeForm.StyleName);
        }

        private void butEditStyle_Click(object sender, EventArgs e)
        {
            if (this.gridReportStyleList.SelectedRows.Count == 0)
                return;

            DataRow selectedRow = this.gridReportStyleList.SelectedRows[0] as DataRow;

            CustomizeReportStyleForm customizeForm = new CustomizeReportStyleForm((string)selectedRow.Cells["FileName"].Value);

            if (customizeForm.ShowDialog(this) == DialogResult.OK)
                this.PopulateReportStyleList(customizeForm.StyleName);
        }

        private void butRemoveStyle_Click(object sender, EventArgs e)
        {
            if (this.gridReportStyleList.SelectedRows.Count == 0)
                return;

            DataRow selectedRow = this.gridReportStyleList.SelectedRows[0] as DataRow;

            DialogResult result = System.Windows.Forms.MessageBox.Show(
              this,
              string.Format("Are you sure you want to permanently remove the style named '{0}'?", selectedRow.Cells["Name"].Value),
              "Remove Style Confirmation",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string fileName = (string)selectedRow.Cells["FileName"].Value;
                selectedRow.Remove();
                this.gridReportStyleList.SelectedRows.Clear();
                if (this.gridReportStyleList.DataRows.Count > 0)
                {
                    this.gridReportStyleList.SelectedRows.Add(this.gridReportStyleList.DataRows[0]);
                    this.gridReportStyleList.CurrentRow = this.gridReportStyleList.DataRows[0];
                }

                if ((fileName != null) && (fileName.Length > 0))
                    File.Delete(fileName); //#TODO: Handle exceptions
            }
        }

        private void butResetLayout_Click(object sender, System.EventArgs e)
        {
            foreach (Column col in m_gridLiveData.Columns)
            {
                if (col.IsReportStyleDefined)
                {
                    col.ReportStyle.ResetVisible();
                    col.ReportStyle.ResetWidth();
                }
            }

            this.ResetLayoutCombo();

            this.gridVisibleFields.Invalidate(true);
            this.InvalidatePreview();
        }

        private void butResetPrinter_Click(object sender, System.EventArgs e)
        {
            ReportSettings settings = m_gridLiveData.ReportSettings;

            settings.ResetColor();
            settings.ResetLandscape();
            settings.ResetMargins();
            settings.ResetPrinterName();
            settings.ResetPaperSize();
            settings.ResetPaperSourceName();

            // Setting m_currentPrintDocument to null will have the effect of creating a new 
            // Report instance (with the new settings) in the following InvalidatePreview.
            m_currentPrintDocument = null;
            m_currentPrinterSettings = null;
            m_currentPageSettings = null;
            this.SetSelectedPrinter();
            this.InvalidatePreview();
        }

        private void butClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #endregion Button Click Handlers

        #region Utility Methods

        private void LoadFormBoundsFromRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(GenerateReportForm.RegistryAddress))
                {
                    object objX = null, objY = null, objW = null, objH = null;

                    objX = key.GetValue("X");

                    if (objX != null)
                        objY = key.GetValue("Y");

                    if (objY != null)
                        objW = key.GetValue("W");

                    if (objW != null)
                        objH = key.GetValue("H");

                    if (objH != null)
                    {
                        this.Bounds = new Rectangle((int)objX, (int)objY, (int)objW, (int)objH);
                        this.StartPosition = FormStartPosition.Manual;
                    }

                    object objMaximized = key.GetValue("Maximized");

                    if (objMaximized != null)
                    {
                        if ((int)objMaximized != 0)
                            this.WindowState = FormWindowState.Maximized;
                    }
                }
            }
            catch (System.Security.SecurityException) { }
            catch (System.UnauthorizedAccessException) { }
        }

        private void SaveFormBoundsToRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(GenerateReportForm.RegistryAddress))
                {
                    bool maximized = (this.WindowState == FormWindowState.Maximized);

                    key.SetValue("Maximized", maximized ? 1 : 0);

                    if (!maximized)
                    {
                        Rectangle b = this.Bounds;

                        key.SetValue("X", b.X);
                        key.SetValue("Y", b.Y);
                        key.SetValue("W", b.Width);
                        key.SetValue("H", b.Height);
                    }
                }
            }
            catch (System.Security.SecurityException) { }
            catch (System.UnauthorizedAccessException) { }
        }

        private ReportStyleSheet CurrentStyleSheet
        {
            get
            {
                if (this.gridReportStyleList.SelectedRows.Count == 0)
                    return null;

                DataRow selectedRow = this.gridReportStyleList.SelectedRows[0] as DataRow;
                return (ReportStyleSheet)selectedRow.Cells["ReportStyleSheet"].Value;
            }
        }

        private PrintDocument m_currentPrintDocument;
        private PrintDocument CurrentPrintDocument
        {
            get
            {
                if (m_report == null)
                    return null;

                if (m_currentPrintDocument == null)
                    m_currentPrintDocument = m_report.CreatePrintDocument();

                m_currentPrintDocument.PrinterSettings = this.CurrentPrinterSettings;
                m_currentPrintDocument.DefaultPageSettings = this.CurrentPageSettings;

                return m_currentPrintDocument;
            }
        }

        private PrinterSettings m_currentPrinterSettings;
        private PrinterSettings CurrentPrinterSettings
        {
            get
            {
                if (m_currentPrinterSettings == null)
                    m_currentPrinterSettings = new PrinterSettings();

                return m_currentPrinterSettings;
            }
        }

        private PageSettings m_currentPageSettings;
        private PageSettings CurrentPageSettings
        {
            get
            {
                ReportSettings settings = m_gridLiveData.ReportSettings;

                if (m_currentPageSettings == null)
                    m_currentPageSettings = new PageSettings(this.CurrentPrinterSettings);

                if (settings.IsColorDefined)
                    m_currentPageSettings.Color = settings.Color;

                if (settings.IsLandscapeDefined)
                    m_currentPageSettings.Landscape = settings.Landscape;

                if (settings.IsMarginsDefined)
                    m_currentPageSettings.Margins = settings.Margins;

                if (settings.IsPaperSizeDefined)
                    m_currentPageSettings.PaperSize = settings.PaperSize;

                if (settings.IsPaperSourceNameDefined)
                {
                    PaperSource paperSource = settings.PaperSource;

                    if (paperSource != null)
                        m_currentPageSettings.PaperSource = paperSource;
                }

                return m_currentPageSettings;
            }
        }

        private ExportFormat ConvertExportFileDialogFilterToExportFormat(int filterIndex)
        {
            ExportFormat exportFormat;

            switch (filterIndex)
            {
                case 1:
                default:
                    exportFormat = ExportFormat.Pdf;
                    break;

                case 2:
                    exportFormat = ExportFormat.Html;
                    break;

                case 3:
                    exportFormat = ExportFormat.Jpeg;
                    break;

                case 4:
                    exportFormat = ExportFormat.Tiff;
                    break;
            }

            return exportFormat;
        }

        #endregion Utility Methods

        #region Private Fields

        private GridControl m_gridLiveData; // = null
        private string m_styleFolder = string.Empty;
        private string m_styleFolderGlobal = string.Empty;
        private Report m_report; // = null
        private bool m_initialized; // = false
        private int m_defaultLayoutIndex;
        private bool m_hasDetailGridColumns; // = false;

        private static readonly string RegistryAddress = "";

        private void labelPrinter_Click(object sender, EventArgs e)
        {

        } //@"Software\Xceed Software\Xceed Grid for .NET\" + _XceedVersionInfo.BaseVersion + @"\Report Configuration Forms\GenerateReportForm";

        #endregion Private Fields
    }
}
