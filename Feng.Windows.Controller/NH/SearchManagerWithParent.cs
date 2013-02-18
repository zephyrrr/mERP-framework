using System;
using System.Collections.Generic;
using System.Text;
using Feng.Async;

namespace Feng.NH
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SearchManagerWithParent<T> : SearchManagerProxy<T>, ISearchManagerWithParent
        where T : IEntity
    {
        private IDisplayManager m_dmParent;

        /// <summary>
        /// ParentDisplayManager
        /// </summary>
        public IDisplayManager ParentDisplayManager
        {
            get { return m_dmParent; }
            set { m_dmParent = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmParent"></param>
        /// <param name="repCfgName"></param>
        public SearchManagerWithParent(IControlManager cmParent, string repCfgName)
            : this(cmParent.DisplayManager, repCfgName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dmParent"></param>
        /// <param name="repCfgName"></param>
        public SearchManagerWithParent(IDisplayManager dmParent, string repCfgName)
            : base(repCfgName)
        {
            m_dmParent = dmParent;
        }

        //private bool m_laodParentFirst = false;
        ///// <summary>
        ///// 读入子项目时，是否先重新读入父项目
        ///// </summary>
        //public bool LoadParentFirst
        //{
        //    get { return m_laodParentFirst; }
        //    set { m_laodParentFirst = value; }
        //}


        /// <summary>
        /// 是否需要重新读入（如果不重新读入，会利用缓存中数据）
        /// </summary>
        private bool m_isReload;

        /// <summary>
        /// 
        /// </summary>
        protected bool IsReload
        {
            get { return m_isReload; }
        }

        /// <summary>
        /// 重新读入数据
        /// </summary>
        public override void ReloadData()
        {
            // 需要设置NeedReload，但因为Load是异步的，不能直接设置NeedReload为false，需要在Load结束时设置
            m_isReload = true;

            LoadDataAccordSearchControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void LoadWork_WorkerDone(object sender, WorkerDoneEventArgs<DataLoadedEventArgs> e)
        {
            m_isReload = false;

            base.LoadWork_WorkerDone(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            System.Collections.IList dv = GetData(searchExpression, searchOrders) as System.Collections.IList;
            this.Result = dv;
            if (dv != null)
            {
                this.Count = dv.Count;
            }
            else
            {
                this.Count = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public override int GetCount(ISearchExpression searchExpression)
        {
            return this.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        public void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem)
        {
            System.Collections.IList dv = GetData(searchExpression, searchOrders, parentItem) as System.Collections.IList;
            this.Result = dv;
            if (dv != null)
            {
                this.Count = dv.Count;
            }
            else
            {
                this.Count = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        public abstract System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem);

        /// <summary>
        /// 根据查询条件查询数据条数
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        public int GetCount(ISearchExpression searchExpression, object parentItem)
        {
            return this.Count;
        }

        /// <summary>
        /// CreateSearchControlCollection
        /// </summary>
        /// <returns></returns>
        protected override ISearchControlCollection CreateSearchControlCollection()
        {
            return Feng.Collections.SearchControlCollection.Empty;
        }
    }
}
