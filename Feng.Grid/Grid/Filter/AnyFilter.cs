using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class AnyFilter : IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Evaluate(object value)
        {
            return true;
        }
    }
}