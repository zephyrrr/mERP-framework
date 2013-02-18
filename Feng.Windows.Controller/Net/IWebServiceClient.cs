using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Net
{
    /// <summary>
    /// Search Client with WebService
    /// </summary>
    public interface IWebServiceClient
    {
        /// <summary>
        /// 读入数据
        /// </summary>
        /// <returns>符合条件的数据（根据具体查找方式，可能为IList或者DataView</returns>
        System.Collections.IEnumerable GetData(string searchExpression = null, string searchOrder = null, int? firstResult = null, int? maxResult = null);

        /// <summary>
        /// 读入数据条数
        /// </summary>
        /// <param name="searchExpression">查找条件</param>
        /// <returns>符合条件的数据条数</returns>
        int GetDataCount(string searchExpression = null);
    }
}
