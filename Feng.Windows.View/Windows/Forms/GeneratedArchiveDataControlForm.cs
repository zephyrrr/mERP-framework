using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Utils;

namespace Feng.Windows.Forms
{
    public class GeneratedArchiveDataControlForm : ArchiveDataControlForm
    {
        public GeneratedArchiveDataControlForm(WindowInfo windowInfo)
            : base()
        {
            this.Name = windowInfo.Name;
            this.Text = windowInfo.Text;

            IList<WindowTabInfo> tabInfos = ADInfoBll.Instance.GetWindowTabInfosByWindowId(windowInfo.Name);
            if (tabInfos == null)
            {
                throw new ArgumentException("there is no windowTab with windowId of " + windowInfo.Name);
            }
            if (tabInfos.Count == 0)
            {
                throw new ArgumentException("There should be at least one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }
            if (tabInfos.Count > 1)
            {
                throw new ArgumentException("There should be at most one TabInfo in WindowInfo with name is " + windowInfo.Name + "!");
            }

            IControlManager cmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(tabInfos[0], null);

            base.Initialize(cmMaster, ADInfoBll.Instance.GetGridColumnInfos(tabInfos[0].GridName));
        }
    }
}
