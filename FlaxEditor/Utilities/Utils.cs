////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;
using FlaxEngine.Json;
using Object = System.Object;

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
            if (typeof(Object).IsAssignableFrom(type))
                return null;
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Tries to create object instance of the given full typename. Searches in-build Flax Engine/Editor asssemblies and game assemblies.
        /// </summary>
        /// <param name="typeName">The full name of the type.</param>
        /// <returns>The create object or null if failed.</returns>
        public static object CreateInstance(string typeName)
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblies = new[]
            {
                FlaxEngine.Utils.GetAssemblyByName("Assembly", allAssemblies),
                FlaxEngine.Utils.GetAssemblyByName("Assembly.Editor", allAssemblies),
                FlaxEngine.Utils.GetAssemblyByName("FlaxEditor", allAssemblies),
                FlaxEngine.Utils.GetAssemblyByName("FlaxEngine", allAssemblies),
            };

            for (int i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                if (assembly != null)
                {
                    var type = assembly.GetType(typeName);
                    if (type != null)
                    {
                        object obj = null;
                        try
                        {
                            // Create instance
                            return obj = Activator.CreateInstance(type);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }

                        return obj;
                    }
                }
            }
            return null;
        }
    }
}
