// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using FlaxEngine;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Ediotr interface options data container.
    /// </summary>
    [CustomEditor(typeof(Editor<InterfaceOptions>))]
    public class InterfaceOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether center mouse position on window focus in play mode. Helps when working with games that lock mouse cursor.
        /// </summary>
        [EditorDisplay("Interface", "Center Mouse On Focus"), EditorOrder(100), Tooltip("Determines whether center mouse position on window focus in play mode. Helps when working with games that lock mouse cursor.")]
        public bool CenterMouseOnFocus { get; set; } = false;
    }
}
