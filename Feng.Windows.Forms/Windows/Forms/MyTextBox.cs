namespace Feng.Windows.Forms
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// 软件默认MyTextBox，包含如下属性：
    /// <list type="bullet">
    /// <item>BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D</item>
    /// <item>TextAlign = System.Windows.Forms.HorizontalAlignment.Left</item>
    /// <item>Multiline = false</item>
    /// <item>MaxLength = 20</item>
    /// <item>Size =(120, 21)</item>
    /// </list>
    /// </summary>
    public class MyTextBox : System.Windows.Forms.TextBox, IDataValueControl, IFormatControl
    {
        #region "Default Property"

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyTextBox()
            : base()
        {
            base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            base.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            base.Multiline = false;
            base.MaxLength = 24;

            base.Size = new System.Drawing.Size(120, 21);
        }

        /// <summary>
        /// Default BorderStyle 
        /// </summary>
        [DefaultValue(System.Windows.Forms.BorderStyle.Fixed3D)]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        /// Default TextAlign 
        /// </summary>
        [DefaultValue(System.Windows.Forms.HorizontalAlignment.Left)]
        public new System.Windows.Forms.HorizontalAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }


        /// <summary>
        /// Default Multiline 
        /// </summary>
        [DefaultValue(false)]
        public new bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = value; }
        }


        /// <summary>
        /// Default MaxLength 
        /// </summary>
        [DefaultValue(24)]
        public new Int32 MaxLength
        {
            get { return base.MaxLength; }
            set { base.MaxLength = value; }
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
                        string v = value.ToString().Trim();
                        v = v.Replace(System.Environment.NewLine, " ");

                        base.Text = v;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyTextBox's SelectedDataValue must be string", ex);
                    }
                }
            }
        }

        #endregion

        private string m_format;
        /// <summary>
        /// 
        /// </summary>
        public string Format
        {
            get { return m_format; }
            set
            {
                m_format = value;
                this.CharacterCasing = m_format == "Upper" ? System.Windows.Forms.CharacterCasing.Upper :
                    (m_format == "Lower" ? System.Windows.Forms.CharacterCasing.Lower : System.Windows.Forms.CharacterCasing.Normal);
            }
        }
    }
}