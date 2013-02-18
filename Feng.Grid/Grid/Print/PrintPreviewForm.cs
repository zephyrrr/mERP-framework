/*
* Xceed Grid for .NET - StandardPrinting Sample Application
* Copyright (c) 2006 - Xceed Software Inc.
*
* [PrintPreviewForm.cs]
*
* This application demonstrates the basic steps necessary to print a grid programmatically.
* It uses a print preview dialog as well as printer and page setup settings forms. 
* 
* This file is part of Xceed Grid for .NET. The source code in this file 
* is only intended as a supplement to the documentation, and is provided 
* "as is", without warranty of any kind, either expressed or implied.
*/

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Xceed.Grid;

namespace Feng.Grid.Print
{
    // This form ca be used to demonstrate the zoomable print preview functionality 
    // of a GridControl using a GridPrintDocument.
    // It also offers page setup and header/footer edition.
    /// <summary>
    /// 
    /// </summary>
    public class PrintPreviewForm : System.Windows.Forms.Form
    {
        private int m_numberOfPagesRendered = 1;

        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.ToolBarButton tbtPrint;
        private System.Windows.Forms.ToolBarButton tbtPageSetup;
        private System.Windows.Forms.ImageList toolbarImages;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.ToolBarButton tbtTwo;
        private System.Windows.Forms.ToolBarButton tbtThree;
        private System.Windows.Forms.ToolBarButton tbtFour;
        private System.Windows.Forms.ToolBarButton tbtSix;
        private System.Windows.Forms.ToolBarButton tlbtZoom;
        private System.Windows.Forms.ToolBarButton tbtOne;
        private System.Windows.Forms.ToolBarButton separator2;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuAuto;
        private System.Windows.Forms.MenuItem menu10;
        private System.Windows.Forms.MenuItem menu25;
        private System.Windows.Forms.MenuItem menu50;
        private System.Windows.Forms.MenuItem menu75;
        private System.Windows.Forms.MenuItem menu100;
        private System.Windows.Forms.MenuItem menu150;
        private System.Windows.Forms.MenuItem menu200;
        private System.Windows.Forms.MenuItem menu250;
        private System.Windows.Forms.MenuItem menu500;
        private Xceed.Editors.WinButton winButton1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPrintDocument"></param>
        public PrintPreviewForm(MyGridPrintDocument gridPrintDocument)
        {
            m_gridPrintDocument = gridPrintDocument;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            printPreviewControl1.Document = gridPrintDocument;
            m_headerFooterEditor = new HeaderFooterEditor();
            this.RegisterHeaderFooter();
        }

        // Form overrides dispose to clean up the component list.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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

