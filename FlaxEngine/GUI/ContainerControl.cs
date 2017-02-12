////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace FlaxEngine.GUI
{
    /// <summary>
    /// Base interface for all GUI controls that can contain controls
    /// </summary>
    public class ContainerControl : Control
    {
        protected List<Control> _children;
        protected Vector2 _viewOffset;
        protected bool _containsFocus;

        /// <summary>
        /// Gets child controls list
        /// </summary>
        public List<Control> Children
        {
            get { return _children; }
        }

        /// <summary>
        /// Gets amount of the children controls
        /// </summary>
        public int ChuldrenCount
        {
            get { return _children.Count; }
        }

        /// <summary>
        /// Checks if container has any child controls
        /// </summary>
        public bool HasChildren
        {
            get { return _children.Count > 0; }
        }

        /// <summary>
        /// Gets current view offset for all the controls (used by the scroll bars)
        /// </summary>
        public Vector2 ViewOffset
        {
            get { return _viewOffset; }
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="canFocus">True if control can accept user focus</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        protected ContainerControl(bool canFocus, float x, float y, float width, float height)
            : base(canFocus, x, y, width, height)
        {
            IsLayoutLocked = true;
        }

        #region Layout locking

        /// <summary>
        /// True if automatic updates for control layout are locked (usefull when createing a lot of GUI control to prevent lags)
        /// </summary>
        public bool IsLayoutLocked;

        /// <summary>
        /// Lock all child controls and itself
        /// </summary>
        public virtual void LockChildrenRecursive()
        {
            // Itself
            IsLayoutLocked = true;

            // Every child container control
            for (int i = 0; i < _children.Count; i++)
            {
                var cc = _children[i] as ContainerControl;
                if (cc != null)
                    cc.LockChildrenRecursive();
            }
        }

        /// <summary>
        /// Unlocks all child controls and itself
        /// </summary>
        public virtual void UnlockChildrenRecursive()
        {            
            // Itself
            IsLayoutLocked = false;

            // Every child container control
            for (int i = 0; i < _children.Count; i++)
            {
                var cc = _children[i] as ContainerControl;
                if (cc != null)
                    cc.UnlockChildrenRecursive();
            }
        }

        #endregion

        /// <summary>
        /// Unlink all child controls
        /// </summary>
        public virtual void RemoveChildren()
        {
            throw new NotImplementedException();
            /*// Delete children
            bool wereUpdatesLocked = IsUpdateLocked;
            IsUpdateLocked = true;
            while (_children.Count() > 0)
            {
                _children[0]->SetParent(nullptr);
            }
            IsUpdateLocked = wereUpdatesLocked;
            PerformLayout();*/
        }

        /// <summary>
        /// Remove and dispose all child controls
        /// </summary>
        public virtual void DisposeChildren()
        {
            throw new NotImplementedException();
            /*// Defocus any child
            if (ContainsFocus() && !IsFocused())
                Focus();

            // Delete children
            bool wereUpdatesLocked = IsUpdateLocked;
            IsUpdateLocked = true;
            while (_children.Count() > 0)
            {
                _children[0]->Destroy(forceNow);
            }
            IsUpdateLocked = wereUpdatesLocked;
            PerformLayout();*/
        }

        /// <summary>
        /// Add control to the container
        /// </summary>
        /// <param name="c">Control to add</param>
        public void AddChild(Control c)
        {
            throw new NotImplementedException();
            //ASSERT(c);
            //c->SetParent(this);
        }

        /// <summary>
        /// Remove control from the container
        /// </summary>
        /// <param name="c">Control to remove</param>
        public void RemvoeChild(Control c)
        {
            throw new NotImplementedException();
            //ASSERT(c && c->GetParent() == this);
            //c->SetParent(nullptr);
        }

        /// <summary>
        /// Gets child control at given idnex
        /// </summary>
        /// <param name="index">Control index</param>
        /// <returns>Control handle</returns>
        public Control GetChild(int index)
        {
            return _children[index];
        }

        /// <summary>
        /// Gets zero-based index in the list of control children
        /// </summary>
        /// <param name="c">Child control</param>
        /// <returns>Zero-based index in the list of control children</returns>
        public int GetChildIndex(Control c)
        {
            return _children.IndexOf(c);
        }

        /// <summary>
        /// Tries to find any child contol at given point in control local coordinates
        /// </summary>
        /// <param name="point">Local point to check</param>
        /// <returns>Found control or null</returns>
        public Control GetChildAt(Vector2 point)
        {
            Control result = null;
            for (int i = 9; i < _children.Count; i++)
            {
                var c = _children[i];
                Vector2 location = point;
                if (c.IsScrollable)
                    location -= _viewOffset;

                // Check collision
                if (isMouseOver(c, ref location))
                {
                    result = c;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Tries to find lowest child contol at given point in control local coordinates
        /// </summary>
        /// <param name="point">Local point to check</param>
        /// <returns>Found control or null</returns>
        public Control GetChildAtRecursive(Vector2 point)
        {
            Control result = null;
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                Vector2 location = point;
                if (c.IsScrollable)
                    location -= _viewOffset;

                // Check collision
                if (isMouseOver(c, ref location))
                {
                    var cc = c as ContainerControl;
                    if (cc != null)
                    {
                        var ccc = cc.GetChildAtRecursive(location - c.Location);
                        if (ccc != null)
                            c = ccc;
                    }
                    result = c;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets rectangle in local control coordinates with area for controls (without scroll bars, docked controls, etc.)
        /// </summary>
        /// <returns>Rectangle in local control coordinates with area for controls (without scroll bars etc.)</returns>
        public Rectangle GetClientArea()
        {
            Control c;
            Rectangle clientArea;
            getDesireClientArea(out clientArea);

            for (int i = 0; i < _children.Count; i++)
            {
                c = _children[i];
                if (c.Visible)
                {
                    switch (c.DockStyle)
                    {
                        case DockStyle.None:
                            break;
                        case DockStyle.Top:
                        {
                            float height = Mathf.Min(c.Height, clientArea.Height);
                            clientArea.Location.Y += height;
                            clientArea.Size.Y -= height;
                            break;
                        }
                        case DockStyle.Bottom:
                        {
                            float height = Mathf.Min(c.Height, clientArea.Height);
                            clientArea.Size.Y -= height;
                            break;
                        }
                        case DockStyle.Fill:
                        {
                            getDesireClientArea(out clientArea);
                            break;
                        }
                        case DockStyle.Left:
                        {
                            float width = Mathf.Min(c.Width, clientArea.Width);
                            clientArea.Location.X += width;
                            clientArea.Size.X -= width;
                            break;
                        }
                        case DockStyle.Right:
                        {
                            float width = Mathf.Min(c.Width, clientArea.Width);
                            clientArea.Size.X -= width;
                            break;
                        }
                    }
                }
            }

            return clientArea;
        }

        /// <summary>
        /// Sort child controls list
        /// </summary>
        public void SortChildren()
        {
            _children.Sort();
            PerformLayout();
        }

        /// <summary>
        /// Sort children using recursion
        /// </summary>
        public void SortChildrenRecursive()
        {
            SortChildren();

            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i] as ContainerControl;
                if (c != null)
                    c.SortChildrenRecursive();
            }
        }

        #region Internal Events

        /// <summary>
        /// Add child control to the container
        /// Note: no friend class support in C# so this must be public function, Control class will use it.
        /// </summary>
        /// <param name="c">Control to add</param>
        public virtual void addChild(Control c)
        {
            Debug.Assert(c != null, "Invalid control.");

            // Add child
            _children.Add(c);

            // Arragne child controls
            PerformLayout();
        }

        /// <summary>
        /// Remove child control from this container
        /// Note: no friend class support in C# so this must be public function, Control class will use it.
        /// </summary>
        /// <param name="c">Control to remove</param>
        public virtual void removeChild(Control c)
        {
            Debug.Assert(c != null, "Invalid control.");

            // Remove child
            _children.Remove(c);

            // Check if control isn't durig disposing state
            if (!IsDisposing)
            {
                // Arragne child controls
                PerformLayout();
            }
        }

        /// <summary>
        /// When child control gets resized
        /// </summary>
        /// <param name="c">Child that has been resized</param>
        protected virtual void onChildResized(Control c)
        {
        }

        /// <summary>
        /// Check if mouse is over given control
        /// </summary>
        /// <param name="c">Control to check</param>
        /// <param name="location">Mouse location</param>
        /// <returns>True if mouse is over the control</returns>
        protected virtual bool isMouseOver(Control c, ref Vector2 location)
        {
            return c.ContainsPoint(ref location);
        }

        /// <summary>
        /// Get desire cleint area rectangle for all controls
        /// </summary>
        /// <param name="rect">Rectangle for controls</param>
        protected virtual void getDesireClientArea(out Rectangle rect)
        {
            rect = new Rectangle(0, 0, _width, _height);
        }

        /// <summary>
        /// Update contain focus state and all it's children
        /// </summary>
        protected void updateContainsFocus()
        {
            // Get current state and update all children
            bool result = base.ContainsFocus;

            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i] as ContainerControl;
                if (c != null)
                    c.updateContainsFocus();
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
                    OnStartContainsFocus();
                else
                    OnEndContainsFocus();
            }
        }

        /// <summary>
        /// Arrange docked controls and return final client area for other controls
        /// </summary>
        /// <param name="clientArea">Result client area</param>
        protected void arrangeDockedControls(ref Rectangle clientArea)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible)
                {
                    switch (c.DockStyle)
                    {
                        case DockStyle.None:
                            break;

                        case DockStyle.Bottom:
                        {
                            float height = c.Height;
                            float width = clientArea.Width;
                            c.SetSize(width, height);
                            c.SetLocation(clientArea.Left, clientArea.Bottom - height);
                            clientArea.Size.Y -= height;
                            break;
                        }
                        case DockStyle.Fill:
                        {
                            c.Size = clientArea.Size;
                            c.Location = clientArea.Location;
                            getDesireClientArea(out clientArea);
                            break;
                        }
                        case DockStyle.Left:
                        {
                            float width = c.Width;
                            float height = clientArea.Height;
                            c.SetSize(width, height);
                            c.SetLocation(clientArea.Left, clientArea.Top);
                            clientArea.Location.X += width;
                            clientArea.Size.X -= width;
                            break;
                        }
                        case DockStyle.Right:
                        {
                            float width = c.Width;
                            float height = clientArea.Height;
                            c.SetSize(width, height);
                            c.SetLocation(clientArea.Right - width, clientArea.Top);
                            clientArea.Size.X -= width;
                            break;
                        }
                        case DockStyle.Top:
                        {
                            float height = c.Height;
                            float width = clientArea.Width;
                            c.SetSize(width, height);
                            c.SetLocation(clientArea.Left, clientArea.Top);
                            clientArea.Location.Y += height;
                            clientArea.Size.Y -= height;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Use docked controls to calculate return final client area for other controls
        /// </summary>
        /// <param name="clientArea">Result client area</param>
        protected void calculateDockedControlsClientRect(out Rectangle clientArea)
        {
            getDesireClientArea(out clientArea);

            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible)
                {
                    switch (c.DockStyle)
                    {
                        case DockStyle.None:
                            break;
                        case DockStyle.Top:
                        {
                            float height = c.Height;
                            clientArea.Location.Y += height;
                            clientArea.Size.Y -= height;
                            break;
                        }
                        case DockStyle.Bottom:
                        {
                            clientArea.Size.Y -= c.Height;
                            break;
                        }
                        case DockStyle.Fill:
                        {
                            getDesireClientArea(out clientArea);
                            break;
                        }
                        case DockStyle.Left:
                        {
                            float width = c.Width;
                            clientArea.Location.X += width;
                            clientArea.Size.X -= width;
                            break;
                        }
                        case DockStyle.Right:
                        {
                            clientArea.Size.X -= c.Width;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Perform layout for that container control
        /// </summary>
        protected virtual void performLayoutSelf()
        {
            // By default we arrange only docked controls
            Rectangle clientArea;
            getDesireClientArea(out clientArea);
            arrangeDockedControls(ref clientArea);
        }

        #endregion

        #region Control

        /// <inheritdoc />
        public override void Dispose()
        {
            if (!IsDisposing)
            {
                // Steal focus from children
                if (ContainsFocus)
                    Focus();

                // Base
                base.Dispose();
            }
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Pass event futher
            for (int i = 0; i < _children.Count; i++)
                _children[i].OnDestroy();
        }

        /// <inheritdoc />
        public override bool IsMouseOver
        {
            get
            {
                if (base.IsMouseOver)
                    return true;

                for (int i = 0; i < _children.Count; i++)
                {
                    if (_children[i].IsMouseOver)
                        return true;
                }

                return false;
            }
        }

        /// <inheritdoc />
        public override void Update(float dt)
        {
            // Base
            base.Update(dt);

            // Update all enabled child controls
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Enabled)
                    c.Update(dt);
            }
        }

        /// <inheritdoc />
        public override void Draw(ref Vector2 root)
        {
            // TODO: finish render2D interop

            /*// Push clipping mask
            Rectangle clientArea;
            getDesireClientArea(out clientArea);
            Render2D.PushClip(clientArea);

            // Draw all visible child controls
            for (int i = 0; i < _children.Count; i++)
            {
                Control c = _children[i];
                if (c.Visible)
                {
                    Vector2 cRoot = root + c.Location;
                    if (c.IsScrollable)
                        cRoot += _viewOffset;

                    Render2D.SetTransform(cRoot);
                    c.Draw(render, cRoot);
                }
            }

            // Pop clipping mask
            Render2D.PopClip();

            // Restore render transform
            Render2D.SetTransform(ref root);*/

            // Base
            base.Draw(ref root);
        }

        /// <inheritdoc />
        public override void PerformLayout()
        {
            // Check if update is locked
            if (IsLayoutLocked)
                return;

            IsLayoutLocked = true;

            // Update itself
            performLayoutSelf();

            // Update children
            for (int i = 0; i < _children.Count; i++)
                _children[i].PerformLayout();

            IsLayoutLocked = false;
        }

        /// <inheritdoc />
        public override bool ContainsFocus
        {
            get { return _containsFocus; }
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            // Check all children collsiions with mouse and fire events for them
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Visible && c.Enabled)
                {
                    var cLocation = location;
                    if (c.IsScrollable)
                        cLocation -= _viewOffset;

                    // Check collision
                    bool isOver = isMouseOver(c, ref cLocation);

                    // Fire event
                    if (isOver)
                    {
                        // Enter
                        c.OnMouseEnter(cLocation - c.Location);
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
                var c = _children[i];
                if (c.Visible && c.Enabled)
                {
                    var cLocation = location;
                    if (c.IsScrollable)
                        cLocation -= _viewOffset;

                    // Check collision
                    bool isOver = isMouseOver(c, ref cLocation);

                    // Fire events
                    if (isOver || c.HasMouseCapture)
                    {
                        if (c.IsMouseOver)
                        {
                            // Move
                            c.OnMouseMove(cLocation - c.Location);
                        }
                        else
                        {
                            // Enter
                            c.OnMouseEnter(cLocation - c.Location);
                        }
                    }
                    else if (c.IsMouseOver)
                    {
                        // Leave
                        c.OnMouseLeave();
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
                var c = _children[i];
                if (c.Visible && c.Enabled && c.IsMouseOver)
                {
                    // Leave
                    c.OnMouseLeave();
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
                var c = _children[i];
                if (c.Visible && c.Enabled)
                {
                    var cLocation = location;
                    if (c.IsScrollable)
                        cLocation -= _viewOffset;

                    // Check collision
                    bool isOver = isMouseOver(c, ref cLocation);

                    // Fire events
                    if (isOver)
                    {
                        // Wheel
                        if (c.OnMouseWheel(cLocation - c.Location, delta))
                            return true;
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
                var c = _children[i];
                if (c.Visible && c.Enabled)
                {
                    var cLocation = location;
                    if (c.IsScrollable)
                        cLocation -= _viewOffset;

                    // Check collision
                    bool isOver = isMouseOver(c, ref cLocation);

                    // Fire event
                    if (isOver)
                    {
                        // Send event futher
                        if (c.OnMouseDown(buttons, cLocation - c.Location))
                            return true;
                    }
                    else if (c.HasMouseCapture)
                    {
                        // Cancel forced user focus
                        c.OnLostMouseCapture();
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
                var c = _children[i];
                if (c.Visible && c.Enabled)
                {
                    var cLocation = location;
                    if (c.IsScrollable)
                        cLocation -= _viewOffset;

                    // Fire event
                    if (c.HasMouseCapture)
                    {
                        // Send event futher
                        if (c.OnMouseUp(buttons, cLocation - c.Location))
                            return true;
                    }
                }
            }
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Visible && c.Enabled)
                {
                    var cLocation = location;
                    if (c.IsScrollable)
                        cLocation -= _viewOffset;

                    // Check collision
                    bool isOver = isMouseOver(c, ref cLocation);

                    // Fire event
                    if (isOver)
                    {
                        // Send event futher
                        if (c.OnMouseUp(buttons, cLocation - c.Location))
                            return true;
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
                var c = _children[i];
                if (c.Visible && c.Enabled)
                {
                    var cLocation = location;
                    if (c.IsScrollable)
                        cLocation -= _viewOffset;

                    // Check collision
                    bool isOver = isMouseOver(c, ref cLocation);

                    // Fire event
                    if (isOver)
                    {
                        // Send event futher
                        if (c.OnMouseDoubleClick(buttons, cLocation - c.Location))
                            return true;
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
                var c = _children[i];
                if (c.Enabled && (c.ContainsFocus || c.HasMouseCapture))
                {
                    return c.OnKeyDown(key);
                }
            }
            return false;
        }

        /// <inheritdoc />
        public override void OnKeyUp(KeyCode key)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                var c = _children[i];
                if (c.Enabled && (c.ContainsFocus || c.HasMouseCapture))
                {
                    c.OnKeyUp(key);
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
        protected override void setSize(float width, float height)
        {
            // Lock updates to revent form additional layout calculations
            bool wasLocked = IsLayoutLocked;
            IsLayoutLocked = true;

            // Cache previous size
            Vector2 prevSize = Size;

            // Base
            base.setSize(width, height);

            // Fire event
            for (int i = 0; i < _children.Count; i++)
                _children[i].OnParentResized(ref prevSize);

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
