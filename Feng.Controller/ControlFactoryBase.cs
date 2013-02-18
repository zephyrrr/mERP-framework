using System;
using System.Collections.Generic;
using System.Text;
using Feng.Collections;

namespace Feng
{
    /// <summary>
    /// Base ControlFactory
    /// </summary>
    public class ControlCollectionFactoryBase : IControlCollectionFactory
    {
        /// <summary>
        /// 创建IDataControlCollection
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public virtual IDataControlCollection CreateDataControlCollection(IDisplayManager dm)
        {
            IDataControlCollection ret = new DataControlCollection();
            ret.ParentManager = dm;
            return ret;
        }

        /// <summary>
        /// 创建ISearchControlCollection
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public virtual ISearchControlCollection CreateSearchControlCollection(ISearchManager sm)
        {
            ISearchControlCollection ret = new SearchControlCollection();
            ret.ParentManager = sm;
            return ret;
        }

        /// <summary>
        /// 创建IBindingControlCollection
        /// </summary>
        /// <param name="dm"></param>
        /// <returns></returns>
        public virtual IBindingControlCollection CreateBindingControlCollection(IDisplayManager dm)
        {
            IBindingControlCollection ret = new BindingControlCollection();
            ret.ParentManager = dm;
            return ret;
        }

        /// <summary>
        /// 创建IStateControlCollection
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public virtual IStateControlCollection CreateStateControlCollection(IControlManager cm)
        {
            IStateControlCollection ret = new StateControlCollection();
            ret.ParentManager = cm;
            return ret;
        }

        /// <summary>
        /// 创建ICheckControlCollection
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public virtual ICheckControlCollection CreateCheckControlCollection(IControlManager cm)
        {
            ICheckControlCollection ret = new CheckControlCollection();
            ret.ParentManager = cm;
            return ret;
        }

        /// <summary>
        /// 创建IControlCheckExceptionProcess
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public virtual IControlCheckExceptionProcess CreateControlCheckExceptionProcess(IControlManager cm)
        {
            return null;
        }
    }
}
