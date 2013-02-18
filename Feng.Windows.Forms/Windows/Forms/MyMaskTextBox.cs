using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MyMaskTextBox
    /// </summary>
    public class MyMaskTextBox : /*Xceed.Editors.WinTextBox*/MaskedTextBox, IDataValueControl, IFormatControl
    {
        #region "Default Property"

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyMaskTextBox()
            : base()
        {
            base.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            base.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            base.Multiline = false;
            base.TextMaskFormat = MaskFormat.IncludeLiterals;
            base.Size = new System.Drawing.Size(120, 21);
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

        ///// <summary>
        ///// ReadOnly
        ///// </summary>
        //public bool ReadOnly
        //{
        //    get { return this.ReadOnly; }
        //    set
        //    {
        //        if (this.ReadOnly != value)
        //        {
        //            this.ReadOnly = value;
        //            if (ReadOnlyChanged != null)
        //            {
        //                ReadOnlyChanged(this, System.EventArgs.Empty);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public event EventHandler ReadOnlyChanged;
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
                if (!this.MaskCompleted)
                    return null;

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
                    this.Text = null;
                }
                else
                {
                    try
                    {
                        base.Text = value.ToString().Trim();
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("MyMaskTextBox's SelectedDataValue must be string", ex);
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
                this.Mask = m_format;
            }
        }
    }
}
