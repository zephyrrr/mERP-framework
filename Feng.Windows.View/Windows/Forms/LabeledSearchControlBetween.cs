using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;
using Feng;
using Feng.Search;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// LabeledFindControlBetween
    /// </summary>
    //[Designer("Feng.Windows.Forms.Design.LabeledFindControlBetweenDesigner, Feng.Windows.Forms.Design.dll")]
    [DefaultProperty("PropertyName")]
    public partial class LabeledSearchControlBetween : System.Windows.Forms.UserControl, ISearchControl, IWindowControlBetween, IDisposable
    {
        private Label m_label;
        private Control m_control1, m_control2;

        #region "Constructor"

        /// <summary>
        /// Constructor
        /// </summary>
        public LabeledSearchControlBetween()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_propertyName;
        }

        #endregion

        #region "Properties"

        private string m_caption;

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public string Caption
        {
            get { return m_caption; }
            set
            {
                m_caption = value;

                if (m_label != null)
                {
                    m_label.Text = value;
                }
            }
        }

        private string m_propertyName;

        /// <summary>
        /// 控件相关Column
        /// </summary>
        [DefaultValue(null)]
        public string PropertyName
        {
            get { return m_propertyName; }
            set
            {
                m_propertyName = value;

                if (string.IsNullOrEmpty(m_caption)
                    || m_caption == "标题")
                {
                    Caption = value;
                }
            }
        }

        private string m_navigator;

        /// <summary>
        /// Navigator
        /// </summary>
        [DefaultValue(null)]
        public string Navigator
        {
            get { return m_navigator; }
            set { m_navigator = value; }
        }

        /// <summary>
        /// 返回类型
        /// </summary>
        public Type ResultType
        {
            get;
            set;
        }

        /// <summary>
        ///  在父控件中的排列顺序
        /// </summary>
        [DefaultValue(-1)]
        public int Index
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.Controls.GetChildIndex(this);
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (this.Parent != null)
                {
                    this.Parent.Controls.SetChildIndex(this, value);
                }
            }
        }

        /// <summary>
        /// 内部IDataControl。若不是Control返回Null 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Label Label
        {
            get { return m_label; }
            set
            {
                if (m_label == value)
                {
                    return;
                }

                if (m_label != null)
                {
                    this.Controls.Remove(m_label);
                }
                m_label = value;
                if (m_label != null)
                {
                    this.Controls.Add(m_label);
                }
            }
        }

        /// <summary>
        /// 内部IDataControl。若不是Control返回Null 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Control Control1
        {
            get { return m_control1; }
            set { m_control1 = value; }
        }

        /// <summary>
        /// 内部IDataControl。若不是Control返回Null 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Control Control2
        {
            get { return m_control2; }
            set { m_control2 = value; }
        }

        private bool m_available = true;
        /// <summary>
        /// 是否可见
        /// The Available property is different from the Visible property in that Available indicates whether the ToolStripItem is shown, while Visible indicates whether the ToolStripItem and its parent are shown.
        /// </summary>
        public bool Available
        {
            get { return m_available; }
            set
            {
                if (m_available != value)
                {
                    m_available = value;
                    if (AvailableChanged != null)
                    {
                        AvailableChanged(this, System.EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// AvailableChanged
        /// </summary>
        public event EventHandler AvailableChanged;

        /// <summary>
        /// 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnly
        {
            get
            {
                IDataValueControl control = m_control1 as IDataValueControl;
                if (control != null)
                {
                    return control.ReadOnly;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                IDataValueControl control1 = m_control1 as IDataValueControl;
                IDataValueControl control2 = m_control2 as IDataValueControl;
                if (control1 != null && control2 != null && control1.ReadOnly != value)
                {
                    control1.ReadOnly = value;
                    control2.ReadOnly = value;
                    if (ReadOnlyChanged != null)
                    {
                        ReadOnlyChanged(this, System.EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ReadOnlyChanged;
        #endregion

        #region "ISearchControl"

        /// <summary>
        /// 设定值1
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue1
        {
            get
            {
                IDataValueControl control = m_control1 as IDataValueControl;
                if (control != null)
                {
                    if (this.ResultType != null)
                    {
                        return Feng.Utils.ConvertHelper.ChangeType(control.SelectedDataValue, this.ResultType);
                    }
                    else
                    {
                        return control.SelectedDataValue;
                    }
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (this.ResultType != null)
                {
                    value = Feng.Utils.ConvertHelper.ChangeType(value, this.ResultType);
                }
                IDataValueControl control = m_control1 as IDataValueControl;
                if (control != null)
                {
                    control.SelectedDataValue = value;
                }
            }
        }

        /// <summary>
        /// 设定值2
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue2
        {
            get
            {
                IDataValueControl control = m_control2 as IDataValueControl;
                if (control != null)
                {
                    if (this.ResultType != null)
                    {
                        return Feng.Utils.ConvertHelper.ChangeType(control.SelectedDataValue, this.ResultType);
                    }
                    else
                    {
                        return control.SelectedDataValue;
                    }
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (this.ResultType != null)
                {
                    value = Feng.Utils.ConvertHelper.ChangeType(value, this.ResultType);
                }
                IDataValueControl control = m_control2 as IDataValueControl;
                if (control != null)
                {
                    control.SelectedDataValue = value;
                }
            }
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
                ArrayList array = new ArrayList();
                array.Add(this.SelectedDataValue1);
                array.Add(this.SelectedDataValue2);
                return array;
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    SelectedDataValue1 = null;
                    SelectedDataValue2 = null;
                }
                else if (value.Count == 1)
                {
                    SelectedDataValue1 = value[0];
                    SelectedDataValue2 = null;
                }
                else
                {
                    SelectedDataValue1 = value[0];
                    SelectedDataValue2 = value[1];
                }
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        public bool? UseFuzzySearch
        {
            get { return false; }
            set { }
        }

        public bool CanSelectFuzzySearch
        {
            get { return false; }
            set { }
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
                    if (SearchNullUseFull)
                    {
                        searchConditions.Add(SearchExpression.IsNull(PropertyNameToSearch));
                    }
                    else
                    {
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
                if (SelectedDataValue1 == null && SelectedDataValue2 == null)
                {
                    return;
                }

                if (SelectedDataValue1 != null && SelectedDataValue2 == null)
                {
                    if (!IsNot)
                    {
                        searchConditions.Add(SearchExpression.Ge(PropertyNameToSearch, this.SelectedDataValue1));
                    }
                    else
                    {
                        searchConditions.Add(SearchExpression.Lt(PropertyNameToSearch, this.SelectedDataValue1));
                    }
                }
                else if (SelectedDataValue1 == null && SelectedDataValue2 != null)
                {
                    if (!IsNot)
                    {
                        searchConditions.Add(SearchExpression.Le(PropertyNameToSearch, this.SelectedDataValue2));
                    }
                    else
                    {
                        searchConditions.Add(SearchExpression.Gt(PropertyNameToSearch, this.SelectedDataValue2));
                    }
                }
                else
                {
                    if (!IsNot)
                    {
                        searchConditions.Add(SearchExpression.And(SearchExpression.Ge(PropertyNameToSearch, this.SelectedDataValue1),
                                                                    SearchExpression.Le(PropertyNameToSearch, this.SelectedDataValue2)));
                    }
                    else
                    {
                        searchConditions.Add(SearchExpression.Or(SearchExpression.Lt(PropertyNameToSearch, this.SelectedDataValue1),
                                                                    SearchExpression.Gt(PropertyNameToSearch, this.SelectedDataValue2)));
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.AdditionalSearchExpression))
            {
                searchConditions.Add(SearchExpression.Parse(this.AdditionalSearchExpression));
            }
        }

        #endregion

        #region "Not, IsNull, Order"
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
                        this.SelectedDataValue1 = null;
                        this.SelectedDataValue2 = null;
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

        private void tsm隐藏_Click(object sender, EventArgs e)
        {
            this.Available = !this.Available;

            //SaveLayout(this);
        }
        #endregion

        #region "Layout"
        /// <summary>
        /// 移动控件到默认位置
        /// </summary>
        /// <param name="control1"></param>
        /// <param name="control2"></param>
        /// <param name="label"></param>
        /// <param name="label1"></param>
        /// <param name="label2"></param>
        public static void ResetLayout(Control control1, Control control2, Label label, Label label1, Label label2)
        {
            control1.Location = new System.Drawing.Point(60, 0);
            control1.Size = new Size(112, 21);
            control2.Location = new System.Drawing.Point(60, 24);
            control2.Size = new Size(112, 21);

            label.Location = new System.Drawing.Point(0, 15);
            label.Size = new Size(50, 35);
            label1.Location = new System.Drawing.Point(50, 5);
            label1.Size = new Size(10, 21);
            label2.Location = new System.Drawing.Point(50, 30);
            label2.Size = new Size(10, 21);

            control1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            control2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        }

        #endregion

        //internal static void SaveLayout(Control c)
        //{
        //    if (c == null)
        //        return;

        //    if (c is ILayoutControl)
        //    {
        //        (c as ILayoutControl).SaveLayout();
        //    }
        //    else
        //    {
        //        SaveLayout(c.Parent);
        //    }
        //}
    }
}