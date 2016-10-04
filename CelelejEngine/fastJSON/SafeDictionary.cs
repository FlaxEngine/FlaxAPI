// Celelej Game Engine scripting API

// -----------------------------------------------------------------------------
// Original code from fastJSON project. https://github.com/mgholam/fastJSON
// Greetings to Mehdi Gholam
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace fastJSON
{
    public sealed class SafeDictionary <TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly object _locker = new object();

        /// <summary>
        /// Amount of objects in a dictionary
        /// </summary>
        public int Count
        {
            get
            {
                lock (_locker)
                {
                    return _dictionary.Count;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_locker)
                {
                    return _dictionary[key];
                }
            }
            set
            {
                lock (_locker)
                {
                    _dictionary[key] = value;
                }
            }
        }

        public SafeDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public SafeDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_locker)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_locker)
            {
                if (_dictionary.ContainsKey(key) == false)
                    _dictionary.Add(key, value);
            }
        }

        public void Clear()
        {
            lock (_locker)
            {
                _dictionary.Clear();
            }
        }
    }
}
