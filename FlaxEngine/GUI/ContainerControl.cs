////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2018 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEngine.Assertions;

namespace FlaxEngine.GUI
{
    /// <summary>
    ///     Base interface for all GUI controls that can contain controls
    /// </summary>
    public class ContainerControl : Control
    {
        /// <summary>
        /// The children collection.
        /// </summary>
        protected readonly List<Control> _children = new List<Control>();

        /// <summary>
        /// The contains focus cached flag.
        /// </summary>
        protected bool _containsFocus;

        /// <summary>
        /// The option to update child controls layout first before self.
        /// Useful for controls which dimensions are based on children.
        /// </summary>
        protected bool _performChildrenLayoutFirst;

        /// <summary>
        ///      Action is invoked, when child control gets resized
        /// </summary>
        public event Action<Control> OnChildControlResized;

        ///<inheritdoc />
        public ContainerControl()
            : base(0, 0, 64, 64)
        {
            IsLayoutLocked = true;
        }

        /// <inheritdoc />
        public ContainerControl(float x, float y, float width, float height)
            : base(x, y, width, height)
        {
            IsLayoutLocked = true;
        }

        /// <inheritdoc />
        public ContainerControl(Vector2 location, Vector2 size)
            : base(location, size)
        {
            IsLayoutLocked = true;
        }

        /// <inheritdoc />
        public ContainerControl(Rectangle bounds)
            : base(bounds)
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
        /// Gets or sets a value indicating whether apply clipping mask on children during rendering.
        /// </summary>
        /// <value>
        ///   <c>true</c> if clip children; otherwise, <c>false</c>.
        /// </value>
        public bool ClipChildren { get; set; } = true;

