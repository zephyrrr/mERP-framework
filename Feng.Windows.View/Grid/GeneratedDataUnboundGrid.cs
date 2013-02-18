using System;
using System.Collections.Generic;
using System.Text;
using Feng.Windows.Forms;
using Feng.Windows.Utils;

namespace Feng.Grid
{
    public class GeneratedDataUnboundGrid : DataUnboundGrid
    {
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //    }
        //    base.Dispose(disposing);
        //}


        public GeneratedDataUnboundGrid(WindowInfo windowInfo)
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

        public GeneratedDataUnboundGrid(WindowTabInfo windowTabInfo)
            : base()
        {
            this.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

            this.Name = windowTabInfo.Name;
            this.Text = windowTabInfo.Text;

            Initialize(windowTabInfo, null);
        }

        internal GeneratedDataUnboundGrid(WindowTabInfo windowTabInfo, IDisplayManager dmParent)
            : base()
        {
            this.FixedHeaderRows.Add(new Xceed.Grid.ColumnManagerRow());

            this.Name = windowTabInfo.Name;
            this.Text = windowTabInfo.Text;

            Initialize(windowTabInfo, dmParent);
        }

        private void Initialize(WindowTabInfo windowTabInfo, IDisplayManager dmParent)
        {
            ISearchManager smMaster = ServiceProvider.GetService<IManagerFactory>().GenerateSearchManager(windowTabInfo, dmParent);

            IDisplayManager dmMaster = ServiceProvider.GetService<IManagerFactory>().GenerateDisplayManager(windowTabInfo, smMaster);

            this.SetDisplayManager(dmMaster, windowTabInfo.GridName);
            
            //ArchiveSearchForm searchForm = new ArchiveSearchForm(this, smMaster, tabInfos[0]);
            //this.SearchPanel = searchForm;

            if (this is IBoundGridWithDetailGridLoadOnDemand)
            {
                ArchiveFormFactory.GenerateDetailGrids((IBoundGridWithDetailGridLoadOnDemand)this, windowTabInfo);
            }
        }
    }
}
