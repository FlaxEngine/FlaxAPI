////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Editor utilities and helper functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Formats the amount of bytes to get a human-readable data size in bytes with abbreviation. Eg. 32 kB
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The formated amount of bytes.</returns>
        public static string FormatBytesCount(ulong bytes)
        {
            string[] sizes = { "B", "kB", "MB", "GB", "TB", "PB" };

            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes = bytes / 1024;
            }

            return string.Format("{0:0.##} {1}", bytes, sizes[order]);
        }

        /// <summary>
        /// Gets the default value for the given type (can be value type or reference type).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The created instace.</returns>
        public static object GetDefaultValue(Type type)
        {
            if (type == typeof(string))
                return String.Empty;
            if (type.IsValueType)
            {
                if (type == typeof(int))
                    return 0;
                return null;
            }
            return Activator.CreateInstance(type);
        }
    }
}