        /// <summary>
        ///     Lock all child controls layout and itself
        /// </summary>
        public virtual void LockChildrenRecursive()
        {
            // Itself
            IsLayoutLocked = true;

            // Every child container control
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ContainerControl child)
                    child.LockChildrenRecursive();
            }
        }

        /// <summary>
        ///     Unlocks all child controls layout and itself
        /// </summary>
        public virtual void UnlockChildrenRecursive()
        {
            // Itself
            IsLayoutLocked = false;

            // Every child container control
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] is ContainerControl child)
                    child.UnlockChildrenRecursive();
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
        /// <returns>Added control.</returns>
        public T AddChild<T>(T child) where T : Control
        {
            if (child == null)
                throw new ArgumentNullException();
            if (child.Parent == this && _children.Contains(child))
                throw new InvalidOperationException("Argument child cannot be added, if current container is already its parent.");

            // Set child new parent
            child.Parent = this;

            return child;
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

        internal void ChangeChildIndex(Control child, int newIndex)
        {
            int oldIndex = _children.IndexOf(child);
            _children.RemoveAt(oldIndex);
            _children.Insert(newIndex, child);
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

                // Check collision
                Vector2 childLocation;
                if (IntersectsChildContent(child, point, out childLocation))
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

                // Check collision
                Vector2 childLocation;
                if (IntersectsChildContent(child, point, out childLocation))
                {
                    var containerControl = child as ContainerControl;
                    var childAtRecursive = containerControl?.GetChildAtRecursive(childLocation);
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

            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                if (!child.Visible)
                    continue;

                switch (child.DockStyle)
                {
                    case DockStyle.None: break;
                    case DockStyle.Top:
                    {
                        float height = Mathf.Min(child.Height, clientArea.Height);
                        clientArea.Location.Y += height;
                        clientArea.Size.Y -= height;
                        break;
                    }
                    case DockStyle.Bottom:
                    {
                        float height = Mathf.Min(child.Height, clientArea.Height);
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
                        float width = Mathf.Min(child.Width, clientArea.Width);
                        clientArea.Location.X += width;
                        clientArea.Size.X -= width;
                        break;
                    }
                    case DockStyle.Right:
                    {
                        float width = Mathf.Min(child.Width, clientArea.Width);
                        clientArea.Size.X -= width;
                        break;
                    }
                    default: throw new ArgumentOutOfRangeException();
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

        /// <summary>
        /// Called when child control gets resized.
        /// </summary>
        /// <param name="control">The resized control.</param>
        public virtual void OnChildResized(Control control)
        {
            OnChildControlResized?.Invoke(control);
        }

        /// <summary>
        /// Called when children collection gets changed (child added or removed).
        /// </summary>
        public virtual void OnChildrenChanged()
        {
            // Check if control isn't durig disposing state
            if (!IsDisposing)
            {
                // Arrange child controls
                PerformLayout();
            }
        }

        #region Internal Events

        /// <summary>
        ///     Add child control to the container
        /// </summary>
        /// <param name="child">Control to add</param>
        internal virtual void AddChildInternal(Control child)
        {
            Assert.IsNotNull(child, "Invalid control.");

            // Add child
            _children.Add(child);

            OnChildrenChanged();
        }

        /// <summary>
        ///     Remove child control from this container
        /// </summary>
        /// <param name="child">Control to remove</param>
        internal virtual void RemoveChildInternal(Control child)
        {
            Assert.IsNotNull(child, "Invalid control.");

            // Remove child
            _children.Remove(child);

            OnChildrenChanged();
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
        /// Checks if given point in thi container control space intersects with the child control content.
        /// Also calculates result location in child control space which can be used to feed control with event at that point.
        /// </summary>
        /// <param name="child">The child control to check.</param>
        /// <param name="location">The location in this container control space.</param>
        /// <param name="childSpaceLocation">The output location in child control space.</param>
        /// <returns>True if point is over the control content, otherwise false.</returns>
        protected virtual bool IntersectsChildContent(Control child, Vector2 location, out Vector2 childSpaceLocation)
        {
            return child.IntersectsContent(ref location, out childSpaceLocation);
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
                if (_children[i] is ContainerControl child)
                    child.UpdateContainsFocus();

                if (_children[i].ContainsFocus)
                    result = true;
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
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];

                if (!child.Visible)
                    continue;

                switch (child.DockStyle)
                {
                    case DockStyle.None: break;

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
                    default: throw new ArgumentOutOfRangeException();
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

            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];

                if (!child.Visible)
                    continue;

                switch (child.DockStyle)
                {
                    case DockStyle.None: break;
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
                    default: throw new ArgumentOutOfRangeException();
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
            base.OnDestroy();

            // Pass event futher
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].OnDestroy();
            }
            _children.Clear();
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
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Enabled)
                {
                    _children[i].Update(deltaTime);
                }
            }
        }

        /// <inheritdoc />
        public override void Draw()
        {
            base.Draw();

            // Push clipping mask
            if (ClipChildren)
            {
                Rectangle clientArea;
                GetDesireClientArea(out clientArea);
                Render2D.PushClip(ref clientArea);
            }

            DrawChildren();

            // Pop clipping mask
            if (ClipChildren)
            {
                Render2D.PopClip();
            }
        }

        /// <summary>
        /// Draws the children. Can be overriden to provide some customizations. Draw is performed with applied clipping mask fro the client area.
        /// </summary>
        protected virtual void DrawChildren()
        {
            // Draw all visible child controls
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];

                if (child.Visible)
                {
                    Render2D.PushTransform(ref child._cachedTransform);
                    child.Draw();
                    Render2D.PopTransform();
                }
            }
        }
		
	    /// <inheritdoc />
	    public override void PerformLayout(bool force = false)
        {
            // Check if update is locked
            if (IsLayoutLocked && !force)
                return;

	        bool wasLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Switch based on current mode
            if (_performChildrenLayoutFirst)
            {
                // Update children
                for (int i = 0; i < _children.Count; i++)
                    _children[i].PerformLayout(force);

                // Update itself
                PerformLayoutSelf();
            }
            else
            {
                // Update itself
                PerformLayoutSelf();

                // Update children
                for (int i = 0; i < _children.Count; i++)
                    _children[i].PerformLayout(force);
            }

            IsLayoutLocked = wasLocked;
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire event
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        // Enter
                        child.OnMouseEnter(childLocation);
                    }
                }
            }

            // Base
            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire events
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        if (child.IsMouseOver)
                        {
                            // Move
                            child.OnMouseMove(childLocation);
                        }
                        else
                        {
                            // Enter
                            child.OnMouseEnter(childLocation);
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
            // Check all children collisions with mouse and fire events for them
            for (int i = 0; i < _children.Count && _children.Count > 0; i++)
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
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire events
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        // Wheel
                        if (child.OnMouseWheel(childLocation, delta))
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
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire event
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        // Send event futher
                        if (child.OnMouseDown(childLocation, buttons))
                        {
                            return true;
                        }
                    }
                }
            }

            // Base
            return base.OnMouseDown(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire event
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        // Send event futher
                        if (child.OnMouseUp(childLocation, buttons))
                        {
                            return true;
                        }
                    }
                }
            }

            // Base
            return base.OnMouseUp(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire event
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        // Send event futher
                        if (child.OnMouseDoubleClick(childLocation, buttons))
                        {
                            return true;
                        }
                    }
                }
            }

            // Base
            return base.OnMouseDoubleClick(location, buttons);
        }

        /// <inheritdoc />
        public override bool OnCharInput(char c)
        {
            for (int i = 0; i < _children.Count && _children.Count > 0; i++)
            {
                var child = _children[i];
                if (child.Enabled && child.ContainsFocus)
                {
                    return child.OnCharInput(c);
                }
            }
            return false;
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            for (int i = 0; i < _children.Count && _children.Count > 0; i++)
            {
                var child = _children[i];
                if (child.Enabled && child.ContainsFocus)
                {
                    return child.OnKeyDown(key);
                }
            }
            return false;
        }

        /// <inheritdoc />
        public override void OnKeyUp(Keys key)
        {
            for (int i = 0; i < _children.Count && _children.Count > 0; i++)
            {
                var child = _children[i];
                if (child.Enabled && child.ContainsFocus)
                {
                    child.OnKeyUp(key);
                    break;
                }
            }
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragEnter(ref Vector2 location, DragData data)
        {
            // Base
            var result = base.OnDragEnter(ref location, data);

            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire event
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        // Enter
                        result = child.OnDragEnter(ref childLocation, data);
                        if (result != DragDropEffect.None)
                            break;
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragMove(ref Vector2 location, DragData data)
        {
            // Base
            var result = base.OnDragMove(ref location, data);

            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire events
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        if (child.IsDragOver)
                        {
                            // Move
                            var tmpResult = child.OnDragMove(ref childLocation, data);
                            if (tmpResult != DragDropEffect.None)
                                result = tmpResult;
                        }
                        else
                        {
                            // Enter
                            var tmpResult = child.OnDragEnter(ref childLocation, data);
                            if (tmpResult != DragDropEffect.None)
                                result = tmpResult;
                        }
                    }
                    else if (child.IsDragOver)
                    {
                        // Leave
                        child.OnDragLeave();
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override void OnDragLeave()
        {
            // Base
            base.OnDragLeave();

            // Check all children collisions with mouse and fire events for them
            for (int i = 0; i < _children.Count && _children.Count > 0; i++)
            {
                var child = _children[i];
                if (child.IsDragOver)
                {
                    // Leave
                    child.OnDragLeave();
                }
            }
        }

        /// <inheritdoc />
        public override DragDropEffect OnDragDrop(ref Vector2 location, DragData data)
        {
            // Base
            var result = base.OnDragDrop(ref location, data);

            // Check all children collisions with mouse and fire events for them
            for (int i = _children.Count - 1; i >= 0 && _children.Count > 0; i--)
            {
                var child = _children[i];
                if (child.Visible && child.Enabled)
                {
                    // Fire event
                    Vector2 childLocation;
                    if (IntersectsChildContent(child, location, out childLocation))
                    {
                        // Enter
                        result = child.OnDragDrop(ref childLocation, data);
                        if (result != DragDropEffect.None)
                            break;
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(Vector2 size)
        {
            // Lock updates to prevent additional layout calculations
            bool wasLayoutLocked = IsLayoutLocked;
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
            IsLayoutLocked = wasLayoutLocked;

            // Arrange child controls
            PerformLayout();
        }

        #endregion
    }
}
