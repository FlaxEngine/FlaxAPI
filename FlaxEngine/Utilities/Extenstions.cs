// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlaxEngine.Utilities
{
    public static partial class Extenstions
    {
        /// <summary>
        /// Creates deep clone for a class if all members of this class are marked as serializable (uses Json serialization).
        /// </summary>
        /// <param name="instance">Current instance of an object</param>
        /// <typeparam name="T">Instance type of an object</typeparam>
        /// <returns>Returns new object of provided class</returns>
        public static T DeepClone<T>(this T instance)
        where T : new()
        {
            Type type = typeof(T);

            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(Json.JsonSerializer.Settings);
            jsonSerializer.Formatting = Formatting.Indented;

            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                // Prepare writer settings
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;
                jsonWriter.Formatting = jsonSerializer.Formatting;
                jsonWriter.DateFormatHandling = jsonSerializer.DateFormatHandling;
                jsonWriter.DateTimeZoneHandling = jsonSerializer.DateTimeZoneHandling;
                jsonWriter.FloatFormatHandling = jsonSerializer.FloatFormatHandling;
                jsonWriter.StringEscapeHandling = jsonSerializer.StringEscapeHandling;
                jsonWriter.Culture = jsonSerializer.Culture;
                jsonWriter.DateFormatString = jsonSerializer.DateFormatString;

                JsonSerializerInternalWriter serializerWriter = new JsonSerializerInternalWriter(jsonSerializer);

                serializerWriter.Serialize(jsonWriter, instance, type);
            }

            return JsonConvert.DeserializeObject<T>(sb.ToString());
        }

        /// <summary>
        /// Splits string into lines
        /// </summary>
        /// <param name="str">Text to split</param>
        /// <param name="removeEmptyLines">True if remove empty lines, otherwise keep them</param>
        /// <returns>Array with all lines</returns>
        public static string[] GetLines(this string str, bool removeEmptyLines = false)
        {
            return str.Split(new[]
            {
                "\r\n",
                "\r",
                "\n"
            }, removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
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
        /// Gets a random Vector2 with components in range [0;1].
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns></returns>
        public static Vector2 NextVector2(this Random random)
        {
            return new Vector2(NextFloat(random, 1.0f),
                               NextFloat(random, 1.0f));
        }

        /// <summary>
        /// Gets a random Vector3 with components in range [0;1].
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns></returns>
        public static Vector3 NextVector3(this Random random)
        {
            return new Vector3(NextFloat(random, 1.0f),
                               NextFloat(random, 1.0f),
                               NextFloat(random, 1.0f));
        }

        /// <summary>
        /// Gets a random Vector4 with components in range [0;1].
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns></returns>
        public static Vector4 NextVector4(this Random random)
        {
            return new Vector4(NextFloat(random, 1.0f),
                               NextFloat(random, 1.0f),
                               NextFloat(random, 1.0f),
                               NextFloat(random, 1.0f));
        }

        /// <summary>
        /// Gets a random Quaternion.
        /// </summary>
        /// <param name="random">The random.</param>
        /// <returns></returns>
        public static Quaternion NextQuaternion(this Random random)
        {
            return Quaternion.Euler(NextFloat(random, -180, 180),
                                    NextFloat(random, -180, 180),
                                    NextFloat(random, -180, 180));
        }

        /// <summary>
        /// Gets a random 64-bit signed integer value.
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
