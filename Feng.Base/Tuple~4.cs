using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// Tuple
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public class Tuple<T1, T2, T3, T4> : Tuple<T1, Tuple<T2, T3, T4>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
            : base(t1, new Tuple<T2, T3, T4>(t2, t3, t4))
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
            get { return this.Second.Second.First; }
        }

        /// <summary>
        /// 
        /// </summary>
        public T4 Item4
        {
            get { return this.Second.Second.Second; }
        }
    }
}
