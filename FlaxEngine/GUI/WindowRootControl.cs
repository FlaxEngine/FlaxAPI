// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Root control implementation used by the <see cref="FlaxEngine.Window"/>.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.RootControl" />
    public sealed class WindowRootControl : RootControl
    {
        private Window _window;

        /// <summary>
        /// Gets the native window object.
        /// </summary>
        public Window Window => _window;

        /// <summary>
        /// Sets the window title.
        /// </summary>
        public string Title
        {
            get => _window.Title;
            set => _window.Title = value;
        }

        /// <summary>
        /// Gets a value indicating whether this window is in fullscreen mode.
        /// </summary>
        public bool IsFullscreen => _window.IsFullscreen;

        /// <summary>
        /// Gets a value indicating whether this window is in windowed mode.
        /// </summary>
        public bool IsWindowed => _window.IsWindowed;

        /// <summary>
        /// Gets a value indicating whether this instance is visible.
        /// </summary>
        public bool IsShown => _window.IsVisible;

        /// <summary>
        /// Gets a value indicating whether this window is minimized.
        /// </summary>
        public bool IsMinimized => _window.IsMinimized;

        /// <summary>
        /// Gets a value indicating whether this window is maximized.
        /// </summary>
        public bool IsMaximized => _window.IsMaximized;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowRootControl"/> class.
        /// </summary>
        /// <param name="window">Native window object.</param>
        internal WindowRootControl(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

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

        /// <inheritdoc />
        public override CursorType Cursor
        {
            get => _window.Cursor;
            set => _window.Cursor = value;
        }

        /// <inheritdoc />
        public override Vector2 TrackingMouseOffset => _window.TrackingMouseOffset;

        /// <inheritdoc />
        public override Vector2 MousePosition
        {
            get => _window.MousePosition;
            set => _window.MousePosition = value;
        }

        /// <inheritdoc />
        public override void StartTrackingMouse(Control control, bool useMouseScreenOffset)
        {
            base.StartTrackingMouse(control, useMouseScreenOffset);

            _window.StartTrackingMouse(useMouseScreenOffset);
        }

        /// <inheritdoc />
        public override void EndTrackingMouse()
        {
            base.EndTrackingMouse();

            _window.EndTrackingMouse();
        }

        /// <inheritdoc />
        public override bool GetKey(Keys key)
        {
            return _window.GetKey(key);
        }

        /// <inheritdoc />
        public override bool GetKeyDown(Keys key)
        {
            return _window.GetKeyDown(key);
        }

        /// <inheritdoc />
        public override bool GetKeyUp(Keys key)
        {
            return _window.GetKeyUp(key);
        }

        /// <inheritdoc />
        public override bool GetMouseButton(MouseButton button)
        {
            return _window.GetMouseButton(button);
        }

        /// <inheritdoc />
        public override bool GetMouseButtonDown(MouseButton button)
        {
            return _window.GetMouseButtonDown(button);
        }

        /// <inheritdoc />
        public override bool GetMouseButtonUp(MouseButton button)
        {
            return _window.GetMouseButtonUp(button);
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
        public override void DoDragDrop(DragData data)
        {
            _window.DoDragDrop(data);
        }
    }
}
