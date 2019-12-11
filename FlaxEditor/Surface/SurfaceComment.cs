// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using FlaxEditor.GUI;
using FlaxEditor.GUI.Input;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface comment control.
    /// </summary>
    /// <seealso cref="SurfaceNode" />
    public class SurfaceComment : SurfaceNode
    {
        private Rectangle _colorButtonRect;
        private Rectangle _resizeButtonRect;
        private Vector2 _startResizingSize;

        /// <summary>
        /// True if sizing tool is in use.
        /// </summary>
        protected bool _isResizing;

        /// <summary>
        /// Gets or sets the color of the comment.
        /// </summary>
        public Color Color
        {
            get => BackgroundColor;
            set => BackgroundColor = value;
        }

        private string TitleValue
        {
            get => (string)Values[0];
            set => SetValue(0, value, false);
        }

        private Color ColorValue
        {
            get => (Color)Values[1];
            set => SetValue(1, value, false);
        }

        private Vector2 SizeValue
        {
            get => (Vector2)Values[2];
            set => SetValue(2, value, false);
        }

        /// <inheritdoc />
        public SurfaceComment(uint id, VisjectSurfaceContext context, NodeArchetype nodeArch, GroupArchetype groupArch)
        : base(id, context, nodeArch, groupArch)
        {
        }

        /// <inheritdoc />
        public override void OnSurfaceLoaded()
        {
            base.OnSurfaceLoaded();

            // Read node data
            Title = TitleValue;
            Color = ColorValue;
            Size = SizeValue;
        }

        /// <inheritdoc />
        public override void OnValuesChanged()
        {
            base.OnValuesChanged();

            // Read node data
            Title = TitleValue;
            Color = ColorValue;
            Size = SizeValue;
        }

        private void EndResizing()
        {
            // Clear state
            _isResizing = false;

            if (_startResizingSize != Size)
            {
                SizeValue = Size;
                Surface.MarkAsEdited(false);
            }

            EndMouseCapture();
        }

        /// <inheritdoc />
        public override bool CanSelect(ref Vector2 location)
        {
            return _headerRect.MakeOffsetted(Location).Contains(ref location) && !_resizeButtonRect.MakeOffsetted(Location).Contains(ref location);
        }

        /// <inheritdoc />
        public override bool IsSelectionIntersecting(ref Rectangle selectionRect)
        {
            return _headerRect.MakeOffsetted(Location).Intersects(ref selectionRect);
        }

        /// <inheritdoc />
        protected override void UpdateRectangles()
        {
            const float headerSize = Constants.NodeHeaderSize;
            const float buttonMargin = Constants.NodeCloseButtonMargin;
            const float buttonSize = Constants.NodeCloseButtonSize;
            _headerRect = new Rectangle(0, 0, Width, headerSize);
            _closeButtonRect = new Rectangle(Width - buttonSize - buttonMargin, buttonMargin, buttonSize, buttonSize);
            _colorButtonRect = new Rectangle(_closeButtonRect.Left - buttonSize - buttonMargin, buttonMargin, buttonSize, buttonSize);
            _resizeButtonRect = new Rectangle(_closeButtonRect.Left, Height - buttonSize - buttonMargin, buttonSize, buttonSize);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;
            var color = Color;
            var backgroundRect = new Rectangle(Vector2.Zero, Size);
            var headerColor = new Color(Mathf.Clamp(color.R, 0.1f, 0.3f), Mathf.Clamp(color.G, 0.1f, 0.3f), Mathf.Clamp(color.B, 0.1f, 0.3f), 0.4f);
            if (IsSelected)
                headerColor *= 2.0f;

            // Paint background
            Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), BackgroundColor);

            // Draw child controls
            DrawChildren();

            // Header
            Render2D.FillRectangle(_headerRect, headerColor);
            Render2D.DrawText(style.FontLarge, Title, _headerRect, style.Foreground, TextAlignment.Center, TextAlignment.Center);

            // Close button
            float alpha = _closeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
            Render2D.DrawSprite(style.Cross, _closeButtonRect, new Color(alpha));

            // Color button
            alpha = _colorButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
            Render2D.DrawSprite(style.Settings, _colorButtonRect, new Color(alpha));

            // Check if is resizing
            if (_isResizing)
            {
                // Draw overlay
                Render2D.FillRectangle(_resizeButtonRect, Color.Orange * 0.3f);
            }

            // Resize button
            alpha = _resizeButtonRect.Contains(_mousePosition) ? 1.0f : 0.7f;
            Render2D.DrawSprite(style.Scale, _resizeButtonRect, new Color(alpha));

            // Selection outline
            if (_isSelected)
            {
                backgroundRect.Expand(1.5f);
                var colorTop = Color.Orange;
                var colorBottom = Color.OrangeRed;
                Render2D.DrawRectangle(backgroundRect, colorTop, colorTop, colorBottom, colorBottom);
            }
        }

        /// <inheritdoc />
        protected override Vector2 CalculateNodeSize(float width, float height)
        {
            return Size;
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Check if was resizing
            if (_isResizing)
            {
                EndResizing();
            }

            // Base
            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void OnEndMouseCapture()
        {
            // Check if was resizing
            if (_isResizing)
            {
                EndResizing();
            }
            else
            {
                base.OnEndMouseCapture();
            }
        }

        /// <inheritdoc />
        public override bool ContainsPoint(ref Vector2 location)
        {
            return _headerRect.Contains(ref location) || _resizeButtonRect.Contains(ref location);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            // Check if can start resizing
            if (buttons == MouseButton.Left && _resizeButtonRect.Contains(ref location))
            {
                // Start sliding
                _isResizing = true;
                _startResizingSize = Size;
                StartMouseCapture();

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check if is resizing
            if (_isResizing)
            {
                // Update size
                Size = Vector2.Max(location, new Vector2(140.0f, _headerRect.Bottom));
            }
            else
            {
                // Base
                base.OnMouseMove(location);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDoubleClick(location, buttons))
                return true;

            // Rename
            if (_headerRect.Contains(ref location))
            {
                StartRenaming();
                return true;
            }

            return false;
        }

        public void StartRenaming()
        {
            Surface.Select(this);
            var dialog = RenamePopup.Show(this, _headerRect, Title, false);
            dialog.Renamed += OnRenamed;
        }

        private void OnRenamed(RenamePopup renamePopup)
        {
            Title = TitleValue = renamePopup.Text;
            Surface.MarkAsEdited(false);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left && _isResizing)
            {
                EndResizing();
                return true;
            }

            if (base.OnMouseUp(location, buttons))
                return true;

            // Close
            if (_closeButtonRect.Contains(ref location))
            {
                Surface.Delete(this);
                return true;
            }

            // Color
            if (_colorButtonRect.Contains(ref location))
            {
                ColorValueBox.ShowPickColorDialog?.Invoke(Color, OnColorChanged);
                return true;
            }

            return false;
        }

        private void OnColorChanged(Color color, bool sliding)
        {
            Color = ColorValue = color;
            Surface.MarkAsEdited(false);
        }
    }
}
