using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Grid.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Evaluate(object value);
    }
}