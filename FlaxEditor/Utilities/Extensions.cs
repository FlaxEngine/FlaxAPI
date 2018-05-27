// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Helper methods used by the editor.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        ///     Gets a list of MemberComparison values that represent the fields and/or properties that differ between the two
        ///     objects.
        /// </summary>
        /// <typeparam name="T">Type of object to compare.</typeparam>
        /// <param name="first">First object to compare.</param>
        /// <param name="second">Second object to compare.</param>
        /// <returns>Returns list of <see cref="MemberComparison" /> structs with all different fields and properties.</returns>
        public static List<MemberComparison> ReflectiveCompare<T>(this T first, T second)
        {
            if (first.GetType() != second.GetType())
            {
                throw new ArgumentException("both first and second parameters has to be of the same type");
            }

            var list = new List<MemberComparison>(); //The list to be returned

            var members = first.GetType().GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            for (int i = 0; i < members.Length; i++)
            {
                var m = members[i];

                if (m.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)m;
                    var xValue = field.GetValue(first);
                    var yValue = field.GetValue(second);
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
                        var xValue = prop.GetValue(first, null);
                        var yValue = prop.GetValue(second, null);
                        if (!object.Equals(xValue, yValue))
                        {
                            list.Add(new MemberComparison(prop, xValue, yValue));
                        }
                    }
                    else
                    {
                        //Ignore properties that aren't readable or are indexers
                        continue;
                    }
                }
            }
            return list;
        }
    }
}
