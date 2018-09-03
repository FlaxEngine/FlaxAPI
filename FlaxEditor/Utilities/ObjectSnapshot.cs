// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlaxEngine;
using FlaxEngine.Json;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Helper class to gather object snapshots and compare them using reflection.
    /// </summary>
    public sealed class ObjectSnapshot
    {
        private readonly List<TypeEntry> _members;
        private readonly List<object> _values;

        /// <summary>
        /// The object type.
        /// </summary>
        public readonly Type ObjectType;

        private ObjectSnapshot(Type type, List<object> values, List<TypeEntry> members)
        {
            ObjectType = type;
            _values = values;
            _members = members;
        }

        private struct TypeEntry
        {
            public MemberInfoPath Path;
            public int SubEntriesCount;

            public TypeEntry(MemberInfoPath membersPath, int subEntriesCount)
            {
                Path = membersPath;
                SubEntriesCount = subEntriesCount;
            }
        }

        private static Type[] _attributesIgnoreList =
        {
            typeof(NonSerializedAttribute),
            typeof(NoSerializeAttribute)
        };

        private static void GetEntries(MemberInfoPath.Entry member, Stack<MemberInfoPath.Entry> membersPath, Type type, List<TypeEntry> result, List<object> values, Stack<object> refStack, Type memberType, object memberValue)
        {
            membersPath.Push(member);
            var path = new MemberInfoPath(membersPath);
            var beforeCount = result.Count;

            // Check if record object sub members (skip flax objects)
            // It's used for ref types bu not null types and with checking cyclic references
            if ((memberType.IsClass || memberType.IsArray)
                && memberValue != null
                && !refStack.Contains(memberValue))
            {
                if (memberType.IsArray && !typeof(FlaxEngine.Object).IsAssignableFrom(memberType.GetElementType()))
                {
                    var array = (Array)memberValue;
                    var elementType = memberType.GetElementType();
                    var length = array.Length;

                    refStack.Push(memberValue);
                    for (int i = 0; i < length; i++)
                    {
                        var elementValue = array.GetValue(i);
                        GetEntries(new MemberInfoPath.Entry(member.Member, i), membersPath, type, result, values, refStack, elementType, elementValue);
                    }
                    refStack.Pop();
                }
                else if (memberType.IsClass && !typeof(FlaxEngine.Object).IsAssignableFrom(memberType))
                {
                    refStack.Push(memberValue);
                    GetEntries(memberValue, membersPath, memberType, result, values, refStack);
                    refStack.Pop();
                }
            }

            var afterCount = result.Count;
            result.Add(new TypeEntry(path, afterCount - beforeCount));
            values.Add(memberValue);
            membersPath.Pop();
        }

        private static void GetEntries(object instance, Stack<MemberInfoPath.Entry> membersPath, Type type, List<TypeEntry> result, List<object> values, Stack<object> refStack)
        {
            // Note: this should match Flax serialization rules and attributes (see ExtendedDefaultContractResolver)

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < fields.Length; i++)
            {
                var f = fields[i];
                var attributes = f.GetCustomAttributes();

                // Serialize non-public fields only with a proper attribute
                if (!f.IsPublic && !attributes.Any(x => x is SerializeAttribute))
                    continue;

                // Check if has attribute to skip serialization
                bool noSerialize = false;
                foreach (var attribute in attributes)
                {
                    if (_attributesIgnoreList.Contains(attribute.GetType()))
                    {
                        noSerialize = true;
                        break;
                    }
                }
                if (noSerialize)
                    continue;

                var memberType = f.FieldType;
                var memberValue = f.GetValue(instance);
                GetEntries(new MemberInfoPath.Entry(f), membersPath, type, result, values, refStack, memberType, memberValue);
            }

            for (int i = 0; i < properties.Length; i++)
            {
                var p = properties[i];

                // Serialize only properties with read/write
                if (!(p.CanRead && p.CanWrite && p.GetIndexParameters().GetLength(0) == 0))
                    continue;

                var attributes = p.GetCustomAttributes();

                // Check if has attribute to skip serialization
                bool noSerialize = false;
                foreach (var attribute in attributes)
                {
                    if (_attributesIgnoreList.Contains(attribute.GetType()))
                    {
                        noSerialize = true;
                        break;
                    }
                }
                if (noSerialize)
                    continue;

                var memberType = p.PropertyType;
                var memberValue = p.GetValue(instance, null);
                GetEntries(new MemberInfoPath.Entry(p), membersPath, type, result, values, refStack, memberType, memberValue);
            }
        }

        private static List<TypeEntry> GetMembers(object instance, Type type, out List<object> values)
        {
            values = new List<object>();
            var result = new List<TypeEntry>();
            var membersPath = new Stack<MemberInfoPath.Entry>(8);
            var refsStack = new Stack<object>(8);

            refsStack.Push(instance);
            GetEntries(instance, membersPath, type, result, values, refsStack);
            return result;
        }

        /// <summary>
        /// Captures the snapshot of the object.
        /// </summary>
        /// <param name="obj">The object to capture.</param>
        /// <returns>The object snapshot.</returns>
        public static ObjectSnapshot CaptureSnapshot(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            var type = obj.GetType();

            List<object> values;
            var members = GetMembers(obj, type, out values);

            //Debug.Log("-------------- CaptureSnapshot:  " + obj.GetType() + "  --------------");
            //for (int i = 0; i < values.Count; i++)
            //    Debug.Log(members[i].Path.Path + " =  " + (values[i] ?? "<null>"));

            return new ObjectSnapshot(type, values, members);
        }

        /// <summary>
        /// Gets a list of MemberComparison values that represent the fields and/or properties
        /// that differ between the given object and the captured state.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>The collection of modified properties.</returns>
        public List<MemberComparison> Compare(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException();
            if (ObjectType != obj.GetType())
                throw new ArgumentException("Given object must be the same type as captured object.");

            var list = new List<MemberComparison>();

            for (int i = _members.Count - 1; i >= 0; i--)
            {
                var m = _members[i];
                object xValue = _values[i];
                object yValue = m.Path.GetLastValue(obj);

                if (!JsonSerializer.ValueEquals(xValue, yValue))
                {
                    //Debug.Log("Diff on: " + (new MemberComparison(m.Path, xValue, yValue)));

                    list.Add(new MemberComparison(m.Path, xValue, yValue));

                    // Value changed, skip sub entries compare
                    i -= m.SubEntriesCount;
                }
            }

            return list;
        }
    }
}
