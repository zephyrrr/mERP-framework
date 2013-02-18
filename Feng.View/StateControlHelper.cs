using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// ◊¥Ã¨øÿº˛∞Ô÷˙¿‡
    /// </summary>
    public sealed class StateControlHelper
    {
        private StateControlHelper()
        {
        }

        /// <summary>
        /// …Ë÷√◊¥Ã¨
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state"></param>
        public static void SetState(IReadOnlyControl control, StateType state)
        {
            SetState(control, state, true);
        }

        /// <summary>
        /// …Ë÷√◊¥Ã¨
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state"></param>
        /// <param name="readOnlyWhenView"></param>
        public static void SetState(IReadOnlyControl control, StateType state, bool readOnlyWhenView)
        {
            switch (state)
            {
                case StateType.None:
                    control.ReadOnly = readOnlyWhenView;
                    break;
                case StateType.View:
                    control.ReadOnly = readOnlyWhenView;
                    break;
                case StateType.Add:
                    control.ReadOnly = !readOnlyWhenView;
                    break;
                case StateType.Edit:
                    control.ReadOnly = !readOnlyWhenView;
                    break;
                default:
                    throw new NotSupportedException("Invalid State");
            }
        }

        /// <summary>
        /// …Ë÷√◊¥Ã¨
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state"></param>
        public static void SetState(IDataValueControl control, StateType state)
        {
            SetState(control, state, true);
        }

        /// <summary>
        /// …Ë÷√◊¥Ã¨
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state"></param>
        /// <param name="readOnlyWhenView"></param>
        private static void SetState(IDataValueControl control, StateType state, bool readOnlyWhenView)
        {
            switch (state)
            {
                case StateType.None:
                    control.SelectedDataValue = null;
                    control.ReadOnly = readOnlyWhenView;
                    break;
                case StateType.View:
                    control.ReadOnly = readOnlyWhenView;
                    break;
                case StateType.Add:
                    control.SelectedDataValue = null;
                    control.ReadOnly = !readOnlyWhenView;
                    break;
                case StateType.Edit:
                    control.ReadOnly = !readOnlyWhenView;
                    break;
                default:
                    throw new NotSupportedException("Invalid State of " + state);
            }
        }

        /// <summary>
        /// …Ë÷√◊¥Ã¨
        /// </summary>
        /// <param name="control"></param>
        /// <param name="state"></param>
        public static void SetState(IDataControl control, StateType state)
        {
            SetState(control as IDataValueControl, state);
        }
    }
}