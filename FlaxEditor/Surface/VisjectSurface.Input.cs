// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System.Linq;
using FlaxEditor.Surface.Elements;
using FlaxEngine;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// The create comment key shortcut.
        /// </summary>
        public Keys CreateCommentKey = Keys.C;

        private string _currentInputText = string.Empty;

        private string CurrentInputText
        {
            get => _currentInputText;
            set
            {
                _currentInputText = value;
                CurrentInputTextChanged(_currentInputText);
            }
        }

        /// <summary>
        /// Occurs when handling custom mouse button down event.
        /// </summary>
        public event Window.MouseButtonDelegate CustomMouseDown;

        /// <summary>
        /// Occurs when handling custom mouse button up event.
        /// </summary>
        public event Window.MouseButtonDelegate CustomMouseUp;

        /// <summary>
        /// Occurs when handling custom mouse double click event.
        /// </summary>
        public event Window.MouseButtonDelegate CustomMouseDoubleClick;

        /// <summary>
        /// Occurs when handling custom mouse move event.
        /// </summary>
        public event Window.MouseMoveDelegate CustomMouseMove;

        /// <summary>
        /// Occurs when handling custom mouse wheel event.
        /// </summary>
        public event Window.MouseWheelDelegate CustomMouseWheel;

        /// <summary>
        /// Gets the node under the mouse location.
        /// </summary>
        /// <returns>The node or null if no intersection.</returns>
        public SurfaceNode GetNodeUnderMouse()
        {
            var pos = _rootControl.PointFromParent(ref _mousePos);
            if (_rootControl.GetChildAt(pos) is SurfaceNode node)
                return node;
            return null;
        }

        /// <summary>
        /// Gets the control under the mouse location.
        /// </summary>
        /// <returns>The control or null if no intersection.</returns>
        public SurfaceControl GetControlUnderMouse()
        {
            var pos = _rootControl.PointFromParent(ref _mousePos);
            if (_rootControl.GetChildAt(pos) is SurfaceControl control)
                return control;
            return null;
        }

        private void UpdateSelectionRectangle()
        {
            var p1 = _rootControl.PointFromParent(ref _leftMouseDownPos);
            var p2 = _rootControl.PointFromParent(ref _mousePos);
            var selectionRect = Rectangle.FromPoints(p1, p2);

            // Find controls to select
            for (int i = 0; i < _rootControl.Children.Count; i++)
            {
                if (_rootControl.Children[i] is SurfaceControl control)
                {
                    control.IsSelected = control.IsSelectionIntersecting(ref selectionRect);
                }
            }
        }

        /// <inheritdoc />
        public override void OnMouseEnter(Vector2 location)
        {
            _lastInstigatorUnderMouse = null;

            // Cache mouse location
            _mousePos = location;

            base.OnMouseEnter(location);
        }

        /// <inheritdoc />
        public override void OnMouseMove(Vector2 location)
        {
            _lastInstigatorUnderMouse = null;

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
                    _rootControl.Location += delta;
                    _rightMouseDownPos = location;
                    Cursor = CursorType.SizeAll;
                }

                // Handled
                return;
            }

            // Check if user is selecting or moving node(s)
            if (_leftMouseDown)
            {
                // Connecting
                if (_connectionInstigator != null)
                {
                }
                // Moving
                else if (_isMovingSelection)
                {
                    // Calculate delta (apply view offset)
                    Vector2 viewDelta = _rootControl.Location - _movingSelectionViewPos;
                    _movingSelectionViewPos = _rootControl.Location;
                    Vector2 delta = location - _leftMouseDownPos - viewDelta;
                    if (delta.LengthSquared > 0.01f)
                    {
                        // Move selected surface control
                        delta /= _targetScale;
                        for (int i = 0; i < _rootControl.Children.Count; i++)
                        {
                            if (_rootControl.Children[i] is SurfaceControl control && control.IsSelected)
                            {
                                control.Location += delta;
                            }
                        }
                        _leftMouseDownPos = location;
                        Cursor = CursorType.SizeAll;
                        MarkAsEdited(false);
                    }

                    // Handled
                    return;
                }
                // Commenting
                else if (_isCommentCreateKeyDown)
                {
                    // Handled
                    return;
                }
                // Selecting
                else
                {
                    UpdateSelectionRectangle();

                    // Handled
                    return;
                }
            }

            base.OnMouseMove(location);
            CustomMouseMove?.Invoke(ref location);
        }

        /// <inheritdoc />
        public override void OnLostFocus()
        {
            // Clear flags and state
            if (_leftMouseDown)
            {
                _leftMouseDown = false;
            }
            if (_rightMouseDown)
            {
                _rightMouseDown = false;
                Cursor = CursorType.Default;
            }
            _isMovingSelection = false;
            ConnectingEnd(null);

            base.OnLostFocus();
        }

        /// <inheritdoc />
        public override bool OnMouseWheel(Vector2 location, float delta)
        {
            // Base
            bool handled = base.OnMouseWheel(location, delta);
            if (!handled)
                CustomMouseWheel?.Invoke(ref location, delta, ref handled);
            if (handled)
            {
                return true;
            }

            // Change scale (disable scaling during selecting nodes)
            if (IsMouseOver && !_leftMouseDown)
            {
                var viewCenter = ViewCenterPosition;
                ViewScale += delta * 0.1f;
                ViewCenterPosition = viewCenter;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OnMouseDoubleClick(Vector2 location, MouseButton buttons)
        {
            // Base
            bool handled = base.OnMouseDoubleClick(location, buttons);
            if (!handled)
                CustomMouseDoubleClick?.Invoke(ref location, buttons, ref handled);
            if (handled)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool OnMouseDown(Vector2 location, MouseButton buttons)
        {
            _wasMouseDownSinceCommentCreatingStart = true;

            // Check if user is connecting boxes
            if (_connectionInstigator != null)
                return true;

            // Cache data
            _isMovingSelection = false;
            _mousePos = location;
            if (buttons == MouseButton.Left)
            {
                _leftMouseDown = true;
                _leftMouseDownPos = location;
            }
            if (buttons == MouseButton.Right)
            {
                _rightMouseDown = true;
                _rightMouseDownPos = location;
            }

            // Check if any node is under the mouse
            SurfaceControl controlUnderMouse = GetControlUnderMouse();
            Vector2 cLocation = _rootControl.PointFromParent(ref location);
            if (controlUnderMouse != null)
            {
                // Check if mouse is over header and user is pressing mouse left button
                if (_leftMouseDown && controlUnderMouse.CanSelect(ref cLocation))
                {
                    // Check if user is pressing control
                    if (Root.GetKey(Keys.Control))
                    {
                        // Add to selection
                        AddToSelection(controlUnderMouse);
                    }
                    // Check if node isn't selected
                    else if (!controlUnderMouse.IsSelected)
                    {
                        // Select node
                        Select(controlUnderMouse);
                    }

                    // Start moving selected nodes
                    StartMouseCapture();
                    _isMovingSelection = true;
                    _movingSelectionViewPos = _rootControl.Location;
                    Focus();
                    return true;
                }
            }
            else
            {
                // Cache flags and state
                if (_leftMouseDown)
                {
                    // Start selecting or commenting
                    StartMouseCapture();
                    ClearSelection();
                    Focus();
                    return true;
                }
                if (_rightMouseDown)
                {
                    // Start navigating
                    StartMouseCapture();
                    Focus();
                    return true;
                }
            }

            // Base
            bool handled = base.OnMouseDown(location, buttons);
            if (!handled)
                CustomMouseDown?.Invoke(ref location, buttons, ref handled);
            if (handled)
            {
                // Clear flags to disable handling mouse events by itself (children should do)
                _leftMouseDown = _rightMouseDown = false;
                return true;
            }

            Focus();
            return true;
        }

        /// <inheritdoc />
        public override bool OnMouseUp(Vector2 location, MouseButton buttons)
        {
            // Cache mouse location
            _mousePos = location;

            // Check if any control is under the mouse
            SurfaceControl controlUnderMouse = GetControlUnderMouse();

            // Cache flags and state
            if (_leftMouseDown && buttons == MouseButton.Left)
            {
                _leftMouseDown = false;
                EndMouseCapture();
                Cursor = CursorType.Default;

                // Commenting
                if (_isCommentCreateKeyDown)
                {
                    var p1 = _rootControl.PointFromParent(ref _leftMouseDownPos);
                    var p2 = _rootControl.PointFromParent(ref _mousePos);
                    var selectionRect = Rectangle.FromPoints(p1, p2);
                    Context.CreateComment(ref selectionRect);
                }
                // Selecting
                else if (!_isMovingSelection && _connectionInstigator == null)
                {
                    UpdateSelectionRectangle();
                }
            }
            if (_rightMouseDown && buttons == MouseButton.Right)
            {
                _rightMouseDown = false;
                EndMouseCapture();
                Cursor = CursorType.Default;

                // Check if no move has been made at all
                if (_mouseMoveAmount < 3.0f)
                {
                    // Check if any control is under the mouse
                    _cmStartPos = location;
                    if (controlUnderMouse != null)
                    {
                        if (!HasNodesSelection)
                            Select(controlUnderMouse);

                        // Show secondary context menu
                        ShowSecondaryCM(_cmStartPos);
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
            bool handled = base.OnMouseUp(location, buttons);
            if (!handled)
                CustomMouseUp?.Invoke(ref location, buttons, ref handled);
            if (handled)
            {
                return true;
            }

            // Right clicking while attempting to connect a node to something
            if (!_rightMouseDown && !_isMovingSelection && _connectionInstigator != null)
            {
                _cmStartPos = location;
                ShowPrimaryMenu(_cmStartPos);
                EndMouseCapture();
            }

            return true;
        }

        /// <inheritdoc />
        public override bool OnCharInput(char c)
        {
            if (base.OnCharInput(c))
                return true;

            if (HasInputSelection)
            {
                if (_hasInputSelectionChanged)
                {
                    ResetInputSelection();
                }

                CurrentInputText += c;

                return true;
            }

            return false;
        }

        private bool HasInputSelection => HasNodesSelection;

        private bool _hasInputSelectionChanged = false;

        private void ResetInputSelection()
        {
            CurrentInputText = "";
            _hasInputSelectionChanged = false;
        }

        private void CurrentInputTextChanged(string currentInputText)
        {
            var selection = SelectedNodes;
            if (selection.Count != 1)
                return;
            if (string.IsNullOrEmpty(currentInputText))
                return;
            if (currentInputText.Length == 1 && char.ToLower(currentInputText[0]) == char.ToLower((char)CreateCommentKey))
                return;
            if (_activeVisjectCM.Visible)
                return;

            // # => color
            // 1,43 => Vector2
            // Current node should be modify-able

            // Multiple nodes selected?

            var node = selection[0];
            var firstOutputBox = node.GetBoxes().DefaultIfEmpty(null).FirstOrDefault(box => box.IsOutput);
            if (firstOutputBox == null)
                return;

            _cmStartPos = _rootControl.PointToParent(_rootControl.Parent, PositionAfterNode(node));
            _cmStartPos = Vector2.Max(_cmStartPos, Vector2.Zero);

            // If the menu is not fully visible, move the surface a bit
            Vector2 overflow = (_cmStartPos + _activeVisjectCM.Size);
            if (_connectionInstigator is SurfaceNode instigatorAsSurfaceNode)
                overflow -= instigatorAsSurfaceNode.Size;
            else if (_connectionInstigator is Box instigatorAsBox)
                overflow -= instigatorAsBox.Parent.Size;
            overflow = Vector2.Max(overflow, Vector2.Zero);

            ViewPosition += overflow;
            _cmStartPos -= overflow;

            // Show it
            _connectionInstigator = firstOutputBox;
            ShowPrimaryMenu(_cmStartPos);

            foreach (char character in currentInputText)
            {
                // OnKeyDown-- > VisjectCM focuses on the text-thingy
                _activeVisjectCM.OnKeyDown(Keys.None);
                _activeVisjectCM.OnCharInput(character);
                _activeVisjectCM.OnKeyUp(Keys.None);
            }
            ResetInputSelection();
        }

        private Vector2 PositionAfterNode(SurfaceNode node)
        {
            const float DistanceBetweenNodes = 40;
            //TODO: Doge the other nodes
            return node.Location + new Vector2(node.Width + DistanceBetweenNodes, 0);
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (base.OnKeyDown(key))
                return true;

            if (key == Keys.Delete)
            {
                Delete();
                return true;
            }

            if (Root.GetKey(Keys.Control))
            {
                switch (key)
                {
                case Keys.A:
                    SelectAll();
                    return true;

                case Keys.C:
                    Copy();
                    return true;

                case Keys.V:
                    Paste();
                    return true;

                case Keys.X:
                    Cut();
                    return true;

                case Keys.D:
                    Duplicate();
                    return true;
                }
            }

            if (key == CreateCommentKey)
            {
                _isCommentCreateKeyDown = true;
                _wasMouseDownSinceCommentCreatingStart = false;
            }

            if (HasInputSelection)
            {
                if (_hasInputSelectionChanged)
                {
                    ResetInputSelection();
                }

                if (key == Keys.Backspace)
                {
                    if (CurrentInputText.Length > 0)
                        CurrentInputText = CurrentInputText.Substring(0, CurrentInputText.Length - 1);
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public override void OnKeyUp(Keys key)
        {
            base.OnKeyUp(key);

            if (key == CreateCommentKey && !RootWindow.GetKey(Keys.Control))
            {
                if (!_wasMouseDownSinceCommentCreatingStart)
                    CommentSelection();

                _wasMouseDownSinceCommentCreatingStart = false;
                _isCommentCreateKeyDown = false;
            }
        }
    }
}
