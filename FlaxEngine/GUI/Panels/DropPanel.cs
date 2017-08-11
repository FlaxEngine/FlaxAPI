////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Drop Panel arranges control vertically and provides feature to collapse contents.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class DropPanel : ContainerControl
    {
        protected float _headerHeight = 14.0f, _headerMargin = 2.0f;
        protected bool _isClosed;
        protected bool _mouseOverHeader;
        protected bool _mouseDown;
        protected float _animationProgress = 1.0f;
        protected float _cachedHeight = 16.0f;

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>
        /// The header text.
        /// </value>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the height of the header.
        /// </summary>
        /// <value>
        /// The height of the header.
        /// </value>
        public float HeaderHeight
        {
            get => _headerHeight;
            set
            {
                _headerHeight = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the header margin.
        /// </summary>
        /// <value>
        /// The header margin.
        /// </value>
        public float HeaderMargin
        {
            get => _headerMargin;
            set
            {
                _headerMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Occurs when drop panel is opened or closed.
        /// </summary>
        public event Action<DropPanel> OnClosedChanged;

        /// <summary>
        /// Gets a value indicating whether this panel is closed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this panel is closed; otherwise, <c>false</c>.
        /// </value>
        public bool IsClosed => _isClosed;

        /// <summary>
        /// Gets the left margin.
        /// </summary>
        /// <value>
        /// The left margin.
        /// </value>
        protected virtual float LeftMargin => HeaderMargin;

        /// <summary>
        /// Gets the header rectangle.
        /// </summary>
        protected Rectangle HeaderRectangle => new Rectangle(0, 0, Width, HeaderHeight);

        /// <summary>
        /// Initializes a new instance of the <see cref="DropPanel"/> class.
        /// </summary>
        /// <param name="text">The header text.</param>
        public DropPanel(string text)
            : base(false, 0, 0, 64, 16.0f)
        {
            _performChildrenLayoutFirst = true;

            HeaderText = text;
        }

        /// <summary>
        /// Opens the group.
        /// </summary>
        public void Open()
        {
            // Check if state will change
            if (_isClosed)
            {
                // Set flag
                _isClosed = false;
                _animationProgress = 1 - _animationProgress;

                // Update
                PerformLayout();

                // Fire event
                OnClosedChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Closes the group.
        /// </summary>
        public void Close()
        {
            // Check if state will change
            if (!_isClosed)
            {
                // Set flag
                _isClosed = true;
                _animationProgress = 1 - _animationProgress;

                // Update
                PerformLayout();

                // Fire event
                OnClosedChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Toggles open state
        /// </summary>
        public void Toggle()
        {
            if (_isClosed)
                Open();
            else
                Close();
        }

        /// <summary>
        /// Ends open/close animation by force.
        /// </summary>
        public void EndAnimation()
        {
            if (_animationProgress < 1.0f)
            {
                _animationProgress = 1.0f;
                PerformLayout();
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Drop/down animation
            if (_animationProgress < 1.0f)
            {
                // Update progress
                const float openCloseAniamtionTime = 0.2f;
                _animationProgress += deltaTime / openCloseAniamtionTime;
                if (_animationProgress > 1.0f)
                    _animationProgress = 1.0f;

                // Arrange controls
                PerformLayout();
            }
            else
            {
                base.Update(deltaTime);
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            var style = Style.Current;

            // Header
            Render2D.FillRectangle(new Rectangle(0, 0, Width, HeaderHeight), _mouseOverHeader ? style.BackgroundHighlighted : style.BackgroundNormal);

            // Text
            Render2D.DrawText(style.FontMedium, HeaderText, new Rectangle(4, 0, Width - 6, HeaderHeight), Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center);

            // Check if isn't fully closed
            if (!_isClosed || _animationProgress < 0.998f)
            {
                // Draw children with clipping mask (so none of the controls will override the header)
                Rectangle clipMask = new Rectangle(0, HeaderHeight, Width, Height - HeaderHeight);
                Render2D.PushClip(ref clipMask);
                DrawChildren();
                Render2D.PopClip();
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            _mouseOverHeader = HeaderRectangle.Contains(location);

            if (buttons == MouseButtons.Left && _mouseOverHeader)
            {
                _mouseDown = true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _mouseOverHeader = HeaderRectangle.Contains(location);

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            _mouseOverHeader = HeaderRectangle.Contains(location);

            if (buttons == MouseButtons.Left && _mouseDown)
            {
                _mouseDown = false;

                if (_mouseOverHeader)
                {
                    Toggle();
                }
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            _mouseDown = false;
            _mouseOverHeader = false;

            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            base.OnChildResized(control);

            PerformLayout();
        }

        /// <inheritdoc />
        protected override void GetDesireClientArea(out Rectangle rect)
        {
            var topMargin = HeaderHeight + HeaderMargin;
            rect = new Rectangle(0, topMargin, Width, Height - topMargin);
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            // Arrange docked controls
            Rectangle clientArea;
            GetDesireClientArea(out clientArea);
            float dropOffset = _cachedHeight * (_isClosed ? _animationProgress : 1.0f - _animationProgress);
            clientArea.Location.Y -= dropOffset;
            ArrangeDockedControls(ref clientArea);

            // Arrange undocked controls
            float minHeight = HeaderHeight + HeaderMargin;
            float leftMargin = clientArea.Left + LeftMargin;
            float spacing = HeaderMargin;
            float topMargin = clientArea.Top;
            float y = topMargin;
            float height = clientArea.Top + dropOffset;
            float itemsWidth = clientArea.Width - leftMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.DockStyle == DockStyle.None && c.Visible)
                {
                    var h = c.Height;
                    c.Bounds = new Rectangle(leftMargin, y, itemsWidth, h);
                    y += h + spacing;
                    height += h + spacing;
                }
            }

            // Cache calculated height
            _cachedHeight = height;

            // Force to be closed
            if (_animationProgress >= 1.0f && _isClosed)
            {
                y = minHeight;
            }

            // Set height
            Height = Mathf.Max(minHeight, y);
        }
    }
}
