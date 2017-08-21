////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

namespace FlaxEngine.GUI
{
    /// <summary>
    /// The basic GUI image control. Shows texture, sprite or render target.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class Image : ContainerControl
    {
        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public IImageSource ImageSource { get; set; }

        /// <summary>
        /// Gets or sets the margin for the image.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        public Margin Margin { get; set; }

        /// <summary>
        /// Gets or sets the color used to multiply the image pixels.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets a value indicating whether render image with alpha blending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use alpha blending; otherwise, <c>false</c>.
        /// </value>
        public bool WithAlpha { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether keep aspect ratio whend rawing the image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if keep source aspect ratio; otherwise, <c>false</c>.
        /// </value>
        public bool KeepAspectRatio { get; set; } = true;

        /// <inheritdoc />
        public Image(bool canFocus, float x, float y, float width, float height)
            : base(canFocus, x, y, width, height)
        {
        }

        /// <inheritdoc />
        public Image(bool canFocus, Vector2 location, Vector2 size)
            : base(canFocus, location, size)
        {
        }

        /// <inheritdoc />
        public Image(bool canFocus, Rectangle bounds)
            : base(canFocus, bounds)
        {
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            if (ImageSource == null)
                return;

            Rectangle rect;
            if (KeepAspectRatio)
            {
                // Figure out the ratio
                var size = Size;
                var imageSize = ImageSource.Size;
                if (imageSize.LengthSquared < 1)
                    return;
                var ratio = size / imageSize;
                var aspectRatio = ratio.MinValue;

                // Get the new height and width
                var newSize = imageSize * aspectRatio;

                // Calculate the X,Y position of the upper-left corner 
                // (one of these will always be zero)
                var newPos = (size - newSize) / 2;

                rect = new Rectangle(newPos, newSize);
            }
            else
            {
                rect = new Rectangle(Vector2.Zero, Size);
            }

            Margin.ShrinkRectangle(ref rect);

            ImageSource.Draw(rect, Color, WithAlpha);
        }
    }
}
