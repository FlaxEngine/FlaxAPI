////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Window root control. Main control that is represented by a window and can contain children but has no parent at all.
    /// </summary>
    public sealed class Window : ContainerControl
    {
        private Control _focusedControl;

        /// <summary>
        /// Gets current focused control
        /// </summary>
        public Control FocusedControl
        {
            get { return _focusedControl; }
        }

        private Window()
            : base(false, 0, 0, 1, 1)
        {
        }

        #region ContainerControl

        /// <inheritdoc />
        public override Window ParentWindow { get { return this; } }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();
            OnLostMouseCapture();
        }

        //Vector2 ScreenToClient(const Vector2& location);
	    //Vector2 ClientToScreen(const Vector2& location);
        
         /// <inheritdoc />
        protected override void OnLocationChange(float x, float y)
        {
            throw new NotImplementedException("Change window position");
        }

        /// <inheritdoc />
        protected override void OnSizeChange(float width, float height)
        {
            throw new NotImplementedException("Change window size");
        }

        /// <inheritdoc />
        protected override void OnVisibleChanged()
        {
            // TODO: should we show/hide the window?
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Focus()
        {
            base.Focus();

            // TODO: focus window
        }

        /// <inheritdoc />
        protected override bool focus(Control c)
        {
            if (IsDisposing || _focusedControl == c)
                return false;

            // Change focused control
            Control prevous = _focusedControl;
            _focusedControl = c;

            // Fire events
            if (prevous != null)
            {
                prevous.OnLostFocus();
                Debug.Assert(!prevous.IsFocused);
            }
            if (_focusedControl != null)
            {
                _focusedControl.OnGotFocus();
                Debug.Assert(_focusedControl.IsFocused);
            }

            // Update flags
            updateContainsFocus();

            return true;
        }

        #endregion
    }
}
