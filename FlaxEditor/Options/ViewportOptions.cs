// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.ComponentModel;
using FlaxEngine;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Editor viewport options data container.
    /// </summary>
    [CustomEditor(typeof(Editor<ViewportOptions>))]
    public class ViewportOptions
    {
        /// <summary>
        /// Gets or sets the mouse movement sensitivity scale applied when using the viewport camera.
        /// </summary>
        [DefaultValue(1.0f), Limit(0.01f, 100.0f)]
        [EditorDisplay("General"), EditorOrder(100), Tooltip("The mouse movement sensitivity scale applied when using the viewport camera.")]
        public float MouseSensitivity { get; set; } = 1.0f;
    }
}
