////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Color value editor with picking support.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class ColorValueBox : Control
    {
        /// <summary>
        /// The value.
        /// </summary>
        protected Color _value;

        /// <summary>
        /// Occurs when value gets changed.
        /// </summary>
        public event Action ValueChanged;

        /// <summary>
        /// Gets or sets the color value.
        /// </summary>
        /// <value>
        /// The color value.
        /// </value>
        public Color Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;

                    // Fire event
                    OnValueChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorValueBox"/> class.
        /// </summary>
        public ColorValueBox()
            : base(true, 0, 0, 32, 18)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorValueBox"/> class.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="x">The x location</param>
        /// <param name="y">The y location</param>
        public ColorValueBox(Color value, float x, float y)
            : base(true, x, y, 32, 18)
        {
            _value = value;
        }

        /// <summary>
        /// Called when value gets changed.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke();
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            var style = Style.Current;
            var r = new Rectangle(2, 2, Width - 4, Height - 4);

            Render2D.FillRectangle(r, _value);
            Render2D.DrawRectangle(r, IsMouseOver ? style.BackgroundSelected : Color.Black);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Show color picker dialog
            Style.Current.ShowPickColorDialog?.Invoke(_value, x => Value = x);

            return true;
        }
    }
}
