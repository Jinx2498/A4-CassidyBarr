using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameBrains.DataStructures
{
    // A priority queue item whose value is of type
    // This might be derived from a tutorial by Jim Mischel.
    [ComVisible(false)]
    public struct PriorityQueueItem<TValue, TPriority>
    {
        readonly TPriority priority;

        readonly TValue value;

        internal PriorityQueueItem(TValue val, TPriority pri)
        {
            value = val;
            priority = pri;
        }
        
        public TPriority Priority => priority;
        
        public TValue Value => value;
    }
    
    // A Priority Queue class
    [ComVisible(false)]
    public class PriorityQueue<TValue, TPriority> : ICollection, IEnumerable<PriorityQueueItem<TValue, TPriority>>
    {
        const int DefaultCapacity = 16;
        int capacity;
        System.Comparison<TPriority> compareFunc;
        PriorityQueueItem<TValue, TPriority>[] items;
        int count;
        int prioritySign;
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the default initial capacity, and uses the default comparer and
        // is ordered high first.
        public PriorityQueue()
            : this(DefaultCapacity, Comparer<TPriority>.Default, PriorityOrder.HighFirst)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the default initial capacity, and uses the default comparer and
        // is ordered as specified.
        public PriorityQueue(PriorityOrder priorityOrder)
            : this(DefaultCapacity, Comparer<TPriority>.Default, priorityOrder)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the given initial capacity, and uses the default comparer and
        // is ordered high first.
        public PriorityQueue(int initialCapacity)
            : this(initialCapacity, Comparer<TPriority>.Default, PriorityOrder.HighFirst)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the default initial capacity, and uses the given comparer and
        // is ordered high first.
        public PriorityQueue(IComparer<TPriority> comparer)
            : this(DefaultCapacity, comparer, PriorityOrder.HighFirst)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the given initial capacity, and uses the given comparer and
        // is ordered high first.
        public PriorityQueue(int initialCapacity, IComparer<TPriority> comparer)
        {
            Init(initialCapacity, comparer.Compare, PriorityOrder.HighFirst);
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the default initial capacity, and uses the given comparison and
        // is ordered high first.
        public PriorityQueue(System.Comparison<TPriority> comparison)
            : this(DefaultCapacity, comparison, PriorityOrder.HighFirst)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the given initial capacity, and uses the given comparison and
        // is ordered high first.
        public PriorityQueue(int initialCapacity, System.Comparison<TPriority> comparison)
        {
            Init(initialCapacity, comparison, PriorityOrder.HighFirst);
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the given initial capacity, and uses the default comparer and
        // has the given priority order.
        public PriorityQueue(int initialCapacity, PriorityOrder priorityOrder)
            : this(initialCapacity, Comparer<TPriority>.Default, priorityOrder)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the default initial capacity, and uses the given comparer and
        // has the given priority order.
        public PriorityQueue(IComparer<TPriority> comparer, PriorityOrder priorityOrder)
            : this(DefaultCapacity, comparer, priorityOrder)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the given initial capacity, and uses the given comparer and
        // has the given priority order.
        public PriorityQueue(int initialCapacity, IComparer<TPriority> comparer, PriorityOrder priorityOrder)
        {
            Init(initialCapacity, comparer.Compare, priorityOrder);
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the default initial capacity, and uses the given comparison and
        // has the given priority order.
        public PriorityQueue(System.Comparison<TPriority> comparison, PriorityOrder priorityOrder)
            : this(DefaultCapacity, comparison, priorityOrder)
        {
        }
        
        // Initializes a new instance of the PriorityQueue class that is empty,
        // has the given initial capacity, and uses the given comparison and
        // has the given priority order.
        public PriorityQueue(int initialCapacity, System.Comparison<TPriority> comparison, PriorityOrder priorityOrder)
        {
            Init(initialCapacity, comparison, priorityOrder);
        }
        
        // Priority order (highest first or lowest first)
        public enum PriorityOrder
        {
            HighFirst,
            LowFirst
        }
        
        // Capacity of the priority queue
        public int Capacity
        {
            get => items.Length;

            set => SetCapacity(value);
        }
        
        // Gets number of items in the priority queue
        public int Count => count;
        
        // Gets a value indicating whether access to this priority queue is
        // synchronized (thread safe).
        public bool IsSynchronized => false;
        
        // Gets an object that can be used to synchronize access to this
        // priority queue.
        public object SyncRoot => items.SyncRoot;
        
        // Clear priority queue
        public void Clear()
        {
            for (int i = 0; i < count; ++i)
            {
                items[i] = default(PriorityQueueItem<TValue, TPriority>);
            }

            count = 0;
            TrimExcess();
        }
        
        // Tests if priority queue contains the given value
        public bool Contains(TValue value)
        {
            foreach (PriorityQueueItem<TValue, TPriority> x in items)
            {
                if (x.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }
        
        // Copy to array starting at given array index.
        public void CopyTo(PriorityQueueItem<TValue, TPriority>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new System.ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new System.ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
            }

            if (array.Rank > 1)
            {
                throw new System.ArgumentException("array is multidimensional.");
            }

            if (count == 0)
            {
                return;
            }

            if (arrayIndex >= array.Length)
            {
                throw new System.ArgumentException("arrayIndex is equal to or greater than the length" + " of the array.");
            }

            if (count > (array.Length - arrayIndex))
            {
                throw new System.ArgumentException(
                    "The number of elements in the source ICollection is" +
                    " greater than the available space from arrayIndex to" + " the end of the destination array.");
            }

            for (int i = 0; i < count; i++)
            {
                array[arrayIndex + i] = items[i];
            }
        }
        
        public PriorityQueueItem<TValue, TPriority> Dequeue()
        {
            if (Count == 0)
            {
                throw new System.InvalidOperationException("The queue is empty");
            }

            return RemoveAt(0);
        }

        public void Enqueue(PriorityQueueItem<TValue, TPriority> newItem)
        {
            if (count == capacity)
            {
                // need to increase capacity
                // grow by 50 percent
                SetCapacity((3 * Capacity) / 2);
            }

            int i = count;
            ++count;
            while ((i > 0) && (prioritySign * compareFunc(items[(i - 1) / 2].Priority, newItem.Priority) < 0))
            {
                items[i] = items[(i - 1) / 2];
                i = (i - 1) / 2;
            }

            items[i] = newItem;

            // if (!VerifyQueue())
            // {
            //      Debug.Log("ERROR: Queue out of order!");
            // }
        }
        
        public void Enqueue(TValue value, TPriority priority)
        {
            Enqueue(new PriorityQueueItem<TValue, TPriority>(value, priority));
        }
        
        // Get (peek but not dequeue) first item
        public PriorityQueueItem<TValue, TPriority> Peek()
        {
            if (Count == 0)
            {
                throw new System.InvalidOperationException("The queue is empty.");
            }

            return items[0];
        }
        
        // Removes the item with the specified value from the queue.
        // The passed equality comparison is used.
        public void Remove(TValue item, IEqualityComparer comparer)
        {
            // need to find the PriorityQueueItem that has the Data value of item
            for (int index = 0; index < count; ++index)
            {
                if (!comparer.Equals(item, items[index].Value))
                {
                    continue;
                }

                RemoveAt(index);
                return;
            }

            throw new System.Exception("The specified item is not in the queue.");
        }
        
        // Removes the item with the specified value from the queue.
        // The default type comparison function is used.
        public void Remove(TValue item)
        {
            Remove(item, EqualityComparer<TValue>.Default);
        }
		
		public bool Remove(System.Predicate<TValue> match)
        {
            if (match == null)
            {
                throw new System.ArgumentNullException("match");
            }

            bool matchedAtLeastOne = false;

            for (int index = 0; index < Count; ++index)
            {
                if (match(items[index].Value))
                {
                    RemoveAt(index);
                    index--;
                    matchedAtLeastOne = true;
                }
            }

            return matchedAtLeastOne;
        }
        
        // Set the capacity to the actual number of items, if the current
        // number of items is less than 90 percent of the current capacity.
        public void TrimExcess()
        {
            if (count < (float)0.9 * capacity)
            {
                SetCapacity(count);
            }
        }
        
        // Function to check that the queue is coherent.
        public bool VerifyQueue()
        {
            int i = 0;
            while (i < count / 2)
            {
                int leftChild = (2 * i) + 1;
                int rightChild = leftChild + 1;
                if (prioritySign * compareFunc(items[i].Priority, items[leftChild].Priority) < 0)
                {
                    return false;
                }

                if (rightChild < count &&
                    prioritySign * compareFunc(items[i].Priority, items[rightChild].Priority) < 0)
                {
                    return false;
                }

                ++i;
            }

            return true;
        }
        
        // Copy to array starting at given index.
        public void CopyTo(System.Array array, int index)
        {
            CopyTo((PriorityQueueItem<TValue, TPriority>[])array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        // Returns an enumerator that iterates through this priority queue.
        public IEnumerator<PriorityQueueItem<TValue, TPriority>> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return items[i];
            }
        }

        void Init(int initialCapacity, System.Comparison<TPriority> comparison, PriorityOrder priorityOrder)
        {
            count = 0;
            compareFunc = comparison;
            SetCapacity(initialCapacity);

            // multiplier to apply to result of compareFunc
            // 1 for high priority first, -1 for low priority first
            prioritySign = (priorityOrder == PriorityOrder.HighFirst) ? 1 : -1;
        }

        PriorityQueueItem<TValue, TPriority> RemoveAt(int index)
        {
            PriorityQueueItem<TValue, TPriority> o = items[index];
            --count;

            // move the last item to fill the hole
            PriorityQueueItem<TValue, TPriority> tmp = items[count];

            // If you forget to clear this, you have a potential memory leak.
            items[count] = default(PriorityQueueItem<TValue, TPriority>);
            if (count > 0 && index != count)
            {
                // If the new item is greater than its parent, bubble up.
                int i = index;
                int parent = (i - 1) / 2;
                while (prioritySign * compareFunc(tmp.Priority, items[parent].Priority) > 0)
                {
                    items[i] = items[parent];
                    i = parent;
                    parent = (i - 1) / 2;
                }

                // if i == index, then we didn't move the item up
                if (i == index)
                {
                    // bubble down ...
                    while (i < count / 2)
                    {
                        int j = (2 * i) + 1;
                        if ((j < count - 1) &&
                            (prioritySign * compareFunc(items[j].Priority, items[j + 1].Priority) < 0))
                        {
                            ++j;
                        }

                        if (prioritySign * compareFunc(items[j].Priority, tmp.Priority) <= 0)
                        {
                            break;
                        }

                        items[i] = items[j];
                        i = j;
                    }
                }

                // Be sure to store the item in its place.
                items[i] = tmp;
            }

            // if (!VerifyQueue())
            // {
            //     Debug.Log("ERROR: Queue out of order!");
            // }
            return o;
        }

        void SetCapacity(int newCapacity)
        {
            int newCap = newCapacity;
            if (newCap < DefaultCapacity)
            {
                newCap = DefaultCapacity;
            }

            // throw exception if newCapacity < NumItems
            if (newCap < count)
            {
                throw new System.ArgumentOutOfRangeException("newCapacity", "New capacity is less than Count");
            }

            capacity = newCap;
            if (items == null)
            {
                items = new PriorityQueueItem<TValue, TPriority>[newCap];
                return;
            }

            // Resize the array.
            System.Array.Resize(ref items, newCap);
        }
    }
}