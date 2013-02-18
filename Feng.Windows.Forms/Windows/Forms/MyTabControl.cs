using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// StateTabControl
    /// </summary>
    public class MyTabControl : System.Windows.Forms.TabControl, IStateControl, IReadOnlyControl
    {
        #region "Constructor"

        /// <summary>
        /// Constructor
        /// </summary>
        public MyTabControl()
            : base()
        {
        }

        #endregion

        #region "Set Child BackColor"

        ///// <summary>
        ///// 设置内部控件BackColor使之与TabPage的BackColor一致
        ///// </summary>
        //public void SetTabPageBackColor()
        //{
        //    foreach (TabPage tabPage in base.TabPages)
        //    {
        //        tabPage.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //}

        #endregion

        #region "IStateControl"

        /// <summary>
        /// See<see cref="IReadOnlyControl.ReadOnly"/>
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

        /// <summary>
        /// See<see cref="IStateControl.SetState"/>
        /// </summary>
        public void SetState(StateType state)
        {
            StateControlHelper.SetState(this, state);
        }

        #endregion
    }
}