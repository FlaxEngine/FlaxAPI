// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using FlaxEngine;
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
            string[] sizes =
            {
                "B",
                "kB",
                "MB",
                "GB",
                "TB",
                "PB"
            };

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
        /// Gets all the derived types from the given base type (expluding that type) within the given assembly.
        /// </summary>
        /// <param name="assembly">The target assembly to check its types.</param>
        /// <param name="baseType">The base type.</param>
        /// <param name="result">The result collection. Elements will be added to it. Clear it before usage.</param>
        public static void GetDerivedTypes(Assembly assembly, Type baseType, List<Type> result)
        {
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (t.IsAssignableFrom(t) && t != baseType)
                    result.Add(t);
            }
        }

        /// <summary>
        /// Gets all the derived types from the given base type (expluding that type) within all the loaded assemblies.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="result">The result collection. Elements will be added to it. Clear it before usage.</param>
        public static void GetDerivedTypes(Type baseType, List<Type> result)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                GetDerivedTypes(assemblies[i], baseType, result);
            }
        }

        /// <summary>
        /// Gets all the derived types from the given base type (expluding that type) within the given assembly.
        /// </summary>
        /// <param name="assembly">The target assembly to check its types.</param>
        /// <param name="baseType">The base type.</param>
        /// <param name="result">The result collection. Elements will be added to it. Clear it before usage.</param>
        /// <param name="checkFunc">Additional callback used to check if the given type is valid. Returns true if add type, otherwise false.</param>
        public static void GetDerivedTypes(Assembly assembly, Type baseType, List<Type> result, Func<Type, bool> checkFunc)
        {
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (baseType.IsAssignableFrom(t) && t != baseType && checkFunc(t))
                    result.Add(t);
            }
        }

        /// <summary>
        /// Gets all the derived types from the given base type (expluding that type) within all the loaded assemblies.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="result">The result collection. Elements will be added to it. Clear it before usage.</param>
        /// <param name="checkFunc">Additional callback used to check if the given type is valid. Returns true if add type, otherwise false.</param>
        public static void GetDerivedTypes(Type baseType, List<Type> result, Func<Type, bool> checkFunc)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                GetDerivedTypes(assemblies[i], baseType, result, checkFunc);
            }
        }

        /// <summary>
        /// Gets all the derived types from the given base type (expluding that type) within all the loaded assemblies.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="result">The result collection. Elements will be added to it. Clear it before usage.</param>
        /// <param name="checkFunc">Additional callback used to check if the given type is valid. Returns true if add type, otherwise false.</param>
        /// <param name="checkAssembly">Additional callback used to check if the given assembly is valid. Returns true if search for types in the given assembly, otherwise false.</param>
        public static void GetDerivedTypes(Type baseType, List<Type> result, Func<Type, bool> checkFunc, Func<Assembly, bool> checkAssembly)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                if (checkAssembly(assemblies[i]))
                    GetDerivedTypes(assemblies[i], baseType, result, checkFunc);
            }
        }

        /// <summary>
        /// Tries to get the object type from the given full typename. Searches in-build Flax Engine/Editor asssemblies and game assemblies.
        /// </summary>
        /// <param name="typeName">The full name of the type.</param>
        /// <returns>The type or null if failed.</returns>
        public static Type GetType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                if (assembly != null)
                {
                    var type = assembly.GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the type of the base class for the given content domain. Used by the editor internal layer to convert static enum to the runtime asset type.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <returns>The asset object type.</returns>
        public static Type GetType(ContentDomain domain)
        {
            switch (domain)
            {
            case ContentDomain.Texture: return typeof(Texture);
            case ContentDomain.CubeTexture: return typeof(CubeTexture);
            case ContentDomain.Material: return typeof(MaterialBase);
            case ContentDomain.Model: return typeof(Model);
            case ContentDomain.Shader: return typeof(Shader);
            case ContentDomain.Font: return typeof(FontAsset);
            case ContentDomain.IESProfile: return typeof(IESProfile);
            case ContentDomain.Document: return typeof(JsonAsset);
            case ContentDomain.Audio: return typeof(AudioClip);
            case ContentDomain.Animation: return typeof(Animation);
            case ContentDomain.SkeletonMask: return typeof(SkeletonMask);
            }

            // Anything
            return typeof(Asset);
        }

        /// <summary>
        /// Tries to create object instance of the given full typename. Searches in-build Flax Engine/Editor asssemblies and game assemblies.
        /// </summary>
        /// <param name="typeName">The full name of the type.</param>
        /// <returns>The created object or null if failed.</returns>
        public static object CreateInstance(string typeName)
        {
            var type = GetType(typeName);
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

            return null;
        }
    }
}
