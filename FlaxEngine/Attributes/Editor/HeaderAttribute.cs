// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Inserts a header control with a custom text into the editor layout.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class HeaderAttribute : Attribute
    {
        /// <summary>
        /// The header text.
        /// </summary>
        public string Text;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderAttribute"/> class.
        /// </summary>
        /// <param name="text">The header text.</param>
        public HeaderAttribute(string text)
        {
            Text = text;
        }
    }
}
