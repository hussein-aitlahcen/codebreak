using System;
using System.Collections.Generic;

namespace Codebreak.Framework.Generic
{
    /// <summary>
    /// PriorityQueue&lt;T&gt; is a C# implementation of a generic priority queue based on a binary min-heap.
    /// This C# implementation provides O(log(n)) time for insertion methods; O(log(n)) time for removal methods; and constant time for retrieval methods.
    /// The implementation is derived from the List&lt;T&gt; class, therefore most List&lt;T&gt; methods can also be applied to a PriorityQueue&lt;T&gt;, 
    /// however, List&lt;T&gt; methods that would destroy the heap condition of the priority queue will throw an InvalidOperationException, e.g. Reverse().
    /// </summary>
    /// <typeparam name="T">
    /// The type of values in the priority queue. The type must implement the IComparable&lt;T&gt; interface.
    /// </typeparam>
    public class PriorityQueue<T> : List<T>, IList<T> where T : IComparable<T>
    {
        private readonly object _lock;
        private readonly IComparer<T> _comparer;

        #region Heap handling

        private int CompareItems(T left, T right)
        {
            if (_comparer == null)
            {
                if (left == null)
                {
                    if (right == null)
                    {
                        return 0;
                    }
                    return -1;
                }
                return left.CompareTo(right);
            }
            return _comparer.Compare(left, right);
        }

        private int LastParentIndex
        {
            // For zero based arrays all elements with an index bigger
            // than 'Count / 2 - 1' do not have any children
            get
            {
                return Count / 2 - 1;
            }
        }

        private void EnsureHeapConditionDownward(int index)
        {
            int lastParentIndex = LastParentIndex;
            if (index <= lastParentIndex)
            {
                var entry = base[index];

                // As long as the entry has at least one child
                //
                while (index <= lastParentIndex)
                {
                    // For zero based arrays the left child is at '2 * index + 1'
                    //
                    int childIndex = 2 * index + 1;

                    var child = base[childIndex];

                    // If the entry also has a right child, the smaller child
                    // needs to be considered for the swap
                    //
                    if (childIndex < Count - 1)
                    {
                        // The right child is next to the left child in the array
                        //
                        int rightChildIndex = childIndex + 1;
                        var rightChild = base[rightChildIndex];

                        if (CompareItems(rightChild, child) < 0)
                        {
                            // Use the right child for the swap
                            //
                            child = rightChild;
                            childIndex = rightChildIndex;
                        }
                    }

                    if (CompareItems(entry, child) <= 0)
                    {
                        // The heap condition is fulfilled for the entry
                        //
                        return;
                    }

                    // Do the swap with the child
                    //
                    base[childIndex] = entry;
                    base[index] = child;

                    index = childIndex;
                }
            }

            // The entry does not have any children, therefore
            // the heap condition is fulfilled for the entry
        }

        private int EnsureHeapConditionUpward(int index)
        {
            if (index > 0)
            {
                var entry = base[index];

                // As long as the entry is not the top of the heap
                //
                while (index > 0)
                {
                    // For zero based arrays the parent index is '( index - 1 ) / 2'
                    //
                    int parentIndex = (index - 1) / 2;
                    var parent = this[parentIndex];

                    if (CompareItems(entry, parent) >= 0)
                    {
                        // The heap condition is fulfilled for the entry
                        //
                        return index;
                    }

                    // Do the swap with the parent
                    //
                    base[parentIndex] = entry;
                    base[index] = parent;

                    index = parentIndex;
                }
            }

            // The entry is the top of the heap
            //
            return index;
        }

        /// <summary>
        /// Ensures the heap condition for an item in the PriorityQueue&lt;T&gt; at a given index. Call this method after you change the priority of an item in the PriorityQueue&lt;T&gt;.
        /// </summary>
        /// <remarks>
        /// Ensuring the heap condition for a single item is an O(Log(N)) operation, where N is the number of items in the queue. 
        /// </remarks>
        public void EnsureHeapCondition(int index)
        {
            int rc = EnsureHeapConditionUpward(index);
            if (rc == index)
            {
                EnsureHeapConditionDownward(index);
            }
        }

