// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System.Runtime.InteropServices;

namespace FlaxEngine
{
    /// <summary>
    /// Specifies the alignment of the text along horizontal or vertical direction in the layout box.
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// Align text near the edge.
        /// </summary>
        Near = 0,

        /// <summary>
        /// Align text to the center.
        /// </summary>
        Center,

        /// <summary>
        /// Align text to the far edge.
        /// </summary>
        Far
    };

    /// <summary>
    /// Specifies text wrapping to be used in a particular multiline paragraph.
    /// </summary>
    public enum TextWrapping
    {
        /// <summary>
        /// No text wrapping.
        /// </summary>
        NoWrap = 0,

        /// <summary>
        /// Wrap only whole words that overflow.
        /// </summary>
        WrapWords,

        /// <summary>
        /// Wrap single characters that overflow.
        /// </summary>
        WrapChars
    };

    /// <summary>
    /// Structure which describes text layout properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TextLayoutOptions
    {
        /// <summary>
        /// The layout rectangle (text bounds).
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// The horizontal alignment mode.
        /// </summary>
        public TextAlignment HorizontalAlignment;

        /// <summary>
        /// The vertical alignment mode.
        /// </summary>
        public TextAlignment VerticalAlignment;

        /// <summary>
        /// The text wrapping mode.
        /// </summary>
        public TextWrapping TextWrapping;

        /// <summary>
        /// The text scale factor. Default is 1.
        /// </summary>
        public float Scale;

        /// <summary>
        /// Base line gap scale. Default is 1.
        /// </summary>
        public float BaseLinesGapScale;

        /// <summary>
        /// Gets the default layout.
        /// </summary>
        public static TextLayoutOptions Default => new TextLayoutOptions
        {
            Bounds = new Rectangle(0, 0, float.MaxValue, float.MaxValue),
            Scale = 1.0f,
            BaseLinesGapScale = 1.0f,
        };
    }
}
