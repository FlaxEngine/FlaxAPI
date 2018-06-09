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
        [EditorOrder(10)]
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
        [EditorOrder(20)]
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the horizontal text alignment.
        /// </summary>
        [EditorOrder(30)]
        public TextAlignment HorizontalAlignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// Gets or sets the vertical text alignment.
        /// </summary>
        [EditorOrder(40)]
        public TextAlignment VerticalAlignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// Gets or sets the text wrapping.
        /// </summary>
        [EditorOrder(50)]
        public TextWrapping Wrapping { get; set; } = TextWrapping.NoWrap;

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        [EditorOrder(60)]
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the margin for the text.
        /// </summary>
        [EditorOrder(70)]
        public Margin Margin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether clip text during rendering.
        /// </summary>
        [EditorOrder(80)]
        public bool ClipText { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether set automatic height based on text contents.
        /// </summary>
        [EditorOrder(90)]
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
