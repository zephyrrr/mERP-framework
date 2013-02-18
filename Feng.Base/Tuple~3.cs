using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public class Tuple<T1, T2, T3> : Tuple<T1, Tuple<T2, T3>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        public Tuple(T1 t1, T2 t2, T3 t3)
            : base(t1, new Tuple<T2, T3>(t2, t3))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public T1 Item1
        {
            get { return this.First; }
        }

        /// <summary>
        /// 
        /// </summary>
        public T2 Item2
        {
            get { return this.Second.First; }
        }

        /// <summary>
        /// 
        /// </summary>
        public T3 Item3
        {
            get { return this.Second.Second; }
        }
    }
}
