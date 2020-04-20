// Copyright (c) 2012-2020 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Options;
using FlaxEditor.Surface.Archetypes;
using FlaxEditor.Surface.ContextMenu;
using FlaxEditor.Surface.GUI;
using FlaxEditor.Surface.Undo;
using FlaxEngine;
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
        private static readonly List<VisjectSurfaceContext> NavUpdateCache = new List<VisjectSurfaceContext>(8);

        /// <summary>
        /// The surface control.
        /// </summary>
        protected SurfaceRootControl _rootControl;

        private float _targetScale = 1.0f;
        private float _moveViewWithMouseDragSpeed = 1.0f;
        private bool _isReleasing;
        private VisjectCM _activeVisjectCM;
        private GroupArchetype _customNodesGroup;
        private List<NodeArchetype> _customNodes;
        private Action _onSave;

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
        /// The connection start.
        /// </summary>
        protected IConnectionInstigator _connectionInstigator;

        /// <summary>
        /// The last connection instigator under mouse.
        /// </summary>
        protected IConnectionInstigator _lastInstigatorUnderMouse;

        /// <summary>
        /// The primary context menu.
        /// </summary>
        protected VisjectCM _cmPrimaryMenu;

        /// <summary>
        /// The context menu start position.
        /// </summary>
        protected Vector2 _cmStartPos = Vector2.Minimum;

        /// <summary>
        /// Occurs when selection gets changed.
        /// </summary>
        protected event Action SelectionChanged;

        /// <summary>
        /// The surface owner.
        /// </summary>
        public readonly IVisjectSurfaceOwner Owner;

        /// <summary>
        /// The style used by the surface.
        /// </summary>
        public readonly SurfaceStyle Style;

        /// <summary>
        /// The undo system to use for the history actions recording (optional, can be null).
        /// </summary>
        public readonly FlaxEditor.Undo Undo;

        /// <summary>
        /// Gets a value indicating whether surface is edited.
        /// </summary>
        public bool IsEdited => RootContext.IsModified;

        /// <summary>
        /// Gets the current context surface root control (nodes and all other surface elements container).
        /// </summary>
        public SurfaceRootControl SurfaceRoot => _rootControl;

        /// <summary>
        /// Gets or sets the view position (upper left corner of the view) in the surface space.
        /// </summary>
        public Vector2 ViewPosition
        {
            get => _rootControl.Location / -ViewScale;
            set => _rootControl.Location = value * -ViewScale;
        }

        /// <summary>
        /// Gets or sets the view center position (middle point of the view) in the surface space.
        /// </summary>
        public Vector2 ViewCenterPosition
        {
            get => (_rootControl.Location - Size * 0.5f) / -ViewScale;
            set => _rootControl.Location = Size * 0.5f + value * -ViewScale;
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
                _rootControl.Scale = new Vector2(_targetScale);
            }
        }

        /// <summary>
        /// Gets a value indicating whether user is selecting nodes.
        /// </summary>
        public bool IsSelecting => _leftMouseDown && !_isMovingSelection && _connectionInstigator == null;

        /// <summary>
        /// Gets a value indicating whether user is moving selected nodes.
        /// </summary>
        public bool IsMovingSelection => _leftMouseDown && _isMovingSelection && _connectionInstigator == null;

        /// <summary>
        /// Gets a value indicating whether user is connecting nodes.
        /// </summary>
        public bool IsConnecting => _connectionInstigator != null;

        /// <summary>
        /// Gets a value indicating whether the left mouse button is down.
        /// </summary>
        public bool IsLeftMouseButtonDown => _leftMouseDown;

        /// <summary>
        /// Gets a value indicating whether the right mouse button is down.
        /// </summary>
        public bool IsRightMouseButtonDown => _rightMouseDown;

        /// <summary>
        /// Returns true if any node is selected by the user (one or more).
        /// </summary>
        public bool HasNodesSelection
        {
            get
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[i].IsSelected)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the list of the selected nodes.
        /// </summary>
        public List<SurfaceNode> SelectedNodes
        {
            get
            {
                var selection = new List<SurfaceNode>();
                for (int i = 0; i < Nodes.Count; i++)
                {
                    if (Nodes[i].IsSelected)
                        selection.Add(Nodes[i]);
                }
                return selection;
            }
        }

        /// <summary>
        /// Gets the list of the selected controls (comments and nodes).
        /// </summary>
        public List<SurfaceControl> SelectedControls
        {
            get
            {
                var selection = new List<SurfaceControl>();
                for (int i = 0; i < _rootControl.Children.Count; i++)
                {
                    if (_rootControl.Children[i] is SurfaceControl control && control.IsSelected)
                        selection.Add(control);
                }
                return selection;
            }
        }

        /// <summary>
        /// Gets the list of the surface comments.
        /// </summary>
        /// <remarks>
        /// Don't call it too often. It does memory allocation and iterates over the surface controls to find comments in the graph.
        /// </remarks>
        public List<SurfaceComment> Comments => _context.Comments;

        /// <summary>
        /// The current surface context nodes collection. Read-only.
        /// </summary>
        public List<SurfaceNode> Nodes => _context.Nodes;

        /// <summary>
        /// The surface node descriptors collection.
        /// </summary>
        public readonly List<GroupArchetype> NodeArchetypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisjectSurface"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="onSave">The save action called when user wants to save the surface.</param>
        /// <param name="undo">The undo/redo to use for the history actions recording. Optional, can be null to disable undo support.</param>
        /// <param name="style">The custom surface style. Use null to create the default style.</param>
        /// <param name="groups">The custom surface node types. Pass null to use the default nodes set.</param>
        public VisjectSurface(IVisjectSurfaceOwner owner, Action onSave, FlaxEditor.Undo undo = null, SurfaceStyle style = null, List<GroupArchetype> groups = null)
        {
            AnchorPreset = AnchorPresets.StretchAll;
            Offsets = Margin.Zero;
            AutoFocus = false; // Disable to prevent autofocus and event handling on OnMouseDown event

            Owner = owner;
            Style = style ?? SurfaceStyle.CreateStyleHandler(Editor.Instance);
            if (Style == null)
                throw new InvalidOperationException("Missing visject surface style.");
            NodeArchetypes = groups ?? NodeFactory.DefaultGroups;
            Undo = undo;
            _onSave = onSave;

            // Initialize with the root context
            OpenContext(owner);
            RootContext.Modified += OnRootContextModified;

            // Setup input actions
            InputActions = new InputActionsContainer(new[]
            {
                new InputActionsContainer.Binding(options => options.Delete, Delete),
                new InputActionsContainer.Binding(options => options.SelectAll, SelectAll),
                new InputActionsContainer.Binding(options => options.Copy, Copy),
                new InputActionsContainer.Binding(options => options.Paste, Paste),
                new InputActionsContainer.Binding(options => options.Cut, Cut),
                new InputActionsContainer.Binding(options => options.Duplicate, Duplicate),
            });

            Context.ControlSpawned += OnSurfaceControlSpawned;
            Context.ControlDeleted += OnSurfaceControlDeleted;

            // Init drag handlers
            DragHandlers.Add(_dragAssets = new DragAssets<DragDropEventArgs>(ValidateDragItem));
            DragHandlers.Add(_dragParameters = new DragNames<DragDropEventArgs>(SurfaceParameter.DragPrefix, ValidateDragParameter));
        }

        /// <summary>
        /// Gets the display name of the connection type used in the surface.
        /// </summary>
        /// <param name="type">The graph connection type.</param>
        /// <returns>The display name (for UI).</returns>
        public virtual string GetConnectionTypeName(ConnectionType type)
        {
            return type.ToString();
        }

        private void OnRootContextModified(VisjectSurfaceContext context, bool graphEdited)
        {
            Owner.OnSurfaceEditedChanged();

            if (graphEdited)
            {
                Owner.OnSurfaceGraphEdited();
            }
        }

        /// <summary>
        /// Gets the custom nodes group archetype with custom nodes archetypes. May be null if no custom nodes in use.
        /// </summary>
        /// <returns>The custom nodes or null if no used.</returns>
        public GroupArchetype GetCustomNodes()
        {
            return _customNodesGroup;
        }

        /// <summary>
        /// Adds the custom nodes archetypes to the surface (user can spawn them and surface can deserialize).
        /// </summary>
        /// <remarks>Custom nodes has to have a node logic typename in DefaultValues[0] and group name in DefaultValues[1].</remarks>
        /// <param name="archetypes">The archetypes.</param>
        public void AddCustomNodes(IEnumerable<NodeArchetype> archetypes)
        {
            if (_customNodes == null)
            {
                // First time setup
                _customNodes = new List<NodeArchetype>(archetypes);
                _customNodesGroup = new GroupArchetype
                {
                    GroupID = Custom.GroupID,
                    Name = "Custom",
                    Color = Color.Wheat
                };
            }
            else
            {
                // Add more nodes
                _customNodes.AddRange(archetypes);
            }

            // Update collection
            _customNodesGroup.Archetypes = _customNodes.ToArray();
        }

        /// <summary>
        /// Updates the navigation bar of the toolstrip from window that uses this surface. Updates the navigation bar panel buttons to match the current view path.
        /// </summary>
        /// <param name="navigationBar">The navigation bar to update.</param>
        /// <param name="toolStrip">The toolstrip to use as layout reference.</param>
        /// <param name="hideIfRoot">True if skip showing nav button if the current context is the root location (user has no option to change context).</param>
        public void UpdateNavigationBar(NavigationBar navigationBar, ToolStrip toolStrip, bool hideIfRoot = true)
        {
            if (navigationBar == null || toolStrip == null)
                return;

            bool wasLayoutLocked = navigationBar.IsLayoutLocked;
            navigationBar.IsLayoutLocked = true;

            // Remove previous buttons
            navigationBar.DisposeChildren();

            // Spawn buttons
            var nodes = NavUpdateCache;
            nodes.Clear();
            var context = Context;
            if (hideIfRoot && context == RootContext)
                context = null;
            while (context != null)
            {
                nodes.Add(context);
                context = context.Parent;
            }
            float x = NavigationBar.DefaultButtonsMargin;
            float h = toolStrip.ItemsHeight - 2 * ToolStrip.DefaultMarginV;
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                var button = new VisjectContextNavigationButton(this, nodes[i].Context, x, ToolStrip.DefaultMarginV, h);
                button.PerformLayout();
                x += button.Width + NavigationBar.DefaultButtonsMargin;
                navigationBar.AddChild(button);
            }
            nodes.Clear();

            // Update
            navigationBar.IsLayoutLocked = wasLayoutLocked;
            navigationBar.PerformLayout();
        }

        /// <summary>
        /// Determines whether the specified node archetype can be spawned into the surface.
        /// </summary>
        /// <param name="nodeArchetype">The node archetype.</param>
        /// <returns>True if can spawn this node archetype, otherwise false.</returns>
        public virtual bool CanSpawnNodeType(NodeArchetype nodeArchetype)
        {
            return (nodeArchetype.Flags & NodeFlags.NoSpawnViaGUI) == 0;
        }

        /// <summary>
        /// Shows the whole graph by changing the view scale and the position.
        /// </summary>
        public void ShowWholeGraph()
        {
            if (Nodes.Count == 0)
                return;

            // Find surface bounds
            Rectangle area = Nodes[0].Bounds;
            for (int i = 1; i < Nodes.Count; i++)
                area = Rectangle.Union(area, Nodes[i].Bounds);

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
            _context.MarkAsModified(graphEdited);
        }

        /// <summary>
        /// Selects all the nodes.
        /// </summary>
        public void SelectAll()
        {
            for (int i = 0; i < _rootControl.Children.Count; i++)
            {
                if (_rootControl.Children[i] is SurfaceControl control)
                    control.IsSelected = true;
            }
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ClearSelection()
        {
            for (int i = 0; i < _rootControl.Children.Count; i++)
            {
                if (_rootControl.Children[i] is SurfaceControl control)
                    control.IsSelected = false;
            }
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Adds the specified control to the selection.
        /// </summary>
        /// <param name="control">The control.</param>
        public void AddToSelection(SurfaceControl control)
        {
            control.IsSelected = true;
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Selects the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void Select(SurfaceControl control)
        {
            ClearSelection();
            control.IsSelected = true;
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Selects the specified controls collection.
        /// </summary>
        /// <param name="controls">The controls.</param>
        public void Select(IEnumerable<SurfaceControl> controls)
        {
            ClearSelection();
            foreach (var control in controls)
            {
                control.IsSelected = true;
            }
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Deselects the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void Deselect(SurfaceControl control)
        {
            control.IsSelected = false;
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Creates the comment around the selected nodes.
        /// </summary>
        public SurfaceComment CommentSelection(string text = "")
        {
            var selection = SelectedNodes;
            if (selection.Count == 0)
                return null;
            Rectangle surfaceArea = GetNodesBounds(selection).MakeExpanded(80.0f);

            return _context.CreateComment(ref surfaceArea, string.IsNullOrEmpty(text) ? "Comment" : text, new Color(1.0f, 1.0f, 1.0f, 0.2f));
        }

        private static Rectangle GetNodesBounds(List<SurfaceNode> nodes)
        {
            if (nodes.Count == 0)
                return Rectangle.Empty;

            Rectangle surfaceArea = nodes[0].Bounds;
            for (int i = 1; i < nodes.Count; i++)
            {
                surfaceArea = Rectangle.Union(surfaceArea, nodes[i].Bounds);
            }

            return surfaceArea;
        }

        /// <summary>
        /// Deletes the specified collection of the controls.
        /// </summary>
        /// <param name="controls">The controls.</param>
        public void Delete(IEnumerable<SurfaceControl> controls)
        {
            foreach (var control in controls)
            {
                Delete(control);
            }
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Deletes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void Delete(SurfaceControl control)
        {
            if (control is SurfaceNode node)
            {
                if ((node.Archetype.Flags & NodeFlags.NoRemove) != 0)
                    return;

                Nodes.Remove(node);
            }

            Context.OnControlDeleted(control);
            MarkAsEdited();
            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Deletes the selected controls.
        /// </summary>
        public void Delete()
        {
            bool edited = false;

            List<SurfaceNode> nodes = null;
            for (int i = 0; i < _rootControl.Children.Count; i++)
            {
                if (_rootControl.Children[i] is SurfaceNode node)
                {
                    if (node.IsSelected && (node.Archetype.Flags & NodeFlags.NoRemove) == 0)
                    {
                        if (nodes == null)
                            nodes = new List<SurfaceNode>();
                        nodes.Add(node);
                        edited = true;
                    }
                }
                else if (_rootControl.Children[i] is SurfaceControl control && control.IsSelected)
                {
                    i--;
                    Context.OnControlDeleted(control);
                    edited = true;
                }
            }

            if (nodes != null)
            {
                if (Undo == null)
                {
                    // Remove all nodes
                    foreach (var node in nodes)
                    {
                        node.RemoveConnections();
                        Nodes.Remove(node);
                        Context.OnControlDeleted(node);
                    }
                }
                else
                {
                    var actions = new List<IUndoAction>();

                    // Break connections for all nodes
                    foreach (var node in nodes)
                    {
                        var action = new EditNodeConnections(Context, node);
                        node.RemoveConnections();
                        action.End();
                        actions.Add(action);
                    }

                    // Remove all nodes
                    foreach (var node in nodes)
                    {
                        var action = new AddRemoveNodeAction(node, false);
                        action.Do();
                        actions.Add(action);
                    }

                    Undo.AddAction(new MultiUndoAction(actions, nodes.Count == 1 ? "Remove node" : "Remove nodes"));
                }
            }

            if (edited)
            {
                MarkAsEdited();
            }

            SelectionChanged?.Invoke();
        }

        /// <summary>
        /// Finds the node of the given type.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>Found node or null if cannot.</returns>
        public SurfaceNode FindNode(ushort groupId, ushort typeId)
        {
            return _context.FindNode(groupId, typeId);
        }

        /// <summary>
        /// Finds the node with the given ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Found node or null if cannot.</returns>
        public SurfaceNode FindNode(int id)
        {
            return _context.FindNode(id);
        }

        /// <summary>
        /// Finds the node with the given ID.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Found node or null if cannot.</returns>
        public SurfaceNode FindNode(uint id)
        {
            return _context.FindNode(id);
        }

        /// <inheritdoc />
        public override void OnDestroy()
        {
            if (IsDisposing)
                return;
            _isReleasing = true;

            // Cleanup context cache
            _root = null;
            _context = null;
            _onSave = null;
            ContextStack.Clear();
            foreach (var context in _contextCache.Values)
            {
                context.Clear();
            }
            _contextCache.Clear();

            // Cleanup
            _activeVisjectCM = null;
            _cmPrimaryMenu?.Dispose();

            base.OnDestroy();
        }

        /// <summary>
        /// Gets the type of the parameter (enum to runtime value type).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The runtime value time.</returns>
        public static Type GetParameterValueType(ParameterType type)
        {
            switch (type)
            {
            case ParameterType.Bool: return typeof(bool);
            case ParameterType.SceneTexture:
            case ParameterType.Integer: return typeof(int);
            case ParameterType.ChannelMask: return typeof(ChannelMask);
            case ParameterType.Float: return typeof(float);
            case ParameterType.Vector2: return typeof(Vector2);
            case ParameterType.Vector3: return typeof(Vector3);
            case ParameterType.Vector4: return typeof(Vector4);
            case ParameterType.Color: return typeof(Color);
            case ParameterType.Texture:
            case ParameterType.NormalMap: return typeof(Texture);
            case ParameterType.String: return typeof(string);
            case ParameterType.Box: return typeof(BoundingBox);
            case ParameterType.Rotation: return typeof(Quaternion);
            case ParameterType.Transform: return typeof(Transform);
            case ParameterType.Asset: return typeof(Asset);
            case ParameterType.Actor: return typeof(Actor);
            case ParameterType.Rectangle: return typeof(Rectangle);
            case ParameterType.CubeTexture: return typeof(CubeTexture);
            case ParameterType.GPUTexture: return typeof(GPUTexture);
            case ParameterType.Matrix: return typeof(Matrix);
            case ParameterType.GPUTextureArray: return typeof(GPUTextureView);
            case ParameterType.GPUTextureVolume: return typeof(GPUTextureView);
            case ParameterType.GPUTextureCube: return typeof(GPUTextureView);
            default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        /// Gets the type of the connection (runtime value type to enum).
        /// </summary>
        /// <param name="type">The runtime value type.</param>
        /// <returns>The connection time.</returns>
        public static ConnectionType GetValueTypeConnectionType(Type type)
        {
            if (type == typeof(bool))
                return ConnectionType.Bool;
            if (type == typeof(int))
                return ConnectionType.Integer;
            if (type == typeof(float))
                return ConnectionType.Float;
            if (type == typeof(Vector2))
                return ConnectionType.Vector2;
            if (type == typeof(Vector3))
                return ConnectionType.Vector3;
            if (type == typeof(Vector4) || type == typeof(Color))
                return ConnectionType.Vector4;
            if (type == typeof(string))
                return ConnectionType.String;
            if (type == typeof(BoundingBox))
                return ConnectionType.Box;
            if (type == typeof(Quaternion))
                return ConnectionType.Rotation;
            if (type == typeof(Transform))
                return ConnectionType.Transform;
            if (type == typeof(object))
                return ConnectionType.Object;
            if (type == typeof(uint))
                return ConnectionType.UnsignedInteger;
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
