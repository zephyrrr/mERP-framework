using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Utils;
using Feng.Windows.Forms;
using Feng.Search;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormCustomFilter : System.Windows.Forms.Form
    {
        /// <summary>
        /// 
        /// </summary>
        public FormCustomFilter()
        {
            InitializeComponent();

            cobOperator1.Columns.Add(new Xceed.Editors.ColumnInfo("操作符", typeof(ComparisonType)));
            cobOperator1.Columns.Add(new Xceed.Editors.ColumnInfo("说明", typeof(string)));
            this.cobOperator1.DisplayFormat = "%说明%";
            this.cobOperator1.ValueMember = "操作符";
            cobOperator2.Columns.Add(new Xceed.Editors.ColumnInfo("操作符", typeof(ComparisonType)));
            cobOperator2.Columns.Add(new Xceed.Editors.ColumnInfo("说明", typeof(string)));
            this.cobOperator2.DisplayFormat = "%说明%";
            this.cobOperator2.ValueMember = "操作符";

            foreach (var i in Feng.Utils.EnumHelper.EnumToList(typeof(ComparisonType)))
            {
                cobOperator1.Items.Add(new object[] { i.Value, i.Description });
                cobOperator2.Items.Add(new object[] { i.Value, i.Description });
            }

            cobOperator1.AdjustDropDownControlSize();
            cobOperator2.AdjustDropDownControlSize();

            cobValue1.Columns.Clear();
            cobValue1.Columns.Add(new Xceed.Editors.ColumnInfo("Text", typeof (string)));
            cobValue1.ValueMember = "Text";
            cobValue1.DisplayFormat = "%Text%";

            cobValue2.Columns.Clear();
            cobValue2.Columns.Add(new Xceed.Editors.ColumnInfo("Text", typeof (string)));
            cobValue2.ValueMember = "Text";
            cobValue2.DisplayFormat = "%Text%";
        }

        private Xceed.Grid.GridControl m_grid;
        private string m_columnName;

        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnName"></param>
        public DialogResult ShowDialog(Xceed.Grid.GridControl grid, string columnName)
        {
            m_grid = grid;
            m_columnName = columnName;
            m_data.Clear();

            if (m_grid.Columns[columnName].CellViewerManager is INameValueControl)
            {
                foreach (Xceed.Grid.DataRow row in grid.DataRows)
                {
                    string s = row.Cells[m_columnName].GetDisplayText();
                    {
                        m_data[s] = s;
                    }
                }
            }
            else
            {
                foreach (Xceed.Grid.DataRow row in grid.DataRows)
                {
                    string s = row.Cells[m_columnName].GetDisplayText();
                    if (!string.IsNullOrEmpty(s))
                    {
                        m_data[s] = row.Cells[m_columnName].Value;
                    }
                }
            }
            cobValue1.Items.Clear();
            cobValue2.Items.Clear();
            foreach (KeyValuePair<string, object> kvp in m_data)
            {
                cobValue1.Items.Add(new object[] {kvp.Key});
                cobValue2.Items.Add(new object[] {kvp.Key});
            }
            cobValue1.AdjustDropDownControlSize();
            cobValue2.AdjustDropDownControlSize();

            return base.ShowDialog();
        }

        private bool m_ok;
        private IFilter m_filter;

        /// <summary>
        /// 
        /// </summary>
        public IFilter Filter
        {
            get { return m_filter; }
        }

        private object GetComboValueSelectedValue(MyFreeComboBox cobValue)
        {
            string v = cobValue.SelectedDataValue.ToString();
            if (m_data.ContainsKey(v))
            {
                return m_data[v];
            }
            else
            {
                if (m_grid.Columns[m_columnName].CellViewerManager is INameValueControl)
                {
                    return v;
                }
                else
                {
                    return Feng.Utils.ConvertHelper.ChangeType(v, m_grid.Columns[m_columnName].DataType);
                }
            }
        }
        private IFilter GetFilter()
        {
            try
            {
                if (cobOperator1.SelectedDataValue == null
                    || cobValue1.SelectedDataValue == null)
                {
                    return null;
                }
                IFilter filter1 = new ComparisonFilter((ComparisonType) cobOperator1.SelectedDataValue, GetComboValueSelectedValue(cobValue1));

                if (cobOperator2.SelectedDataValue == null || cobValue2.SelectedDataValue == null)
                {
                    return filter1;
                }

                IFilter filter2 = new ComparisonFilter((ComparisonType)cobOperator2.SelectedDataValue, GetComboValueSelectedValue(cobValue2));

                return rbnLogic.Checked ? new LogicalFilter(LogicalOperator.And, filter1, filter2)
                           : new LogicalFilter(LogicalOperator.Or, filter1, filter2);
            }
            catch (Exception ex)
            {
                ExceptionProcess.ProcessWithResume(ex);
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_filter = GetFilter();
            if (m_filter != null)
            {
                m_ok = true;
            }
            else
            {
                MessageForm.ShowError("输入格式有误！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_ok = true;
        }

        private void FormCustomFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_ok)
            {
                e.Cancel = true;
            }
        }
    }
}