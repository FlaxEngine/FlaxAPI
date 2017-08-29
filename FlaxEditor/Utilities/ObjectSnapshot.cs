////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlaxEngine;

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

        private static void GetEntries(object instance, Stack<MemberInfo> membersPath, Type type, List<TypeEntry> result, List<object> values, Stack<object> refStack)
        {
            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            for (int i = 0; i < members.Length; i++)
            {
                var member = members[i];

                var attributes = member.GetCustomAttributes(true);
                if (attributes.Any(x => x is NoSerializeAttribute))
                    continue;

                Type memberType;
                object memberValue;
                if (member is FieldInfo fieldInfo)
                {
                    if (attributes.Any(x => x is NonSerializedAttribute))
                        continue;

                    memberType = fieldInfo.FieldType;
                    memberValue = fieldInfo.GetValue(instance);
                }
                else if (member is PropertyInfo propertyInfo)
                {
                    continue;

                    if (!propertyInfo.CanRead || !propertyInfo.CanWrite || propertyInfo.GetGetMethod().GetParameters().Length != 0)
                        continue;

                    memberType = propertyInfo.PropertyType;
                    memberValue = propertyInfo.GetValue(instance, null);
                }
                else
                {
                    continue;
                }

                membersPath.Push(member);
                var path = new MemberInfoPath(membersPath);
                var beforeCount = result.Count;

                // Check if record object sub members (skip flax objects)
                // It's used for ref types bu not null types and with checking cyclic references
                if (memberType.IsClass
                    && !typeof(FlaxEngine.Object).IsAssignableFrom(memberType)
                    && memberValue != null
                    && !refStack.Contains(memberValue))
                {
                    refStack.Push(memberValue);
                    GetEntries(memberValue, membersPath, memberType, result, values, refStack);
                    refStack.Pop();
                }

                var afterCount = result.Count;
                result.Add(new TypeEntry(path, afterCount - beforeCount));
                values.Add(memberValue);
                membersPath.Pop();
            }
        }

        private static List<TypeEntry> GetMembers(object instance, Type type, out List<object> values)
        {
            values = new List<object>();
            var result = new List<TypeEntry>();
            var membersPath = new Stack<MemberInfo>(8);
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
            
            return new ObjectSnapshot(type, values, members);
        }

        /// <summary>
        /// Gets a list of MemberComparison values that represent the fields and/or properties
        /// that differbetween the given object and the captured state.
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

                if (!Equals(xValue, yValue))
                {
                    list.Add(new MemberComparison(m.Path, xValue, yValue));

                    // Value changed, skip sub entries compare
                    i -= m.SubEntriesCount;
                }
            }
            
            return list;
        }
    }
}
