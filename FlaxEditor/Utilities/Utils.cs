// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using FlaxEditor.SceneGraph;
using FlaxEngine;
using FlaxEngine.GUI;
using Newtonsoft.Json;
using Object = System.Object;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Editor utilities and helper functions.
    /// </summary>
    public static class Utils
    {
        private static readonly string[] MemorySizePostfixes =
        {
            "B",
            "kB",
            "MB",
            "GB",
            "TB",
            "PB"
        };

        /// <summary>
        /// Formats the amount of bytes to get a human-readable data size in bytes with abbreviation. Eg. 32 kB
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The formatted amount of bytes.</returns>
        public static string FormatBytesCount(int bytes)
        {
            int order = 0;
            while (bytes >= 1024 && order < MemorySizePostfixes.Length - 1)
            {
                order++;
                bytes = bytes / 1024;
            }

            return string.Format("{0:0.##} {1}", bytes, MemorySizePostfixes[order]);
        }

        /// <summary>
        /// Formats the amount of bytes to get a human-readable data size in bytes with abbreviation. Eg. 32 kB
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The formatted amount of bytes.</returns>
        public static string FormatBytesCount(ulong bytes)
        {
            int order = 0;
            while (bytes >= 1024 && order < MemorySizePostfixes.Length - 1)
            {
                order++;
                bytes = bytes / 1024;
            }

            return string.Format("{0:0.##} {1}", bytes, MemorySizePostfixes[order]);
        }

        /// <summary>
        /// The colors for the keyframes used by the curve editor.
        /// </summary>
        internal static readonly Color[] CurveKeyframesColors =
        {
            Color.OrangeRed,
            Color.ForestGreen,
            Color.CornflowerBlue,
            Color.White,
        };

        /// <summary>
        /// The time/value axes tick steps for editors with timeline.
        /// </summary>
        internal static readonly float[] CurveTickSteps =
        {
            0.0000001f, 0.0000005f, 0.000001f, 0.000005f, 0.00001f,
            0.00005f, 0.0001f, 0.0005f, 0.001f, 0.005f,
            0.01f, 0.05f, 0.1f, 0.5f, 1,
            5, 10, 50, 100, 500,
            1000, 5000, 10000, 50000, 100000,
            500000, 1000000, 5000000, 10000000, 100000000
        };

        /// <summary>
        /// Determines whether the specified path string contains any invalid character.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if the given string cannot be used as a path because it contains one or more illegal characters; otherwise, <c>false</c>.</returns>
        public static bool HasInvalidPathChar(string path)
        {
            char[] illegalChars =
            {
                '?',
                '\\',
                '/',
                '\"',
                '<',
                '>',
                '|',
                ':',
                '*',
                '\u0001',
                '\u0002',
                '\u0003',
                '\u0004',
                '\u0005',
                '\u0006',
                '\a',
                '\b',
                '\t',
                '\n',
                '\v',
                '\f',
                '\r',
                '\u000E',
                '\u000F',
                '\u0010',
                '\u0011',
                '\u0012',
                '\u0013',
                '\u0014',
                '\u0015',
                '\u0016',
                '\u0017',
                '\u0018',
                '\u0019',
                '\u001A',
                '\u001B',
                '\u001C',
                '\u001D',
                '\u001E',
                '\u001F'
            };
            return path.IndexOfAny(illegalChars) != -1;
        }

        internal static Transform[] GetTransformsAndBounds(List<SceneGraphNode> nodes, out BoundingBox bounds)
        {
            var transforms = new Transform[nodes.Count];
            bounds = BoundingBox.Empty;
            for (int i = 0; i < nodes.Count; i++)
            {
                transforms[i] = nodes[i].Transform;
                if (nodes[i] is ActorNode actorNode)
                {
                    bounds = BoundingBox.Merge(bounds, actorNode.Actor.BoxWithChildren);
                }
            }
            return transforms;
        }

        /// <summary>
        /// Gets the default value for the given type (can be value type or reference type).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The created instance.</returns>
        public static object GetDefaultValue(Type type)
        {
            if (type == typeof(string))
                return string.Empty;
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            if (typeof(Object).IsAssignableFrom(type))
                return null;
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Determines whether the given type comes from the game scripts assembly or the editor scripts assembly.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True if type is from gae or editor assembly, otherwise false.</returns>
        public static bool IsTypeFromGameScripts(Type type)
        {
            var name = type.Assembly.GetName().Name;
            return name == "Game" || name == "Game.Editor";
        }

        /// <summary>
        /// Gets all the derived types from the given base type (excluding that type) within the given assembly.
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
        /// Gets all the derived types from the given base type (excluding that type) within all the loaded assemblies.
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
        /// Gets all the derived types from the given base type (excluding that type) within the given assembly.
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
        /// Gets all the derived types from the given base type (excluding that type) within all the loaded assemblies.
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
        /// Gets all the derived types from the given base type (excluding that type) within all the loaded assemblies.
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
        /// Gets all the types that have the given attribute defined within the given assembly.
        /// </summary>
        /// <param name="assembly">The target assembly to check its types.</param>
        /// <param name="attributeType">The attribute type.</param>
        /// <param name="result">The result collection. Elements will be added to it. Clear it before usage.</param>
        /// <param name="checkFunc">Additional callback used to check if the given type is valid. Returns true if add type, otherwise false.</param>
        public static void GetTypesWithAttributeDefined(Assembly assembly, Type attributeType, List<Type> result, Func<Type, bool> checkFunc)
        {
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (Attribute.IsDefined(t, attributeType, false) && checkFunc(t))
                    result.Add(t);
            }
        }

        /// <summary>
        /// Gets all the types that have the given attribute defined within all the loaded assemblies.
        /// </summary>
        /// <param name="attributeType">The attribute type.</param>
        /// <param name="result">The result collection. Elements will be added to it. Clear it before usage.</param>
        /// <param name="checkFunc">Additional callback used to check if the given type is valid. Returns true if add type, otherwise false.</param>
        /// <param name="checkAssembly">Additional callback used to check if the given assembly is valid. Returns true if search for types in the given assembly, otherwise false.</param>
        public static void GetTypesWithAttributeDefined(Type attributeType, List<Type> result, Func<Type, bool> checkFunc, Func<Assembly, bool> checkAssembly)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                if (checkAssembly(assemblies[i]))
                    GetTypesWithAttributeDefined(assemblies[i], attributeType, result, checkFunc);
            }
        }

        /// <summary>
        /// Tries to get the object type from the given full typename. Searches in-build Flax Engine/Editor assemblies and game assemblies.
        /// </summary>
        /// <param name="typeName">The full name of the type.</param>
        /// <returns>The type or null if failed.</returns>
        public static Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

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
        /// Tries to create object instance of the given full typename. Searches in-build Flax Engine/Editor assemblies and game assemblies.
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

        /// <summary>
        /// Removes the file if it exists.
        /// </summary>
        /// <param name="file">The file path.</param>
        public static void RemoveFileIfExists(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }

        /// <summary>
        /// Copies the directory. Supports subdirectories copy with files override option.
        /// </summary>
        /// <param name="srcDirectoryPath">The source directory path.</param>
        /// <param name="dstDirectoryPath">The destination directory path.</param>
        /// <param name="copySubdirs">If set to <c>true</c> copy subdirectories.</param>
        /// <param name="overrideFiles">if set to <c>true</c> override existing files.</param>
        public static void DirectoryCopy(string srcDirectoryPath, string dstDirectoryPath, bool copySubdirs = true, bool overrideFiles = false)
        {
            var dir = new DirectoryInfo(srcDirectoryPath);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Missing source directory to copy. " + srcDirectoryPath);
            }

            if (!Directory.Exists(dstDirectoryPath))
            {
                Directory.CreateDirectory(dstDirectoryPath);
            }

            var files = dir.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                string tmp = Path.Combine(dstDirectoryPath, files[i].Name);
                files[i].CopyTo(tmp, overrideFiles);
            }

            if (copySubdirs)
            {
                var dirs = dir.GetDirectories();
                for (int i = 0; i < dirs.Length; i++)
                {
                    string tmp = Path.Combine(dstDirectoryPath, dirs[i].Name);
                    DirectoryCopy(dirs[i].FullName, tmp, true, overrideFiles);
                }
            }
        }

        /// <summary>
        /// Converts the raw bytes into the structure. Supported only for structures with simple types and no GC objects.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="bytes">The data bytes.</param>
        /// <returns>The structure.</returns>
        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            // #stupid c#
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }

        /// <summary>
        /// Converts the raw bytes into the structure. Supported only for structures with simple types and no GC objects.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="bytes">The data bytes.</param>
        /// <param name="result">The result.</param>
        public static void ByteArrayToStructure<T>(byte[] bytes, out T result) where T : struct
        {
            // #stupid c#
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
        }

        /// <summary>
        /// Converts the structure to the raw bytes. Supported only for structures with simple types and no GC objects.
        /// </summary>
        /// <typeparam name="T">The structure type.</typeparam>
        /// <param name="value">The structure value.</param>
        /// <returns>The bytes array that contains a structure data.</returns>
        public static byte[] StructureToByteArray<T>(ref T value) where T : struct
        {
            // #stupid c#
            int size = Marshal.SizeOf(typeof(T));
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        internal static unsafe string ReadStr(BinaryReader stream, int check)
        {
            int length = stream.ReadInt32();
            if (length > 0 && length < 2000)
            {
                var str = stream.ReadBytes(length * 2);
                fixed (byte* strPtr = str)
                {
                    var ptr = (char*)strPtr;
                    for (int j = 0; j < length; j++)
                        ptr[j] = (char)(ptr[j] ^ check);
                }
                return System.Text.Encoding.Unicode.GetString(str);
            }

            return string.Empty;
        }

        internal static unsafe void WriteStr(BinaryWriter stream, string str, int check)
        {
            int length = str.Length;
            stream.Write(length);
            var bytes = System.Text.Encoding.Unicode.GetBytes(str);
            if (bytes.Length != length * 2)
                throw new ArgumentException();
            fixed (byte* bytesPtr = bytes)
            {
                var ptr = (char*)bytesPtr;
                for (int j = 0; j < length; j++)
                    ptr[j] = (char)(ptr[j] ^ check);
            }
            stream.Write(bytes);
        }

        internal static Guid ReadGuid(this BinaryReader stream)
        {
            // TODO: use static bytes array to reduce dynamic allocs
            return new Guid(stream.ReadBytes(16));
        }

        internal static void WriteGuid(this BinaryWriter stream, ref Guid value)
        {
            // TODO: use static bytes array to reduce dynamic allocs
            stream.Write(value.ToByteArray());
        }

        internal static void ReadCommonValue(this BinaryReader stream, ref object value)
        {
            byte type = stream.ReadByte();

            switch (type)
            {
            case 0: // CommonType::Bool:
                value = stream.ReadByte() != 0;
                break;
            case 1: // CommonType::Integer:
            {
                value = stream.ReadInt32();
            }
                break;
            case 2: // CommonType::Float:
            {
                value = stream.ReadSingle();
            }
                break;
            case 3: // CommonType::Vector2:
            {
                value = stream.ReadVector2();
            }
                break;
            case 4: // CommonType::Vector3:
            {
                value = stream.ReadVector3();
            }
                break;
            case 5: // CommonType::Vector4:
            {
                value = stream.ReadVector4();
            }
                break;
            case 6: // CommonType::Color:
            {
                value = stream.ReadColor();
            }
                break;
            case 7: // CommonType::Guid:
            {
                value = stream.ReadGuid();
            }
                break;
            case 8: // CommonType::String:
            {
                int length = stream.ReadInt32();
                if (length <= 0)
                {
                    value = string.Empty;
                }
                else
                {
                    var data = new char[length];
                    for (int i = 0; i < length; i++)
                    {
                        var c = stream.ReadUInt16();
                        data[i] = (char)(c ^ 953);
                    }
                    value = new string(data);
                }
                break;
            }
            case 9: // CommonType::Box:
            {
                value = new BoundingBox(new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()),
                                        new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()));
            }
                break;
            case 10: // CommonType::Rotation:
            {
                value = stream.ReadQuaternion();
            }
                break;
            case 11: // CommonType::Transform:
            {
                value = new Transform(new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()),
                                      new Quaternion(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()),
                                      new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()));
            }
                break;
            case 12: // CommonType::Sphere:
            {
                value = new BoundingSphere(new Vector3(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle()),
                                           stream.ReadSingle());
            }
                break;
            case 13: // CommonType::Rect:
            {
                value = new Rectangle(new Vector2(stream.ReadSingle(), stream.ReadSingle()),
                                      new Vector2(stream.ReadSingle(), stream.ReadSingle()));
            }
                break;
            case 15: // CommonType::Matrix
            {
                value = new Matrix(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(),
                                   stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(),
                                   stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(),
                                   stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
                break;
            }
            case 16: // CommonType::Blob
            {
                int length = stream.ReadInt32();
                value = stream.ReadBytes(length);
                break;
            }
            default: throw new SystemException();
            }
        }

        internal static void WriteCommonValue(this BinaryWriter stream, object value)
        {
            if (value is bool asBool)
            {
                stream.Write((byte)0);
                stream.Write((byte)(asBool ? 1 : 0));
            }
            else if (value is int asInt)
            {
                stream.Write((byte)1);
                stream.Write(asInt);
            }
            else if (value is float asFloat)
            {
                stream.Write((byte)2);
                stream.Write(asFloat);
            }
            else if (value is double asDouble)
            {
                stream.Write((byte)2);
                stream.Write((float)asDouble);
            }
            else if (value is Vector2 asVector2)
            {
                stream.Write((byte)3);
                stream.Write(asVector2.X);
                stream.Write(asVector2.Y);
            }
            else if (value is Vector3 asVector3)
            {
                stream.Write((byte)4);
                stream.Write(asVector3.X);
                stream.Write(asVector3.Y);
                stream.Write(asVector3.Z);
            }
            else if (value is Vector4 asVector4)
            {
                stream.Write((byte)5);
                stream.Write(asVector4.X);
                stream.Write(asVector4.Y);
                stream.Write(asVector4.Z);
                stream.Write(asVector4.W);
            }
            else if (value is Color asColor)
            {
                stream.Write((byte)6);
                stream.Write(asColor.R);
                stream.Write(asColor.G);
                stream.Write(asColor.B);
                stream.Write(asColor.A);
            }
            else if (value is Guid asGuid)
            {
                stream.Write((byte)7);
                stream.WriteGuid(ref asGuid);
            }
            else if (value is string asString)
            {
                stream.Write((byte)8);
                stream.Write(asString.Length);
                for (int i = 0; i < asString.Length; i++)
                    stream.Write((ushort)(asString[i] ^ 953));
            }
            else if (value is BoundingBox asBox)
            {
                stream.Write((byte)9);
                stream.Write(asBox.Minimum.X);
                stream.Write(asBox.Minimum.Y);
                stream.Write(asBox.Minimum.Z);
                stream.Write(asBox.Maximum.X);
                stream.Write(asBox.Maximum.Y);
                stream.Write(asBox.Maximum.Z);
            }
            else if (value is Quaternion asRotation)
            {
                stream.Write((byte)10);
                stream.Write(asRotation.X);
                stream.Write(asRotation.Y);
                stream.Write(asRotation.Z);
                stream.Write(asRotation.X);
            }
            else if (value is Transform asTransform)
            {
                stream.Write((byte)11);
                stream.Write(asTransform.Translation.X);
                stream.Write(asTransform.Translation.Y);
                stream.Write(asTransform.Translation.Z);
                stream.Write(asTransform.Orientation.X);
                stream.Write(asTransform.Orientation.Y);
                stream.Write(asTransform.Orientation.Z);
                stream.Write(asTransform.Orientation.X);
                stream.Write(asTransform.Scale.X);
                stream.Write(asTransform.Scale.Y);
                stream.Write(asTransform.Scale.Z);
            }
            else if (value is BoundingSphere asSphere)
            {
                stream.Write((byte)12);
                stream.Write(asSphere.Center.X);
                stream.Write(asSphere.Center.Y);
                stream.Write(asSphere.Center.Z);
                stream.Write(asSphere.Radius);
            }
            else if (value is Rectangle asRect)
            {
                stream.Write((byte)13);
                stream.Write(asRect.Location.X);
                stream.Write(asRect.Location.Y);
                stream.Write(asRect.Size.X);
                stream.Write(asRect.Size.Y);
            }
            else if (value is Matrix asMatrix)
            {
                stream.Write((byte)15);
                stream.Write(asMatrix.M11);
                stream.Write(asMatrix.M12);
                stream.Write(asMatrix.M13);
                stream.Write(asMatrix.M14);
                stream.Write(asMatrix.M21);
                stream.Write(asMatrix.M22);
                stream.Write(asMatrix.M23);
                stream.Write(asMatrix.M24);
                stream.Write(asMatrix.M31);
                stream.Write(asMatrix.M32);
                stream.Write(asMatrix.M33);
                stream.Write(asMatrix.M34);
                stream.Write(asMatrix.M41);
                stream.Write(asMatrix.M42);
                stream.Write(asMatrix.M43);
                stream.Write(asMatrix.M44);
            }
            else if (value is byte[] asBlob)
            {
                stream.Write((byte)16);
                stream.Write(asBlob.Length);
                stream.Write(asBlob);
            }
            else
            {
                throw new NotSupportedException(string.Format("Invalid Common Value type {0}", value != null ? value.GetType().ToString() : "null"));
            }
        }

        internal static void WriteCommonValue(this JsonWriter stream, object value)
        {
            if (value is bool asBool)
            {
                stream.WriteValue(asBool);
            }
            else if (value is int asInt)
            {
                stream.WriteValue(asInt);
            }
            else if (value is float asFloat)
            {
                stream.WriteValue(asFloat);
            }
            else if (value is Vector2 asVector2)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("X");
                stream.WriteValue(asVector2.X);
                stream.WritePropertyName("Y");
                stream.WriteValue(asVector2.Y);
                stream.WriteEndObject();
            }
            else if (value is Vector3 asVector3)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("X");
                stream.WriteValue(asVector3.X);
                stream.WritePropertyName("Y");
                stream.WriteValue(asVector3.Y);
                stream.WritePropertyName("Z");
                stream.WriteValue(asVector3.Z);
                stream.WriteEndObject();
            }
            else if (value is Vector4 asVector4)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("X");
                stream.WriteValue(asVector4.X);
                stream.WritePropertyName("Y");
                stream.WriteValue(asVector4.Y);
                stream.WritePropertyName("Z");
                stream.WriteValue(asVector4.Z);
                stream.WritePropertyName("W");
                stream.WriteValue(asVector4.W);
                stream.WriteEndObject();
            }
            else if (value is Color asColor)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("R");
                stream.WriteValue(asColor.R);
                stream.WritePropertyName("G");
                stream.WriteValue(asColor.G);
                stream.WritePropertyName("B");
                stream.WriteValue(asColor.B);
                stream.WritePropertyName("A");
                stream.WriteValue(asColor.A);
                stream.WriteEndObject();
            }
            else if (value is Rectangle asRectangle)
            {
                stream.WriteStartObject();
                stream.WritePropertyName("Location");
                WriteCommonValue(stream, asRectangle.Location);
                stream.WritePropertyName("Size");
                WriteCommonValue(stream, asRectangle.Size);
                stream.WriteEndObject();
            }
            else if (value is Guid asGuid)
            {
                stream.WriteValue(asGuid);
            }
            else if (value is string asString)
            {
                stream.WriteValue(asString);
            }
            else if (value is Matrix asMatrix)
            {
                stream.WriteStartObject();

                stream.WritePropertyName("M11");
                stream.WriteValue(asMatrix.M11);
                stream.WritePropertyName("M12");
                stream.WriteValue(asMatrix.M12);
                stream.WritePropertyName("M13");
                stream.WriteValue(asMatrix.M13);
                stream.WritePropertyName("M14");
                stream.WriteValue(asMatrix.M14);

                stream.WritePropertyName("M21");
                stream.WriteValue(asMatrix.M21);
                stream.WritePropertyName("M22");
                stream.WriteValue(asMatrix.M22);
                stream.WritePropertyName("M23");
                stream.WriteValue(asMatrix.M23);
                stream.WritePropertyName("M24");
                stream.WriteValue(asMatrix.M24);

                stream.WritePropertyName("M31");
                stream.WriteValue(asMatrix.M31);
                stream.WritePropertyName("M32");
                stream.WriteValue(asMatrix.M32);
                stream.WritePropertyName("M33");
                stream.WriteValue(asMatrix.M33);
                stream.WritePropertyName("M34");
                stream.WriteValue(asMatrix.M34);

                stream.WritePropertyName("M41");
                stream.WriteValue(asMatrix.M41);
                stream.WritePropertyName("M42");
                stream.WriteValue(asMatrix.M42);
                stream.WritePropertyName("M43");
                stream.WriteValue(asMatrix.M43);
                stream.WritePropertyName("M44");
                stream.WriteValue(asMatrix.M44);

                stream.WriteEndObject();
            }
            else if (value is byte[] asBlob)
            {
                stream.WriteValue(Convert.ToBase64String(asBlob));
            }
            else
            {
                throw new NotSupportedException(string.Format("Invalid Common Value type {0}", value != null ? value.GetType().ToString() : "null"));
            }
        }

        /// <summary>
        /// Shows the source code window.
        /// </summary>
        /// <param name="material">The material asset.</param>
        public static void ShowSourceCode(string source, string title)
        {
            if (string.IsNullOrEmpty(source))
            {
                MessageBox.Show("No generated shader source code.", "No source.");
                return;
            }

            CreateWindowSettings settings = CreateWindowSettings.Default;
            settings.ActivateWhenFirstShown = true;
            settings.AllowMaximize = false;
            settings.AllowMinimize = false;
            settings.HasSizingFrame = false;
            settings.StartPosition = WindowStartPosition.CenterScreen;
            settings.Size = new Vector2(500, 600) * Platform.DpiScale;
            settings.Title = title;
            var dialog = Window.Create(settings);

            var copyButton = new Button(4, 4, 100);
            copyButton.Text = "Copy";
            copyButton.Clicked += () => Platform.ClipboardText = source;
            copyButton.Parent = dialog.GUI;

            var sourceTextBox = new TextBox(true, 2, copyButton.Bottom + 4, settings.Size.X - 4);
            sourceTextBox.Height = settings.Size.Y - sourceTextBox.Top - 2;
            sourceTextBox.Text = source;
            sourceTextBox.Parent = dialog.GUI;

            dialog.Show();
            dialog.Focus();
        }
    }
}
