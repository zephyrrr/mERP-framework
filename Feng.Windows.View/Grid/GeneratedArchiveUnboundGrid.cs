using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Forms;
using Feng.Windows.Utils;

namespace Feng.Grid
{
    public class GeneratedArchiveUnboundGrid : ArchiveUnboundGrid
    {
        public GeneratedArchiveUnboundGrid(WindowInfo windowInfo)
            : base()
        {
            this.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

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

            Initialize(tabInfos[0], null);
        }

        public GeneratedArchiveUnboundGrid(WindowTabInfo windowTabInfo)
            : base()
        {
            this.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

            this.Name = windowTabInfo.Name;
            this.Text = windowTabInfo.Text;

            Initialize(windowTabInfo, null);
        }
        internal GeneratedArchiveUnboundGrid(WindowTabInfo windowTabInfo, IControlManager cmParent)
            : base()
        {
            this.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

            this.Name = windowTabInfo.Name;
            this.Text = windowTabInfo.Text;

            Initialize(windowTabInfo, cmParent);
        }

        private void Initialize(WindowTabInfo windowTabInfo, IControlManager cmParent)
        {
            ISearchManager smMaster = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(windowTabInfo, cmParent == null ? null : cmParent.DisplayManager);
            IWindowControlManager cmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateControlManager(windowTabInfo, smMaster) as IWindowControlManager;

            if (cmParent != null && cmParent.Dao is IRelationalDao)
            {
                ManagerFactory.GenerateBusinessLayer(cmParent.Dao as IRelationalDao, windowTabInfo);
            }
            else
            {
                IBaseDao daoMaster = ServiceProvider.GetService<IManagerFactory>().GenerateBusinessLayer(windowTabInfo);
                cmMaster.Dao = daoMaster;
            }
            
            this.SetControlManager(cmMaster, windowTabInfo.GridName);

            //ArchiveSearchForm searchForm = new ArchiveSearchForm(this, smMaster, tabInfos[0]);
            //this.SearchPanel = searchForm;
            //cmMaster.StateControls.Add(searchForm);

            // daoParent's subDao is inserted in detailForm
            if (this is IBoundGridWithDetailGridLoadOnDemand)
            {
                ArchiveFormFactory.GenerateDetailGrids(this as IBoundGridWithDetailGridLoadOnDemand, windowTabInfo);
            }
        }
    }
}
