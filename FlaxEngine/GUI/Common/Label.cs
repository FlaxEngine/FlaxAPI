////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// The basic GUI label control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class Label : Control
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        /// <value>
        /// The horizontal alignment.
        /// </value>
        public TextAlignment HorizontalAlignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        /// <value>
        /// The vertical alignment.
        /// </value>
        public TextAlignment VerticalAlignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// Gets or sets the text wrapping.
        /// </summary>
        /// <value>
        /// The wrapping.
        /// </value>
        public TextWrapping Wrapping { get; set; } = TextWrapping.NoWrap;

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the margins for the text. Each vector component represents other control side in order: left, right, top and bottom.
        /// </summary>
        /// <value>
        /// The margins.
        /// </value>
        public Vector4 Margins { get; set; }

        /// <summary>
        /// Gets or sets the left margin.
        /// </summary>
        /// <value>
        /// The left margin.
        /// </value>
        public float LeftMargin
        {
            get => Margins.X;
            set
            {
                var v = Margins;
                v.X = value;
                Margins = v;
            }
        }

        /// <summary>
        /// Gets or sets the right margin.
        /// </summary>
        /// <value>
        /// The right margin.
        /// </value>
        public float RightMargin
        {
            get => Margins.Y;
            set
            {
                var v = Margins;
                v.Y = value;
                Margins = v;
            }
        }

        /// <summary>
        /// Gets or sets the top margin.
        /// </summary>
        /// <value>
        /// The top margin.
        /// </value>
        public float TopMargin
        {
            get => Margins.Z;
            set
            {
                var v = Margins;
                v.Z = value;
                Margins = v;
            }
        }

        /// <summary>
        /// Gets or sets the bottom margin.
        /// </summary>
        /// <value>
        /// The bottom margin.
        /// </value>
        public float BottomMargin
        {
            get => Margins.W;
            set
            {
                var v = Margins;
                v.W = value;
                Margins = v;
            }
        }

        /// <inheritdoc />
        public Label(bool canFocus, float x, float y, float width, float height)
            : base(canFocus, x, y, width, height)
        {
        }

        /// <inheritdoc />
        public Label(bool canFocus, Vector2 location, Vector2 size)
            : base(canFocus, location, size)
        {
        }

        /// <inheritdoc />
        public Label(bool canFocus, Rectangle bounds)
            : base(canFocus, bounds)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;
            var font = Font ?? style.FontMedium;
            var rect = new Rectangle(Margins.X, Margins.Z, Width - Margins.X - Margins.Y, Height - Margins.Z - Margins.W);

            Render2D.DrawText(
                font,
                Text,
                rect,
                style.Foreground,
                HorizontalAlignment,
                VerticalAlignment,
                Wrapping
            );
        }
    }
}
