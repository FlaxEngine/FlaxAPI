// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using FlaxEditor.Surface.ContextMenu;
using FlaxEditor.Surface.Elements;
using FlaxEngine;
using FlaxEngine.Assertions;
using FlaxEngine.GUI;

namespace FlaxEditor.Surface
{
    /// <summary>
    /// Visject Surface control for editing Nodes Graph.
    /// </summary>
    /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
    /// <seealso cref="IParametersDependantNode" />
    public partial class VisjectSurface : ContainerControl, IParametersDependantNode
    {
        private class SurfaceControl : ContainerControl
        {
            /// <inheritdoc />
            public SurfaceControl()
            {
                CanFocus = false;
                ClipChildren = false;
                Pivot = Vector2.Zero;
            }

            /// <inheritdoc />
            public override bool IntersectsContent(ref Vector2 locationParent, out Vector2 location)
            {
                location = PointFromParent(locationParent);
                return true;
            }
        }

        private SurfaceControl _surface;

        private bool _edited;
        private float _targeScale = 1.0f;
        private readonly List<SurfaceNode> _nodes = new List<SurfaceNode>(64);
        private float _moveViewWithMouseDragSpeed = 1.0f;

        private bool _leftMouseDown;
        private bool _rightMouseDown;
        private Vector2 _leftMouseDownPos = Vector2.Minimum;
        private Vector2 _rightMouseDownPos = Vector2.Minimum;
        private Vector2 _mousePos = Vector2.Minimum;
        private float _mouseMoveAmount;

        private bool _isMovingSelection;
        private Vector2 _movingSelectionViewPos;
        private Box _startBox;
        private Box _lastBoxUnderMouse;

        private VisjectCM _cmPrimaryMenu;
        private FlaxEngine.GUI.ContextMenu _cmSecondaryMenu;
        private Vector2 _cmStartPos = Vector2.Minimum;

        /// <summary>
        /// The owner.
        /// </summary>
        public readonly IVisjectSurfaceOwner Owner;

        /// <summary>
        /// The surface type.
        /// </summary>
        public readonly SurfaceType Type;

        /// <summary>
        /// The style used by the surface.
        /// </summary>
        public readonly SurfaceStyle Style;

        /// <summary>
        /// Gets a value indicating whether surface is edited.
        /// </summary>
        public bool IsEdited => _edited;

        /// <summary>
        /// Gets or sets the view position (upper left corner of the view) in the surface space.
        /// </summary>
        public Vector2 ViewPosition
        {
            get => _surface.Location / -ViewScale;
            set => _surface.Location = value * -ViewScale;
        }

        /// <summary>
        /// Gets or sets the view center position (middle point of the view) in the surface space.
        /// </summary>
        public Vector2 ViewCenterPosition
        {
            get => (_surface.Location - Size * 0.5f) / -ViewScale;
            set => _surface.Location = Size * 0.5f + value * -ViewScale;
        }

        /// <summary>
        /// Gets or sets the view scale.
        /// </summary>
        public float ViewScale
        {
            get => _targeScale;
            set
            {
                // Clamp
                value = Mathf.Clamp(value, 0.05f, 1.6f);

                // Check if value will change
                if (Mathf.Abs(value - _targeScale) > 0.0001f)
                {
                    // Set new target scale
                    _targeScale = value;
                }

                // disable view scale animation
                _surface.Scale = new Vector2(_targeScale);
            }
        }

        /// <summary>
        /// Gets a value indicating whether user is selecting nodes.
        /// </summary>
        public bool IsSelecting => _leftMouseDown && !_isMovingSelection && _startBox == null;

        /// <summary>
        /// Gets a value indicating whether user is moving selected nodes.
        /// </summary>
        public bool IsMovignSelection => _leftMouseDown && _isMovingSelection && _startBox == null;

        /// <summary>
        /// Gets a value indicating whether user is connecting nodes.
        /// </summary>
        public bool IsConnecting => _startBox != null;

