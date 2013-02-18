using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDataLoader
    {
        /// <summary>
        /// 名字
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 结果是否为唯一
        /// </summary>
        bool IsResultDistinct
        {
            get;
            set;
        }

        /// <summary>
        /// 根据查询条件查询数据
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders);

        /// <summary>
        /// 根据查询条件查询数据条数
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <returns></returns>
        int GetCount(ISearchExpression searchExpression);

        /// <summary>
        /// 提早读入
        /// </summary>
        IList<string> EagerFetchs
        {
            get;
        }

        /// <summary>
        /// 是否用分页读取
        /// </summary>
        bool EnablePage { get; set; }

        /// <summary>
        /// 查询结果首行
        /// </summary>
        int FirstResult { get; set; }

        /// <summary>
        /// 查询结果最大数量
        /// </summary>
        int MaxResult { get; set; }
    }

    /// <summary>
    /// 查询管理器
    /// </summary>
    public interface ISearchManager : IDataLoader, IDisposable
    {
        /// <summary>
        /// 显示管理器
        /// </summary>
        IDisplayManager DisplayManager
        {
            get;
            set;
        }

        /// <summary>
        /// 根据给定查询条件搜索，并填充到DisplayManager中
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        void LoadData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders);

        /// <summary>
        /// 停止读取数据。如果未开始读取，返回false。
        /// </summary>
        bool StopLoadData();

        /// <summary>
        /// 等待直到查找线程返回。如果未开始读取，返回false。
        /// </summary>
        bool WaitLoadData();

        /// <summary>
        /// 重新读取数据（查询条件不变）
        /// 如果原来无查询条件，则调用<see cref="LoadData"/>
        /// </summary>
        void ReloadData();

        /// <summary>
        /// 重新读入某一行
        /// </summary>
        void ReloadItem(int idx);

        /// <summary>
        /// 查询结果
        /// 可为IList[IEntity] Or DataView, DataTable, DataSet Or 
        /// </summary>
        System.Collections.IEnumerable Result { get; set; }

        /// <summary>
        /// 查询结果数量
        /// </summary>
        int Count { get; set; }


        /// <summary>
        /// 
        /// </summary>
        bool UseStreamLoad { get; set; }
        

        /// <summary>
        /// 根据序号得到查询历史
        /// </summary>
        /// <param name="idx"></param>
        /// <return></return>
        SearchHistoryInfo GetHistory(int idx);

        /// <summary>
        /// 设置查询历史
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        SearchHistoryInfo SetHistory(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders);

        /// <summary>
        /// 数据查询前发生
        /// </summary>
        event EventHandler<DataLoadingEventArgs> DataLoading;

        /// <summary>
        /// 数据查询后发生
        /// </summary>
        event EventHandler<DataLoadedEventArgs> DataLoaded;

        /// <summary>
        /// 引发<see cref="DataLoading"/>事件
        /// </summary>
        void OnDataLoading(DataLoadingEventArgs e);

        /// <summary>
        /// 引发<see cref="DataLoaded"/>事件
        /// </summary>
        void OnDataLoaded(DataLoadedEventArgs e);

        /// <summary>
        /// 读入Entity Schema，使Grid不读入数据时就能显示框架
        /// </summary>
        IEnumerable GetSchema();

        /// <summary>
        /// AdditionalSearchExpression
        /// </summary>
        string AdditionalSearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// AdditionalSearchOrder
        /// </summary>
        string AdditionalSearchOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        object Clone();

        /// <summary>
        /// 查询控件集合
        /// </summary>
        ISearchControlCollection SearchControls { get; }

        /// <summary>
        /// 自动生成查询条件，并根据查询条件搜索，并填充到DisplayManager中
        /// 结果填充到<see cref="Result"/>和<see cref="Count"/>
        /// </summary>
        void LoadDataAccordSearchControls();
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="count"></param>
        public DataLoadedEventArgs(object dataSource, int count)
        {
            this.DataSource = dataSource;
            this.Count = count;
        }

        /// <summary>
        /// 
        /// </summary>
        public object DataSource
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataLoadingEventArgs : System.ComponentModel.CancelEventArgs
    {
         /// <summary>
        /// 
        /// </summary>
        /// <param name="se"></param>
        /// <param name="so"></param>
        public DataLoadingEventArgs(ISearchExpression se, IList<ISearchOrder> so)
        {
            this.SearchExpression = se;
            this.SearchOrders = so;
        }

        /// <summary>
        /// 
        /// </summary>
        public ISearchExpression SearchExpression
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<ISearchOrder> SearchOrders
        {
            get;
            set;
        }
    }

}
