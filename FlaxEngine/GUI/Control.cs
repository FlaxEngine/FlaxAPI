////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    ///     Base class for all GUI controls
    /// </summary>
    public partial class Control
    {
        private ContainerControl _parent;
        private bool _isDisposing, _isFocused;

        // State
        private bool _isMouseOver, _isDragOver;

        // Dimensions
        private Rectangle _bounds;

        // Properties
        private bool _isVisible = true;
        private bool _isEnabled = true;
        private bool _canFocus;

        private string _tooltipText;
        private DockStyle _dockStyle;
        private Color _backgroundColor = Color.Transparent;

        /// <summary>
        ///     Action is invoked, when location is changed
        /// </summary>
        public event Action<Control> OnLocationChanged;

        /// <summary>
        ///     Action is invoked, when size is changed
        /// </summary>
        public event Action<Control> OnSizeChanged;

        /// <summary>
        ///     Action is invoked, when parent is changed
        /// </summary>
        public event Action<Control> OnParentChanged;

        /// <summary>
        ///     Action is invoked, when visibility is changed
        /// </summary>
        public event Action<Control> OnVisibleChanged;

        #region Public Properties

        /// <summary>
        ///     Parent control (the one above in the tree hierachy)
        /// </summary>
        public ContainerControl Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value)
                {
                    return;
                }
                _parent?.RemoveChild(this);

                Defocus();
                _parent = value;
                _parent?.AddChild(this);

                OnParentChanged?.Invoke(this);
            }
        }

        /// <summary>
        ///     Checks if control has parent container control
        /// </summary>
        public bool HasParent => _parent != null;

        /// <summary>
        ///     Gets zero-based index of the control inside the parent container list
        /// </summary>
        public int IndexInParent => HasParent ? _parent.GetChildIndex(this) : -1;

        /// <summary>
        ///     Gets or sets control background color (transparent color (alpha=0) means no background rendering)
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        /// <summary>
        ///     Gets the docking style of the control
        /// </summary>
        /// <returns>Dock style of the control</returns>
        public DockStyle DockStyle
        {
            get { return _dockStyle; }
            set
            {
                if (_dockStyle != value)
                {
                    _dockStyle = value;

                    // Update parent's container
                    _parent?.PerformLayout();
                }
            }
        }

        /// <summary>
        ///     Returns true if control can use scrolling feature
        /// </summary>
        public virtual bool IsScrollable => _dockStyle == DockStyle.None;

        /// <summary>
        ///     Gets or sets a value indicating whether the control can respond to user interaction
        /// </summary>
        public bool Enabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;

                    // Check if control has been disabled
                    if (!_isEnabled)
                    {
                        // Clear flags
                        //_mouseOver = false;
                        //_dragOver = false;
                        // TODO: should we call mosue leave or sth?
                    }
                }
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the control is visible
        /// </summary>
        public bool Visible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;

                    // Check if control hasn't been hidden
                    if (!_isVisible)
                    {
                        /*// Defocus itself
                        if (ContainsFocus() && !IsFocused())
                            Focus();
                        Defocus();

                        // Clear flags
                        _mouseOver = false;
                        _dragOver = false;*/
                        // TODO: call events?
                    }

                    OnVisibleChanged?.Invoke(this);
                    if (HasParent)
                    {
                        _parent.PerformLayout();
                    }
                }
            }
        }

        /// <summary>
        ///     Returns true if control is during disposing state (on destroy)
        /// </summary>
        public bool IsDisposing => _isDisposing;

        /// <summary>
        ///     Gets window which contains that control (or null if not linked to any)
        /// </summary>
        public virtual Window ParentWindow => HasParent ? _parent.ParentWindow : null;

        /// <summary>
        /// Gets screen position of the control (upper left corner).
        /// </summary>
        /// <returns>Screen position of the control.</returns>
        public Vector2 ScreenPos
        {
            get
            {
                var parentWin = ParentWindow;
                if (parentWin == null)
                    throw new InvalidOperationException("Missing parent window.");
                var clientPos = PointToWindow(Vector2.Zero);
                return parentWin.ClientToScreen(clientPos);
            }
        }

        #endregion

        /// <summary>
        ///     Init
        /// </summary>
        /// <param name="canFocus">True if control can accept user focus</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        protected Control(bool canFocus, float x, float y, float width, float height)
            : this(canFocus, new Vector2(x, y), new Vector2(width, height))
        {
        }

        /// <summary>
        ///     Init
        /// </summary>
        /// <param name="canFocus">True if control can accept user focus</param>
        /// <param name="location">Upper left corner location.</param>
        /// <param name="size">Bounds size.</param>
        protected Control(bool canFocus, Vector2 location, Vector2 size)
            : this(canFocus, new Rectangle(location, size))
        {
        }

        /// <summary>
        ///     Init
        /// </summary>
        /// <param name="canFocus">True if control can accept user focus</param>
        /// <param name="bounds">Window bounds</param>
        protected Control(bool canFocus, Rectangle bounds)
        {
            _canFocus = canFocus;
            _bounds = bounds;
        }

        /// <summary>
        ///     Delete control (will unlink from the parent and start to dispose)
        /// </summary>
        public virtual void Dispose()
        {
            if (_isDisposing)
            {
                return;
            }

            // Set disposing flag
            _isDisposing = true;

            // Call event
            OnDestroy();

            // Unlink
            Parent = null;
        }

        /// <summary>
        ///     Perform control update and all its children
        /// </summary>
        /// <param name="deltaTime">Delta time in seconds</param>
        public virtual void Update(float deltaTime)
        {
            // Update tooltip
            //if (_tooltip && _mouseOver)
            //     _tooltip->OnControlMouseOver(this, dt);
        }

        /// <summary>
        ///     Draw control
        /// </summary>
        public virtual void Draw()
        {
            if (!Visible)
            {
                return;
            }

            // Paint Background
            if (_backgroundColor.A > 0.0f)
            {
                Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), _backgroundColor, true);
            }
        }

        /// <summary>
        ///     Update control layout
        /// </summary>
        public virtual void PerformLayout()
        {
        }

        /// <summary>
        ///     Sets control's location
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public virtual void SetLocation(float x, float y)
        {
            SetLocation(new Vector2(x, y));
        }

        /// <summary>
        ///     Sets control's location
        /// </summary>
        /// <param name="location">Location coordinates of the upper left corner</param>
        public virtual void SetLocation(Vector2 location)
        {
            if (!_bounds.Location.Equals(location))
            {
                SetLocationInternal(location);
            }
        }

        /// <summary>
        ///     Sets control's size
        /// </summary>
        /// <param name="width">Control's width</param>
        /// <param name="height">Control's height</param>
        public virtual void SetSize(float width, float height)
        {
            SetSize(new Vector2(width, height));
        }

        /// <summary>
        ///     Sets control's size
        /// </summary>
        /// <param name="size">Control's size</param>
        public virtual void SetSize(Vector2 size)
        {
            if (!_bounds.Size.Equals(size))
            {
                SetSizeInternal(size);
            }
        }

        #region Focus

        /// <summary>
        ///     Gets a value indicating whether the control can receive focus
        /// </summary>
        /// <returns>True if the control can receive focus, otherwise false</returns>
        public bool CanFocus => _canFocus;

        /// <summary>
        ///     Gets a value indicating whether the control, currently has the input focus
        /// </summary>
        /// <returns>True if the control, currently has the input focus</returns>
        public virtual bool ContainsFocus => _isFocused;

        /// <summary>
        ///     Gets a value indicating whether the control has input focus
        /// </summary>
        /// <returns>True if control has input focus, otherwise false</returns>
        public virtual bool IsFocused => _isFocused;

        /// <summary>
        ///     Sets input focus to the control
        /// </summary>
        public virtual void Focus()
        {
            if (!IsFocused)
            {
                Focus(this);
            }
        }

        /// <summary>
        ///     Removes input focus from the control
        /// </summary>
        public virtual void Defocus()
        {
            if (IsFocused)
            {
                Focus(null);
            }
        }

        /// <summary>
        ///     When control gets input focus
        /// </summary>
        public virtual void OnGotFocus()
        {
            // Cache flag
            _isFocused = true;
        }

        /// <summary>
        ///     When control losts input focus
        /// </summary>
        public virtual void OnLostFocus()
        {
            // Clear flag
            _isFocused = false;
        }

        /// <summary>
        ///     Action fired when control gets 'Contains Focus' state
        /// </summary>
        public virtual void OnStartContainsFocus()
        {
        }

        /// <summary>
        ///     Action fired when control losts 'Contains Focus' state
        /// </summary>
        public virtual void OnEndContainsFocus()
        {
        }

        /// <summary>
        ///     Focus that control
        /// </summary>
        /// <param name="c">Control to focus</param>
        /// <returns>True if control got a focus</returns>
        protected virtual bool Focus(Control c)
        {
            return _parent != null && _parent.Focus(c);
        }

        /// <summary>
        ///     Returns true if user is using that control so OnMouseMove and other events will be send to that control even if
        ///     shouldn't be (used by scroll bars, sliders etc.)
        /// </summary>
        public virtual bool HasMouseCapture => false;

        /// <summary>
        ///     When mouse goes up/down not over the control but it has user focus so remove that focus from it (used by scroll
        ///     bars, sliders etc.)
        /// </summary>
        public virtual void OnLostMouseCapture()
        {
        }

        #endregion

        #region Mouse

        /// <summary>
        ///     Check if mouse is over that item or its child items
        /// </summary>
        /// <returns>True if mouse is over</returns>
        public virtual bool IsMouseOver => _isMouseOver;

        /// <summary>
        ///     When mouse enters control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        public virtual void OnMouseEnter(Vector2 location)
        {
            // Set flag
            _isMouseOver = true;

            // Update tooltip
            //if (_tooltip)
            //    _tooltip->OnControlMouseEnter(this);
        }

        /// <summary>
        ///     When mouse moves over control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        public virtual void OnMouseMove(Vector2 location)
        {
        }

        /// <summary>
        ///     When mosue leaves control's area
        /// </summary>
        public virtual void OnMouseLeave()
        {
            // Clear flag
            _isMouseOver = false;
        }

        /// <summary>
        ///     When mouse wheel moves
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="delta">
        ///     Mosue wheeel move delta. A positive value indicates that the wheel was rotated forward, away from
        ///     the user; a negative value indicates that the wheel was rotated backward, toward the user
        /// </param>
        /// <returns>True if event has been handled</returns>
        public virtual bool OnMouseWheel(Vector2 location, short delta)
        {
            return false;
        }

        /// <summary>
        ///     When mouse goes down over control's area
        /// </summary>
        /// <param name="buttons">Mouse buttons state (flags)</param>
        /// <param name="location">Mouse location in Control Space</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnMouseDown(MouseButtons buttons, Vector2 location)
        {
            return _canFocus && Focus(this);
        }

        /// <summary>
        ///     When mouse goes up over control's area
        /// </summary>
        /// <param name="buttons">Mouse buttons state (flags)</param>
        /// <param name="location">Mouse location in Control Space</param>
        /// <returns>True if event has been handled, oherwise false</returns>
        public virtual bool OnMouseUp(MouseButtons buttons, Vector2 location)
        {
            return false;
        }

        /// <summary>
        ///     When mouse double clicks over control's area
        /// </summary>
        /// <param name="buttons">Mouse buttons state (flags)</param>
        /// <param name="location">Mouse location in Control Space</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnMouseDoubleClick(MouseButtons buttons, Vector2 location)
        {
            return false;
        }

        #endregion

        #region Keyboard

        /// <summary>
        ///     When key goes down
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnKeyDown(KeyCode key)
        {
            return false;
        }

        /// <summary>
        ///     When key goes up
        /// </summary>
        /// <param name="key">Key value</param>
        public virtual void OnKeyUp(KeyCode key)
        {
        }

        #endregion

        #region Drag&Drop

        // TODO: move drag and drop support from C++

        #endregion

        #region Tooltip

        // TODO: move tooltips support from C++

        #endregion

        #region Helper Functions

        /// <summary>
        ///     Checks if control contains given point
        /// </summary>
        /// <param name="location">Point location in Control Space to check</param>
        /// <returns>True if point is inside control's area</returns>
        public virtual bool ContainsPoint(ref Vector2 location)
        {
            return Bounds.Contains(ref location);
        }

        /// <summary>
        ///     Converts point in local control's space into parent control coordinates
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in parent control coordinates</returns>
        public virtual Vector2 PointToParent(Vector2 location)
        {
            return location + _bounds.Location;
        }

        /// <summary>
        ///     Converts point in parent control coordinates into local control's space
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in control's space</returns>
        public virtual Vector2 PointFromParent(Vector2 location)
        {
            return location - _bounds.Location;
        }

        /// <summary>
        ///     Converts point in local control's space into window coordinates
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in window coordinates</returns>
        public virtual Vector2 PointToWindow(Vector2 location)
        {
            if (HasParent)
            {
                return _parent.PointToWindow(location + Location);
            }
            return location;
        }

        /// <summary>
        ///     Converts point in the window coordinates into control's space
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in control's space</returns>
        public virtual Vector2 PointFromWindow(Vector2 location)
        {
            if (HasParent)
            {
                return _parent.PointFromWindow(location - Location);
            }
            return location;
        }

        /// <summary>
        ///     Converts point in screen coordinates into the local control's space
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in local control's space</returns>
        public virtual Vector2 ScreenToClient(Vector2 location)
        {
            if (HasParent)
            {
                return _parent.ScreenToClient(location - Location);
            }
            return location;
        }

        /// <summary>
        ///     Converts point in the local control's space into screen coordinates
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in screen coordinates</returns>
        public virtual Vector2 ClientToScreen(Vector2 location)
        {
            if (HasParent)
            {
                return _parent.ClientToScreen(location + Location);
            }
            return location;
        }

        #endregion

        #region Control Action

        /// <summary>
        ///     Sets location of control and calls event
        ///     <remarks>This method is called from engine when necessary</remarks>
        /// </summary>
        /// <param name="location">Location to set</param>
        protected virtual void SetLocationInternal(Vector2 location)
        {
            _bounds.Location = location;
            OnLocationChanged?.Invoke(this);
        }

        /// <summary>
        ///     Sets size of control and calls event
        ///     <remarks>This method is called form engine when necessary</remarks>
        /// </summary>
        /// <param name="size"></param>
        protected virtual void SetSizeInternal(Vector2 size)
        {
            _bounds.Size = size;
            OnSizeChanged?.Invoke(this);
        }


        /// <summary>
        ///     Action fred when parent control gets resized (also when control gets non-null parent)
        /// </summary>
        /// <param name="oldSize">Previous size of the parent control</param>
        public virtual void OnParentResized(ref Vector2 oldSize)
        {
            // TODO: move C++ anchor styles or design better wayt for that
        }

        /// <summary>
        ///     Method called when managed instance should be destoryed
        /// </summary>
        public virtual void OnDestroy()
        {
        }

        #endregion
    }
}