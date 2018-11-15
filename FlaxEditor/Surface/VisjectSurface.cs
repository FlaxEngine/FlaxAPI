// Copyright (c) 2012-2018 Wojciech Figat. All rights reserved.

using System;
using System.Collections.Generic;
using FlaxEditor.GUI;
using FlaxEditor.GUI.Drag;
using FlaxEditor.Surface.ContextMenu;
using FlaxEditor.Surface.Elements;
using FlaxEditor.Surface.GUI;
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
        private bool _wasMouseDownSinceCommentCreatingStart;
        private bool _isReleasing;
        private VisjectCM _activeVisjectCM;

        /// <summary>
        /// The left mouse down flag.
        /// </summary>
        protected bool _leftMouseDown;

        /// <summary>
        /// The right mouse down flag.
        /// </summary>
        protected bool _rightMouseDown;

        /// <summary>
        /// The flag for keyboard key down for comment creating.
        /// </summary>
        protected bool _isCommentCreateKeyDown;

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
        /// The style used by the surface.
        /// </summary>
        public readonly SurfaceStyle Style;

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
        public bool IsSelecting => _leftMouseDown && !_isMovingSelection && _connectionInstigator == null && !_isCommentCreateKeyDown;

        /// <summary>
        /// Gets a value indicating whether user is moving selected nodes.
        /// </summary>
        public bool IsMovingSelection => _leftMouseDown && _isMovingSelection && _connectionInstigator == null && !_isCommentCreateKeyDown;

        /// <summary>
        /// Gets a value indicating whether user is connecting nodes.
        /// </summary>
        public bool IsConnecting => _connectionInstigator != null;

        /// <summary>
        /// Gets a value indicating whether user is creating comment.
        /// </summary>
        public bool IsCreatingComment => _isCommentCreateKeyDown && _leftMouseDown && !_isMovingSelection && _connectionInstigator == null;

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
        /// <param name="style">The custom surface style. Use null to create the default style.</param>
        /// <param name="groups">The custom surface node types. Pass null to use the default nodes set.</param>
        /// <param name="primaryContextMenu">The custom surface context menu. Pass null to use the default one.</param>
        public VisjectSurface(IVisjectSurfaceOwner owner, Action onSave, SurfaceStyle style = null, List<GroupArchetype> groups = null, VisjectCM primaryContextMenu = null)
        {
            DockStyle = DockStyle.Fill;

            Owner = owner;
            Style = style ?? SurfaceStyle.CreateStyleHandler(Editor.Instance);
            if (Style == null)
                throw new InvalidOperationException("Missing visject surface style.");
            NodeArchetypes = groups ?? NodeFactory.DefaultGroups;

            // Initialize with the root context
            OpenContext(owner);
            RootContext.Modified += OnRootContextModified;

            // Create primary menu (for nodes spawning)
            _cmPrimaryMenu = primaryContextMenu ?? new VisjectCM(NodeArchetypes, CanSpawnNodeType, () => Parameters);
            SetPrimaryMenu(_cmPrimaryMenu);

            // Create secondary menu (for other actions)
            _cmSecondaryMenu = new FlaxEngine.GUI.ContextMenu();
            _cmSecondaryMenu.AddButton("Save", onSave);
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

            // Init drag handlers
            DragHandlers.Add(_dragAssets = new DragAssets<DragDropEventArgs>(ValidateDragItem));
            DragHandlers.Add(_dragParameters = new DragSurfaceParameters<DragDropEventArgs>(ValidateDragParameter));
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
            return true;
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
            _hasInputSelectionChanged = true;

            for (int i = 0; i < _rootControl.Children.Count; i++)
            {
                if (_rootControl.Children[i] is SurfaceControl control)
                    control.IsSelected = true;
            }
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public void ClearSelection()
        {
            _hasInputSelectionChanged = true;

            for (int i = 0; i < _rootControl.Children.Count; i++)
            {
                if (_rootControl.Children[i] is SurfaceControl control)
                    control.IsSelected = false;
            }
        }

        /// <summary>
        /// Adds the specified control to the selection.
        /// </summary>
        /// <param name="control">The control.</param>
        public void AddToSelection(SurfaceControl control)
        {
            _hasInputSelectionChanged = true;

            control.IsSelected = true;
        }

        /// <summary>
        /// Selects the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void Select(SurfaceControl control)
        {
            _hasInputSelectionChanged = true;

            ClearSelection();

            control.IsSelected = true;
        }

        /// <summary>
        /// Selects the specified controls collection.
        /// </summary>
        /// <param name="controls">The controls.</param>
        public void Select(IEnumerable<SurfaceControl> controls)
        {
            _hasInputSelectionChanged = true;

            ClearSelection();

            foreach (var control in controls)
            {
                control.IsSelected = true;
            }
        }

        /// <summary>
        /// Deselects the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void Deselect(SurfaceControl control)
        {
            _hasInputSelectionChanged = true;

            control.IsSelected = false;
        }

        /// <summary>
        /// Creates the comment around the selected nodes.
        /// </summary>
        public void CommentSelection()
        {
            var selection = SelectedNodes;
            if (selection.Count == 0)
                return;

            Rectangle surfaceArea = selection[0].Bounds.MakeExpanded(80.0f);
            for (int i = 1; i < selection.Count; i++)
            {
                surfaceArea = Rectangle.Union(surfaceArea, selection[i].Bounds.MakeExpanded(80.0f));
            }

            _context.CreateComment(ref surfaceArea);
        }

        /// <summary>
        /// Deletes the specified collection of the controls.
        /// </summary>
        /// <param name="controls">The controls.</param>
        public void Delete(IEnumerable<SurfaceControl> controls)
        {
            _hasInputSelectionChanged = true;

            foreach (var control in controls)
            {
                Delete(control);
            }
        }

        /// <summary>
        /// Deletes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        public void Delete(SurfaceControl control)
        {
            _hasInputSelectionChanged = true;

            if (control is SurfaceNode node)
            {
                if ((node.Archetype.Flags & NodeFlags.NoRemove) != 0)
                    return;

                node.RemoveConnections();
                Nodes.Remove(node);
            }

            control.Dispose();
            MarkAsEdited();
        }

        /// <summary>
        /// Deletes the selected controls.
        /// </summary>
        public void Delete()
        {
            _hasInputSelectionChanged = true;

            bool edited = false;

            for (int i = 0; i < _rootControl.Children.Count; i++)
            {
                if (_rootControl.Children[i] is SurfaceNode node)
                {
                    if (node.IsSelected && (node.Archetype.Flags & NodeFlags.NoRemove) == 0)
                    {
                        node.RemoveConnections();
                        node.Dispose();

                        Nodes.Remove(node);
                        i--;

                        edited = true;
                    }
                }
                else if (_rootControl.Children[i] is SurfaceControl control && control.IsSelected)
                {
                    i--;
                    control.Dispose();
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
            _isReleasing = true;

            // Cleanup context cache
            _root = null;
            _context = null;
            ContextStack.Clear();
            foreach (var context in _contextCache.Values)
            {
                context.Clear();
            }
            _contextCache.Clear();

            // Cleanup
            _activeVisjectCM = null;
            _cmPrimaryMenu.Dispose();
            _cmSecondaryMenu.Dispose();

            base.OnDestroy();
        }
    }
}
