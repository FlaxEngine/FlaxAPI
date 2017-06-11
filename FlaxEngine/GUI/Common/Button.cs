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
        protected bool _mosueDown;

        /// <summary>
        /// Button text property
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Event fired when user clicks on the button
        /// </summary>
        public Action Clicked;

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="x">Position X coordinate</param>
        /// <param name="y">Position Y coordinate</param>
        /// <param name="width">Width</param>
        public Button(float x, float y, float width = 120)
            : base(true, x, y, width, 24.0f)
        {
        }

        protected virtual void onClick()
        {
            Clicked?.Invoke();
        }

        #region Control

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            Rectangle clientRect = new Rectangle(0, 0, Size);

            // Background
            Color backgroundColor = style.BackgroundNormal;
            Color borderColor = style.BorderNormal;
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
            Render2D.DrawText(style.FontMedium, Text, clientRect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Center, TextAlignment.Center);
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
