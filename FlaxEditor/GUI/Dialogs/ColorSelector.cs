////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.GUI.Dialogs
{
    /// <summary>
    /// Color selecting control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class ColorSelector : Control
    {
        private Color _color;

        private Sprite _colorWheelSprite;
        private float _boxSize;
        private float _slidersThickness;
        private Rectangle _boxRect;
        private Rectangle _slider1Rect;
        private Rectangle _slider2Rect;

        private bool _isMouseDownWheel;
        private bool _isMouseDownSlider1;
        private bool _isMouseDownSlider2;

        /// <summary>
        /// Occurs when selected color gets changed.
        /// </summary>
        public event Action<Color> ColorChanged;

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                ColorChanged?.Invoke(_color);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSelector"/> class.
        /// </summary>
        /// <param name="boxSize">Size of the box.</param>
        /// <param name="slidersThickness">The sliders thickness.</param>
        public ColorSelector(float boxSize, float slidersThickness)
            : base(true, 0, 0, 64, 64)
        {
            _boxSize = boxSize;
            _slidersThickness = slidersThickness;
            _colorWheelSprite = Editor.Instance.UI.GetIcon("ColorWheel");

            // Setup dimensions
            UpdateRect();
            Size = new Vector2(_slider2Rect.Right + _boxRect.X, _boxRect.Bottom + _boxRect.Y);
        }

        private void UpdateRect()
        {
            const float slidersMargin = 8.0f;
            _boxRect = new Rectangle(0, 0, _boxSize, _boxSize);
            _slider1Rect = new Rectangle(_boxRect.Right + slidersMargin, _boxRect.Y, _slidersThickness, _boxRect.Height);
            _slider2Rect = new Rectangle(_slider1Rect.Right + slidersMargin, _slider1Rect.Y, _slidersThickness, _slider1Rect.Height);
        }

        private void UpdateMouse(ref Vector2 location)
        {
            if (_isMouseDownSlider1)
            {
                var hsv = _color.ToHSV();
                hsv.Z = 1.001f - Mathf.Saturate((location.Y - _slider1Rect.Y) / _slider1Rect.Height);

                Color = Color.FromHSV(hsv);
            }
            else if (_isMouseDownSlider2)
            {
                var color = _color;
                color.A = 1.0f - Mathf.Saturate((location.Y - _slider2Rect.Y) / _slider2Rect.Height);

                Color = color;
            }
            else if (_isMouseDownWheel)
            {
                Vector2 delta = location - _boxRect.Center;
                float distance = delta.Length;

                float degrees;
                if (Mathf.IsZero(delta.X))
                {
                    // The point is on the y-axis. Determine whether 
                    // it's above or below the x-axis, and return the 
                    // corresponding angle. Note that the orientation of the
                    // y-coordinate is backwards. That is, A positive Y value 
                    // indicates a point BELOW the x-axis.
                    if (delta.Y > 0)
                    {
                        degrees = 270;
                    }
                    else
                    {
                        degrees = 90;
                    }
                }
                else
                {
                    // This value needs to be multiplied
                    // by -1 because the y-coordinate
                    // is opposite from the normal direction here.
                    // That is, a y-coordinate that's "higher" on 
                    // the form has a lower y-value, in this coordinate
                    // system. So everything's off by a factor of -1 when
                    // performing the ratio calculations.
                    degrees = -Mathf.Atan(delta.Y / delta.X) * Mathf.RadiansToDegrees;

                    // If the x-coordinate of the selected point
                    // is to the left of the center of the circle, you 
                    // need to add 180 degrees to the angle. ArcTan only
                    // gives you a value on the right-hand side of the circle.
                    if (delta.X < 0)
                    {
                        degrees += 180;
                    }

                    // Ensure that the return value is between 0 and 360
                    while (degrees > 360)
                        degrees -= 360;
                    while (degrees < 360)

                        degrees += 360;
                }

                var hsv = _color.ToHSV();
                hsv.X = degrees;
                hsv.Y = Mathf.Saturate(distance / (_boxRect.Width * 0.5f));

                Color = Color.FromHSV(hsv);
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Cache data
            var style = Style.Current;
            var hsv = _color.ToHSV();
            var hs = hsv;
            hs.Z = 1.0f;
            Color hsC = Color.FromHSV(hs);
            const float slidersOffset = 3.0f;
            const float slidersThickness = 4.0f;

            // Wheel
            float boxExpand = (2.0f * 4.0f / 128.0f) * _boxRect.Width;
            Render2D.DrawSprite(_colorWheelSprite, _boxRect.MakeExpanded(boxExpand));
            float hAngle = hsv.X * Mathf.DegreesToRadians;
            float hRadius = hsv.Y * _boxRect.Width * 0.5f;
            Vector2 hsPos = new Vector2(hRadius * Mathf.Cos(hAngle), -hRadius * Mathf.Sin(hAngle));
            const float wheelBoxSize = 4.0f;
            Render2D.DrawRectangle(new Rectangle(hsPos - (wheelBoxSize * 0.5f) + _boxRect.Center, new Vector2(wheelBoxSize)), _isMouseDownWheel ? Color.Gray : Color.Black);

            // Value
            float valueY = _slider2Rect.Height * (1 - hsv.Z);
            var valueR = new Rectangle(_slider1Rect.X - slidersOffset, _slider1Rect.Y + valueY - slidersThickness / 2, _slider1Rect.Width + slidersOffset * 2, slidersThickness);
            Render2D.FillRectangle(_slider1Rect, hsC, hsC, Color.Black, Color.Black, true);
            Render2D.DrawRectangle(_slider1Rect, _isMouseDownSlider1 ? style.BackgroundSelected : Color.Black);
            Render2D.DrawRectangle(valueR, _isMouseDownSlider1 ? Color.White : Color.Gray);

            // Alpha
            float alphaY = _slider2Rect.Height * (1 - _color.A);
            var alphaR = new Rectangle(_slider2Rect.X - slidersOffset, _slider2Rect.Y + alphaY - slidersThickness / 2, _slider2Rect.Width + slidersOffset * 2, slidersThickness);
            Render2D.FillRectangle(_slider2Rect, _color, _color, Color.Transparent, Color.Transparent, true);
            Render2D.DrawRectangle(_slider2Rect, _isMouseDownSlider2 ? style.BackgroundSelected : Color.Black);
            Render2D.DrawRectangle(alphaR, _isMouseDownSlider2 ? Color.White : Color.Gray);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear flags
            _isMouseDownWheel = false;
            _isMouseDownSlider1 = false;
            _isMouseDownSlider2 = false;

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            UpdateMouse(ref location);

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left)
            {
                _isMouseDownWheel = _boxRect.Contains(location);
                _isMouseDownSlider1 = _slider1Rect.Contains(location);
                _isMouseDownSlider2 = _slider2Rect.Contains(location);

                UpdateMouse(ref location);
            }

            Focus();
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left)
            {
                // Clear flags
                _isMouseDownWheel = false;
                _isMouseDownSlider1 = false;
                _isMouseDownSlider2 = false;
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool HasMouseCapture => _isMouseDownWheel || _isMouseDownSlider1 || _isMouseDownSlider2 || base.HasMouseCapture;

        /// <inheritdoc />
        public override void OnLostMouseCapture()
        {
            // Clear flags
            _isMouseDownWheel = false;
            _isMouseDownSlider1 = false;
            _isMouseDownSlider2 = false;

            base.OnLostMouseCapture();
        }
    }
}
