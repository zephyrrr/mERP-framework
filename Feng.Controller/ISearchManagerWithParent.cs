using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// interface for SearchManagerProxyWithParent
    /// </summary>
    public interface ISearchManagerWithParent : ISearchManager
    {
        /// <summary>
        /// 
        /// </summary>
        IDisplayManager ParentDisplayManager
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        void GetDataCount(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem);

        /// <summary>
        /// 根据查询条件查询数据条数
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        int GetCount(ISearchExpression searchExpression, object parentItem);
    }
}
