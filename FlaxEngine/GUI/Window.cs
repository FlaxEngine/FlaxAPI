////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using FlaxEngine.Assertions;

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
        public Vector2 TrackingMouseOffset => _window.TrackingMouseOffset;
        
        /// <summary>
        /// Gets or sets the position of the mouse in the window space coordinates.
        /// </summary>
        public bool MousePosition => _window.MousePosition;

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
            : base(false, 0, 0, 100, 60)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));
            _window = window;

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
        /// Starts the mouse tracking.
        /// </summary>
        /// <param name="useMouseScreenOffset">If set to <c>true</c> will use mouse screen offset.</param>
        public void StartTrackingMouse(bool useMouseScreenOffset)
        {
            _window.StartTrackingMouse(useMouseScreenOffset);
        }

        /// <summary>
        /// Ends the mouse tracking.
        /// </summary>
        public void EndTrackingMouse()
        {
            _window.EndTrackingMouse();
        }
        
        #region ContainerControl

        /// <inheritdoc />
        public override Window ParentWindow => this;

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            base.OnMouseLeave();
            OnLostMouseCapture();
        }

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

        #endregion
    }
}