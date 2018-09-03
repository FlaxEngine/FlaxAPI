// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Helper class used to store path made of <see cref="MemberInfo"/>.
    /// </summary>
    public struct MemberInfoPath
    {
        /// <summary>
        /// The path entry.
        /// </summary>
        public struct Entry
        {
            /// <summary>
            /// The member.
            /// </summary>
            public readonly MemberInfo Member;

            /// <summary>
            /// The array index.
            /// </summary>
            public readonly int Index;

            /// <summary>
            /// Gets the member type (field or property type).
            /// </summary>
            /// <value>
            /// The type.
            /// </value>
            public Type Type
            {
                get
                {
                    Type result;
                    if (Member is FieldInfo fieldInfo)
                        result = fieldInfo.FieldType;
                    else
                        result = ((PropertyInfo)Member).PropertyType;

                    // Special case for arrays
                    if (Index != -1)
                        result = result.GetElementType();

                    return result;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> struct.
            /// </summary>
            /// <param name="member">The member.</param>
            /// <param name="index">The array index.</param>
            public Entry(MemberInfo member, int index = -1)
            {
                Member = member;
                Index = index;
            }

            /// <summary>
            /// Gets the value. Handles arrays.
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <returns>The result value.</returns>
            public object GetValue(object instance)
            {
                object value;

                // Special case for arrays
                if (Index != -1)
                {
                    // Get array value at index
                    var array = (Array)instance;
                    value = array.GetValue(Index);
                }
                else
                {
                    // Get value
                    if (Member is FieldInfo fieldInfo)
                        value = fieldInfo.GetValue(instance);
                    else
                        value = ((PropertyInfo)Member).GetValue(instance, null);
                }

                return value;
            }

            /// <summary>
            /// Sets the value.
            /// </summary>
            /// <param name="instance">The instance.</param>
            /// <param name="value">The value.</param>
            public void SetValue(object instance, object value)
            {
                // Special case for arrays
                if (Index != -1)
                {
                    // Set array value at index
                    var array = (Array)instance;
                    array.SetValue(value, Index);
                }
                else
                {
                    // Set value
                    if (Member is FieldInfo fieldInfo)
                        fieldInfo.SetValue(instance, value);
                    else
                        ((PropertyInfo)Member).SetValue(instance, value);
                }
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (!(obj is Entry))
                {
                    return false;
                }

                var entry = (Entry)obj;
                return EqualityComparer<MemberInfo>.Default.Equals(Member, entry.Member) && Index == entry.Index;
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                var hashCode = 2005110182;
                hashCode = hashCode * -1521134295 + EqualityComparer<MemberInfo>.Default.GetHashCode(Member);
                hashCode = hashCode * -1521134295 + Index.GetHashCode();
                return hashCode;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                if (Index != -1)
                    return "[" + Index + "]";
                return Member.Name;
            }
        }

        private Entry[] _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInfoPath"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="index">The array index.</param>
        public MemberInfoPath(MemberInfo member, int index = -1)
        {
            if (member == null)
                throw new ArgumentNullException();
            _stack = new Entry[1];
            _stack[0] = new Entry(member, index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInfoPath"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public MemberInfoPath(Stack<Entry> members)
        : this()
        {
            if (members == null || members.Count == 0)
                throw new ArgumentNullException();

            _stack = members.ToArray();
            Array.Reverse(_stack);
        }

        /// <summary>
        /// Gets the members path string.
        /// </summary>
        public string Path
        {
            get
            {
                string result = _stack[0].ToString();
                for (int i = 1; i < _stack.Length; i++)
                {
                    result += "." + _stack[i];
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the last member (returns it) and the instance (by the ref parameter).
        /// </summary>
        /// <param name="instance">The instance. Also contains the result instance for the last member.</param>
        /// <returns>The last member info.</returns>
        public Entry GetLastMember(ref object instance)
        {
            Entry finalMember = _stack[0];
            for (int i = 1; i < _stack.Length; i++)
            {
                instance = finalMember.GetValue(instance);
                finalMember = _stack[i];
            }
            return finalMember;
        }

        /// <summary>
        /// Gets the last member value.
        /// </summary>
        /// <param name="instance">The top object instance.</param>
        /// <returns>The result value.</returns>
        public object GetLastValue(object instance)
        {
            var member = GetLastMember(ref instance);
            return member.GetValue(instance);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Path;
        }
    }
}
