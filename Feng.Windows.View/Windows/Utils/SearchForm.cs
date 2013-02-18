using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Utils
{
    public partial class AdInfoSearchForm : Feng.Windows.Forms.MyChildForm
    {
        public AdInfoSearchForm()
        {
            InitializeComponent();
        }

        protected override void Form_Load(object sender, EventArgs e)
        {
            m_grid = new Grid.MyGrid();
            m_grid.GridName = "AdInfoSearch";   // for save layout
            m_grid.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());
            m_grid.Columns.Add(new Xceed.Grid.Column("Table", typeof(string)));
            m_grid.Columns.Add(new Xceed.Grid.Column("Column", typeof(string)));
            m_grid.Columns.Add(new Xceed.Grid.Column("Value", typeof(string)));
            m_grid.Dock = DockStyle.Fill;
            m_grid.ReadOnly = true;
            m_grid.DataRowTemplate.Cells["Value"].CellViewerManager = new Feng.Grid.Viewers.MultiLineViewer();
            m_grid.DataRowTemplate.Cells["Value"].DoubleClick += new EventHandler(AdInfoSearchForm_DoubleClick);
            splitContainer1.Panel1.Controls.Add(m_grid);

            base.Form_Load(sender, e);
        }

        void AdInfoSearchForm_DoubleClick(object sender, EventArgs e)
        {
            Xceed.Grid.DataCell cell = sender as Xceed.Grid.DataCell;
            var row = cell.ParentRow;
            var dmc = ServiceProvider.GetService<IApplication>().ExecuteAction(row.Cells["Table"].Value.ToString()) as IDisplayManagerContainer;
            dmc.DisplayManager.SearchManager.LoadData(SearchExpression.Eq(row.Cells["Column"].Value.ToString(), row.Cells["Value"].Value.ToString()), null);
        }
        private Feng.Grid.MyGrid m_grid;
        private void btnRun_Click(object sender, EventArgs e)
        {
            m_grid.DataRows.Clear();
            if (string.IsNullOrEmpty(txtQuery.Text))
                return;
            var list = ADUtils.FindInAdInfos(txtQuery.Text);
            foreach (var i in list)
            {
                var row = m_grid.DataRows.AddNew();
                row.Cells[0].Value = i.Item1;
                row.Cells[1].Value = i.Item2;
                row.Cells[2].Value = i.Item3;
            }
        }

        private void txtClear_Click(object sender, EventArgs e)
        {
            string filePath = SystemDirectory.WorkDirectory + "\\AllAdInfos.xml";
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            //txtQuery.Clear();
            m_grid.DataRows.Clear();
        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnRun_Click(btnRun, System.EventArgs.Empty);
            }
        }
    }
}
