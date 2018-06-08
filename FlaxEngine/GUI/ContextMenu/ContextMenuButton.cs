// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Context Menu button control.
    /// </summary>
    /// <seealso cref="ContextMenuItem" />
    [HideInEditor]
    public class ContextMenuButton : ContextMenuItem
    {
        private bool _isMouseDown;

        /// <summary>
        /// Event fired when user clicks on the button.
        /// </summary>
        public event Action Clicked;

        /// <summary>
        /// Event fired when user clicks on the button.
        /// </summary>
        public event Action<ContextMenuButton> ButtonClicked;

        /// <summary>
        /// The button text.
        /// </summary>
        public string Text
        {
            get => Name;
            set => Name = value;
        }

        /// <summary>
        /// The button short keys information (eg. 'Ctrl+C').
        /// </summary>
        public string Shortkeys;

        /// <summary>
        /// Item icon (best is 16x16).
        /// </summary>
        public Sprite Icon;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenuButton"/> class.
        /// </summary>
        /// <param name="parent">The parent context menu.</param>
        /// <param name="text">The text.</param>
        /// <param name="shortKeys">The short keys tip.</param>
        public ContextMenuButton(ContextMenu parent, string text, string shortKeys = "")
        : base(parent, 8, 22)
        {
            Name = text;
            Shortkeys = shortKeys;
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            var backgroundRect = new Rectangle(-X + 3, 0, Parent.Width - 6, Height);
            var textRect = new Rectangle(0, 0, Width - 4, Height);
            var textColor = Enabled ? style.Foreground : style.ForegroundDisabled;

            // Draw background
            if (IsMouseOver && Enabled)
                Render2D.FillRectangle(backgroundRect, style.LightBackground);

            base.Draw();

            // Draw text
            Render2D.DrawText(style.FontMedium, Name, textRect, textColor, TextAlignment.Near, TextAlignment.Center);

            // Draw shortkeys
            Render2D.DrawText(style.FontMedium, Shortkeys, textRect, textColor, TextAlignment.Far, TextAlignment.Center);

            // Draw icon
            const float iconSize = 14;
            if (Icon.IsValid)
                Render2D.DrawSprite(Icon, new Rectangle(-iconSize - 1, (Height - iconSize) / 2, iconSize, iconSize));
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flag
            _isMouseDown = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            // Set flag
            _isMouseDown = true;

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;

            // Check if mouse was pressing
            if (_isMouseDown)
            {
                // Clear flag
                _isMouseDown = false;

                // Close topmost context menu
                ParentContextMenu?.TopmostCM.Hide();

                // Fire event
                Clicked?.Invoke();
                ButtonClicked?.Invoke(this);
                ParentContextMenu?.OnButtonClicked(this);

                // Event handled
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear flag
            _isMouseDown = false;

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override float MinimumWidth
        {
            get
            {
                var style = Style.Current;
                float width = 20;

                if (style.FontMedium)
                {
                    width += style.FontMedium.MeasureText(Text).X;

                    if (Shortkeys.Length > 0)
                    {
                        width += 40 + style.FontMedium.MeasureText(Shortkeys).X;
                    }
                }

                return Mathf.Max(width, base.MinimumWidth);
            }
        }
    }
}
