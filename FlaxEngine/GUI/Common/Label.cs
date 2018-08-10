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
        [EditorOrder(10), MultilineText, Tooltip("The label text.")]
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
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the text.")]
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the color of the text when it is highlighted (mouse is over).
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000), Tooltip("The color of the text when it is highlighted (mouse is over).")]
        public Color TextColorHighlighted { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the horizontal text alignment within the control bounds.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2010), Tooltip("The horizontal text aligment within the control bounds.")]
        public TextAlignment HorizontalAlignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// Gets or sets the vertical text alignment within the control bounds.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2020), Tooltip("The vertical text aligment within the control bounds.")]
        public TextAlignment VerticalAlignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// Gets or sets the text wrapping within the control bounds.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2030), Tooltip("The text wrapping within the control bounds.")]
        public TextWrapping Wrapping { get; set; } = TextWrapping.NoWrap;

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000)]
        public FontReference Font { get; set; }

        /// <summary>
        /// Gets or sets the custom material used to render the text. It must has domain set to GUI and have a public texture parameter named Font used to sample font atlas texture with font characters data.
        /// </summary>
        [EditorDisplay("Style"), EditorOrder(2000)]
        public MaterialBase Material { get; set; }

        /// <summary>
        /// Gets or sets the margin for the text within the control bounds.
        /// </summary>
        [EditorOrder(70), Tooltip("The margin for the text within the control bounds.")]
        public Margin Margin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether clip text during rendering.
        /// </summary>
        [EditorOrder(80), Tooltip("If checked, text will be clipped during rendering.")]
        public bool ClipText { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether set automatic height based on text contents.
        /// </summary>
        [EditorOrder(90), Tooltip("If checked, the control height will be based on text contents.")]
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
            var style = Style.Current;
            Font = new FontReference(style.FontMedium);
        }

        /// <inheritdoc />
        public Label(float x, float y, float width, float height)
        : base(x, y, width, height)
        {
            CanFocus = false;
            var style = Style.Current;
            Font = new FontReference(style.FontMedium);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var rect = new Rectangle(new Vector2(Margin.Left, Margin.Top), Size - Margin.Size);

            if (ClipText)
                Render2D.PushClip(ref rect);

            var color = IsMouseOver ? TextColorHighlighted : TextColor;

            if (!EnabledInHierarchy)
                color *= 0.6f;

            Render2D.DrawText(
                Font.GetFont(),
                Material,
                Text,
                rect,
                color,
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
                var font = Font.GetFont();
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
