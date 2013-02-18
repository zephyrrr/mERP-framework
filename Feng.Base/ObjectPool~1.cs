using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ObjectPool<T> : IDisposable
        where T: class, new()
    {
        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (poolQ.Count > 0)
                {
                    T t = poolQ.Dequeue();
                    if (t is IDisposable)
                    {
                        (t as IDisposable).Dispose();
                    }
                }
            }
        }

        private Queue<T> poolQ = new Queue<T>();
        private static readonly object objLock = new object();
        private int PoolSize = 10;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T GetOne()
        {
            T oMyClass;
            lock (objLock)
            {
                if (poolQ.Count > 0)
                    oMyClass = ReleaseObjectFromPool();
                else
                    oMyClass = GetNewOne();
                return oMyClass;
            }
        }

        private T GetNewOne()
        {
            T myObj = new T();
            return myObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void CreatePoolObjects(T obj)
        {
            T _obj = obj;
            poolQ.Clear();

            for (int objCounter = 0; objCounter < PoolSize; objCounter++)
            {
                _obj = new T();

                AddObjectToPool(_obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddObjectToPool(T obj)
        {
            lock (objLock)
            {
                poolQ.Enqueue(obj);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private T ReleaseObjectFromPool()
        {
            if (poolQ.Count == 0)
                return null;

            lock (objLock)
            {
                T ret = poolQ.Dequeue();
                return ret;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int CurrentObjectsInPool
        {
            get
            {
                return poolQ.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxObjectsInPool
        {
            get
            {
                return PoolSize;
            }
            set
            {
                PoolSize = value;
            }
        }
    }
}
