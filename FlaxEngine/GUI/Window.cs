////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Window root control. Main control that is represented by a window and can contain children but has no parent at all.
    /// </summary>
    public class Window : ContainerControl
    {
        /// <summary>
        /// Closing window delegate.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="cancel">If set to <c>true</c> closing window will be canceled.</param>
        public delegate void ClosingDelegate(Window window, ClosingReason reason, ref bool cancel);

        private Control _focusedControl;
        private FlaxEngine.Window _window;

        /// <summary>
        /// Event fired when windows wants to be closed. Should return true if suspend window closing, otherwise returns false.
        /// </summary>
        public event ClosingDelegate OnClosing;
        
        /// <summary>
        /// Event fired when left mouse button goes down (hit test performed etc.).
        /// Returns true if event has been processed and further actions should be canceled, otherwise false.
        /// </summary>
        public event Func<WindowHitCodes, bool> OnLButtonHit;

        /// <summary>
        /// Event fired when window performs hit test, parameter is a mouse position
        /// </summary>
        public event Func<Vector2, WindowHitCodes> OnHitTest;
        
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
        /// Gets a value indicating whether this window is in fullscreen mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is in fullscreen mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullscreen => _window.IsFullscreen;

        /// <summary>
        /// Gets a value indicating whether this window is in widowed mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is in widowed mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsWidowed => _window.IsWidowed;

        /// <summary>
        /// Gets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsShown => _window.IsVisible;

        /// <summary>
        /// Gets a value indicating whether this window is minimized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is minimized; otherwise, <c>false</c>.
        /// </value>
        public bool IsMinimized => _window.IsMinimized;

        /// <summary>
        /// Gets a value indicating whether this window is maximized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is maximized; otherwise, <c>false</c>.
        /// </value>
        public bool IsMaximized => _window.IsMaximized;

        /// <summary>
        /// Gets the tracking mouse offset.
        /// </summary>
        /// <value>
        /// The mouse tracking offset.
        /// </value>
        public Vector2 TrackingMouseOffset => throw new NotImplementedException("Window.TrackingMouseOffset");

        /// <summary>
        /// Gets the native window object.
        /// </summary>
        /// <value>
        /// The native window object.
        /// </value>
        public FlaxEngine.Window NativeWindow => _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="window">Native window object.</param>
        internal Window(FlaxEngine.Window window)
            : base(false, 0, 0, 100, 60)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));
            _window = window;
        }

        /// <summary>
        /// Shows this window.
        /// </summary>
        public void Show()
        {
            throw new NotImplementedException("Window.Show");
        }

        /// <summary>
        /// Closes this window.
        /// </summary>
        /// <param name="reason">Window closing reason.</param>
        public void Close(ClosingReason reason)
        {
            throw new NotImplementedException("Window.Close");
        }

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
