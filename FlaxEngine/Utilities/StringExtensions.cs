// ////////////////////////////////////////////////////////////////////////////////////
// // Copyright (c) 2012-2017 Flax Engine. All rights reserved.
// ////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace FlaxEngine
{
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if given string is null or empty (utility method)
        /// </summary>
        /// <param name="value">The string to test</param>
        /// <returns>Returns false is given string an empty string <see cref="string.Empty"/> or null</returns>
        [Pure]
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Checks if given string is equal to <see cref="string.Empty"/>
        /// </summary>
        /// <param name="value">The string to test</param>
        /// <returns>Returns false is given string an empty string <see cref="string.Empty"/></returns>
        public static bool IsEmpty(this string value)
        {
            return value == String.Empty;
        }

        /// <summary>
        /// Default Split implementation with 0 options <see cref="Split"/>
        /// </summary>
        /// <param name="value">Value to split from</param>
        /// <param name="separator">Separators based on which split will be performed</param>
        /// <seealso cref="string.Split(string[], StringSplitOptions)"></seealso>
        /// <returns>Returns splitted value</returns>
        [Pure]
        public static string[] Split(this string value, params string[] separator)
        {
            return value.Split(separator, StringSplitOptions.None);
        }

        /// <summary>
        /// Extedned implementation of <see cref="string.Contains"/> using <see cref="StringComparison"/> options
        /// </summary>
        /// <param name="original">Orginal value to validate</param>
        /// <param name="value">Value to search for</param>
        /// <param name="comparisonType">Comparasion type</param>
        /// <returns>Returns true if <see cref="original"/> contains <see cref="value"/></returns>
        [Pure]
        public static bool Contains(this string original, string value, StringComparison comparisonType)
        {
            return original.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static string SubstringBefore(this string original, string value)
        {
            return original.SubstringBefore(original.IndexOf(value));
        }

        /// <summary>
        /// Takes string value before specified string
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="value">Value to search for</param>
        /// <param name="comparisonType">Comparasion type</param>
        /// <returns>Returns first substring</returns>
        [Pure]
        public static string SubstringBefore(this string original, string value, StringComparison comparisonType)
        {
            return original.SubstringBefore(original.IndexOf(value, comparisonType));
        }

        /// <summary>
        /// Takes string value before specified string
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="value">Value to search for</param>
        /// <returns>Returns first substring</returns>
        [Pure]
        public static string SubstringBeforeLast(this string original, string value)
        {
            return original.SubstringBefore(original.LastIndexOf(value));
        }

        /// <summary>
        /// Takes string value before specified string
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="value">Value to search for</param>
        /// <param name="comparisonType">Comparasion type</param>
        /// <returns>Returns first substring</returns>
        [Pure]
        public static string SubstringBeforeLast(this string original, string value, StringComparison comparisonType)
        {
            return original.SubstringBefore(original.LastIndexOf(value, comparisonType));
        }

        /// <summary>
        /// Takes string value before specified index
        /// <para>Returns orginal text if index is less then 0</para>
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="index">Index to search for</param>
        /// <returns>Returns first substring</returns>
        private static string SubstringBefore(this string original, int index)
        {
            if (index < 0)
                return original;
            return original.Substring(0, index);
        }

        /// <summary>
        /// Takes string value after first occurance of the value
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="value">Value to search for</param>
        /// <returns>Returns first substring after index</returns>
        [Pure]
        public static string SubstringAfter(this string original, string value)
        {
            return original.SubstringAfter(original.IndexOf(value), value.Length);
        }

        /// <summary>
        /// Takes string value after first occurance of the value
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="value">Value to search for</param>
        /// <param name="comparisonType">Comparasion type</param>
        /// <returns>Returns first substring after index</returns>
        [Pure]
        public static string SubstringAfter(this string original, string value, StringComparison comparisonType)
        {
            return original.SubstringAfter(original.IndexOf(value, comparisonType), value.Length);
        }

        /// <summary>
        /// Takes string value after last occurance of the value
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="value">Value to search for</param>
        /// <returns>Returns first substring after index</returns>
        [Pure]
        public static string SubstringAfterLast(this string original, string value)
        {
            return original.SubstringAfter(original.LastIndexOf(value), value.Length);
        }

        /// <summary>
        /// Takes string value after last occurance of the value
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="value">Value to search for</param>
        /// <param name="comparisonType">Comparasion type</param>
        /// <returns>Returns first substring after index</returns>
        [Pure]
        public static string SubstringAfterLast(this string original, string value, StringComparison comparisonType)
        {
            return original.SubstringAfter(original.LastIndexOf(value, comparisonType), value.Length);
        }

        /// <summary>
        /// Takes string value after specified index
        /// <para>Returns orginal text if index is less then 0</para>
        /// </summary>
        /// <param name="original">Orginal value to take</param>
        /// <param name="index">Index to search for</param>
        /// <param name="length">Lenght of requested string</param>
        /// <returns>Returns first substring after index</returns>
        private static string SubstringAfter(this string original, int index, int length)
        {
            if (index < 0)
                return original;
            index += length;
            return original.Substring(index, original.Length - index);
        }

        /// <summary>
        /// Removes first occurance of the string withing original text
        /// </summary>
        /// <param name="original">Orginal text to look for prefix</param>
        /// <param name="prefix">Prefix to remove from original string</param>
        /// <returns>Returns substring with with removed prefix</returns>
        [Pure]
        public static string RemoveStart(this string original, string prefix)
        {
            if (!original.StartsWith(prefix))
                return original;
            return original.Substring(prefix.Length);
        }

        /// <summary>
        /// Removes last occurance of the string withing original text
        /// </summary>
        /// <param name="original">Orginal text to look for suffix</param>
        /// <param name="suffix">Suffix to remove from original string</param>
        /// <returns>Returns substring with with removed suffix</returns>
        [Pure]
        public static string RemoveEnd(this string original, string suffix)
        {
            if (!original.EndsWith(suffix))
                return original;
            return original.Substring(0, original.Length - suffix.Length);
        }

        /// <summary>
        /// Converts text to base 64 based on <see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="text">Value to convert from</param>
        /// <returns></returns>
        [Pure]
        public static string ToBase64(this string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// Converts text from base 64 based on <see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="text">Value to convert from</param>
        /// <returns></returns>
        [Pure]
        public static string FromBase64(this string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
