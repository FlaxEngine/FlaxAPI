////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
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
        protected bool _mouseDown;

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
        /// Event fired when user clicks on the button
        /// </summary>
        public event Action<Button> ButtonClicked;
        
        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the background color when button is selected.
        /// </summary>
        public Color BackgroundColorSelected { get; set; }

        /// <summary>
        /// Gets or sets the border color when button is selected.
        /// </summary>
        public Color BorderColorSelected { get; set; }

        /// <summary>
        /// Gets or sets the background color when button is highlighted.
        /// </summary>
        public Color BackgroundColorHighlighted { get; set; }

        /// <summary>
        /// Gets or sets the border color when button is highlighted.
        /// </summary>
        public Color BorderColorHighlighted { get; set; }

		/// <summary>
		/// Init
		/// </summary>
		/// <param name="x">Position X coordinate</param>
		/// <param name="y">Position Y coordinate</param>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		public Button(float x = 0, float y = 0, float width = 120, float height = DefaultHeight)
            : base(x, y, width, height)
        {
            var style = Style.Current;
            Font = style.FontMedium;
            BackgroundColor = style.BackgroundNormal;
            BorderColor = style.BorderNormal;
            BackgroundColorSelected = style.BackgroundSelected;
            BorderColorSelected = style.BorderSelected;
            BackgroundColorHighlighted = style.BackgroundHighlighted;
            BorderColorHighlighted = style.BorderHighlighted;
        }

        /// <summary>
        /// Called when mouse clicks the button.
        /// </summary>
        protected virtual void OnClick()
        {
            Clicked?.Invoke();
            ButtonClicked?.Invoke(this);
        }

        /// <summary>
        /// Sets the button colors palette based on a given main color.
        /// </summary>
        /// <param name="color">The main color.</param>
        public void SetColors(Color color)
        {
            BackgroundColor = color;
            BorderColor = color.RGBMultiplied(0.5f);
            BackgroundColorSelected = color.RGBMultiplied(0.8f);
            BorderColorSelected = BorderColor;
            BackgroundColorHighlighted = color.RGBMultiplied(1.2f);
            BorderColorHighlighted = BorderColor;
        }

        #region Control

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            Rectangle clientRect = new Rectangle(Vector2.Zero, Size);
            bool enabled = EnabledInHierarchy;

            // Draw background
            Color backgroundColor = BackgroundColor;
            Color borderColor = BorderColor;
            if (!enabled)
            {
                backgroundColor *= 0.5f;
                borderColor *= 0.5f;
            }
            else if (_mouseDown)
            {
                backgroundColor = BackgroundColorSelected;
                borderColor = BorderColorSelected;
            }
            else if (IsMouseOver)
            {
                backgroundColor = BackgroundColorHighlighted;
                borderColor = BorderColorHighlighted;
            }
            Render2D.FillRectangle(clientRect, backgroundColor);
            Render2D.DrawRectangle(clientRect, borderColor);
            
            // Draw text
            Render2D.DrawText(Font, Text, clientRect, enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flag
            _mouseDown = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            // Check mouse button
            if (buttons == MouseButton.Left)
            {
                // Set flag
                _mouseDown = true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Check mouse button and flag
            if (_mouseDown && buttons == MouseButton.Left)
            {
                // Clear flag
                _mouseDown = false;

                // Call event
                OnClick();

                // Handled
                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear flag
            _mouseDown = false;

            base.OnLostFocus();
        }

        #endregion
    }
}
