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
        private readonly Dictionary<TKey, TValue> _Dictionary;
        private readonly object _Padlock = new object();

        public int Count
        {
            get
            {
                lock (_Padlock)
                {
                    return _Dictionary.Count;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_Padlock)
                {
                    return _Dictionary[key];
                }
            }
            set
            {
                lock (_Padlock)
                {
                    _Dictionary[key] = value;
                }
            }
        }

        public SafeDictionary(int capacity)
        {
            _Dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public SafeDictionary()
        {
            _Dictionary = new Dictionary<TKey, TValue>();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_Padlock)
            {
                return _Dictionary.TryGetValue(key, out value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_Padlock)
            {
                if (_Dictionary.ContainsKey(key) == false)
                    _Dictionary.Add(key, value);
            }
        }
    }
}
