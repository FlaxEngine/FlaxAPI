// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Globalization;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Float value editor with fixed size text box and slider.
    /// </summary>
    [HideInEditor]
    public class SliderControl : ContainerControl
    {
        /// <summary>
        /// The horizontal slider control.
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.Control" />
        [HideInEditor]
        protected class Slider : Control
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
            /// The default minimum size.
            /// </summary>
            public const int DefaultMinimumSize = 12;

            /// <summary>
            /// The minimum value (constant)
            /// </summary>
            public const float Minimum = 0.0f;

            /// <summary>
            /// The maximum value (constant).
            /// </summary>
            public const float Maximum = 100.0f;

            private float _value;
            private float _mouseOffset;

            // Thumb data
            private Rectangle _thumbRect;

            private bool _thumbClicked;
            private float _thumbCenter, _thumbSize;

            /// <summary>
            /// Gets or sets the value (normalized to range 0-100).
            /// </summary>
            public float Value
            {
                get => _value;
                set
                {
                    value = Mathf.Clamp(value, Minimum, Maximum);
                    if (!Mathf.NearEqual(value, _value))
                    {
                        _value = value;

                        // Update
                        updateThumb();
                        ValueChanged?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Occurs when value gets changed.
            /// </summary>
            public Action ValueChanged;

            /// <summary>
            /// Gets a value indicating whether user is using a slider.
            /// </summary>
            public bool IsSliding => _thumbClicked;

            /// <summary>
            /// Occurs when sliding starts.
            /// </summary>
            public Action SlidingStart;

            /// <summary>
            /// Occurs when sliding ends.
            /// </summary>
            public Action SlidingEnd;

            /// <summary>
            /// Initializes a new instance of the <see cref="Slider"/> class.
            /// </summary>
            /// <param name="width">The width.</param>
            /// <param name="height">The height.</param>
            public Slider(float width, float height)
            : base(0, 0, width, height)
            {
                CanFocus = false;
            }

            private void updateThumb()
            {
                // Cache data
                float trackSize = TrackSize;
                float range = Maximum - Minimum;
                _thumbSize = Mathf.Min(trackSize, Mathf.Max(trackSize / range * 10.0f, 30.0f));
                float pixelRange = trackSize - _thumbSize;
                float perc = (_value - Minimum) / range;
                float thumbPosition = (int)(perc * pixelRange);
                _thumbCenter = thumbPosition + _thumbSize / 2;
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

                    SlidingEnd?.Invoke();
                }
            }

            /// <summary>
            /// Gets the size of the track.
            /// </summary>
            private float TrackSize => Width;

            /// <inheritdoc />
            public override void Draw()
            {
                base.Draw();

                var style = Style.Current;

                // Draw track line
                var lineRect = new Rectangle(4, Height / 2, Width - 8, 1);
                Render2D.FillRectangle(lineRect, style.BackgroundHighlighted);

                // Draw thumb
                Render2D.FillRectangle(_thumbRect, _thumbClicked ? style.BackgroundSelected : style.BackgroundNormal);
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
                    Vector2 slidePosition = location + Root.TrackingMouseOffset;
                    float mousePosition = slidePosition.X;

                    float perc = (mousePosition - _mouseOffset - _thumbSize / 2) / (TrackSize - _thumbSize);
                    Value = perc * Maximum;
                }
            }

            /// <inheritdoc />
            public override bool OnMouseDown(Vector2 location, MouseButton buttons)
            {
                if (buttons == MouseButton.Left)
                {
                    // Remove focus
                    var parentWin = Root;
                    parentWin.FocusedControl?.Defocus();

                    float mousePosition = location.X;

                    if (_thumbRect.Contains(ref location))
                    {
                        // Start moving thumb
                        _thumbClicked = true;
                        _mouseOffset = mousePosition - _thumbCenter;

                        // Start capturing mouse
                        StartMouseCapture();

                        SlidingStart?.Invoke();
                    }
                    else
                    {
                        // Click change
                        Value += (mousePosition < _thumbCenter ? -1 : 1) * 10f;
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
            protected override void SetSizeInternal(ref Vector2 size)
            {
                base.SetSizeInternal(ref size);

                updateThumb();
            }
        }

        /// <summary>
        /// The slider.
        /// </summary>
        protected Slider _slider;

        /// <summary>
        /// The text box.
        /// </summary>
        protected TextBox _textBox;

        /// <summary>
        /// The text box size (rest will be the slider area).
        /// </summary>
        protected const float TextBoxSize = 30.0f;

        private float _value;
        private float _min, _max;

        private bool _valueIsChanging;

        /// <summary>
        /// Occurs when value gets changed.
        /// </summary>
        public event Action ValueChanged;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float Value
        {
            get => _value;
            set
            {
                value = Mathf.Clamp(value, _min, _max);
                if (Math.Abs(_value - value) > Mathf.Epsilon)
                {
                    // Set value
                    _value = value;

                    // Update
                    _valueIsChanging = true;
                    UpdateText();
                    UpdateSlider();
                    _valueIsChanging = false;
                    OnValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public float MinValue
        {
            get => _min;
            set
            {
                if (!Mathf.NearEqual(_min, value))
                {
                    if (value > _max)
                        throw new ArgumentException();

                    _min = value;
                    Value = Value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public float MaxValue
        {
            get => _max;
            set
            {
                if (!Mathf.NearEqual(_max, value))
                {
                    if (value < _min)
                        throw new ArgumentException();

                    _max = value;
                    Value = Value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether user is using a slider.
        /// </summary>
        public bool IsSliding => _slider.IsSliding;

        /// <summary>
        /// Occurs when sliding starts.
        /// </summary>
        public event Action SlidingStart;

        /// <summary>
        /// Occurs when sliding ends.
        /// </summary>
        public event Action SlidingEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderControl"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="x">The position x.</param>
        /// <param name="y">The position y.</param>
        /// <param name="width">The width.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public SliderControl(float value, float x = 0, float y = 0, float width = 120, float min = Single.MinValue, float max = Single.MaxValue)
        : base(x, y, width, TextBox.DefaultHeight)
        {
            _min = min;
            _max = max;
            _value = Mathf.Clamp(value, min, max);

            float split = Width - TextBoxSize;
            _slider = new Slider(split, Height)
            {
                Parent = this,
            };
            _slider.ValueChanged += SliderOnValueChanged;
            _slider.SlidingStart += SlidingStart;
            _slider.SlidingEnd += SlidingEnd;
            _textBox = new TextBox(false, split, Height, TextBoxSize)
            {
                Parent = this
            };
            _textBox.EditEnd += TextBoxOnEditEnd;
        }

        private void SliderOnValueChanged()
        {
            if (_valueIsChanging)
                return;

            Value = Mathf.Map(_slider.Value, Slider.Minimum, Slider.Maximum, MinValue, MaxValue);
        }

        private void TextBoxOnEditEnd()
        {
            if (_valueIsChanging)
                return;

            var text = _textBox.Text.Replace(',', '.');
            if (double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var value))
            {
                Value = (float)Math.Round(value, 5);
            }
            else
            {
                UpdateText();
            }
        }

        /// <summary>
        /// Sets the limits from the attribute.
        /// </summary>
        /// <param name="limits">The limits.</param>
        public void SetLimits(RangeAttribute limits)
        {
            _min = limits.Min;
            _max = Mathf.Max(_min, limits.Max);
            Value = Value;
        }

        /// <summary>
        /// Updates the text of the textbox.
        /// </summary>
        protected virtual void UpdateText()
        {
            _textBox.Text = _value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Updates the slider value.
        /// </summary>
        protected virtual void UpdateSlider()
        {
            _slider.Value = Mathf.Map(_value, MinValue, MaxValue, Slider.Minimum, Slider.Maximum);
        }

        /// <summary>
        /// Called when value gets changed.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke();
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            base.PerformLayoutSelf();

            float split = Width - TextBoxSize;
            _slider.Bounds = new Rectangle(0, 0, split, Height);
            _textBox.Bounds = new Rectangle(split, 0, TextBoxSize, Height);
        }
    }
}
