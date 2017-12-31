////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Table column descriptor.
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// Converts raw cell value to the string used by the column formatting policy.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value string.</returns>
        public delegate string ValueFormatDelegate(object value);

        /// <summary>
        /// True if use expand/collapse rows feature for this column. See <see cref="Row.Depth"/> property which is used to describe the rows hierarchy.
        /// </summary>
        public bool UseExpandCollapseMode;

        /// <summary>
        /// The cell text alignment horizontally.
        /// </summary>
        public TextAlignment CellAlignment = TextAlignment.Far;

        /// <summary>
        /// The column title.
        /// </summary>
        public string Title;

        /// <summary>
        /// The title font.
        /// </summary>
        public Font TitleFont;

        /// <summary>
        /// The column title text color.
        /// </summary>
        public Color TitleColor = Color.White;

        /// <summary>
        /// The column title bakground background.
        /// </summary>
        public Color TitleBackgroundColor = Color.Brown;
        
        /// <summary>
        /// The minimum size (in pixels) of the column.
        /// </summary>
        public float MinSize = 10.0f;

        /// <summary>
        /// The minimum size percentage of the column (in range 0-100).
        /// </summary>
        public float MinSizePercentage = 0.0f;

        /// <summary>
        /// The maximum size (in pixels) of the column.
        /// </summary>
        public float MaxSize = float.MaxValue;

        /// <summary>
        /// The maximum size percentage of the column (in range 0-100).
        /// </summary>
        public float MaxSizePercentage = 1.0f;

        /// <summary>
        /// The value formatting delegate.
        /// </summary>
        public ValueFormatDelegate FormatValue;
    }
}
