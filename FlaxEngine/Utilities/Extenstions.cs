////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlaxEngine.Utilities
{
    public static partial class Extenstions
    {
        /// <summary>
        /// Creates deep clone for a class if all members of this class are marked as serializable
        /// </summary>
        /// <param name="instance">Current instance of an object</param>
        /// <typeparam name="T">Instance type of an object</typeparam>
        /// <returns>Returns new object of provided class</returns>
        public static T DeepClone<T>(this T instance)
            where T : new()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, instance);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Splits string into lines
        /// </summary>
        /// <param name="str">Text to split</param>
        /// <param name="removeEmptyLines">True if remove empty lines, otherwise keep them</param>
        /// <returns>Array with all lines</returns>
        public static string[] GetLines(this string str, bool removeEmptyLines = false)
        {
            return str.Split(new[] { "\r\n", "\r", "\n" }, removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// Gets a random double.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <param name="maxValue">The maximum value</param>
        /// <returns>A random double</returns>
        public static double NextDouble(this Random random, double maxValue)
        {
            return random.NextDouble() * maxValue;
        }

        /// <summary>
        /// Gets a random double.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <returns>A random double</returns>
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Gets a random float.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns>A random float</returns>
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Gets a random float.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <param name="maxValue">The maximum value</param>
        /// <returns>A random float</returns>
        public static float NextFloat(this Random random, float maxValue)
        {
            return (float)random.NextDouble() * maxValue;
        }

        /// <summary>
        /// Gets a random float.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <param name="minValue">The minimum value</param>
        /// <param name="maxValue">The maximum value</param>
        /// <returns></returns>
        public static float NextFloat(this Random random, float minValue, float maxValue)
        {
            return (float)random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Gets a random Color.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns></returns>
        public static Color NextColor(this Random random)
        {
            return new Color(NextFloat(random, 1.0f),
                             NextFloat(random, 1.0f),
                             NextFloat(random, 1.0f),
                             NextFloat(random, 1.0f));
        }

        /// <summary>
        /// Gets a random 64-bit signed inteager value.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns></returns>
        internal static long NextLong(this Random random)
        {
            var numArray = new byte[8];
            random.NextBytes(numArray);
            return (long)(BitConverter.ToUInt64(numArray, 0) & 9223372036854775807L);
        }
    }
}