        /// <summary>
        /// Returns true if any node is selected by the user (one or more).
        /// </summary>
        public bool HasSelection
        {
            get
            {
                for (int i = 0; i < _nodes.Count; i++)
                {
                    if (_nodes[i].IsSelected)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the list of the selected nodes.
        /// </summary>
        public List<SurfaceNode> Selection
        {
            get
            {
                List<SurfaceNode> selection = new List<SurfaceNode>();
                for (int i = 0; i < _nodes.Count; i++)
                {
                    if (_nodes[i].IsSelected)
                        selection.Add(_nodes[i]);
                }
                return selection;
            }
        }

        /// <summary>
        /// The metadata.
        /// </summary>
        public readonly SurfaceMeta Meta = new SurfaceMeta();

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectSurface"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="type">The type.</param>
        public VisjectSurface(IVisjectSurfaceOwner owner, SurfaceType type)
        {
            DockStyle = DockStyle.Fill;

            Owner = owner;
            Type = type;
            Style = SurfaceStyle.CreateStyleHandler(Editor.Instance, Type);
            if (Style == null)
                throw new InvalidOperationException("Missing visject surface style.");

            // Surface control used to navigate around the view (scale and move it)
            _surface = new SurfaceControl();
            _surface.Parent = this;

            // Create primary menu (for nodes spawning)
            _cmPrimaryMenu = new VisjectCM(type, () => Parameters);
            _cmPrimaryMenu.OnItemClicked += OnPrimaryMenuButtonClick;

            // Create secondary menu (for other actions)
            _cmSecondaryMenu = new FlaxEngine.GUI.ContextMenu();
            _cmSecondaryMenu.AddButton("Save", Owner.OnSurfaceSave);
            _cmSecondaryMenu.AddSeparator();
            _cmCopyButton = _cmSecondaryMenu.AddButton("Copy", Copy);
            _cmPasteButton = _cmSecondaryMenu.AddButton("Paste", Paste);
            _cmDuplicateButton = _cmSecondaryMenu.AddButton("Duplicate", Duplicate);
            _cmCutButton = _cmSecondaryMenu.AddButton("Cut", Cut);
            _cmDeleteButton = _cmSecondaryMenu.AddButton("Delete", Delete);
            _cmSecondaryMenu.AddSeparator();
            _cmRemoveNodeConnectionsButton = _cmSecondaryMenu.AddButton("Remove all connections to that node(s)", () =>
            {
                var nodes = ((List<SurfaceNode>)_cmSecondaryMenu.Tag);
                foreach (var node in nodes)
                {
                    node.RemoveConnections();
                }
                MarkAsEdited();
            });
            _cmRemoveBoxConnectionsButton = _cmSecondaryMenu.AddButton("Remove all connections to that box", () =>
            {
                var boxUnderMouse = (Box)_cmRemoveBoxConnectionsButton.Tag;
                boxUnderMouse.RemoveConnections();
                MarkAsEdited();
            });

            // Set initial scale to provide nice zoom in effect on startup
            _surface.Scale = new Vector2(0.5f);

            _dragOverItems = new GUI.Drag.DragAssets(ValidateDragItemFunc);
        }

        /// <summary>
        /// Shows the whole graph by changing the view scale and the position.
        /// </summary>
        public void ShowWholeGraph()
        {
            if (_nodes.Count == 0)
                return;

            // Find surface bounds
            Rectangle area = _nodes[0].Bounds;
            for (int i = 1; i < _nodes.Count; i++)
                area = Rectangle.Union(area, _nodes[i].Bounds);

            ShowArea(area);
        }

        /// <summary>
        /// Shows the given surface area by changing the view scale and the position.
        /// </summary>
        /// <param name="areaRect">The area rectangle.</param>
        public void ShowArea(Rectangle areaRect)
        {
            ViewScale = (Size / areaRect.Size).MinValue * 0.95f;
            ViewCenterPosition = areaRect.Center;
        }

        /// <summary>
        /// Mark surface as edited.
        /// </summary>
        /// <param name="graphEdited">True if graph has been edited (nodes structure or parameter value).</param>
        public void MarkAsEdited(bool graphEdited = true)
        {
            if (!_edited)
            {
                _edited = true;
                Owner.OnSurfaceEditedChanged();
            }

            if (graphEdited)
            {
                Owner.OnSurfaceGraphEdited();
            }
        }

        /// <summary>
        /// Selects all the nodes.
        /// </summary>
        public void SelectAll()
        {
            for (int i = 0; i < _nodes.Count; i++)
                _nodes[i].IsSelected = true;
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ClearSelection()
        {
            for (int i = 0; i < _nodes.Count; i++)
                _nodes[i].IsSelected = false;
        }

        /// <summary>
        /// Adds the specified node to the selection.
        /// </summary>
        /// <param name="node">The node.</param>
        public void AddToSelection(SurfaceNode node)
        {
            node.IsSelected = true;
        }

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Select(SurfaceNode node)
        {
            ClearSelection();

            node.IsSelected = true;
        }

        /// <summary>
        /// Selects the specified nodes collection.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public void Select(IEnumerable<SurfaceNode> nodes)
        {
            ClearSelection();

            foreach (var node in nodes)
            {
                node.IsSelected = true;
            }
        }

        /// <summary>
        /// Deselects the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Deselect(SurfaceNode node)
        {
            node.IsSelected = false;
        }

        /// <summary>
        /// Deletes the specified collection of the nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public void Delete(IEnumerable<SurfaceNode> nodes)
        {
            foreach (var node in nodes)
            {
                Delete(node);
            }
        }

        /// <summary>
        /// Deletes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Delete(SurfaceNode node)
        {
            if ((node.Archetype.Flags & NodeFlags.NoRemove) == 0)
            {
                node.RemoveConnections();
                node.Dispose();

                _nodes.Remove(node);

                MarkAsEdited();
            }
        }

        /// <summary>
        /// Deletes the selected nodes.
        /// </summary>
        public void Delete()
        {
            bool edited = false;

            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];

                if (node.IsSelected && (node.Archetype.Flags & NodeFlags.NoRemove) == 0)
                {
                    node.RemoveConnections();
                    node.Dispose();

                    _nodes.RemoveAt(i);
                    i--;

                    edited = true;
                }
            }

            if (edited)
                MarkAsEdited();
        }

        /// <summary>
        /// Finds the node of the given type.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>Found node or null if cannot.</returns>
        public SurfaceNode FindNode(ushort groupId, ushort typeId)
        {
            SurfaceNode result = null;
            uint type = ((uint)groupId << 16) | typeId;
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                if (node.Type == type)
                {
                    result = node;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Finds the node with the given ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Found node or null if cannot.</returns>
        public SurfaceNode FindNode(uint id)
        {
            SurfaceNode result = null;
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                if (node.ID == id)
                {
                    result = node;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the parameter by the given ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Found parameter instance or null if missing.</returns>
        public SurfaceParameter GetParameter(Guid id)
        {
            SurfaceParameter result = null;
            for (int i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                if (parameter.ID == id)
                {
                    result = parameter;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the parameter by the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Found parameter instance or null if missing.</returns>
        public SurfaceParameter GetParameter(string name)
        {
            SurfaceParameter result = null;
            for (int i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                if (parameter.Name == name)
                {
                    result = parameter;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Spawns the node.
        /// </summary>
        /// <param name="groupID">The group archetype ID.</param>
        /// <param name="typeID">The node archetype ID.</param>
        /// <param name="location">The location.</param>
        /// <param name="customValues">The custom values array. Must match node archetype <see cref="NodeArchetype.DefaultValues"/> size. Pass null to use default values.</param>
        /// <returns>Created node.</returns>
        public SurfaceNode SpawnNode(ushort groupID, ushort typeID, Vector2 location, object[] customValues = null)
        {
            GroupArchetype groupArchetype;
            NodeArchetype nodeArchetype;
            if (NodeFactory.GetArchetype(groupID, typeID, out groupArchetype, out nodeArchetype))
            {
                return SpawnNode(groupArchetype, nodeArchetype, location, customValues);
            }

            return null;
        }

        private uint GetFreeNodeID()
        {
            uint result = 1;
            while (true)
            {
                bool valid = true;
                for (int i = 0; i < _nodes.Count; i++)
                {
                    if (_nodes[i].ID == result)
                    {
                        result++;
                        valid = false;
                        break;
                    }
                }
                if (valid)
                    break;
            }
            return result;
        }

        /// <summary>
        /// Spawns the node.
        /// </summary>
        /// <param name="groupArchetype">The group archetype.</param>
        /// <param name="nodeArchetype">The node archetype.</param>
        /// <param name="location">The location.</param>
        /// <param name="customValues">The custom values array. Must match node archetype <see cref="NodeArchetype.DefaultValues"/> size. Pass null to use default values.</param>
        /// <returns>Created node.</returns>
        public SurfaceNode SpawnNode(GroupArchetype groupArchetype, NodeArchetype nodeArchetype, Vector2 location, object[] customValues = null)
        {
            if (groupArchetype == null || nodeArchetype == null)
                throw new ArgumentNullException();
            Assert.IsTrue(groupArchetype.Archetypes.Contains(nodeArchetype));

            var id = GetFreeNodeID();

            // Create node
            var node = NodeFactory.CreateNode(id, this, groupArchetype, nodeArchetype);
            if (node == null)
            {
                Debug.LogError("Failed to create node.");
                return null;
            }
            _nodes.Add(node);

            // Intiialize
            if (customValues != null)
            {
                if (node.Values != null && node.Values.Length == customValues.Length)
                    Array.Copy(customValues, node.Values, customValues.Length);
                else
                    throw new InvalidOperationException("Invalid node custom values.");
            }
            OnNodeLoaded(node);
            node.OnSurfaceLoaded();
            node.Location = location;

            MarkAsEdited();

            return node;
        }

        /// <summary>
        /// Called when node gets loaded and should be added to the surface. Creates node elements from the archetype.
        /// </summary>
        /// <param name="node">The node.</param>
        private void OnNodeLoaded(SurfaceNode node)
        {
            // Create child elements of the node based on it's archetype
            for (int i = 0; i < node.Archetype.Elements.Length; i++)
            {
                var arch = node.Archetype.Elements[i];
                ISurfaceNodeElement element = null;
                switch (arch.Type)
                {
                case NodeElementType.Input:
                    element = new InputBox(node, arch);
                    break;
                case NodeElementType.Output:
                    element = new OutputBox(node, arch);
                    break;
                case NodeElementType.BoolValue:
                    element = new BoolValue(node, arch);
                    break;
                case NodeElementType.FloatValue:
                    element = new FloatValue(node, arch);
                    break;
                case NodeElementType.IntegerValue:
                    element = new IntegerValue(node, arch);
                    break;
                case NodeElementType.ColorValue:
                    element = new ColorValue(node, arch);
                    break;
                case NodeElementType.ComboBox:
                    element = new ComboBoxElement(node, arch);
                    break;
                case NodeElementType.Asset:
                    element = new AssetSelect(node, arch);
                    break;
                case NodeElementType.Text:
                    element = new TextView(node, arch);
                    break;
                case NodeElementType.TextBox:
                    element = new TextBoxView(node, arch);
                    break;
                case NodeElementType.SkeletonNodeSelect:
                    element = new SkeletonNodeSelectElement(node, arch);
                    break;
                }
                if (element != null)
                {
                    // Link element
                    node.AddElement(element);
                }
            }

            // Load metadata
            var meta = node.Meta.GetEntry(11);
            if (meta.Data != null)
            {
                var meta11 = ByteArrayToStructure<VisjectSurfaceMeta11>(meta.Data);
                node.Location = meta11.Position;
                //node.IsSelected = meta11.Selected;
            }

            // Link node
            node.OnLoaded();
            node.Parent = _surface;
        }

        /// <inheritdoc />
        public override void Update(float deltaTime)
        {
            // Update scale
            var currentScale = _surface.Scale.X;
            if (Mathf.Abs(_targeScale - currentScale) > 0.001f)
            {
                var scale = new Vector2(Mathf.Lerp(currentScale, _targeScale, deltaTime * 10.0f));
                _surface.Scale = scale;
            }

            // Navigate when mouse is near the edge and is doing sth
            bool isMovingWithMouse = false;
            if (IsMovignSelection || IsConnecting)
            {
                Vector2 moveVector = Vector2.Zero;
                float edgeDetectDistance = 22.0f;
                if (_mousePos.X < edgeDetectDistance)
                {
                    moveVector.X -= 1;
                }
                if (_mousePos.Y < edgeDetectDistance)
                {
                    moveVector.Y -= 1;
                }
                if (_mousePos.X > Width - edgeDetectDistance)
                {
                    moveVector.X += 1;
                }
                if (_mousePos.Y > Height - edgeDetectDistance)
                {
                    moveVector.Y += 1;
                }
                moveVector.Normalize();
                isMovingWithMouse = moveVector.LengthSquared > Mathf.Epsilon;
                if (isMovingWithMouse)
                {
                    _surface.Location -= moveVector * _moveViewWithMouseDragSpeed;
                }
            }
            _moveViewWithMouseDragSpeed = isMovingWithMouse ? Mathf.Clamp(_moveViewWithMouseDragSpeed + deltaTime * 20.0f, 1.0f, 8.0f) : 1.0f;

            base.Update(deltaTime);
        }

        /// <inheritdoc />
        public override void Draw()
        {
            // Cache data
            var style = FlaxEngine.GUI.Style.Current;
            var rect = new Rectangle(Vector2.Zero, Size);

            // Draw background
            var background = Owner.GetSurfaceBackground();
            if (background && background.ResidentMipLevels > 0)
            {
                var bSize = background.Size;
                float bw = bSize.X;
                float bh = bSize.Y;
                Vector2 pos = Vector2.Mod(bSize);

                if (pos.X > 0)
                    pos.X -= bw;
                if (pos.Y > 0)
                    pos.Y -= bh;

                int maxI = Mathf.CeilToInt(rect.Width / bw + 1.0f);
                int maxJ = Mathf.CeilToInt(rect.Height / bh + 1.0f);

                for (int i = 0; i < maxI; i++)
                {
                    for (int j = 0; j < maxJ; j++)
                    {
                        Render2D.DrawTexture(background, new Rectangle(pos.X + i * bw, pos.Y + j * bh, bw, bh), Color.White);
                    }
                }
            }

            // Selection
            if (IsSelecting)
            {
                var selectionRect = Rectangle.FromPoints(_leftMouseDownPos, _mousePos);
                Render2D.FillRectangle(selectionRect, Color.Orange * 0.4f);
                Render2D.DrawRectangle(selectionRect, Color.Orange);
            }

            // Push surface view transform (scale and offset)
            Render2D.PushTransform(ref _surface._cachedTransform);

            // Draw all connections at once to boost batching process
            for (int i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                for (int j = 0; j < node.Elements.Count; j++)
                {
                    if (node.Elements[j] is OutputBox ob && ob.HasAnyConnection)
                    {
                        ob.DrawConnections();
                    }
                }
            }

            // Draw connecting line
            if (IsConnecting)
            {
                // Get start position
                Vector2 startPos = _startBox.ConnectionOrigin;

                // Check if mouse is over any of box
                Vector2 endPos = _cmPrimaryMenu.Visible ? _surface.PointFromParent(_cmStartPos) : _surface.PointFromParent(_mousePos);
                Color lineColor = Style.Colors.Connecting;
                if (_lastBoxUnderMouse != null)
                {
                    // Check if can connect boxes
                    bool canConnect = CanConnectBoxes(_startBox, _lastBoxUnderMouse);
                    lineColor = canConnect ? Style.Colors.ConnectingValid : Style.Colors.ConnectingInvalid;
                    endPos = _lastBoxUnderMouse.ConnectionOrigin;
                }

                // Draw connection
                OutputBox.DrawConnection(ref startPos, ref endPos, ref lineColor);
            }

            Render2D.PopTransform();

            // Base
            base.Draw();

            //Render2D.DrawText(style.FontTitle, string.Format("Scale: {0}", _surface.Scale), rect, Enabled ? Color.Red : Color.Black);

            // Draw border
            if (ContainsFocus)
                Render2D.DrawRectangle(new Rectangle(0, 0, rect.Width - 2, rect.Height - 2), style.BackgroundSelected);

            // Draw disabled overlay
            //if (!Enabled)
            //    Render2D.FillRectangle(rect, new Color(0.2f, 0.2f, 0.2f, 0.5f), true);
        }

        /// <inheritdoc />
        protected override void SetSizeInternal(ref Vector2 size)
        {
            // Keep view stable
            var viewCenter = ViewCenterPosition;

            base.SetSizeInternal(ref size);

            ViewCenterPosition = viewCenter;
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            // Cleanup
            _cmPrimaryMenu.Dispose();
            _cmSecondaryMenu.Dispose();

            base.OnDestroy();
        }
    }
}