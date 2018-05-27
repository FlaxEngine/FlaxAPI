// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Describes the space around a control.
    /// </summary>
    public struct Margin
    {
        /// <summary>
        /// Holds the margin to the left.
        /// </summary>
        public float Left;

        /// <summary>
        /// Holds the margin to the right.
        /// </summary>
        public float Right;

        /// <summary>
        /// Holds the margin to the top.
        /// </summary>
        public float Top;

        /// <summary>
        /// Holds the margin to the bottom.
        /// </summary>
        public float Bottom;

        /// <summary>
        /// Gets the margin's total size. Cumulative margin size.
        /// </summary>
        public Vector2 Size => new Vector2(Left + Right, Top + Bottom);

        /// <summary>
        /// Gets the width (left + right).
        /// </summary>
        public float Width => Left + Right;

        /// <summary>
        /// Gets the height (top + bottom).
        /// </summary>
        public float Height => Top + Bottom;

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Margin(float value)
        {
            Left = Right = Top = Bottom = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Margin"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <param name="top">The top.</param>
        /// <param name="bottom">The bottom.</param>
        public Margin(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Shrinks the rectangle by this margin.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        public void ShrinkRectangle(ref Rectangle rect)
        {
            rect.Location.X += Left;
            rect.Location.Y += Top;
            rect.Size.X -= Left + Right;
            rect.Size.Y -= Top + Bottom;
        }
    }
}
