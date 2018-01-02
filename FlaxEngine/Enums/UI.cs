////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    /// <summary>
    /// Specifies the alignment of the text along horizontal or vetical direction in the layout box
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
    /// Specifies text wrapping to be used in a particular multiline paragraph
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
    /// Structure which describes text layout properties
    /// </summary>
    public struct TextLayoutOptions
    {
        /// <summary>
        /// Layout rectangle
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Horizontal alignment
        /// </summary>
        public TextAlignment HorizontalAlignment;

        /// <summary>
        /// Vertical alignment
        /// </summary>
        public TextAlignment VerticalAlignment;

        /// <summary>
        /// Text wrapping mode
        /// </summary>
        public TextWrapping TextWrapping;

        /// <summary>
        /// Base line gap scale
        /// </summary>
        public float BaseLinesGapScale;

        /// <summary>
        /// Gets the default layout.
        /// </summary>
        public static TextLayoutOptions Default
        {
            get
            {
                return new TextLayoutOptions
                {
                    Bounds = new Rectangle(0, 0, float.MaxValue, float.MaxValue),
                    BaseLinesGapScale = 1.0f,
                };
            }
        }
    }
}
