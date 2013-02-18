namespace Feng.Windows.Forms
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// MyButton
    /// </summary>
    public class MyButton : System.Windows.Forms.Button, IStateControl, IReadOnlyControl, IButton
    {
        #region "Default Property"

        /// <summary>
        /// 初始化默认属性
        /// </summary>
        public MyButton() : base()
        {
            base.FlatStyle = System.Windows.Forms.FlatStyle.System;
            base.Size = new System.Drawing.Size(72, 21);
        }

        /// <summary>
        /// Default FlatStyle = FlatStyle.System
        /// </summary>
        [DefaultValue(System.Windows.Forms.FlatStyle.System)]
        public new System.Windows.Forms.FlatStyle FlatStyle
        {
            get { return base.FlatStyle; }
            set { base.FlatStyle = value; }
        }

        #endregion

        #region "IStateControl"

        /// <summary>
        /// See<see cref="IReadOnlyControl.ReadOnly"/>
        /// </summary>
        [DefaultValue(false)]
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

        private bool m_readOnlyWhenView = true;

        /// <summary>
        /// State==StatusType.View时，Button Enable or Unable
        /// 默认为View时Unable(ActionButtons)
        /// 当需要在Edit(Add)时候Enable，则是因为需要选择某些数据项
        /// </summary>
        [Description("设置是否在View状态ReadOnly")]
        [DefaultValue(true)]
        public bool ReadOnlyWhenView
        {
            get { return m_readOnlyWhenView; }
            set { m_readOnlyWhenView = value; }
        }

        /// <summary>
        /// See<see cref="IStateControl.SetState"/>
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state, m_readOnlyWhenView);
        }

        #endregion
    }
}