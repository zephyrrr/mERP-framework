using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommnadBindingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static void BindingCommands()
        {
            IList<CommandBindingInfo> list = ADInfoBll.Instance.GetInfos<CommandBindingInfo>();
            foreach (var i in list)
            {
                CommandManager.Register(i.Name, new ExecutedCommandHandler(delegate(object sender, ExecutedEventArgs e)
                    {
                        ProcessInfoHelper.ExecuteProcess(i.Command.Name, new Dictionary<string, object> { { "sender", sender }, { "e", e } });
                    }));
            }
        }
    }
}
