using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 控件Factory
    /// </summary>
    public interface IControlCollectionFactory
    {
        /// <summary>
        /// 创建IDataControlCollection
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        IDataControlCollection CreateDataControlCollection(IDisplayManager dm);

        /// <summary>
        /// 创建ISearchControlCollection
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        ISearchControlCollection CreateSearchControlCollection(ISearchManager sm);

        /// <summary>
        /// 创建IBindingControlCollection
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        IBindingControlCollection CreateBindingControlCollection(IDisplayManager dm);

        /// <summary>
        /// 创建IStateControlCollection
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        IStateControlCollection CreateStateControlCollection(IControlManager cm);

        /// <summary>
        /// 创建ICheckControlCollection
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        ICheckControlCollection CreateCheckControlCollection(IControlManager cm);

        /// <summary>
        /// 创建IControlCheckExceptionProcess
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        IControlCheckExceptionProcess CreateControlCheckExceptionProcess(IControlManager cm);
    }
}
