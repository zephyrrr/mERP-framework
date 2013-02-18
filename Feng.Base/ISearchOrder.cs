using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Interface for SearchOrder
    /// </summary>
    public interface ISearchOrder
    {
        /// <summary>
        /// PropertyName
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Ascending
        /// </summary>
        bool Ascending { get; }
    }
}
