////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlaxEditor.Utilities
{
    /// <summary>
    /// Helper class used to store path made of <see cref="MemberInfo"/>.
    /// </summary>
    public struct MemberInfoPath
    {
        // Store the first item without array to reduce allocations
        private MemberInfo _top;

        private MemberInfo[] _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInfoPath"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        public MemberInfoPath(MemberInfo member)
        {
            _top = member ?? throw new ArgumentNullException();
            _stack = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInfoPath"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public MemberInfoPath(MemberInfo[] members)
        {
            if (members == null || members.Length == 0)
                throw new ArgumentNullException();

            _top = members[0];
            int membersLeft = members.Length - 1;
            if (membersLeft > 0)
            {
                _stack = new MemberInfo[membersLeft];
                for (int i = 0; i < membersLeft; i++)
                {
                    _stack[i] = members[i + 1];
                }
            }
            else
            {
                _stack = null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInfoPath"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public MemberInfoPath(Stack<MemberInfo> members)
            : this()
        {
            if (members == null || members.Count == 0)
                throw new ArgumentNullException();

            var membersData = members.ToArray();
            Array.Reverse(membersData);

            _top = membersData[0];
            int membersLeft = members.Count - 1;
            if (membersLeft > 0)
            {
                _stack = new MemberInfo[membersLeft];
                for (int i = 0; i < membersLeft; i++)
                {
                    _stack[i] = membersData[i + 1];
                }
            }
            else
            {
                _stack = null;
            }
        }
        
        /// <summary>
        /// Gets the members path string.
        /// </summary>
        public string Path
        {
            get
            {
                string result = _top.Name;
                if (_stack != null)
                {
                    for (int i = 0; i < _stack.Length; i++)
                    {
                        result += '.' + _stack[i].Name;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the last member (returns it) and the instance (by the ref parameter).
        /// </summary>
        /// <param name="instance">The instance. Also contains the result instance for the last member.</param>
        /// <returns>The last member info.</returns>
        public MemberInfo GetLastMember(ref object instance)
        {
            MemberInfo finalMember = _top;
            if (_stack != null)
            {
                for (int i = 0; i < _stack.Length; i++)
                {
                    if (finalMember is FieldInfo fieldInfo)
                        instance = fieldInfo.GetValue(instance);
                    else
                        instance = ((PropertyInfo)finalMember).GetValue(instance, null);
                    finalMember = _stack[i];
                }
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

            if (member is FieldInfo fieldInfo)
                return fieldInfo.GetValue(instance);
            return ((PropertyInfo)member).GetValue(instance, null);
        }
    }
}
