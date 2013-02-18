/*
 * Xceed Grid for .NET - "Customize Report Style" form - Source code for customization
 * Copyright (c) 2005 - Xceed Software Inc.
 * 
 * [CustomizeReportStyleForm.cs]
 * 
 * This file is part of Xceed Grid for .NET. The source code provided in this file 
 * is intended to allow developers invoking Xceed Grid for .NET's "Customize Report Style"
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
using System.IO;
using Microsoft.Win32;
using Xceed.Grid;
using Xceed.Grid.Reporting;

namespace Feng.Grid.Print
{
    /// <summary>
    /// Represents a form which can be used to customize a report style.
    /// </summary>
    /// <remarks><para>
    /// The CustomizeReportStyleForm is also available through the "New" and "Edit" button of the 
    /// <see cref="GenerateReportForm"/> when it is created using the overload of the constructor that
    /// accepts a report style folder as a parameter.
    /// </para><para>
    /// The CustomizeReportStyleForm does not support customizing the report styles of master/detail grids.
    /// </para></remarks>
    public class CustomizeReportStyleForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox txtStyleName;
        private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Panel panelElementList;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenu propertyGridContextMenu;
        private System.Windows.Forms.MenuItem cmiReset;
        private System.Windows.Forms.MenuItem cmiSeparator;
        private System.Windows.Forms.MenuItem cmiDescription;
        private System.Windows.Forms.LinkLabel butAddDataRow;
        private System.Windows.Forms.LinkLabel butAddGroupLevel;
        private System.Windows.Forms.GroupBox groupPreview;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Label labelStyleName;
        private System.Windows.Forms.GroupBox groupReportElements;
        private System.Windows.Forms.GroupBox groupElementProperties;
        private System.Windows.Forms.Panel panelPropertyGrid;
        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Label labelControlPlaceholder1;
        private System.Windows.Forms.Label labelControlPlaceholder2;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Intializes a new instance of the CustomizeReportStyleForm specifying the filename to which the
        /// report style will be saved.
        /// </summary>
        /// <param name="fileName">The filename to which the report style will be saved.</param>
        /// <remarks><para>
        /// The CustomizeReportStyleForm is also available through the "New" and "Edit" button of the 
        /// <see cref="GenerateReportForm"/> when it is created using the overload of the constructor that
        /// accepts a report style folder as a parameter.
        /// </para><para>
        /// The CustomizeReportStyleForm does not support customizing the report styles of master/detail grids.
        /// </para></remarks>
        public CustomizeReportStyleForm(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (fileName.Length == 0)
                throw new ArgumentException("The report stylesheet file name cannot be an empty string.", "fileName");

            m_fileName = fileName;

            if (File.Exists(m_fileName))
            {
                m_styleSheet = ReportStyleSheet.Load(m_fileName);
            }
            else
            {
                m_styleSheet = ReportStyleSheet.Default;
            }

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.LoadFormBoundsFromRegistry();

            Xceed.Grid.Reporting.Design.EndUserPropertyDescriptor.SetEndUserMode();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Gets the name of the report style.
        /// </summary>
        /// <value>A string representing the name of the report style.</value>
        /// <remarks><para>
        /// The StyleName is the name of the style passed at construction of the CustomizeReportStyleForm
        /// without the extension.
        /// </para></remarks>
        public string StyleName
        {
            get { return this.txtStyleName.Text; }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the form is disposing; <see langword="false"/> otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            Xceed.Grid.Reporting.Design.EndUserPropertyDescriptor.ResetEndUserMode();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomizeReportStyleForm));
            this.txtStyleName = new System.Windows.Forms.TextBox();
            this.groupReportElements = new System.Windows.Forms.GroupBox();
            this.butAddGroupLevel = new System.Windows.Forms.LinkLabel();
            this.butAddDataRow = new System.Windows.Forms.LinkLabel();
            this.panelElementList = new System.Windows.Forms.Panel();
            this.labelControlPlaceholder1 = new System.Windows.Forms.Label();
            this.propertyGridContextMenu = new System.Windows.Forms.ContextMenu();
            this.cmiReset = new System.Windows.Forms.MenuItem();
            this.cmiSeparator = new System.Windows.Forms.MenuItem();
            this.cmiDescription = new System.Windows.Forms.MenuItem();
            this.groupPreview = new System.Windows.Forms.GroupBox();
            this.panelPreview = new System.Windows.Forms.Panel();
            this.labelControlPlaceholder2 = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.butOk = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.groupElementProperties = new System.Windows.Forms.GroupBox();
            this.panelPropertyGrid = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.groupGeneral = new System.Windows.Forms.GroupBox();
            this.labelStyleName = new System.Windows.Forms.Label();
            this.groupReportElements.SuspendLayout();
            this.panelElementList.SuspendLayout();
            this.groupPreview.SuspendLayout();
            this.panelPreview.SuspendLayout();
            this.groupElementProperties.SuspendLayout();
            this.panelPropertyGrid.SuspendLayout();
            this.groupGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStyleName
            // 
            this.txtStyleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStyleName.Location = new System.Drawing.Point(96, 26);
            this.txtStyleName.Name = "txtStyleName";
            this.txtStyleName.Size = new System.Drawing.Size(278, 21);
            this.txtStyleName.TabIndex = 1;
            // 
            // groupReportElements
            // 
            this.groupReportElements.Controls.Add(this.butAddGroupLevel);
            this.groupReportElements.Controls.Add(this.butAddDataRow);
            this.groupReportElements.Controls.Add(this.panelElementList);
            this.groupReportElements.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupReportElements.Location = new System.Drawing.Point(10, 78);
            this.groupReportElements.Name = "groupReportElements";
            this.groupReportElements.Size = new System.Drawing.Size(384, 241);
            this.groupReportElements.TabIndex = 2;
            this.groupReportElements.TabStop = false;
            this.groupReportElements.Text = "报表元素(&E)";
            // 
            // butAddGroupLevel
            // 
            this.butAddGroupLevel.AutoSize = true;
            this.butAddGroupLevel.Location = new System.Drawing.Point(173, 215);
            this.butAddGroupLevel.Name = "butAddGroupLevel";
            this.butAddGroupLevel.Size = new System.Drawing.Size(77, 12);
            this.butAddGroupLevel.TabIndex = 4;
            this.butAddGroupLevel.TabStop = true;
            this.butAddGroupLevel.Text = "添加子组级别";
            this.butAddGroupLevel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.butAddGroupLevel_LinkClicked);
            // 
            // butAddDataRow
            // 
            this.butAddDataRow.AutoSize = true;
            this.butAddDataRow.Location = new System.Drawing.Point(60, 215);
            this.butAddDataRow.Name = "butAddDataRow";
            this.butAddDataRow.Size = new System.Drawing.Size(65, 12);
            this.butAddDataRow.TabIndex = 3;
            this.butAddDataRow.TabStop = true;
            this.butAddDataRow.Text = "添加交替行";
            this.butAddDataRow.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.butAddDataRow_LinkClicked);
            // 
            // panelElementList
            // 
            this.panelElementList.Controls.Add(this.labelControlPlaceholder1);
            this.panelElementList.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelElementList.Location = new System.Drawing.Point(3, 17);
            this.panelElementList.Name = "panelElementList";
            this.panelElementList.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
            this.panelElementList.Size = new System.Drawing.Size(378, 191);
            this.panelElementList.TabIndex = 2;
            // 
            // labelControlPlaceholder1
            // 
            this.labelControlPlaceholder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControlPlaceholder1.Enabled = false;
            this.labelControlPlaceholder1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlPlaceholder1.Location = new System.Drawing.Point(8, 8);
            this.labelControlPlaceholder1.Name = "labelControlPlaceholder1";
            this.labelControlPlaceholder1.Size = new System.Drawing.Size(362, 183);
            this.labelControlPlaceholder1.TabIndex = 1;
            this.labelControlPlaceholder1.Text = "A GridControl named gridStyleElementList, that contains a list of available repor" +
                "t style elements, will be added here at run-time. The code is in the \"Style Elem" +
                "ent List\" code region.";
            this.labelControlPlaceholder1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelControlPlaceholder1.Visible = false;
            // 
            // propertyGridContextMenu
            // 
            this.propertyGridContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmiReset,
            this.cmiSeparator,
            this.cmiDescription});
            this.propertyGridContextMenu.Popup += new System.EventHandler(this.propertyGridContextMenu_Popup);
            // 
            // cmiReset
            // 
            this.cmiReset.Index = 0;
            this.cmiReset.Text = "重置";
            this.cmiReset.Click += new System.EventHandler(this.cmiReset_Click);
            // 
            // cmiSeparator
            // 
            this.cmiSeparator.Index = 1;
            this.cmiSeparator.Text = "-";
            // 
            // cmiDescription
            // 
            this.cmiDescription.Checked = true;
            this.cmiDescription.Index = 2;
            this.cmiDescription.Text = "描述";
            this.cmiDescription.Click += new System.EventHandler(this.cmiDescription_Click);
            // 
            // groupPreview
            // 
            this.groupPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPreview.Controls.Add(this.panelPreview);
            this.groupPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupPreview.Location = new System.Drawing.Point(403, 9);
            this.groupPreview.Name = "groupPreview";
            this.groupPreview.Size = new System.Drawing.Size(480, 562);
            this.groupPreview.TabIndex = 0;
            this.groupPreview.TabStop = false;
            this.groupPreview.Text = "预览";
            // 
            // panelPreview
            // 
            this.panelPreview.Controls.Add(this.labelControlPlaceholder2);
            this.panelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPreview.Location = new System.Drawing.Point(3, 17);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Padding = new System.Windows.Forms.Padding(8);
            this.panelPreview.Size = new System.Drawing.Size(474, 542);
            this.panelPreview.TabIndex = 0;
            // 
            // labelControlPlaceholder2
            // 
            this.labelControlPlaceholder2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelControlPlaceholder2.Enabled = false;
            this.labelControlPlaceholder2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlPlaceholder2.Location = new System.Drawing.Point(8, 8);
            this.labelControlPlaceholder2.Name = "labelControlPlaceholder2";
            this.labelControlPlaceholder2.Size = new System.Drawing.Size(458, 526);
            this.labelControlPlaceholder2.TabIndex = 1;
            this.labelControlPlaceholder2.Text = "A ReportPreviewControl named reportPreview, that displays a preview of the first " +
                "page of the report, will be added here at run-time. The code is in the \"Report P" +
                "review\" code region.";
            this.labelControlPlaceholder2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelControlPlaceholder2.Visible = false;
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butCancel.Location = new System.Drawing.Point(787, 579);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(90, 25);
            this.butCancel.TabIndex = 3;
            this.butCancel.Text = "取消";
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butOk
            // 
            this.butOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.butOk.Location = new System.Drawing.Point(691, 579);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(90, 25);
            this.butOk.TabIndex = 4;
            this.butOk.Text = "确定";
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            // 
            // groupElementProperties
            // 
            this.groupElementProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupElementProperties.Controls.Add(this.panelPropertyGrid);
            this.groupElementProperties.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupElementProperties.Location = new System.Drawing.Point(10, 327);
            this.groupElementProperties.Name = "groupElementProperties";
            this.groupElementProperties.Size = new System.Drawing.Size(384, 244);
            this.groupElementProperties.TabIndex = 5;
            this.groupElementProperties.TabStop = false;
            this.groupElementProperties.Text = "报表元素属性(&P)";
            // 
            // panelPropertyGrid
            // 
            this.panelPropertyGrid.Controls.Add(this.propertyGrid);
            this.panelPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPropertyGrid.Location = new System.Drawing.Point(3, 17);
            this.panelPropertyGrid.Name = "panelPropertyGrid";
            this.panelPropertyGrid.Padding = new System.Windows.Forms.Padding(8);
            this.panelPropertyGrid.Size = new System.Drawing.Size(378, 224);
            this.panelPropertyGrid.TabIndex = 0;
            // 
            // propertyGrid
            // 
            this.propertyGrid.ContextMenu = this.propertyGridContextMenu;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(8, 8);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.propertyGrid.Size = new System.Drawing.Size(362, 208);
            this.propertyGrid.TabIndex = 3;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // groupGeneral
            // 
            this.groupGeneral.Controls.Add(this.txtStyleName);
            this.groupGeneral.Controls.Add(this.labelStyleName);
            this.groupGeneral.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupGeneral.Location = new System.Drawing.Point(10, 9);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(384, 60);
            this.groupGeneral.TabIndex = 6;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "基本";
            // 
            // labelStyleName
            // 
            this.labelStyleName.AutoSize = true;
            this.labelStyleName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelStyleName.Location = new System.Drawing.Point(10, 28);
            this.labelStyleName.Name = "labelStyleName";
            this.labelStyleName.Size = new System.Drawing.Size(77, 12);
            this.labelStyleName.TabIndex = 0;
            this.labelStyleName.Text = "样式名称(&N):";
            // 
            // CustomizeReportStyleForm
            // 
            this.AcceptButton = this.butOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(892, 612);
            this.Controls.Add(this.groupGeneral);
            this.Controls.Add(this.groupElementProperties);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.groupReportElements);
            this.Controls.Add(this.groupPreview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 646);
            this.Name = "CustomizeReportStyleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自定义报表样式";
            this.groupReportElements.ResumeLayout(false);
            this.groupReportElements.PerformLayout();
            this.panelElementList.ResumeLayout(false);
            this.groupPreview.ResumeLayout(false);
            this.panelPreview.ResumeLayout(false);
            this.groupElementProperties.ResumeLayout(false);
            this.panelPropertyGrid.ResumeLayout(false);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
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
            this.InitializeSampleDataGrid();
            this.InitializeStyleName();
            this.InitializeStyleElementList();
            this.InitializePreview();
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
                this.SaveFormBoundsToRegistry();

            if ((e.Cancel) || (this.DialogResult == DialogResult.Cancel))
                return;

            if (txtStyleName.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                  this,
                  "Please provide a name for this report style.",
                  "Required Field",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);

                txtStyleName.Select();
                e.Cancel = true;
                return;
            }

            ArrayList invalidCharList = new ArrayList();
            invalidCharList.AddRange(Path.GetInvalidFileNameChars());
            invalidCharList.Add(Path.DirectorySeparatorChar);
            invalidCharList.Add(Path.AltDirectorySeparatorChar);
            invalidCharList.Add(':');
            invalidCharList.Add('*');
            invalidCharList.Add('?');

            char[] invalidChars = (char[])invalidCharList.ToArray(typeof(char));

            if (txtStyleName.Text.IndexOfAny(invalidChars) != -1)
            {
                System.Text.StringBuilder invalidCharString = new System.Text.StringBuilder();

                foreach (char c in invalidChars)
                {
                    if (!char.IsControl(c))
                    {
                        if (invalidCharString.Length > 0)
                            invalidCharString.Append("   ");

                        invalidCharString.Append(c);
                    }
                }

                System.Windows.Forms.MessageBox.Show(
                  this,
                  "The specified report style name contains invalid characters. You may not include any of the following characters: \n\n" + invalidCharString.ToString(),
                  "Invalid Report Style Name",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Exclamation);

                txtStyleName.Select();
                e.Cancel = true;
                return;
            }

            string newFileName = Path.Combine(Path.GetDirectoryName(m_fileName), txtStyleName.Text + ".xml");

            if ((newFileName != m_fileName) && (File.Exists(newFileName)))
            {
                bool replace = System.Windows.Forms.MessageBox.Show(
                  this,
                  string.Format("A style named '{0}' already exists. Do you want to replace it?", txtStyleName.Text),
                  "Style Already Exists",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question) == DialogResult.Yes;

                if (!replace)
                {
                    txtStyleName.Select();
                    e.Cancel = true;
                    return;
                }
            }

            m_styleSheet.Save(newFileName, true);
        }

        private void SetupTabOrder()
        {
            int tabIndex = 0;

            this.groupGeneral.TabIndex = tabIndex++;
            this.labelStyleName.TabIndex = tabIndex++;
            this.txtStyleName.TabIndex = tabIndex++;
            this.groupReportElements.TabIndex = tabIndex++;
            this.gridStyleElementList.TabIndex = tabIndex++;
            this.panelElementList.TabIndex = tabIndex++;
            this.butAddDataRow.TabIndex = tabIndex++;
            this.butAddGroupLevel.TabIndex = tabIndex++;
            this.groupElementProperties.TabIndex = tabIndex++;
            this.propertyGrid.TabIndex = tabIndex++;
            this.butOk.TabIndex = tabIndex++;
            this.butCancel.TabIndex = tabIndex++;

            this.gridStyleElementList.Select();
        }

        #endregion Form Initialization

        #region Style Name

        private void InitializeStyleName()
        {
            this.txtStyleName.Text = Path.GetFileNameWithoutExtension(m_fileName);
        }

        #endregion Style Name

        #region Style Element List

        #region ElementDataRow Class

        private class ElementDataRow : DataRow
        {
            public ElementDataRow()
                : base()
            {
            }

            protected ElementDataRow(ElementDataRow template)
                : base(template)
            {
            }

            protected override Row CreateInstance()
            {
                return new ElementDataRow(this);
            }

            protected override Cell CreateCell(Column parentColumn)
            {
                Cell cell = base.CreateCell(parentColumn);

                if (parentColumn.Title == "Name")
                    cell.DoubleClick += new EventHandler(cell_DoubleClick);

                return cell;
            }

            protected override bool IsInputKey(Keys keyData)
            {
                if ((keyData == Keys.Space) || (keyData == Keys.Left) || (keyData == Keys.Right))
                    return true;

                return base.IsInputKey(keyData);
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                if (this.DetailGrids.Count == 0)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;

                    switch (e.KeyCode)
                    {
                        case Keys.Space:
                            {
                                this.DetailGrids[0].Collapsed = !this.DetailGrids[0].Collapsed;
                                break;
                            }
                        case Keys.Left:
                            {
                                this.DetailGrids[0].Collapse();
                                break;
                            }
                        case Keys.Right:
                            {
                                this.DetailGrids[0].Expand();
                                break;
                            }
                        default:
                            {
                                e.Handled = false;
                                break;
                            }
                    }
                }

                base.OnKeyDown(e);
            }

            private void cell_DoubleClick(object sender, EventArgs e)
            {
                DataCell cell = (DataCell)sender;

                DataRow row = (DataRow)cell.ParentRow;

                if (row.DetailGrids.Count > 0)
                    row.DetailGrids[0].Collapsed = !row.DetailGrids[0].Collapsed;
            }
        }

        #endregion ElementDataRow Class

        private GridControl gridStyleElementList = new GridControl();

        private void InitializeStyleElementList()
        {
            //
            // Configure general grid settings
            //

            this.gridStyleElementList.FixedColumnSplitter.Visible = false;
            this.gridStyleElementList.Dock = DockStyle.Fill;
            this.gridStyleElementList.BorderStyle = BorderStyle.FixedSingle;
            this.gridStyleElementList.GridLineColor = Color.Transparent;
            this.gridStyleElementList.SelectionMode = SelectionMode.One;
            this.gridStyleElementList.AllowCellNavigation = false;
            this.gridStyleElementList.RowSelectorPane.Visible = false;
            this.gridStyleElementList.ScrollBars = GridScrollBars.Vertical;
            this.gridStyleElementList.BindingContext = new BindingContext();
            this.gridStyleElementList.SynchronizeDetailGrids = true;
            this.gridStyleElementList.ShowTreeLines = false;
            this.gridStyleElementList.DataRowTemplate = new ElementDataRow();
            this.gridStyleElementList.SelectedRowsChanged += new EventHandler(gridStyleElementList_SelectedRowsChanged);
            this.gridStyleElementList.SizeChanged += new EventHandler(gridStyleElementList_SizeChanged);

            //
            // Configure columns
            //

            this.gridStyleElementList.Columns.Add(new Column("Name", typeof(string)));
            this.gridStyleElementList.Columns.Add(new Column("RemoveButton", typeof(Image)));
            this.gridStyleElementList.Columns.Add(new Column("Element", typeof(object)));
            this.gridStyleElementList.Columns.Add(new Column("Group", typeof(string)));

            // Change to Chinese
            this.gridStyleElementList.Columns["Name"].Title = "名字";
            this.gridStyleElementList.Columns["RemoveButton"].Title = "删除";
            this.gridStyleElementList.Columns["Element"].Title = "元素";
            this.gridStyleElementList.Columns["Group"].Title = "组";

            this.gridStyleElementList.Columns["RemoveButton"].Width = 16;
            this.gridStyleElementList.Columns["Element"].Visible = false;
            this.gridStyleElementList.Columns["Group"].Visible = false;

            this.gridStyleElementList.DataRowTemplate.Height = 17;

            //
            // Configure groups
            //

            Group group = new Group("Group");

            group.SideMargin.Width = 0;
            group.HeaderRows.Clear();
            this.gridStyleElementList.GroupTemplates.Add(group);

            //
            // Configure Detail Grids
            //

            DetailGrid detailGrid = new DetailGrid();

            detailGrid.AllowCellNavigation = false;
            detailGrid.Collapsed = true;
            detailGrid.TopMargin.Visible = false;
            detailGrid.BottomMargin.Visible = false;
            detailGrid.CollapsedChanged += new EventHandler(detailGrid_CollapsedChanged);

            detailGrid.Columns.Add(new Column("Name", typeof(string)));
            detailGrid.Columns.Add(new Column("RemoveButton", typeof(Image)));
            detailGrid.Columns.Add(new Column("Element", typeof(object)));

            // Change to Chinese
            detailGrid.Columns["Name"].Title = "名字";
            detailGrid.Columns["RemoveButton"].Title = "删除";
            detailGrid.Columns["Element"].Title = "元素";

            detailGrid.Columns["RemoveButton"].Width = 16;
            detailGrid.Columns["Element"].Visible = false;

            this.gridStyleElementList.DetailGridTemplates.Add(detailGrid);

            //
            // Add elements to the list
            //

            this.AddStyleElementToList("全局", "Global", m_styleSheet.Grid, null);

            DataRow parentRow = this.AddStyleElementToList("页眉", "Global", m_styleSheet.PageHeader, null);
            this.AddSubStyleElementToList(parentRow, "左", m_styleSheet.PageHeader.LeftElement, null);
            this.AddSubStyleElementToList(parentRow, "中", m_styleSheet.PageHeader.CenterElement, null);
            this.AddSubStyleElementToList(parentRow, "右", m_styleSheet.PageHeader.RightElement, null);

            parentRow = this.AddStyleElementToList("页脚", "Global", m_styleSheet.PageFooter, null);
            this.AddSubStyleElementToList(parentRow, "左", m_styleSheet.PageFooter.LeftElement, null);
            this.AddSubStyleElementToList(parentRow, "中", m_styleSheet.PageFooter.CenterElement, null);
            this.AddSubStyleElementToList(parentRow, "右", m_styleSheet.PageFooter.RightElement, null);

            this.AddStyleElementToList("列标题", "Global", m_styleSheet.Grid.ColumnManagerRow, null);
            this.AddStyleElementToList("摘要行", "Global", m_styleSheet.Grid.SummaryRow, null);

            //
            // Add data rows to the list
            //

            if (m_styleSheet.Grid.DataRowCount == 0)
                m_styleSheet.Grid.DataRows.Add(new RowReportStyle());

            this.AddStyleElementToList("数据行", "Data Rows", m_styleSheet.Grid.DataRows[0], null);
            this.UpdateAlternatingDataRows();


            // DetailGrid
            if (m_styleSheet.Grid.DetailGridCount == 0)
                m_styleSheet.Grid.DetailGrids.Add(new GridReportStyle());
            parentRow = this.AddStyleElementToList("子表", "DetailGrid", m_styleSheet.Grid.DetailGrids[0], null);

            this.AddSubStyleElementToList(parentRow, "列标题", m_styleSheet.Grid.DetailGrids[0].ColumnManagerRow, null);
            this.AddSubStyleElementToList(parentRow, "摘要行", m_styleSheet.Grid.DetailGrids[0].SummaryRow, null);
            if (m_styleSheet.Grid.DetailGrids[0].DataRowCount == 0)
                m_styleSheet.Grid.DetailGrids[0].DataRows.Add(new RowReportStyle());
            this.AddSubStyleElementToList(parentRow, "数据行", m_styleSheet.Grid.DetailGrids[0].DataRows[0], null);


            //
            // Add group levels to the list
            //

            if (m_styleSheet.Grid.GroupLevelCount == 0)
                m_styleSheet.Grid.GroupLevels.Add(new GroupReportStyle());

            parentRow = this.AddStyleElementToList("分组", "Groups", m_styleSheet.Grid.GroupLevels[0], null);
            this.AddSubStyleElementToList(parentRow, "头", m_styleSheet.Grid.GroupLevels[0].HeaderRow, null);
            this.AddSubStyleElementToList(parentRow, "尾", m_styleSheet.Grid.GroupLevels[0].FooterRow, null);
            this.AddSubStyleElementToList(parentRow, "摘要行", m_styleSheet.Grid.GroupLevels[0].SummaryRow, null);
            this.UpdateGroupLevels();

            //
            // Add the grid to the form
            //

            this.panelElementList.Controls.Add(this.gridStyleElementList);
        }

        private DataRow AddStyleElementToList(string name, string group, object styleElement, EventHandler removeClickHandler)
        {
            DataRow newRow = this.gridStyleElementList.DataRows.AddNew();

            newRow.ShowPlusMinus = ShowPlusMinus.Never;

            newRow.Cells["Name"].Value = name;
            newRow.Cells["Element"].Value = styleElement;
            newRow.Cells["Group"].Value = group;

            if (removeClickHandler != null)
            {
                Cell imageCell = newRow.Cells["RemoveButton"];

                imageCell.Value = this.imageList.Images[0];
                imageCell.Click += new EventHandler(removeClickHandler);
                imageCell.MouseEnter += new EventHandler(this.HandCursor_MouseEnter);
                imageCell.MouseLeave += new EventHandler(this.HandCursor_MouseLeave);
            }

            newRow.EndEdit();

            return newRow;
        }

        private DataRow AddSubStyleElementToList(DataRow parentRow, string name, object styleElement, EventHandler removeClickHandler)
        {
            DetailGrid detailGrid = parentRow.DetailGrids[0];

            DataRow newRow = detailGrid.DataRows.AddNew();

            newRow.Cells["Name"].Value = name;
            newRow.Cells["Element"].Value = styleElement;

            if (removeClickHandler != null)
            {
                Cell imageCell = newRow.Cells["RemoveButton"];

                imageCell.Value = this.imageList.Images[0];
                imageCell.Click += new EventHandler(removeClickHandler);
                imageCell.MouseEnter += new EventHandler(this.HandCursor_MouseEnter);
                imageCell.MouseLeave += new EventHandler(this.HandCursor_MouseLeave);
            }

            newRow.EndEdit();

            parentRow.ShowPlusMinus = ShowPlusMinus.WhenDetailGridPresent;

            if (m_detailGrid == null)
                m_detailGrid = detailGrid;

            return newRow;
        }

        private void UpdateElementNameColumnWidth()
        {
            int width = this.gridStyleElementList.DisplayRectangle.Width -
              this.gridStyleElementList.Columns["RemoveButton"].Width -
              this.gridStyleElementList.SideMargin.Width;

            this.gridStyleElementList.Columns["Name"].Width = width;

            if (m_detailGrid != null)
            {
                m_detailGrid.Columns["Name"].Width = width - m_detailGrid.SideMargin.Width;
            }
        }

        private void gridStyleElementList_SizeChanged(object sender, EventArgs e)
        {
            this.UpdateElementNameColumnWidth();
        }

        private void gridStyleElementList_SelectedRowsChanged(object sender, EventArgs e)
        {
            if (this.gridStyleElementList.SelectedRows.Count == 0)
            {
                this.propertyGrid.SelectedObject = null;
            }
            else
            {
                Xceed.Grid.DataRow selectedRow = this.gridStyleElementList.SelectedRows[0] as Xceed.Grid.DataRow;
                object element = (selectedRow != null) ? selectedRow.Cells["Element"].Value : null;

                this.propertyGrid.SelectedObject = element;

                this.SetBrowsableAttributes(element == m_styleSheet.Grid);
            }
        }

        private void detailGrid_CollapsedChanged(object sender, EventArgs e)
        {
            this.UpdateElementNameColumnWidth();
        }

        private DataRow AddAlternatingDataRow(int index, RowReportStyle dataRowReportStyle)
        {
            DataRow dataRow = this.AddStyleElementToList(
              "交替数据行" + index.ToString(),
              "Data Rows",
              dataRowReportStyle,
              new EventHandler(this.RemoveAlternatingDataRow_Click));

            m_alternatingDataRows.Add(dataRow);

            return dataRow;
        }

        private void UpdateAlternatingDataRows()
        {
            foreach (DataRow dataRow in m_alternatingDataRows)
            {
                dataRow.Remove();
            }

            m_alternatingDataRows.Clear();

            for (int i = 1; i < m_styleSheet.Grid.DataRowCount; i++)
            {
                this.AddAlternatingDataRow(i, m_styleSheet.Grid.DataRows[i]);
            }

            this.UpdateElementNameColumnWidth();
            this.InvalidatePreview();
        }

        private void butAddDataRow_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RowReportStyle rowStyle = new RowReportStyle();
            int index = m_styleSheet.Grid.DataRows.Add(rowStyle);

            DataRow newRow = this.AddAlternatingDataRow(index, rowStyle);

            this.gridStyleElementList.SelectedRows.Clear();
            this.gridStyleElementList.SelectedRows.Add(newRow);
            this.gridStyleElementList.CurrentRow = newRow;
            newRow.BringIntoView();

            this.UpdateElementNameColumnWidth();
            this.InvalidatePreview();
            this.gridStyleElementList.Select();
        }

        private void RemoveAlternatingDataRow_Click(object sender, EventArgs e)
        {
            Cell cell = sender as Cell;

            if (cell == null)
                return;

            RowReportStyle rowStyle = cell.ParentRow.Cells["Element"].Value as RowReportStyle;

            if (rowStyle == null)
                return;

            m_styleSheet.Grid.DataRows.Remove(rowStyle);

            this.UpdateAlternatingDataRows();
        }

        private void UpdateGroupLevels()
        {
            foreach (DataRow dataRow in m_groupLevels)
            {
                dataRow.Remove();
            }

            m_groupLevels.Clear();

            for (int i = 1; i < m_styleSheet.Grid.GroupLevelCount; i++)
            {
                this.AddGroupLevel(i, m_styleSheet.Grid.GroupLevels[i]);
            }

            this.UpdateElementNameColumnWidth();
            this.InvalidatePreview();
        }

        private DataRow AddGroupLevel(int index, GroupReportStyle groupReportStyle)
        {
            bool groupLevelExists = (index < m_gridSampleData.GroupTemplates.Count);

            DataRow dataRow = this.AddStyleElementToList(
              "子分组 (级别 " + index.ToString() + (groupLevelExists ? ")" : ") (不显示)"),
              "Groups",
              groupReportStyle,
              new EventHandler(this.RemoveGroupLevel_Click));

            this.AddSubStyleElementToList(dataRow, "头", m_styleSheet.Grid.GroupLevels[index].HeaderRow, null);
            this.AddSubStyleElementToList(dataRow, "尾", m_styleSheet.Grid.GroupLevels[index].FooterRow, null);
            this.AddSubStyleElementToList(dataRow, "摘要行", m_styleSheet.Grid.GroupLevels[index].SummaryRow, null);

            m_groupLevels.Add(dataRow);

            return dataRow;
        }

        private void butAddGroupLevel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GroupReportStyle groupStyle = new GroupReportStyle();
            int index = m_styleSheet.Grid.GroupLevels.Add(groupStyle);

            DataRow newRow = this.AddGroupLevel(index, groupStyle);

            this.gridStyleElementList.SelectedRows.Clear();
            this.gridStyleElementList.SelectedRows.Add(newRow);
            this.gridStyleElementList.CurrentRow = newRow;
            newRow.BringIntoView();

            this.UpdateElementNameColumnWidth();
            this.InvalidatePreview();
            this.gridStyleElementList.Select();
        }

        private void RemoveGroupLevel_Click(object sender, EventArgs e)
        {
            Cell cell = sender as Cell;

            if (cell == null)
                return;

            GroupReportStyle groupStyle = cell.ParentRow.Cells["Element"].Value as GroupReportStyle;

            if (groupStyle == null)
                return;

            m_styleSheet.Grid.GroupLevels.Remove(groupStyle);

            this.UpdateGroupLevels();
        }

        private void HandCursor_MouseEnter(object sender, EventArgs e)
        {
            this.gridStyleElementList.Cursor = Cursors.Hand;
        }

        private void HandCursor_MouseLeave(object sender, EventArgs e)
        {
            this.gridStyleElementList.Cursor = Cursors.Default;
        }

        private void Report_QueryVariableText(object sender, QueryVariableTextEventArgs e)
        {
            // The only variables that can displayed with a meaningful value in the context of 
            // a ReportStyleSheet are PAGE, TOTALPAGES and DATETIME. Since, at this point, we
            // have no idea what value the other variables will have (including TITLE),
            // we simply replace them with their name.
            if ((string.Compare(e.VariableName, "PAGE", true) != 0)
              && (string.Compare(e.VariableName, "TOTALPAGES", true) != 0)
              && (string.Compare(e.VariableName, "DATETIME", true) != 0))
                e.VariableText = e.VariableName;
        }

        private ArrayList m_alternatingDataRows = new ArrayList();
        private ArrayList m_groupLevels = new ArrayList();
        private DetailGrid m_detailGrid;

        #endregion Style Element List

        #region Property Grid

        private AttributeCollection m_originalBrowsableAttributes;

        private void SetBrowsableAttributes(bool isGlobal)
        {
            if (m_originalBrowsableAttributes == null)
                m_originalBrowsableAttributes = this.propertyGrid.BrowsableAttributes;

            int nbAttributes = isGlobal ? 2 : 1;
            Attribute[] attributes = new Attribute[m_originalBrowsableAttributes.Count + nbAttributes];

            attributes[0] = new Xceed.Grid.Reporting.Design.EndUserBrowsableAttribute(true);

            if (isGlobal)
                attributes[1] = new Xceed.Grid.Reporting.Design.EndUserGlobalBrowsableAttribute(true);

            for (int i = 0; i < m_originalBrowsableAttributes.Count; i++)
            {
                attributes[i + nbAttributes] = m_originalBrowsableAttributes[i];
            }

            this.propertyGrid.BrowsableAttributes = new AttributeCollection(attributes);
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.InvalidatePreview();
        }

        private void propertyGridContextMenu_Popup(object sender, System.EventArgs e)
        {
            if (this.propertyGrid.SelectedObjects.Length == 1)
            {
                cmiReset.Enabled =
                  this.propertyGrid.SelectedGridItem.PropertyDescriptor.CanResetValue(this.propertyGrid.SelectedGridItem.Parent.Value);
            }
            else
            {
                // We don't enable the Reset command when multiple objects are being edited by the
                // property grid, because CanResetValue() throws an InvalidCastException.
                cmiReset.Enabled = false;
            }

            cmiDescription.Checked = this.propertyGrid.HelpVisible;
        }

        private void cmiReset_Click(object sender, EventArgs e)
        {
            this.propertyGrid.SelectedGridItem.PropertyDescriptor.ResetValue(this.propertyGrid.SelectedGridItem.Parent.Value);
            this.InvalidatePreview();
            this.propertyGrid.Refresh();
        }

        private void cmiDescription_Click(object sender, EventArgs e)
        {
            cmiDescription.Checked = !cmiDescription.Checked;
            this.propertyGrid.HelpVisible = cmiDescription.Checked;
        }

        #endregion Property Grid

        #region Report Preview

        private ReportPreviewControl reportPreview = new ReportPreviewControl();

        private void InitializePreview()
        {
            this.reportPreview.Dock = DockStyle.Fill;
            this.panelPreview.Controls.Add(this.reportPreview);
        }

        private void InvalidatePreview()
        {
            if (!m_initialized)
                return;

            if (m_report == null)
            {
                m_report = new Report(m_gridSampleData, m_styleSheet);
                m_report.QueryVariableText += new QueryVariableTextEventHandler(Report_QueryVariableText);
            }

            PrintDocument document = m_report.CreatePrintDocument();

            this.reportPreview.PrintDocument = document;
        }

        #endregion Report Preview

        #region Button Click Handlers

        private void butOk_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void butCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        #endregion Button Click Handlers

        #region Utility Methods

        private void LoadFormBoundsFromRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(CustomizeReportStyleForm.RegistryAddress))
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
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(CustomizeReportStyleForm.RegistryAddress))
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

        #endregion Utility Methods

        #region Sample Data

        private void InitializeSampleDataGrid()
        {
            EmployeeSaleInfo[] sampleData = new EmployeeSaleInfo[]
        {
          new EmployeeSaleInfo( "Steven", "Maria Anders", "Appliances", 431.18f, "Suspendisse et tortor sit amet lectus porttitor elementum. Aenean mi diam, vestibulum viverra, fringilla laoreet.", true ),
          new EmployeeSaleInfo( "Steven", "Ana Trujillo", "Appliances", 218.49f, "Aenean rutrum rutrum magna. Pellentesque enim diam, facilisis ut, gravida sit amet, laoreet ac, libero.", true ),
          new EmployeeSaleInfo( "Steven", "Antonio Moreno", "Kitchenware", 139.87f, "Donec sagittis dolor nec mauris. In eget urna sed quam lacinia nonummy. Integer leo nunc.", false ),
          new EmployeeSaleInfo( "Steven", "Thomas Hardy", "Appliances", 87.29f, "Etiam vestibulum. Nunc sit amet tortor. Vivamus odio nisl, commodo placerat, mollis sit amet, luctus.", true ),
          new EmployeeSaleInfo( "Anne", "Christina Berglund", "Furniture", 296.46f, "Praesent lacinia faucibus nulla. Quisque pellentesque pellentesque nunc. Donec tempus facilisis eros. Morbi non quam.", false ),
          new EmployeeSaleInfo( "Anne", "Hanna Moos", "Appliances", 187.73f, "Phasellus blandit metus ut diam condimentum tristique. Morbi eu leo ut dui rutrum pulvinar. Sed.", false ),
          new EmployeeSaleInfo( "Anne", "Fr閐閞ique Citeaux", "Furniture", 987.65f, "Vestibulum sodales turpis in augue. Integer non lectus nec elit sagittis porttitor. Praesent id justo.", false ),
          new EmployeeSaleInfo( "Janet", "Mart韓 Sommer", "Men's Clothing", 684.26f, "Nam consequat, pede pretium tempus posuere, diam turpis rutrum nisl, vitae dapibus sapien neque iaculis.", false ),
          new EmployeeSaleInfo( "Janet", "Laurence Lebihan", "Men's Clothing", 48.00f, "Suspendisse potenti. Mauris sollicitudin enim sit amet purus. Ut varius adipiscing lorem. Integer adipiscing. Curabitur.", true ),
          new EmployeeSaleInfo( "Bob", "Elizabeth Lincoln", "Electronics", 659.29f, "Nulla congue facilisis odio. Etiam dapibus tempus pede. Vestibulum sed quam. Maecenas dictum enim eu.", true ),
          new EmployeeSaleInfo( "Bob", "Victoria Ashworth", "Electronics", 196.36f, "Suspendisse potenti. Duis iaculis tincidunt ante. Sed quam. Fusce commodo, augue id egestas pharetra, elit.", false ),
          new EmployeeSaleInfo( "Bill", "Patricio Simpson", "Home Hardware", 396.14f, "Curabitur velit lacus, cursus ac, sollicitudin vel, porttitor ut, ipsum. Donec posuere purus ut mauris.", true ),
          new EmployeeSaleInfo( "Bill", "Francisco Chang", "Car Accessories", 571.38f, "Maecenas ut elit. Nam lacus. Sed blandit fringilla lorem. Duis cursus lectus vel velit. Pellentesque.", false ),
      };

            m_gridSampleData = new GridControl();

            m_gridSampleData.FixedColumnSplitter.Visible = false;
            m_gridSampleData.Columns.GenerateFromType(typeof(EmployeeSaleInfo));

            m_gridSampleData.FixedHeaderRows.Add(new ColumnManagerRow());

            Group divisionGroup = new Group("Division");
            m_gridSampleData.GroupTemplates.Add(divisionGroup);

            SummaryRow divisionSummary = new SummaryRow();
            divisionGroup.FooterRows.Add(divisionSummary);

            SummaryCell statCell = (SummaryCell)divisionSummary.Cells["SaleAmount"];
            statCell.StatFunction = StatFunction.Sum;
            statCell.TitleFormat = "Total for %GROUPKEY%:";

            Group employeeGroup = new Group("SalesAgent");
            m_gridSampleData.GroupTemplates.Add(employeeGroup);

            SummaryRow employeeSummary = new SummaryRow();
            employeeGroup.FooterRows.Add(employeeSummary);

            statCell = (SummaryCell)employeeSummary.Cells["SaleAmount"];
            statCell.StatFunction = StatFunction.Sum;
            statCell.TitleFormat = "Total for %GROUPKEY%:";

            m_gridSampleData.Columns["Division"].Visible = false;
            m_gridSampleData.Columns["SalesAgent"].Visible = false;

            int visibleIndex = 0;

            m_gridSampleData.Columns["CustomerName"].VisibleIndex = visibleIndex++;
            m_gridSampleData.Columns["SaleAmount"].VisibleIndex = visibleIndex++;
            m_gridSampleData.Columns["Shipped"].VisibleIndex = visibleIndex++;
            m_gridSampleData.Columns["Comment"].VisibleIndex = visibleIndex++;

            m_gridSampleData.Columns["CustomerName"].Title = "Customer Name";
            m_gridSampleData.Columns["SaleAmount"].Title = "Sale Amount";

            m_gridSampleData.Columns["SaleAmount"].FormatSpecifier = "c";

            m_gridSampleData.Columns["CustomerName"].Width = 175;
            m_gridSampleData.Columns["Shipped"].Width = m_gridSampleData.Columns["Shipped"].GetFittedWidth();
            m_gridSampleData.Columns["Comment"].Width = 200;

            m_gridSampleData.BindingContext = new BindingContext();
            m_gridSampleData.DataSource = sampleData;
        }

        private class EmployeeSaleInfo
        {
            public EmployeeSaleInfo(
              string salesAgent,
              string customerName,
              string division,
              float saleAmount,
              string comment,
              bool shipped)
            {
                m_salesAgent = salesAgent;
                m_customerName = customerName;
                m_division = division;
                m_saleAmount = saleAmount;
                m_comment = comment;
                m_shipped = shipped;
            }

            private string m_salesAgent;
            public string SalesAgent
            {
                get { return m_salesAgent; }
            }

            private string m_customerName;
            public string CustomerName
            {
                get { return m_customerName; }
            }

            private string m_division;
            public string Division
            {
                get { return m_division; }
            }

            private float m_saleAmount;
            public float SaleAmount
            {
                get { return m_saleAmount; }
            }

            private string m_comment;
            public string Comment
            {
                get { return m_comment; }
            }

            private bool m_shipped;
            public bool Shipped
            {
                get { return m_shipped; }
            }
        }

        #endregion Sample Data

        #region Private Fields

        private bool m_initialized; // = false
        private ReportStyleSheet m_styleSheet;
        private string m_fileName;
        private Report m_report;
        private GridControl m_gridSampleData;

        private static readonly string RegistryAddress = ""; //@"Software\Xceed Software\Xceed Grid for .NET\" + _XceedVersionInfo.BaseVersion + @"\Report Configuration Forms\CustomizeReportStyleForm";

        #endregion Private Fields
    }
}
