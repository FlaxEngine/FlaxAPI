////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Tree node control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControlChildrenSized" />
    public class TreeNode : ContainerControlChildrenSized
    {
        /// <summary>
        /// The default node header height.
        /// </summary>
        public const float DefaultHeaderHeight = 16.0f;

        public const float DefaultDragInsertPositionMargin = 2.0f;
        public const float DefaultNodeOffset = 1;

        protected Tree _tree;
        protected bool _opened, _canChangeOrder;
        protected float _animationProgress, _cachedHeight;
        protected bool _mouseOverArrow, _mouseOverHeader;
        protected float _xOffset, _textWidth;
        protected Rectangle _headerRect;
        protected Sprite _iconCollaped, _iconOpened;
        protected string _text;
        protected bool _isMouseDown;
        protected float _mouseDownTime;
        protected Vector2 _mouseDownPos;

        protected DragItemPositioning _dragOverMode;
        protected bool _isDragOverHeader;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this node is expanded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this node is expanded; otherwise, <c>false</c>.
        /// </value>
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
        /// <value>
        ///   <c>true</c> if this node is collapsed; otherwise, <c>false</c>.
        /// </value>
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
        /// <value>
        /// The parent tree.
        /// </value>
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
        /// <value>
        ///   <c>true</c> if this node is root; otherwise, <c>false</c>.
        /// </value>
        public bool IsRoot => !(Parent is TreeNode);

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        /// <value>
        /// The color of the text.
        /// </value>
        public virtual Color TextColor => Enabled ? Style.Current.Foreground : Style.Current.ForegroundDisabled;

        /// <summary>
        /// Gets the minimum width of the node sub-tree.
        /// </summary>
        /// <value>
        /// The minimum width.
        /// </value>
        public virtual float MinimumWidth
        {
            get
            {
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
            : base(true, 0, 0, 64, 16)
        {
            _canChangeOrder = canChangeOrder;
            _animationProgress = 1.0f;
            _cachedHeight = DefaultHeaderHeight;
            _iconCollaped = iconCollapsed;
            _iconOpened = iconOpened;
            _mouseDownTime = -1;
        }

        /// <summary>
        /// Expand node.
        /// </summary>
        public void Expand()
        {
            // Parents first
            ExpandAllParents();

            // Chnage state
            bool prevStae = _opened;
            _opened = true;
            if (prevStae != _opened)
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
            bool prevStae = _opened;
            _opened = false;
            if (prevStae != _opened)
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
        /// Select node in the tree.
        /// </summary>
        public void Select()
        {
            ParentTree.Select(this);
        }

        // TODO: finsih drag and drop
        /*protected virtual DragDropEffect onDragEnter(IGuiData* data);
        protected virtual DragDropEffect onDragOver(IGuiData* data);
        protected virtual DragDropEffect onDragDrop(IGuiData* data);
        protected virtual void onDragLeave();
        protected virtual void doDragDrop();*/

        protected virtual void OnLongPress()
        {
        }

        protected virtual bool testHeaderHit(ref Vector2 location)
        {
            return _headerRect.Contains(ref location);
        }

        private void updateDrawPositioning(ref Vector2 location)
        {

        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Drop/down animation
            if (_animationProgress < 1.0f)
            {
                // Update progress
                const float openCloseAniamtionTime = 0.1f;
                _animationProgress += deltaTime / openCloseAniamtionTime;
                if (_animationProgress > 1.0f)
                    _animationProgress = 1.0f;

                // Arrange controls
                PerformLayout();
            }

            // Check for long press
            const float longPressTimeSeconds = 0.6f;
            if (_isMouseDown && Time.UnscaledTime - _mouseDownTime > longPressTimeSeconds)
            {
                OnLongPress();
            }

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Check if it's a root
            if (!IsRoot)
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
                bool hasChildren = _children.Count > 0;
                bool isSelected = tree.Selection.Contains(this);
                bool isFocused = tree.ContainsFocus;
                var textRect = new Rectangle(_xOffset + 4 + DefaultHeaderHeight, 0, 10000, DefaultHeaderHeight);

                // Draw background
                if (isSelected || _mouseOverHeader)
                    Render2D.FillRectangle(_headerRect, (isSelected && isFocused) ? style.BackgroundSelected : (IsMouseOver ? style.BackgroundHighlighted : style.LightBackground));

                // Draw arrow
                if (hasChildren)
                    Render2D.DrawSprite(_opened ? style.ArrowDown : style.ArrowRight, new Rectangle(_xOffset + 2, 2, 12, 12), _mouseOverHeader ? Color.White : new Color(0.8f, 0.8f, 0.8f, 0.8f));

                // Draw icon
                if (_iconCollaped.IsValid)
                {
                    Render2D.DrawSprite(_opened ? _iconOpened : _iconCollaped, new Rectangle(textRect.Left - 2, 0, 16, 16));
                    textRect.Offset(16, 0);
                }

                // Draw text
                Render2D.DrawText(style.FontSmall, _text, textRect, TextColor, TextAlignment.Near, TextAlignment.Center);

                // Draw drag and drop effect
                if (IsDragOver)
                {
                    Color dragOverColor = style.BackgroundHighlighted * 0.4f;
                    Rectangle rect;
                    switch (_dragOverMode)
                    {
                        case DragItemPositioning.At:
                            rect = textRect;
                            break;
                        case DragItemPositioning.Above:
                            rect = new Rectangle(textRect.X, textRect.Y - DefaultDragInsertPositionMargin - DefaultNodeOffset, textRect.Width, DefaultDragInsertPositionMargin * 2.0f);
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

        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check if mosue hits bar and node isn't a root
            if (_mouseOverHeader && !IsRoot)
            {
                // Check if left buton goes down
                if (buttons == MouseButtons.Left)
                {
                    _isMouseDown = true;
                    _mouseDownPos = location;
                    _mouseDownTime = Time.UnscaledTime;
                }

                // Handled
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
        public override void OnDestroy()
        {
            // Deselect
            ParentTree?.Selection.Remove(this);

            base.OnDestroy();
        }
    }
}
