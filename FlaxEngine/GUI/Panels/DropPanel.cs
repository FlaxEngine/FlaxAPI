////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
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
	    protected float _headerHeight = 14.0f;
	    protected Margin _headerMargin = new Margin(2.0f);
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
        public Margin HeaderMargin
        {
            get => _headerMargin;
            set
            {
                _headerMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the color of the header.
        /// </summary>
        /// <value>
        /// The color of the header.
        /// </value>
        public Color HeaderColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the header when mouse is over.
        /// </summary>
        /// <value>
        /// The color of the header when mouse is over.
        /// </value>
        public Color HeaderColorMouseOver { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enable drop down icon drawing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enable drop down icon drawing; otherwise, <c>false</c>.
        /// </value>
        public bool EnableDropDownIcon { get; set; }
        
        /// <summary>
        /// Occurs when drop panel is opened or closed.
        /// </summary>
        public event Action<DropPanel> ClosedChanged;

        /// <summary>
        /// Gets a value indicating whether this panel is closed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this panel is closed; otherwise, <c>false</c>.
        /// </value>
        public bool IsClosed => _isClosed;
		
        /// <summary>
        /// Gets the header rectangle.
        /// </summary>
        protected Rectangle HeaderRectangle => new Rectangle(0, 0, Width, HeaderHeight);

        /// <summary>
        /// Initializes a new instance of the <see cref="DropPanel"/> class.
        /// </summary>
        /// <param name="text">The header text.</param>
        public DropPanel(string text)
            : base(0, 0, 64, 16.0f)
        {
            _performChildrenLayoutFirst = true;
            CanFocus = false;

            HeaderText = text;

            var style = Style.Current;
            HeaderColor = style.BackgroundNormal;
            HeaderColorMouseOver = style.BackgroundHighlighted;
        }

        /// <summary>
        /// Opens the group.
        /// </summary>
        /// <param name="animate">Enable/disable animation feature.</param>
        public void Open(bool animate = true)
        {
            // Check if state will change
            if (_isClosed)
            {
                // Set flag
                _isClosed = false;
                if (animate)
                    _animationProgress = 1 - _animationProgress;
                else
                    _animationProgress = 1.0f;

                // Update
                PerformLayout();

                // Fire event
                ClosedChanged?.Invoke(this);
            }
        }

        /// <summary>
        /// Closes the group.
        /// </summary>
        /// <param name="animate">Enable/disable animation feature.</param>
        public void Close(bool animate = true)
        {
            // Check if state will change
            if (!_isClosed)
            {
                // Set flag
                _isClosed = true;
                if (animate)
                    _animationProgress = 1 - _animationProgress;
                else
                    _animationProgress = 1.0f;

                // Update
                PerformLayout();

                // Fire event
                ClosedChanged?.Invoke(this);
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
        
        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Drop/down animation
            if (_animationProgress < 1.0f)
            {
                bool isDeltaSlow = deltaTime > (1 / 20.0f);

                // Update progress
                if (isDeltaSlow)
                {
                    _animationProgress = 1.0f;
                }
                else
                {
                    const float openCloseAniamtionTime = 0.2f;
                    _animationProgress += deltaTime / openCloseAniamtionTime;
                    if (_animationProgress > 1.0f)
                        _animationProgress = 1.0f;
                }

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
            var color = _mouseOverHeader ? HeaderColorMouseOver : HeaderColor;
            if (color.A > 0)
                Render2D.FillRectangle(new Rectangle(0, 0, Width, HeaderHeight), color);
            
            // Drop down icon
            float textLeft = 0;
            if (EnableDropDownIcon)
            {
                textLeft += 14;
                var dropDownRect = new Rectangle(2, (HeaderHeight - 12) / 2, 12, 12);
                Render2D.DrawSprite(_isClosed ? style.ArrowRight : style.ArrowDown, dropDownRect, _mouseOverHeader ? Color.White : new Color(0.8f, 0.8f, 0.8f, 0.8f));
            }

            // Text
	        var textRect = new Rectangle(textLeft, 0, Width - textLeft, HeaderHeight);
	        _headerMargin.ShrinkRectangle(ref textRect);
			Render2D.DrawText(style.FontMedium, HeaderText, textRect, Enabled ? style.Foreground : style.ForegroundDisabled, TextAlignment.Near, TextAlignment.Center);

            // Draw child controls that are not arranged (pined to the header, etc.)
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (!child.IsScrollable && child.Visible)
                {
                    Render2D.PushTransform(ref child._cachedTransform);
                    child.Draw();
                    Render2D.PopTransform();
                }
            }

            // Check if isn't fully closed
            if (!_isClosed || _animationProgress < 0.998f)
            {
                // Draw children with clipping mask (so none of the controls will override the header)
                Rectangle clipMask = new Rectangle(0, HeaderHeight, Width, Height - HeaderHeight);
                Render2D.PushClip(ref clipMask);
                for (int i = 0; i < _children.Count; i++)
                {
                    var child = _children[i];
                    if (child.IsScrollable && child.Visible)
                    {
                        Render2D.PushTransform(ref child._cachedTransform);
                        child.Draw();
                        Render2D.PopTransform();
                    }
                }
                Render2D.PopClip();
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseDown(location, buttons))
                return true;

            _mouseOverHeader = HeaderRectangle.Contains(location);

            if (buttons == MouseButton.Left && _mouseOverHeader)
            {
                _mouseDown = true;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _mouseOverHeader = HeaderRectangle.Contains(location);

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (base.OnMouseUp(location, buttons))
                return true;
            
            _mouseOverHeader = HeaderRectangle.Contains(location);

            if (buttons == MouseButton.Left && _mouseDown)
            {
                _mouseDown = false;

                if (_mouseOverHeader)
                {
                    Toggle();
                }

                return true;
            }

            return false;
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
            var topMargin = HeaderHeight + _headerMargin.Height;
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
            float minHeight = HeaderHeight + _headerMargin.Height;
            float leftMargin = clientArea.Left + 2.0f;
            float spacing = 2.0f;
            float topMargin = clientArea.Top;
            float y = topMargin;
            float height = clientArea.Top + dropOffset;
            float itemsWidth = clientArea.Width - leftMargin;
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.IsScrollable && c.Visible)
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
