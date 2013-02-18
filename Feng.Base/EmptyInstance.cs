using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmptyInstance
    {
        private static Dictionary<Type, object> m_empties = new Dictionary<Type, object>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetEmpty<T>()
            where T:class, new()
        {
            if (!m_empties.ContainsKey(typeof(T)))
            {
                m_empties[typeof(T)] = new T();
            }
            return m_empties[typeof(T)] as T;
        }
    }
}
