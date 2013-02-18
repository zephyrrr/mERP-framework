namespace Feng.Windows.Forms
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// 软件默认Button，包含如下属性：
    /// <list type="bullet">
    /// <item>FlatStyle = FlatStyle.System</item>
    /// <item>Size = (32,24)</item>
    /// <item>TextAlign = System.Drawing.ContentAlignment.MiddleLeft</item>
    /// </list>
    /// </summary>
    public class MyCheckBox : System.Windows.Forms.CheckBox, IDataValueControl
    {
        #region "Default Property"

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyCheckBox()
            : base()
        {
            this.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Size = new System.Drawing.Size(120, 21);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AutoSize = true;
            this.Text = " ";
        }

        /// <summary>
        /// Default FlatStyle
        /// </summary>
        [DefaultValue(System.Windows.Forms.FlatStyle.System)]
        public new System.Windows.Forms.FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        /// <summary>
        /// Default TextAlign
        /// </summary>
        [DefaultValue(System.Drawing.ContentAlignment.MiddleLeft)]
        public new System.Drawing.ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        /// <summary>
        /// Default AutoSize
        /// </summary>
        [DefaultValue(true)]
        public new bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        /// <summary>
        /// Default Text
        /// </summary>
        [DefaultValue(" ")]
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// SelectedDataValue = (bool)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue
        {
            get
            {
                switch (base.CheckState)
                {
                    case CheckState.Unchecked:
                        return false;
                    case CheckState.Checked:
                        return true;
                    default:
                        throw new NotSupportedException("MyCheckBox's SelectedDataValue must be bool");
                }
            }
            set
            {
                if (value == null)
                {
                    this.CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
                else
                {
                    try
                    {
                        bool b = Feng.Utils.ConvertHelper.ToBoolean(value).Value;
                        base.CheckState = b ? CheckState.Checked : CheckState.Unchecked;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyCheckBox's SelectedDataValue must be bool", ex);
                    }
                }
            }
        }


        /// <summary>
        /// ReadOnly = !Enable
        /// </summary>
        public bool ReadOnly
        {
            get { return !base.Enabled; }
            set
            {
                if (base.Enabled != !value)
                {
                    base.Enabled = !value;
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

        #region "IStateControl"

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion
    }
}