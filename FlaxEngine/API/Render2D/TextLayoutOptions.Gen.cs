// This code was auto-generated. Do not modify it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies the alignment of the text along horizontal or vertical direction in the layout box.
    /// </summary>
    [Tooltip("Specifies the alignment of the text along horizontal or vertical direction in the layout box.")]
    public enum TextAlignment
    {
        /// <summary>
        /// Align text near the edge.
        /// </summary>
        [Tooltip("Align text near the edge.")]
        Near = 0,

        /// <summary>
        /// Align text to the center.
        /// </summary>
        [Tooltip("Align text to the center.")]
        Center,

        /// <summary>
        /// Align text to the far edge.
        /// </summary>
        [Tooltip("Align text to the far edge.")]
        Far,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Specifies text wrapping to be used in a particular multiline paragraph.
    /// </summary>
    [Tooltip("Specifies text wrapping to be used in a particular multiline paragraph.")]
    public enum TextWrapping
    {
        /// <summary>
        /// No text wrapping.
        /// </summary>
        [Tooltip("No text wrapping.")]
        NoWrap = 0,

        /// <summary>
        /// Wrap only whole words that overflow.
        /// </summary>
        [Tooltip("Wrap only whole words that overflow.")]
        WrapWords,

        /// <summary>
        /// Wrap single characters that overflow.
        /// </summary>
        [Tooltip("Wrap single characters that overflow.")]
        WrapChars,
    }
}

namespace FlaxEngine
{
    /// <summary>
    /// Structure which describes text layout properties.
    /// </summary>
    [Tooltip("Structure which describes text layout properties.")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe partial struct TextLayoutOptions
    {
        /// <summary>
        /// The layout rectangle (text bounds).
        /// </summary>
        [Tooltip("The layout rectangle (text bounds).")]
        public Rectangle Bounds;

        /// <summary>
        /// The horizontal alignment mode.
        /// </summary>
        [Tooltip("The horizontal alignment mode.")]
        public TextAlignment HorizontalAlignment;

        /// <summary>
        /// The vertical alignment mode.
        /// </summary>
        [Tooltip("The vertical alignment mode.")]
        public TextAlignment VerticalAlignment;

        /// <summary>
        /// The text wrapping mode.
        /// </summary>
        [Tooltip("The text wrapping mode.")]
        public TextWrapping TextWrapping;

        /// <summary>
        /// The text scale factor. Default is 1.
        /// </summary>
        [Tooltip("The text scale factor. Default is 1.")]
        public float Scale;

        /// <summary>
        /// Base line gap scale. Default is 1.
        /// </summary>
        [Tooltip("Base line gap scale. Default is 1.")]
        public float BaseLinesGapScale;
    }
}
