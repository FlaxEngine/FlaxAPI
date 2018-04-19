// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Inserts an empty space between controls in the editor.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SpaceAttribute : Attribute
    {
        /// <summary>
        /// The spacing in pixel (vertically).
        /// </summary>
        public float Height;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceAttribute"/> class.
        /// </summary>
        /// <param name="height">The spacing.</param>
        public SpaceAttribute(float height)
        {
            Height = height;
        }
    }
}
