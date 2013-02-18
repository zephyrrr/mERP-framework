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
    /// 筛选对话框
    /// </summary>
    public partial class FilterForm : MyForm
    {
        /// <summary>
        /// 操作符，对筛选条件比较
        /// </summary>
        protected enum Operators
        {
            /// <summary>
            /// Equal
            /// </summary>
            Equal,
            /// <summary>
            /// NotEqual
            /// </summary>
            NotEqual,
            /// <summary>
            /// GreaterThan
            /// </summary>
            GreaterThan,
            /// <summary>
            /// LessThan
            /// </summary>
            LessThan,
            /// <summary>
            /// GreaterOrEqualThan
            /// </summary>
            GreaterOrEqualThan,
            /// <summary>
            /// LessOrEqualThan
            /// </summary>
            LessOrEqualThan,
            /// <summary>
            /// Like
            /// </summary>
            Like
        }


        /// <summary>
        /// Constructor
        /// </summary>
        private FilterForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filterGrid"></param>
        public FilterForm(IGrid filterGrid)
        {
            m_filterGrid = filterGrid;
            InitializeComponent();

            if (m_filterGrid.Columns[Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName] != null)
            {
                m_selectCaption = Feng.Grid.Columns.CheckColumn.DefaultSelectColumnName;
            }
            else
            {
                throw new ArgumentException("filterGrid must have checkColumn!", "filterGrid");
            }
        }

        private string m_selectCaption;

        /// <summary>
        /// Form_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Load(object sender, EventArgs e)
        {
            base.Form_Load(sender, e);

            IList<NameValue> list = new List<NameValue>();
            list.Add(new NameValue("重新筛选", "0"));
            list.Add(new NameValue("在结果中筛选", "1"));
            list.Add(new NameValue("在结果中增加", "2"));
            list.Add(new NameValue("在结果中去除", "3"));
            rbgFilterMethod.DataSource = list;
            rbgFilterMethod.DisplayMember = "Name";
            rbgFilterMethod.ValueMember = "Value";

            rbgFilterMethod.SelectedIndex = 0;

            if (m_bFilterShow)
            {
                foreach (KeyValuePair<IDataControl, Operators> kvp in m_ht)
                {
                    if (kvp.Key.NotNull && kvp.Key.SelectedDataValue != null)
                    {
                        kvp.Key.ReadOnly = true;
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<IDataControl, Operators> kvp in m_ht)
                {
                    kvp.Key.ReadOnly = false;
                }
            }
        }

        /// <summary>
        /// Form_Closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                return;
            }

            foreach (KeyValuePair<IDataControl, Operators> kvp in m_ht)
            {
                if (kvp.Key.NotNull && kvp.Key.SelectedDataValue == null)
                {
                    MessageForm.ShowError("必须选择" + kvp.Key.Caption);
                    e.Cancel = true;
                    break;
                }
            }

            base.Form_Closing(sender, e);
        }

        //private DataControlCollection m_dataControlGroup = new DataControlCollection();
        private Dictionary<IDataControl, Operators> m_ht = new Dictionary<IDataControl, Operators>();

        private IGrid m_filterGrid;

        /// <summary>
        /// 加入比较控件
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="notNull"></param>
        /// <param name="op"></param>
        protected void AddDataControl(IDataControl dc, bool notNull, Operators op)
        {
            //m_dataControlGroup.Add(dc);
            m_ht.Add(dc, op);
            dc.NotNull = notNull;
        }

        /// <summary>
        /// 加入比较控件(比较默认为相等)
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="allowNull"></param>
        protected void AddDataControl(IDataControl dc, bool allowNull)
        {
            AddDataControl(dc, allowNull, Operators.Equal);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_bFilterShow)
            {
                Filter();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void SelectRow(Xceed.Grid.DataRow row, Xceed.Grid.DataRow rowMaster)
        {
            bool prevSelect = Convert.ToBoolean(row.Cells[m_selectCaption].Value);
            bool nowSelect = true;

            foreach (KeyValuePair<IDataControl, Operators> kvp in m_ht)
            {
                if (kvp.Key.SelectedDataValue == null)
                {
                    continue;
                }

                object cellValue = null;
                if (row.Cells[kvp.Key.Name] != null)
                {
                    cellValue = row.Cells[kvp.Key.Name].Value;
                }
                else if (rowMaster != null)
                {
                    cellValue = rowMaster.Cells[kvp.Key.Name].Value;
                }

                if (cellValue == null)
                {
                    nowSelect = false;
                    continue;
                }

                switch (kvp.Value)
                {
                    case Operators.Equal:
                        nowSelect &= (cellValue.ToString() == kvp.Key.SelectedDataValue.ToString());
                        break;
                    case Operators.NotEqual:
                        nowSelect &= (cellValue.ToString() != kvp.Key.SelectedDataValue.ToString());
                        break;
                    case Operators.GreaterThan:
                        nowSelect &= (Convert.ToDateTime(cellValue) > Convert.ToDateTime(kvp.Key.SelectedDataValue));
                        break;
                    case Operators.GreaterOrEqualThan:
                        nowSelect &= (Convert.ToDateTime(cellValue) >= Convert.ToDateTime(kvp.Key.SelectedDataValue));
                        break;
                    case Operators.LessThan:
                        nowSelect &= (Convert.ToDateTime(cellValue) <
                                      Convert.ToDateTime(kvp.Key.SelectedDataValue).AddDays(1));
                        break;
                    case Operators.LessOrEqualThan:
                        nowSelect &= (Convert.ToDateTime(cellValue) <=
                                      Convert.ToDateTime(kvp.Key.SelectedDataValue).AddDays(1));
                        break;
                    case Operators.Like:
                        nowSelect &= (cellValue.ToString().Contains(kvp.Key.SelectedDataValue.ToString()));
                        break;
                }
            }
            switch (rbgFilterMethod.SelectedValue.ToString())
            {
                case "0":
                    row.Cells[m_selectCaption].Value = nowSelect;
                    break;
                case "1":
                    row.Cells[m_selectCaption].Value = prevSelect && nowSelect;
                    break;
                case "2":
                    row.Cells[m_selectCaption].Value = prevSelect || nowSelect;
                    break;
                case "3":
                    row.Cells[m_selectCaption].Value = prevSelect && !nowSelect;
                    break;
            }
        }

        /// <summary>
        /// Filter
        /// 对应多极选项，如果主项有，只考虑主项，如果主项没有，考虑相应子项
        /// </summary>
        public void Filter()
        {
            foreach (Xceed.Grid.DataRow rowMaster in m_filterGrid.DataRows)
            {
                if (rowMaster.DetailGrids.Count > 0)
                {
                    foreach (Xceed.Grid.DataRow row in rowMaster.DetailGrids[0].DataRows)
                    {
                        SelectRow(row, rowMaster);
                    }
                }
                    // 非二级Grid
                else
                {
                    SelectRow(rowMaster, null);
                }
            }
        }

        private bool m_bFilterShow = true;

        /// <summary>
        /// ShowDialog
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public new DialogResult ShowDialog(IWin32Window owner)
        {
            return ShowDialog(owner, true);
        }

        /// <summary>
        /// ShowDiaglog
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="bFilterShow">是否是Filter操作引起(OK后是否Filter, 如为否，则是手工调用Filter</param>
        public DialogResult ShowDialog(IWin32Window owner, bool bFilterShow)
        {
            m_bFilterShow = bFilterShow;
            return base.ShowDialog(owner);
        }

        /// <summary>
        /// 取得选定的值
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public object GetControlValue(string caption)
        {
            foreach (KeyValuePair<IDataControl, Operators> kvp in m_ht)
            {
                if (kvp.Key.Caption == caption
                    || kvp.Key.Name == caption)
                {
                    return kvp.Key.SelectedDataValue;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置选定的值
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="setValue"></param>
        public void SetControlValue(string caption, object setValue)
        {
            foreach (KeyValuePair<IDataControl, Operators> kvp in m_ht)
            {
                if (kvp.Key.Caption == caption
                    || kvp.Key.Name == caption)
                {
                    kvp.Key.SelectedDataValue = setValue;
                    return;
                }
            }
        }
    }
}