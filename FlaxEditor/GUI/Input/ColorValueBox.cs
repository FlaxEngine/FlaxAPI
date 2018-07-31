// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEditor.GUI.Dialogs;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Color value editor with picking support.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    [HideInEditor]
    public class ColorValueBox : Control
    {
        /// <summary>
        /// Delegate function used for the color picker events handling.
        /// </summary>
        /// <param name="color">The selected color.</param>
        /// <param name="sliding">True if user is using a slider, otherwise false.</param>
        public delegate void ColorPickerEvent(Color color, bool sliding);

        /// <summary>
        /// Delegate function used to handle showing color picking dialog.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        /// <param name="colorChanged">The color changed event.</param>
        /// <param name="useDynamicEditing">True if allow dynamic value editing (slider-like usage), otherwise will fire color change event only on editing end.</param>
        /// <returns>The created color picker dialog or null if failed.</returns>
        public delegate IColorPickerDialog ShowPickColorDialogDelegate(Color initialValue, ColorPickerEvent colorChanged, bool useDynamicEditing = true);

        /// <summary>
        /// Shows picking color dialog (see <see cref="ShowPickColorDialogDelegate"/>).
        /// </summary>
        public static ShowPickColorDialogDelegate ShowPickColorDialog;

        /// <summary>
        /// The current opened dialog.
        /// </summary>
        protected IColorPickerDialog _currentDialog;

        /// <summary>
        /// True if slider is in use.
        /// </summary>
        protected bool _isSliding;

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
        /// Gets a value indicating whether user is using a slider.
        /// </summary>
        public bool IsSliding => _isSliding;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorValueBox"/> class.
        /// </summary>
        public ColorValueBox()
        : base(0, 0, 32, 18)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorValueBox"/> class.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="x">The x location</param>
        /// <param name="y">The y location</param>
        public ColorValueBox(Color value, float x, float y)
        : base(x, y, 32, 18)
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
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Show color picker dialog
            _currentDialog = ShowPickColorDialog?.Invoke(_value, OnColorChanged);

            return true;
        }

        private void OnColorChanged(Color color, bool sliding)
        {
            // HACK: Force send ValueChanged event is sliding state gets modified by the color picker (e.g the color picker window closing event)
            if (_isSliding != sliding)
            {
                _value = _value == Color.Black ? Color.Red : Color.Black;
            }

            _isSliding = sliding;
            Value = color;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (_currentDialog != null)
            {
                _currentDialog.ClosePicker();
                _currentDialog = null;
            }

            base.Dispose();
        }
    }
}
