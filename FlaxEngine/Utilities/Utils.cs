////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace FlaxEngine
{
    /// <summary>
    /// Class with helper functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Determines whether two arrays are equal by comparing the elements by using the default equality comparer for their type.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="a1">The first array.</param>
        /// <param name="a2">The second array.</param>
        /// <returns><c>true</c> if the two source sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type; otherwise, <c>false</c>.</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;
            }

            return true;
        }
    }
}
