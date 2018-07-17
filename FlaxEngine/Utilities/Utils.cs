// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FlaxEngine
{
    /// <summary>
    /// Class with helper functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Rounds the floating point value up to 1 decimal place.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rounded result.</returns>
        public static float RoundTo1DecimalPlace(float value)
        {
            return (float)Math.Round(value * 10) / 10;
        }

        /// <summary>
        /// Rounds the floating point value up to 2 decimal places.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rounded result.</returns>
        public static float RoundTo2DecimalPlaces(float value)
        {
            return (float)Math.Round(value * 100) / 100;
        }

        /// <summary>
        /// Rounds the floating point value up to 3 decimal places.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The rounded result.</returns>
        public static float RoundTo3DecimalPlaces(float value)
        {
            return (float)Math.Round(value * 1000) / 1000;
        }

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

        /// <summary>
        /// Determines whether two arrays are equal by comparing the elements by using the default equality comparer for their type.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="a1">The first array.</param>
        /// <param name="a2">The second array.</param>
        /// <returns><c>true</c> if the two source sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type; otherwise, <c>false</c>.</returns>
        public static bool ArraysEqual<T>(T[] a1, List<T> a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Count)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the assembly with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The assembly or null if not found.</returns>
        public static Assembly GetAssemblyByName(string name)
        {
            return GetAssemblyByName(name, AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Gets the assembly with the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="assemblies">The assemblies collection to search for.</param>
        /// <returns>The assembly or null if not found.</returns>
        public static Assembly GetAssemblyByName(string name, Assembly[] assemblies)
        {
            Assembly result = null;
            for (int i = 0; i < assemblies.Length; i++)
            {
                var assemblyName = assemblies[i].GetName();
                if (assemblyName.Name == name)
                {
                    result = assemblies[i];
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}
