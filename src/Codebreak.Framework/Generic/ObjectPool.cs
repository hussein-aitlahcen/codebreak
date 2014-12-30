using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// Generic object pool class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ObjectPool<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Thread-safe queue of pooled object
        /// </summary>        
        private LockFreeQueue<T> m_objects = new LockFreeQueue<T>();
        private Func<T> m_creator;

        /// <summary>
        /// 
        /// </summary>
        public ObjectPool(Func<T> creator, int baseObjectCount = 0)
        {
            m_creator = creator;
            for (int i = 0; i < baseObjectCount; i++)            
                m_objects.Enqueue(m_creator());            
        }

        /// <summary>
        /// Try to pop an object or create it if the stack is empty.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T obj = null;
            if (!m_objects.TryDequeue(out obj))            
                obj = m_creator();            
            return obj;
        }

        /// <summary>
        /// Push back an object in the pool and clean it if its a IPoolable object.
        /// </summary>
        /// <param name="obj"></param>
        public void Push(T obj)
        {
            if (obj is IPoolable)            
                ((IPoolable)obj).Cleanup();
            
            m_objects.Enqueue(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            while (m_objects.Count > 0)
            {
                var obj = Pop();
                if (obj is IPoolable)
                    ((IPoolable)obj).Cleanup();
                if (obj is IDisposable)
                    ((IDisposable)obj).Dispose();
            }
        }
    }
}
