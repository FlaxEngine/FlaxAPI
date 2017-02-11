using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.Collections
{
    /// <summary>
    /// Creates new structure array like, with fast front and back insertion.
    /// <para>Every overflow of this buffer removes last item form other side of insertion</para>
    /// </summary>
    /// <typeparam name="T">Type of items to insert into buffer</typeparam>
    public class CircularBuffer<T> : IEnumerable<T>, IReadOnlyCollection<T>
    {
        private T[] _buffer;
        private int _frontItem = 0;
        private int _backItem = 0;

        /// <summary>
        /// Amount of items currently in buffer
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Current capacity of internal buffer
        /// </summary>
        public int Capacity => _buffer.Length;

        /// <summary>
        /// Returns true if there are no items in sturcutre, or false if there are
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Returns true if buffer is filled with whole of its capacity with items
        /// </summary>
        public bool IsFull => Count == Capacity;

        /// <summary>
        /// Creates new instance of object with given capacity
        /// </summary>
        /// <param name="capacity">Capacity of internal structure</param>
        public CircularBuffer(int capacity)
            : this(capacity, new T[] { })
        {
        }

        /// <summary>
        /// Creates new instance of object with given capacity and adds array of items to internall buffer
        /// </summary>
        /// <param name="capacity">Capacity of internal structure</param>
        /// <param name="items">Items to input</param>
        /// <param name="arrayIndex">Index of items to input at in internal buffer</param>
        public CircularBuffer(int capacity, T[] items, int arrayIndex = 0)
        {
            if(capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "argument cannot be lower or equal zero");
            }
            if(items.Length + arrayIndex > capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(items), "argument cannot be larger then requested capaclity with moved arrayIndex");
            }
            _buffer = new T[capacity];
            items.CopyTo(_buffer, arrayIndex);
            _backItem = arrayIndex;
            _frontItem = items.Length + arrayIndex;
            if(items.Length > 0)
            {
                // Move items back if there are any items in provided array
                _frontItem -= 1;
            }
            Count = items.Length;
        }

        /// <summary>
        /// Gets or sets item from list at given index.
        /// <remarks>All items are in order of input regardless of overlow that may occur</remarks>
        /// </summary>
        /// <param name="index">Index to item required</param>
        public T this[int index]
        {
            get
            {
                if(index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "argument cannot be lower then zero");
                }
                if(index >= Count)
                {
                    throw new IndexOutOfRangeException("argument cannot be bigger then amount of elements");
                }
                var currentIndex = (index + _backItem) % Capacity;
                return _buffer[currentIndex];
            }
            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "argument cannot be lower then zero");
                }
                if (index >= Count)
                {
                    throw new IndexOutOfRangeException("argument cannot be bigger then amount of elements");
                }
                var currentIndex = (index + _backItem) % Capacity;
                _buffer[currentIndex] = value;
            }
        }

        /// <summary>
        /// Adds item to the front of the buffer
        /// </summary>
        /// <param name="item">Item to add</param>
        public void PushFront(T item)
        {
            if(!IsEmpty)
            {
                IncreaseFrontIndex();
            }

            _buffer[_frontItem] = item;
            if (Count < Capacity)
            {
                Count++;
            }
        }

        /// <summary>
        /// Adds item to the back of the buffer
        /// </summary>
        /// <param name="item">Item to add</param>
        public void PushBack(T item)
        {
            if (!IsEmpty)
            {
                DecreaseBackIndex();
            }

            _buffer[_backItem] = item;
            if (Count < Capacity)
            {
                Count++;
            }
        }

        /// <summary>
        /// Gets top first element form collection
        /// </summary>
        public T Front()
        {
            if (Count == 0)
            {
                throw new IndexOutOfRangeException("Collection cannot be empty");
            }
            return _buffer[_frontItem];
        }

        /// <summary>
        /// Gets bottom first element form collection
        /// </summary>
        public T Back()
        {
            if (Count == 0)
            {
                throw new IndexOutOfRangeException("Collection cannot be empty");
            }
            return _buffer[_backItem];
        }

        /// <summary>
        /// Removes first item from the front of the buffer
        /// </summary>
        /// <returns></returns>
        public T PopFront()
        {
            if (IsEmpty)
            {
                throw new IndexOutOfRangeException("You cannot remove item from empty collection");
            }
            var result = Front();
            Count--;
            if (!IsEmpty)
            {
                DecreaseFrontIndex();
            }
            else
            {
                _frontItem = 0;
                _backItem = 0;
            }
            return result;
        }

        /// <summary>
        /// Removes first item from the back of the buffer
        /// </summary>
        /// <returns></returns>
        public T PopBack()
        {
            if (IsEmpty)
            {
                throw new IndexOutOfRangeException("You cannot remove item from empty collection");
            }
            var result = Back();
            Count--;
            if (!IsEmpty)
            {
                IncreaseBackIndex();
            }
            else
            {
                _frontItem = 0;
                _backItem = 0;
            }
            return result;
        }


        /// <summary>
        /// Copies the buffer contents to an array, acording to the logical
        /// contents of the buffer (i.e. independent of the internal
        /// order/contents)
        /// </summary>
        /// <returns>A new array with a copy of the buffer contents.</returns>
        public T[] ToArray()
        {
            lock(_buffer)
            {
                var result = new T[Count];
                if(_backItem > _frontItem)
                {
                    Array.Copy(_buffer, _backItem, result, 0, Capacity - _backItem);
                    Array.Copy(_buffer, 0, result, Capacity - _backItem, _frontItem + 1);
                }
                else
                {
                    Array.Copy(_buffer, _backItem, result, 0, _frontItem - _backItem + 1);
                }

                return result;
            }
        }

        /// <summary>
        /// CopyTo copies a collection into an Array, starting at a particular index into the array.
        /// </summary>
        /// <returns>A new array with a copy of the buffer contents.</returns>
        public void CopyTo(T[] array, int arrayIndex)
        {
            ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Clears buffer and remains capacity
        /// </summary>
        public void Clear()
        {
            _buffer = new T[Capacity];
            _frontItem = 0;
            _backItem = 0;
            Count = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty) yield break;
            var array = ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                yield return array[i];
            }
        }

        /// <summary>
        /// Decrease index of _backItem and warp it round if fall below 0
        /// <para>Move _frontItem back index if they've met</para>
        /// </summary>
        private void DecreaseBackIndex()
        {
            var currentIndex = (--_backItem) % Capacity;
            if (currentIndex < 0)
            {
                currentIndex = Capacity + currentIndex;
            }
            _backItem = currentIndex;
            if (_backItem == _frontItem && IsFull)
            {
                DecreaseFrontIndex();
            }
        }

        /// <summary>
        /// Decrease index of _frontItem and warp it round if fall below 0
        /// <para>Move _backItem back index if they've met</para>
        /// </summary>
        private void DecreaseFrontIndex()
        {
            var currentIndex = (--_frontItem) % Capacity;
            if (currentIndex < 0)
            {
                currentIndex = Capacity + currentIndex;
            }
            _frontItem = currentIndex;
            if (_backItem == _frontItem && IsFull)
            {
                DecreaseBackIndex();
            }
        }

        /// <summary>
        /// Increases index of _backItem and warp it round if exceded capacity
        /// <para>Move _frontItem forward index if they've met</para>
        /// </summary>
        private void IncreaseBackIndex()
        {
            _backItem = (++_backItem) % Capacity;
            if (_backItem == _frontItem)
            {
                IncreaseFrontIndex();
            }
        }

        /// <summary>
        /// Increases index of _frontItem and warp it round if exceded capacity
        /// <para>Move _backItem forward index if they've met</para>
        /// </summary>
        private void IncreaseFrontIndex()
        {
            _frontItem = (++_frontItem) % Capacity;
            if (_backItem == _frontItem)
            {
                IncreaseBackIndex();
            }
        }
    }
}