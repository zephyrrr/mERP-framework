using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    [CLSCompliant(false)]
    public partial class ArchiveDataSetReportForm : MyChildForm
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                if (this.myCrystalReportViewer != null)
                {
                    this.myCrystalReportViewer.Dispose();
                    this.myCrystalReportViewer = null;
                }
                base.RevertMergeToolStrip(this.pageNavigator1);
                base.RevertMergeToolStrip(this.toolStrip1);
            }
            base.Dispose(disposing);
        }

        public ArchiveDataSetReportForm()
        {
            InitializeComponent();
            this.bindingNavigatorMoveLastItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconLast.png").Reference;
            this.bindingNavigatorMoveNextItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNext.png").Reference;
            this.bindingNavigatorMovePreviousItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPrevious.png").Reference;
            this.bindingNavigatorMoveFirstItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFirst.png").Reference;

            base.MergeToolStrip(this.toolStrip1);
            base.MergeToolStrip(this.pageNavigator1);
        }

        protected override void Form_Load(object sender, System.EventArgs e)
        {
            this.OnSearchManagerChanged();
        }

        private ISearchManager m_sm;
        /// <summary>
        /// 
        /// </summary>
        public ISearchManager SearchManager
        {
            get { return m_sm; }
            set { m_sm = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnSearchManagerChanged()
        {
            if (this.SearchManager == null)
            {
                pageNavigator1.Enabled = false;
                //tsbSearchConditions.Enabled = false;
            }
            else
            {
                pageNavigator1.BindingSource = new PageBindingSource(this.SearchManager);
                pageNavigator1.Enabled = this.SearchManager.EnablePage;

                //tsbSearchConditions.LoadMenus(this.DisplayManager.SearchManager, this.Text);
            }

            IWinFormApplication mdiForm = ServiceProvider.GetService<IApplication>() as IWinFormApplication; 
            if (mdiForm != null)
            {
                mdiForm.OnChildFormShow(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MyCrystalReportViewer ReportViewer
        {
            get { return this.myCrystalReportViewer; }
        }
    }
}
