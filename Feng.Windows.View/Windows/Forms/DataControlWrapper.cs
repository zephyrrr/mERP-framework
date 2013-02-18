using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Feng;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// DataControlWrapper
    /// </summary>
    //[Designer("Feng.Windows.Forms.Design.DataControlWrapperDesigner, Feng.Windows.Forms.Design.dll")]
    [DefaultProperty("PropertyName")]
    public partial class DataControlWrapper : UserControl, IWindowDataControl, IControlWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        object IControlWrapper.InnerControl
        {
            get { return this.Control; }
        }

        #region "Properties"

        /// <summary>
        /// Constructor
        /// </summary>
        public DataControlWrapper()
        {
            InitializeComponent();
        }

        private string m_caption;

        /// <summary>
        /// 字段名称。提示标题
        /// </summary>
        [DefaultValue(null)]
        public string Caption
        {
            get { return m_caption; }
            set { m_caption = value; }
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

        private int m_group;

        /// <summary>
        /// 分组信息
        /// </summary>
        [DefaultValue(0)]
        public int Group
        {
            get { return m_group; }
            set { m_group = value; }
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

                if (string.IsNullOrEmpty(m_caption))
                {
                    Caption = value;
                }
            }
        }

        private string m_navigator;

        /// <summary>
        /// 控件相关Navigator
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

        public DataControlType ControlType
        {
            get;
            set;
        }

        //private bool m_insertable;
        ///// <summary>
        ///// 是否允许Insert
        ///// 默认除了主键,时间戳=false,和控件相关的=true
        ///// </summary>
        //[DefaultValue(false)]
        //public bool Insertable
        //{
        //    get { return m_insertable; }
        //    set { m_insertable = value; }
        //}

        //private bool m_editable;
        ///// <summary>
        ///// 是否允许Edit
        ///// 默认和控件相关的=false
        ///// </summary>
        //[DefaultValue(false)]
        //public bool Editable
        //{
        //    get { return m_editable; }
        //    set { m_editable = value; }
        //}

        private bool m_notNull = true;

        /// <summary>
        /// 是否允许空
        /// </summary>
        [DefaultValue(true)]
        public bool NotNull
        {
            get { return m_notNull; }
            set { m_notNull = value; }
        }


        private Control m_control;

        /// <summary>
        /// 内部IDataControl。若不是Control返回Null 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Control Control
        {
            get { return m_control; }
            set
            {
                if (m_control == value)
                {
                    return;
                }

                if (m_control != null)
                {
                    this.Controls.Remove(m_control);
                }

                if (value == null)
                {
                    return;
                }

                if (!(value is IDataValueControl))
                {
                    throw new ArgumentException("Invalid Control. Must Be IDataValueControl.", "value");
                }
                m_control = value;

                if (m_control != null)
                {
                    this.Controls.Add(m_control);
                    m_control.BringToFront();
                    m_control.BringToFront();

                    m_control.Enter -= new EventHandler(m_control_Enter);
                    m_control.Validated -= new EventHandler(m_control_Validated);
                    m_control.Enter += new EventHandler(m_control_Enter);
                    m_control.Validated += new EventHandler(m_control_Validated);
                }
            }
        }


        /// <summary>
        /// SelectedDataValueChanged
        /// </summary>
        public event EventHandler SelectedDataValueChanged;

        protected virtual void OnSelectedDataValueChanged(System.EventArgs e)
        {
            if (SelectedDataValueChanged != null)
            {
                SelectedDataValueChanged(this, e);
            }
        }

        private object m_originalSelectedDataValue;
        void m_control_Validated(object sender, EventArgs e)
        {
            if (!Feng.Utils.ReflectionHelper.ObjectEquals(m_originalSelectedDataValue, this.SelectedDataValue))
            {
                m_originalSelectedDataValue = this.SelectedDataValue;
                OnSelectedDataValueChanged(System.EventArgs.Empty);
            }
        }

        void m_control_Enter(object sender, EventArgs e)
        {
            m_originalSelectedDataValue = this.SelectedDataValue;
        }

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// SelectedDataValue
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get
            {
                IDataValueControl control = m_control as IDataValueControl;
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
                IDataValueControl control = m_control as IDataValueControl;
                if (control != null)
                {
                    object selectedDataValue = control.SelectedDataValue;
                    if (this.ResultType != null)
                    {
                        value = Feng.Utils.ConvertHelper.ChangeType(value, this.ResultType);
                        selectedDataValue = Feng.Utils.ConvertHelper.ChangeType(selectedDataValue, this.ResultType);
                    }

                    if (!Feng.Utils.ReflectionHelper.ObjectEquals(value, selectedDataValue))
                    {
                        control.SelectedDataValue = value;

                        OnSelectedDataValueChanged(System.EventArgs.Empty);
                    }
                }
            }
        }

        #endregion

        #region "IStateControl"
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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnly
        {
            get
            {
                IDataValueControl control = m_control as IDataValueControl;
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
                IDataValueControl control = m_control as IDataValueControl;
                if (control != null && control.ReadOnly != value)
                {
                    control.ReadOnly = value;
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
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            //IStateControl control = m_control as IStateControl;
            //if (control != null)
            //{
            //    control.SetState(state);
            //}
            StateControlHelper.SetState(this, state);
        }

        #endregion

        #region "Position"

        /// <summary>
        /// 重新计算控件位置
        /// Additional 20 is for error icon
        /// </summary>
        public virtual void ResetLayout()
        {
            if (this.Control == null)
                return;

            this.Control.Location = new Point(0, 0);

            if (m_control is MyMultilineTextBox2)
            {
                m_control.Size = new Size(112, 42);
                this.Size = new System.Drawing.Size(112 + 16, 42);
                this.Control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            }
            else
            {
                this.Control.Size = new Size(112, 21);
                if (this.Control.Width < 100)
                {
                    this.Control.Location = new Point((100 - this.Control.Width) / 2, 0);
                }
                this.Size = new System.Drawing.Size(112 + 16, 21);
                this.Control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            }

            this.Control.BringToFront();

            Invalidate();
        }

        #endregion

    }
}