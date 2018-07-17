// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FlaxEngine
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Get value of resultOnFail if provided dictionary doesn't have a key requested
        /// </summary>
        /// <typeparam name="TKey">Key of the dictionary</typeparam>
        /// <typeparam name="TValue">Value of the dictionary</typeparam>
        /// <param name="dictionary">Dictionary to get value in safe way</param>
        /// <param name="key">Key to look for</param>
        /// <param name="resultOnFail">Desired result on fail</param>
        /// <returns>Returns resultOnFail if key was not found, other wise <see cref="TValue"/></returns>
        [Pure]
        public static TValue GetValueSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue resultOnFail = null)
            where TValue : class
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
                return obj;
            return resultOnFail;
        }

        /// <summary>
        /// Get value of resultOnFail if provided dictionary doesn't have a key requested
        /// </summary>
        /// <typeparam name="TKey">Key of the dictionary</typeparam>
        /// <typeparam name="TValue">Value of the dictionary</typeparam>
        /// <param name="dictionary">Dictionary to get value in safe way</param>
        /// <param name="key">Key to look for</param>
        /// <param name="resultOnFail">Desired result on fail</param>
        /// <returns>Returns resultOnFail if key was not found, other wise <see cref="TValue"/></returns>
        [Pure]
        public static TValue GetReadOnlyValueSafe<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue resultOnFail = null)
            where TValue : class
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
                return obj;
            return resultOnFail;
        }

        /// <summary>
        /// Get value of resultOnFail if provided dictionary doesn't have a key requested
        /// </summary>
        /// <typeparam name="TKey">Key of the dictionary</typeparam>
        /// <typeparam name="TValue">Value of the dictionary</typeparam>
        /// <param name="dictionary">Dictionary to get value in safe way</param>
        /// <param name="key">Key to look for</param>
        /// <param name="resultOnFail">Desired result on fail</param>
        /// <returns>Returns resultOnFail if key was not found, other wise <see cref="TValue"/></returns>
        [Pure]
        public static TValue GetValueSafe<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue resultOnFail = null)
            where TValue : class
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
                return obj;
            return resultOnFail;
        }

        /// <summary>
        /// Short hand for finding a key based on value
        /// </summary>
        /// <typeparam name="TKey">Key of the dictionary</typeparam>
        /// <typeparam name="TValue">Value of the dictionary</typeparam>
        /// <param name="dictionary">Dictionary to get find key within</param>
        /// <param name="value">Value to match key with</param>
        /// <returns>Returns key</returns>
        [Pure]
        public static TKey FindKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>)dictionary)
            {
                if (object.Equals((object)value, (object)keyValuePair.Value))
                    return keyValuePair.Key;
            }
            return default(TKey);
        }

        /// <summary>
        /// Check if both provided dictionaries have the same key value pairs
        /// </summary>
        /// <typeparam name="TKey">Key of the dictionary</typeparam>
        /// <typeparam name="TValue">Value of the dictionary</typeparam>
        /// <param name="left">One dictionary to compare</param>
        /// <param name="right">One dictionary to compare</param>
        /// <returns>Returns true if both dictonaries have the same key value pairs</returns>
        [Pure]
        public static bool DictionaryEquals<TKey, TValue>(this IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
        {
            return left.DictionaryEquals<TKey, TValue>(right, (IEqualityComparer<TValue>)EqualityComparer<TValue>.Default);
        }

        /// <summary>
        /// Check if both provided dictionaries have the same key value pairs
        /// </summary>
        /// <typeparam name="TKey">Key of the dictionary</typeparam>
        /// <typeparam name="TValue">Value of the dictionary</typeparam>
        /// <param name="left">One dictionary to compare</param>
        /// <param name="right">One dictionary to compare</param>
        /// <param name="valueComparer">Custom <see cref="EqualityComparer<TValue>"/></param>
        /// <returns>Returns true if both dictonaries have the same key value pairs</returns>
        [Pure]
        public static bool DictionaryEquals<TKey, TValue>(this IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right, IEqualityComparer<TValue> valueComparer)
        {
            if (left == null)
                return right == null;
            if (right == null || left.Count != right.Count)
                return false;
            foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>)left)
            {
                TValue x;
                if (!right.TryGetValue(keyValuePair.Key, out x) || !valueComparer.Equals(x, keyValuePair.Value))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Use special algorithm for calculating dictionary hash code (instead of default reference algorithm)
        /// </summary>
        /// <typeparam name="TKey">Key of the dictionary</typeparam>
        /// <typeparam name="TValue">Value of the dictionary</typeparam>
        /// <param name="dictionary">Dictionary to get hash code from</param>
        /// <returns>Hash Code</returns>
        [Pure]
        public static int DictionaryGetHashCode<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                return 0;
            int num = 0;
            foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>)dictionary)
            {
                num = num * 397 ^ keyValuePair.Key.GetHashCode();
                num = num * 397 ^ keyValuePair.Value.GetHashCode();
            }
            return num;
        }
    }
}
