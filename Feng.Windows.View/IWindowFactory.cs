using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWindowFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowInfo"></param>
        /// <returns></returns>
        object CreateWindow(WindowInfo windowInfo);

        /// <summary>
        /// 
        /// </summary>
        event EventHandler WindowCreated;
    }
}
