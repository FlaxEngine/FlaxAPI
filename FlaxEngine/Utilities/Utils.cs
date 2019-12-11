// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FlaxEngine
{
    /// <summary>
    /// Class with helper functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Copies data from one memory location to another using an unmanaged memory pointers.
        /// </summary>
        /// <remarks>
        /// Uses low-level memcpy call.
        /// </remarks>
        /// <param name="source">The source location.</param>
        /// <param name="destination">The destination location.</param>
        /// <param name="length">The length (amount of bytes to copy).</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void MemoryCopy(IntPtr source, IntPtr destination, int length);

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
        /// Gets the empty array of the given type (shared one).
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The empty array object.</returns>
        public static T[] GetEmptyArray<T>()
        {
            return Enumerable.Empty<T>() as T[];
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
        public static bool ArraysEqual<T>(T[] a1, IReadOnlyList<T> a2)
        {
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
        /// Determines whether two arrays are equal by comparing the elements by using the default equality comparer for their type.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="a1">The first array.</param>
        /// <param name="a2">The second array.</param>
        /// <returns><c>true</c> if the two source sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type; otherwise, <c>false</c>.</returns>
        public static bool ArraysEqual<T>(List<T> a1, List<T> a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Count != a2.Count)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Count; i++)
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

        internal static T[] ExtractArrayFromList<T>(List<T> list)
        {
            T[] result = null;
            if (list != null)
            {
                // TODO: move it to the native code
                var field = list.GetType().GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
                result = (T[])field.GetValue(list);
            }
            return result;
        }

        /// <summary>
        /// Reads the color from the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The value.</returns>
        public static Color32 ReadColor32(this BinaryReader stream)
        {
            return new Color32(stream.ReadByte(), stream.ReadByte(), stream.ReadByte(), stream.ReadByte());
        }

        /// <summary>
        /// Reads the color from the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The value.</returns>
        public static Color ReadColor(this BinaryReader stream)
        {
            return new Color(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
        }

        /// <summary>
        /// Reads the Vector2 from the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The value.</returns>
        public static Vector2 ReadVector2(this BinaryReader stream)
        {
            return new Vector2(stream.ReadSingle(), stream.ReadSingle());
        }

        /// <summary>
        /// Reads the Vector3 from the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The value.</returns>
        public static Vector3 ReadVector3(this BinaryReader stream)
        {
            return new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
        }

        /// <summary>
        /// Reads the Vector4 from the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The value.</returns>
        public static Vector4 ReadVector4(this BinaryReader stream)
        {
            return new Vector4(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
        }

        /// <summary>
        /// Reads the Quaternion from the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The value.</returns>
        public static Quaternion ReadQuaternion(this BinaryReader stream)
        {
            return new Quaternion(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
        }

        /// <summary>
        /// Writes the color to the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter stream, Color32 value)
        {
            stream.Write(value.R);
            stream.Write(value.G);
            stream.Write(value.B);
            stream.Write(value.A);
        }

        /// <summary>
        /// Writes the color to the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter stream, Color value)
        {
            stream.Write(value.R);
            stream.Write(value.G);
            stream.Write(value.B);
            stream.Write(value.A);
        }

        /// <summary>
        /// Writes the Vector2 to the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter stream, Vector2 value)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
        }

        /// <summary>
        /// Writes the Vector3 to the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter stream, Vector3 value)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
            stream.Write(value.Z);
        }

        /// <summary>
        /// Writes the Vector4 to the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter stream, Vector4 value)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
            stream.Write(value.Z);
            stream.Write(value.W);
        }

        /// <summary>
        /// Writes the Quaternion to the binary stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this BinaryWriter stream, Quaternion value)
        {
            stream.Write(value.X);
            stream.Write(value.Y);
            stream.Write(value.Z);
            stream.Write(value.W);
        }
    }
}
