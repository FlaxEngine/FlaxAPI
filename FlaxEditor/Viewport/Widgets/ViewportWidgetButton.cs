////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Viewport.Widgets
{
    /// <summary>
    /// Viewport Widget Button class.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class ViewportWidgetButton : Control
    {
        private string _text;
        private Sprite _icon;
        private ContextMenu _cm;
        private bool _checked;
        private bool _autoCheck;

        /// <summary>
        /// Event fired when user toggles checked state.
        /// </summary>
        public Action<ViewportWidgetButton> OnToggle;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                PerformLayout();
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public ViewportWidgetButton(string text, Sprite icon, ContextMenu contextMenu = null, bool autoCheck = false)
            : base(true, 0, 0, calculateButtonWidth(0, icon.IsValid), ViewportWidgetsContainer.WidgetsHeight)
        {
            _text = text;
            _icon = icon;
            _cm = contextMenu;
            _autoCheck = autoCheck;

            if (_cm != null)
                _cm.OnVisibleChanged += CmOnOnVisibleChanged;
        }

        private void CmOnOnVisibleChanged(Control control)
        {
            if (_cm != null && !_cm.IsOpened)
            {
                if (HasParent && Parent.HasParent)
                {
                    // Focus viewport
                    Parent.Parent.Focus();
                }
            }
        }

        private static float calculateButtonWidth(float textWidth, bool hasIcon)
        {
            return (hasIcon ? ViewportWidgetsContainer.WidgetsIconSize : 0) + (textWidth > 0 ? textWidth + 8 : 0);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = Style.Current;
            const float iconSize = ViewportWidgetsContainer.WidgetsIconSize;
            var iconRect = new Rectangle(0, (Height - iconSize) / 2, iconSize, iconSize);
            var textRect = new Rectangle(0, 0, Width + 1, Height + 1);

            // Check if is checked or mosue is over and auto check feature is enabled
            if (_checked)
                Render2D.FillRectangle(textRect, style.BackgroundSelected * (IsMouseOver ? 0.8f : 0.4f), true);
            else if (_autoCheck && IsMouseOver)
                Render2D.FillRectangle(textRect, style.BackgroundHighlighted);

            // Check if has icon
            if (_icon.IsValid)
            {
                // Draw icon
                Render2D.DrawSprite(_icon, iconRect);

                // Update text rectangle
                textRect.Location.X += iconSize;
                textRect.Size.X -= iconSize;
            }

            // Draw text
            Render2D.DrawText(style.FontMedium, _text, textRect, style.Foreground * (IsMouseOver ? 0.8f : 1.0f), TextAlignment.Center, TextAlignment.Center);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check if auto check feature is enabled
            if (_autoCheck)
            {
                // Toggle
                Checked = !_checked;

                // Fire event
                OnToggle?.Invoke(this);
            }

            // Check if has context menu binded
            if (_cm != null)
            {
                // Show
                _cm.Show(this, new Vector2(-1, Height + 2));
            }


            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override void PerformLayout()
        {
            var style = Style.Current;

            if (style != null && style.FontMedium)
                Width = calculateButtonWidth(style.FontMedium.MeasureText(_text).X, _icon.IsValid);
        }
    }
}
