// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Scroll Bars base class - allows to scroll contents of the GUI panel.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    [HideInEditor]
    public abstract class ScrollBar : Control
    {
        /// <summary>
        /// The default size.
        /// </summary>
        public const int DefaultSize = 16;

        /// <summary>
        /// The default thickness.
        /// </summary>
        public const int DefaultThickness = 6;

        /// <summary>
        /// The default minimum opacity.
        /// </summary>
        public const float DefaultMinimumOpacity = 0.7f;

        /// <summary>
        /// The default minimum size.
        /// </summary>
        public const int DefaultMinimumSize = 12;

        // Scrolling
        private float _clickChange = 20, _scrollChange = 30;

        private float _minimum, _maximum = 100;
        private float _value, _targetValue;
        private readonly Orientation _orientation;

        // Input
        private float _mouseOffset;

        // Thumb data
        private Rectangle _thumbRect;

        private bool _thumbClicked;
        private float _thumbCenter, _thumbSize;

        // Smoothing
        private float _thumbOpacity = DefaultMinimumOpacity;

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        public Orientation Orientation => _orientation;

        /// <summary>
        /// Gets or sets the value smoothing scale (0 to not use it).
        /// </summary>
        public float SmoothingScale { get; set; } = 1;

        /// <summary>
        /// Gets a value indicating whether use scroll value smoothing.
        /// </summary>
        public bool UseSmoothing => !Mathf.IsZero(SmoothingScale);

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public float Minimum
        {
            get => _minimum;
            set
            {
                if (value > _maximum)
                    throw new ArgumentOutOfRangeException();
                _minimum = value;
                if (Value < _minimum)
                    Value = _minimum;
            }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public float Maximum
        {
            get => _maximum;
            set
            {
                if (value < _minimum)
                    throw new ArgumentOutOfRangeException();
                _maximum = value;
                if (Value > _maximum)
                    Value = _maximum;
            }
        }

        /// <summary>
        /// Gets or sets the scroll value (current, smooth).
        /// </summary>
        public float Value
        {
            get => _value;
            set
            {
                value = Mathf.Clamp(value, _minimum, _maximum);
                if (!Mathf.NearEqual(value, _targetValue))
                {
                    _targetValue = value;

                    // Check if skip smoothing
                    if (!UseSmoothing)
                    {
                        SetValue(value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the target value (target, not smooth).
        /// </summary>
        public float TargetValue => _targetValue;

        /// <summary>
        /// Gets the value slow down.
        /// </summary>
        public float ValueSlowDown => _targetValue - _value;

        /// <summary>
        /// Gets the size of the track.
        /// </summary>
        protected abstract float TrackSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        protected ScrollBar(Orientation orientation, float x, float y, float width, float height)
        : base(x, y, width, height)
        {
            CanFocus = false;

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
            /*else if (Mathf.IsNotInRange(max, viewMin, viewMax))
            {
                Value = max - viewSize;
            }*/
        }

        private void UpdateThumb()
        {
            // Cache data
            float trackSize = TrackSize;
            float range = _maximum - _minimum;
            _thumbSize = Mathf.Min(trackSize, Mathf.Max(trackSize / range * 10.0f, 30.0f));
            float pixelRange = trackSize - _thumbSize;
            float perc = (_value - _minimum) / range;
            float thumbPosition = (int)(perc * pixelRange);
            _thumbCenter = thumbPosition + _thumbSize / 2;

            if (_orientation == Orientation.Vertical)
                _thumbRect = new Rectangle((Width - DefaultThickness) / 2, thumbPosition + 4, DefaultThickness, _thumbSize - 8);
            else
                _thumbRect = new Rectangle(thumbPosition + 4, (Height - DefaultThickness) / 2, _thumbSize - 8, DefaultThickness);
        }

        private void EndTracking()
        {
            // Check flag
            if (_thumbClicked)
            {
                // Clear flag
                _thumbClicked = false;

                // End capturing mouse
                EndMouseCapture();
            }
        }

        internal void Reset()
        {
            _value = _targetValue = 0;
        }

        private void SetValue(float value)
        {
            _value = value;

            // Update
            UpdateThumb();

            // Change parent panel view offset
            if (Parent is Panel panel)
                panel.SetViewOffset(_orientation, _value);
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            bool isDeltaSlow = deltaTime > (1 / 20.0f);

            // Opacity smoothing
            float targetOpacity = Parent.IsMouseOver ? 1.0f : DefaultMinimumOpacity;
            _thumbOpacity = isDeltaSlow ? targetOpacity : Mathf.Lerp(_thumbOpacity, targetOpacity, deltaTime * 10.0f);

            // Ensure scroll bar is visible
            if (Visible)
            {
                // Value smoothing
                if (Mathf.Abs(_targetValue - _value) > 0.01f)
                {
                    // Interpolate or not if running slow
                    float value;
                    if (!isDeltaSlow && UseSmoothing)
                        value = Mathf.Lerp(_value, _targetValue, deltaTime * 20.0f * SmoothingScale);
                    else
                        value = _targetValue;
                    SetValue(value);
                }
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;

            // Draw track line
            var lineRect = _orientation == Orientation.Vertical ? new Rectangle(Width / 2, 4, 1, Height - 8) : new Rectangle(4, Height / 2, Width - 8, 1);
            Render2D.FillRectangle(lineRect, style.BackgroundHighlighted * _thumbOpacity);

            // Draw thumb
            Render2D.FillRectangle(_thumbRect, (_thumbClicked ? style.BackgroundSelected : style.BackgroundNormal) * _thumbOpacity);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            EndTracking();

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            if (_thumbClicked)
            {
                Vector2 slidePosition = location + Root.TrackingMouseOffset;
                if (Parent is Panel panel)
                    slidePosition += panel.ViewOffset; // Hardcoded fix
                float mousePosition = _orientation == Orientation.Vertical ? slidePosition.Y : slidePosition.X;

                float percentage = (mousePosition - _mouseOffset - _thumbSize / 2) / (TrackSize - _thumbSize);
                Value = percentage * _maximum;
            }
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            // Scroll
            Value = _value - delta * _scrollChange;
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                // Remove focus
                var parentWin = Root;
                parentWin.FocusedControl?.Defocus();

                float mousePosition = _orientation == Orientation.Vertical ? location.Y : location.X;

                if (_thumbRect.Contains(ref location))
                {
                    // Start moving thumb
                    _thumbClicked = true;
                    _mouseOffset = mousePosition - _thumbCenter;

                    // Start capturing mouse
                    StartMouseCapture();
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
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            EndTracking();

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnEndMouseCapture()
        {
            EndTracking();
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(ref Vector2 size)
        {
            base.SetSizeInternal(ref size);

            UpdateThumb();
        }
    }
}
