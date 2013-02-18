using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyObjectSelectTextBox
    /// </summary>
    public class MyObjectSelectTextBox : MyButtonTextBox, IDataValueControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MyObjectSelectTextBox()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        protected MyObjectSelectTextBox(MyObjectSelectTextBox template)
            : base(template)
        {
            this.DisplayMember = template.DisplayMember;

            this.DisplayMember = template.DisplayMember;
            this.ButtonClick = template.ButtonClick;
        }

        #region "ReadOnly, Visible and Enable"

        private bool m_bReadOnly;

        /// <summary>
        /// 获取或设置只读属性
        /// </summary>
        public bool ReadOnly
        {
            get { return m_bReadOnly; }
            set
            {
                if (m_bReadOnly != value)
                {
                    m_bReadOnly = value;

                    this.TextBoxArea.ReadOnly = value;
                    this.SideButtons[0].Enabled = !value;

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

        #region "IDataValueControl"
        private object m_selectedObject;
        /// <summary>
        /// 显示时候用于显示的属性名
        /// </summary>
        public string DisplayMember
        {
            get;
            set;
        }

        /// <summary>
        /// 设置OptionPicker时对应的值的组合
        /// </summary>
        public object SelectedDataValue
        {
            get
            {
                return m_selectedObject;
            }
            set
            {
                m_selectedObject = value;
                if (value == null)
                {
                    this.TextBoxArea.Text = String.Empty;
                }
                else
                {
                    try
                    {
                        this.TextBoxArea.Text = EntityHelper.ReplaceEntity(DisplayMember, m_selectedObject, null);
                    }
                    catch(Exception ex)
                    {
                        throw new ArgumentException("MyObjectSelectTextBox's SelectedDataValue must be object", ex);
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion

        public override object Clone()
        {
            return new MyObjectSelectTextBox(this);
        }
    }
}
