using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManagerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <returns></returns>
        IBaseDao GenerateBusinessLayer(WindowTabInfo tabInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        IControlManager GenerateControlManager(WindowTabInfo tabInfo, ISearchManager sm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
        IDisplayManager GenerateDisplayManager(WindowTabInfo tabInfo, ISearchManager sm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchManagerClassName"></param>
        /// <param name="searchManagerClassParams"></param>
        /// <returns></returns>
        ISearchManager GenerateSearchManager(string searchManagerClassName, string searchManagerClassParams = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabInfo"></param>
        /// <param name="dmParent"></param>
        /// <returns></returns>
        ISearchManager GenerateSearchManager(WindowTabInfo tabInfo, IDisplayManager dmParent);

    }
}
