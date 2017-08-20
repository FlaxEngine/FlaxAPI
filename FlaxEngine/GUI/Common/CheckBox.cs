////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Check box control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.Control" />
    public class CheckBox : Control
    {
        protected bool _mouseDown;
        protected bool _checked;
        protected bool _intermediate;
        protected bool _mouseOverBox;
        protected float _boxSize;
        protected Rectangle _box;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckBox"/> is checked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is checked; otherwise, <c>false</c>.
        /// </value>
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked != value || _intermediate)
                {
                    // Disable intermidiate value
                    _intermediate = false;

                    _checked = value;
                    CheckChanged?.Invoke();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckBox"/> is in the intermediate state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if checkbox is in the intermediate state; otherwise, <c>false</c>.
        /// </value>
        public bool Intermediate
        {
            get => _intermediate;
            set => _intermediate = value;
        }

        /// <summary>
        /// Gets or sets the size of the box.
        /// </summary>
        /// <value>
        /// The size of the box.
        /// </value>
        public float BoxSize
        {
            get => _boxSize;
            set
            {
                _boxSize = value;
                CacheBox();
            }
        }

        /// <summary>
        /// Event fired when 'checked' state gets changed.
        /// </summary>
        public event Action CheckChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="isChecked">if set to <c>true</c> set checked on start.</param>
        public CheckBox(float x, float y, bool isChecked = false)
            : base(true, x, y, 18, 18)
        {
            _checked = isChecked;
            _boxSize = 16.0f;

            CacheBox();
        }

        /// <summary>
        /// Toggles the checked state.
        /// </summary>
        public void Toggle()
        {
            Checked = !_intermediate && !_checked;
        }

        private void CacheBox()
        {
            _box = new Rectangle(0, (Height - _boxSize) / 2, _boxSize, _boxSize);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Border
            var style = Style.Current;
            Color borderColor = style.BorderNormal;
            if (!Enabled)
                borderColor *= 0.5f;
            else if (_mouseDown || _mouseOverBox)
                borderColor = style.BorderSelected;
            Render2D.DrawRectangle(_box, borderColor);

            // Icon
            if (_intermediate || _checked)
                Render2D.DrawSprite(_checked ? style.CheckBoxTick : style.CheckBoxIntermediate, _box, Enabled ? style.BorderSelected * 1.2f : style.ForegroundDisabled);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            base.OnMouseMove(location);

            _mouseOverBox = _box.Contains(ref location);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left)
            {
                // Set flag
                _mouseDown = true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            if (buttons == MouseButtons.Left && _mouseDown)
            {
                // Clear flag
                _mouseDown = false;

                // Check if mouse is still over the box
                if (_mouseOverBox)
                {
                    Toggle();
                }
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            // Clear flags
            _mouseOverBox = false;
            _mouseDown = false;
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(Vector2 size)
        {
            base.SetSizeInternal(size);

            CacheBox();
        }
    }
}
