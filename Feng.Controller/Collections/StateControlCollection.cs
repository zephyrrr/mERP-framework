using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Feng.Collections
{
	/// <summary>
	/// 状态控件集合
	/// </summary>
    public class StateControlCollection : ControlCollection<IStateControl, IControlManager>, IStateControlCollection
	{
        private StateType m_state = StateType.None;

		/// <summary>
		/// 设置状态
		/// </summary>
		/// <param name="state"></param>
		public void SetState(StateType state)
		{
            m_state = state;

			foreach (IStateControl sc in this)
			{
				sc.SetState(state);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public override void Add(IStateControl item)
        {
            item.SetState(m_state);

            base.Add(item);
        }
	}
}