        /// <summary>
        /// Ensures the heap condition for the entire PriorityQueue&lt;T&gt;. Call this method after you change the priority of many items in the PriorityQueue&lt;T&gt;.
        /// </summary>
        /// <remarks>
        /// Ensuring the heap condition for the entire priority queue is an O(N) operation, where N is the number of items in the queue. 
        /// </remarks>
        public void EnsureHeapCondition()
        {
            // The heap condition is always fulfilled for entries without children,
            // therefore 'bottom-up heap construction' only needs to look
            // at elements that do have children
            //
            for (int index = LastParentIndex; index >= 0; index--)
            {
                EnsureHeapConditionDownward(index);
            }
        }

        private void HeapRemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(string.Format("Index {0} is less than 0, or index is equal to or greater than priority queue count {1}.", index, Count));
            }
            else if (index == Count - 1)
            {
                base.RemoveAt(index);
                return;
            }

            // Remove the last entry in the list
            //
            base[index] = base[Count - 1];
            base.RemoveAt(Count - 1);

            if (Count > 1)
            {
                // Copying the values might have violated the heap condition
                // at position index, it needs to ensured
                //
                EnsureHeapCondition(index);
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; class that is empty and has the default initial capacity.
        /// </summary>
        public PriorityQueue()
        {
            _lock = new object();
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        public PriorityQueue(IEnumerable<T> enumerable)
            : base(enumerable)
        {
            _lock = new object();
            EnsureHeapCondition();
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; class that is empty and has the specified initial capacity.
        /// </summary>
        public PriorityQueue(Int32 capacity)
            : base(capacity)
        {
            _lock = new object();
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; class that uses a specified comparer.
        /// </summary>
        public PriorityQueue(IComparer<T> comparer)
        {
            _comparer = comparer;
            _lock = new object();
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; class that contains elements copied from a specified enumerable collection and that uses a specified comparer.
        /// </summary>
        public PriorityQueue(IEnumerable<T> enumerable, IComparer<T> comparer)
            : base(enumerable)
        {
            _comparer = comparer;
            _lock = new object();
            EnsureHeapCondition();
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue&lt;T&gt; class that is empty, has the specified initial capacity and uses a specified comparer.
        /// </summary>
        public PriorityQueue(Int32 capacity, IComparer<T> comparer)
            : base(capacity)
        {
            _comparer = comparer;
            _lock = new object();
        }
        #endregion

        #region Queue methods

        /// <summary>
        /// Adds an object to the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="item">
        /// The object to add to the PriorityQueue&lt;T&gt;.
        /// </param>
        /// <remarks>
        /// PriorityQueue&lt;T&gt; accepts null as a valid value for reference types and allows duplicate elements.
        ///
        /// If the number of items in the queue is less than Capacity, this method is an O(Log(N)) operation, where N is the number of items in the queue. If the capacity needs to be increased to accommodate the new element, this method becomes an O(N) operation, where N is the number of items in the queue.
        /// </remarks>
        public void Enqueue(T item)
        {
            base.Add(item);
            EnsureHeapConditionUpward(Count - 1);
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the PriorityQueue&lt;T&gt;. I.e. an item with the lowest priority from the priority queue, maintaining the heap condition of the queue.
        /// </summary>
        /// <returns>
        /// The object that is removed from the beginning of the PriorityQueue&lt;T&gt;.
        /// </returns>
        /// <remarks>
        /// This method is similar to the Peek method, but Peek does not modify the PriorityQueue&lt;T&gt;
        ///
        /// This method is an O(Log(N)) operation, where N is the number of items in the queue.
        /// </remarks>
        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }
            var result = base[0];
            HeapRemoveAt(0);
            return result;
        }

        /// <summary>
        /// Returns the object at the beginning of the PriorityQueue&lt;T&gt; without removing it. I.e. an item with the lowest priority.
        /// </summary>
        /// <returns>
        /// The object at the beginning of the PriorityQueue&lt;T&gt;.
        /// </returns>
        /// <remarks>
        /// This method is similar to the Dequeue method, but Peek does not modify the PriorityQueue&lt;T&gt;
        ///
        /// This method is an O(1) operation.
        /// </remarks>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }
            return base[0];
        }
        #endregion

        #region List methods

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <remarks>
        /// PriorityQueue&lt;T&gt; accepts null as a valid value for reference types and allows duplicate elements.
        /// 
        /// This property provides the ability to access a specific element in the collection by using the following syntax: myCollection[index].
        /// 
        /// Retrieving the value of this property is an O(1) operation; setting the property is an O(Log(N)) operation, where N is the number of items in the queue.

        /// </remarks>
        public new T this[int index]
        {
            get { return base[index]; }
            set
            {
                base[index] = value;
                EnsureHeapCondition(index);
            }
        }

        /// <summary>
        /// Adds an object to the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="item">
        /// The object to add to the PriorityQueue&lt;T&gt;.
        /// </param>
        /// <remarks>
        /// PriorityQueue&lt;T&gt; accepts null as a valid value for reference types and allows duplicate elements.
        ///
        /// If the number of items in the queue is less than Capacity, this method is an O(Log(N)) operation, where N is the number of items in the queue. If the capacity needs to be increased to accommodate the new element, this method becomes an O(N) operation, where N is the number of items in the queue.
        /// </remarks>
        public new void Add(T item)
        {
            Enqueue(item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="collection">
        /// The collection whose elements should be added the PriorityQueue&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.
        /// </param>
        /// <remarks>
        /// This method is an O(N) operation, where N is the number of items in the queue after the operation.
        /// </remarks>
        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            EnsureHeapCondition();
        }

        /// <summary>
        /// Inserts an object to the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <param name="item">
        /// The object to add to the PriorityQueue&lt;T&gt;.
        /// </param>
        /// <remarks>
        /// As this method maintains the heap condition of the queue, the item inserted might actually be moved to an index different to the one specified.
        /// 
        /// PriorityQueue&lt;T&gt; accepts null as a valid value for reference types and allows duplicate elements.
        ///
        /// If the number of items in the queue is less than Capacity, this method is an O(Log(N)) operation, where N is the number of items in the queue. If the capacity needs to be increased to accommodate the new element, this method becomes an O(N) operation, where N is the number of items in the queue.
        /// </remarks>
        public new void Insert(int index, T item)
        {
            Enqueue(item);
        }

        /// <summary>
        /// Inserts the elements of the specified collection to the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <param name="collection">
        /// The collection whose elements should be added the PriorityQueue&lt;T&gt;. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.
        /// </param>
        /// <remarks>
        /// As this method maintains the heap condition of the queue, the items inserted might actually be moved to indices different to the ones specified.
        /// 
        /// This method is an O(N) operation, where N is the number of items in the queue after the operation.
        /// </remarks>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            EnsureHeapCondition();
        }

        /// <summary>
        /// Removes the element at the specified index of PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to remove.
        /// </param>
        /// <remarks>
        /// This method is an O(Log(N)) operation, where N is the number of items in the queue.
        /// </remarks>
        public new void RemoveAt(int index)
        {
            HeapRemoveAt(index);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the PriorityQueue&lt;T&gt;. The value can be null for reference types.
        /// </param>
        /// <returns>
        /// true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the PriorityQueue&lt;T&gt;.
        /// </returns>
        /// <remarks>
        /// This method performs a linear search; therefore, this method is an O(N) operation, where N is the number of items in the queue.
        /// </remarks>
        public new bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            HeapRemoveAt(index);
            return true;
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="match">
        /// The Predicate&lt;T&gt; delegate that defines the conditions of the elements to remove.
        /// </param>
        /// <returns>
        /// The number of elements removed from the PriorityQueue&lt;T&gt;.
        /// </returns>
        /// <remarks>
        /// This method performs a linear search; therefore, this method is an O(N) operation, where N is the number of items in the queue.
        /// </remarks>
        public new int RemoveAll(Predicate<T> match)
        {
            var result = base.RemoveAll(match);
            if (result > 0)
            {
                EnsureHeapCondition();
            }
            return result;
        }

        /// <summary>
        /// Removes a range of elements from the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <param name="index">
        /// The zero-based starting index of the range of elements to remove.
        /// </param>
        /// <param name="count">
        /// The number of elements to remove.
        /// </param>
        /// <remarks>
        /// This method is an O(N) operation, where N is the number of items in the queue.
        /// </remarks>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            if (index > LastParentIndex)
            {
                EnsureHeapCondition();
            }
        }

        /// <summary>
        /// As the heap condition of the queue needs to be maintained, this method will throw an InvalidOperationException, if the queue is not empty.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This method will throw an InvalidOperationException, if the queue is not empty.
        /// </exception>
        public new void Reverse()
        {
            if (Count > 1)
            {
                throw new InvalidOperationException("A priority queue cannot be reversed.");
            }
        }

        /// <summary>
        /// As the heap condition of the queue needs to be maintained, this method will throw an InvalidOperationException, if the queue is not empty.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This method will throw an InvalidOperationException, if the queue is not empty.
        /// </exception>
        public new void Reverse(int index, int count)
        {
            if (index > LastParentIndex)
            {
                throw new InvalidOperationException("A priority queue cannot be partially reversed.");
            }
            base.Reverse(index, count);
        }

        /// <summary>
        /// Sort the elements of the PriorityQueue&lt;T&gt;, maintaining the heap condition of the queue.
        /// </summary>
        /// <remarks>
        /// This method uses Array.Sort, which uses the QuickSort algorithm. This implementation performs an unstable sort; that is, if two elements are equal, their order might not be preserved. In contrast, a stable sort preserves the order of elements that are equal.
        /// 
        /// On average, this method is an O(N log(N)) operation, where N is the number of items in the queue; in the worst case it is an O(N ^ 2) operation.
        /// </remarks>
        public new void Sort()
        {
            if (Count > 1)
            {
                if (_comparer != null)
                {
                    base.Sort(_comparer);
                }
                else
                {
                    base.Sort();
                }
            }
        }

        /// <summary>
        /// As the heap condition of the queue needs to be maintained, this method will throw an InvalidOperationException, if the comparer given is no the same specified in the constructor and the queue is not empty.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This method will throw an InvalidOperationException, if the comparer given is no the same specified in the constructor and the queue is not empty.
        /// </exception>
        public new void Sort(IComparer<T> comparer)
        {
            if (Count > 1)
            {
                if (!ReferenceEquals(comparer, _comparer))
                {
                    throw new InvalidOperationException("A priority queue cannot be sorted with a comparer other than the one specified for the priority queue.");
                }
                base.Sort(_comparer);
            }
        }

        /// <summary>
        /// As the heap condition of the queue needs to be maintained, this method will throw an InvalidOperationException, if the comparer given is no the same specified in the constructor and the queue is not empty.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This method will throw an InvalidOperationException, if the comparer given is no the same specified in the constructor and the queue is not empty.
        /// </exception>
        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            if (Count > 1)
            {
                if (!ReferenceEquals(comparer, _comparer) || index != 0 || count != Count)
                {
                    throw new InvalidOperationException("A priority queue cannot be partially sorted.");
                }
                base.Sort(index, count, _comparer);
            }
        }

        /// <summary>
        /// As the heap condition of the queue needs to be maintained, this method will throw an InvalidOperationException, if the queue is not empty.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This method will throw an InvalidOperationException, if the queue is not empty.
        /// </exception>
        public new void Sort(Comparison<T> comparison)
        {
            if (Count > 1)
            {
                throw new InvalidOperationException("A priority queue cannot be sorted with a comparison.");
            }
        }
        #endregion

        #region Misc

        /// <summary>
        /// Gets an object that can be used to synchronize access to the priority queue.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the priority queue.
        /// </returns>
        public object SyncRoot
        {
            get { return _lock; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the PriorityQueue&lt;T&gt; is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// false.
        /// </returns>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the IComparer&lt;T&gt; object that is used to order the values in the PriorityQueue&lt;T&gt;.
        /// </summary>
        /// <returns>
        /// The comparer that is used to order the values in the PriorityQueue&lt;T&gt; or null if no comparer has be specified.
        /// </returns>
        public IComparer<T> Comparer { get { return _comparer; } }

        #endregion
    }
}
