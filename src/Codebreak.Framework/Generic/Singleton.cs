using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T>
        where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public static ILog Logger = LogManager.GetLogger(typeof(T));

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                return SingletonAllocator.instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal static class SingletonAllocator
        {
            internal static T instance;

            static SingletonAllocator()
            {
                instance = new T();
            }
        }
    }
}
