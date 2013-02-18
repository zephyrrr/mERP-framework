using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Search
{
    /// <summary>
    /// 查询顺序
    /// </summary>
    public class Order : ISearchOrder
    {
        private bool _ascending;
        private string _propertyName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="ascending">升序或者降序</param>
        public Order(string propertyName, bool ascending)
        {
            this._propertyName = propertyName;
            this._ascending = ascending;
        }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// 升序或者降序
        /// </summary>
        public bool Ascending
        {
            get { return _ascending; }
        }

        /// <summary>
        /// 查询顺序的字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (this._propertyName + (this._ascending ? " asc" : " desc"));
        }
    }
}
