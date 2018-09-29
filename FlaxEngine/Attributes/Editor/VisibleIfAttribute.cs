// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Shows property/field in the editor only if the specified member has a given value. Can be used to hide properties based on other properties (also private properties). The given member has to be bool type.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class VisibleIfAttribute : Attribute
    {
        /// <summary>
        /// The member name.
        /// </summary>
        public string MemberName;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderAttribute"/> class.
        /// </summary>
        /// <param name="memberName">The name of the field or property of the object. Must be a bool type.</param>
        public VisibleIfAttribute(string memberName)
        {
            MemberName = memberName;
        }
    }
}
