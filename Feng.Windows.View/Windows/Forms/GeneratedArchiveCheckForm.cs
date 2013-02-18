using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Forms;
using Feng.Windows.Utils;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    public class GeneratedArchiveCheckForm : ArchiveCheckForm
    {
        public GeneratedArchiveCheckForm(WindowInfo windowInfo)
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

            ISearchManager smMaster = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(tabInfos[0], null);
            IDisplayManager dmMaster;
            if (!string.IsNullOrEmpty(tabInfos[0].DisplayManagerClassName))
            {
                dmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(tabInfos[0], smMaster);
            }
            else
            {
                dmMaster = (ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(tabInfos[0], smMaster)).DisplayManager;
            }

            (base.MasterGrid as IBoundGrid).SetDisplayManager(dmMaster, tabInfos[0].GridName);

            ArchiveSearchForm searchForm = null;
            this.SetSearchPanel(() =>
            {
                if (searchForm == null)
                {
                    searchForm = new ArchiveSearchForm(this, smMaster, tabInfos[0]);
                }
                return searchForm;
            });

            // in CheckWindow, no page
            smMaster.EnablePage = false;

            GeneratedArchiveSeeForm.InitializeWindowProcess(windowInfo, this);
        }
    }
}
