using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandManager
    {
        private static Dictionary<string, ICommand> m_cmdIds = new Dictionary<string, ICommand>();
        private static Dictionary<ICommand, ExecutedCommandHandler> m_cmdExecutes = new Dictionary<ICommand, ExecutedCommandHandler>();

        private static Dictionary<string, ExecutedCommandHandler> m_cmdIdExecutes = new Dictionary<string, ExecutedCommandHandler>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="handler"></param>
        public static void Register(string cmdId, ExecutedCommandHandler handler)
        {
            if (m_cmdIds.ContainsKey(cmdId))
            {
                m_cmdExecutes[m_cmdIds[cmdId]] = handler;
            }
            else
            {
                m_cmdIdExecutes[cmdId] = handler;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="handler"></param>
        public static void Register(ICommand cmd, ExecutedCommandHandler handler)
        {
            m_cmdIds[cmd.Name] = cmd;
            if (!m_cmdIdExecutes.ContainsKey(cmd.Name))
            {
                m_cmdExecutes[cmd] = handler;
            }
            else
            {
                m_cmdExecutes[cmd] = m_cmdIdExecutes[cmd.Name];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Execute(ICommand cmd, object sender, ExecutedEventArgs e)
        {
            if (m_cmdExecutes.ContainsKey(cmd))
            {
                m_cmdExecutes[cmd](sender, e);
            }
        }
    }
}
