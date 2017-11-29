////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
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

        private int _id;
        private Sprite _icon;
        private string _text;
        private bool _mouseDown;

        /// <summary>
        /// Event fired when user clicks the button.
        /// </summary>
        public Action OnClicked;

        /// <summary>
        /// The checked state.
        /// </summary>
        public bool Checked;

        /// <summary>
        /// The automatic check mode.
        /// </summary>
        public bool AutoCheck;

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID => _id;

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
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolStripButton"/> class.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="icon">The icon.</param>
        public ToolStripButton(float height, int id, ref Sprite icon)
            : base(0, 0, height, height)
        {
            _id = id;
            _icon = icon;
            _text = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolStripButton"/> class.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="text">The text.</param>
        public ToolStripButton(float height, int id, ref Sprite icon, ref string text)
            : base(0, 0, height, height)
        {
            _id = id;
            _icon = icon;
            _text = text;
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

            // Draw background
            if (Enabled && (IsMouseOver || Checked))
                Render2D.FillRectangle(clientRect, Checked ? style.BackgroundSelected : _mouseDown ? style.BackgroundHighlighted : (style.LightBackground * 1.3f));

            // Draw icon
            if (_icon.IsValid)
            {
                Render2D.DrawSprite(_icon, iconRect, Enabled ? Color.White : style.ForegroundDisabled);
                textRect.Location.X += iconSize + DefaultMargin;
            }

            // Draw text
            if (_text.Length > 0)
            {
                textRect.Size.X = Width - DefaultMargin - textRect.Left;
                Render2D.DrawText(
                    style.FontMedium,
                    _text,
                    textRect,
                    Enabled ? style.Foreground : style.ForegroundDisabled,
                    TextAlignment.Near,
                    TextAlignment.Center);
            }
        }

        /// <inheritdoc />
        public override void PerformLayout()
        {
            var style = Style.Current;
            float iconSize = Height - DefaultMargin;
            bool hasSprite = _icon.IsValid;
            float width = DefaultMargin * 2;

            if (hasSprite)
                width += iconSize;
            if (_text.Length > 0 && style.FontMedium)
                width += style.FontMedium.MeasureText(_text).X + (hasSprite ? DefaultMargin : 0);

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
                OnClicked?.Invoke();
                (Parent as ToolStrip)?.OnButtonClickedInternal(_id);

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
