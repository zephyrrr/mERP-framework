using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class StateControl : IStateControl
    {
        private Control m_control;
        private StateType m_enableStates;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enableWhenView"></param>
        public StateControl(Control control, bool enableWhenView)
            : this(control, enableWhenView ? (StateType.None | StateType.View) : (StateType.Add | StateType.Edit))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="enableState"></param>
        public StateControl(Control control, StateType enableState)
        {
            m_control = control;
            m_enableStates = enableState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void SetState(StateType state)
        {
            m_control.Enabled = (m_enableStates & state) != 0;
        }
    }
}
