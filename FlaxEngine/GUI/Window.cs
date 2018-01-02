////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.CompilerServices;
using FlaxEngine.Assertions;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Window root control. Main control that is represented by a window and can contain children but has no parent at all.
    /// </summary>
    public class Window : ContainerControl
    {
        /// <summary>
        /// Gets the main GUI control (it can be window or editor overriden control). Use it to plug-in custom GUI controls.
        /// </summary>
        public static ContainerControl Root { get; internal set; }

        /// <summary>
        /// Closing window delegate.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="cancel">If set to <c>true</c> closing window will be canceled.</param>
        public delegate void ClosingDelegate(Window window, ClosingReason reason, ref bool cancel);

        private Control _focusedControl;
        private Control _trackingControl;
        private FlaxEngine.Window _window;

        /// <summary>
        /// Gets or sets the current focused control
        /// </summary>
        public Control FocusedControl
        {
            get => _focusedControl;
            set
            {
                Assert.IsTrue(_focusedControl == null || _focusedControl.ParentWindow == this, "Invalid control to focus");
                Focus(value);
            }
        }

        /// <summary>
        /// Sets the window title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return _window.Title; }
            set { _window.Title = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this window is in fullscreen mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is in fullscreen mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullscreen => _window.IsFullscreen;

        /// <summary>
        /// Gets a value indicating whether this window is in windowed mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this window is in windowed mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsWindowed => _window.IsWindowed;

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
        public Vector2 TrackingMouseOffset => _window.TrackingMouseOffset;

        /// <summary>
        /// Gets or sets the position of the mouse in the window space coordinates.
        /// </summary>
        public Vector2 MousePosition
        {
            get => _window.MousePosition;
            set => _window.MousePosition = value;
        }

        /// <summary>
        /// Gets or sets the mouse cursor.
        /// </summary>
        public CursorType Cursor
        {
            get { return _window.Cursor; }
            set { _window.Cursor = value; }
        }

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
            : base(0, 0, 100, 60)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

            CanFocus = false;
            if (Style.Current != null)
                BackgroundColor = Style.Current.Background;
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        public void Show()
        {
            _window.Show();
        }

        /// <summary>
        /// Hides the window.
        /// </summary>
        public void Hide()
        {
            _window.Show();
        }

        /// <summary>
        /// Minimizes the window.
        /// </summary>
        public void Minimize()
        {
            _window.Minimize();
        }

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        public void Maximize()
        {
            _window.Maximize();
        }

        /// <summary>
        /// Restores the window state before minimizing or maximazing.
        /// </summary>
        public void Restore()
        {
            _window.Restore();
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="reason">The closing reason.</param>
        public void Close(ClosingReason reason = ClosingReason.CloseEvent)
        {
            _window.Close(reason);
        }

        /// <summary>
        /// Brings window to the front of the Z order.
        /// </summary>
        /// <param name="force">True if move to the front by force, otheriwse false.</param>
        public void BringToFront(bool force = false)
        {
            _window.BringToFront(force);
        }

        /// <summary>
        /// Flashes the window to bring use attention.
        /// </summary>
        public void FlashWindow()
        {
            _window.FlashWindow();
        }

        /// <summary>
        /// Starts drag and drop operation
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The result.</returns>
        public DragDropEffect DoDragDrop(string data)
        {
            return _window.DoDragDrop(data);
        }

        /// <summary>
        /// Starts the mouse tracking. Used by the scrollbars, splitters, etc.
        /// </summary>
        /// <param name="control">The target control that want to track mouse. It will recive OnMouseMove event.</param>
        /// <param name="useMouseScreenOffset">If set to <c>true</c> will use mouse screen offset.</param>
        public void StartTrackingMouse(Control control, bool useMouseScreenOffset)
        {
            if (control == null)
                throw new ArgumentNullException();
            if (_trackingControl == control)
                return;
            if (_trackingControl != null)
                EndTrackingMouse();
            if (control.CanFocus)
                Focus(control);
            _trackingControl = control;
            _window.StartTrackingMouse(useMouseScreenOffset);
        }

        /// <summary>
        /// Ends the mouse tracking.
        /// </summary>
        public void EndTrackingMouse()
        {
            if (_trackingControl != null)
            {
                var control = _trackingControl;
                _trackingControl = null;
                _window.EndTrackingMouse();
                control.OnEndMouseCapture();
            }
        }

        /// <summary>
        /// Gets keyboard key state.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True while the user holds down the key identified by id.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetKey(Keys key)
        {
            return _window.GetKey(key);
        }

        /// <summary>
        /// Gets keyboard key down state.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True during the frame the user releases the key.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetKeyDown(Keys key)
        {
            return _window.GetKeyDown(key);
        }

        /// <summary>
        /// Gets keyboard key up state.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True during the frame the user starts pressing down the key.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetKeyUp(Keys key)
        {
            return _window.GetKeyUp(key);
        }

        /// <summary>
        /// Gets mouse button state.
        /// </summary>
        /// <param name="button">Mouse button to check.</param>
        /// <returns>True while the user holds down the button.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetMouseButton(MouseButton button)
        {
            return _window.GetMouseButton(button);
        }

        /// <summary>
        /// Gets mouse button down state.
        /// </summary>
        /// <param name="button">Mouse button to check.</param>
        /// <returns>True during the frame the user starts pressing down the button.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetMouseButtonDown(MouseButton button)
        {
            return _window.GetMouseButtonDown(button);
        }

        /// <summary>
        /// Gets mouse button up state.
        /// </summary>
        /// <param name="button">Mouse button to check.</param>
        /// <returns>True during the frame the user releases the button.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetMouseButtonUp(MouseButton button)
        {
            return _window.GetMouseButtonUp(button);
        }

        #region ContainerControl

        /// <inheritdoc />
        public override Window ParentWindow => this;

        /// <inheritdoc />
        public override Vector2 ScreenToClient(Vector2 location)
        {
            return _window.ScreenToClient(location);
        }

        /// <inheritdoc />
        public override Vector2 ClientToScreen(Vector2 location)
        {
            return _window.ClientToScreen(location);
        }

        /// <inheritdoc />
        public override void Focus()
        {
            _window.Focus();
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
                Assert.IsFalse(prevous.IsFocused);
            }
            if (_focusedControl != null)
            {
                _focusedControl.OnGotFocus();
                Assert.IsTrue(_focusedControl.IsFocused);
            }

            // Update flags
            UpdateContainsFocus();

            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            if (_trackingControl != null)
            {
                return _trackingControl.OnMouseDown(_trackingControl.PointFromWindow(location), buttons);
            }

            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            if (_trackingControl != null)
            {
                return _trackingControl.OnMouseUp(_trackingControl.PointFromWindow(location), buttons);
            }

            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            if (_trackingControl != null)
            {
                return _trackingControl.OnMouseDoubleClick(_trackingControl.PointFromWindow(location), buttons);
            }

            return base.OnMouseDoubleClick(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            if (_trackingControl != null)
            {
                return _trackingControl.OnMouseWheel(_trackingControl.PointFromWindow(location), delta);
            }

            return base.OnMouseWheel(location, delta);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            if (_trackingControl != null)
            {
                _trackingControl.OnMouseMove(_trackingControl.PointFromWindow(location));
                return;
            }
            
            base.OnMouseMove(location);
        }

        #endregion
    }
}
