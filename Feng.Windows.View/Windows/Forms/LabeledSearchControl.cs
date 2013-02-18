using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// LabeledFindControl
    /// </summary>
    public partial class LabeledSearchControl : LabeledDataControl, ISearchControl, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public LabeledSearchControl()
            : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public override System.Windows.Forms.Label Label
        {
            get
            {
                return base.Label;
            }
            set
            {
                base.Label = value;

                base.Label.ContextMenuStrip = null;
            }
        }

        /// <summary>
        /// 重新计算控件位置
        /// Additional 20 is for error icon
        /// </summary>
        public override void ResetLayout()
        {
            if (this.Control == null || this.Label == null)
                return;

            this.Label.Location = new System.Drawing.Point(0, 0);
            this.Label.Size = new Size(50, 24);

            this.Control.Location = new Point(60, 0);

            //if (m_control is MyMultilineTextBox)
            //{
            //    m_control.Size = new Size(304, 30);
            //    this.Size = new System.Drawing.Size(384, 33);
            //}
            //else
            this.Control.Size = new Size(112, 21);
            if (this.Control.Width < 100)
            {
                this.Control.Location = new Point(this.Label.Size.Width + (100 - this.Control.Width) / 2, 0);
            }
            this.Size = new System.Drawing.Size(172, 24);

            this.Control.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            this.Control.BringToFront();

            Invalidate();

            //this.Control.Size = new System.Drawing.Size(132, 21);
            //this.Size = new System.Drawing.Size(172, 24);
        }

        /// <summary>
        /// SelectedDataValues
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedDataValues
        {
            get
            {
                IMultiDataValueControl mdc = this.Control as IMultiDataValueControl;
                if (mdc != null)
                {
                    if (this.ResultType != null)
                    {
                        Feng.Utils.ConvertHelper.ChangeType(mdc.SelectedDataValues, this.ResultType);
                        return mdc.SelectedDataValues;
                    }
                    else
                    {
                        return mdc.SelectedDataValues;
                    }
                }
                else
                {
                    ArrayList array = new ArrayList();
                    array.Add(this.SelectedDataValue);
                    return array;
                }
            }
            set
            {
                IMultiDataValueControl mdc = this.Control as IMultiDataValueControl;
                if (mdc != null)
                {
                    if (this.ResultType != null)
                    {
                        Feng.Utils.ConvertHelper.ChangeType(value, this.ResultType);
                    }
                    mdc.SelectedDataValues = value;
                }
                else
                {
                    this.SelectedDataValue = (value == null || value.Count == 0) ? null : value[0];
                }
            }
        }

        /// <summary>
        /// 用于查找的PropertyName
        /// </summary>
        private string PropertyNameToSearch
        {
            get
            {
                if (string.IsNullOrEmpty(this.SpecialPropertyName))
                {
                    if (string.IsNullOrEmpty(this.Navigator))
                    {
                        return this.PropertyName;
                    }
                    else
                    {
                        return this.Navigator.Replace('.', ':') + ":" + this.PropertyName;
                    }
                }
                else
                {
                    return this.SpecialPropertyName;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string SpecialPropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string AdditionalSearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SearchNullUseFull
        {
            get;
            set;
        }

        public bool? UseFuzzySearch
        {
            get;
            set;
        }

        public bool CanSelectFuzzySearch
        {
            get;
            set;
        }

        void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.CanSelectFuzzySearch)
            {
                tsm模糊.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <param name="searchOrders"></param>
        public virtual void FillSearchConditions(IList<ISearchExpression> searchConditions, IList<ISearchOrder> searchOrders)
        {
            if (Order.HasValue)
            {
                if (Order.Value)
                {
                    searchOrders.Add(SearchOrder.Asc(PropertyNameToSearch));
                }
                else
                {
                    searchOrders.Add(SearchOrder.Desc(PropertyNameToSearch));
                }
            }

            if (IsNull)
            {
                if (!IsNot)
                {
                    //// 对帐单.对账单号 为空，其实意义就是对帐单为空
                    //if (string.IsNullOrEmpty(this.Navigator))
                    //{
                    //    searchConditions.Add(SearchExpression.IsNull(PropertyNameToSearch));
                    //}
                    //else
                    //{
                    //    // 为空的话，只去最前面一个
                    //    string[] ss = this.Navigator.Split(new char[] { '.', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    //    searchConditions.Add(SearchExpression.IsNull(ss[0]));
                    //}
                    if (SearchNullUseFull || string.IsNullOrEmpty(this.Navigator))
                    {
                        searchConditions.Add(SearchExpression.IsNull(PropertyNameToSearch));
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(this.Navigator))
                        {
                            throw new ArgumentException("When SearchNullUseFull is false, it will use Navigator's first part, so navigator must not null!");
                        }
                        string[] ss = this.Navigator.Split(new char[] { '.', ':' }, StringSplitOptions.RemoveEmptyEntries);
                        searchConditions.Add(SearchExpression.IsNull(ss[0]));
                    }
                }
                else
                {
                    searchConditions.Add(SearchExpression.IsNotNull(PropertyNameToSearch));
                }
            }
            else
            {
                IMultiDataValueControl mdc = this.Control as IMultiDataValueControl;
                if (mdc != null)
                {
                    if (this.SelectedDataValues.Count == 0)
                    {
                        return;
                    }

                    if (!this.UseFuzzySearch.HasValue)
                    {
                        if (this.CanSelectFuzzySearch && mdc.SelectedDataValues.Count == 1)
                        {
                            if (!IsNot)
                            {
                                searchConditions.Add(SearchExpression.Like(PropertyNameToSearch, mdc.SelectedDataValues[0]));
                            }
                            else
                            {
                                searchConditions.Add(SearchExpression.NotLike(PropertyNameToSearch, mdc.SelectedDataValues[0]));
                            }
                        }
                        else
                        {
                            if (!IsNot)
                            {
                                searchConditions.Add(SearchExpression.InG(PropertyNameToSearch, mdc.SelectedDataValues));
                            }
                            else
                            {
                                searchConditions.Add(SearchExpression.NotInG(PropertyNameToSearch, mdc.SelectedDataValues));
                            }
                        }
                    }
                    else if (this.UseFuzzySearch.Value)
                    {
                        if (!IsNot)
                        {
                            searchConditions.Add(SearchExpression.GInG(PropertyNameToSearch, mdc.SelectedDataValues));
                        }
                        else
                        {
                            searchConditions.Add(SearchExpression.NotGInG(PropertyNameToSearch, mdc.SelectedDataValues));
                        }
                    }
                    else
                    {
                        if (!IsNot)
                        {
                            searchConditions.Add(SearchExpression.InG(PropertyNameToSearch, mdc.SelectedDataValues));
                        }
                        else
                        {
                            searchConditions.Add(SearchExpression.NotInG(PropertyNameToSearch, mdc.SelectedDataValues));
                        }
                    }
                }
                else
                {
                    if (this.SelectedDataValue == null)
                    {
                        return;
                    }

                    if (this.CanSelectFuzzySearch && (!this.UseFuzzySearch.HasValue || this.UseFuzzySearch.Value))
                    {
                        string s = this.SelectedDataValue.ToString();
                        if (!IsNot)
                        {
                            searchConditions.Add(SearchExpression.Like(PropertyNameToSearch, s));
                        }
                        else
                        {
                            searchConditions.Add(SearchExpression.NotLike(PropertyNameToSearch, s));
                        }
                    }
                    else
                    {
                        if (!IsNot)
                        {
                            searchConditions.Add(SearchExpression.Eq(PropertyNameToSearch, this.SelectedDataValue));
                        }
                        else
                        {
                            searchConditions.Add(SearchExpression.NotEq(PropertyNameToSearch, this.SelectedDataValue));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.AdditionalSearchExpression))
            {
                searchConditions.Add(SearchExpression.Parse(this.AdditionalSearchExpression));
            }
        }

        private bool m_isNot;
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool IsNot
        {
            get { return m_isNot; }
            set
            {
                if (m_isNot != value)
                {
                    m_isNot = value;

                    if (this.Label != null)
                    {
                        this.Label.BackColor = (m_isNot
                                                    ? System.Drawing.SystemColors.HighlightText
                                                    : System.Drawing.SystemColors.Control);
                    }
                    tsm取反.Checked = m_isNot;
                }
            }
        }

        private bool m_isNull;
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool IsNull
        {
            get { return m_isNull; }
            set
            {
                if (m_isNull != value)
                {
                    m_isNull = value;

                    this.ReadOnly = m_isNull;
                    if (m_isNull)
                    {
                        this.SelectedDataValue = null;
                    }
                    tsm为空.Checked = m_isNull;
                }
            }
        }

        private bool? m_order;
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public bool? Order
        {
            get { return m_order; }
            set
            {
                if (m_order != value)
                {
                    m_order = value;

                    if (this.Label != null)
                    {
                        bool haveSign = this.Label.Text[0] == '↓' || this.Label.Text[0] == '↑';
                        if (value == null)
                        {
                            if (haveSign)
                            {
                                this.Label.Text = this.Label.Text.Substring(1);
                            }
                        }
                        else if (value.Value)
                        {
                            this.Label.Text = '↑' + this.Label.Text.Substring(haveSign ? 1 : 0);
                        }
                        else
                        {
                            this.Label.Text = '↓' + this.Label.Text.Substring(haveSign ? 1 : 0);
                        }
                    }

                    tsc顺序.SelectedIndex = m_order == null ? 0 : (m_order.Value ? 1 : 2);
                }
            }
        }

        private void tsc顺序_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tsc顺序.SelectedIndex == 0)
            {
                Order = null;
            }
            else if (tsc顺序.SelectedIndex == 1)
            {
                Order = true;
            }
            else if (tsc顺序.SelectedIndex == 2)
            {
                Order = false;
            }
            else
            {
                throw new ArgumentException("Invalid SelectedIndex");
            }
        }

        private void tsm取反_Click(object sender, System.EventArgs e)
        {
            IsNot = !IsNot;
        }

        private void tsm为空_Click(object sender, System.EventArgs e)
        {
            IsNull = !IsNull;
        }

        private void tsm模糊_Click(object sender, System.EventArgs e)
        {
            this.UseFuzzySearch = tsm模糊.CheckState == System.Windows.Forms.CheckState.Indeterminate ? (bool?)null :
                (tsm模糊.CheckState == System.Windows.Forms.CheckState.Unchecked ? false : true);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public event EventHandler ControlInvisibled;

        private void tsm隐藏_Click(object sender, System.EventArgs e)
        {
            this.Available = !this.Available;

            //LabeledSearchControlBetween.SaveLayout(this);
        }
    }
}