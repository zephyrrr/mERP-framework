using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Feng.Collections;
using Feng.Async;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class SearchManagerDefaultValue
    {
        /// <summary>
        /// 
        /// </summary>
        public static int MaxResult = 25;
    }

    /// <summary>
    /// 查询管理器
    /// </summary>
    public abstract class AbstractSearchManager : ISearchManager
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DataLoaded = null;
                this.DataLoading = null;

                if (m_searchControls != null)
                {
                    foreach (ISearchControl sc in m_searchControls)
                    {
                        IDisposable i = sc as IDisposable;
                        if (i != null)
                        {
                            i.Dispose();
                        }
                    }
                    m_searchControls.Clear();
                }

                this.m_dm = null;

                //if (m_loadingThread != null)
                //{
                //    m_loadingThread = null;
                //}
                DisposeAsyncManager();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="smName"></param>
        protected AbstractSearchManager(string smName)
        {
            this.Name = smName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AbstractSearchManager()
        {
            this.Name = this.GetType().Name;

            m_searchControls = CreateSearchControlCollection();
        }

        /// <summary>
        /// CreateSearchControlCollection
        /// </summary>
        /// <returns></returns>
        protected virtual ISearchControlCollection CreateSearchControlCollection()
        {
            var ccf = ServiceProvider.GetService<IControlCollectionFactory>();
            if (ccf != null)
            {
                return ccf.CreateSearchControlCollection(this);
            }
            else
            {
                return SearchControlCollection.Empty;
            }
        }

        private ISearchControlCollection m_searchControls;
        /// <summary>
        /// 查询控件
        /// </summary>
        public ISearchControlCollection SearchControls
        {
            get { return m_searchControls; }
        }

        /// <summary>
        /// 根据查询控件内容填充查询条件
        /// </summary>
        private void FillSearchConditions(out IList<ISearchExpression> searchExpressions, out IList<ISearchOrder> searchOrders)
        {
            searchExpressions = new List<ISearchExpression>();
            searchOrders = new List<ISearchOrder>();

            foreach (ISearchControl fc in SearchControls)
            {
                fc.FillSearchConditions(searchExpressions, searchOrders);
            }
        }

        
        /// <summary>
        ///
        /// </summary>
        public string AdditionalSearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string AdditionalSearchOrder
        {
            get;
            set;
        }

        private IDisplayManager m_dm;
        /// <summary>
        /// 显示管理器
        /// </summary>
        public IDisplayManager DisplayManager
        {
            get { return m_dm; }
            set { m_dm = value; }
        }

        private bool m_enablePage = true;
        /// <summary>
        /// 是否使用分页
        /// </summary>
        public bool EnablePage
        {
            get { return m_enablePage; }
            set { m_enablePage = value; }
        }

        private int m_maxResult = SearchManagerDefaultValue.MaxResult;
        /// <summary>
        /// 查询结果最大数量
        /// </summary>
        public int MaxResult
        {
            get { return m_maxResult; }
            set { m_maxResult = value; }
        }

        private int m_firstResult;
        /// <summary>
        /// 查询结果首行
        /// </summary>
        public int FirstResult
        {
            get { return m_firstResult; }
            set { m_firstResult = value; }
        }


        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 结果是否为唯一
        /// </summary>
        public bool IsResultDistinct
        {
            get;
            set;
        }

        private IList<string> m_eagerFetchs = new List<string>();
        /// <summary>
        /// EagerFetchs
        /// </summary>
        public IList<string> EagerFetchs
        {
            get { return m_eagerFetchs; }
        }

        /// <summary>
        /// 查询结果数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public System.Collections.IEnumerable Result { get; set; }

        /// <summary>
        /// 数据查询前发生
        /// </summary>
        public event EventHandler<DataLoadingEventArgs> DataLoading;

        /// <summary>
        /// 数据查询后发生
        /// </summary>
        public event EventHandler<DataLoadedEventArgs> DataLoaded;

        /// <summary>
        /// 引发<see cref="DataLoading"/>事件
        /// </summary>
        public void OnDataLoading(DataLoadingEventArgs e)
        {
            if (DataLoading != null)
            {
                DataLoading(this, e);
            }
        }

        /// <summary>
        /// 引发<see cref="DataLoaded"/>事件
        /// </summary>
        public void OnDataLoaded(DataLoadedEventArgs e)
        {
            if (DataLoaded != null)
            {
                DataLoaded(this, e);
            }
        }

        /// <summary>
        /// 根据查询条件查询数据(查询结果和条数，结果保存在<see cref="Result"/>，条数保存在<see cref="Count"/>
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        protected abstract void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="func"></param>
        protected virtual void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, Action<object> func) 
        {
            throw new NotSupportedException("GetDataCount is not supported in base class.");
        }

        private bool m_useStreamLoad = false;
        /// <summary>
        /// 是否采用流读取
        /// </summary>
        public bool UseStreamLoad
        {
            get { return m_useStreamLoad; }
            set { m_useStreamLoad = value; }
        }

        /// <summary>
        /// 根据查询条件查询数据，只返回查询结果，无条数。如需要查询条数，调用<see cref="GetCount"/>
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public abstract System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders);

        /// <summary>
        /// 根据查询条件返回符合条件的条数
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        public abstract int GetCount(ISearchExpression searchExpression);

        private AsyncManager<object, DataLoadedEventArgs> m_asyncManager;
        private void DisposeAsyncManager()
        {
            if (m_asyncManager != null)
            {
                m_asyncManager.WorkerDone -= new EventHandler<WorkerDoneEventArgs<DataLoadedEventArgs>>(LoadWork_WorkerDone);
                m_asyncManager = null;
            }
        }
        private void InitAsyncManager(DataLoadingEventArgs e)
        {
            m_asyncManager = new AsyncManager<object, DataLoadedEventArgs>(this.Name, new DataLoadWorker(this, e));
            m_asyncManager.WorkerDone += new EventHandler<WorkerDoneEventArgs<DataLoadedEventArgs>>(LoadWork_WorkerDone);
            m_asyncManager.WorkerProgress += new EventHandler<WorkerProgressEventArgs<object>>(m_asyncManager_WorkerProgress);
        }

        void m_asyncManager_WorkerProgress(object sender, WorkerProgressEventArgs<object> e)
        {
            object entity = e.ProgressData;
            if (this.DisplayManager != null && entity != null)
            {
                this.DisplayManager.AddDataItem(entity);
            }
        }

        /// <summary>
        /// Occur when Load Thread is done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void LoadWork_WorkerDone(object sender, WorkerDoneEventArgs<DataLoadedEventArgs> e)
        {
            // 当中途停止，e.Interrupted = true, result = null;
            DataLoadedEventArgs e2 = e.Result;

            try
            {
                if (this.DisplayManager != null && e2 != null && e2.DataSource != null)
                {
                    this.DisplayManager.SetDataBinding(e2.DataSource, string.Empty);
                }
            }
            finally
            {
                OnDataLoaded(e2);
            }

            if (e.Exception != null)
            {
                ExceptionProcess.ProcessWithNotify(e.Exception);
            }
        }

        /// <summary>
        /// 自动填充查找条件然后查找
        /// </summary>
        public void LoadDataAccordSearchControls()
        {
            IList<ISearchExpression> searchExpressions;
            IList<ISearchOrder> searchOrders;
            FillSearchConditions(out searchExpressions, out searchOrders);
            LoadData(SearchExpression.ToSingle(searchExpressions), searchOrders);
        }

        /// <summary>
        /// 根据查询条件查询数据，并设置查询历史，引发<see cref="DataLoading"/>和<see cref="DataLoaded"/>事件
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        public void LoadData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            lock (m_syncObject)
            {
                //if (m_loadingThread != null && m_loadingThread.IsAlive)
                //{
                //    m_loadingThread.Abort();
                //    m_loadingThread.Join();
                //}

                //if (m_isDataLoding)
                //{
                //    throw new InvalidUserOperationException("请先停止搜索才能重新开始搜索！");
                //}

                // Reload也调用此函数，不能在这里设置
                //this.FirstResult = 0;


                if (m_asyncManager != null && m_asyncManager.IsWorkerBusy)
                {
                    return;
                }

                SetHistory(searchExpression, searchOrders);

                this.FillSearchAdditionals(ref searchExpression, ref searchOrders);

                DisposeAsyncManager();

                DataLoadingEventArgs e = new DataLoadingEventArgs(searchExpression, searchOrders);
                OnDataLoading(e);

                if (e.Cancel)
                {
                    return;
                }

                //if (m_loadingThread != null && m_loadingThread.IsAlive)
                //{
                //    m_loadingThread.Abort();
                //}

                //// 线程运行了就不能重新启动，要重新建一个
                //m_loadingThread = new Thread(new ParameterizedThreadStart(ThreadLoadData));
                //m_loadingThread.IsBackground = true;

                //if (asyncOperation == null)
                //{
                //    this.asyncOperation = System.ComponentModel.AsyncOperationManager.CreateOperation(null);
                //    this.operationCompleted = new SendOrPostCallback(this.AsyncOperationCompleted);
                //}

                //m_isDataLoding = true;

                //m_loadingThread.Start(e);

                InitAsyncManager(e);

                m_asyncManager.StartWorker(SystemConfiguration.UseMultiThread);
            }
        }

        //private bool m_isDataLoding;
        private object m_syncObject = new object();

        /// <summary>
        /// 停止读取数据
        /// </summary>
        public bool StopLoadData()
        {
            //if (m_loadingThread != null && m_loadingThread.IsAlive)
            //{
            //    m_loadingThread.Abort();
            //    m_loadingThread.Join();
            //}
            if (m_asyncManager != null)
            {
                WorkerDoneEventArgs<DataLoadedEventArgs> e = m_asyncManager.AbortWorker();
                LoadWork_WorkerDone(m_asyncManager, e);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 等待直到查找线程返回
        /// </summary>
        public bool WaitLoadData()
        {
            //if (m_loadingThread != null && m_loadingThread.IsAlive)
            //{
            //    m_loadingThread.Join();
            //}
            if (m_asyncManager != null)
            {
                WorkerDoneEventArgs<DataLoadedEventArgs> e = m_asyncManager.WaitForWorker();
                LoadWork_WorkerDone(m_asyncManager, e);

                return true;
            }
            return false;
        }

        private static SearchHistoryInfo m_emptySearchHistory = new SearchHistoryInfo();
        /// <summary>
        /// 根据序号得到查询历史(0为最新的, 递增变老)
        /// </summary>
        /// <param name="idx"></param>
        public SearchHistoryInfo GetHistory(int idx)
        {
            if (idx < 0)
            {
                idx += historyCnt;
            }
            if (idx < 0 || idx >= historyCnt)
            {
                return m_emptySearchHistory;
            }

            if (m_searchHistoryInfos[idx] == null)
            {
                return m_emptySearchHistory;
            }
            else
            {
                return m_searchHistoryInfos[idx];
            }
        }

        private SearchHistoryInfo[] m_searchHistoryInfos = new SearchHistoryInfo[10];
        private const int historyCnt = 10;
        //private string[] historySearchExpressions = new string[historyCnt];
        //private string[] historySearchOrders = new string[historyCnt];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        public SearchHistoryInfo SetHistory(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            return SetHistory(searchExpression, searchOrders, true);
        }

        /// <summary>
        /// SetHistory
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="isCurrent"></param>
        private SearchHistoryInfo SetHistory(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, bool isCurrent)
        {
            string nowExpression = SearchExpression.ToString(searchExpression);

            int existIdx = -1;

            // 发现有重复的也一样添加，因为翻页用到历史里的最近一条(只和最近一条比较）
            for (int i = 0; i < 1; ++i)
            {
                if (m_searchHistoryInfos[i] != null 
                    && m_searchHistoryInfos[i].Expression == nowExpression)
                {
                    existIdx = i;
                    break;
                }
            }

            if (existIdx == -1)
            {
                //historySearchExpressions[historyNow] = SearchExpression.ToString(searchExpressions);
                //historySearchOrders[historyNow] = SearchOrder.ToString(searchOrders);
                //historyNow = (historyNow + 1) % historyCnt;
                for (int i = historyCnt - 1; i > 0; --i)
                {
                    m_searchHistoryInfos[i] = m_searchHistoryInfos[i - 1];
                }
                m_searchHistoryInfos[0] = new SearchHistoryInfo(SearchExpression.ToString(searchExpression),
                    SearchOrder.ToString(searchOrders), isCurrent);

                return m_searchHistoryInfos[0];
            }
            else
            {
                m_searchHistoryInfos[existIdx].IsCurrentSession = isCurrent;
                return m_searchHistoryInfos[existIdx];
            }
        }

        //private bool m_needSearchExpression = true;
        ///// <summary>
        ///// 是否需要查找条件（如需要，刷新的时候必须要现有条件）
        ///// </summary>
        //protected bool NeedSearchConddition
        //{
        //    get { return m_needSearchExpression; }
        //    set { m_needSearchExpression = value; }
        //}

        /// <summary>
        /// 重新读入当前行
        /// </summary>
        public void ReloadItem(int idx)
        {
            //if (this.DisplayManager.CurrentItem == null)
            //    return;
            if (this.DisplayManager.EntityInfo == null)
                return;
            if (idx < 0 || idx >= this.DisplayManager.Count)
            {
                return;
            }
            object entity = this.DisplayManager.Items[idx];
            object id = EntityScript.GetPropertyValue(entity, this.DisplayManager.EntityInfo.IdName);

            IList list = this.GetData(SearchExpression.Eq(this.DisplayManager.EntityInfo.IdName, id), null) as IList;
            System.Diagnostics.Debug.Assert(list.Count <= 1);

            if (list.Count == 0)
            {
                return;
            }
            else if (list.Count == 1)
            {
                this.DisplayManager.Items[idx] = list[0];
            }
        }

        /// <summary>
        /// 根据上一次查询条件重新读入数据
        /// 第一条记录设置不变
        /// </summary>
        public virtual void ReloadData()
        {
            //// 还没查找过，不能刷新
            //if (m_needFindConddition && !m_findConditionLoaded)
            //    return;

            //if (m_needSearchExpression)
            //{
                SearchHistoryInfo his = GetHistory(0);

                // his.Expression = string.Empty 代表查询条件为空，当时已经查询过
                if (his != null && his.IsCurrentSession && his.Expression != null)
                {
                    LoadData(SearchExpression.Parse(his.Expression), SearchOrder.Parse(his.Order));
                }
                // 不过无查询条件，不刷新
                //else
                //{
                //    LoadData();
                //}
            //}
            //else
            //{
            //    LoadData(new List<ISearchExpression>(), null, this.FirstResult);
            //    return true;
            //}
        }

        /// <summary>
        /// 读入Entity Schema，使Grid不读入数据时就能显示框架
        /// </summary>
        public virtual IEnumerable GetSchema()
        {
            int saveMaxResult = this.MaxResult;
            this.MaxResult = 0;
            this.FirstResult = 0;
            IEnumerable ret = GetData(SearchExpression.False(), null);
            this.MaxResult = saveMaxResult;
            return ret;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        protected void Copy(AbstractSearchManager src, AbstractSearchManager dest)
        {
            dest.EnablePage = src.EnablePage;
            dest.FirstResult = src.FirstResult;
            dest.IsResultDistinct = src.IsResultDistinct;
            dest.MaxResult = src.MaxResult;
            dest.Name = src.Name;
            dest.AdditionalSearchExpression = src.AdditionalSearchExpression;
            dest.AdditionalSearchOrder = src.AdditionalSearchOrder;
            dest.m_searchHistoryInfos = src.m_searchHistoryInfos;

            dest.EagerFetchs.Clear();
            foreach (string s in src.EagerFetchs)
            {
                dest.EagerFetchs.Add(s);
            }

            dest.SearchControls.Clear();
            dest.SearchControls.AddRange(src.SearchControls);
        }

        private class DataLoadWorker : WorkerBase<DataLoadingEventArgs, object, DataLoadedEventArgs>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="sm"></param>
            /// <param name="inputParams"></param>
            public DataLoadWorker(AbstractSearchManager sm, DataLoadingEventArgs inputParams)
                : base(inputParams)
            {
                m_sm = sm;
            }

            private AbstractSearchManager m_sm;
            protected override DataLoadedEventArgs DoWork(DataLoadingEventArgs inputParams)
            {
                //bool threadAborted = false;
                //try
                //{
                if (m_sm.UseStreamLoad)
                {
                    m_sm.GetDataCount(inputParams.SearchExpression, inputParams.SearchOrders, new Action<object>(delegate(object entity)
                        {
                            this.WorkerProgressSignal(entity);
                        }));
                    return new DataLoadedEventArgs(null, -1);
                }
                else
                {
                    m_sm.GetDataCount(inputParams.SearchExpression, inputParams.SearchOrders);
                    return new DataLoadedEventArgs(m_sm.Result, m_sm.Count);
                }

                //}
                //catch (ThreadAbortException)
                //{
                //    //threadAborted = true;
                //}
                //catch (Exception ex)
                //{
                    //if (ex.InnerException is ThreadAbortException)
                    //{
                    //    threadAborted = true;
                    //}
                    //else
                    //{
                    //    ExceptionProcess.ProcessWithNotify(ex);
                    //}
                //}
                //finally
                //{
                    //if (!threadAborted)
                    //{
                    //    this.asyncOperation.PostOperationCompleted(this.operationCompleted, new DataLoadedEventArgs(dataSource));
                    //}
                    //this.asyncOperation = null;

                    //m_loadingThread = null;

                    //m_isDataLoding = false;
                //}
                
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class SearchManagerExtention
    {
        /// <summary>
        /// 无条件查询
        /// </summary>
        /// <param name="sm"></param>
        public static void LoadData(this ISearchManager sm)
        {
            sm.LoadData(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sm"></param>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        public static void FillSearchAdditionals(this ISearchManager sm, ref ISearchExpression searchExpression, ref IList<ISearchOrder> searchOrders)
        {
            if (!string.IsNullOrEmpty(sm.AdditionalSearchExpression))
            {
                searchExpression = SearchExpression.And(searchExpression,
                    SearchExpression.Parse(sm.AdditionalSearchExpression));
            }
            if (!string.IsNullOrEmpty(sm.AdditionalSearchOrder))
            {
                if (searchOrders == null)
                {
                    searchOrders = new List<ISearchOrder>();
                }
                foreach (var i in SearchOrder.Parse(sm.AdditionalSearchOrder))
                {
                    searchOrders.Add(i);
                }
            }
        }
    }
}