////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Base class for text boxes for float/int value editing. Supports slider and range clamping.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FlaxEngine.GUI.TextBox" />
    public abstract class ValueBox<T> : TextBox where T : struct, IComparable<T>
    {
        /// <summary>
        /// The sliding box size.
        /// </summary>
        protected const float SlidingBoxSize = 12.0f;

        /// <summary>
        /// The current value.
        /// </summary>
        protected T _value;

        /// <summary>
        /// The minimum value.
        /// </summary>
        protected T _min;

        /// <summary>
        /// The maximum value.
        /// </summary>
        protected T _max;

        /// <summary>
        /// The slider speed.
        /// </summary>
        protected float _slideSpeed;

        /// <summary>
        /// True if slider is in use.
        /// </summary>
        protected bool _isSliding;

        /// <summary>
        /// The value cached on sliding start.
        /// </summary>
        protected T _startSlideValue;

        private Vector2 _startSlideLocation;

        /// <summary>
        /// Occurs when value gets changed.
        /// </summary>
        public event Action ValueChanged;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public abstract T Value { get; set; }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public abstract T MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public abstract T MaxValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueBox{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="sliderSpeed">The slider speed.</param>
        protected ValueBox(T value, float x, float y, float width, T min, T max, float sliderSpeed)
            : base(false, x, y, width)
        {
            _value = value;
            _min = min;
            _max = max;
            _slideSpeed = sliderSpeed;
        }

        /// <summary>
        /// Updates the text of the textbox.
        /// </summary>
        protected abstract void UpdateText();

        /// <summary>
        /// Tries the get value from the textbox text.
        /// </summary>
        protected abstract void TryGetValue();

        /// <summary>
        /// Applies the sliding delta to the value.
        /// </summary>
        /// <param name="delta">The delta (scaled).</param>
        protected abstract void ApplySliding(float delta);

        /// <summary>
        /// Called when value gets changed.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke();
        }

        /// <summary>
        /// Gets a value indicating whether this value box can use sliding.
        /// </summary>
        protected virtual bool CanUseSliding => _slideSpeed > Mathf.Epsilon;

        /// <summary>
        /// Gets the slide rectangle.
        /// </summary>
        protected virtual Rectangle SlideRect
        {
            get
            {
                float x = Width - SlidingBoxSize - 1.0f;
                float y = (Height - SlidingBoxSize) * 0.5f;
                return new Rectangle(x, y, SlidingBoxSize, SlidingBoxSize);
            }
        }

        private void endSliding()
        {
            // Clear state
            _isSliding = false;

            // End capturing mouse
            var parentWin = ParentWindow;
            parentWin?.EndTrackingMouse();
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Base
            base.Draw();

            // Check if can slide
            if (CanUseSliding)
            {
                var style = Style.Current;

                // Draw sliding UI
                Render2D.DrawSprite(style.Scale16, SlideRect);

                // Check if is sliding
                if (_isSliding)
                {
                    // Draw overlay
                    // TODO: render nicer overlay with some glow from the borders (inside)
                    Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), Color.Orange * 0.3f, true);
                }
            }
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Check if was sliding
            if (_isSliding)
            {
                endSliding();

                // Base
                base.OnLostFocus();
            }
            else
            {
                // Base
                base.OnLostFocus();

                // Update
                TryGetValue();
                UpdateText();
            }

            ResetViewOffset();
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check if can start sliding
            if (buttons == MouseButtons.Left && CanUseSliding && SlideRect.Contains(location))
            {
                // Start sliding
                _isSliding = true;
                _startSlideLocation = location;
                _startSlideValue = _value;

                // Start capturing mouse
                Focus();
                ParentWindow.StartTrackingMouse(true);
                return true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check if is sliding
            if (_isSliding)
            {
                // Update sliding
                Vector2 slideLocation = location + ParentWindow.TrackingMouseOffset;
                ApplySliding(Mathf.RoundToInt(slideLocation.X - _startSlideLocation.X) * _slideSpeed);
            }
            else
            {
                // Base
                base.OnMouseMove(location);
            }
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left && _isSliding)
            {
                // End sliding
                endSliding();
                return true;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        protected override void OnEditEnd()
        {
            // Update value
            TryGetValue();

            base.OnEditEnd();
        }

        /// <inheritdoc />
        public override bool HasMouseCapture => _isSliding || base.HasMouseCapture;

        /// <inheritdoc />
        public override void OnLostMouseCapture()
        {
            // Check if was sliding
            if (_isSliding)
            {
                endSliding();
            }
            else
            {
                base.OnLostMouseCapture();
            }
        }

        /// <inheritdoc />
        protected override Rectangle TextRectangle
        {
            get
            {
                var result = base.TextRectangle;
                if (CanUseSliding)
                {
                    result.Size.X -= SlidingBoxSize;
                }
                return result;
            }
        }

        /// <inheritdoc />
        protected override Rectangle TextClipRectangle
        {
            get
            {
                var result = base.TextRectangle;
                if (CanUseSliding)
                {
                    result.Size.X -= SlidingBoxSize;
                }
                return result;
            }
        }
    }
}
