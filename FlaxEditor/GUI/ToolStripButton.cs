////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI
{
    /// <summary>
    /// Tool strip button control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class ToolStripButton : Control
    {
        /// <summary>
        /// The default margin for button parts (icon, text, etc.).
        /// </summary>
        public const int DefaultMargin = 2;
		
        private Sprite _icon;
        private bool _mouseDown;

        /// <summary>
        /// Event fired when user clicks the button.
        /// </summary>
        public Action Clicked;

        /// <summary>
        /// The checked state.
        /// </summary>
        public bool Checked;

        /// <summary>
        /// The automatic check mode.
        /// </summary>
        public bool AutoCheck;
		
        /// <summary>
        /// The icon.
        /// </summary>
        public Sprite Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolStripButton"/> class.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="icon">The icon.</param>
        public ToolStripButton(float height, ref Sprite icon)
            : base(0, 0, height, height)
        {
            _icon = icon;
        }
		
        /// <summary>
        /// Sets the automatic check mode.
        /// </summary>
        /// <param name="value">True if use ato check, otherwise false.</param>
        /// <returns>This button.</returns>
        public ToolStripButton SetAutoCheck(bool value)
        {
            AutoCheck = value;
            return this;
        }

        /// <summary>
        /// Sets the checked state.
        /// </summary>
        /// <param name="value">True if check it, otherwise false.</param>
        /// <returns>This button.</returns>
        public ToolStripButton SetChecked(bool value)
        {
            Checked = value;
            return this;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            float iconSize = Height - DefaultMargin;
            var clientRect = new Rectangle(Vector2.Zero, Size);
            var iconRect = new Rectangle(DefaultMargin, DefaultMargin, iconSize, iconSize);
            var textRect = new Rectangle(DefaultMargin, 0, 0, Height);
            bool enabled = EnabledInHierarchy;

            // Draw background
            if (enabled && (IsMouseOver || Checked))
                Render2D.FillRectangle(clientRect, Checked ? style.BackgroundSelected : _mouseDown ? style.BackgroundHighlighted : (style.LightBackground * 1.3f));

            // Draw icon
            if (_icon.IsValid)
            {
                Render2D.DrawSprite(_icon, iconRect, enabled ? Color.White : style.ForegroundDisabled);
                textRect.Location.X += iconSize + DefaultMargin;
            }

            // Draw text
            if (!string.IsNullOrEmpty(Name))
            {
                textRect.Size.X = Width - DefaultMargin - textRect.Left;
                Render2D.DrawText(
                    style.FontMedium,
                    Name,
                    textRect,
                    enabled ? style.Foreground : style.ForegroundDisabled,
                    TextAlignment.Near,
                    TextAlignment.Center);
            }
        }
		
	    /// <inheritdoc />
	    public override void PerformLayout(bool force = false)
        {
            var style = Style.Current;
            float iconSize = Height - DefaultMargin;
            bool hasSprite = _icon.IsValid;
            float width = DefaultMargin * 2;

            if (hasSprite)
                width += iconSize;
            if (!string.IsNullOrEmpty(Name) && style.FontMedium)
                width += style.FontMedium.MeasureText(Name).X + (hasSprite ? DefaultMargin : 0);

            Width = width;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                // Set flag
                _mouseDown = true;

                Focus();
                return true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left && _mouseDown)
            {
                // Clear flag
                _mouseDown = false;

                // Fire events
                if (AutoCheck)
                    Checked = !Checked;
                Clicked?.Invoke();
                (Parent as ToolStrip)?.OnButtonClicked(this);

                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flag
            _mouseDown = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear flag
            _mouseDown = false;

            base.OnLostFocus();
        }
    }
}
