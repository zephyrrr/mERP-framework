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
    public class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        private readonly T1 _first;
        /// <summary>
        /// First
        /// </summary>
        public T1 First
        {
            get { return _first; }
        }

        private readonly T2 _second;
        /// <summary>
        /// Second
        /// </summary>
        public T2 Second
        {
            get { return _second; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Tuple(T1 first, T2 second)
        {
            _first = first;
            _second = second;
        }

        #region IEquatable<Tuple<T1,T2>> Members
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Tuple<T1, T2> other)
        {
            return _first.Equals(other._first) &&
                    _second.Equals(other._second);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Tuple<T1, T2> that = obj as Tuple<T1, T2>;
            if (that != null)
                return this.Equals(that);
            else
                return false;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _first.GetHashCode() ^ _second.GetHashCode();
        }
        #endregion
    }

}
