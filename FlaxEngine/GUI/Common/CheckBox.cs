////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
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
        /// <summary>
        /// The mouse is down.
        /// </summary>
        protected bool _mouseDown;

        /// <summary>
        /// The checked state.
        /// </summary>
        protected bool _checked;

        /// <summary>
        /// The intermediate state.
        /// </summary>
        protected bool _intermediate;

        /// <summary>
        /// The mouse over box state.
        /// </summary>
        protected bool _mouseOverBox;

        /// <summary>
        /// The box size.
        /// </summary>
        protected float _boxSize;

        /// <summary>
        /// The box rectangle.
        /// </summary>
        protected Rectangle _box;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckBox"/> is checked.
        /// </summary>
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
                    CheckChanged?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckBox"/> is in the intermediate state.
        /// </summary>
        public bool Intermediate
        {
            get => _intermediate;
            set => _intermediate = value;
        }

        /// <summary>
        /// Gets or sets the size of the box.
        /// </summary>
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
        public event Action<CheckBox> CheckChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBox"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="isChecked">if set to <c>true</c> set checked on start.</param>
        public CheckBox(float x, float y, bool isChecked = false)
            : base(x, y, 18, 18)
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

            bool enabled = EnabledInHierarchy;

            // Border
            var style = Style.Current;
            Color borderColor = style.BorderNormal;
            if (!enabled)
                borderColor *= 0.5f;
            else if (_mouseDown || _mouseOverBox)
                borderColor = style.BorderSelected;
            Render2D.DrawRectangle(_box, borderColor);

            // Icon
            if (_intermediate || _checked)
                Render2D.DrawSprite(_checked ? style.CheckBoxTick : style.CheckBoxIntermediate, _box, enabled ? style.BorderSelected * 1.2f : style.ForegroundDisabled);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            base.OnMouseMove(location);

            _mouseOverBox = _box.Contains(ref location);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left)
            {
                // Set flag
                _mouseDown = true;
	            Focus();
	            return true;
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (buttons == MouseButton.Left && _mouseDown)
            {
                // Clear flag
                _mouseDown = false;

                // Check if mouse is still over the box
                if (_mouseOverBox)
                {
                    Toggle();
	                Focus();
	                return true;
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
