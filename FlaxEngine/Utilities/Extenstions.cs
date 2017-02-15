////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
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
        public static T DeepClone <T>(this T instance)
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
            return str.Split(new[] {"\r\n", "\r", "\n"}, removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }
    }
}
