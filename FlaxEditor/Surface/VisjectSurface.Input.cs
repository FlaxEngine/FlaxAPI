// Copyright (c) 2012-2019 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Options;
using FlaxEditor.Surface.Elements;
using FlaxEditor.Surface.Undo;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    public partial class VisjectSurface
    {
        /// <summary>
        /// The input actions collection to processed during user input.
        /// </summary>
        public readonly InputActionsContainer InputActions;

        private string _currentInputText = string.Empty;
        private Vector2 _movingNodesDelta;
        private HashSet<SurfaceNode> _movingNodes;
        private readonly Stack<InputBracket> _inputBrackets = new Stack<InputBracket>();

        private class InputBracket
        {
            private readonly Margin _padding = new Margin(10f);
            public Box Box { get; }
            public Vector2 EndBracketPosition { get; }
            public List<SurfaceNode> Nodes { get; } = new List<SurfaceNode>();
            public Rectangle Area { get; private set; }

            public InputBracket(Box box, Vector2 nodePosition)
            {
                Box = box;
                EndBracketPosition = nodePosition;
                Update();
            }


            public void Update()
            {
                Rectangle area;
                if (Nodes.Count > 0)
                {
                    area = VisjectSurface.GetNodesBounds(Nodes);
                }
                else
                {
                    area = new Rectangle(EndBracketPosition, new Vector2(120f, 80f));
                }
                _padding.ExpandRectangle(ref area);
                Vector2 endPoint = area.Location + new Vector2(area.Width, area.Height / 2f);
                Vector2 offset = EndBracketPosition - endPoint;
                area.Location += offset;
                Area = area;
                if (!offset.IsZero)
                {
                    foreach (var node in Nodes)
                    {
                        node.Location += offset;
                    }
                }
            }
        }

        private bool HasInputSelection => HasNodesSelection;

        private string InputText
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
            if (_rootControl.GetChildAtRecursive(pos) is SurfaceControl control)
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

        private void OnSurfaceControlSpawned(SurfaceControl control)
        {
            if (_inputBrackets.Count > 0 && control is SurfaceNode node)
            {
                _inputBrackets.Peek().Nodes.Add(node);
                _inputBrackets.Peek().Update();
            }
        }

        private void OnSurfaceControlDeleted(SurfaceControl control)
        {
            if (_inputBrackets.Count > 0 && control is SurfaceNode node)
            {
                _inputBrackets.Peek().Nodes.Remove(node);
                _inputBrackets.Peek().Update();
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
                        // Move selected nodes
                        delta /= _targetScale;
                        foreach (var node in _movingNodes)
                        {
                            node.Location += delta;
                        }
                        _leftMouseDownPos = location;
                        _movingNodesDelta += delta;
                        Cursor = CursorType.SizeAll;
                        MarkAsEdited(false);
                    }

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
            if (IsMouseOver && !_leftMouseDown && !IsPrimaryMenuOpened)
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
            // Check if user is connecting boxes
            if (_connectionInstigator != null)
                return true;

            // Base
            bool handled = base.OnMouseDown(location, buttons);
            if (!handled)
                CustomMouseDown?.Invoke(ref location, buttons, ref handled);
            if (handled)
            {
                // Clear flags
                _isMovingSelection = false;
                _rightMouseDown = false;
                _leftMouseDown = false;
                return true;
            }

            // Just reset the input text whenever the user presses anywhere
            ResetInput();

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
                        if (!controlUnderMouse.IsSelected)
                        {
                            AddToSelection(controlUnderMouse);
                        }
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
                    _movingNodesDelta = Vector2.Zero;
                    if (_movingNodes == null)
                        _movingNodes = new HashSet<SurfaceNode>();
                    else
                        _movingNodes.Clear();
                    for (int i = 0; i < _rootControl.Children.Count; i++)
                    {
                        if (_rootControl.Children[i] is SurfaceNode node && node.IsSelected && (node.Archetype.Flags & NodeFlags.NoMove) != NodeFlags.NoMove)
                        {
                            _movingNodes.Add(node);

                            // Move nodes inside the comment
                            if (node is SurfaceComment comment)
                            {
                                var commentBounds = comment.Bounds;
                                for (int j = 0; j < _rootControl.Children.Count; j++)
                                {
                                    if (_rootControl.Children[j] is SurfaceNode childNode && commentBounds.Contains(childNode.Bounds))
                                    {
                                        _movingNodes.Add(childNode);
                                    }
                                }
                            }
                        }
                    }
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

                // Moving nodes
                if (_isMovingSelection)
                {
                    if (_movingNodes != null && _movingNodes.Count > 0)
                    {
                        if (Undo != null && !_movingNodesDelta.IsZero)
                            Undo.AddAction(new MoveNodesAction(Context, _movingNodes.Select(x => x.ID).ToArray(), _movingNodesDelta));
                        _movingNodes.Clear();
                    }
                    _movingNodesDelta = Vector2.Zero;
                }
                // Connecting
                else if (_connectionInstigator != null)
                {
                }
                // Selecting
                else
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
                // Clear flags
                _rightMouseDown = false;
                _leftMouseDown = false;
                return true;
            }

            // Right clicking while attempting to connect a node to something
            if (!_rightMouseDown && !_isMovingSelection && _connectionInstigator != null)
            {
                _cmStartPos = location;
                Cursor = CursorType.Default;
                EndMouseCapture();
                ShowPrimaryMenu(_cmStartPos);
            }

            return true;
        }

        /// <inheritdoc />
        public override bool OnCharInput(char c)
        {
            if (base.OnCharInput(c))
                return true;

            InputText += c;
            return true;
        }

        /// <inheritdoc />
        public override bool OnKeyDown(Keys key)
        {
            if (base.OnKeyDown(key))
                return true;

            if (InputActions.Process(Editor.Instance, this, key))
                return true;

            if (HasInputSelection)
            {
                if (key == Keys.Backspace)
                {
                    if (InputText.Length > 0)
                        InputText = InputText.Substring(0, InputText.Length - 1);
                    return true;
                }
                else if (key == Keys.Escape)
                {
                    ClearSelection();
                }
                else if (key == Keys.Return)
                {
                    Box selectedBox = GetSelectedBox(SelectedNodes);
                    if (selectedBox != null)
                    {
                        Box toSelect = selectedBox.ParentNode.GetNextBox(selectedBox);

                        if (toSelect != null)
                        {
                            Select(toSelect.ParentNode);
                            toSelect.ParentNode.SelectBox(toSelect);
                        }
                    }
                }
            }

            return false;
        }

        private void ResetInput()
        {
            InputText = "";
            _inputBrackets.Clear();
        }

        private void CurrentInputTextChanged(string currentInputText)
        {
            if (string.IsNullOrEmpty(currentInputText))
                return;
            if (IsPrimaryMenuOpened)
            {
                InputText = "";
                return;
            }

            var selection = SelectedNodes;
            if (selection.Count == 0)
            {
                if (_inputBrackets.Count == 0)
                {
                    ResetInput();
                    ShowPrimaryMenu(_mousePos, currentInputText);
                }
                else
                {
                    InputText = "";
                    ShowPrimaryMenu(_rootControl.PointToParent(_inputBrackets.Peek().Area.Location), currentInputText);
                }
                return;
            }

            // Multi-Node Editing
            const string Comment = "//";
            if (currentInputText.StartsWith(Comment))
            {
                InputText = "";
                var comment = CommentSelection(currentInputText.Substring(Comment.Length));
                comment.StartRenaming();
                return;
            }

            // TODO: What should happen when multiple nodes or multiple boxes are selected?
            if (selection.Count != 1)
                return;

            // Single Box Editing
            Box selectedBox = GetSelectedBox(selection);

            if (selectedBox == null)
                return;

            // TODO: Editing a primitive
            /* #    => color
             * 1,43 => Vector2
             * =    => set node name
             * etc.
             */
            if (currentInputText.StartsWith("("))
            {
                InputText = InputText.Substring(1);

                // Opening bracket
                if (!selectedBox.IsOutput)
                {
                    var bracket = new InputBracket(selectedBox, FindEmptySpace(selectedBox));
                    _inputBrackets.Push(bracket);
                    Deselect(selectedBox.ParentNode);
                }
            }
            else if (currentInputText.StartsWith(")"))
            {
                InputText = InputText.Substring(1);

                // Closing bracket
                if (_inputBrackets.Count > 0)
                {
                    var bracket = _inputBrackets.Pop();
                    bracket.Update();

                    if (selectedBox.IsOutput)
                    {
                        TryConnect(selectedBox, bracket.Box);
                    }
                }
            }
            else
            {
                InputText = "";

                // Add a new node
                ConnectingStart(selectedBox);
                Cursor = CursorType.Default; // Do I need this?
                EndMouseCapture();
                ShowPrimaryMenu(_rootControl.PointToParent(FindEmptySpace(selectedBox)), currentInputText);
            }
        }

        private Box GetSelectedBox(List<SurfaceNode> selection)
        {
            if (selection.Count != 1)
                return null; // TODO: Handle multiple selected nodes

            SurfaceNode selectedNode = selection[0];

            Box selectedBox = null;
            // Get selected box
            for (int i = 0; i < selectedNode.Elements.Count; i++)
            {
                if (selectedNode.Elements[i] is Box box && box.IsSelected)
                {
                    if (selectedBox == null)
                    {
                        selectedBox = box;
                    }
                    else
                    {
                        // TODO: Multiple boxes are selected. How should this be handled?
                        return null;
                    }
                }
            }

            // Or get the first output box when a node with only output boxes is selected
            if (selectedBox == null)
            {
                for (int i = 0; i < selectedNode.Elements.Count; i++)
                {
                    if (selectedNode.Elements[i] is Box box)
                    {
                        if (box.IsOutput)
                        {
                            selectedBox = box;
                            break;
                        }
                    }
                }

                for (int i = 0; i < selectedNode.Elements.Count; i++)
                {
                    if (selectedNode.Elements[i] is Box box)
                    {
                        if (!box.IsOutput)
                        {
                            selectedBox = null;
                            break;
                        }
                    }
                }
            }

            return selectedBox;
        }

        private Vector2 FindEmptySpace(Box box)
        {
            int boxIndex = 0;

            var node = box.ParentNode;
            for (int i = 0; i < node.Elements.Count; i++)
            {
                // Box on the same side above the current box
                if (node.Elements[i] is Box nodeBox && nodeBox.IsOutput == box.IsOutput && nodeBox.Y < box.Y)
                {
                    boxIndex++;
                }
            }
            // TODO: Dodge the other nodes

            Vector2 DistanceBetweenNodes = new Vector2(40, 20);
            const float NodeHeight = 120;

            float direction = box.IsOutput ? 1 : -1;

            Vector2 newNodeLocation = node.Location +
                                      new Vector2(
                                          (node.Width + DistanceBetweenNodes.X) * direction,
                                          boxIndex * (NodeHeight + DistanceBetweenNodes.Y)
                                      );

            return newNodeLocation;
        }
    }
}
