using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchHistoryInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SearchHistoryInfo()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="order"></param>
        /// <param name="isCurrent"></param>
        public SearchHistoryInfo(string expression, string order, bool isCurrent)
        {
            this.Expression = expression;
            this.Order = order;
            this.IsCurrentSession = isCurrent;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Expression
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Order
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是当前程序的还是保存的上次程序的
        /// </summary>
        public bool IsCurrentSession
        {
            get;
            set;
        }
    }
}
