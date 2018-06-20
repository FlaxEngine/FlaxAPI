// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using FlaxEngine.Assertions;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// GUI root control that is represented by a window or an canvas and can contain children but has no parent at all. It's a source of the input events.
    /// </summary>
    public abstract class RootControl : ContainerControl
    {
        private static ContainerControl _gameRoot;
        private static CanvasContainer _canvasContainer = new CanvasContainer();

        /// <summary>
        /// Gets the main GUI control (it can be window or editor overriden control). Use it to plug-in custom GUI controls.
        /// </summary>
        public static ContainerControl GameRoot
        {
            get => _gameRoot;
            internal set
            {
                _gameRoot = value;
                _canvasContainer.Parent = _gameRoot;
            }
        }

        /// <summary>
        /// Gets the canvas controls root container.
        /// </summary>
        internal static CanvasContainer CanvasRoot => _canvasContainer;

        private Control _focusedControl;
        private Control _trackingControl;

        /// <summary>
        /// Gets or sets the current focused control
        /// </summary>
        public Control FocusedControl
        {
            get => _focusedControl;
            set
            {
                Assert.IsTrue(_focusedControl == null || _focusedControl.Root == this, "Invalid control to focus");
                Focus(value);
            }
        }

        /// <summary>
        /// Gets the tracking mouse offset.
        /// </summary>
        public abstract Vector2 TrackingMouseOffset { get; }

        /// <summary>
        /// Gets or sets the position of the mouse in the window space coordinates.
        /// </summary>
        public abstract Vector2 MousePosition { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootControl"/> class.
        /// </summary>
        protected RootControl()
        : base(0, 0, 100, 60)
        {
            CanFocus = false;
        }

        /// <summary>
        /// Starts the mouse tracking. Used by the scrollbars, splitters, etc.
        /// </summary>
        /// <param name="control">The target control that want to track mouse. It will recive OnMouseMove event.</param>
        /// <param name="useMouseScreenOffset">If set to <c>true</c> will use mouse screen offset.</param>
        public virtual void StartTrackingMouse(Control control, bool useMouseScreenOffset)
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
        }

        /// <summary>
        /// Ends the mouse tracking.
        /// </summary>
        public virtual void EndTrackingMouse()
        {
            if (_trackingControl != null)
            {
                var control = _trackingControl;
                _trackingControl = null;
                control.OnEndMouseCapture();
            }
        }

        /// <summary>
        /// Gets keyboard key state.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True while the user holds down the key identified by id.</returns>
        public abstract bool GetKey(Keys key);

        /// <summary>
        /// Gets keyboard key down state.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True during the frame the user releases the key.</returns>
        public abstract bool GetKeyDown(Keys key);

        /// <summary>
        /// Gets keyboard key up state.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True during the frame the user starts pressing down the key.</returns>
        public abstract bool GetKeyUp(Keys key);

        /// <summary>
        /// Gets mouse button state.
        /// </summary>
        /// <param name="button">Mouse button to check.</param>
        /// <returns>True while the user holds down the button.</returns>
        public abstract bool GetMouseButton(MouseButton button);

        /// <summary>
        /// Gets mouse button down state.
        /// </summary>
        /// <param name="button">Mouse button to check.</param>
        /// <returns>True during the frame the user starts pressing down the button.</returns>
        public abstract bool GetMouseButtonDown(MouseButton button);

        /// <summary>
        /// Gets mouse button up state.
        /// </summary>
        /// <param name="button">Mouse button to check.</param>
        /// <returns>True during the frame the user releases the button.</returns>
        public abstract bool GetMouseButtonUp(MouseButton button);
        
        /// <inheritdoc />
        public override RootControl Root => this;

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
    }
}
