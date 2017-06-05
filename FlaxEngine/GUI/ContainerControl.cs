////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlaxEngine.GUI
{
    /// <summary>
    ///     Base interface for all GUI controls that can contain controls
    /// </summary>
    public class ContainerControl : Control
    {
        protected readonly List<Control> _children = new List<Control>();
        protected Vector2 _viewOffset;
        protected bool _containsFocus;

        /// <summary>
        ///      Action is invoked, when child control gets resized
        /// </summary>
        public event Action<Control> OnChildResized;

        ///<inheritdoc />
        protected ContainerControl(bool canFocus)
            : base(canFocus, 0, 0, 64, 64)
        {
        }

        ///<inheritdoc />
        protected ContainerControl(bool canFocus, float x, float y, float width, float height)
            : base(canFocus, x, y, width, height)
        {
        }

        ///<inheritdoc />
        protected ContainerControl(bool canFocus, Vector2 location, Vector2 size)
            : base(canFocus, location, size)
        {
        }

        ///<inheritdoc />
        protected ContainerControl(bool canFocus, Rectangle bounds)
            : base(canFocus, bounds)
        {
            IsLayoutLocked = true;
        }

        /// <summary>
        ///     Gets child controls list
        /// </summary>
        public List<Control> Children => _children;

        /// <summary>
        ///     Gets amount of the children controls
        /// </summary>
        public int ChildrenCount => _children.Count;

        /// <summary>
        ///     Checks if container has any child controls
        /// </summary>
        public bool HasChildren => _children.Count > 0;

        /// <summary>
        ///     Gets current view offset for all the controls (used by the scroll bars)
        /// </summary>
        public Vector2 ViewOffset => _viewOffset;

        /// <summary>
        ///     Gets a value indicating whether the control, or one of its child controls, currently has the input focus
        /// </summary>
        /// <returns>True if the control, or one of its child controls, currently has the input focus</returns>
        public override bool ContainsFocus => _containsFocus;

        /// <summary>
        ///     True if automatic updates for control layout are locked (usefull when createing a lot of GUI control to prevent
        ///     lags)
        /// </summary>
        public bool IsLayoutLocked { get; set; }

        /// <summary>
        ///     Lock all child controls and itself
        /// </summary>
        public virtual void LockChildrenRecursive()
        {
            // Itself
            IsLayoutLocked = true;

            // Every child container control
            for (int i = 0; i < _children.Count; i++)
            {
                var containerControl = _children[i] as ContainerControl;
                containerControl?.LockChildrenRecursive();
            }
        }

        /// <summary>
        ///     Unlocks all child controls and itself
        /// </summary>
        public virtual void UnlockChildrenRecursive()
        {
            // Itself
            IsLayoutLocked = false;

            // Every child container control
            for (int i = 0; i < _children.Count; i++)
            {
                var containerControl = _children[i] as ContainerControl;
                containerControl?.UnlockChildrenRecursive();
            }
        }

        /// <summary>
        ///     Unlink all child controls
        /// </summary>
        public virtual void RemoveChildren()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;
            
            // Delete children
            while (_children.Count > 0)
            {
                _children[0].Parent = null;
            }

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        ///     Remove and dispose all child controls
        /// </summary>
        public virtual void DisposeChildren()
        {
            bool wasLayoutLocked = IsLayoutLocked;
            IsLayoutLocked = true;
            
            // Delete children
            while (_children.Count > 0)
            {
                _children[0].Dispose();
            }

            IsLayoutLocked = wasLayoutLocked;
            PerformLayout();
        }

        /// <summary>
        ///     Add control to the container
        /// </summary>
        /// <param name="child">Control to add</param>
        public void AddChild(Control child)
        {
            if(child == null)
                throw new ArgumentNullException();
            if (child.Parent == this && _children.Contains(child))
                throw new InvalidOperationException("Argument child cannot be added, if current container is already its parent.");

            // Remove child from his old parent
            child.Parent?.RemoveChildInternal(child);
            // Set child new parent
            child.Parent = this;
            
            // Add this child to current parent
            AddChildInternal(child);
        }

        /// <summary>
        ///     Remove control from the container
        /// </summary>
        /// <param name="child">Control to remove</param>
        public void RemoveChild(Control child)
        {
            if (child == null)
                throw new ArgumentNullException();
            if (child.Parent != this)
                throw new InvalidOperationException("Argument child cannot be removed, if current container is not its parent.");

            // Remove child from his current parent
            RemoveChildInternal(child);

            // Unlink
            child.Parent = null;
        }

        /// <summary>
        ///     Gets child control at given idnex
        /// </summary>
        /// <param name="index">Control index</param>
        /// <returns>Control handle</returns>
        public Control GetChild(int index)
        {
            return _children[index];
        }

        /// <summary>
        ///     Gets zero-based index in the list of control children
        /// </summary>
        /// <param name="child">Child control</param>
        /// <returns>Zero-based index in the list of control children</returns>
        public int GetChildIndex(Control child)
        {
            return _children.IndexOf(child);
        }

        /// <summary>
        ///     Tries to find any child contol at given point in control local coordinates
        /// </summary>
        /// <param name="point">Local point to check</param>
        /// <returns>Found control or null</returns>
        public Control GetChildAt(Vector2 point)
        {
            Control result = null;
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                Vector2 scrollOffsetLocation = point;
                if (child.IsScrollable)
                {
                    scrollOffsetLocation -= _viewOffset;
                }

                // Check collision
                if (child.ContainsPoint(ref scrollOffsetLocation))
                {
                    result = child;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        ///     Tries to find lowest child contol at given point in control local coordinates
        /// </summary>
        /// <param name="point">Local point to check</param>
        /// <returns>Found control or null</returns>
        public Control GetChildAtRecursive(Vector2 point)
        {
            Control result = null;
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                Vector2 scrollOffsetLocation = point;
                if (child.IsScrollable)
                {
                    scrollOffsetLocation -= _viewOffset;
                }

                // Check collision
                if (child.ContainsPoint(ref scrollOffsetLocation))
                {
                    var containerControl = child as ContainerControl;
                    var childAtRecursive = containerControl?.GetChildAtRecursive(scrollOffsetLocation - child.Location);
                    if (childAtRecursive != null)
                    {
                        child = childAtRecursive;
                    }
                    result = child;
                }
            }
            return result;
        }

        /// <summary>
        ///     Gets rectangle in local control coordinates with area for controls (without scroll bars, docked controls, etc.)
        /// </summary>
        /// <returns>Rectangle in local control coordinates with area for controls (without scroll bars etc.)</returns>
        public Rectangle GetClientArea()
        {
            Rectangle clientArea;
            GetDesireClientArea(out clientArea);

            foreach (var control in _children)
            {
                if (!control.Visible)
                {
                    continue;
                }
                switch (control.DockStyle)
                {
                    case DockStyle.None:
                        break;
                    case DockStyle.Top:
                    {
                        float height = Mathf.Min(control.Height, clientArea.Height);
                        clientArea.Location.Y += height;
                        clientArea.Size.Y -= height;
                        break;
                    }
                    case DockStyle.Bottom:
                    {
                        float height = Mathf.Min(control.Height, clientArea.Height);
                        clientArea.Size.Y -= height;
                        break;
                    }
                    case DockStyle.Fill:
                    {
                        GetDesireClientArea(out clientArea);
                        break;
                    }
                    case DockStyle.Left:
                    {
                        float width = Mathf.Min(control.Width, clientArea.Width);
                        clientArea.Location.X += width;
                        clientArea.Size.X -= width;
                        break;
                    }
                    case DockStyle.Right:
                    {
                        float width = Mathf.Min(control.Width, clientArea.Width);
                        clientArea.Size.X -= width;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return clientArea;
        }

        /// <summary>
        ///     Sort child controls list
        /// </summary>
        public void SortChildren()
        {
            _children.Sort();
            PerformLayout();
        }

        /// <summary>
        ///     Sort children using recursion
        /// </summary>
        public void SortChildrenRecursive()
        {
            SortChildren();

            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i] as ContainerControl;
                child?.SortChildrenRecursive();
            }
        }

        #region Internal Events

        /// <summary>
        ///     Add child control to the container
        /// </summary>
        /// <param name="child">Control to add</param>
        protected virtual void AddChildInternal(Control child)
        {
            Debug.Assert(child != null, "Invalid control.");

            // Add child
            _children.Add(child);

            // Arragne child controls
            PerformLayout();
        }

        /// <summary>
        ///     Remove child control from this container
        /// </summary>
        /// <param name="child">Control to remove</param>
        protected virtual void RemoveChildInternal(Control child)
        {
            Debug.Assert(child != null, "Invalid control.");

            // Remove child
            _children.Remove(child);

            // Check if control isn't durig disposing state
            if (!IsDisposing)
            {
                // Arragne child controls
                PerformLayout();
            }
        }

        /// <summary>
        ///     Get desire cleint area rectangle for all controls
        /// </summary>
        /// <param name="rect">Rectangle for controls</param>
        protected virtual void GetDesireClientArea(out Rectangle rect)
        {
            rect = new Rectangle(0, 0, Size);
        }

        /// <summary>
        ///     Update contain focus state and all it's children
        /// </summary>
        protected void UpdateContainsFocus()
        {
            // Get current state and update all children
            bool result = base.ContainsFocus;

            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i] as ContainerControl;
                child?.UpdateContainsFocus();
                if (_children[i].ContainsFocus)
                {
                    result = true;
                }
            }

            // Check if state has been changed
            if (result != _containsFocus)
            {
                // Cache flag
                _containsFocus = result;

                // Fire event
                if (result)
                {
                    OnStartContainsFocus();
                }
                else
                {
                    OnEndContainsFocus();
                }
            }
        }

        /// <summary>
        ///     Arrange docked controls and return final client area for other controls
        /// </summary>
        /// <param name="clientArea">Result client area</param>
        protected void ArrangeDockedControls(ref Rectangle clientArea)
        {
            foreach (var child in _children)
            {
                if (!child.Visible)
                {
                    continue;
                }
                switch (child.DockStyle)
                {
                    case DockStyle.None:
                        break;

                    case DockStyle.Bottom:
                    {
                        float height = child.Height;
                        float width = clientArea.Width;
                        child.SetSize(width, height);
                        child.SetLocation(clientArea.Left, clientArea.Bottom - height);
                        clientArea.Size.Y -= height;
                        break;
                    }
                    case DockStyle.Fill:
                    {
                        child.Size = clientArea.Size;
                        child.Location = clientArea.Location;
                        GetDesireClientArea(out clientArea);
                        break;
                    }
                    case DockStyle.Left:
                    {
                        float width = child.Width;
                        float height = clientArea.Height;
                        child.SetSize(width, height);
                        child.SetLocation(clientArea.Left, clientArea.Top);
                        clientArea.Location.X += width;
                        clientArea.Size.X -= width;
                        break;
                    }
                    case DockStyle.Right:
                    {
                        float width = child.Width;
                        float height = clientArea.Height;
                        child.SetSize(width, height);
                        child.SetLocation(clientArea.Right - width, clientArea.Top);
                        clientArea.Size.X -= width;
                        break;
                    }
                    case DockStyle.Top:
                    {
                        float height = child.Height;
                        float width = clientArea.Width;
                        child.SetSize(width, height);
                        child.SetLocation(clientArea.Left, clientArea.Top);
                        clientArea.Location.Y += height;
                        clientArea.Size.Y -= height;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Use docked controls to calculate return final client area for other controls
        /// </summary>
        /// <param name="clientArea">Result client area</param>
        protected void CalculateDockedControlsClientRect(out Rectangle clientArea)
        {
            GetDesireClientArea(out clientArea);

            foreach (var child in _children)
            {
                if (!child.Visible)
                {
                    continue;
                }
                switch (child.DockStyle)
                {
                    case DockStyle.None:
                        break;
                    case DockStyle.Top:
                    {
                        float height = child.Height;
                        clientArea.Location.Y += height;
                        clientArea.Size.Y -= height;
                        break;
                    }
                    case DockStyle.Bottom:
                    {
                        clientArea.Size.Y -= child.Height;
                        break;
                    }
                    case DockStyle.Fill:
                    {
                        GetDesireClientArea(out clientArea);
                        break;
                    }
                    case DockStyle.Left:
                    {
                        float width = child.Width;
                        clientArea.Location.X += width;
                        clientArea.Size.X -= width;
                        break;
                    }
                    case DockStyle.Right:
                    {
                        clientArea.Size.X -= child.Width;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Perform layout for that container control
        /// </summary>
        protected virtual void PerformLayoutSelf()
        {
            // By default we arrange only docked controls
            Rectangle clientArea;
            GetDesireClientArea(out clientArea);
            ArrangeDockedControls(ref clientArea);
        }

        #endregion

        #region Control

        /// <inheritdoc />
        public override bool HasMouseCapture
        {
            get
            {
                for (int i = 0; i < _children.Count; i++)
                {
                    if (_children[i].HasMouseCapture)
                        return true;
                }
                return false;
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (IsDisposing)
                return;

            // Steal focus from children
            if (ContainsFocus)
            {
                Focus();
            }

            // Base
            base.Dispose();
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Pass event futher
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].OnDestroy();
            }
            _children.Clear();

            base.OnDestroy();
        }

        /// <inheritdoc />
        public override bool IsMouseOver
        {
            get
            {
                if (base.IsMouseOver)
                {
                    return true;
                }

                return _children.Any(child => child.IsMouseOver);
            }
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Base
            base.Update(deltaTime);

            // Update all enabled child controls
            foreach (var child in _children)
            {
                if (child.Enabled)
                {
                    child.Update(deltaTime);
                }
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Base
            base.Draw();

            Vector2 transform = Render2D.Transform;

            // Push clipping mask
            Rectangle clientArea;
            GetDesireClientArea(out clientArea);
            Render2D.PushClip(clientArea);

            // Draw all visible child controls
            foreach (Control child in _children)
            {
                if (child.Visible)
                {
                    Vector2 childTransform = transform + child.Location;
                    if (child.IsScrollable)
                    {
                        childTransform += _viewOffset;
                    }

                    Render2D.Transform = childTransform;
                    child.Draw();
                }
            }

            // Pop clipping mask
            Render2D.PopClip();

            // Restore render transform
            Render2D.Transform = transform;
        }

        /// <inheritdoc />
        public override void PerformLayout()
        {
            // Check if update is locked
            if (IsLayoutLocked)
            {
                return;
            }

            IsLayoutLocked = true;

            // Update itself
            PerformLayoutSelf();

            // Update children
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].PerformLayout();
            }

            IsLayoutLocked = false;
        }


        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    var scrollOffsetLocation = location;
                    if (child.IsScrollable)
                    {
                        scrollOffsetLocation -= _viewOffset;
                    }

                    // Fire event
                    if (child.ContainsPoint(ref scrollOffsetLocation))
                    {
                        // Enter
                        child.OnMouseEnter(scrollOffsetLocation - child.Location);
                    }
                }
            }

            // Base
            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    var scrollOffsetLocation = location;
                    if (child.IsScrollable)
                    {
                        scrollOffsetLocation -= _viewOffset;
                    }

                    // Fire events
                    if (child.ContainsPoint(ref scrollOffsetLocation) || child.HasMouseCapture)
                    {
                        if (child.IsMouseOver)
                        {
                            // Move
                            child.OnMouseMove(scrollOffsetLocation - child.Location);
                        }
                        else
                        {
                            // Enter
                            child.OnMouseEnter(scrollOffsetLocation - child.Location);
                        }
                    }
                    else if (child.IsMouseOver)
                    {
                        // Leave
                        child.OnMouseLeave();
                    }
                }
            }
        }

        /// <inheritdoc />
        public override void OnMouseLeave()
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled && child.IsMouseOver)
                {
                    // Leave
                    child.OnMouseLeave();
                }
            }

            // Base
            base.OnMouseLeave();
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, short delta)
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    var scrollOffsetLocation = location;
                    if (child.IsScrollable)
                    {
                        scrollOffsetLocation -= _viewOffset;
                    }

                    // Fire events
                    if (child.ContainsPoint(ref scrollOffsetLocation))
                    {
                        // Wheel
                        if (child.OnMouseWheel(scrollOffsetLocation - child.Location, delta))
                        {
                            return true;
                        }
                    }
                }
            }

            // Base
            return base.OnMouseWheel(location, delta);
        }

        /// <inheritdoc />
        public override bool OnMouseDown(MouseButtons buttons, Vector2 location)
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    var scrollOffsetLocation = location;
                    if (child.IsScrollable)
                    {
                        scrollOffsetLocation -= _viewOffset;
                    }

                    // Fire event
                    if (child.ContainsPoint(ref scrollOffsetLocation))
                    {
                        // Send event futher
                        if (child.OnMouseDown(buttons, scrollOffsetLocation - child.Location))
                        {
                            return true;
                        }
                    }
                    else if (child.HasMouseCapture)
                    {
                        // Cancel forced user focus
                        child.OnLostMouseCapture();
                    }
                }
            }

            // Base
            return base.OnMouseDown(buttons, location);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(MouseButtons buttons, Vector2 location)
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    var scrollOffsetLocation = location;
                    if (child.IsScrollable)
                    {
                        scrollOffsetLocation -= _viewOffset;
                    }

                    // Fire event
                    if (child.HasMouseCapture)
                    {
                        // Send event futher
                        if (child.OnMouseUp(buttons, scrollOffsetLocation - child.Location))
                        {
                            return true;
                        }
                    }
                }
            }
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    var scrollOffsetLocation = location;
                    if (child.IsScrollable)
                    {
                        scrollOffsetLocation -= _viewOffset;
                    }

                    // Fire event
                    if (child.ContainsPoint(ref scrollOffsetLocation))
                    {
                        // Send event futher
                        if (child.OnMouseUp(buttons, scrollOffsetLocation - child.Location))
                        {
                            return true;
                        }
                    }
                }
            }

            // Base
            return base.OnMouseUp(buttons, location);
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(MouseButtons buttons, Vector2 location)
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    var scrollOffsetLocation = location;
                    if (child.IsScrollable)
                    {
                        scrollOffsetLocation -= _viewOffset;
                    }


                    // Fire event
                    if (child.ContainsPoint(ref scrollOffsetLocation))
                    {
                        // Send event futher
                        if (child.OnMouseDoubleClick(buttons, scrollOffsetLocation - child.Location))
                        {
                            return true;
                        }
                    }
                }
            }

            // Base
            return base.OnMouseDoubleClick(buttons, location);
        }

        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Enabled && (child.ContainsFocus || child.HasMouseCapture))
                {
                    return child.OnKeyDown(key);
                }
            }
            return false;
        }

        /// <inheritdoc />
        public override void OnKeyUp(KeyCode key)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (child.Enabled && (child.ContainsFocus || child.HasMouseCapture))
                {
                    child.OnKeyUp(key);
                    break;
                }
            }
        }

        // TODO: drag and drop support
        //DragDropEffect OnDragEnter(IGuiData data, Vector2 location);
        //DragDropEffect OnDragOver(IGuiData data, Vector2 location);
        //void OnDragLeave() override;
        //DragDropEffect OnDragDrop(IGuiData data, Vector2 location);

        /// <inheritdoc />
        public override Vector2 PointToParent(Vector2 location)
        {
            return base.PointToParent(location) + _viewOffset;
        }

        /// <inheritdoc />
        public override Vector2 PointFromParent(Vector2 location)
        {
            return base.PointFromParent(location) - _viewOffset;
        }

        /// <inheritdoc />
        public override Vector2 PointToWindow(Vector2 location)
        {
            return base.PointToWindow(location) + _viewOffset;
        }

        /// <inheritdoc />
        public override Vector2 PointFromWindow(Vector2 location)
        {
            return base.PointFromWindow(location) - _viewOffset;
        }

        /// <inheritdoc />
        public override Vector2 ScreenToClient(Vector2 location)
        {
            return base.ScreenToClient(location) - _viewOffset;
        }

        /// <inheritdoc />
        public override Vector2 ClientToScreen(Vector2 location)
        {
            return base.ClientToScreen(location) + _viewOffset;
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(Vector2 size)
        {
            // Lock updates to prevent additional layout calculations
            bool wasLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Cache previous size
            Vector2 prevSize = Size;

            // Base
            base.SetSizeInternal(size);

            // Fire event
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].OnParentResized(ref prevSize);
            }

            // Restore state
            IsLayoutLocked = wasLocked;

            // Arrange child controls
            PerformLayout();
        }

        // TODO: implement user focus like in C++ or do it in another way?
        //bool hasUserFocus();
        //void onLostUserFocus();

        #endregion
    }
}