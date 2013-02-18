namespace Feng.Windows.Forms
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// 软件默认Button，包含如下属性：
    /// <list type="bullet">
    /// <item>BorderStyle = BorderStyle.None</item>
    /// <item>FlatStyle = FlatStyle.Standard</item>
    /// <item>Size = (50,21)</item>
    /// <item>TextAlign = System.Drawing.ContentAlignment.MiddleLeft</item>
    /// </list>
    /// </summary>
    public class MyLabel : System.Windows.Forms.Label, IDataValueControl
    {
        #region "Default Property"

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyLabel()
            : base()
        {
            base.BorderStyle = System.Windows.Forms.BorderStyle.None;
            base.FlatStyle = System.Windows.Forms.FlatStyle.System;
            base.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            base.Size = new System.Drawing.Size(50, 21);
            base.AutoSize = false;
        }

        /// <summary>
        /// AutoSize
        /// </summary>
        [DefaultValue(false)]
        public new bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
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
        /// Default BorderStyle 
        /// </summary>
        [DefaultValue(System.Windows.Forms.BorderStyle.None)]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
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

        #region "IStateControl"

        /// <summary>
        /// 对显示控件设置State
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReadOnly
        {
            get { return true; }
            set 
            {
                if (!value)
                {
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

        /// <summary>
        /// 文本框内容
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object SelectedDataValue
        {
            get
            {
                if (string.IsNullOrEmpty(Text))
                {
                    return null;
                }
                else
                {
                    return Text.Trim();
                }
            }
            set
            {
                if (value == null)
                {
                    this.Text = String.Empty;
                }
                else
                {
                    try
                    {
                        base.Text = value.ToString().Trim();
                    }
                    catch
                    {
                        throw new ArgumentException("MyLabel's SelectedDataValue must be string");
                    }
                }
            }
        }

        #endregion
    }
}