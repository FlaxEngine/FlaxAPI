////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2012-2017 Flax Engine. All rights reserved.
////////////////////////////////////////////////////////////////////////////////////

using FlaxEngine;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// Gets the node under the mouse location.
        /// </summary>
        /// <returns>The node or null if no intersection.</returns>
        private SurfaceNode GetNodeUnderMouse()
        {
            var pos = _surface.PointFromParent(_mousePos);
            if (_surface.GetChildAt(pos) is SurfaceNode node)
                return node;
            return null;
        }

        private void UpdateSelectionRectangle()
        {
            var p1 = _surface.PointFromParent(_leftMouseDownPos);
            var p2 = _surface.PointFromParent(_mousePos);
            var selectionRect = Rectangle.FromPoints(p1, p2);

            // Find nodes to select
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].IsSelected = _nodes[i].Bounds.Intersects(ref selectionRect);
            }
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            _lastBoxUnderMouse = null;

            // Cache mouse location
            _mousePos = location;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _lastBoxUnderMouse = null;

            // Cache mouse location
            _mousePos = location;

            // Moving around surface with mouse
            if (_rightMouseDown)
            {
                // Calculate delta
                Vector2 delta = location - _rightMouseDownPos;
                if (delta.LengthSquared > 0.01f)
                {
                    // Move view
                    _mouseMoveAmount += delta.Length;
                    _surface.Location += delta;
                    _rightMouseDownPos = location;
                    Cursor = CursorType.SizeAll;
                }

                // Handled
                return;
            }

            // Check if user is selecting or moving node(s)
            if (_leftMouseDown)
            {
                if (_startBox != null)// Connecting
                {
                }
                else if (_isMovingSelection)// Moving
                {
                    // Calculate delta
                    Vector2 delta = location - _leftMouseDownPos;
                    if (delta.LengthSquared > 0.01f)
                    {
                        // Move selected nodes
                        delta /= _targeScale;
                        for (int i = 0; i < _nodes.Count; i++)
                        {
                            if (_nodes[i].IsSelected)
                            {
                                _nodes[i].Location += delta;
                            }
                        }
                        _leftMouseDownPos = location;
                        Cursor = CursorType.SizeAll;
                        MarkAsEdited(false);
                    }

                    // Handled
                    return;
                }
                else// Selecting
                {
                    UpdateSelectionRectangle();

                    // Handled
                    return;
                }
            }

            base.OnMouseMove(location);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear flags
            if (_leftMouseDown)
            {
                _leftMouseDown = false;
            }
            if (_startBox != null)
            {
                ConnectingEnd(null);
            }
            if (_rightMouseDown)
            {
                _rightMouseDown = false;
                Cursor = CursorType.Default;
            }

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override bool HasMouseCapture => _leftMouseDown || _rightMouseDown || base.HasMouseCapture;

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, int delta)
        {
            if (base.OnMouseWheel(location, delta))
                return true;

            // Change scale (disable scalig during selecting nodes)
            if (IsMouseOver && !_leftMouseDown)
            {
                var viewCenter = ViewCenterPosition;
                ViewScale += delta * 0.0008f;
                ViewCenterPosition = viewCenter;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButtons buttons)
        {
            // Check if user is connecting boxes
            if (_startBox != null)
                return true;

            // Cache data
            _isMovingSelection = false;
            _mousePos = location;
            if (buttons == MouseButtons.Left)
            {
                _leftMouseDown = true;
                _leftMouseDownPos = location;
            }
            if (buttons == MouseButtons.Right)
            {
                _rightMouseDown = true;
                _rightMouseDownPos = location;
            }

            // Check if any node is under the mouse
            SurfaceNode nodeAtMouse = GetNodeUnderMouse();
            Vector2 cLocation = _surface.PointFromParent(location);
            if (nodeAtMouse != null)
            {
                // Check if mouse is over header and user is pressing mouse left button
                if (_leftMouseDown && nodeAtMouse.HitsHeader(ref cLocation))
                {
                    // Check if user is pressing control
                    if (ParentWindow.GetKey(KeyCode.Control))
                    {
                        // Add to selection
                        AddToSelection(nodeAtMouse);
                    }
                    // Check if node isn't selected
                    else if (!nodeAtMouse.IsSelected)
                    {
                        // Select node
                        Select(nodeAtMouse);
                    }

                    // Start moving selected nodes
                    ParentWindow.StartTrackingMouse(false);
                    _isMovingSelection = true;
                    Focus();
                    return true;
                }
            }
            else
            {
                // Cache flags and state
                if (_leftMouseDown)
                {
                    // Start selecting
                    ParentWindow.StartTrackingMouse(false);
                    ClearSelection();
                    Focus();
                    return true;
                }
                if (_rightMouseDown)
                {
                    // Start navigating
                    ParentWindow.StartTrackingMouse(false);
                    Focus();
                    return true;
                }
            }

            // Base
            if (base.OnMouseDown(location, buttons))
            {
                // Clear flags to disable handling mouse events by itself (children should do)
                _leftMouseDown = _rightMouseDown = false;
                return true;
            }

            Focus();
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButtons buttons)
        {
            // Cache mouse location
            _mousePos = location;

            // Check if any node is under the mouse
            SurfaceNode nodeAtMouse = GetNodeUnderMouse();
            if (nodeAtMouse == null)
            {
                // Check if no move has been made at all
                if (_mouseMoveAmount < 5.0f)
                {
                    ClearSelection();
                }
            }

            // Cache flags and state
            if (_leftMouseDown && buttons == MouseButtons.Left)
            {
                _leftMouseDown = false;
                ParentWindow.EndTrackingMouse();
                Cursor = CursorType.Default;

                if (!_isMovingSelection && _startBox == null)
                {
                    UpdateSelectionRectangle();
                }
            }
            if (_rightMouseDown && buttons == MouseButtons.Right)
            {
                _rightMouseDown = false;
                ParentWindow.EndTrackingMouse();
                Cursor = CursorType.Default;

                // Check if no move has been made at all
                if (_mouseMoveAmount < 3.0f)
                {
                    // Check if any node is under the mouse
                    _cmStartPos = location;
                    if (nodeAtMouse != null)
                    {
                        // Show secondary context menu
                        ShowSecondaryCM(nodeAtMouse, _cmStartPos);
                    }
                    else
                    {
                        // Show primary context menu
                        ShowPrimaryMenu(_cmStartPos);
                    }
                }
                _mouseMoveAmount = 0;
            }

            // Base
            if (base.OnMouseUp(location, buttons))
                return true;

            if (buttons == MouseButtons.Left)
            {
                ConnectingEnd(null);
            }

            return true;
        }

        /// <inheritdoc />
        public override bool OnKeyDown(KeyCode key)
        {
            if (base.OnKeyDown(key))
                return true;

            if (key == KeyCode.Delete)
            {
                DeleteSelection();
                return true;
            }
            if (ParentWindow.GetKey(KeyCode.Control))
            {
                switch (key)
                {
                    case KeyCode.A:
                        SelectAll();
                        return true;
                }
            }

            return false;
        }
    }
}
