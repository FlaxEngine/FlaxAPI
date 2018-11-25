// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.ComponentModel;
using FlaxEngine;

namespace FlaxEditor.Options
{
    /// <summary>
    /// Visual editor options data container.
    /// </summary>
    [CustomEditor(typeof(Editor<VisualOptions>))]
    public sealed class VisualOptions
    {
        /// <summary>
        /// Gets or sets the first outline color.
        /// </summary>
        [DefaultValue(typeof(Color), "0.039,0.827,0.156")]
        [EditorDisplay("Gizmo"), EditorOrder(100), Tooltip("The first color of the selection outline gradient.")]
        public Color SelectionOutlineColor0 { get; set; } = new Color(0.039f, 0.827f, 0.156f);

        /// <summary>
        /// Gets or sets the second outline color.
        /// </summary>
        [DefaultValue(typeof(Color), "0.019,0.615,0.101")]
        [EditorDisplay("Gizmo"), EditorOrder(101), Tooltip("The second color of the selection outline gradient.")]
        public Color SelectionOutlineColor1 { get; set; } = new Color(0.019f, 0.615f, 0.101f);

        /// <summary>
        /// Gets or sets a value indicating whether enable MSAA for DebugDraw primitives rendering. Helps with pixel aliasing but reduces performance.
        /// </summary>
        [DefaultValue(true)]
        [EditorDisplay("Quality", "Enable MSAA For Debug Draw"), EditorOrder(500), Tooltip("Determines whether enable MSAA for DebugDraw primitives rendering. Helps with pixel aliasing but reduces performance.")]
        public bool EnableMSAAForDebugDraw { get; set; } = true;
    }
}
