// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    ///     Base class for all GUI controls
    /// </summary>
    public partial class Control : IComparable
    {
        private ContainerControl _parent;
        private RootControl _root;
        private bool _isDisposing, _isFocused;

        // State

        // TODO: convert to flags

        private bool _isMouseOver, _isDragOver;
        private bool _isVisible = true;
        private bool _isEnabled = true;
        private bool _canFocus = true;

        // Dimensions

        private Rectangle _bounds;

        // Transform

        private Vector2 _scale = new Vector2(1.0f);
        private Vector2 _pivot = new Vector2(0.5f);
        private Vector2 _shear;
        private float _rotation;
        internal Matrix3x3 _cachedTransform;
        internal Matrix3x3 _cachedTransformInv;

        // Style

        private DockStyle _dockStyle;
        private AnchorStyle _anchorStyle;
        private Color _backgroundColor = Color.Transparent;

        // Tooltip

        private string _tooltipText;
        private Tooltip _tooltip;

        /// <summary>
        ///     Action is invoked, when location is changed
        /// </summary>
        public event Action<Control> LocationChanged;

        /// <summary>
        ///     Action is invoked, when size is changed
        /// </summary>
        public event Action<Control> SizeChanged;

        /// <summary>
        ///     Action is invoked, when parent is changed
        /// </summary>
        public event Action<Control> ParentChanged;

        /// <summary>
        ///     Action is invoked, when visibility is changed
        /// </summary>
        public event Action<Control> VisibleChanged;

        #region Public Properties

        /// <summary>
        ///     Parent control (the one above in the tree hierarchy)
        /// </summary>
        [HideInEditor, NoSerialize]
        public ContainerControl Parent
        {
            get => _parent;
            set
            {
                if (_parent == value)
                    return;

                Defocus();

                Vector2 oldParentSize;
                if (_parent != null)
                {
                    oldParentSize = _parent.Size;
                    _parent.RemoveChildInternal(this);
                }
                else
                {
                    oldParentSize = Vector2.Zero;
                }

                _parent = value;
                _parent?.AddChildInternal(this);

                CacheRootHandle();
                OnParentChangedInternal();

                // Check if parent size has been changed
                if (_parent != null && !_parent.IsLayoutLocked && !_parent.Size.Equals(ref oldParentSize))
                {
                    OnParentResized(ref oldParentSize);
                }
            }
        }

        /// <summary>
        ///     Checks if control has parent container control
        /// </summary>
        public bool HasParent => _parent != null;

        /// <summary>
        /// Gets or sets zero-based index of the control inside the parent container list.
        /// </summary>
        [HideInEditor, NoSerialize]
        public int IndexInParent
        {
            get => _parent?.GetChildIndex(this) ?? -1;
            set => _parent?.ChangeChildIndex(this, value);
        }

        /// <summary>
        ///     Gets or sets control background color (transparent color (alpha=0) means no background rendering)
        /// </summary>
        [ExpandGroups, EditorDisplay("Style"), EditorOrder(2000), Tooltip("The control background color. Use transparent color (alpha=0) to hide background.")]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        /// <summary>
        ///     Gets or sets the docking style of the control.
        /// If value set is other than <see cref="FlaxEngine.GUI.DockStyle.None"/> then <see cref="IsScrollable"/> will be disabled by auto.
        /// </summary>
        [EditorDisplay("Transform"), EditorOrder(1060), Tooltip("The docking style of the control. Defines how control will be docked into the parent container. Use None to disable it. Docked controls have disabled scrolling.")]
        public DockStyle DockStyle
        {
            get => _dockStyle;
            set
            {
                if (_dockStyle != value)
                {
                    _dockStyle = value;

                    // Disable scrolling for docked controls (by default but can be manually restored)
                    if (_dockStyle != DockStyle.None)
                        IsScrollable = false;

                    // Update parent's container
                    _parent?.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the anchor style of the control.
        /// </summary>
        [EditorDisplay("Transform"), EditorOrder(1070), Tooltip("The anchor style of the control. Defines how control location and size will be constrained when parent container is being resized.")]
        public AnchorStyle AnchorStyle
        {
            get => _anchorStyle;
            set
            {
                if (_anchorStyle != value)
                {
                    _anchorStyle = value;

                    // Update layout
                    if (_parent != null)
                    {
                        if (_anchorStyle == AnchorStyle.Center)
                        {
                            UpdateCenterAnchor();
                        }
                        else
                        {
                            _parent.PerformLayout();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is scrollable (scroll bars affect it).
        /// </summary>
        [HideInEditor, NoSerialize]
        public bool IsScrollable { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether the control can respond to user interaction
        /// </summary>
        [EditorOrder(520), Tooltip("If checked, control will receive input events of the user interaction.")]
        public bool Enabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;

                    // Check if control has been disabled
                    if (!_isEnabled)
                    {
                        Defocus();

                        // Clear flags
                        if (_isMouseOver)
                            OnMouseLeave();
                        if (_isDragOver)
                            OnDragLeave();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is enabled in the hierarchy (it's enabled and all it's parents are enabled as well).
        /// </summary>
        public bool EnabledInHierarchy
        {
            get
            {
                if (!_isEnabled)
                    return false;
                if (_parent != null)
                    return _parent.EnabledInHierarchy;
                return true;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the control is visible
        /// </summary>
        [EditorOrder(510), Tooltip("If checked, control will be visible.")]
        public bool Visible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;

                    // Check on control hide event
                    if (!_isVisible)
                    {
                        Defocus();

                        // Clear flags
                        if (_isMouseOver)
                            OnMouseLeave();
                        if (_isDragOver)
                            OnDragLeave();
                    }

                    OnVisibleChanged();
                    _parent?.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is visible in the hierarchy (it's visible and all it's parents are visible as well).
        /// </summary>
        public bool VisibleInHierarchy
        {
            get
            {
                if (!_isVisible)
                    return false;
                if (_parent != null)
                    return _parent.VisibleInHierarchy;
                return true;
            }
        }

        /// <summary>
        ///     Returns true if control is during disposing state (on destroy)
        /// </summary>
        public bool IsDisposing => _isDisposing;

        /// <summary>
        /// Gets the GUI tree root control which contains that control (or null if not linked to any)
        /// </summary>
        public virtual RootControl Root => _root;

        /// <summary>
        /// Gets the GUI window root control which contains that control (or null if not linked to any).
        /// </summary>
        public virtual WindowRootControl RootWindow => _parent?.RootWindow;

        /// <summary>
        /// Gets screen position of the control (upper left corner).
        /// </summary>
        /// <returns>Screen position of the control.</returns>
        public Vector2 ScreenPos
        {
            get
            {
                var parentWin = Root;
                if (parentWin == null)
                    throw new InvalidOperationException("Missing parent window.");
                var clientPos = PointToWindow(Vector2.Zero);
                return parentWin.ClientToScreen(clientPos);
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the cursor (per window). Control should restore cursor to the default value eg. when mouse leaves it's area.
        /// </summary>
        [HideInEditor, NoSerialize]
        public virtual CursorType Cursor
        {
            get => _parent?.Cursor ?? CursorType.Default;
            set
            {
                if (_parent != null)
                    _parent.Cursor = value;
            }
        }

        /// <summary>
        /// The custom tag object value linked to the control.
        /// </summary>
        [HideInEditor, NoSerialize]
        public object Tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        protected Control()
        {
            _bounds = new Rectangle(0, 0, 64, 64);

            UpdateTransform();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        protected Control(float x, float y, float width, float height)
        {
            _bounds = new Rectangle(x, y, width, height);

            UpdateTransform();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="location">Upper left corner location.</param>
        /// <param name="size">Bounds size.</param>
        protected Control(Vector2 location, Vector2 size)
        {
            _bounds = new Rectangle(location, size);

            UpdateTransform();
        }

        /// <summary>
        ///     Init
        /// </summary>
        /// <param name="bounds">Window bounds</param>
        protected Control(Rectangle bounds)
        {
            _bounds = bounds;

            UpdateTransform();
        }

        /// <summary>
        ///     Delete control (will unlink from the parent and start to dispose)
        /// </summary>
        public virtual void Dispose()
        {
            if (_isDisposing)
                return;

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
            if (IsMouseOver && ShowTooltip)
            {
                Tooltip.OnMouseOverControl(this, deltaTime);
            }
        }

        /// <summary>
        ///     Draw control
        /// </summary>
        public virtual void Draw()
        {
            // Paint Background
            if (_backgroundColor.A > 0.0f)
            {
                Render2D.FillRectangle(new Rectangle(Vector2.Zero, Size), _backgroundColor);
            }
        }

        /// <summary>
        ///     Update control layout
        /// </summary>
        /// <param name="force">True if perform layout by force even if cached state wants to skip it due to optimization.</param>
        public virtual void PerformLayout(bool force = false)
        {
        }

        #region Focus

        /// <summary>
        ///     Gets a value indicating whether the control can receive focus
        /// </summary>
        [HideInEditor]
        public bool CanFocus
        {
            get => _canFocus;
            set => _canFocus = value;
        }

        /// <summary>
        ///     Gets a value indicating whether the control, currently has the input focus
        /// </summary>
        public virtual bool ContainsFocus => _isFocused;

        /// <summary>
        ///     Gets a value indicating whether the control has input focus
        /// </summary>
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
        /// Starts the mouse tracking. Used by the scrollbars, splitters, etc.
        /// </summary>
        /// <param name="useMouseScreenOffset">If set to <c>true</c> will use mouse screen offset.</param>
        public void StartMouseCapture(bool useMouseScreenOffset = false)
        {
            var parent = Root;
            parent.StartTrackingMouse(this, useMouseScreenOffset);
        }

        /// <summary>
        /// Ends the mouse tracking.
        /// </summary>
        public void EndMouseCapture()
        {
            var parent = Root;
            parent.EndTrackingMouse();
        }

        /// <summary>
        ///     When mouse goes up/down not over the control but it has user focus so remove that focus from it (used by scroll
        ///     bars, sliders etc.)
        /// </summary>
        public virtual void OnEndMouseCapture()
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
            if (ShowTooltip)
                Tooltip.OnMouseEnterControl(this);
        }

        /// <summary>
        ///     When mouse moves over control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        public virtual void OnMouseMove(Vector2 location)
        {
        }

        /// <summary>
        ///     When mouse leaves control's area
        /// </summary>
        public virtual void OnMouseLeave()
        {
            // Clear flag
            _isMouseOver = false;

            // Update tooltip
            if (ShowTooltip)
                Tooltip.OnMouseLeaveControl(this);
        }

        /// <summary>
        ///     When mouse wheel moves
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="delta">
        ///   Mouse wheel move delta. A positive value indicates that the wheel was rotated forward, away from
        ///   the user; a negative value indicates that the wheel was rotated backward, toward the user. Normalized to [-1;1] range.
        /// </param>
        /// <returns>True if event has been handled</returns>
        public virtual bool OnMouseWheel(Vector2 location, float delta)
        {
            return false;
        }

        /// <summary>
        ///     When mouse goes down over control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="buttons">Mouse buttons state (flags)</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            return _canFocus && Focus(this);
        }

        /// <summary>
        ///     When mouse goes up over control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="buttons">Mouse buttons state (flags)</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            return false;
        }

        /// <summary>
        ///     When mouse double clicks over control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="buttons">Mouse buttons state (flags)</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            return false;
        }

        #endregion

        #region Keyboard

        /// <summary>
        ///     On input character
        /// </summary>
        /// <param name="c">Input character</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnCharInput(char c)
        {
            return false;
        }

        /// <summary>
        ///     When key goes down
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>True if event has been handled, otherwise false</returns>
        public virtual bool OnKeyDown(Keys key)
        {
            return false;
        }

        /// <summary>
        ///     When key goes up
        /// </summary>
        /// <param name="key">Key value</param>
        public virtual void OnKeyUp(Keys key)
        {
        }

        #endregion

        #region Drag&Drop

        /// <summary>
        ///     Check if mouse dragging is over that item or its child items.
        /// </summary>
        public virtual bool IsDragOver => _isDragOver;

        /// <summary>
        ///     When mouse dragging enters control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="data">The data. See <see cref="DragDataText"/> and <see cref="DragDataFiles"/>.</param>
        /// <returns>The drag event result effect.</returns>
        public virtual DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            // Set flag
            _isDragOver = true;

            return DragDropEffect.None;
        }

        /// <summary>
        ///     When mouse dragging moves over control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="data">The data. See <see cref="DragDataText"/> and <see cref="DragDataFiles"/>.</param>
        /// <returns>The drag event result effect.</returns>
        public virtual DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            return DragDropEffect.None;
        }

        /// <summary>
        ///     When mouse dragging drops on control's area
        /// </summary>
        /// <param name="location">Mouse location in Control Space</param>
        /// <param name="data">The data. See <see cref="DragDataText"/> and <see cref="DragDataFiles"/>.</param>
        /// <returns>The drag event result effect.</returns>
        public virtual DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            // Clear flag
            _isDragOver = false;

            return DragDropEffect.None;
        }

        /// <summary>
        ///     When mouse dragging leaves control's area
        /// </summary>
        public virtual void OnDragLeave()
        {
            // Clear flag
            _isDragOver = false;
        }

        /// <summary>
        /// Starts the drag and drop operation.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void DoDragDrop(DragData data)
        {
            Root.DoDragDrop(data);
        }

        #endregion

        #region Tooltip

        /// <summary>
        /// Gets or sets the tooltip text.
        /// </summary>
        [HideInEditor]
        public string TooltipText
        {
            get => _tooltipText;
            set => _tooltipText = value;
        }

        /// <summary>
        /// Gets or sets the custom tooltip control linked. Use null to show default shared tooltip from the current <see cref="Style"/>.
        /// </summary>
        [HideInEditor]
        public Tooltip CustomTooltip
        {
            get => _tooltip;
            set => _tooltip = value;
        }

        /// <summary>
        /// Gets the tooltip used by this control (custom or shared one).
        /// </summary>
        public Tooltip Tooltip => _tooltip ?? Style.Current.SharedTooltip;

        /// <summary>
        /// Gets a value indicating whether show control tooltip (control is in a proper state, tooltip text is valid, etc.). Can be used to implement custom conditions for showing tooltips (eg. based on current mouse location within the control bounds).
        /// </summary>
        /// <remarks>
        /// Tooltip can be only visible if mouse is over the control area (see <see cref="IsMouseOver"/>).
        /// </remarks>
        protected virtual bool ShowTooltip => _tooltipText != null;

        /// <summary>
        /// Links the tooltip.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="customTooltip">The custom tooltip.</param>
        /// <returns>This control pointer. Useful for creating controls in code.</returns>
        public Control LinkTooltip(string text, Tooltip customTooltip = null)
        {
            _tooltipText = text;
            _tooltip = customTooltip;
            return this;
        }

        /// <summary>
        /// Unlinks the tooltip.
        /// </summary>
        public void UnlinkTooltip()
        {
            _tooltipText = null;
            _tooltip = null;
        }

        /// <summary>
        /// Called when tooltip wants to be shown. Allows modifying its appearance.
        /// </summary>
        /// <param name="text">The tooltip text to show.</param>
        /// <param name="location">The popup start location (in this control local space).</param>
        /// <param name="area">The allowed area of mouse movement to show tooltip (in this control local space).</param>
        /// <returns>True if can show tooltip, otherwise false to skip.</returns>
        public virtual bool OnShowTooltip(out string text, out Vector2 location, out Rectangle area)
        {
            text = _tooltipText;
            location = Size * new Vector2(0.5f, 1.0f);
            area = new Rectangle(Vector2.Zero, Size);
            return true;
        }

        /// <summary>
        /// Called when tooltip is visible and tests if the given mouse location (in control space) is valid (is over the content).
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>True if tooltip can be still visible, otherwise false.</returns>
        public virtual bool OnTestTooltipOverControl(ref Vector2 location)
        {
            return ContainsPoint(ref location);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Checks if given location point in Parent Space intersects with the control content and calculates local position.
        /// </summary>
        /// <param name="locationParent">The location in Parent Space.</param>
        /// <param name="location">The location of intersection in Control Space.</param>
        /// <returns></returns>
        public virtual bool IntersectsContent(ref Vector2 locationParent, out Vector2 location)
        {
            location = PointFromParent(ref locationParent);
            return ContainsPoint(ref location);
        }

        /// <summary>
        ///     Checks if control contains given point in local Control Space.
        /// </summary>
        /// <param name="location">Point location in Control Space to check</param>
        /// <returns>True if point is inside control's area</returns>
        public virtual bool ContainsPoint(ref Vector2 location)
        {
            return location.X >= 0 &&
                   location.Y >= 0 &&
                   location.X <= _bounds.Size.X &&
                   location.Y <= _bounds.Size.Y;
        }

        /// <summary>
        ///     Converts point in local control's space into one of the parent control coordinates
        /// </summary>
        /// <param name="parent">This control parent of any other parent.</param>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in parent control coordinates</returns>
        public Vector2 PointToParent(ContainerControl parent, Vector2 location)
        {
            if (parent == null)
                throw new ArgumentNullException();

            Control c = this;
            while (c != null)
            {
                location = c.PointToParent(ref location);

                c = c.Parent;
                if (c == parent)
                    return location;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Converts point in local control's space into parent control coordinates.
        /// </summary>
        /// <param name="location">The input location of the point to convert.</param>
        /// <returns>The converted point location in parent control coordinates.</returns>
        public virtual Vector2 PointToParent(ref Vector2 location)
        {
            Vector2 result;
            Matrix3x3.Transform2D(ref location, ref _cachedTransform, out result);
            return result;
        }

        /// <summary>
        /// Converts point in parent control coordinates into local control's space.
        /// </summary>
        /// <param name="locationParent">The input location of the point to convert.</param>
        /// <returns>The converted point location in control's space.</returns>
        public virtual Vector2 PointFromParent(ref Vector2 locationParent)
        {
            Vector2 result;
            Matrix3x3.Transform2D(ref locationParent, ref _cachedTransformInv, out result);
            return result;
        }

        /// <summary>
        ///     Converts point in local control's space into window coordinates
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in window coordinates</returns>
        public Vector2 PointToWindow(Vector2 location)
        {
            location = PointToParent(ref location);
            if (HasParent)
            {
                location = _parent.PointToWindow(location);
            }
            return location;
        }

        /// <summary>
        ///     Converts point in the window coordinates into control's space
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in control's space</returns>
        public Vector2 PointFromWindow(Vector2 location)
        {
            if (HasParent)
                location = _parent.PointFromWindow(location);
            return PointFromParent(ref location);
        }

        /// <summary>
        ///     Converts point in the local control's space into screen coordinates
        /// </summary>
        /// <param name="location">Input location of the point to convert</param>
        /// <returns>Converted point location in screen coordinates</returns>
        public virtual Vector2 ClientToScreen(Vector2 location)
        {
            location = PointToParent(ref location);
            if (HasParent)
            {
                location = _parent.ClientToScreen(location);
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
            location = PointFromParent(ref location);
            if (HasParent)
            {
                location = _parent.ScreenToClient(location);
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
        protected virtual void SetLocationInternal(ref Vector2 location)
        {
            _bounds.Location = location;

            UpdateTransform();
            LocationChanged?.Invoke(this);
        }

        /// <summary>
        ///     Sets size of control and calls event
        ///     <remarks>This method is called form engine when necessary</remarks>
        /// </summary>
        /// <param name="size"></param>
        protected virtual void SetSizeInternal(ref Vector2 size)
        {
            _bounds.Size = size;

            UpdateTransform();
            SizeChanged?.Invoke(this);
            _parent?.OnChildResized(this);

            // Auto-center
            if (_anchorStyle == AnchorStyle.Center)
            {
                UpdateCenterAnchor();
            }
        }

        /// <summary>
        /// Sets the scale and updates the transform.
        /// </summary>
        /// <param name="scale">The scale.</param>
        protected virtual void SetScaleInternal(ref Vector2 scale)
        {
            _scale = scale;

            UpdateTransform();
            _parent?.OnChildResized(this);
        }

        /// <summary>
        /// Sets the pivot and updates the transform.
        /// </summary>
        /// <param name="pivot">The pivot.</param>
        protected virtual void SetPivotInternal(ref Vector2 pivot)
        {
            _pivot = pivot;

            UpdateTransform();
            _parent?.OnChildResized(this);
        }

        /// <summary>
        /// Sets the shear and updates the transform.
        /// </summary>
        /// <param name="shear">The shear.</param>
        protected virtual void SetShearInternal(ref Vector2 shear)
        {
            _shear = shear;

            UpdateTransform();
            _parent?.OnChildResized(this);
        }

        /// <summary>
        /// Sets the rotation angle and updates the transform.
        /// </summary>
        /// <param name="rotation">The rotation (in degrees).</param>
        protected virtual void SetRotationInternal(float rotation)
        {
            _rotation = rotation;

            UpdateTransform();
            _parent?.OnChildResized(this);
        }

        /// <summary>
        /// Called when visible state gets changed.
        /// </summary>
        protected virtual void OnVisibleChanged()
        {
            VisibleChanged?.Invoke(this);
        }

        /// <summary>
        ///     Action fred when parent control gets changed.
        /// </summary>
        protected virtual void OnParentChangedInternal()
        {
            ParentChanged?.Invoke(this);
        }

        /// <summary>
        /// Caches the root control handle.
        /// </summary>
        internal virtual void CacheRootHandle()
        {
            _root = _parent?.Root;
        }

        private void UpdateCenterAnchor()
        {
            Location = (_parent.Size - Size) * 0.5f;
        }

        /// <summary>
        ///     Action fred when parent control gets resized (also when control gets non-null parent)
        /// </summary>
        /// <param name="oldSize">Previous size of the parent control</param>
        public virtual void OnParentResized(ref Vector2 oldSize)
        {
            if (_anchorStyle == AnchorStyle.UpperLeft || oldSize.LengthSquared < Mathf.Epsilon)
                return;

            var bounds = Bounds;

            switch (_anchorStyle)
            {
            case AnchorStyle.UpperCenter:
            {
                bounds.X = (_parent.Width - bounds.Width) * 0.5f;
                break;
            }
            case AnchorStyle.UpperRight:
            {
                bounds.X = _parent.Width - (oldSize.X - bounds.Left);
                break;
            }
            case AnchorStyle.Upper:
            {
                bounds.Width = _parent.Width - bounds.X - (oldSize.X - bounds.Right);
                break;
            }

            case AnchorStyle.CenterLeft:
            {
                bounds.Y = (_parent.Height - bounds.Height) * 0.5f;
                break;
            }
            case AnchorStyle.Center:
            {
                bounds.Location = (_parent.Size - bounds.Size) * 0.5f;
                break;
            }
            case AnchorStyle.CenterRight:
            {
                bounds.X = _parent.Width - (oldSize.X - bounds.Left);
                bounds.Y = (_parent.Height - bounds.Height) * 0.5f;
                break;
            }

            case AnchorStyle.BottomLeft:
            {
                bounds.Y = _parent.Height - (oldSize.Y - bounds.Y);
                break;
            }
            case AnchorStyle.BottomCenter:
            {
                bounds.X = (_parent.Width - bounds.Width) * 0.5f;
                bounds.Y = _parent.Height - (oldSize.Y - bounds.Y);
                break;
            }
            case AnchorStyle.BottomRight:
            {
                bounds.X = _parent.Width - (oldSize.X - bounds.Left);
                bounds.Y = _parent.Height - (oldSize.Y - bounds.Y);
                break;
            }
            case AnchorStyle.Bottom:
            {
                bounds.Width = _parent.Width - bounds.X - (oldSize.X - bounds.Right);
                bounds.Y = _parent.Height - (oldSize.Y - bounds.Y);
                break;
            }

            case AnchorStyle.Left:
            {
                bounds.Height = _parent.Height - bounds.Y - (oldSize.Y - bounds.Bottom);
                break;
            }
            case AnchorStyle.Right:
            {
                bounds.Height = _parent.Height - bounds.Y - (oldSize.Y - bounds.Bottom);
                bounds.X = _parent.Width - (oldSize.X - bounds.Left);
                break;
            }

            default: throw new ArgumentOutOfRangeException();
            }

            Bounds = bounds;
        }

        /// <summary>
        ///     Method called when managed instance should be destroyed
        /// </summary>
        public virtual void OnDestroy()
        {
            // Set disposing flag
            _isDisposing = true;

            Defocus();

            UnlinkTooltip();
            Tag = null;
        }

        #endregion

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj is Control c)
                return Compare(c);
            return 0;
        }

        /// <summary>
        /// Compares this control with the other control.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>Comparision result.</returns>
        public virtual int Compare(Control other)
        {
            return (int)(Y - other.Y);
        }
    }
}
