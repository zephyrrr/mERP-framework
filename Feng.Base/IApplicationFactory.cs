using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IApplication CreateApplication();
    }
}
