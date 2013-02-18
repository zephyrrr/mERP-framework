using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SearchManagerWithParent : AbstractSearchManager, ISearchManagerWithParent
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
        public SearchManagerWithParent(IControlManager cmParent)
            : this(cmParent.DisplayManager)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dmParent"></param>
        public SearchManagerWithParent(IDisplayManager dmParent)
            : base()
        {
            m_dmParent = dmParent;

            this.EnablePage = false;
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
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected override void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            System.Data.DataView dv = GetData(searchExpression, searchOrders) as System.Data.DataView;
            this.Result = dv;
            this.Count = dv.Count;
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
            System.Data.DataView dv = GetData(searchExpression, searchOrders, parentItem) as System.Data.DataView;
            this.Result = dv;
            this.Count = dv.Count;
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
