namespace Feng.Windows.Forms
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// 软件默认RadioButton，包含如下属性：
    /// <list type="bullet">
    /// <item>FlatStyle = FlatStyle.System</item>
    /// <item>TextAlign = System.Drawing.ContentAlignment.MiddleLeft</item>
    /// </list>
    /// </summary>
    public class MyRadioButton : System.Windows.Forms.RadioButton, IDataValueControl
    {
        #region "Default Property"

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyRadioButton()
            : base()
        {
            base.FlatStyle = System.Windows.Forms.FlatStyle.System;
            base.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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

        #endregion

        #region "IDataValueControl"

        /// <summary>
        /// SelectedDataValue = CheckState
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedDataValue
        {
            get { return this.Checked; }
            set
            {
                if (value == null)
                {
                    this.Checked = false;
                }
                else
                {
                    try
                    {
                        this.Checked = Feng.Utils.ConvertHelper.ToBoolean(value).Value;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyRadioButton's SelectedDataValue must be bool", ex);
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