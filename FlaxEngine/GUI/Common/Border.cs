// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Border control that draws the border around the control edges (inner and outer sides).
    /// </summary>
    public class Border : Control
    {
        /// <summary>
        /// Gets or sets the color used to draw border lines.
        /// </summary>
        [EditorOrder(0), Tooltip("The color used to draw border lines.")]
        public Color BorderColor { get; set; }

        /// <summary>
        /// The border lines width.
        /// </summary>
        [EditorOrder(10), Limit(0, float.MaxValue, 0.1f), Tooltip("The border lines width.")]
        public float BorderWidth { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
        {
            BorderColor = Color.White;
            BorderWidth = 2.0f;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var width = BorderWidth;
            if (width > Mathf.Epsilon)
            {
                var color = BorderColor;
                var widthOver2 = width * 0.5f;
                var size = Size;

                Render2D.DrawLine(Vector2.Zero, new Vector2(size.X - widthOver2, 0), color, width);
                Render2D.DrawLine(new Vector2(0, -widthOver2), new Vector2(0, size.Y + widthOver2), color, width);
                Render2D.DrawLine(new Vector2(size.X, -widthOver2), new Vector2(size.X, size.Y + widthOver2), color, width);
                Render2D.DrawLine(new Vector2(widthOver2, size.Y), new Vector2(size.X - widthOver2, size.Y), color, width);
            }
        }
    }
}
