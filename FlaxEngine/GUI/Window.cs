////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Window root control. Main control that is represented by a window and can contain children but has no parent at all.
    /// </summary>
    public abstract class Window : ContainerControl
    {
        private Control _focusedControl;

        /// <summary>
        /// Gets current focused control
        /// </summary>
        public Control FocusedControl
        {
            get { return _focusedControl; }
        }

        /// <summary>
        /// Sets the window title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            set
            {
                throw new NotImplementedException("Window.Title");
            }
        }

        /// <summary>
        /// Init
        /// </summary>
        protected Window()
            : base(false, 0, 0, 100, 60)
        {
        }

        /// <summary>
        /// Function called before window popup used to initialize it's controls
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Starts the mouse tracking.
        /// </summary>
        /// <param name="useMouseScreenOffset">If set to <c>true</c> will apply screen offset to mouse position (during mouse tracing mouse can jump over screen edges).</param>
        public void StartTrackingMouse(bool useMouseScreenOffset)
        {
            throw new NotImplementedException("Window.StartTrackingMouse");
        }

        /// <summary>
        /// Ends the mouse tracking.
        /// </summary>
        public void EndTrackingMouse()
        {
            throw new NotImplementedException("Window.EndTrackingMouse");
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

        public override Vector2 ScreenToClient(Vector2 location)
        {
            throw new NotImplementedException();
            return base.ScreenToClient(location);
        }

        public override Vector2 ClientToScreen(Vector2 location)
        {
            throw new NotImplementedException();
            return base.ClientToScreen(location);
        }

        /// <inheritdoc />
        public override void Focus()
        {
            base.Focus();

            // TODO: focus window
        }

        /// <inheritdoc />
        protected override bool Focus(Control c)
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
            UpdateContainsFocus();

            return true;
        }

        #endregion
    }
}
