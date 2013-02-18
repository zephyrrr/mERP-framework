using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDisplayManagerContainer
    {
        /// <summary>
        /// 
        /// </summary>
        IDisplayManager DisplayManager
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IControlManagerContainer
    {
        /// <summary>
        /// 
        /// </summary>
        IControlManager ControlManager
        {
            get;
        }
    }
}