        // Required method for Designer support - do not modify
        // the contents of this method with the code editor.
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPreviewForm));
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.tbtPrint = new System.Windows.Forms.ToolBarButton();
            this.tlbtZoom = new System.Windows.Forms.ToolBarButton();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuAuto = new System.Windows.Forms.MenuItem();
            this.menu10 = new System.Windows.Forms.MenuItem();
            this.menu25 = new System.Windows.Forms.MenuItem();
            this.menu50 = new System.Windows.Forms.MenuItem();
            this.menu75 = new System.Windows.Forms.MenuItem();
            this.menu100 = new System.Windows.Forms.MenuItem();
            this.menu150 = new System.Windows.Forms.MenuItem();
            this.menu200 = new System.Windows.Forms.MenuItem();
            this.menu250 = new System.Windows.Forms.MenuItem();
            this.menu500 = new System.Windows.Forms.MenuItem();
            this.separator2 = new System.Windows.Forms.ToolBarButton();
            this.tbtOne = new System.Windows.Forms.ToolBarButton();
            this.tbtTwo = new System.Windows.Forms.ToolBarButton();
            this.tbtThree = new System.Windows.Forms.ToolBarButton();
            this.tbtFour = new System.Windows.Forms.ToolBarButton();
            this.tbtSix = new System.Windows.Forms.ToolBarButton();
            this.tbtPageSetup = new System.Windows.Forms.ToolBarButton();
            this.toolbarImages = new System.Windows.Forms.ImageList(this.components);
            this.printPreviewDialog2 = new System.Windows.Forms.PrintPreviewDialog();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.winButton1 = new Xceed.Editors.WinButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.AutoZoom = false;
            this.printPreviewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPreviewControl1.Location = new System.Drawing.Point(0, 34);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(592, 332);
            this.printPreviewControl1.TabIndex = 0;
            this.printPreviewControl1.Zoom = 0.30000001192092896;
            this.printPreviewControl1.SizeChanged += new System.EventHandler(this.printPreviewControl1_SizeChanged);
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.ShowHelp = true;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.AutoSize = false;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbtPrint,
            this.tlbtZoom,
            this.separator2,
            this.tbtOne,
            this.tbtTwo,
            this.tbtThree,
            this.tbtFour,
            this.tbtSix,
            this.tbtPageSetup});
            this.toolBar1.ButtonSize = new System.Drawing.Size(20, 20);
            this.toolBar1.Divider = false;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.toolbarImages;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(592, 34);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // tbtPrint
            // 
            this.tbtPrint.ImageIndex = 0;
            this.tbtPrint.Name = "tbtPrint";
            this.tbtPrint.Tag = "Print";
            this.tbtPrint.ToolTipText = "¥Ú”°";
            // 
            // tlbtZoom
            // 
            this.tlbtZoom.DropDownMenu = this.contextMenu1;
            this.tlbtZoom.ImageIndex = 1;
            this.tlbtZoom.Name = "tlbtZoom";
            this.tlbtZoom.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAuto,
            this.menu10,
            this.menu25,
            this.menu50,
            this.menu75,
            this.menu100,
            this.menu150,
            this.menu200,
            this.menu250,
            this.menu500});
            // 
            // menuAuto
            // 
            this.menuAuto.Checked = true;
            this.menuAuto.DefaultItem = true;
            this.menuAuto.Index = 0;
            this.menuAuto.Text = "◊‘∂Ø";
            this.menuAuto.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu10
            // 
            this.menu10.Index = 1;
            this.menu10.Text = "10%";
            this.menu10.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu25
            // 
            this.menu25.Index = 2;
            this.menu25.Text = "25%";
            this.menu25.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu50
            // 
            this.menu50.Index = 3;
            this.menu50.Text = "50%";
            this.menu50.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu75
            // 
            this.menu75.Index = 4;
            this.menu75.Text = "75%";
            this.menu75.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu100
            // 
            this.menu100.Index = 5;
            this.menu100.Text = "100%";
            this.menu100.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu150
            // 
            this.menu150.Index = 6;
            this.menu150.Text = "150%";
            this.menu150.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu200
            // 
            this.menu200.Index = 7;
            this.menu200.Text = "200%";
            this.menu200.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu250
            // 
            this.menu250.Index = 8;
            this.menu250.Text = "250%";
            this.menu250.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menu500
            // 
            this.menu500.Index = 9;
            this.menu500.Text = "500%";
            this.menu500.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbtOne
            // 
            this.tbtOne.ImageIndex = 5;
            this.tbtOne.Name = "tbtOne";
            this.tbtOne.Pushed = true;
            // 
            // tbtTwo
            // 
            this.tbtTwo.ImageIndex = 6;
            this.tbtTwo.Name = "tbtTwo";
            // 
            // tbtThree
            // 
            this.tbtThree.ImageIndex = 7;
            this.tbtThree.Name = "tbtThree";
            // 
            // tbtFour
            // 
            this.tbtFour.ImageIndex = 8;
            this.tbtFour.Name = "tbtFour";
            // 
            // tbtSix
            // 
            this.tbtSix.ImageIndex = 9;
            this.tbtSix.Name = "tbtSix";
            // 
            // tbtPageSetup
            // 
            this.tbtPageSetup.ImageIndex = 10;
            this.tbtPageSetup.Name = "tbtPageSetup";
            this.tbtPageSetup.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbtPageSetup.Tag = "PageSetup";
            this.tbtPageSetup.ToolTipText = "“≥√Ê…Ë÷√";
            // 
            // toolbarImages
            // 
            this.toolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbarImages.ImageStream")));
            this.toolbarImages.TransparentColor = System.Drawing.Color.Transparent;
            this.toolbarImages.Images.SetKeyName(0, "");
            this.toolbarImages.Images.SetKeyName(1, "");
            this.toolbarImages.Images.SetKeyName(2, "");
            this.toolbarImages.Images.SetKeyName(3, "");
            this.toolbarImages.Images.SetKeyName(4, "");
            this.toolbarImages.Images.SetKeyName(5, "");
            this.toolbarImages.Images.SetKeyName(6, "");
            this.toolbarImages.Images.SetKeyName(7, "");
            this.toolbarImages.Images.SetKeyName(8, "");
            this.toolbarImages.Images.SetKeyName(9, "");
            this.toolbarImages.Images.SetKeyName(10, "");
            // 
            // printPreviewDialog2
            // 
            this.printPreviewDialog2.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog2.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog2.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog2.Enabled = true;
            this.printPreviewDialog2.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog2.Icon")));
            this.printPreviewDialog2.Name = "printPreviewDialog2";
            this.printPreviewDialog2.Visible = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(336, 4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(58, 21);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(288, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "“≥¬Î";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // printDialog1
            // 
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.ShowHelp = true;
            // 
            // winButton1
            // 
            this.winButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.winButton1.Location = new System.Drawing.Point(420, 3);
            this.winButton1.Name = "winButton1";
            this.winButton1.Size = new System.Drawing.Size(153, 26);
            this.winButton1.TabIndex = 7;
            this.winButton1.Text = "“≥√º“≥Ω≈…Ë÷√";
            this.winButton1.Click += new System.EventHandler(this.button2_Click);
            // 
            // PrintPreviewForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(592, 366);
            this.Controls.Add(this.winButton1);
            this.Controls.Add(this.printPreviewControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.toolBar1);
            this.Name = "PrintPreviewForm";
            this.Text = "¥Ú”°‘§¿¿";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #region PAGE SETUP MANAGEMENT

        private void PageSetup()
        {
            pageSetupDialog1.PageSettings = m_gridPrintDocument.PageSettings;

            Debug.WriteLine(pageSetupDialog1.PageSettings.Margins.ToString());

            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                m_gridPrintDocument.PageSettings = this.pageSetupDialog1.PageSettings;

            Debug.WriteLine(pageSetupDialog1.PageSettings.Margins.ToString());

            this.printPreviewControl1.InvalidatePreview();
        }

        #endregion PAGE SETUP MANAGEMENT

        #region ZOOM MANAGEMENT

        private void SetPages(int numberOfPages)
        {
            m_numberOfPagesRendered = numberOfPages;

            if (numberOfPages % 2 == 0)
            {
                double sqrt = Math.Sqrt(numberOfPages);
                printPreviewControl1.Columns = (int)Math.Ceiling(sqrt);
                printPreviewControl1.Rows = (int)Math.Floor(sqrt);
            }
            else
            {
                printPreviewControl1.Columns = numberOfPages;
                printPreviewControl1.Rows = 1;
            }

            printPreviewControl1.Zoom = Math.Min(
              ((double)this.printPreviewControl1.Width / ((double)m_gridPrintDocument.PageSettings.Bounds.Width * printPreviewControl1.Columns)),
              (double)this.printPreviewControl1.Height / ((double)m_gridPrintDocument.PageSettings.Bounds.Height * printPreviewControl1.Rows));

            this.contextMenu1.MenuItems[0].Checked = true;
        }

        #endregion ZOOM MANAGEMENT

        private void Print()
        {
            try
            {
                printDialog1.Document = m_gridPrintDocument;
                printDialog1.PrinterSettings = m_gridPrintDocument.PrinterSettings;
                if (printDialog1.ShowDialog() == DialogResult.OK)
                    this.printPreviewControl1.Document.Print();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // Catches cancelled operations
            }
        }

        #region UI HANDLERS

        private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            switch ((string)e.Button.Tag)
            {
                case "Print":
                    // The following line prints the grid's content using the default printer dialog.
                    this.Print();
                    break;
                case "Cancel":
                    this.Close();
                    break;
                case "PageSetup":
                    this.PageSetup();
                    break;
                default:
                    {
                        foreach (ToolBarButton button in this.toolBar1.Buttons)
                        {
                            button.Pushed = false;
                        }

                        if (e.Button == this.tbtOne)
                        {
                            this.SetPages(1);
                            this.tbtOne.Pushed = true;
                        }
                        else if (e.Button == this.tbtTwo)
                        {
                            this.SetPages(2);
                            this.tbtTwo.Pushed = true;
                        }
                        else if (e.Button == this.tbtThree)
                        {
                            this.SetPages(3);
                            this.tbtThree.Pushed = true;
                        }
                        else if (e.Button == this.tbtFour)
                        {
                            this.SetPages(4);
                            this.tbtFour.Pushed = true;
                        }
                        else if (e.Button == this.tbtSix)
                        {
                            this.SetPages(6);
                            this.tbtSix.Pushed = true;
                        }
                        break;
                    }
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            printPreviewControl1.StartPage = (int)numericUpDown1.Value - 1;
        }

        #endregion

        #region HEADER FOOTER EDITION

        private void DeregisterHeaderFooter()
        {
            m_gridPrintDocument.PrintableElements.Clear();
        }

        private void AddTextBox(PrintableTextBox printableTextBox)
        {
            if (printableTextBox != null)
                m_gridPrintDocument.PrintableElements.Add(printableTextBox.GetHashCode(), printableTextBox);
        }

        private void RegisterHeaderFooter()
        {
            this.DeregisterHeaderFooter();
            this.AddTextBox(this.m_headerFooterEditor.Header);
            this.AddTextBox(this.m_headerFooterEditor.Footer);
            this.printPreviewControl1.InvalidatePreview();
        }

        private void HeaderFooterDialog()
        {
            if (this.m_headerFooterEditor.ShowDialog() != DialogResult.Cancel)
                this.RegisterHeaderFooter();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.HeaderFooterDialog();
        }

        #endregion

        #region FIELDS

        private HeaderFooterEditor m_headerFooterEditor;
        private MyGridPrintDocument m_gridPrintDocument;

        #endregion

        #region CONSTANTS

        private const double ZOOM_STEP = 0.2;
        private const double MINIMUM_ZOOM_FACTOR = 0.02;

        #endregion

        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            foreach (MenuItem menu in this.contextMenu1.MenuItems)
            {
                menu.Checked = false;
            }

            if (item == this.menuAuto)
            {
                this.menuAuto.Checked = true;
                this.SetPages(m_numberOfPagesRendered);
            }
            if (item == this.menu10)
            {
                printPreviewControl1.Zoom = .10;
                this.menu10.Checked = true;
            }
            else if (item == this.menu25)
            {
                printPreviewControl1.Zoom = .25;
                this.menu25.Checked = true;
            }
            else if (item == this.menu50)
            {
                printPreviewControl1.Zoom = .50;
                this.menu50.Checked = true;
            }
            else if (item == this.menu75)
            {
                printPreviewControl1.Zoom = 0.75;
                this.menu75.Checked = true;
            }
            else if (item == this.menu100)
            {
                printPreviewControl1.Zoom = 1.00;
                this.menu100.Checked = true;
            }
            else if (item == this.menu150)
            {
                printPreviewControl1.Zoom = 1.50;
                this.menu150.Checked = true;
            }
            else if (item == this.menu200)
            {
                printPreviewControl1.Zoom = 2.00;
                this.menu200.Checked = true;
            }
            else if (item == this.menu250)
            {
                printPreviewControl1.Zoom = 2.50;
                this.menu250.Checked = true;
            }
            else if (item == this.menu500)
            {
                printPreviewControl1.Zoom = 5.00;
                this.menu500.Checked = true;
            }
        }

        private void printPreviewControl1_SizeChanged(object sender, System.EventArgs e)
        {
            if (this.menuAuto.Checked)
                this.SetPages(m_numberOfPagesRendered);
        }
    }
}
