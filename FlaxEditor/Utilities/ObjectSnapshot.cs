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
        /// <summary>
        /// The object type.
        /// </summary>
        public readonly Type ObjectType;

        /// <summary>
        /// The stored values.
        /// </summary>
        public readonly List<object> Values;

        private ObjectSnapshot(Type type, List<object> values)
        {
            ObjectType = type;
            Values = values;
        }

        private static List<MemberInfo> GetMembers(Type type)
        {
            // TODO: cache it per type

            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var result = new List<MemberInfo>(members.Length);

            for (int i = 0; i < members.Length; i++)
            {
                var m = members[i];

                var attributes = m.GetCustomAttributes(true);
                if (attributes.Any(x => x is NoSerializeAttribute))
                    continue;

                if (m.MemberType == MemberTypes.Field)
                {
                    if (attributes.Any(x => x is NonSerializedAttribute))
                        continue;

                    result.Add(m);
                }
                else if (m.MemberType == MemberTypes.Property)
                {
                    var prop = (PropertyInfo)m;
                    if (prop.CanRead && prop.CanWrite && prop.GetGetMethod().GetParameters().Length == 0)
                    {
                        result.Add(m);
                    }
                }
            }

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
            var values = new List<object>();

            var members = GetMembers(type);
            for (int i = 0; i < members.Count; i++)
            {
                var m = members[i];

                if (m is FieldInfo fieldInfo)
                {
                    values.Add(fieldInfo.GetValue(obj));
                }
                else
                {
                    values.Add(((PropertyInfo)m).GetValue(obj, null));
                }
            }

            return new ObjectSnapshot(type, values);
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

            var members = GetMembers(ObjectType);
            int index = 0;
            for (int i = 0; i < members.Count; i++)
            {
                var m = members[i];
                object xValue = Values[index++];
                object yValue;

                if (m is FieldInfo fieldInfo)
                {
                    yValue = fieldInfo.GetValue(obj);
                }
                else
                {
                    var propertyInfo = (PropertyInfo)m;
                    yValue = propertyInfo.GetValue(obj, null);
                }
                
                if (!Equals(xValue, yValue))
                {
                    list.Add(new MemberComparison(m, xValue, yValue));
                }
            }

            return list;
        }
    }
}
