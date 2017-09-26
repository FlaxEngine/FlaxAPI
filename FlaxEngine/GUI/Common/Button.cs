////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Button control
    /// </summary>
    public class Button : Control
    {
        /// <summary>
        /// The default height fro the buttons.
        /// </summary>
        public const float DefaultHeight = 24.0f;

        /// <summary>
        /// The mosue down flag.
        /// </summary>
        protected bool _mosueDown;

        /// <summary>
        /// Button text property.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the font used to draw button text.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Event fired when user clicks on the button
        /// </summary>
        public event Action Clicked;
        
        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="x">Position X coordinate</param>
        /// <param name="y">Position Y coordinate</param>
        /// <param name="width">Width</param>
        public Button(float x = 0, float y = 0, float width = 120)
            : base(x, y, width, DefaultHeight)
        {
            var style = Style.Current;
            Font = style.FontMedium;
            BackgroundColor = style.BackgroundNormal;
            BorderColor = style.BorderNormal;
        }

        /// <summary>
        /// Called when mouse clicks button.
        /// </summary>
        protected virtual void onClick()
        {
            Clicked?.Invoke();
        }

        #region Control

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            Rectangle clientRect = new Rectangle(Vector2.Zero, Size);
            
            // Background
            Color backgroundColor = BackgroundColor;
            Color borderColor = BorderColor;
            if (!Enabled)
            {
                backgroundColor *= 0.5f;
                borderColor *= 0.5f;
            }
            else if (_mosueDown)
            {
                backgroundColor = style.BackgroundSelected;
                borderColor = style.BorderSelected;
            }
            else if (IsMouseOver)
            {
                backgroundColor = style.BackgroundHighlighted;
                borderColor = style.BorderHighlighted;
            }
            Render2D.FillRectangle(clientRect, backgroundColor);
            Render2D.DrawRectangle(clientRect, borderColor);

            // Text
            Render2D.DrawText(Font, Text, clientRect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flag
            _mosueDown = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check mouse button
            if (buttons == MouseButtons.Left)
            {
                // Set flag
                _mosueDown = true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Check mouse button and flag
            if (_mosueDown && buttons == MouseButtons.Left)
            {
                // Clear flag
                _mosueDown = false;

                // Call event
                onClick();

                // Handled
                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear flag
            _mosueDown = false;

            base.OnLostFocus();
        }

        #endregion
    }
}
