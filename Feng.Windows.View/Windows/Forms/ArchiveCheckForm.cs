using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 选择列表窗体
    /// </summary>
    public partial class ArchiveCheckForm : ArchiveGridForm
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
                if (this.m_masterGrid != null)
                {
                    this.m_masterGrid.Dispose();
                    this.m_masterGrid = null;
                }

                base.RevertMergeMenu(this.menuStrip1);
                base.RevertMergeToolStrip(this.pageNavigator1);
                base.RevertMergeToolStrip(this.toolStrip1);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveCheckForm()
            : this(false)
        {
        }

        private IBoundGrid m_masterGrid;
        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveCheckForm(bool isArchive)
        {
            InitializeComponent();
            this.bindingNavigatorMoveLastItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconLast.png").Reference;
            this.bindingNavigatorMoveNextItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconNext.png").Reference;
            this.bindingNavigatorMovePreviousItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconPrevious.png").Reference;
            this.bindingNavigatorMoveFirstItem.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFirst.png").Reference;

            if (isArchive)
            {
                // wrong. There is no ControlManager
                //m_grid = new ArchiveUnboundWithDetailGridLoadOnDemand();
            }
            else
            {
                m_masterGrid = new DataUnboundWithDetailGridLoadOnDemand();
            }

            m_masterGrid.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());
            this.splitContainer1.Panel1.Controls.Add(this.m_masterGrid as Control);
            (this.m_masterGrid as Control).Dock = DockStyle.Fill;
            (this.m_masterGrid as Control).BringToFront();
            (this.m_masterGrid as MyGrid).SingleClickEdit = true;

            this.tsbRefresh.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRefresh.png").Reference;
            this.tsbSearch.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSearch.png").Reference;
            this.tsbFind.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFind.png").Reference;
            this.tsbFilter.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFilter.png").Reference;
            this.tsbSetup.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSetup.png").Reference;
            this.tsbOk.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconOk.png").Reference;
            this.tsmRefresh.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconRefresh.png").Reference;
            this.tsmSearch.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSearch.png").Reference;
            this.tsmFind.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFind.png").Reference;
            this.tsmFilter.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconFilter.png").Reference;

            base.MergeMenu(this.menuStrip1);
            base.MergeToolStrip(this.toolStrip1);
            base.MergeToolStrip(this.pageNavigator1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            base.Form_Load(sender, e);

            // 设置Readonly时，只设置True。如果为False的，不设置Readonly，然后开启外层Readonly
            foreach (Xceed.Grid.Column column in m_masterGrid.Columns)
            {
                column.ReadOnly = true;
            }
            m_checkColumns.Add(m_masterGrid.AddCheckColumn()); //.ReadOnly = false;

            foreach (MyDetailGrid detailGrid in m_masterGrid.DetailGridTemplates)
            {
                AddCheckColumnToDetailGrid(detailGrid);
            }
            m_masterGrid.ReadOnly = false;

            tsbSearch_Click(tsbSearch, System.EventArgs.Empty);
            tsbSearch.Visible = false;

            if (!this.splitContainer1.LoadLayout())
            {
                this.splitContainer1.SplitterDistance = 630;
            }

            m_masterGrid.DisplayManager.SearchManager.DataLoaded += new EventHandler<DataLoadedEventArgs>(SearchManager_DataLoaded);

            if (this.DisplayManager == null || this.DisplayManager.SearchManager == null)
            {
                pageNavigator1.Enabled = false;
                //tsbSearchConditions.Enabled = false;
            }
            else
            {
                pageNavigator1.BindingSource = new PageBindingSource(this.DisplayManager.SearchManager);
                pageNavigator1.Enabled = this.DisplayManager.SearchManager.EnablePage;

                //tsbSearchConditions.LoadMenus(this.DisplayManager.SearchManager, this.Text);
            }
        }

        private IList<Feng.Grid.Columns.CheckColumn> m_checkColumns = new List<Feng.Grid.Columns.CheckColumn>();
        void SearchManager_DataLoaded(object sender, DataLoadedEventArgs e)
        {
            try
            {
                m_masterGrid.BeginInit();

                ResetCheckColumnValue(m_masterGrid);

                foreach (Feng.Grid.Columns.CheckColumn column in m_checkColumns)
                {
                    column.ResetSumRow();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
            }
            finally
            {
                m_masterGrid.EndInit();
            }
        }

        private void ResetCheckColumnValue(IGrid grid)
        {
            foreach (Xceed.Grid.DataRow row in grid.DataRows)
            {
                row.Cells[Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName].Value = false;

                foreach (IGrid detailGrid in row.DetailGrids)
                {
                    ResetCheckColumnValue(detailGrid);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            this.splitContainer1.SaveLayout();

            base.Form_Closing(sender, e);
        }

        private void AddCheckColumnToDetailGrid(MyDetailGrid detailGrid)
        {
            foreach (Xceed.Grid.Column column in detailGrid.Columns)
            {
                column.ReadOnly = true;
            }
            m_checkColumns.Add(detailGrid.AddCheckColumn());
            detailGrid.ReadOnly = false;

            foreach (MyDetailGrid subDetailGrid in detailGrid.DetailGridTemplates)
            {
                AddCheckColumnToDetailGrid(subDetailGrid);
            }
        }

        /// <summary>
        /// DisplayManager
        /// </summary>
        public IDisplayManager DisplayManager
        {
            get { return m_masterGrid.DisplayManager; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override IGrid MasterGrid
        {
            get
            {
                return m_masterGrid;
            }
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            //ArchiveSeeForm.ShowSearchDialog(m_grid.DisplayManager.SearchManager, this.Name);

            tsbSearch.Checked = !tsbSearch.Checked;
            if (this.splitContainer1.Panel2.Controls.Count == 0)
            {
                var s = this.GetSearchPanel();
                if (s != null)
                {
                    s.Dock = DockStyle.Fill;
                    this.splitContainer1.Panel2.Controls.Add(s);
                }
            }
            else
            {
                this.splitContainer1.Panel2.Visible = tsbSearch.Checked;
            }
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            if (this.MasterGrid != null)
            {
                Feng.Windows.Forms.FindForm.Instance.Show(this);
                Feng.Windows.Forms.FindForm.Instance.ToFindGrid = this.MasterGrid.GridControl;
            }
        }

        private void tsbFilter_Click(object sender, EventArgs e)
        {
            if (this.MasterGrid != null)
            {
                tsbFilter.Checked = !tsbFilter.Checked;
                this.MasterGrid.SetFilterRowVisible(tsbFilter.Checked);
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            m_masterGrid.ReloadData();
        }

        private IList<object> m_selectedEntities = new List<object>();
        /// <summary>
        /// SelectedEntites
        /// </summary>
        public IList<object> SelectedEntites
        {
            get { return m_selectedEntities; }
        }

        private void tsbOk_Click(object sender, EventArgs e)
        {
            m_selectedEntities.Clear();
            foreach (Xceed.Grid.DataRow row in m_masterGrid.DataRows)
            {
                GetSelectedEntities(row);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void GetSelectedEntities(Xceed.Grid.DataRow row)
        {
            if (!row.Visible)
                return;

            if (Convert.ToBoolean(row.Cells[Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName].Value))
            {
                m_selectedEntities.Add(row.Tag);
            }

            foreach (MyDetailGrid detailGrid in row.DetailGrids)
            {
                foreach (Xceed.Grid.DataRow subRow in detailGrid.DataRows)
                {
                    GetSelectedEntities(subRow);
                }
            }
        }

        private void tsbSetup_Click(object sender, EventArgs e)
        {
            using (ArchiveSetupForm form = new ArchiveSetupForm(m_masterGrid, m_masterGrid.DisplayManager))
            {
                form.ShowDialog(this);
            }
        }
    }
}
