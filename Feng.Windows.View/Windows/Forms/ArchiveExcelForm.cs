using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using Feng.Grid;

namespace Feng.Windows.Forms
{
	/// <summary>
	/// Description of ArchiveExcelForm.
	/// </summary>
    public partial class ArchiveExcelForm : ArchiveGridForm
	{
		public ArchiveExcelForm()
		{
			InitializeComponent();

            base.MergeMenu(this.menuStrip1);
            base.MergeToolStrip(this.toolStrip1);

            this.tsbImport.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconOpen.png").Reference;
            this.tsbExport.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconExportExcel.png").Reference;
            this.tsbSave.Image = Feng.Windows.ImageResource.Get("Feng", "Icons.iconSave.png").Reference;

			m_excelGrid = new Feng.Grid.MyExcelGrid();
			m_excelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Controls.Add(m_excelGrid);
			
			this.WindowState = FormWindowState.Maximized;
		}

        private Feng.Grid.MyExcelGrid m_excelGrid;
        /// <summary>
        /// 
        /// </summary>
        public MyExcelGrid ExcelGrid
        {
            get { return m_excelGrid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public override IGrid MasterGrid
        {
            get { return m_excelGrid; }
        }

        protected override void AssociateMenuToToolStrip()
        {
            m_assoMenu2ToolStrip.Associate(tsmImport, tsbImport);
            m_assoMenu2ToolStrip.Associate(tsmExport, tsbExport);
            m_assoMenu2ToolStrip.Associate(tsmSave, tsbSave);
        }

        private void tsbImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "所有 Excel 文件|*.xls;*.xlsx;*.xml|Excel 97-2003 文件|*.xls|Excel 2007 文件|*.xlsx|XML 文件|*.xml";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    IList<DataTable> list = Feng.Windows.Utils.ExcelHelper.ReadExcel(openFileDialog1.FileName, true);
                    foreach (DataTable dt in list)
                    {
                        for(int i=0; i<dt.Rows.Count; ++i)
                        {
                            DataRow row = dt.Rows[i];
                            for (int j = 0; j < dt.Columns.Count; ++j)
                            {
                                if (i >= m_excelGrid.DataRows.Count)
                                {
                                    m_excelGrid.DataRows.AddNew().EndEdit();
                                }

                                Xceed.Grid.Cell destCell = null;
                                // Caption<->GridColumnName
                                foreach (Xceed.Grid.Cell cell in m_excelGrid.DataRows[i].Cells)
                                {
                                    if (cell.ParentColumn.Title == dt.Columns[j].ColumnName)
                                    {
                                        destCell = cell;
                                        break;
                                    }
                                }
                                if (destCell == null)
                                    continue;

                                object destFromValue = row[j];
                                object destToValue = null;
                                if (destFromValue == null || destFromValue == System.DBNull.Value)
                                {
                                    destToValue = null;
                                }
                                else
                                {
                                    if (destCell.CellEditorManager != null && destCell.CellEditorManager is INameValueControl)
                                    {
                                        destToValue = ((INameValueControl)destCell.CellEditorManager).GetValue(destFromValue.ToString());
                                    }
                                    else
                                    {
                                        destToValue = Feng.Utils.ConvertHelper.ChangeType(destFromValue, destCell.ParentColumn.DataType);
                                        if (destToValue == null)
                                        {
                                            ServiceProvider.GetService<IMessageBox>().ShowWarning("\"" + destFromValue + "\"类型不匹配目标类型！");
                                            continue;
                                        }
                                    }
                                }

                                destCell.Value = destToValue;
                            }
                        }
                        // only to sheet 1
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcess.ProcessWithNotify(ex);
                }
            }
            openFileDialog1.Dispose();
        }
        private void tsbExport_Click(object sender, EventArgs e)
        {
            MyGrid.ExportToExcelCommand.Execute(this.m_excelGrid, ExecutedEventArgs.Empty);
        }

        public IControlManager ControlManager
        {
            get { return m_cm; }
            set { m_cm = value; }
        }

        private IControlManager m_cm;
        private bool m_haveSaved;
        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (m_haveSaved)
            {
                if (!ServiceProvider.GetService<IMessageBox>().ShowYesNoDefaultNo("已保存过一次，是否再次保存(会导致2份记录)？", "确认"))
                    return;
            }
            int cnt = 0;

            IBatchDao batchDao = m_cm.Dao as IBatchDao;
            if (batchDao == null)
            {
                ServiceProvider.GetService<IMessageBox>().ShowWarning("不支持批量保存，将逐条保存！");
            }

            try
            {
                m_cm.CancelEdit();
                MyGrid.CancelEditCurrentDataRow(m_excelGrid);

                if (batchDao != null)
                {
                    batchDao.SuspendOperation();
                }

                foreach (Xceed.Grid.DataRow row in m_excelGrid.DataRows)
                {
                    bool hasValue = false;
                    foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(m_excelGrid.GridName))
                    {
                        if (row.Cells[info.GridColumnName] != null && !string.IsNullOrEmpty(info.PropertyName))
                        {
                            if (row.Cells[info.GridColumnName].ReadOnly)
                            {
                                continue;
                            }
                            if (row.Cells[info.GridColumnName].Value != null)
                            {
                                hasValue = true;
                            }
                        }
                    }
                    if (!hasValue)
                        continue;

                    object entity = m_cm.AddNew();
                    if (entity == null)
                        continue;

                    foreach (GridColumnInfo info in ADInfoBll.Instance.GetGridColumnInfos(m_excelGrid.GridName))
                    {
                        if (row.Cells[info.GridColumnName] != null && !string.IsNullOrEmpty(info.PropertyName))
                        {
                            if (row.Cells[info.GridColumnName].ReadOnly)
                            {
                                continue;
                            }

                            EntityScript.SetPropertyValue(entity, info.Navigator, info.PropertyName, row.Cells[info.GridColumnName].Value);
                        }
                    }

                    m_cm.EndEdit(false);

                    m_cm.Dao.Save(entity);
                    cnt++;
                }
                if (batchDao != null)
                {
                    batchDao.ResumeOperation();
                }

                m_haveSaved = true;
                MessageForm.ShowInfo(string.Format("已保存{0}条记录！", cnt));
            }
            catch (Exception ex)
            {
                if (batchDao != null)
                {
                    batchDao.CancelSuspendOperation();
                }
                ExceptionProcess.ProcessWithNotify(ex);
            }
        }
	}
}
