////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Tree node control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    public class TreeNode : ContainerControl
    {
        /// <summary>
        /// The default node header height.
        /// </summary>
        public const float DefaultHeaderHeight = 16.0f;

        public const float DefaultDragInsertPositionMargin = 2.0f;
        public const float DefaultNodeOffsetY = 1;

        private Tree _tree;
        private int _visibleChildNodesCount;

        protected bool _opened, _canChangeOrder;
        protected float _animationProgress, _cachedHeight;
        protected bool _mouseOverArrow, _mouseOverHeader;
        protected float _xOffset, _textWidth;
        protected Rectangle _headerRect;
        protected Sprite _iconCollaped, _iconOpened;
        protected string _text;
        protected bool _textChanged;
        protected bool _isMouseDown;
        protected float _mouseDownTime;
        protected Vector2 _mouseDownPos;
        protected Color _cachedTextColor;

        protected DragItemPositioning _dragOverMode;
        protected bool _isDragOverHeader;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _textChanged = true;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get => _opened;
            set
            {
                if (value)
                    Expand();
                else
                    Collapse();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node is collapsed.
        /// </summary>
        public bool IsCollapsed
        {
            get => !_opened;
            set
            {
                if (value)
                    Collapse();
                else
                    Expand();
            }
        }

        /// <summary>
        /// Gets the parent tree control.
        /// </summary>
        public Tree ParentTree
        {
            get
            {
                if (_tree == null)
                {
                    if (Parent is TreeNode upNode)
                        _tree = upNode.ParentTree;
                    else if (Parent is Tree tree)
                        _tree = tree;
                }

                return _tree;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this node is root.
        /// </summary>
        public bool IsRoot => !(Parent is TreeNode);

        /// <summary>
        /// Gets the minimum width of the node sub-tree.
        /// </summary>
        public virtual float MinimumWidth
        {
            get
            {
                UpdateTextWidth();

                float minWidth = _xOffset + _textWidth + 6 + 16;
                if (_iconCollaped.IsValid)
                    minWidth += 16;

                if (_opened || _animationProgress < 1.0f)
                {
                    for (int i = 0; i < _children.Count; i++)
                    {
                        if (_children[i] is TreeNode node)
                        {
                            minWidth = Mathf.Max(minWidth, node.MinimumWidth);
                        }
                    }
                }

                return minWidth;
            }
        }
        
        /// <summary>
        /// Gets the arrow rectangle.
        /// </summary>
        protected Rectangle ArrowRect => new Rectangle(_xOffset + 2, 2, 12, 12);

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class.
        /// </summary>
        /// <param name="canChangeOrder">Enable/disable changing node order in parent tree node.</param>
        public TreeNode(bool canChangeOrder)
            : this(canChangeOrder, Sprite.Invalid, Sprite.Invalid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class.
        /// </summary>
        /// <param name="canChangeOrder">Enable/disable changing node order in parent tree node.</param>
        /// <param name="iconCollapsed">The icon for node collapsed.</param>
        /// <param name="iconOpened">The icon for node opened.</param>
        public TreeNode(bool canChangeOrder, Sprite iconCollapsed, Sprite iconOpened)
            : base(0, 0, 64, 16)
        {
            _canChangeOrder = canChangeOrder;
            _animationProgress = 1.0f;
            _cachedHeight = DefaultHeaderHeight;
            _iconCollaped = iconCollapsed;
            _iconOpened = iconOpened;
            _mouseDownTime = -1;

            _performChildrenLayoutFirst = true;
        }
        
        /// <summary>
        /// Expand node.
        /// </summary>
        public void Expand()
        {
            // Parents first
            ExpandAllParents();

            // Chnage state
            bool prevState = _opened;
            _opened = true;
            if (prevState != _opened)
                _animationProgress = 1.0f - _animationProgress;

            // Check if drag is over
            if (IsDragOver)
            {
				// Speed up an animation
				_animationProgress = 1.0f;
            }

            // Update
            PerformLayout();
        }

        /// <summary>
        /// Collapse node.
        /// </summary>
        public void Collapse()
        {
            // Chnage state
            bool prevState = _opened;
            _opened = false;
            if (prevState != _opened)
                _animationProgress = 1.0f - _animationProgress;

            // Check if drag is over
            if (IsDragOver)
            {
                // Speed up an animation
                _animationProgress = 1.0f;
            }

            // Update
            PerformLayout();
        }

        /// <summary>
        /// Expand node and all the children.
        /// </summary>
        public void ExpandAll()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            Expand();

            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is TreeNode node)
                {
                    node.ExpandAll();
                }
            }

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Collapse node and all the children.
        /// </summary>
        public void CollapseAll()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            Collapse();

            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is TreeNode node)
                {
                    node.CollapseAll();
                }
            }

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        /// Ensure that all node paents are expanded.
        /// </summary>
        public void ExpandAllParents()
        {
            (Parent as TreeNode)?.Expand();
        }

        /// <summary>
        /// Ends open/close animation by force.
        /// </summary>
        public void EndAnimation()
        {
            if (_animationProgress < 1.0f)
            {
                _animationProgress = 1.0f;
                PerformLayout();
            }
        }

        /// <summary>
        /// Select node in the tree.
        /// </summary>
        public void Select()
        {
            ParentTree.Select(this);
        }

        /// <summary>
        /// Called when drag and drop enters the node header area.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Drag action response.</returns>
        protected virtual DragDropEffect OnDragEnterHeader(DragData data)
        {
            return DragDropEffect.None;
        }

        /// <summary>
        /// Called when drag and drop moves over the node header area.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Drag action response.</returns>
        protected virtual DragDropEffect OnDragMoveHeader(DragData data)
        {
            return DragDropEffect.None;
        }

        /// <summary>
        /// Called when drag and drop performs over the node header area.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Drag action response.</returns>
        protected virtual DragDropEffect OnDragDropHeader(DragData data)
        {
            return DragDropEffect.None;
        }

        /// <summary>
        /// Called when drag and drop leaves the node header area.
        /// </summary>
        protected virtual void OnDragLeaveHeader()
        {
        }

        /// <summary>
        /// Begins the drag drop operation.
        /// </summary>
        protected virtual void DoDragDrop()
        {
        }

        /// <summary>
        /// Called when mouse is pressing node header for a long time.
        /// </summary>
        protected virtual void OnLongPress()
        {
        }

        /// <summary>
        /// Tests the header hit.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        protected virtual bool testHeaderHit(ref Vector2 location)
        {
            return _headerRect.Contains(ref location);
        }

        private void updateDrawPositioning(ref Vector2 location)
        {
            if (new Rectangle(_headerRect.X, _headerRect.Y - DefaultDragInsertPositionMargin - DefaultNodeOffsetY, _headerRect.Width, DefaultDragInsertPositionMargin * 2.0f).Contains(location))
                _dragOverMode = DragItemPositioning.Above;
            else if (IsCollapsed && new Rectangle(_headerRect.X, _headerRect.Bottom - DefaultDragInsertPositionMargin, _headerRect.Width, DefaultDragInsertPositionMargin * 2.0f).Contains(location))
                _dragOverMode = DragItemPositioning.Below;
            else
                _dragOverMode = DragItemPositioning.At;
        }

        /// <summary>
        /// Caches the color of the text for this node. Called during update before children nodes but after parent node so it can reuse parent tree node data.
        /// </summary>
        /// <returns>Text color.</returns>
        protected virtual Color CacheTextColor()
        {
            return Enabled ? Style.Current.Foreground : Style.Current.ForegroundDisabled;
        }

        /// <summary>
        /// Updates the cached width of the text.
        /// </summary>
        protected void UpdateTextWidth()
        {
            if (_textChanged)
            {
                var style = Style.Current;
                if (style.FontSmall)
                {
                    _textWidth = style.FontSmall.MeasureText(_text).X;
                    _textChanged = false;
                }
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Cache text color
            _cachedTextColor = CacheTextColor();

            // Drop/down animation
            if (_animationProgress < 1.0f)
            {
                bool isDeltaSlow = deltaTime > (1 / 20.0f);

                // Update progress
                if (isDeltaSlow)
                {
                    _animationProgress = 1.0f;
                }
                else
                {
                    const float openCloseAniamtionTime = 0.1f;
                    _animationProgress += deltaTime / openCloseAniamtionTime;
                    if (_animationProgress > 1.0f)
                        _animationProgress = 1.0f;
                }

                // Arrange controls
                PerformLayout();
            }

            // Check for long press
            const float longPressTimeSeconds = 0.6f;
            if (_isMouseDown && Time.UnscaledGameTime - _mouseDownTime > longPressTimeSeconds)
            {
                OnLongPress();
            }

            // Don't update collapsed children
            if (_opened)
            {
                base.Update(deltaTime);
            }
            else
            {
                // Manually update tooltip
                if (TooltipText != null && IsMouseOver)
                    Tooltip.OnMouseOverControl(this, deltaTime);
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Check if it's a root
            if (IsRoot)
            {
                // Base
                if (_opened)
                    base.Draw();
            }
            else
            {
                // Cache data
                var style = Style.Current;
                var tree = ParentTree;
                bool isSelected = tree.Selection.Contains(this);
                bool isFocused = tree.ContainsFocus;
                var textRect = new Rectangle(_xOffset + 4 + DefaultHeaderHeight, 0, 10000, DefaultHeaderHeight);

                // Draw background
                if (isSelected || _mouseOverHeader)
                    Render2D.FillRectangle(_headerRect, (isSelected && isFocused) ? style.BackgroundSelected : (_mouseOverHeader ? style.BackgroundHighlighted : style.LightBackground));

                // Draw arrow
                if (_visibleChildNodesCount > 0)
                    Render2D.DrawSprite(_opened ? style.ArrowDown : style.ArrowRight, ArrowRect, _mouseOverHeader ? Color.White : new Color(0.8f, 0.8f, 0.8f, 0.8f));

                // Draw icon
                if (_iconCollaped.IsValid)
                {
                    Render2D.DrawSprite(_opened ? _iconOpened : _iconCollaped, new Rectangle(textRect.Left - 2, 0, 16, 16));
                    textRect.Offset(16, 0);
                }

                // Draw text
                Render2D.DrawText(style.FontSmall, _text, textRect, _cachedTextColor, TextAlignment.Near, TextAlignment.Center);

                // Draw drag and drop effect
                if (IsDragOver)
                {
                    Color dragOverColor = style.BackgroundSelected * 0.4f;
                    Rectangle rect;
                    switch (_dragOverMode)
                    {
                        case DragItemPositioning.At:
                            rect = textRect;
                            break;
                        case DragItemPositioning.Above:
                            rect = new Rectangle(textRect.X, textRect.Y - DefaultDragInsertPositionMargin - DefaultNodeOffsetY, textRect.Width, DefaultDragInsertPositionMargin * 2.0f);
                            break;
                        case DragItemPositioning.Below:
                            rect = new Rectangle(textRect.X, textRect.Bottom - DefaultDragInsertPositionMargin, textRect.Width, DefaultDragInsertPositionMargin * 2.0f);
                            break;
                        default:
                            rect = Rectangle.Empty;
                            break;
                    }
                    Render2D.FillRectangle(rect, dragOverColor, true);
                }

                // Base
                Render2D.PushClip(new Rectangle(0, DefaultHeaderHeight, Width, Height - DefaultHeaderHeight));
                if (_opened)
                    base.Draw();
                Render2D.PopClip();
            }
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            // Check if mosue hits bar and node isn't a root
            if (_mouseOverHeader && !IsRoot)
            {
                // Check if left buton goes down
                if (buttons == MouseButton.Left)
                {
                    _isMouseDown = true;
                    _mouseDownPos = location;
                    _mouseDownTime = Time.UnscaledGameTime;
                }

                // Handled
                Focus();
                return true;
            }

            // Base
            if (_opened)
                return base.OnMouseDown(location, buttons);

            // Handled
            Focus();
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Check if mouse hits bar
            if (buttons == MouseButton.Right && testHeaderHit(ref location))
            {
                ParentTree.OnRightClickInternal(this, ref location);
            }

            // Clear flag for left button
            if (buttons == MouseButton.Left)
            {
                // Clear flag
                _isMouseDown = false;
                _mouseDownTime = -1;
            }

            // Check if mosue hits bar and node isn't a root
            if (_mouseOverHeader && !IsRoot)
            {
                // Prevent from selecting node when user is just clicking at an arrow
                if (!_mouseOverArrow)
                {
                    // Check if user is pressing control key
                    var tree = ParentTree;
                    var window = tree.ParentWindow;
                    if (window.GetKey(Keys.Shift))
                    {
                        // Select range
                        tree.SelectRange(this);
                    }
                    else if (window.GetKey(Keys.Control))
                    {
                        // Add/Remove
                        tree.AddOrRemoveSelection(this);
                    }
                    else
                    {
                        // Select
                        tree.Select(this);
                    }
                }

                // Check if mosue hits arrow
                if (_children.Count > 0 && _mouseOverArrow)
                {
                    // Toggle open state
                    if (_opened)
                        Collapse();
                    else
                        Expand();
                }

                // Handled
                Focus();
                return true;
            }

            // Base
            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            // Check if mosue hits bar
            if (!IsRoot && testHeaderHit(ref location))
            {
                // Toggle open state
                if (_opened)
                    Collapse();
                else
                    Expand();

                // Handled
                return true;
            }

            // Check if animation has been finished
            if (_animationProgress >= 1.0f)
            {
                // Base
                return base.OnMouseDoubleClick(location, buttons);
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Cache flags
            _mouseOverArrow = _children.Count > 0 && ArrowRect.Contains(location);
            _mouseOverHeader = new Rectangle(0, 0, Width, DefaultHeaderHeight - 1).Contains(location);

            // Check if start drag and drop
            if (_isMouseDown && Vector2.Distance(_mouseDownPos, location) > 10.0f)
            {
                // Clear flag
                _isMouseDown = false;
                _mouseDownTime = -1;

                // Start
                DoDragDrop();
            }

            // Check if animation has been finished
            if (_animationProgress >= 1.0f)
            {
                // Base
                if (_opened)
                    base.OnMouseMove(location);
            }
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Clear flags
            _mouseOverArrow = false;
            _mouseOverHeader = false;

            // Check if start drag and drop
            if (_isMouseDown)
            {
                // Clear flag
                _isMouseDown = false;
                _mouseDownTime = -1;

                // Start
                DoDragDrop();
            }

            // Base
            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            // Check if is focused and has any children
            if (IsFocused && _children.Count > 0)
            {
                // Collapse
                if (key == Keys.ArrowLeft)
                {
                    Collapse();
                    return true;
                }

                // Expand
                if (key == Keys.ArrowRight)
                {
                    Expand();
                    return true;
                }
            }

            // Base
            if (_opened)
                return base.OnKeyDown(key);
            return false;
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            PerformLayout();
            ParentTree.UpdateSize();
            base.OnChildResized(control);
        }

        /// <inheritdoc />
        public override void OnParentResized(ref Vector2 oldSize)
        {
            base.OnParentResized(ref oldSize);
            Width = Parent.Width;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            var result = base.OnDragEnter(ref location, data);

            // Check if no children handled that event
            _dragOverMode = DragItemPositioning.None;
            if (result == DragDropEffect.None)
            {
                updateDrawPositioning(ref location);

                // Check if mosue is over header
                _isDragOverHeader = testHeaderHit(ref location);
                if (_isDragOverHeader)
                {
                    // Check if mouse is over arrow
                    if (_children.Count > 0 && ArrowRect.Contains(location))
                    {
                        // Expand node
                        Expand();
                    }

                    result = OnDragEnterHeader(data);
                }

                if (result == DragDropEffect.None)
                    _dragOverMode = DragItemPositioning.None;
            }

            return result;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            var result = base.OnDragMove(ref location, data);

            // Check if no children handled that event
            _dragOverMode = DragItemPositioning.None;
            if (result == DragDropEffect.None)
            {
                updateDrawPositioning(ref location);

                // Check if mosue is over header
                bool isDragOverHeader = testHeaderHit(ref location);
                if (isDragOverHeader)
                {
                    // Check if mouse is over arrow
                    if (_children.Count > 0 && ArrowRect.Contains(location))
                    {
                        // Expand node
                        Expand();
					}

                    if (!_isDragOverHeader)
                        result = OnDragEnterHeader(data);
                    else
                        result = OnDragMoveHeader(data);
                }
                _isDragOverHeader = isDragOverHeader;

                if (result == DragDropEffect.None)
                    _dragOverMode = DragItemPositioning.None;
            }

            return result;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            var result = base.OnDragDrop(ref location, data);

            // Check if no children handled that event
            if (result == DragDropEffect.None)
            {
                updateDrawPositioning(ref location);

                // Check if mosue is over header
                if (testHeaderHit(ref location))
                {
                    result = OnDragDropHeader(data);
                }
            }

            // Clear cache
            _isDragOverHeader = false;
            _dragOverMode = DragItemPositioning.None;

            return result;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            // Clear cache
            if (_isDragOverHeader)
            {
                _isDragOverHeader = false;
                OnDragLeaveHeader();
            }
            _dragOverMode = DragItemPositioning.None;

            base.OnDragLeave();
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(Vector2 size)
        {
            base.SetSizeInternal(size);

            // Cache data
            _headerRect = new Rectangle(0, 0, Width, DefaultHeaderHeight);
        }

        private void CacheVisibleChildren()
        {
            _visibleChildNodesCount = 0;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is TreeNode node && node.Visible)
                {
                    _visibleChildNodesCount++;
                }
            }
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
	        _cachedTextColor = CacheTextColor();

			// Prepare
			float y = DefaultHeaderHeight;
            float height = DefaultHeaderHeight;
            float xOffset = _xOffset + 12;
            if (Parent is Tree tree)
            {
                y = 4;
                xOffset = tree.RootNodesOffset;
            }
            else
            {
                y -= _cachedHeight * (_opened ? 1.0f - _animationProgress : _animationProgress);
            }

            // Arrange children
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is TreeNode node && node.Visible)
                {
                    node._xOffset = xOffset;
                    node.Location = new Vector2(0, y);
                    float nodeHeight = node.Height + DefaultNodeOffsetY;
                    y += nodeHeight;
                    height += nodeHeight;
                }
            }
            CacheVisibleChildren();

            // Cache calculated height
            _cachedHeight = height;

            // Force to be closed
            if (_animationProgress >= 1.0f && !_opened)
            {
                y = DefaultHeaderHeight;
            }

            // Set height
            Height = Mathf.Max(DefaultHeaderHeight, y);
        }
        
        /// <inheritdoc />
        protected override void OnParentChangedInternal()
        {
            // Clear cached tree
            _tree = null;
            if (Parent != null)
                Width = Parent.Width;

            base.OnParentChangedInternal();
        }
        
        /// <inheritdoc />
        public override void OnChildrenChanged()
        {
            base.OnChildrenChanged();

            CacheVisibleChildren();
        }

        /// <inheritdoc />
        public override int Compare(Control other)
        {
            if (other is TreeNode node)
            {
                return string.Compare(Text, node.Text, StringComparison.InvariantCulture);
            }
            return base.Compare(other);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            ParentTree?.Selection.Remove(this);

            base.OnDestroy();
        }
    }
}
