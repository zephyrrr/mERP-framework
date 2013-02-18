using System;

namespace Feng
{
    /// <summary>
    /// Singleton 范型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> 
        where T : new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Singleton()
        {
            if (Instance != null)
            {
                throw new ArgumentException("You have tried to create a new singleton class where you should have instanced it. Replace your \"new class()\" with \"class.Instance\"");
            }
        }

        /// <summary>
        /// Instance
        /// </summary>
        public static T Instance
        {
            get
            {
                return SingletonCreator.instance;
            }
        }

        class SingletonCreator
        {
            private SingletonCreator()
            {
            }

            internal static readonly T instance = new T();
        }
    }
}
