using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Utils
{
    /// <summary>
    /// 集合帮助类
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// 用于<see cref="Group"/>的分组条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public delegate S GetGroupKey<T, S>(T obj);

        /// <summary>
        /// 按照一定规则分组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="list"></param>
        /// <param name="funcGetGroupKey"></param>
        /// <returns></returns>
        public static Dictionary<S, IList<T>> Group<T, S>(IList<T> list, GetGroupKey<T, S> funcGetGroupKey)
        {
            Dictionary<S, IList<T>> dict = new Dictionary<S, IList<T>>();
            foreach (T i in list)
            {
                S key = funcGetGroupKey(i);
                if (key == null)
                    continue;

                if (!dict.ContainsKey(key))
                {
                    dict[key] = new List<T>();
                }
                dict[key].Add(i);
            }
            return dict;
        }
    }
}
