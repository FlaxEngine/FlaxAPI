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

            // Note: this loop must match with Compare method

            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
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

                    FieldInfo field = (FieldInfo)m;
                    values.Add(field.GetValue(obj));
                }
                else if (m.MemberType == MemberTypes.Property)
                {
                    var prop = (PropertyInfo)m;
                    if (prop.CanRead && prop.GetGetMethod().GetParameters().Length == 0)
                    {
                        values.Add(prop.GetValue(obj, null));
                    }
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

            // Note: this loop must match with CaptureSnapshot method

            var members = ObjectType.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            int index = 0;
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

                    FieldInfo field = (FieldInfo)m;
                    var yValue = Values[index++];
                    var xValue = field.GetValue(obj);
                    if (!object.Equals(xValue, yValue))
                    {
                        //Add a new comparison to the list if the value of the member defined on 'first' isn't equal to the value of the member defined on 'second'.
                        list.Add(new MemberComparison(field, xValue, yValue));
                    }
                }
                else if (m.MemberType == MemberTypes.Property)
                {
                    var prop = (PropertyInfo)m;
                    if (prop.CanRead && prop.GetGetMethod().GetParameters().Length == 0)
                    {
                        var yValue = Values[index++];
                        var xValue = prop.GetValue(obj, null);
                        if (!object.Equals(xValue, yValue))
                        {
                            list.Add(new MemberComparison(prop, xValue, yValue));
                        }
                    }
                }
            }

            return list;
        }
    }
}
