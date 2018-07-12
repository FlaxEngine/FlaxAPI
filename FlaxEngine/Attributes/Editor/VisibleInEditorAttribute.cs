// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Attribute to show private field in editor.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class VisibleInEditorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleInEditorAttribute"/> class.
        /// </summary>
        public VisibleInEditorAttribute()
        {

        }
    }
}
