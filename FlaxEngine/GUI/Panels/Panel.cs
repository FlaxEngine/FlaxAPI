// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Panel UI control.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ScrollableControl" />
    public class Panel : ScrollableControl
    {
        private bool _layoutChanged;
        private int _layoutUpdateLock;
        private ScrollBars _scrollBars;

        /// <summary>
        /// The scroll right corner. Used to scroll contents of the panel control.
        /// </summary>
        [NoSerialize]
        protected Vector2 _scrollRightCorner;

        /// <summary>
        /// The vertical scroll bar.
        /// </summary>
        [HideInEditor, NoSerialize]
        public VScrollBar VScrollBar;

        /// <summary>
        /// The horizontal scroll bar.
        /// </summary>
        [HideInEditor, NoSerialize]
        public HScrollBar HScrollBar;

        /// <summary>
        /// Gets the scrolling right corner.
        /// </summary>
        [HideInEditor, NoSerialize]
        public Vector2 ScrollRightCorner
        {
            get => _scrollRightCorner;
            internal set => _scrollRightCorner = value;
        }

        /// <summary>
        /// Gets the view bottom.
        /// </summary>
        public Vector2 ViewBottom => Size + _viewOffset;

        /// <summary>
        /// Gets or sets the scroll bars usage by this panel.
        /// </summary>
        [EditorOrder(0), Tooltip("The scroll bars usage.")]
        public ScrollBars ScrollBars
        {
            get => _scrollBars;
            set
            {
                if (_scrollBars == value)
                    return;

                _scrollBars = value;

                if ((value & ScrollBars.Vertical) == ScrollBars.Vertical)
                {
                    // Create vertical scroll bar
                    VScrollBar = new VScrollBar(Width - ScrollBar.DefaultSize, Height)
                    {
                        DockStyle = DockStyle.Right,
                        Parent = this
                    };
                }
                else if (VScrollBar != null)
                {
                    VScrollBar.Dispose();
                    VScrollBar = null;
                }

                if ((value & ScrollBars.Horizontal) == ScrollBars.Horizontal)
                {
                    // Create vertical scroll bar
                    HScrollBar = new HScrollBar(Height - ScrollBar.DefaultSize, Width)
                    {
                        DockStyle = DockStyle.Bottom,
                        Parent = this
                    };
                }
                else if (HScrollBar != null)
                {
                    HScrollBar.Dispose();
                    HScrollBar = null;
                }

                PerformLayout();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <inheritdoc />
        public Panel()
        : this(ScrollBars.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="scrollBars">The scroll bars.</param>
        /// <param name="canFocus">True if control can accept user focus</param>
        public Panel(ScrollBars scrollBars, bool canFocus = false)
        {
            CanFocus = canFocus;

            ScrollBars = scrollBars;
        }

        /// <summary>
        /// Scrolls the view to the given control area.
        /// </summary>
        /// <param name="c">The control.</param>
        public void ScrollViewTo(Control c)
        {
            if (c == null)
                throw new ArgumentNullException();

            Vector2 location = c.Location;
            Vector2 size = c.Size;
            while (c.HasParent && c.Parent != this)
            {
                c = c.Parent;
                location = c.PointToParent(ref location);
            }

            if (c.HasParent)
            {
                ScrollViewTo(new Rectangle(location, size));
            }
        }

        /// <summary>
        /// Scrolls the view to the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        public void ScrollViewTo(Vector2 location)
        {
            ScrollViewTo(new Rectangle(location, Vector2.Zero));
        }

        /// <summary>
        /// Scrolls the view to the given area.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        public void ScrollViewTo(Rectangle bounds)
        {
            bool wasLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            if (HScrollBar != null && HScrollBar.Visible)
                HScrollBar.ScrollViewTo(bounds.Left, bounds.Right);
            if (VScrollBar != null && VScrollBar.Visible)
                VScrollBar.ScrollViewTo(bounds.Top, bounds.Bottom);

            IsLayoutLocked = wasLocked;
            PerformLayout();
        }

        internal void SetViewOffset(Orientation orientation, float value)
        {
            if (orientation == Orientation.Vertical)
                _viewOffset.Y = -value;
            else
                _viewOffset.X = -value;
            PerformLayout();
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            // Base
            if (base.OnMouseWheel(location, delta))
                return true;

            // Roll back to scroll bars
            if (VScrollBar != null && VScrollBar.Visible && VScrollBar.OnMouseWheel(VScrollBar.PointFromParent(ref location), delta))
                return true;
            if (HScrollBar != null && HScrollBar.Visible && HScrollBar.OnMouseWheel(HScrollBar.PointFromParent(ref location), delta))
                return true;

            // No event handled
            return false;
        }

        /// <inheritdoc />
        public override void RemoveChildren()
        {
            // Keep scroll bars alive
            if (VScrollBar != null)
                _children.Remove(VScrollBar);
            if (HScrollBar != null)
                _children.Remove(HScrollBar);

            base.RemoveChildren();

            // Restore scrollbars
            if (VScrollBar != null)
                _children.Add(VScrollBar);
            if (HScrollBar != null)
                _children.Add(HScrollBar);
            PerformLayout();
        }

        /// <inheritdoc />
        public override void DisposeChildren()
        {
            // Keep scrollbars alive
            if (VScrollBar != null)
                _children.Remove(VScrollBar);
            if (HScrollBar != null)
                _children.Remove(HScrollBar);

            base.DisposeChildren();

            // Restore scrollbars
            if (VScrollBar != null)
                _children.Add(VScrollBar);
            if (HScrollBar != null)
                _children.Add(HScrollBar);
            PerformLayout();
        }

        /// <inheritdoc />
        public override void OnChildResized(Control control)
        {
            base.OnChildResized(control);
            PerformLayout();
        }

        /// <inheritdoc />
        protected override bool IntersectsChildContent(Control child, Vector2 location, out Vector2 childSpaceLocation)
        {
            // For not scroll bars we want to reject any collisions
            if (child != VScrollBar && child != HScrollBar)
            {
                // Check if has v scroll bar to reject points on it
                if (VScrollBar != null && VScrollBar.Visible)
                {
                    Vector2 pos = VScrollBar.PointFromParent(ref location);
                    if (VScrollBar.ContainsPoint(ref pos))
                    {
                        childSpaceLocation = Vector2.Zero;
                        return false;
                    }
                }

                // Check if has h scroll bar to reject points on it
                if (HScrollBar != null && HScrollBar.Visible)
                {
                    Vector2 pos = HScrollBar.PointFromParent(ref location);
                    if (HScrollBar.ContainsPoint(ref pos))
                    {
                        childSpaceLocation = Vector2.Zero;
                        return false;
                    }
                }
            }

            return base.IntersectsChildContent(child, location, out childSpaceLocation);
        }

        /// <inheritdoc />
        internal override void AddChildInternal(Control child)
        {
            base.AddChildInternal(child);
            PerformLayout();
        }

        /// <inheritdoc />
        public override void PerformLayout(bool force = false)
        {
            if (_layoutUpdateLock > 2)
                return;
            _layoutUpdateLock++;

            if (!IsLayoutLocked)
            {
                _layoutChanged = false;
            }

            base.PerformLayout(force);

            if (!IsLayoutLocked && _layoutChanged)
            {
                _layoutChanged = false;
                PerformLayout(true);
            }

            _layoutUpdateLock--;
        }

        /// <inheritdoc />
        protected override void PerformLayoutSelf()
        {
            const float scrollSpaceLeft = 0.1f;

            // Arrange controls and get scroll bounds
            ArrangeAndGetBounds();

            // Scroll bars
            if (VScrollBar != null)
            {
                float height = Height;
                bool vScrollEnabled = _scrollRightCorner.Y > height + 0.01f && height > ScrollBar.DefaultMinimumSize;

                if (VScrollBar.Visible != vScrollEnabled)
                {
                    // Set scroll bar visibility 
                    VScrollBar.Visible = vScrollEnabled;
                    _layoutChanged = true;

                    // Clear scroll state
                    VScrollBar.Reset();
                    _viewOffset.Y = 0;

                    // Update
                    ArrangeAndGetBounds();
                }

                if (vScrollEnabled)
                {
                    VScrollBar.Maximum = _scrollRightCorner.Y - height * (1 - scrollSpaceLeft);
                }
            }
            if (HScrollBar != null)
            {
                float width = Width;
                bool hScrollEnabled = _scrollRightCorner.X > width + 0.01f && width > ScrollBar.DefaultMinimumSize;

                if (HScrollBar.Visible != hScrollEnabled)
                {
                    // Set scroll bar visibility 
                    HScrollBar.Visible = hScrollEnabled;
                    _layoutChanged = true;

                    // Clear scroll state
                    HScrollBar.Reset();

                    _viewOffset.X = 0;

                    // Update
                    ArrangeAndGetBounds();
                }

                if (hScrollEnabled)
                {
                    HScrollBar.Maximum = _scrollRightCorner.X - width * (1 - scrollSpaceLeft);
                }
            }
        }

        /// <summary>
        /// Arranges the child controls and gets their bounds.
        /// </summary>
        protected virtual void ArrangeAndGetBounds()
        {
            Arrange();

            // Calculate scroll area bounds
            Vector2 rigthBottom = Vector2.Zero;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Visible && c.IsScrollable)
                {
                    rigthBottom = Vector2.Max(rigthBottom, c.BottomRight);
                }
            }

            // Cache result
            _scrollRightCorner = rigthBottom;
        }

        /// <summary>
        /// Arranges the child controls.
        /// </summary>
        protected virtual void Arrange()
        {
            base.PerformLayoutSelf();
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            var result = base.OnDragMove(ref location, data);

            float width = Width;
            float height = Height;
            float MinSize = 70;
            float AreaSize = 25;
            float MoveScale = 4.0f;
            Vector2 viewOffset = -_viewOffset;

            if (VScrollBar != null && VScrollBar.Visible && height > MinSize)
            {
                if (new Rectangle(0, 0, width, AreaSize).Contains(ref location))
                {
                    viewOffset.Y -= MoveScale;
                }
                else if (new Rectangle(0, height - AreaSize, width, AreaSize).Contains(ref location))
                {
                    viewOffset.Y += MoveScale;
                }

                viewOffset.Y = Mathf.Clamp(viewOffset.Y, VScrollBar.Minimum, VScrollBar.Maximum);
                VScrollBar.Value = viewOffset.Y;
            }

            if (HScrollBar != null && HScrollBar.Visible && width > MinSize)
            {
                if (new Rectangle(0, 0, AreaSize, height).Contains(ref location))
                {
                    viewOffset.X -= MoveScale;
                }
                else if (new Rectangle(width - AreaSize, 0, AreaSize, height).Contains(ref location))
                {
                    viewOffset.X += MoveScale;
                }

                viewOffset.X = Mathf.Clamp(viewOffset.X, HScrollBar.Minimum, HScrollBar.Maximum);
                HScrollBar.Value = viewOffset.X;
            }

            viewOffset *= -1;

            if (viewOffset != _viewOffset)
            {
                _viewOffset = viewOffset;
                PerformLayout();
            }

            return result;
        }
    }
}
