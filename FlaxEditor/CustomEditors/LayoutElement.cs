// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine.GUI;

namespace FlaxEditor.CustomEditors
{
    /// <summary>
    /// Represents single element of the Custom Editor layout.
    /// </summary>
    public abstract class LayoutElement
    {
        /// <summary>
        /// Gets the control represented by this element.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public abstract Control Control { get; }
    }
}
