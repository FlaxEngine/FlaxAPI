////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Progress bar control shows visual progress of the action or set of actions.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class ProgressBar : Control
    {
        /// <summary>
        /// The value.
        /// </summary>
        protected float _value;

        /// <summary>
        /// The current value (used to apply smooth progress changes).
        /// </summary>
        protected float _current;

        /// <summary>
        /// The minimum progress value.
        /// </summary>
        protected float _minimum;

        /// <summary>
        /// The maximum progress value.
        /// </summary>
        protected float _maximum = 100;

        /// <summary>
        /// Gets or sets the value smoothing scale (0 to not use it).
        /// </summary>
        /// <value>
        /// The value smoothing scale.
        /// </value>
        public float SmoothingScale { get; set; } = 1;

        /// <summary>
        /// Gets a value indicating whether use progress value smoothing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use progress value smoothing; otherwise, <c>false</c>.
        /// </value>
        public bool UseSmoothing => !Mathf.IsZero(SmoothingScale);

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
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
        /// <value>
        /// The maximum value.
        /// </value>
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
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public float Value
        {
            get => _value;
            set
            {
                value = Mathf.Clamp(value, _minimum, _maximum);
                if (!Mathf.NearEqual(value, _value))
                {
                    _value = value;

                    // Check if skip smoothing
                    if (!UseSmoothing)
                    {
                        _current = _value;
                    }
                }
            }
        }

        /// <inheritdoc />
        public ProgressBar(float x, float y, float width, float height = 28)
            : base(false, x, y, width, height)
        {
        }

        /// <inheritdoc />
        public ProgressBar(Vector2 location, Vector2 size)
            : base(false, location, size)
        {
        }

        /// <inheritdoc />
        public ProgressBar(Rectangle bounds)
            : base(false, bounds)
        {
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            bool isDeltaSlow = deltaTime > (1 / 20.0f);

            // Ensure progress bar is visible
            if (Visible)
            {
                // Value smoothing
                if (Mathf.Abs(_current - _value) > 0.01f)
                {
                    // Lerp or not if running slow
                    float value;
                    if (!isDeltaSlow && UseSmoothing)
                        value = Mathf.Lerp(_current, _value, deltaTime * 5.0f * SmoothingScale);
                    else
                        value = _value;
                    _current = value;
                }
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();
            
            var style = Style.Current;
            Render2D.FillRectangle(new Rectangle(0, 0, Width, Height), style.Background);
            Render2D.FillRectangle(new Rectangle(1, 1, (Width - 2) * _current, Height - 2), style.ProgressNormal);
        }
    }
}
