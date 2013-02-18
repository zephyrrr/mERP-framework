using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProjectionComparer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static ProjectionComparer<TSource, TKey> Create<TSource, TKey>
            (Func<TSource, TKey> projection)
        {
            return new ProjectionComparer<TSource, TKey>(projection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="ignored"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static ProjectionComparer<TSource, TKey> Create<TSource, TKey>
            (TSource ignored, Func<TSource, TKey> projection)
        {
            return new ProjectionComparer<TSource, TKey>(projection);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public static class ProjectionComparer<TSource>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static ProjectionComparer<TSource, TKey> Create<TKey>
            (Func<TSource, TKey> projection)
        {
            return new ProjectionComparer<TSource, TKey>(projection);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class ProjectionComparer<TSource, TKey> : IComparer<TSource>
    {
        readonly Func<TSource, TKey> projection;
        readonly IComparer<TKey> comparer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projection"></param>
        public ProjectionComparer(Func<TSource, TKey> projection)
            : this(projection, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projection"></param>
        /// <param name="comparer"></param>
        public ProjectionComparer(Func<TSource, TKey> projection,
                                  IComparer<TKey> comparer)
        {
            if (projection == null)
            {
                throw new ArgumentException("projection");
            }
            this.comparer = comparer ?? Comparer<TKey>.Default;
            this.projection = projection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(TSource x, TSource y)
        {
            // Don't want to project from nullity
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            return comparer.Compare(projection(x), projection(y));
        }
    }
}
