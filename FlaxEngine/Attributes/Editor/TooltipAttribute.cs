// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies a tooltip for a property/field in the editor.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
    public sealed class TooltipAttribute : Attribute
    {
        /// <summary>
        /// The tooltip text.
        /// </summary>
        public string Text;

        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipAttribute"/> class.
        /// </summary>
        /// <param name="text">The tooltip text.</param>
        public TooltipAttribute(string text)
        {
            Text = text;
        }
    }
}
