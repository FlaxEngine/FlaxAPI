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
        /// Gets or sets the margin for the text.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public Margin Margin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether clip text during rendering.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clip text; otherwise, <c>false</c>.
        /// </value>
        public bool ClipText { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
            : base(false, 0, 0, 100, 20)
        {
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
            var rect = new Rectangle(new Vector2(Margin.Left, Margin.Top), Size - Margin.Size);

            if (ClipText)
                Render2D.PushClip(ref rect);

            Render2D.DrawText(
                font,
                Text,
                rect,
                style.Foreground,
                HorizontalAlignment,
                VerticalAlignment,
                Wrapping
            );

            if (ClipText)
                Render2D.PopClip();
        }
    }
}
