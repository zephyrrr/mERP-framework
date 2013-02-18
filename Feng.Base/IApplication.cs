using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// 按照ActionInfo做某一动作
        /// </summary>
        /// <param name="actionName"></param>
        object ExecuteAction(string actionName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        void ExecuteMenu(string menuName);
    }
}
