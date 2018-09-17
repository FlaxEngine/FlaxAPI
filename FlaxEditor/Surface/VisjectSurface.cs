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
        /// <summary>
        /// The surface root control used to navigate around the view (scale and move it).
        /// </summary>
        /// <seealso cref="FlaxEngine.GUI.ContainerControl" />
        protected class SurfaceControl : ContainerControl
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

        /// <summary>
        /// The surface control.
        /// </summary>
        protected SurfaceControl _surface;

        private bool _edited;
        private float _targetScale = 1.0f;
        private float _moveViewWithMouseDragSpeed = 1.0f;

        /// <summary>
        /// The nodes collection.
        /// </summary>
        protected readonly List<SurfaceNode> _nodes = new List<SurfaceNode>(64);

        /// <summary>
        /// The left mouse down flag.
        /// </summary>
        protected bool _leftMouseDown;

        /// <summary>
        /// The right mouse down flag.
        /// </summary>
        protected bool _rightMouseDown;

        /// <summary>
        /// The left mouse down position.
        /// </summary>
        protected Vector2 _leftMouseDownPos = Vector2.Minimum;

        /// <summary>
        /// The right mouse down position.
        /// </summary>
        protected Vector2 _rightMouseDownPos = Vector2.Minimum;

        /// <summary>
        /// The mouse position.
        /// </summary>
        protected Vector2 _mousePos = Vector2.Minimum;

        /// <summary>
        /// The mouse movement amount.
        /// </summary>
        protected float _mouseMoveAmount;

        /// <summary>
        /// The is moving selection flag.
        /// </summary>
        protected bool _isMovingSelection;

        /// <summary>
        /// The moving selection view position.
        /// </summary>
        protected Vector2 _movingSelectionViewPos;

        /// <summary>
        /// The start box (for connecting).
        /// </summary>
        protected Box _startBox;

        /// <summary>
        /// The last box under mouse.
        /// </summary>
        protected Box _lastBoxUnderMouse;

        /// <summary>
        /// The primary context menu.
        /// </summary>
        protected VisjectCM _cmPrimaryMenu;

        /// <summary>
        /// The secondary context menu.
        /// </summary>
        protected FlaxEngine.GUI.ContextMenu _cmSecondaryMenu;

        /// <summary>
        /// The context menu start position.
        /// </summary>
        protected Vector2 _cmStartPos = Vector2.Minimum;

        /// <summary>
        /// The surface owner.
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
            get => _targetScale;
            set
            {
                // Clamp
                value = Mathf.Clamp(value, 0.05f, 1.6f);

                // Check if value will change
                if (Mathf.Abs(value - _targetScale) > 0.0001f)
                {
                    // Set new target scale
                    _targetScale = value;
                }

                // disable view scale animation
                _surface.Scale = new Vector2(_targetScale);
            }
        }

        /// <summary>
        /// Gets a value indicating whether user is selecting nodes.
        /// </summary>
        public bool IsSelecting => _leftMouseDown && !_isMovingSelection && _startBox == null;

        /// <summary>
        /// Gets a value indicating whether user is moving selected nodes.
        /// </summary>
        public bool IsMovingSelection => _leftMouseDown && _isMovingSelection && _startBox == null;

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
            _hasInputSelectionChanged = true;

            for (int i = 0; i < _nodes.Count; i++)
                _nodes[i].IsSelected = true;
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ClearSelection()
        {
            _hasInputSelectionChanged = true;

            for (int i = 0; i < _nodes.Count; i++)
                _nodes[i].IsSelected = false;
        }

        /// <summary>
        /// Adds the specified node to the selection.
        /// </summary>
        /// <param name="node">The node.</param>
        public void AddToSelection(SurfaceNode node)
        {
            _hasInputSelectionChanged = true;

            node.IsSelected = true;
        }

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Select(SurfaceNode node)
        {
            _hasInputSelectionChanged = true;

            ClearSelection();

            node.IsSelected = true;
        }

        /// <summary>
        /// Selects the specified nodes collection.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public void Select(IEnumerable<SurfaceNode> nodes)
        {
            _hasInputSelectionChanged = true;

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
            _hasInputSelectionChanged = true;

            node.IsSelected = false;
        }

        /// <summary>
        /// Deletes the specified collection of the nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        public void Delete(IEnumerable<SurfaceNode> nodes)
        {
            _hasInputSelectionChanged = true;

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
            _hasInputSelectionChanged = true;

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
            _hasInputSelectionChanged = true;

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

            // Initialize
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
            if (IsDisposing)
                return;

            // Cleanup
            _cmPrimaryMenu.Dispose();
            _cmSecondaryMenu.Dispose();

            base.OnDestroy();
        }
    }
}
