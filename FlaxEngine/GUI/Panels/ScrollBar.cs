// Flax Engine scripting API

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Scroll Bars base class - allows to scroll contents of the GUI panel.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public abstract class ScrollBar : Control
    {
        // Scroll bars options
        public const int DefaultSize = 16;
        public const int DefaultThickness = 6;
        public const float DefaultMinimumOpacity = 0.4f;
        public const int DefaultMinimumSize = 12;

        // Scrolling
        private float _clickChange = 20, _scrollChange = 30;
        private float _minimum, _maximum = 100;
        private float _value, _targetValue;
        private Orientation _orientation;

        // Input
        private float _mouseOffset;

        // Thumb data
        private Rectangle _thumbRect;
        private  bool _thumbClicked;
        private float _thumbCenter, _thumbSize;

        // Smoothing
        private float _thumbOpacity = DefaultMinimumOpacity;

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public Orientation Orientation => _orientation;

        /// <summary>
        /// Gets or sets the value smoothing scale (0 to not use it).
        /// </summary>
        /// <value>
        /// The value smoothing scale.
        /// </value>
        public float SmoothingScale { get; set; } = 1;

        /// <summary>
        /// Gets a value indicating whether use scroll value smoothing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use scroll value smoothing; otherwise, <c>false</c>.
        /// </value>
        public bool UseSmoothing => Mathf.IsZero(SmoothingScale);

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public float Minimum
        {
            get { return _minimum; }
            set
            {
                if (value > _maximum)
                    throw new ArgumentOutOfRangeException("Invalid minimum value.");
                _minimum = value;
                Value = value;
                updateThumb();
            }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public float Maximum
        {
            get { return _maximum; }
            set
            {
                if (value < _minimum)
                    throw new ArgumentOutOfRangeException("Invalid maximum value.");
                _maximum = value;
                Value = value;
                updateThumb();
            }
        }

        /// <summary>
        /// Gets or sets the scroll value (current, smooth).
        /// </summary>
        /// <value>
        /// The scroll value.
        /// </value>
        public float Value
        {
            get { return _value; }
            set
            {
                value = Mathf.Clamp(value, _minimum, _maximum);
                if (!Mathf.NearEqual(value, _targetValue))
                {
                    _targetValue = value;

                    // Check if skip smoothing
                    if (!UseSmoothing)
                        _value = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the target value (target, not smooth).
        /// </summary>
        /// <value>
        /// The target value.
        /// </value>
        public float TargetValue => _targetValue;
        
        /// <summary>
        /// Gets the value slow down.
        /// </summary>
        /// <value>
        /// The value slow down.
        /// </value>
        public float ValueSlowDown => _targetValue - _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected ScrollBar(Orientation orientation, float x, float y, float width, float height)
            : base(false, x, y, width, height)
        {
            _orientation = orientation;
        }
        
        /// <summary>
        /// Scrolls the view to the desire range (favors minimum value if cannot cover whole range in a bounds).
        /// </summary>
        /// <param name="min">The view minimum.</param>
        /// <param name="max">The view maximum.</param>
        public void ScrollViewTo(float min, float max)
        {
            // Check if we need to change view
            float viewMin = _value;
            float viewSize = TrackSize;
            float viewMax = viewMin + viewSize;
            if (Mathf.IsNotInRange(min, viewMin, viewMax))
            {
                Value = min;
            }
            else if (Mathf.IsNotInRange(max, viewMin, viewMax))
            {
                Value = max - viewSize;
            }
        }
        
        private void updateThumb()
        {
            // Cache data
            float trackSize = TrackSize;
            _thumbSize = Mathf.Min(trackSize, Mathf.Max(trackSize / _maximum * 10.0f, 30.0f));
            float pixelRange = trackSize - _thumbSize;
            float perc = _value / _maximum;
            float thumbPosition = (int)(perc * pixelRange);
            _thumbCenter = thumbPosition + _thumbSize / 2;

            if (_orientation == Orientation.Vertical)
                _thumbRect = new Rectangle((Width - DefaultThickness) / 2, thumbPosition + 4, DefaultThickness, _thumbSize - 8);
            else
                _thumbRect = new Rectangle(thumbPosition + 4, (Height - DefaultThickness) / 2, _thumbSize - 8, DefaultThickness);
        }

        private void StartTracking()
        {
            // Remove focus
            var parentWin = ParentWindow;
            if(parentWin.FocusedControl != null)
                parentWin.FocusedControl.Defocus();

            // Start moving thumb
            _thumbClicked = true;

            // Start capturing mouse
            parentWin.StartTrackingMouse(false);
        }

        private void EndTracking()
        {
            // Check flag
            if (_thumbClicked)
            {
                // Clear flag
                _thumbClicked = false;

                // End capturing mouse
                var parentWin = ParentWindow;
                if (parentWin != null)
                    parentWin.EndTrackingMouse();
            }
        }

        /// <summary>
        /// Gets the size of the track.
        /// </summary>
        /// <value>
        /// The size of the track.
        /// </value>
        protected abstract float TrackSize { get; }

        internal void Reset()
        {
            _value = _targetValue = 0;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            bool isDeltaSlow = deltaTime < (1 / 20.0f);

            // Opacity smoothing
            float targetOpacity = Parent.IsMouseOver ? 1.0f : DefaultMinimumOpacity;
            if (isDeltaSlow)
                _thumbOpacity = targetOpacity;
            else
                _thumbOpacity = Mathf.Lerp(_thumbOpacity, targetOpacity, deltaTime * 10.0f);

            // Ensure scroll bar is visible
            if (Visible)
            {
                // Value smoothing
                if (Mathf.Abs(_targetValue - _value) > 0.01f)
                {
                    // Lerp or not if running slow
                    if (isDeltaSlow)
                        _value = Mathf.Lerp(_value, _targetValue, deltaTime * 20.0f);
                    else
                        _value = _targetValue;

                    // Update
                    updateThumb();

                    // Change parent panel view offset
                    var panel = Parent as Panel;
                    panel.setViewOffset(_orientation, _value);
                }
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;

            // Draw track line
            var lineRect = _orientation == Orientation.Vertical ? new Rectangle(Width / 2, 4, 1, Height - 8) : new Rectangle(4, Height / 2, Width - 8, 1);
            Render2D.FillRectangle(lineRect, style.BackgroundHighlighted * _thumbOpacity, _thumbOpacity < 0.99f);

            // Draw thumb
            Render2D.FillRectangle(_thumbRect, (_thumbClicked ? style.BackgroundSelected : style.BackgroundNormal) * _thumbOpacity, _thumbOpacity < 0.99f);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            base.OnLostFocus();

            EndTracking();
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            if (_thumbClicked)
            {
                Vector2 slidePosition = location + ParentWindow.TrackingMouseOffset;
                float mousePosition = _orientation == Orientation.Vertical ? slidePosition.Y : slidePosition.X;

                float perc = (mousePosition - _mouseOffset - _thumbSize / 2) / (TrackSize - _thumbSize);
                Value = perc * _maximum;
            }
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, int delta)
        {
            // Scroll
            Value = _value + (delta > 0 ? -_scrollChange : _scrollChange);
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left)
            {
                float mousePosition = _orientation == Orientation.Vertical ? location.Y : location.X;

                if (_thumbRect.Contains(location))
                {
                    // Start capturing mouse
                    _mouseOffset = mousePosition - _thumbCenter;
                    StartTracking();
                }
                else
                {
                    // Click change
                    Value = _value + (mousePosition < _thumbCenter ? -1 : 1) * _clickChange;
                }
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            EndTracking();

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(Vector2 size)
        {
            base.SetSizeInternal(size);
            updateThumb();
        }
    }
}
