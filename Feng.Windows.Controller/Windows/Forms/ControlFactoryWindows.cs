using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Collections;

namespace Feng.Windows.Forms
{
    public class ControlCollectionFactoryWindows : ControlCollectionFactoryBase
    {
        public override IDataControlCollection CreateDataControlCollection(IDisplayManager dm)
        {
            IDataControlCollection ret = new DataControlCollection();
            ret.ParentManager = dm;
            return ret;
        }

        public override ISearchControlCollection CreateSearchControlCollection(ISearchManager sm)
        {
            ISearchControlCollection ret = new SearchControlCollection();
            ret.ParentManager = sm;
            return ret;
        }

        public override IBindingControlCollection CreateBindingControlCollection(IDisplayManager dm)
        {
            if (dm is IDisplayManagerBindingSource)
            {
                BindingControlCollectionBindingSource ret = new BindingControlCollectionBindingSource();
                ret.ParentManager = dm;
                ret.BindingSource = (dm as IDisplayManagerBindingSource).BindingSource;
                return ret;
            }
            else
            {
                return base.CreateBindingControlCollection(dm);
            }
        }

        public override IControlCheckExceptionProcess CreateControlCheckExceptionProcess(IControlManager cm)
        {
            return new ErrorProviderControlCheckExceptionProcess();
        }
    }
}
