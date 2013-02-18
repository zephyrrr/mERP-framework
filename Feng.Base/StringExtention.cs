using System;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Allows case insensitive checks
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string value, StringComparison comp)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(source))
                return false;

            return source.IndexOf(value, comp) >= 0;
        } 
    }
}
