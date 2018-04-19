// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// The basic GUI label control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Label : ContainerControl
    {
        private string _text;
        private bool _autoHeight;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>
        /// The color of the text.
        /// </value>
        public Color TextColor { get; set; } = Color.White;

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
        /// Gets or sets a value indicating whether set automatic height based on text contents.
        /// </summary>
        public bool AutoHeight
        {
            get => _autoHeight;
            set
            {
                if (_autoHeight != value)
                {
                    _autoHeight = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
            : base(0, 0, 100, 20)
        {
            CanFocus = false;
        }

        /// <inheritdoc />
        public Label(float x, float y, float width, float height)
            : base(x, y, width, height)
        {
            CanFocus = false;
        }

        /// <inheritdoc />
        public Label(Vector2 location, Vector2 size)
            : base(location, size)
        {
            CanFocus = false;
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
                TextColor,
                HorizontalAlignment,
                _autoHeight ? TextAlignment.Near : VerticalAlignment,
                Wrapping
            );

            if (ClipText)
                Render2D.PopClip();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Check if size is controlled via text
            if (AutoHeight)
            {
                var style = Style.Current;
                var font = Font ?? style.FontMedium;
                if (font)
                {
                    // Calculate text size
                    Vector2 textSize = font.MeasureText(_text);

                    // Update size
                    Height = textSize.Y + Margin.Size.Y;
                }
            }

            base.PerformLayoutSelf();
        }
    }
}
