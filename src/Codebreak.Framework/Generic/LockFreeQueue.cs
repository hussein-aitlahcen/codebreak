using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SingleLinkNode<T>
    {
        public SingleLinkNode<T> Next;
        public T Item;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LockFreeQueue<T> : IEnumerable<T>
    {
        private SingleLinkNode<T> m_head;
        private SingleLinkNode<T> m_tail;
        private int m_count;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LockFreeQueue()
        {
            m_head = new SingleLinkNode<T>();
            m_tail = m_head;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public LockFreeQueue(IEnumerable<T> items)
            : this()
        {
            foreach (var item in items)
            {
                Enqueue(item);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the queue.
        /// </summary>
        public int Count
        {
            get { return Thread.VolatileRead(ref m_count); }
        }

        /// <summary>
        /// Adds an object to the end of the queue.
        /// </summary>
        /// <param name="item">the object to add to the queue</param>
        public void Enqueue(T item)
        {
            SingleLinkNode<T> oldTail = null;
            SingleLinkNode<T> oldTailNext;

            var newNode = new SingleLinkNode<T> { Item = item };

            bool newNodeWasAdded = false;

            while (!newNodeWasAdded)
            {
                oldTail = m_tail;
                oldTailNext = oldTail.Next;

                if (m_tail == oldTail)
                {
                    if (oldTailNext == null)
                    {
                        newNodeWasAdded =
                            Interlocked.CompareExchange<SingleLinkNode<T>>(ref m_tail.Next, newNode, null) == null;
                    }
                    else
                    {
                        Interlocked.CompareExchange<SingleLinkNode<T>>(ref m_tail, oldTailNext, oldTail);
                    }
                }
            }

            Interlocked.CompareExchange<SingleLinkNode<T>>(ref m_tail, newNode, oldTail);
            Interlocked.Increment(ref m_count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T TryDequeue()
        {
            T item;
            TryDequeue(out item);
            return item;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the queue.
        /// </summary>
        /// <param name="item">
        /// when the method returns, contains the object removed from the beginning of the queue,
        /// if the queue is not empty; otherwise it is the default value for the element type
        /// </param>
        /// <returns>
        /// true if an object from removed from the beginning of the queue;
        /// false if the queue is empty
        /// </returns>
        public bool TryDequeue(out T item)
        {
            item = default(T);
            SingleLinkNode<T> oldHead = null;

            bool haveAdvancedHead = false;
            while (!haveAdvancedHead)
            {
                oldHead = m_head;
                SingleLinkNode<T> oldTail = m_tail;
                SingleLinkNode<T> oldHeadNext = oldHead.Next;

                if (oldHead == m_head)
                {
                    if (oldHead == oldTail)
                    {
                        if (oldHeadNext == null)
                            return false;

                        Interlocked.CompareExchange<SingleLinkNode<T>>(ref m_tail, oldHeadNext, oldTail);
                    }

                    else
                    {
                        item = oldHeadNext.Item;
                        haveAdvancedHead =
                          Interlocked.CompareExchange<SingleLinkNode<T>>(ref m_head, oldHeadNext, oldHead) == oldHead;
                    }
                }
            }

            Interlocked.Decrement(ref m_count);
            return true;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the queue.
        /// </summary>
        /// <returns>the object that is removed from the beginning of the queue</returns>
        public T Dequeue()
        {
            T result;

            if (!TryDequeue(out result))
            {
                throw new InvalidOperationException("the queue is empty");
            }

            return result;
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the queue.
        /// </summary>
        /// <returns>an enumerator for the queue</returns>
        public IEnumerator<T> GetEnumerator()
        {
            SingleLinkNode<T> currentNode = m_head;

            do
            {
                if (currentNode.Item == null)
                {
                    yield break;
                }
                else
                {
                    yield return currentNode.Item;
                }
            }
            while ((currentNode = currentNode.Next) != null);

            yield break;
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the queue.
        /// </summary>
        /// <returns>an enumerator for the queue</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Clears the queue.
        /// </summary>
        /// <remarks>This method is not thread-safe.</remarks>
        public void Clear()
        {
            SingleLinkNode<T> tempNode;
            SingleLinkNode<T> currentNode = m_head;

            while (currentNode != null)
            {
                tempNode = currentNode;
                currentNode = currentNode.Next;

                tempNode.Item = default(T);
                tempNode.Next = null;
            }

            m_head = new SingleLinkNode<T>();
            m_tail = m_head;
            m_count = 0;
        }

    }
}
