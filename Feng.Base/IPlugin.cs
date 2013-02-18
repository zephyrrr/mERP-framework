using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 插件
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Onload
        /// </summary>
        void OnLoad();

        /// <summary>
        /// OnUnload
        /// </summary>
        void OnUnload();
    }
}
