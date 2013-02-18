using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 自定义查找控件
    /// </summary>
    public partial class CustomSearchControl : UserControl, ISearchControl, IWindowControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomSearchControl()
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            this.Controls.Add(myComboBox1);
            myComboBox1.Location = new Point(60, 0);
            myComboBox1.Size = new Size(112, 21);
            //myComboBox1.DropDownControl.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());
            myComboBox1.Columns.Add(new Xceed.Editors.ColumnInfo("状态", typeof (string)));
            myComboBox1.ValueMember = "状态";
            myComboBox1.DisplayMember = "状态";
        }

        MyComboBox myComboBox1 = new MyComboBox();

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
        /// 根据GroupName从数据库读入数据
        /// </summary>
        public void LoadFromDatabase(string groupName)
        {
            Clear();

            IList<CustomSearchInfo> lists = ADInfoBll.Instance.GetCustomSearchInfo(groupName);
            for (int i = 0; i < lists.Count; ++i)
            {
                if (!Authority.AuthorizeByRule(lists[i].Visible))
                    continue;

                myComboBox1.Items.Add(new object[] {lists[i].Text});

                dict[lists[i].Text] = lists[i].SearchExpression;
            }

            myComboBox1.AdjustDropDownControlSize();
        }

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        /// <summary>
        /// 加入Combo's Items
        /// </summary>
        /// <param name="texts">texts</param>
        /// <param name="searchExps"></param>
        [Obsolete()]
        private void AddItems(string[] texts, string[] searchExps)
        {
            if (texts == null || searchExps == null)
            {
                throw new ArgumentNullException("texts");
            }
            if (texts.Length != searchExps.Length)
            {
                throw new ArgumentException("the count should be same");
            }

            for (int i = 0; i < texts.Length; ++i)
            {
                myComboBox1.Items.Add(new object[] { texts[i] });

                dict[texts[i]] = searchExps[i];
            }

            myComboBox1.AdjustDropDownControlSize();
        }

        /// <summary>
        /// 清空Item
        /// </summary>
        public void Clear()
        {
            myComboBox1.Items.Clear();
        }

        /// <summary>
        /// PropertyName
        /// </summary>
        public string PropertyName
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Navigator
        /// </summary>
        public string Navigator
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// ResultType
        /// </summary>
        public Type ResultType
        {
            get { return null; }
            set { }
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
        /// ReadOnly
        /// </summary>
        public bool ReadOnly
        {
            get { return myComboBox1.ReadOnly; }
            set 
            { 
                if (myComboBox1.ReadOnly != value)
                {
                    myComboBox1.ReadOnly = value; 
                    
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
        /// Caption
        /// </summary>
        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = value; }
        }

        /// <summary>
        /// SelectedDataValue
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedDataValues
        {
            get
            {
                ArrayList array = new ArrayList();
                array.Add(myComboBox1.SelectedValue);
                return array;
            }
            set { myComboBox1.SelectedDataValue = (value == null || value.Count == 0) ? null : value[0]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <param name="searchOrders"></param>
        public void FillSearchConditions(IList<ISearchExpression> searchConditions, IList<ISearchOrder> searchOrders)
        {
            if (myComboBox1.SelectedValue != null
                && dict.ContainsKey(myComboBox1.SelectedValue.ToString()))
            {
                searchConditions.Add(SearchExpression.Parse(dict[myComboBox1.SelectedValue.ToString()]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool IsNot
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool IsNull
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public bool? Order
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.Control Control
        {
            get { return this.myComboBox1; }
        }

        private void tsm隐藏_Click(object sender, EventArgs e)
        {
            this.Available = !this.Available;
        }
    }
}