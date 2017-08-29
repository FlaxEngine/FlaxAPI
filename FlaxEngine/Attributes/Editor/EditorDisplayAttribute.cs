////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Allows to change item display name or a group in the editor.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class EditorDisplayAttribute : Attribute
    {
        /// <summary>
        /// The group name. Default is null.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The overriden item display name. Default is null.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorDisplayAttribute"/> class.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="name">The display name. Use special name `__inline__` to inline proerty into the parent container.</param>
        public EditorDisplayAttribute(string group = null, string name = null)
        {
            Group = group;
            Name = name;
        }
    }
}
