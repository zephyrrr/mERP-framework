using System;
using System.Collections.Generic;
using System.Text;
using Feng.Search;

namespace Feng
{
    /// <summary>
    /// SearchOrder
    /// </summary>
    public sealed class SearchOrder
    {
        private SearchOrder()
        {
        }

        /// <summary>
        /// 生成升序查询顺序
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ISearchOrder Asc(string propertyName)
        {
            return new Search.Order(propertyName, true);
        }

        /// <summary>
        /// 生成降序查询顺序
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ISearchOrder Desc(string propertyName)
        {
            return new Search.Order(propertyName, false);
        }

        /// <summary>
        /// 根据查询顺序列表得到可读字符串
        /// </summary>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public static string ToString(IList<ISearchOrder> searchOrders)
        {
            if (searchOrders != null)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < searchOrders.Count; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(searchOrders[i].ToString());
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据可读字符串得到查询顺序
        /// 例如 Title, Name Desc, Help Asc. Asc 默认
        /// </summary>
        /// <param name="searchOrders"></param>
        /// <returns></returns>
        public static IList<ISearchOrder> Parse(string searchOrders)
        {
            if (string.IsNullOrEmpty(searchOrders))
                return null;
            IList<ISearchOrder> ret = new List<ISearchOrder>();
            string[] ss = searchOrders.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in ss)
            {
                string[] s2 = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (s2.Length == 1)
                {
                    ret.Add(SearchOrder.Asc(s2[0].Trim()));
                }
                else if (s2.Length == 2)
                {
                    if (s2[1].ToUpper() == "DESC")
                    {
                        ret.Add(SearchOrder.Desc(s2[0].Trim()));
                    }
                    else if (s2[1].ToUpper() == "ASC")
                    {
                        ret.Add(SearchOrder.Asc(s2[0].Trim()));
                    }
                    else
                    {
                        throw new ArgumentException("SearchOrder of " + searchOrders + " format is invalid!");
                    }
                }
                else
                {
                    throw new ArgumentException("SearchOrder of " + searchOrders + " format is invalid!");
                }
            }
            return ret;
        }
    }
}
