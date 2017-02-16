////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine
{
    /// <summary>
    /// Specifies the alignment of the text along horizontal or vetical direction in the layout box
    /// </summary>
    public enum TextAlignment
    {
        Near = 0,
        Center,
        Far
    };

    /// <summary>
    /// Specifies text wrapping to be used in a particular multiline paragraph
    /// </summary>
    public enum TextWrapping
    {
        NoWrap = 0,
        WrapWords,
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
        /// Gets default layout
        /// </summary>
        public static TextLayoutOptions Default
        {
            get
            {
                return new TextLayoutOptions()
                {
                    Bounds = new Rectangle(0, 0, float.MaxValue, float.MaxValue),
                    BaseLinesGapScale = 1.0f,
                };
            }
        }
    }
}
